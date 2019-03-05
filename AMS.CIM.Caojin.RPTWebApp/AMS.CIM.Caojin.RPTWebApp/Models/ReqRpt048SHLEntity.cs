using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048SHLEntity
    {
        public string LotID { get; set; }

        public string FoupID { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }

        public int Priority { get; set; }

        public string OpeNo { get; set; }

        public string EqpType { get; set; }

        public string Department { get; set; }

        public string QuotaOwner { get; set; }

        public int QuotaType { get; set; }

        public string Project { get; set; }

        public string Purpose { get; set; }

        public int Qty { get; set; }

        public string Description { get; set; }

        public string LotStates { get; set; }

        public string ProcessStates { get; set; }

        public DateTime WaferStart { get; set; }

        public double YSDT { get; set; }

        public DateTime WAT { get; set; }

        public DateTime WaferOut { get; set; }

        public DateTime TargetWaferOut { get; set; }

        //  public TimeSpan Gap { get; set; }

        public string PriChgStage { get; set; }

        public string Remark { get; set; }

        public string strQuotaType { get { return QuotaType==1 ? "Project" : "Normal"; } }

        public string strWaferStart { get { return WaferStart.ToString("yyyy-MM-dd HH:mm"); } }

        public string strWAT { get { return WAT.ToString("yyyy-MM-dd"); } }

        public string strWFOut { get { return WaferOut.ToString("yyyy-MM-dd"); } }

        public string strTargetWFOut { get { return TargetWaferOut.ToString("yyyy-MM-dd"); } }

        public string Gap { get { return (TargetWaferOut - WaferOut).TotalDays.ToString("0.0"); } }

        public string ProductID { get; set; }

        public string QuotaDept { get; set; }

    }
}