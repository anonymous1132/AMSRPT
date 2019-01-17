using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt020MainViewModel
    {
        public ReqRpt020MainViewModel()
        {
            Initialize();
        }

        public Dictionary<string, string> Modules { get; set; } = new Dictionary<string, string>();

        public List<string> EqpTypes { get; set; } = new List<string>();

        public void Initialize()
        {
            DB2DataCatcher<FRCodeModel> ModuleCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE") { Conditions="where category_id='Department'"};
            var list = ModuleCatcher.GetEntities().EntityList;
            list.ToList().ForEach(f=>Modules.Add(f.Code_ID,f.Description));
            DB2DataCatcher<FREQP_EqpType_EqpID_Mapping> EqpTypeCatcher = new DB2DataCatcher<FREQP_EqpType_EqpID_Mapping>("MMVIEW.FREQP");
            EqpTypes= EqpTypeCatcher.GetEntities().EntityList.Select(s=>s.Eqp_Type).Where(w=>!string.IsNullOrEmpty(w)).Distinct().ToList();
            EqpTypes.Sort();
        }
    }
}