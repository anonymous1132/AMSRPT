using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPT_Dept_CT_By_Prod
    {
        public string ProdSpec_ID { get; set; }

        public string Department { get; set; }

        public double Total_CT { get; set; } = 0;
    }
}
