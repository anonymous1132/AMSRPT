using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt028ChartModel
    {
        public string Product { get; set; }

        public List<ReqRpt028ChartEntity> ChartEntities { get; set; } = new List<ReqRpt028ChartEntity>();

    }
}