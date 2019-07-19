using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.EDA_Lot_Wafer
    /// </summary>
    public class EDA_Lot_Wafer_HistModel
    {
        public string Lot_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public DateTime Claim_Time { get; set; }

        public string Ope_Category { get; set; }

        public string Cast_ID { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_No { get; set; }

        public string PD_ID { get; set; }

        public string PD_Name { get; set; }

        public string Claim_Memo { get; set; }

        public string Claim_User_ID { get; set; }

        public string User_Full_Name { get; set; }

        public string Dept { get; set; }

      //  public string ProdSpec_ID { get; set; }

      //  public string Prod_Category_ID { get; set; }

        public string WafeList { get; set; }
    }
}
