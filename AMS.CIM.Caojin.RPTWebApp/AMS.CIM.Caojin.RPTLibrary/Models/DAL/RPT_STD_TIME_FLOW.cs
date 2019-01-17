using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPT_STD_TIME_FLOW
    {
        public DateTime Flow_Record_Date { get; set; } = DateTime.Now.Date;

        public string ProdSpec_ID { get; set; } = "";

        public string MainPD_ID { get; set; } = "";

        public string Ope_No { get; set; } = "";

        public string PD_ID { get; set; } = "";

        public double PD_Proc_Time_Min { get; set; } = 0;

        public int PD_Proc_Time_Type { get; set; } = 0;

        public double PD_Wait_Time_Min { get; set; } = 0;

        public int PD_Wait_Time_Type { get; set; } = 0;

        public double PD_Cycle_Time_Min { get; set; } = 0;

        public int PD_Cycle_Time_Type { get; set; } = 0;

    }
}
