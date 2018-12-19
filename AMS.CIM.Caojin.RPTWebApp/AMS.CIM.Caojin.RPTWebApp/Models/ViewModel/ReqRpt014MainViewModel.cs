using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt014MainViewModel
    {
        public ReqRpt014MainViewModel() { Initialize(); }

        public List<string> ProductList { get; set; } = new List<string>();

        public Dictionary<string, string> HoldCodeKeyValue { get; set; } = new Dictionary<string, string>();

        public void Initialize()
        {
            DB2DataCatcher<FRCodeModel> CodeReasonCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE");
            CodeReasonCatcher.Conditions = "where category_id  like '%Hold'";
            var CodeReasonList = CodeReasonCatcher.GetEntities().EntityList.OrderBy(o=>o.Code_ID);
            foreach (var item in CodeReasonList)
            {
                if (!HoldCodeKeyValue.ContainsKey(item.Code_ID))
                {
                    HoldCodeKeyValue.Add(item.Code_ID, item.Description);
                }
            }
            DB2DataCatcher<FBProd> ProdCatcher = new DB2DataCatcher<FBProd>("SMVIEW.FBPROD")
            { Conditions = "where prodcat_ident='Production' and prod_id not like 'SL%'" };
            ProductList = ProdCatcher.GetEntities().EntityList.Select(s => s.Identifier).ToList();
        }
    }
}