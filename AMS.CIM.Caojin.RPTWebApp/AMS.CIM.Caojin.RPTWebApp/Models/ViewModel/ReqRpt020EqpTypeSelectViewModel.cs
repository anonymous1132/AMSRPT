using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary;


namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt020EqpTypeSelectViewModel
    {
        public ReqRpt020EqpTypeSelectViewModel(string Modules)
        {
            if (string.IsNullOrEmpty(Modules))
            {
                EqpTypes = db.EQPType_Department_Mapping.Select(s => s.EqpType).Distinct().ToList();
            }
            else
            {
                var module_list = Modules.Split(',');
                EqpTypes = db.EQPType_Department_Mapping.Where(s => module_list.Contains(s.Department)).Select(s => s.EqpType).Distinct().ToList();
            }
   
            EqpTypes.Sort();
        }
        public RPTContext db = new RPTContext();

        public List<string> EqpTypes { get; set; } = new List<string>();

    }
}