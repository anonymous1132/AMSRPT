using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt047TableEntity
    {
        public string EdcPlan { get; set; }
        public double Period { get; set; }
        public string PeriodType { get; set; }
        public string TestTime { get; set; }
        public bool SpecResult { get; set; }
        public int Count { get; set; }

        public DateTime GetPeriodStartTime()
        {
            if (PeriodType == "D") { return DateTime.Now.AddDays(Period*-1); }
            if (PeriodType == "M") { return DateTime.Now.AddMonths(Convert.ToInt16(Period) * -1); }
            else throw new Exception("Unknown Period Type");
        }
    }
}