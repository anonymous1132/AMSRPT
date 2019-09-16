using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt213TableRowEntity
    {
        public string LotID { get; set; }

        public string FoupID { get; set; }

        public int Qty { get; set; }

        /// <summary>
        /// MainPD
        /// </summary>
        public string RouteID { get; set; }

        /// <summary>
        /// ModulePD
        /// </summary>
        public string OperID { get; set; }

        public string OperNo { get; set; }

        public string OperName { get; set; }

        public string EqpType { get; set; } = "DF-PLY";

        public string EqpID { get; set; }

        public string RecipeID { get; set; }

        public string Position { get; set; }

        public string OpeStartTime { get; set; }

        public string OpeCompleteTime { get; set; }

        public string RunHrs { get; set; }

        public string UserID { get; set; }

        public string UserFullName { get; set; }

        public string UserDept { get; set; }

        public string Prod { get; set; }
    }
}