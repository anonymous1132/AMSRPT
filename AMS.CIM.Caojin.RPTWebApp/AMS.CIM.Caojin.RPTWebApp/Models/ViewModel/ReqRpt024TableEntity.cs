using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt024TableEntity
    {
        public string Department { get; set; }

        public int Prod_Pcs { get; set; } = 0;

        public int Prod_Lot { get; set; } = 0;

        public int Sem_Pcs { get; set; } = 0;

        public int Sem_Lot { get; set; } = 0;

        public int Sl_Pcs { get; set; } = 0;

        public int Sl_Lot { get; set; } = 0;

        public int Npw_Pcs { get; set; } = 0;

        public int Npw_Lot { get; set; } = 0;

        public string strProd_Lot { get { return Prod_Lot.ToString(); } }

        public string strProd_Pcs { get { return Prod_Pcs.ToString(); } }

        public string strSem_Lot { get { return Sem_Lot.ToString(); } }

        public string strSem_Pcs { get { return Sem_Pcs.ToString(); } }

        public string strSl_Lot { get { return Sl_Lot.ToString(); } }

        public string strSl_Pcs { get { return Sl_Pcs.ToString(); } }

        public string strNpw_Lot { get { return Npw_Lot.ToString(); } }

        public string strNpw_Pcs { get { return Npw_Pcs.ToString(); } }

        public int Total_Prod_Lot { get { return Prod_Lot + Sem_Lot; } }

        public string strTotal_Prod_Lot { get { return Total_Prod_Lot.ToString(); } }

        public int Total_Prod_Pcs { get { return Prod_Pcs + Sem_Pcs; } }

        public string strTotal_Prod_Pcs { get { return Total_Prod_Pcs.ToString(); } }

        public int Total_Npw_Lot { get { return Npw_Lot + Sl_Lot; } }

        public string strTotal_Npw_Lot { get { return Total_Npw_Lot.ToString(); } }

        public int Total_Npw_Pcs { get { return Npw_Pcs + Sl_Pcs; } }

        public string strTotal_Npw_Pcs { get { return Total_Npw_Pcs.ToString(); } }

    }
}