using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt016LotHoldSummaryEntity
    {
        public string Department { get; set; }

        public string ReasonCode { get; set; }

        public int Qty { get; set; }

        public List<double> Values { get; set; } = new List<double>();
 
    }
}