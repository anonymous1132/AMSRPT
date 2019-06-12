using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt022TableRowEntity
    {
        public string LotID { get; set; }

        public string Owner { get; set; }

        public string LotType { get; set; }

        public string ScrapType { get; set; }

        public string ScrapTime { get; set; }

        public string EventTime { get; set; }

        public string MainPD { get; set; }

        public string OpeNo { get; set; }

        public string ModulePD { get; set; }

        public int Qty { get; set; }

        public string EqpType { get; set; }

        public string Module { get; set; }

        public string User { get; set; }

        public string Code { get; set; }

        public string CodeDesc { get; set; }

        public string ClaimMemo { get; set; }
    }
}