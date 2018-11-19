using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FHScrap_SummaryModel
    {
        public string Lot_ID { get; set; }

        public string Wafer_ID { get; set; }

        public string PD_ID { get; set; }

        public DateTime Scrap_Time { get; set; }

        public string Reason_Code { get; set; }
    }
}
