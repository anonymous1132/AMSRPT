using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt214TableRowEntity
    {
        public string LotID { get; set; }

        public string FoupID { get; set; }

        public int Qty { get; set; }
        
        public string RouteID { get; set; }

        public string OperID { get; set; }

        public string OperNo { get; set; }

        public string OperName { get; set; }

        public string EqpType { get; set; }

        public string EqpID { get; set; }

        public string RecipeID { get; set; }

        public List<string> WaferValue { get; set; } = new List<string>();

        public string OpeStartTime { get; set; }

        public string OpeCompleteTime { get; set; }

        public string RunHrs { get; set; }

        public string UserID { get; set; }

        public string UserFullName { get; set; }

        public string UserDept { get; set; }

    }
}