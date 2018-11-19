using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class ScrapSummarizedModel
    {
        public string Lot_ID { get; set; }

        public string Bank_ID { get; set; }

        public string Lot_Type { get; set; }

        public string Sub_Lot_Type { get; set; }

        public DateTime Claim_Time { get; set; }

        public string ProdSpec_ID { get; set; }

        public int Qty { get; set; }

        public string Reason_Code { get; set; }
    }
}
