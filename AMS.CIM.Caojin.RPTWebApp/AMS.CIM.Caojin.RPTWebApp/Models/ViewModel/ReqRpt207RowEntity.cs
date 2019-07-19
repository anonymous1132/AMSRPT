using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt207RowEntity
    {
        public string LotID { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string EqpType { get; set; }

        public string EqpID { get; set; }

        public string CastID { get; set; }

        public int Qty { get; set; }

        public string MainPDID { get; set; }

        public string PDID { get; set; }

        public string OpeNo { get; set; }

        public string PDName { get; set; }

        public string RecipeID { get; set; }

        public string UserID { get; set; }

        public string UserFullName { get; set; }

        public string Dept { get; set; }

        public string RunDur { get; set; }
    }
}