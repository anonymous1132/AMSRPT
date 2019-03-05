using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FREQPModel_CurState
    {
        public string Eqp_ID { get; set; }

        public string E10_State { get; set; }

        public string Cur_State_ID { get; set; }

        public DateTime State_History_Time { get; set; }
    }
}
