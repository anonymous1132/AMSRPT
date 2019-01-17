using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPT_STD_TIME_FLOW_T
    {
        public string Lot_ID { get; set; }

        public string ProdSpec_ID { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_No { get; set; }

        public int PD_Proc_Time_Sec { get; set; }

        public int PD_Wait_Time_Sec { get; set; }
    }
}
