using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018Chart1PostModel
    {
        public string Date { get; set; }

        public List<ReqRpt018Chart1PostEntity> Entities { get; set; }
    }
}