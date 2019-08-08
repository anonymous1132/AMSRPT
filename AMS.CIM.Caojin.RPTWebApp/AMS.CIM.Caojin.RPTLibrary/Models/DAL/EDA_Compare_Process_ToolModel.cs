using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// REQRPT212 ORM Entity
    /// </summary>
    public class EDA_Compare_Process_ToolModel
    {
        public string Lot_ID { get; set; }

        public string Wafer_ID { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_NO { get; set; }

        public string EQP_ID { get; set; }

        public string Procrsc_ID { get; set; }

        public string CtrlJob_ID { get; set; }

        public string PD_Name { get; set; }

        public DateTime Claim_Time { get; set; }

        public string ProdSpec_ID { get; set; }

        public string ModulePD_ID { get; set; }
    }
}
