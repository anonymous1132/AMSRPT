using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt020PostModel
    {
        /// <summary>
        /// From Date
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// To Date
        /// </summary>
        public string To { get; set; }

        public string EqpTypes { get; set; }
    }
}