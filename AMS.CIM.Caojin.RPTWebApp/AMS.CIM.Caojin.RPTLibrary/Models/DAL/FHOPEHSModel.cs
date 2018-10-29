using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FHOPEHSModel
    {
        public string Eqp_ID { get; set; }

        public DateTime Claim_Time { get; set; }

        public int Cur_Wafer_QTY { get; set; }

        public int Ope_Pass_Count { get; set; }
    }
}
