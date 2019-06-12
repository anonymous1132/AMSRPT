using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002SetMoveTargetPostModel
    {
        public DateTime date { get; set; }

        public List<int> dayArray { get; set; }

        public List<ReqRpt002SetMoveTargetDeptEntity> deptData { get; set; }
    }
}