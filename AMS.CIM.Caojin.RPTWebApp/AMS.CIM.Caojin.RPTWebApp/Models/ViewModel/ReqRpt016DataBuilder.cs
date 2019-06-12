using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt016DataBuilder
    {
        public List<FRCodeModel> ModuleList { get; set; } = new List<FRCodeModel>();

        public List<string> HoldReasonCodeList { get; set; } = new List<string>();

        public List<ReqRpt016HoldLotDetailEntity> LotHoldList { get; set; } = new List<ReqRpt016HoldLotDetailEntity> { };

        public List<ReqRpt016LotHoldSummaryEntity> ProdSummaryEntities { get; set; } = new List<ReqRpt016LotHoldSummaryEntity>();

        public List<ReqRpt016LotHoldSummaryEntity> NonProdSummaryEntities { get; set; } = new List<ReqRpt016LotHoldSummaryEntity>();

        public List<DateTime> Items { get; set; } = new List<DateTime>();

        DB2DataCatcher<FRCodeModel> CodeCatcher { get; set; } = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE");

        public void GetModulesAndCodes()
        {
            CodeCatcher.Conditions = "where category_id='Department'";
            ModuleList = CodeCatcher.GetEntities().EntityList.ToList();
            CodeCatcher.Conditions = "where category_id='LotHold'";
            HoldReasonCodeList = CodeCatcher.GetEntities().EntityList.Where(w => w.Code_ID.Length == 5).Select(s => s.Code_ID).ToList();
            //HoldReasonCodeList.Add("system");
        }

        DB2DataCatcher<Report16_LotHold_Summary> LotHoldCatcher { get; set; } = new DB2DataCatcher<Report16_LotHold_Summary>("ISTRPT.Report16_LotHold_Summary");

        /// <summary>
        /// 获取LotHold所有信息
        /// </summary>
        /// <param name="startTime">eg:2019-05-31 00:00:00</param>
        /// <param name="endTime">eg:2019-06-01 23:59:59</param>
        public void GetLotHoldDetail(string startTime, string endTime)
        {
            var date1 = DateTime.Parse(startTime).Date;
            var date2 = DateTime.Parse(endTime).Date;
            if (date1 > date2 || date2 > DateTime.Now) throw new Exception("时间范围错误");
            
         
            LotHoldCatcher.Conditions = string.Format("where hold_time between '{0}' and '{1}'", startTime, endTime);
            var list = LotHoldCatcher.GetEntities().EntityList;
            foreach (var l in list)
            {
                var entity = new ReqRpt016HoldLotDetailEntity
                {
                    Duration = l.Duration,
                    EqpType = l.Eqp_Type,
                    LotID = l.Lot_ID,
                    LotType = l.Lot_Type,
                    Qty = l.Cur_Wafer_Qty,
                    Prod = l.ProdSpec_ID,
                    MainPDID = l.MainPD_ID,
                    PDID = l.PD_ID,
                    PDName = l.PD_Name,
                    OpeNO = l.Ope_NO,
                    HoldType = l.Hold_Type,
                    HoldPDDept = l.Hold_PD_Dept,
                    HoldUserID = l.Hold_User_ID,
                    HoldUserName = l.Hold_User_Name,
                    HoldUserDept = l.Hold_User_Dept,
                    ReleaseUserID = l.Release_User_ID,
                    ReleaseComment = l.Release_Comment,
                    ReleaseUserName = l.Release_User_Name,
                    ReleaseUserDept=l.Release_User_Dept,
                    HoldComment = l.Hold_Comment,
                    ReasonCode = l.Reason_Code,
                    HoldTime = l.Hold_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    ReleaseTime = l.Release_Time.HasValue ? l.Release_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""
                };
                LotHoldList.Add(entity);
            }
            //Summary部分需求取消
            //while (date1 <= date2)
            //{
            //    Items.Add(date1);
            //    date1 = date1.AddDays(1);
            //}
            //var prod = LotHoldList.Where(w => w.LotType == "Production");
            //var nonProd = LotHoldList.Where(w => w.LotType != "Production");
            // ProdSummaryEntities = GetSummaryEntities(prod);
            //  NonProdSummaryEntities = GetSummaryEntities(nonProd);
        }

        private List<ReqRpt016LotHoldSummaryEntity> GetSummaryEntities(IEnumerable<ReqRpt016HoldLotDetailEntity> LotHoldList)
        {
            List<ReqRpt016LotHoldSummaryEntity> Entities = new List<ReqRpt016LotHoldSummaryEntity>();
            var gp = LotHoldList.GroupBy(a => new { a.FinalDept, a.ReasonCode }).Where(w => !string.IsNullOrEmpty(w.Key.FinalDept));
            foreach (var g in gp)
            {
                var entity = new ReqRpt016LotHoldSummaryEntity
                {
                    Department = g.Key.FinalDept,
                    ReasonCode = g.Key.ReasonCode,
                    Qty = g.Sum(s => s.Qty)
                };
                foreach (var item in Items)
                {
                    var value = g.Select(s => new { HoldTime = DateTime.Parse(s.HoldTime), s.ReleaseTime }).Select(s => { if (s.HoldTime.Date > item) return 0; if (s.ReleaseTime == "") return (Min(item.AddDays(1), DateTime.Now) - Max(s.HoldTime, item)).TotalHours; DateTime dt = DateTime.Parse(s.ReleaseTime); if (dt.Date < item) return 0; return (Min(item.AddDays(1), dt) - Max(s.HoldTime, item)).TotalHours; }).Sum();
                    entity.Values.Add(value);
                }
                Entities.Add(entity);
            }
            return Entities;
        }



        private DateTime Min(DateTime dt1,DateTime dt2)
        {
            return dt1 < dt2 ? dt1 : dt2;
        }

        private DateTime Max(DateTime dt1, DateTime dt2)
        {
            return dt1 > dt2 ? dt1 : dt2;
        }
    }
}