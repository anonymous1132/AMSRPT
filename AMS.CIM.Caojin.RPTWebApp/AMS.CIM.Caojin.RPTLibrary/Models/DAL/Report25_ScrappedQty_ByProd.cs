using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class Report25_ScrappedQty_ByProd
    {
        public DateTime Scrap_Time { get; set; }

        public string ProdSpec_ID { get; set; }

        public int Qty { get; set; }

        public string PartName { get; set; }

    }
}
