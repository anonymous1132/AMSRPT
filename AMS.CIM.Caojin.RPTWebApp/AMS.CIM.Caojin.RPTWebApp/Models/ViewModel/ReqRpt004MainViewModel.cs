using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt004MainViewModel
    {
        public ReqRpt004MainViewModel()
        {
            Initialize();
        }

        public List<string> ProductList { get; set; } = new List<string>();

        public void Initialize()
        {
            DB2DataCatcher<FBProd> ProdCatcher = new DB2DataCatcher<FBProd>("SMVIEW.FBPROD");
            ProdCatcher.Conditions = "where prodcat_ident='Production' and prod_id not like 'SL%'";
            ProductList = ProdCatcher.GetEntities().EntityList.Select(s=>s.Identifier).ToList();
        }
    }
}