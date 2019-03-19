using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048ProjectQuotaEntity
    {
        public string Department { get; set; }

        public string Project { get; set; }

        public string Purpose { get; set; }

        public int QuotaType { get; set; }

        public int QuotaSHL { get; set; }

        public int UsedSHL { get; set; }

        public int RemnantSHL { get { return QuotaSHL - UsedSHL; } }

        public int QuotaHL { get; set; }

        public int UsedHL { get; set; }

        public int RemnantHL { get { return QuotaHL - UsedHL; } }
    }
}