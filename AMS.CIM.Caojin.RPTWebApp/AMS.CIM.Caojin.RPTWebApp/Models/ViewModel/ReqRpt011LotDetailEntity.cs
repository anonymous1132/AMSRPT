using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt011LotDetailEntity
    {
        public string ProductID { get; set; }

        public string LotID { get; set; }

        public string Status { get; set; }

        public int Pri { get; set; }

        public int Qty { get; set; }

        public string ShipTime { get; set; }

        public double StayDays { get; set; }

        public string Comment { get; set; }
    }
}