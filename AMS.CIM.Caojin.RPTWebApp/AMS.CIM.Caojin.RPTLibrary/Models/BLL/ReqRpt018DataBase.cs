using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTLibrary.Models
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

        public double PRDHour { get; set; }

        public double SBYHour { get; set; }

        public double ENGHour { get; set; }

        public double SDTHour { get; set; }

        public double UDTHour { get; set; }

        public double NSTHour { get; set; }

        public double PRDTestHour { get; set; }

        public double PMHour { get; set; }

        public double TotalHour { get { return PRDHour + SBYHour + ENGHour + SDTHour + UDTHour + NSTHour; } }

        public double UPHour { get { return PRDHour + SBYHour + ENGHour; } }

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

        public string strPRDHour
        {
            get { return PRDHour.ToString("0.00"); }
        }

        public string strSBYHour
        {
            get { return SBYHour.ToString("0.00"); }
        }

        public string strENGHour
        {
            get { return ENGHour.ToString("0.00"); }
        }

        public string strSDTHour
        {
            get { return SDTHour.ToString("0.00"); }
        }

        public string strUDTHour
        {
            get { return UDTHour.ToString("0.00"); }
        }

        public string strNSTHour
        {
            get { return NSTHour.ToString("0.00"); }
        }

        public string strPRDTestHour
        {
            get { return PRDTestHour.ToString("0.00"); }
        }

        public string strPMHour
        {
            get { return PMHour.ToString("0.00"); }
        }

        public string strTotalHour
        {
            get { return TotalHour.ToString("0.00"); }
        }

        public string strUPHour
        {
            get { return UPHour.ToString("0.00"); }
        }
    }
}