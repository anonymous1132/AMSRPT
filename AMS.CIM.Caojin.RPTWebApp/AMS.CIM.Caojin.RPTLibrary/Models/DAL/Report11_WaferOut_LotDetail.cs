using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.Report11_WaferOut_LotDetail
    /// </summary>
    public class Report11_WaferOut_LotDetail
    {
        public string Lot_ID { get; set; }

        public DateTime Completion_Time { get; set; }

        public int Qty { get; set; }

        public string ProdSpec_ID { get; set; }

        public string Out_Type { get; set; }
    }
}
