using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt212RowEntity
    {
        public string LotID { get; set; }

        public string RouteID { get; set; }

        public string OperID { get; set; }

        public string OpeNo { get; set; }

        public string OpeName { get; set; }

        public string EQP { get; set; }

        public string OperTime { get; set; }

        public string ProdID { get; set; }

        public string[] ChamberArray { get; set; } = new string[25] ;
    }
}