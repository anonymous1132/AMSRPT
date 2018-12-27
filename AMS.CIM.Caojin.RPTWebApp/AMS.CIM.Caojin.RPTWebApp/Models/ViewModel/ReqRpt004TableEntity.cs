using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt004TableEntity
    {

        public DateTime Date { get; set; }

        public double WIP { get; set; }

        public int Move { get; set; }

        public double TurnRate { get { return WIP==0?0: Move / WIP; } }

        public string StrTurnRate { get { return TurnRate.ToString("0.00"); } }

        public string StrDate { get { return Date.ToString("yyyyMMdd"); } }
    }
}