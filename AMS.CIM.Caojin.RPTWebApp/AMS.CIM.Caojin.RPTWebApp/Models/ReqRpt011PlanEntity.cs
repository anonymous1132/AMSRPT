using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt011PlanEntity
    {
        public int Target { get; set; }

        public int Actual { get; set; }

        public int Gap { get { return Actual - Target; } }

        public List<string> Lots { get; set; } = new List<string>();
    }
}