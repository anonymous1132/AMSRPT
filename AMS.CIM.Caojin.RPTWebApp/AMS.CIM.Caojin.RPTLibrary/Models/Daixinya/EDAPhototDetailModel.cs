using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class EDAPhototDetailModel
    {
        public string Lot_ID { get; set; }

        public DateTime Claim_Time { get; set; }

        public string Ope_Category { get; set; }

        public string Cast_ID { get; set; }

        public int Cur_Wafer_Qty { get; set; }

        public string MainPD_ID { get; set; }

        public string Ope_NO { get; set; }

        public string PD_Name { get; set; }

        public string Eqp_ID { get; set; }

        public string Eqp_Type { get; set; }

        public string Recipe_ID { get; set; }

        public string PH_Recipe_ID { get; set; }

        //public string Wafer_ID { get; set; } = "";

        public string Claim_User_ID { get; set; }

        public string User_Full_Name { get; set; }

        public string Dept { get; set; }

        public string Ctrl_Job { get; set; }

        public string ModulePD_ID { get; set; }
    }
}
