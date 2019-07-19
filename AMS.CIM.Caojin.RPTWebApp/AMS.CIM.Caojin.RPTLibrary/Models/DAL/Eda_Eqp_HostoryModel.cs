using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Eda_Eqp_HostoryModel
    {
        public string Lot_ID { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime? End_Time { get; set; }

        public string Eqp_ID { get; set; }

        public string Cast_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; } 

        public string MainPD_ID { get; set; }

        public string PD_ID { get; set; }

        public string Ope_No { get; set; }

        public string PD_Name { get; set; }

        public string Recipe_ID { get; set; }

        public string Owner_ID { get; set; }

        public string User_Full_Name { get; set; }

        public string Dept { get; set; }

        public string Eqp_Type { get; set; }
    }
}
