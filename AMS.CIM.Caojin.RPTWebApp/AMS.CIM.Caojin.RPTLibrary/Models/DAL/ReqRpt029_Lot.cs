using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class ReqRpt029_Lot
    {
        public string Lot_ID { get; set; }

        public string Ope_No { get; set; }

        public string MainPD_ID { get; set; }

        public int Priority_Class { get; set; }

        public int Qty { get; set; }

        public string Lot_Hold_State { get; set; }

        public string Lot_Process_State { get; set; }

        public string From_Ope_No { get; set; }

        public string To_Ope_No { get; set; }

        public int Qtime { get; set; }

        public string Cast_ID { get; set; }

        public string Location { get; set; }

        public string Xfer_State { get; set; }

        public string ReasonCode_ID { get; set; }

        public string Hold_Claim_Memo { get; set; }
    }
}
