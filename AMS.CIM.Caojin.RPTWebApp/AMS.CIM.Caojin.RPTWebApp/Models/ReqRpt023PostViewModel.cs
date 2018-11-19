using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt023PostViewModel
    {
        public string Departments { get; set; }

        public string LotTypes { get; set; }

        public string Product { get; set; }

        public string FromDateTime { get; set; }

        public string ToDateTime { get; set; }
    }
}