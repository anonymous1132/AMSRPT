using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report36_Chamber
    {
        public string EQP_ID { get; set; }

        public string EQP_State { get; set; }

        public string E10_State { get; set; }

        public string New_EQP_State { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime End_Time { get; set; }

        public string Description { get; set; }

        public string Claim_Memo { get; set; }

        public string Claim_User_ID { get; set; }
    }
}
