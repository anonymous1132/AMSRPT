using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.RPT_WaferPlan_Out
    /// Req011
    /// </summary>
    public class RPT_WaferPlan_Out
    {
        public string ProdSpec_ID { get; set; }

        public string Plan_Date { get; set; }

        public int Plan_Ship_Pcs { get; set; }

        public int Plan_Wip_Pcs { get; set; }
    }
}
