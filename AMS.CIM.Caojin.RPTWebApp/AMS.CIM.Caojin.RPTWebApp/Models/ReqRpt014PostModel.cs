using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt014PostModel
    {
        public string ProductID { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string StartCategory { get; set; }

        public string EndCategory { get; set; }
    }
}