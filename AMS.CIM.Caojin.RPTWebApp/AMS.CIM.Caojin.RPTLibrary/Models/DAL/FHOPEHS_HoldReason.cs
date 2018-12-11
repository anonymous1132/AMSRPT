using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    //first used by reqrpt014 hold rate top3
    public class FHOPEHS_HoldReason
    {
        public string Lot_ID { get; set; }

        public string Lot_Type { get; set; }

        public DateTime Claim_Time { get; set; }

        public string ProdSpec_ID { get; set; }

        public string Hold_Type { get; set; }

        public string Hold_Reason_Code { get; set; }

        public string Hold_Reason_Desc { get; set; }

        public string Ope_Category { get; set; }

        public string PD_ID { get; set; }
    }
}
