using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt063ClickCountEntity
    {
        public string PrivilegeID { get; set; }

        public List<double> ClickCountValues { get; set; } = new List<double>();
    }
}