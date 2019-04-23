using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// istrpt.report11_lotdetail
    /// </summary>
    public class Report11_LotDetail
    {
        public string Lot_ID { get; set; }

        public string ProdSpec_ID { get; set; }

        public string Lot_Inv_State { get; set; }

        public int Priority_Class { get; set; }

        public int Qty { get; set; }

        public DateTime Complete_Time { get; set; }

        public DateTime Next_Time { get; set; }

        public string Bank_ID { get; set; }

        public string Comment { get; set; }
    }
}
