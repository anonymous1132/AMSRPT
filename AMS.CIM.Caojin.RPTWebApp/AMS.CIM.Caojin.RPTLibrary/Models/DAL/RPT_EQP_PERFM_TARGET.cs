using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPT_EQP_PERFM_TARGET
    {
        public string Eqp_Type { get; set; }

        public double Upm_Target { get; set; }
        
        public double Uum_Target { get; set; }

        public double PassQty_Target { get; set; }

        public double Rework_Target { get; set; }

        public double Eff_Target { get; set; }

        public double Wph { get; set; }
    }
}
