using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.RPT_WaferPlan
    /// REQ001
    /// </summary>
    public class RPT_WaferPlan
    {
        public string ProdSpec_ID { get; set; }

        public string Plan_Date { get; set; }

        public int Plan_Start_Pcs { get; set; }

       // public string Plan_Out_Pcs { get; set; }
    }
}
