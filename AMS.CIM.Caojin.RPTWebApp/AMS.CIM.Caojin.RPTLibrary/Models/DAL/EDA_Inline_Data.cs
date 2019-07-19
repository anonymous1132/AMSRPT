using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.EDA_Inline_Data
    /// reqrpt209 206
    /// </summary>
    public class EDA_Inline_Data
    {
        public string Meas_Lot_ID { get; set; }

        public string Meas_ProdSpec_ID { get; set; }

        public string Meas_MainPD_ID { get; set; }

        public string Meas_Ope_No { get; set; }

        public string Meas_PD_ID { get; set; }

        public int Meas_Pass_Count { get; set; }

        public string Meas_DcSpec_ID { get; set; }

        public string Meas_Eqp_ID { get; set; }

        public string Eqp_Type { get; set; }

        public DateTime Claim_Time { get; set; }

        public string Cast_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string Recipe_ID { get; set; }
        //public string PD_Type { get; set; }
        public string Wafer_ID { get; set; } = "";

        public string Wafer_Position { get; set; }

        public string Site_Position { get; set; }

        public string DcItem_Value { get; set; }

        public string Meas_Type { get; set; }

        public string Item_Type { get; set; }

        public string DcItem_Name { get; set; }

        public double Spec_Lower_Limit { get; set; }

        public double Cntl_Lower_Limit { get; set; }

        public double Cntl_Upper_Limit { get; set; }

        public double Spec_Upper_Limit { get; set; }

        public double DcItem_Target { get; set; }

        public double? RangeUC { get; set; }
    }
}
