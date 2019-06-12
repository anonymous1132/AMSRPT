using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt011ProductEntity
    {
        public string ProductID { get; set; }

        public List<ReqRpt011PlanEntity> Plans { get; set; } = new List<ReqRpt011PlanEntity>();

    }
}