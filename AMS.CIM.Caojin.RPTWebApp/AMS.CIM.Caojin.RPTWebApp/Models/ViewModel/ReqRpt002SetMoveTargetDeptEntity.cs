using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002SetMoveTargetDeptEntity
    {
        public string dept { get; set; }

        public List<int> targetArray { get; set; } 
    }
}