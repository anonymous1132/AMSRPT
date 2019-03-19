using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048DepartmentQuotaEntity
    {
        public string Department { get; set; }

        public int NormalSHLQuota { get; set; }

        public int NormalSHLUsed { get; set; }

        public int NormalSHLRemnant
        {
            get { return NormalSHLQuota - NormalSHLUsed; }
        }

        public int NormalHLQuota { get; set; }

        public int NormalHLUsed { get; set; }

        public int NormalHLRemnant
        {
            get { return NormalHLQuota - NormalHLUsed; }
        }

        public int ProjectSHLQuota { get; set; }

        public int ProjectSHLUsed { get; set; }

        public int ProjectSHLRemnant { get { return ProjectSHLQuota - ProjectSHLUsed; } }

        public int ProjectHLQuota { get; set; }

        public int ProjectHLUsed { get; set; }

        public int ProjectHLRemnant { get { return ProjectHLQuota - ProjectHLUsed; } }
    }
}