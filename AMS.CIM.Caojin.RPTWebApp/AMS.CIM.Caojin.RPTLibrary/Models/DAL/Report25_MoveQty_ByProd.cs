using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report25_MoveQty_ByProd
    {
        public DateTime Claim_Time { get; set; }

        public string ProdSpec_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string PartName { get; set; }

        // 20181227新增，用于Req05
        public string Eqp_ID { get; set; }
    }
}
