using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class RPT_RealTime_Lin
    {
        public DateTime Start_Time { get; set; }

        public string Product_ID { get; set; }

        public string PartName { get; set; }

        public int MoveQty { get; set; }

        public int ScrappedQty { get; set; }

    }
}
