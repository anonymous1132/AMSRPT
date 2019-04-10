using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt047TableRowEntity
    {
        public string EqpID { get; set; }

        public List<ReqRpt047TableEntity> EdcEntities { get; set; } = new List<ReqRpt047TableEntity>();
    }
}