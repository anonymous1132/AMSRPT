using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// FHOPEHS
    /// REQ001
    /// </summary>
    public class FHOPEHS_STB
    {
        public string Lot_ID { get; set; }

        public string ProdSpec_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public DateTime Claim_Time { get; set; }

    }
}
