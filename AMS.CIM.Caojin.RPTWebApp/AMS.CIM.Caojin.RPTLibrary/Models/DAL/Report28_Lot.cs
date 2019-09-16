using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ORM For Req028.
    /// ISTRPT.Report28_Lot
    /// </summary>
    public class Report28_Lot
    {
        public string Lot_ID { get; set; }

        public string ProdSpec_ID { get; set; }

        public int Qty { get; set; }

        public int Priority_Class { get; set; }

        public string Lot_Hold_State { get; set; }

        public DateTime Claim_Time { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_No { get; set; }

        public string Lot_Inv_State { get; set; }

        public string Lot_Type { get; set; }//wipchart v2

        public string Source { get; set; }
    }
}
