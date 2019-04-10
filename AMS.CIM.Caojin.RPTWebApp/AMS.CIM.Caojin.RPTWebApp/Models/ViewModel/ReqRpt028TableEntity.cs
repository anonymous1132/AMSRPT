using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt028TableEntity
    {
        public string Product { get; set; }

        public int TargetOut { get; set; }

        public int ForecastOut { get; set; }

        public int AlreadyOut { get; set; }

        public int CanOut { get { return ForecastOut + AlreadyOut; } }

        public int Qty { get { return CanOut-TargetOut; } }

        public string KeyStage { get; set; }

        public string MotherStage { get; set; }

        public double Day { get; set; }

    }
}