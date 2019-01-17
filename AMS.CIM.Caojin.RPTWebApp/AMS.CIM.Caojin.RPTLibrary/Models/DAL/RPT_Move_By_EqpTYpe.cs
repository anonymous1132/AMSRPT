using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// Report05
    /// </summary>
    public class RPT_Move_By_EqpType
    {
        public DateTime Start_Time { get; set; }

        public string Product_ID { get; set; }

        public string EqpType { get; set; }

        public int MoveQty { get; set; }
    }
}
