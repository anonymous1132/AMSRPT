using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// 长语句sql，ReqRpt002 All in one
    /// </summary>
    public class RPT_Move_ByDepartmentModel
    {
        public string Lot_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string PD_ID { get; set; }

        public string PD_Name { get; set; }

        public string Department { get; set; }
    }
}
