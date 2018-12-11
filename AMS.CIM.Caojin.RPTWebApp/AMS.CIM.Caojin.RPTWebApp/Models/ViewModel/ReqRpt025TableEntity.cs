using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt025TableEntity
    {
        public ReqRpt025TableEntity(string ProductID)
        {
            this.ProductID = ProductID;
        }
        public string ProductID { get; set; }

        public int FAB_PassQty { get; set; } = 0;

        public int FAB_ScrapQty { get; set; } = 0;

        public int WAT_PassQty { get; set; } = 0;

        public int WAT_ScrapQty { get; set; } = 0;

        public double FAB_Yield { get { return (FAB_PassQty + FAB_ScrapQty) == 0 ? 0 : FAB_PassQty / (FAB_PassQty + FAB_ScrapQty); } }

        public double WAT_Yield { get { return (WAT_PassQty + WAT_ScrapQty) == 0 ? 0 : WAT_PassQty / (WAT_PassQty + WAT_ScrapQty); } }

        public double Yield { get { return FAB_Yield * WAT_Yield; } }

        public string strFAB_Yield { get { return (FAB_Yield * 100).ToString("0.00") + "%"; } }

        public string strWAT_Yield { get { return (WAT_Yield * 100).ToString("0.00") + "%"; } }

        public string strYield { get { return (Yield * 100).ToString("0.00") + "%"; } }
    }
}