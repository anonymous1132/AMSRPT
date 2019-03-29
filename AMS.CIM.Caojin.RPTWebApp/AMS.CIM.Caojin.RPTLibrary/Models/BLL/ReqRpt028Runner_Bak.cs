using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// WIP Chart Runner
    /// </summary>
    public class ReqRpt028Runner_Bak
    {
        public ReqRpt028Runner_Bak(Db2ConnObj connObj)
        {
            HSCatcher = new DB2OperDataCatcher<FHOPEHS_WipChart>("MMVIEW.FHOPEHS", new DB2Oper(connObj)) { Conditions=Condition};
            WipChartPusher = new DB2OperDataPusher<RPT_WipChart_YSTD>("ISTRPT.RPT_WIPCHART_YSTD",new DB2Oper(connObj));
            Initialize();
        }

        DB2OperDataCatcher<FHOPEHS_WipChart> HSCatcher { get; set; }

        string DateString = DateTime.Now.Date.ToString("yyyy-MM-dd");

        readonly string Condition =string.Format("where claim_time >='{0} 08:00:00' and lot_type='Production' and ope_category not in ('OrderChange','ScheduleChange') order by claim_time ", DateTime.Now.Date.ToString("yyyy-MM-dd"));

        DB2OperDataPusher<RPT_WipChart_YSTD> WipChartPusher { get; set; }

        readonly string DelOldRecords = "delete from ISTRPT.RPT_WIPCHART_YSTD";

        private void Initialize()
        {
            HSCatcher.DB2.GetSomeData(DelOldRecords);
            HSCatcher.GetEntities();
            var HistoryList = HSCatcher.entities.EntityList;
            if (HistoryList.Count() < 1) throw new Exception("没有YSTD Record");
            var lotList = HistoryList.Select(s => s.Lot_ID).Distinct();
            foreach (var lot in lotList)
            {
               var hist = HistoryList.Where(w => w.Lot_ID == lot).First();
               var entity=  new RPT_WipChart_YSTD() {
                    Lot_ID = hist.Lot_ID,
                    Claim_Time = hist.Claim_Time,
                    Cur_Wafer_Qty=hist.Cur_Wafer_Qty,
                    Hold_State=hist.Hold_State,
                    MainPD_ID=hist.MainPD_ID,
                    Ope_No=hist.Ope_No,
                    Priority_Class=hist.Priority_Class,
                    ProdSpec_ID=hist.ProdSpec_ID,
                    Ope_Category=hist.Ope_Category,
                    Bank_ID=hist.Bank_ID
                };
                GetInTime(entity,entity.Lot_ID);
                if (entity.In_Time > DateTime.MinValue)
                {
                    if (entity.Ope_Category == "OperationComplete") { GetPreOpeNoIfComplete(entity); }
                    WipChartPusher.entities.EntityList.Add(entity);
                }

            }

            WipChartPusher.PushEntities();
        }

        //获取in_time
        private void GetInTime(RPT_WipChart_YSTD lot,string lot_id)
        {
            HSCatcher.Conditions =string.Format("where lot_id ='{0}'and claim_time <='{1} 08:00:00' and Ope_Category in ('OperationComplete','STB','Split') order by Claim_Time", lot_id,DateString);
            HSCatcher.GetEntities();
            var list=HSCatcher.entities.EntityList;
            var list_findInOpe = list.Where(w => w.Lot_ID == lot_id && w.Ope_Category != "Split");
 
            if (list_findInOpe.Any())
            {
                lot.In_Time = list.Last().Claim_Time;
            }
            else
            {
                //Split情况下获取Intime
                HSCatcher.Conditions =string.Format("where lot_id like '{0}.%' and claim_time <='{1} 08:00:00' and Ope_Category in ('OperationComplete','STB','Split')",lot_id.Split('.')[0],DateString);
                HSCatcher.GetEntities();

                var splitList = HSCatcher.entities.EntityList.Where(w => w.Lot_ID == lot_id && w.Ope_Category == "Split");
                if (!splitList.Any()) return;
                DateTime splitTime = splitList.First().Claim_Time;
                var momLot = HSCatcher.entities.EntityList.Where(w=>w.Claim_Time==splitTime&&w.Lot_ID!=lot_id).First();
                GetInTime(lot,momLot.Lot_ID);
            }
        }
        //如果ope_category是OperationComplete需要抓上一站
        private void GetPreOpeNoIfComplete(RPT_WipChart_YSTD entity)
        {
            string sql = string.Format("select prev_mainpd_id, prev_ope_no from mmview.fhopehs where lot_id='{0}' and Ope_Category='OperationComplete' and claim_time='{1}'", entity.Lot_ID,entity.Claim_Time.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            HSCatcher.DB2.GetSomeData(sql);
            var dt = HSCatcher.DB2.dt;
            if (dt.Rows.Count > 0)
            {
                entity.MainPD_ID =dt.DefaultView[0][0].ToString() ;
                entity.Ope_No = dt.DefaultView[0][1].ToString();
            }
        }
    }
}
