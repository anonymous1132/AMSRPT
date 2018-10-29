using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public class FREQPModel
    {

        public string EQP_Name
        {
            get;
            set;
        }

        public string Owner_ID
        {
            get;
            set;
        }


        public string EQP_ID
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string E10_State
        {
            get;
            set;
        }

        public string Cur_State_ID
        {
            get;
            set;
        }



        public string Eqp_Category
        {
            get;
            set;
        }

        public string Eqp_Type
        {
            get;
            set;
        }


        public DateTime Claim_Time
        {
            get;
            set;
        }

        public DateTime State_History_Time
        {
            get;
            set;
        }

    }
}