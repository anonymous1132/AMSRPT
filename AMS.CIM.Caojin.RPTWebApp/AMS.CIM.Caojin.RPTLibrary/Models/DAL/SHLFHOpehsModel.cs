using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// Used for super hot lot
    /// MMVIEW.FHOpehs
    /// </summary>
    public class SHLFHOpehsModel
    {
        public string Lot_ID { get; set; }
        public string PD_ID { get; set; }
        public string Stage_ID { get; set; }
        public int Priority_Class { get; set; }
        public DateTime Claim_Time { get; set; }
        public string Ope_Category { get; set; }
        public string Ope_No { get; set; }
    }
}
