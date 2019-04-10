using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.Rpt_Non_Lot_Edc_Plan
    /// ReqRpt047
    /// </summary>
    public class Rpt_Non_Lot_Edc_Plan
    {
        public string Eqp_ID { get; set; }

        public string Edc_Plan { get; set; }

        public double Period { get; set; } 

        public string Period_Type { get; set; }
    }
}
