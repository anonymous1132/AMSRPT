using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt023TableEntity
    {
        public Dictionary<string,int> Reasons_Pcs { get; set; }


        public string Department { get; set; }

        public int Total_Pcs { get { return Reasons_Pcs.Values.Sum(); } }
    }
}