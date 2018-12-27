using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt004ChartDataEntity
    {
        public string label { get; set; }

        //public Dictionary<DateTime, double> Data { get; set; } = new Dictionary<DateTime, double>();
        public List<ArrayList> data { get; set; } = new List<ArrayList>();
    }
}