using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt047MaintainEDCHandler
    {
        public ReqRpt047MaintainEDCHandler(ReqRpt047MaintainEdcPostModel postModel)
        {
            OpeType = postModel.OpeType;
            EqpID = postModel.EqpID;
            EdcPlan = postModel.EdcPlan;
            Period = postModel.Period;
            PeriodType = postModel.PeriodType;
            Initialize();
        }

        string OpeType { get; set; }

        public string EqpID { get; set; }

        public string EdcPlan { get; set; }

        public double Period { get; set; }

        public string PeriodType { get; set; }

        public string Msg { get; set; }

        public bool Success { get; set; }

        DB2Helper Db2 { get; set; } = new DB2Helper();

        private void Initialize()
        {
            if (OpeType == "insert")
            {
                InsertHandle();
                return;
            }
            if (OpeType == "delete")
            {
                DelHandle();
                return;
            }
            if (OpeType == "update")
            {
                UpdateHandle();
                return;
            }
            Success = false;
            Msg = "Unknown OpeType";
        }

        void InsertHandle()
        {
            string sql = string.Format("insert into istrpt.rpt_non_lot_edc_plan values('{0}','{1}',{2},'{3}')",EqpID,EdcPlan,Period,PeriodType);
            try
            {
                Db2.GetSomeData(sql);
                Success = true;
                Msg = "成功插入一条数据";
            }
            catch (Exception ex)
            {
                Success = false;
                Msg = ex.Message;
            }
        }

        void DelHandle()
        {
            string sql =string.Format( "delete from istrpt.rpt_non_lot_edc_plan where eqp_id='{0}' and edc_plan='{1}'",EqpID,EdcPlan);
            try
            {
                Db2.GetSomeData(sql);
                Success = true;
                Msg = "成功删除一条数据";
            }
            catch (Exception ex)
            {
                Success = false;
                Msg = ex.Message;
            }
        }

        void UpdateHandle()
        {
            string sql = string.Format("update istrpt.rpt_non_lot_edc_plan set period={0} ,period_type='{1}' where eqp_id='{2}' and edc_plan='{3}'",Period,PeriodType,EqpID,EdcPlan);
            try
            {
                Db2.GetSomeData(sql);
                Success = true;
                Msg = "修改操作成功";
            }
            catch (Exception ex)
            {
                Success = false;
                Msg = ex.Message;
            }
        }
    }
}