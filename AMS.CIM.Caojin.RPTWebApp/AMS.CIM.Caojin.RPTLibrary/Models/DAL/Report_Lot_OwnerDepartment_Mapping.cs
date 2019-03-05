using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// Used for super hot lot
    /// ISTRPT.Rpt_lot_quota_mapping
    /// </summary>
    public class Report_Lot_OwnerDepartment_Mapping
    {
       public string Lot_ID { get; set; }

       public string Lot_Owner_ID { get; set; }

       public string User_Full_Name { get; set; }

       public string Description { get; set; }

        public string DisplayOwner { get { return string.Format("{0}/{1}/{2}", Description, User_Full_Name, Lot_Owner_ID); } }
    }
}
