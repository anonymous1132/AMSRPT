using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// 长sql语句，reqrpt002 All in one
    /// </summary>
    public class Rpt_Move_Target
    {
        public string Department { get; set; }

        public string Plan_Date { get; set; }

        public int Target_Value { get; set; }

        public string Dept_Code { get; set; }
    }
}
