using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// reqrpt028 
    /// istrpt.report28_lotdetail\ istrpt.report28_splitlotdetail
    /// </summary>
    public class Report28_LotDetail
    {
        public string Lot_ID { get; set; }

        public string Ope_No { get; set; }

        public string PD_Name { get; set; }

        public string Cast_ID { get; set; }

        public string Hold_Reason_Code { get; set; }

        public string Hold_Reason_Desc { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string Claim_User_ID { get; set; }

        public string Location { get; set; }

        public string State { get; set; }

        public string User_Name { get; set; }

        public string Lot_Hold_State { get; set; }

        public string Lot_Process_State { get; set; }

        public string ProdSpec_ID { get; set; }

        public string Claim_Time { get; set; }

        public int Priority_Class { get; set; }
    }
}
