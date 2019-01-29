using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018Chart2PostModel
    {
        public string EqpID { get; set; }

        public List<ReqRpt018Chart2PostEntity> Entities { get; set; }
    }
}