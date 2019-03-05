using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FHOPEHS_SHL_Stage2
    {
        public string Lot_ID { get; set; }

        public string MainPD_ID { get; set; }

        public string PD_ID { get; set; }

        public string Ope_NO { get; set; }

        public string Ope_Category { get; set; }

        public int Priority_Class { get; set; }

        public DateTime Claim_Time { get; set; }
    }
}
