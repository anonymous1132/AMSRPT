using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report22_Scrap_List
    {
        public string Lot_ID { get; set; }

        public string Owner_Name { get; set; }

        public string Lot_Type { get; set; }

     //   public string ProdSpec_ID { get; set; }

        public string Reason_MainPD_ID { get; set; }

        public string Reason_Ope_No { get; set; }

        public string Reason_Code { get; set; }

        public string Reason_Description { get; set; }

        public DateTime Scrap_Time { get; set; }

        public int Qty { get; set; }

        public string Claim_Memo { get; set; }

      //  public string Claim_User_ID { get; set; }

        public string Dept { get; set; }

        public string User_Name { get; set; }

        public string Eqp_Type { get; set; }

        public string ModulePD_ID { get; set; }

        public DateTime? Event_Time { get; set; }
    }
}
