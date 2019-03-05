using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048EqpEntity
    {
        public string EqpID { get; set; }

        public string E10Status { get; set; }

        public string StateID { get; set; }

        public string PMS { get; set; }

        public DateTime EqpStateChgTime { get; set; }

        public string strEqpTime { get { return EqpStateChgTime.ToString("yyyy/MM/dd HH:mm:ss"); } }

    }
}