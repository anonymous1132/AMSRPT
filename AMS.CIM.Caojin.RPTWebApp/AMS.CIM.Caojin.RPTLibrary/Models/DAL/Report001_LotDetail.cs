using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report001_LotDetail
    {
        public string Lot_ID { get; set; }

        public string ProdSpec_ID { get; set; }

        public string Qty { get; set; }

        public string Vendor_Lot_ID { get; set; }

        public string Vendor_Name { get; set; }

        public string Lot_Type { get; set; }

        public string Lot_Owner_ID { get; set; }

        public DateTime Created_Time { get; set; }

        public string Vendor_ProdSpec_ID { get; set; }

        public string Foup_ID { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }
    }
}
