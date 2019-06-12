using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002WipEntity
    {
        public string Department { get; set; }

        public int Lots { get; set; }

        public int Wafers { get; set; }

        public int HoldWafers { get; set; }

        public int HoldLots { get; set; }

        public int HoldLotOverTime { get; set; }
    }
}