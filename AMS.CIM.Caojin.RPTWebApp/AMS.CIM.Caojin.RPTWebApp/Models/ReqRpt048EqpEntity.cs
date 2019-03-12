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

        public string PMS_Early_Time { get; set; } = "";

        public string PMS_Late_Time { get; set; } = "";

        public string PMS_Time { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime EqpStateChgTime { get; set; }

        public string strEqpTime { get { return EqpStateChgTime.ToString("yyyy/MM/dd HH:mm:ss"); } }

    }
}