using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPT_DAI_TurnRate
    {
        public DateTime Start_Time { get; set; }

        public string Product_ID { get; set; }

        public int MoveQty { get; set; } = 0;

        public int EffectiveSteps { get; set; } = 0;

        public int WIP { get; set; } = 0;
    }
}
