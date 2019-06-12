using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report64_DetailModel
    {
        public int Gno { get; set; }
        
        public int Cno { get; set; }

        public int Ctype { get; set; }

        public string Ctitle { get; set; }

        public string Mfld_ID { get; set; }

        public string DcSpec_ID { get; set; }

        public string Item_NM { get; set; }

        public string Cname { get; set; }

        public int Sample_Size { get; set; }

        public int Max_Points { get; set; }

        public double? Upl_Value { get; set; }

        public double? Usl_Value { get; set; }

        public double? Ucl_Value { get; set; }

        public double? Uwl_Value { get; set; }

        public double? Target_Value { get; set; }

        public double? Mean_Value { get; set; }

        public double? Lpl_Value { get; set; }

        public double? Lsl_Value { get; set; }

        public double? Lcl_Value { get; set; }

        public double? Lwl_Value { get; set; }

        public double? Act_Mean_Value { get; set; }

        public string Value_List { get; set; }

        public string Time_List { get; set; }

        public DateTime? From_Time { get; set; }

        public DateTime? To_Time { get; set; }
    }
}
