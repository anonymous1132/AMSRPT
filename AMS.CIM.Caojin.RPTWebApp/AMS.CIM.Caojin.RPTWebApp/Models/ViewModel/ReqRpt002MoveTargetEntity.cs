using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002MoveTargetEntity
    {
        public string Department { get; set; }

        public string DeptCode { get; set; }

        public List<int> TargetList { get; set; } = new List<int>();
    }
}