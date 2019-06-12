using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002MoveEntity
    {
        public string Department { get; set; }

        public int MoveValue { get; set; }

        public int MoveTarget { get; set; } = 0;

        public double Percentage { get; set; }

        public double TurnRate { get; set; }

        public double AvaWip { get; set; }
    }
}