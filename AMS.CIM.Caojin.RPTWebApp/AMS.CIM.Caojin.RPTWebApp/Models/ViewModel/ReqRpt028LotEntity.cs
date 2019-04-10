using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt028LotEntity
    {
        public string LotID { get; set; }

        public int Qty { get; set; }

        public int Priority { get; set; }

        public string HoldState { get; set; }

        public bool InBank { get; set; }
    }
}