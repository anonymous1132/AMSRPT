using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt207DataBuilder
    {
        public List<ReqRpt207RowEntity> RowEntities { get; set; } = new List<ReqRpt207RowEntity>();
        public string EqpStr { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public void GetTableData(string eqp, string startDate, string endDate)
        {
            EqpStr = eqp;
            StartDate = startDate;
            EndDate = endDate;
            var eqps = eqp.Split(',');
            var eqps_single = eqps.Where(w => !w.Contains("*"));
            var eqps_like = eqps.Except(eqps_single).Select(s => s.Replace('*', '%'));
            string condition = string.Format("where start_time between '{0}' and '{1} 23:59:59' ", startDate, endDate);
            condition = string.Format("{0} and (eqp_id in ('{1}') or eqp_id like '{2}' ) order by start_time desc", condition, string.Join("','", eqps_single), string.Join("' or eqp_id like '", eqps_like));
            DB2DataCatcher<Eda_Eqp_HostoryModel> InlineCatcher = new DB2DataCatcher<Eda_Eqp_HostoryModel>("ISTRPT.EDA_Eqp_History") { Conditions = condition };
            var list = InlineCatcher.GetEntities().EntityList;
            if (!list.Any()) throw new Exception("没有数据");
            foreach (var l in list)
            {
                TimeSpan ts = l.End_Time.HasValue?(l.End_Time.Value - l.Start_Time):new TimeSpan();
                var entity = new ReqRpt207RowEntity()
                {
                    LotID = l.Lot_ID,
                    EqpID = l.Eqp_ID,
                    EqpType=l.Eqp_Type,
                    CastID = l.Cast_ID,
                    MainPDID = l.MainPD_ID,
                    Dept = l.Dept,
                    StartTime = l.Start_Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = l.End_Time.HasValue? l.End_Time.Value.ToString("yyyy-MM-dd HH:mm:ss"):"",
                    OpeNo = l.Ope_No,
                    PDID = l.PD_ID,
                    PDName = l.PD_Name,
                    Qty = l.Cur_Wafer_Qty,
                    RecipeID = l.Recipe_ID,
                    UserFullName = l.User_Full_Name,
                    UserID = l.Owner_ID,
                    RunDur = string.Format("{0}时{1}分{0}秒",Math.Floor(ts.TotalHours),ts.Minutes,ts.Seconds)
                };
                RowEntities.Add(entity);
            }
        }
    }
}