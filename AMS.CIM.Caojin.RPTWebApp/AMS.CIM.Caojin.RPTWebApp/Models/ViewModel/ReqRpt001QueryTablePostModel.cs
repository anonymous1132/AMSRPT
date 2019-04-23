using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt001QueryTablePostModel
    {
        public List<string> ProdList { get; set; }

        /// <summary>
        /// month or year
        /// </summary>
        public string DateType { get; set; }
        /// <summary>
        /// 2019 or 2019-4
        /// </summary>
        public string DateFromValue { get; set; }

        public string DateToValue { get; set; }
    }
}