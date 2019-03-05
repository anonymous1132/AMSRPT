using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    //ISTRPT.Report48_SM_Qt
    public class Report48_SM_Qt
    {
        public string D_TheSystemKey { get; set; }
        //最终站点
        public string Target { get; set; }
        //minutes
        public double Duration { get; set; }
        //起始站点
        public string Oper_NO { get; set; }
        
        public string QT_Category { get; set; }

    }
}
