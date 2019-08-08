using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    ///Req028 ORM Obj
    /// istrpt.report48_forecast
    /// </summary>
    public class Report28_Flow
    {
        public string ProdSpec_ID { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_No { get; set; }

        public string ModulePD_ID { get; set; }

        public string ModulePD_Name { get; set; }

        public string PD_ID { get; set; }

        public double PD_Std_Cycle_Time_Min { get; set; }
    }
}
