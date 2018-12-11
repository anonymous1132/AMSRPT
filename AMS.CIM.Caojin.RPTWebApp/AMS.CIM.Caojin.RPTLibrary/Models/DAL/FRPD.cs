using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// MMView.FRPD
    /// 第一次用于Req14 Hold Rate Top3
    /// </summary>
    public class FRPD
    {
        public string PD_ID { get; set; }

        /// <summary>
        /// 部门短码
        /// </summary>
        public string Department { get; set; }
    }
}
