using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// Used for SHL(req048)
    /// ISTRPT.Rpt_lot_quota_mapping(取消),改动后，同部门不能同project_desc
    /// (view)ISTRPT.report_lot_quota_mapping
    /// </summary>
    public class Rpt_Lot_Quota_Mapping
    {
        public string Lot_ID { get; set; } = "";
        public int Quota_Type { get; set; } = -1;
        public string Project_Desc { get; set; } = "";
        public string Purpose { get; set; } = "";
    }
}
