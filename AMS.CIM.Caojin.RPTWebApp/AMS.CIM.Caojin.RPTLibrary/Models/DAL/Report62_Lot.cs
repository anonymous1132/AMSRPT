using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.Report62_lot
    /// </summary>
    public class Report62_Lot
    {
        public string Lot_ID { get; set; }

        public int Qty { get; set; }

      //  public string Lot_Owner_ID { get; set; }

        public string Cast_ID { get; set; }

        public string Foup_Owner { get; set; }

        public string Foup_Owner_Name { get; set; }
    }
}
