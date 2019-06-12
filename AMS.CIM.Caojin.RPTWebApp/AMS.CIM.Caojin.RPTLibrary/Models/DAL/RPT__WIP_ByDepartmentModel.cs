using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// 长sql语句查询,ReqRpt002 All in one
    /// </summary>
    public class RPT__WIP_ByDepartmentModel
    {
        public string Lot_ID { get; set; }

        public DateTime Claim_Time { get; set; }

        public string PD_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string Department { get; set; }

        public string Bank_ID { get; set; }

        public string Hold_State { get; set; }

        public string Ope_Name { get; set; }

        public DateTime Hold_Time { get; set; } = DateTime.MinValue;
    }
}
