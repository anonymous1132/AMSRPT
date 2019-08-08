using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt064SpcMainTableEntity
    {
        public int Gno { get; set; }

        public int Cno { get; set; }

        public int Ctype { get; set; }

        public string Department { get; set; }

        public string EqpID { get; set; }

        //public string Prod { get; set; }

        public string Gname { get; set; }
        
        public string ChartTitle{ get; set; }

        public string ChartType { get; set; }

        public string Usl { get; set; }

        public string Ucl { get; set; }

        public string Target { get; set; }

        public string Lsl { get; set; }

        public string Lcl { get; set; }

        public string Mean { get; set; }

        public string Sigma { get; set; }

        public string Ca { get; set; }

        public string Cp { get; set; }

        public string Cpk { get; set; }

        public  void SetChartType(int type)
        {
            Ctype = type;
            switch (type)
            {
                case 1:
                    ChartType = "Mean";
                    break;
                case 2:
                    ChartType = "Range";
                    break;
                case 3:
                    ChartType = "Sigma";
                    break;
                case 4:
                    ChartType = "Uniformity";
                    break;
                case 10:
                    ChartType = "Single";
                    break;
                case 11:
                    ChartType = "Moving Range";
                    break;
                case 20:
                    ChartType = "Count";
                    break;
                default:
                    ChartType = "Undefined";
                    break;
            }
        }

    }
}