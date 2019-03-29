using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    /// <summary>
    /// ORM Obj req028
    /// ISTRPT.RPT_WipChart_TargetOut
    /// </summary>
    public class RPT_WipChart_TargetOut
    {
        public string ProdSpec_ID { get; set; }
        public int CurOut { get; set; }
        public int YstdOut { get; set; }
        public int TargetOut { get; set; }
    }
}
