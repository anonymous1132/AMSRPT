using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// Use for super hot lot
    /// ISTRPT.Report24_Lot_Detail
    /// </summary>
    public class SHLFoupModel
    {
        public string Lot_ID { get; set; }
        public string Cast_ID { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }
}
