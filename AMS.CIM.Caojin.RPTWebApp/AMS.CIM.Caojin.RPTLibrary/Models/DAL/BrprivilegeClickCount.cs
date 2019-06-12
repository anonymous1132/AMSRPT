using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// 长sql语句 reqrpt0063
    /// </summary>
    public class BrprivilegeClickCount
    {
        public string PrivilegeID { get; set; }

        public double Usage_Counter { get; set; }

        public string PrivilegeCategory { get; set; }

        public string PrivilegeName { get; set; }
    }
}
