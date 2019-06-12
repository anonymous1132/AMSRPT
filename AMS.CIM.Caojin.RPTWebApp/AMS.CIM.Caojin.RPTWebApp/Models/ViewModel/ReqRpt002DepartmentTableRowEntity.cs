using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002DepartmentTableRowEntity
    {
        public string Department { get; set; }

        public int WipLot { get; set; } = 0;

        public int WipWafer { get; set; } = 0;

        public string WipDev { get; set; }

        public int HoldLot { get; set; } = 0;

        public int HoldWafer { get; set; } = 0;

        public int HoldLotOverTime { get; set; } = 0;

        public string HoldRate { get { return WipWafer == 0 ? "" : Math.Round(HoldWafer * 100.0 / WipWafer, 2).ToString() + "%"; } }

        public int YstdMoveTarget { get; set; } = 0;

        public int YstdMoveActual { get; set; } = 0;

        public string YstdMovePercentage { get; set; }

        public double YstdAvaWip { get; set; } = 0;

        public string YstdMoveTurnRate { get; set; }

        public int TdMoveTarget { get; set; } = 0;

        public int TdMoveActual { get; set; } = 0;

        public string TdMovePercentage { get; set; }
    }
}