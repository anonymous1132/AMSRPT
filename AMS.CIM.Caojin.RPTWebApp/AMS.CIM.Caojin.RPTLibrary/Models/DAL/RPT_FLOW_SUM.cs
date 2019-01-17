using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// MMVIEW.RPT_FLOW_SUM
    /// </summary>
    public class RPT_FLOW_SUM
    {
        public string ProdSpec_ID { get; set; } = "";
        public string MainPD_ID { get; set; } = "";
        public string Ope_No { get; set; } = "";
        //public string ModulePD_ID { get; set; } = "";
        public string PD_ID { get; set; } = "";
      //  public string PD_Name { get; set; } = "";
       // public string PD_Type { get; set; } = "";
      //  public string Flow_Type { get; set; } = "";
      //  public string Stage_ID { get; set; } = "";
        public string LRecipe { get; set; } = "";
        public string MRecipe_List { get; set; } = "";
        public string Eqp_List { get; set; } = "";
        public string Eqp_Type { get; set; } = "";
      //  public string Last_Update { get; set; } = "";
      //  public DateTime Last_Update_Time { get; set; } = DateTime.Now.Date;
    }
}
