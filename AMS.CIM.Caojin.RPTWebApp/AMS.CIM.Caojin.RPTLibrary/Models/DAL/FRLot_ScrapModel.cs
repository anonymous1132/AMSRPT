using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FRLot_ScrapModel
    {
        public string D_TheSystemKey { get; set; }

        public string Lot_ID { get; set; }

        public string Lot_Finished_State { get; set; }

        public string Bank_ID { get; set; }

        public string Lot_Type { get; set; }

        public string Sub_Lot_Type { get; set; }

        public int QTY { get; set; }

    }
}
