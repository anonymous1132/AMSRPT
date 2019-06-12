using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRptCommonDeptQuerier
    {
        public ReqRptCommonDeptQuerier()
        {
            var list = codeCatcher.GetEntities().EntityList;
            DeptList = list.Any() ? list.ToList() : new List<FRCodeModel>();
        }

        DB2DataCatcher<FRCodeModel> codeCatcher { get; set; } = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE") { Conditions="where category_id='Department'"};

        public List<FRCodeModel> DeptList { get; set; } 
    }
}