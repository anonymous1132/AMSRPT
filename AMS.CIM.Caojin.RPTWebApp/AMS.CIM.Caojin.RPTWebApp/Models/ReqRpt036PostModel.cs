using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt036PostModel
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string EqpTypes { get; set; }

        public bool HasChamebr { get; set; }

        public bool HasLoadPort { get; set; }

        public bool IsToNow{get;set;}
    }
}