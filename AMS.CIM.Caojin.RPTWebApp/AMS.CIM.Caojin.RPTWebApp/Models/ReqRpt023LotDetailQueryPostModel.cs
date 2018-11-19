using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt023LotDetailQueryPostModel
    {
       public string SelectedDepartment { get; set; }

        public string SelectedLotType { get; set; }

        public string SelectedProduct { get; set; }

        public string SelectedFrom { get; set; }

        public string SelectedTo { get; set; }

        public string Reason { get; set; }

        public string Department { get; set; }
    }
}