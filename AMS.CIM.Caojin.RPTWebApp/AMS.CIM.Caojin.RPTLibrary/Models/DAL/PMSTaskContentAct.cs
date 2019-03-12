using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// PMSVIEW.Task_Content_ACT
    /// 用于RPT SHL报表
    /// </summary>
    public class PMSTaskContentAct
    {
        public string Eqp_ID { get; set; }

        public string PM_ID { get; set; }

        public string Trigger_Type { get; set; }

        public string Description { get; set; }

        public string Day_Start_Time { get; set; }

        public string Next_PM_Date { get; set; }

        public string Next_Early_Date { get; set; }

        public string Next_Late_Date { get; set; }
    }
}
