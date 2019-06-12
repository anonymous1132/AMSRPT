using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.Report_SplitLot
    /// RptTrackInUpdaterForSplitLot.cs
    /// </summary>
    public class Report_SplitLot
    {
        public string Lot_ID { get; set; }

        public DateTime Split_Time { get; set; }

        public string Split_Lot_ID { get; set; }
    }
}
