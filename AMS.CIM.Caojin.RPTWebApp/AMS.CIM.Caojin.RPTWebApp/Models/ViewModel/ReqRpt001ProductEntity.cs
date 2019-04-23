using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt001ProductEntity
    {
        public string ProductID { get; set; }

        public List<ReqRpt001PlanEntity> Plans { get; set; } = new List<ReqRpt001PlanEntity>();

        public double OriginalTarget { get; set; }

        public double CurrentTarget { get; set; }

    }
}