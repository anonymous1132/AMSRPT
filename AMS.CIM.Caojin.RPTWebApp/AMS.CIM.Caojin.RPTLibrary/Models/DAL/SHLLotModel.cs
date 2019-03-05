using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// Used for super hot lot
    /// MMVIEW.FRLot
    /// </summary>
    public class SHLLotModel
    {
        public string Lot_ID { get; set; }
        public string ProdSpec_ID { get; set; }
        public int Priority_Class { get; set; }
        public string Lot_Hold_State { get; set; }
        public string Lot_Process_State { get; set; }
        public int Qty { get; set; }
        public DateTime Created_Time { get; set; }
        public string Lot_Family_ID { get; set; }
        public string Split_Lot_ID { get; set; }
        public string Hold_Claim_Memo { get; set; }
    }
}
