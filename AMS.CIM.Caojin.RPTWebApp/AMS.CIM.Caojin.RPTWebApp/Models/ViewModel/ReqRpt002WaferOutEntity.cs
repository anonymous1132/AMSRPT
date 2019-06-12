using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002WaferOutEntity
    {
        public string Product { get; set; }

        public int OutSourceAccTarget { get; set; }

        public int OutSourceAccActual { get; set; }

        public int OutSourceGap { get { return OutSourceAccActual - OutSourceAccTarget; } }

        public int WFOutAccTarget { get; set; }

        public int WFOutAccActual { get; set; }

        public double WFOutYield { get; set; }
    }
}