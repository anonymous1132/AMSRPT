using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018DataBase
    {
        public string Date
        {
            get;
            set;
        }
        public double UPm
        {
            get;
            set;
        }

        public double UUm
        {
            get;
            set;
        }

        public double SD
        {
            get;
            set;
        }

        public double UD
        {
            get;
            set;
        }

        public string strUPm
        {
            get { return (UPm * 100).ToString("0.00") + "%"; }
        }

        public string strUUm
        {
            get { return (UUm * 100).ToString("0.00") + "%"; }
        }

        public string strSD
        {
            get { return (SD * 100).ToString("0.00") + "%"; }
        }

        public string strUD
        {
            get { return (UD * 100).ToString("0.00") + "%"; }
        }
    }
}