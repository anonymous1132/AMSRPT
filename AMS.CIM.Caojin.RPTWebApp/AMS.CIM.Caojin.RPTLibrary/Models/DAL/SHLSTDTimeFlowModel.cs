using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// Used for Super Hot Lot
    /// MMVIEW.RPT_STD_Time_Flow
    /// </summary>
    public class SHLSTDTimeFlowModel
    {
      public string ProdSpec_ID { get; set; }
      public string Ope_No { get; set; }
      public string PD_ID { get; set; }
      public double PD_STD_Cycle_Time_Min { get; set; }
       public double PD_STD_Proc_Time_Min { get; set; }
      public string Eqp_Type { get; set; }
    }
}
