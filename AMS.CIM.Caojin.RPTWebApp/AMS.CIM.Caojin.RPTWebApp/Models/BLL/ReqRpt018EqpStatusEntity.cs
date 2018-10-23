using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018EqpStatusEntity
    {
        public string EQP_ID
        {
            get;
            set;
        }

        public string EQP_Type
        {
            get;
            set;
        }

        public string E10_State
        {
            get;
            set;
        }

        public string Eqp_State
        {
            get;
            set;
        }

        public DateTime Start_Time
        {
            get;
            set;
        }

        public DateTime End_Time
        {
            get;
            set;
        }

        public string Owner_ID
        {
            get;
            set;
        }

    }
}