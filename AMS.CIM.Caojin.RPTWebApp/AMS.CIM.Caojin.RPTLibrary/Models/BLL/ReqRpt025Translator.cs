using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class ReqRpt025Translator
    {
        public ReqRpt025Translator(DateTime start,DateTime end)
        {
            Translate(start,end);
        }

        public ReqRpt025Translator()
        {
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData("select max(Start_Time) from istrpt.RPT_RealTime_Lin");
            LastRecordTime = (DateTime)db2.dt.Rows[0][0];
            Translate(LastRecordTime,DateTime.Now);
        }

        //必要条件：日班、夜班各12小时
        readonly TimeSpan StartOfDayWorkTime = TimeSpan.FromHours(8);
        readonly TimeSpan EndOfDayWorkTime = TimeSpan.FromHours(20);

        private DateTime Start_Time { get; set; }

        private DateTime End_Time { get; set; }

        public DateTime LastRecordTime { get; set; } = DateTime.MinValue;

        DB2DataCatcher<Report25_MoveQty_ByProd> DBCatcher = new DB2DataCatcher<Report25_MoveQty_ByProd>("ISTRPT.Report25_MoveQty_ByProd");

        DB2DataCatcher<Report25_ScrappedQty_ByProd> DBCatcher2 = new DB2DataCatcher<Report25_ScrappedQty_ByProd>("ISTRPT.Report25_ScrappedQty_ByProd");

        DB2DataCatcher<FRLot_TurnRateModel> LotCatcher = new DB2DataCatcher<FRLot_TurnRateModel>("MMVIEW.FRLOT");

        DB2DataCatcher<FHOPEHS_Turn_Daily> OpehsCatcher = new DB2DataCatcher<FHOPEHS_Turn_Daily>("MMVIEW.FHOPEHS");

        DB2DataPusher<RPT_RealTime_Lin> DBPusher = new DB2DataPusher<RPT_RealTime_Lin>("ISTRPT.RPT_RealTime_Lin");

        DB2DataPusher<RPT_Turn_Daily> TurnPusher = new DB2DataPusher<RPT_Turn_Daily>("ISTRPT.RPT_Turn_Daily");

        List<RPT_RealTime_Lin> RealTimeList = new List<RPT_RealTime_Lin>();

        List<RPT_Turn_Daily> TurnDailyList = new List<RPT_Turn_Daily>();

        List<string> UpdateSqlList=new List<string>();
        //为RealTimeList赋值
        private void DoWork()
        {
            DBCatcher.Conditions = string.Format("where lot_type='Production' and  Claim_time >='{0}' and Claim_Time < '{1}'", Start_Time.ToString("yyyy-MM-dd HH:mm:ss"), End_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            DBCatcher2.Conditions= string.Format("where lot_type='Production' and  Scrap_time >='{0}' and Scrap_Time < '{1}'", Start_Time.ToString("yyyy-MM-dd HH:mm:ss"), End_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            var list = DBCatcher.GetEntities().EntityList;
            var list2 = DBCatcher2.GetEntities().EntityList;
            if (list.Count() <= 0) throw new Exception("ReqRpt025Translator.GetBaseList()没有获取符合条件的数据");
            LotCatcher.Conditions = string.Format("where lot_type='Production' and Create_Time <='{0}' and (Completion_Time >='{1}' or Completion='1901-01-01 00:00:00' )", End_Time.ToString("yyyy-MM-dd HH:mm:ss"), Start_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            var lotList = LotCatcher.GetEntities().EntityList;
            OpehsCatcher.Conditions = string.Format("where lot_type='Production' and Ope_Category='OperationComplete' and PD_Type not in ('Dummy','Measurement') and Claim_Time between '{0}' and '{1}'", Start_Time.ToString("yyyy-MM-dd HH:mm:ss"), End_Time.ToString("yyyy-MM-dd HH:mm:ss"));
            var OphesList = OpehsCatcher.GetEntities().EntityList;
            DateTime firstTime = Start_Time;
            while (firstTime < End_Time)
            {
                var rawList=  list.Where(w => w.Claim_Time >= firstTime && w.Claim_Time < firstTime.AddHours(12));
                var rawList2 = list2.Where(w=>w.Scrap_Time>=firstTime&& w.Scrap_Time<firstTime.AddHours(12));
                var rawLotList = lotList.Where(w=>w.Created_Time<=firstTime&&(w.Completion_Time>=firstTime || w.Completion_Time==DateTime.Parse("1901-01-01 00:00:00")));
                var rawOpehsList = OphesList.Where(w => w.Claim_Time >= firstTime && w.Claim_Time < firstTime.AddHours(12));
                if (rawList.Count() > 0)
                {
                    rawList.GroupBy(g => new { g.ProdSpec_ID,g.PartName}).Select(s => new { Product = s.Key, Qty = s.Sum(i => i.Cur_Wafer_Qty) }).ToList().ForEach(f=>RealTimeList.Add(new RPT_RealTime_Lin() {Product_ID=f.Product.ProdSpec_ID,MoveQty=f.Qty,PartName=f.Product.PartName,Start_Time=firstTime,ScrappedQty=0 }));
                }
                if (rawList2.Count() > 0)
                {
                    rawList2.GroupBy(g => new { g.ProdSpec_ID, g.PartName }).Select(s => new { Product = s.Key, Qty = s.Sum(i => i.Qty) }).ToList().ForEach(f => RealTimeList.Where(w=>w.Product_ID == f.Product.ProdSpec_ID&&w.PartName == f.Product.PartName && w.Start_Time == firstTime).FirstOrDefault().ScrappedQty=f.Qty);
                }
                if (rawLotList.Count() > 0)
                {
                    rawLotList.GroupBy(g => g.ProdSpec_ID).ToList().ForEach(
                        f=>TurnDailyList.Add(
                            new RPT_Turn_Daily()
                            {
                                Product_ID =f.Key,
                                Start_Time =firstTime,
                                WIP =f.Count(),
                                MoveQty=RealTimeList.Where(w=>w.Product_ID==f.Key).DefaultIfEmpty().Sum(s=>s.MoveQty),
                                EffectiveSteps=rawOpehsList.Where(w=>w.ProdSpec_ID==f.Key).Count()
                            }
                            )
                            );
                }
                
                firstTime = firstTime.AddHours(12);
            }

              //input or update
            var RealTimeCurrent = new DB2DataCatcher<RPT_RealTime_Lin>("ISTRPT.RPT_RealTime_Lin") { Conditions = string.Format("where Start_Time >='{0}' and Start_Time<='{1}'", Start_Time.ToString("yyyy-MM-dd HH:mm:ss"), End_Time.ToString("yyyy-MM-dd HH:mm:ss")) }.GetEntities().EntityList;
            var TurnDailyCurrent = new DB2DataCatcher<RPT_Turn_Daily>("ISTRPT.RPT_Turn_Daily") { Conditions=string.Format("where Start_Time >='{0}' and Start_Time<='{1}'", Start_Time.ToString("yyyy-MM-dd HH:mm:ss"), End_Time.ToString("yyyy-MM-dd HH:mm:ss")) }.GetEntities().EntityList;

            foreach (var item in RealTimeList)
            {
                if (RealTimeCurrent.Where(w => w.Start_Time == item.Start_Time && w.Product_ID == item.Product_ID && w.PartName == item.PartName).Count() == 0)
                {
                    DBPusher.entities.EntityList.Add(item);
                }
                else
                {
                    UpdateSqlList.Add(string.Format("update ISTRPT.RPT_RealTime_Lin set MOVEQTY={0}, ScrappedQTY={4} where Start_Time='{1}' and Product_ID='{2}' and PartName='{3}'", item.MoveQty, item.Start_Time.ToString("yyyy-MM-dd HH:mm:ss"), item.Product_ID, item.PartName,item.ScrappedQty));
                }
            }

            foreach (var item in TurnDailyList)
            {
                if (TurnDailyCurrent.Where(w => w.Start_Time == item.Start_Time && w.Product_ID == item.Product_ID).Count() == 0)
                {
                    TurnPusher.entities.EntityList.Add(item);
                }
                else
                {
                    UpdateSqlList.Add(string.Format("update ISTRPT.RPT_Turn_Daily set MoveQty={0},EffectiveSteps={1},WIP={2} where Start_Time='{3}' and Product_ID='{4}'",item.MoveQty.ToString(),item.EffectiveSteps.ToString(),item.WIP.ToString(), item.Start_Time.ToString("yyyy-MM-dd HH:mm:ss"),item.Product_ID));
                }
            }

            if (DBPusher.entities.EntityList.Count() > 0) DBPusher.PushEntities();
            if (TurnPusher.entities.EntityList.Count() > 0) TurnPusher.PushEntities();
            if (UpdateSqlList.Count > 0)
            {
                DB2Helper db2 = new DB2Helper();
                db2.UpdateBatchCommand(UpdateSqlList);
            }
            
        }

        private void Translate(DateTime start, DateTime end)
        {
            if (start > start.Date.Add(EndOfDayWorkTime)) Start_Time = start.Date.Add(EndOfDayWorkTime);
            else if (start > start.Date.Add(StartOfDayWorkTime)) Start_Time = start.Date.Add(StartOfDayWorkTime);
            else Start_Time = start.Date.AddDays(-1).Add(EndOfDayWorkTime);
            if (end > end.Date.Add(EndOfDayWorkTime)) End_Time = end.Date.AddDays(1).Add(StartOfDayWorkTime);
            else if (end > end.Date.Add(StartOfDayWorkTime)) End_Time = end.Date.Add(EndOfDayWorkTime);
            else End_Time = end.Date.Add(StartOfDayWorkTime);
            DoWork();
        }












        //根据一段时间，获取该段时间内的prod=》move量
        //private Dictionary<string,int> GetDicByTime(DateTime startTime,DateTime endTime)
        //{
        //   DBCatcher.Conditions =string.Format("where lot_type='Production' and  Claim_time >='{0}' and Claim_Time < '{1}'",startTime.ToString("yyyy-MM-dd HH:mm:ss"),endTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //   var list= DBCatcher.GetEntities().EntityList;
        //    Dictionary<string, int> dic = new Dictionary<string, int>();
        //    if (list.Any())
        //    {
        //     list.GroupBy(g => g.ProdSpec_ID).Select(s=>new { s.Key,value= s.Sum(i=>i.Cur_Wafer_Qty)}).ToList().ForEach(f=>dic.Add(f.Key,f.value));
        //    }
        //    return dic;
        //}

   


    }
}
