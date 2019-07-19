using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt211RowEntity
    {
        public string LotID { get; set; }

        public string FoupID { get; set; }
        
        public int Qty { get; set; }

        public string RouteID { get; set; }

        public string OperID { get; set; }

        public string OperNo { get; set; }

        public string OperName { get; set; }

        public string OperCategory { get; set; }

        public string OperTime { get; set; }

        public string ClaimMemo { get; set; }

        public string UserID { get; set; }

        public string UserFullName { get; set; }

        public string UserDept { get; set; }

        public bool[] WaferList { get; set; } = new bool[25];
    }
}