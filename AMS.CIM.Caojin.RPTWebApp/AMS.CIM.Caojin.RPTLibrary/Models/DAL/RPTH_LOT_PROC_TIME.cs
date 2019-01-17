using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPTH_LOT_PROC_TIME
    {
        public string Lot_ID { get; set; }

        public string ProdSpec_ID { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_No { get; set; }

        public DateTime Ope_Start_Time { get; set; }

        public DateTime Process_Start_Time { get; set; }

        public DateTime Process_End_Time { get; set; }

        public int Process_Duration_Sec { get; set; }

        public string Last_Ope_Category { get; set; }
    }
}
