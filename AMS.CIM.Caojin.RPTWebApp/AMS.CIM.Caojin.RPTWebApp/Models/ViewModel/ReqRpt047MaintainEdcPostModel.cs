using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt047MaintainEdcPostModel
    {
        /// <summary>
        /// Operate Type :insert、update、delete
        /// </summary>
        public string OpeType { get; set; }

        public string EqpID { get; set; }

        public string EdcPlan { get; set; }

        public double Period { get; set; }

        public string PeriodType { get; set; }

    }
}