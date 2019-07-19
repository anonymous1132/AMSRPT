using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ISTRPT.EDA_Prod_Lot_Mapping
    /// </summary>
    public class EDA_Prod_Lot_MappingModel
    {
        public string ProdSpec_ID { get; set; }

        public string Prod_Category_ID { get; set; }

        public string Lot_ID { get; set; }
    }
}
