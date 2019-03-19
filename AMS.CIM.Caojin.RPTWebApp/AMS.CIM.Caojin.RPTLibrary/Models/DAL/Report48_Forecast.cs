using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report48_Forecast
    {
        public string Ope_NO { get; set; }

        public string Department { get; set; }

        public string Eqp_Type { get; set; }

        public string Eqp_List { get; set; }

        public string ModulePD_ID { get; set; }

        public string PD_ID { get; set; }

        public string LRecipe { get; set; }

        public double Pd_Std_Proc_Time_Min { get; set; }

        public double Pd_Std_Cycle_Time_Min { get; set; }

        public string MainPD_ID { get; set; }

        public double CT { get { return Pd_Std_Cycle_Time_Min > 0 ? Pd_Std_Cycle_Time_Min : 0; } }

        public double PT { get { return Pd_Std_Proc_Time_Min > 0 ? Pd_Std_Proc_Time_Min : 0; } }

    }
}
