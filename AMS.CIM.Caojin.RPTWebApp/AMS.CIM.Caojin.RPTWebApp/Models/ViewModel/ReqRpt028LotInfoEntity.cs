using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt028LotInfoEntity
    {
        public string EqpType { get; set; }

        public int Priority { get; set; }

        public string OpeNo { get; set; }

        public string OpeName { get; set; }

        public string ProductID { get; set; }

        public string LotID { get; set; }

        public string Foup { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }

        public string LotProcStatus { get; set; }

        public string LotHoldStatus { get; set; }

        public string HoldReason { get; set; }

        public string HoldReasonDesc { get; set; }

        public int Qty { get; set; }

        public double WaitTime { get; set; }

        public double StatusTime { get; set; }

        public string CustomerDate { get; set; }

        public string OpeStartTime { get; set; }

        public string PreLayer { get; set; }

        public string ChgUserID { get; set; }

        public string ChgUserName { get; set; }
    }
}