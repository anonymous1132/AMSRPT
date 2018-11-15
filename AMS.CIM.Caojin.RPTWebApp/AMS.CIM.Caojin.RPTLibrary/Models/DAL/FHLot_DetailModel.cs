using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FHLot_DetailModel
    {
        public string Lot_ID { get; set; }

        public string Cast_ID { get; set; }

        public string Status { get; set; }

        public string Location { get; set; }

        public string Ope_Category { get; set; }

        public string Lot_Type { get; set; }

        public DateTime Claim_Time { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string Ope_No { get; set; }

        public string ModulePD_ID { get; set; }

        public string PD_Name { get; set; }

        public string Claim_User_ID { get; set; }

        public string Eqp_ID { get; set; }

        public string Recipe_ID { get; set; }

        public string Reticle_ID { get; set; }

        public string Reason_Code { get; set; }

        public string Reason_Description { get; set; }

        public string ProdSpec_ID { get; set; }
    }
}
