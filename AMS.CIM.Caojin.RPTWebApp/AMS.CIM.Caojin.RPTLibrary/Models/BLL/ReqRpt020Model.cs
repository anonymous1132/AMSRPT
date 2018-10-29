using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class ReqRpt020Model
    {
        public string EqpID { get; set; }

        public DateTime SomeDay { get; set; }

        public int PassQty { get; set; }

        public int ReworkQty { get; set; }

        public int EffMove { get; set; }
    }
}
