using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt014DepartmentTableEntity
    {
        //public string Department { get; set; }

        public string HoldCode { get; set; } = "";

        public double HoldRate { get; set; } = 0;

        public int LotCount { get; set; } = 0;

        public string StrHoldRate { get { return HoldRate.ToString("0.00") + "%"; } }
    }
}