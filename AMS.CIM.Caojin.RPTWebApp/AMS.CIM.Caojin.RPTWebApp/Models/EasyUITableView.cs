using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class EasyUITableView<T>
    {
        public int total { get { return rows.Count; } }

        public List<T> rows { get; set; }
    
    }
}