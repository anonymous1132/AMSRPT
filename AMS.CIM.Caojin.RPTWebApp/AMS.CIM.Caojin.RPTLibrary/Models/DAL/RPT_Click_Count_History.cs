using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// ORM ReqRpt0063
/// </summary>
namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPT_Click_Count_History
    {
        public string PrivilegeID { get; set; }

        public double Usage_Counter { get; set; }
        /// <summary>
        /// eg:2019-05-01
        /// </summary>
        public string Date { get; set; }
    }
}
