using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class SHLFRLot_Family
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
    }
}
