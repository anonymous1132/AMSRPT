using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048GapSplitTimeEntity
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<string> Departments { get; set; } = new List<string>();

        public double UnitDuration { get { return (EndTime - StartTime).TotalMinutes / Departments.Count; } }
    }
}