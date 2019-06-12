using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ReqRpt016 ISTRPT.Report16_LotHold_Summary
    /// </summary>
    public class Report16_LotHold_Summary
    {
        public string Lot_ID { get; set; }

        public string Lot_Type { get; set; }

        public string ProdSpec_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string Reason_Code { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_NO { get; set; }

        public string PD_ID { get; set; }

        public string PD_Name { get; set; }

        public string Eqp_Type { get; set; }

        public string Hold_Type { get; set; }

        public string Hold_PD_Dept { get; set; }

        public string Hold_User_ID { get; set; }

        public string Hold_User_Name { get; set; }

        public string Hold_User_Dept { get; set; }

        public string Release_User_ID { get; set; }

        public string Release_User_Name { get; set; }

        public string Release_User_Dept { get; set; }

        public DateTime Hold_Time { get; set; }

        public DateTime? Release_Time { get; set; }

        public decimal Duration { get; set; }

        public string Hold_Comment { get; set; }

        public string Release_Comment { get; set; }
    }
}
