using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Rpt_Quota_Define
    {
        public string Department { get; set; }

        public int Quota_Type { get; set; }
        
        public string Project_Desc { get; set; }

        public string Purpose { get; set; }

        public int Quota_SHL { get; set; }

        public int Quota_HL { get; set; }
    }
}
