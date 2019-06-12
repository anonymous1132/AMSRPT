using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt0063BrprivilegeDataBuilder
    {
        public ReqRpt0063BrprivilegeDataBuilder()
        {
            Initialize();
        }

        const string sql = @"select rpt.privilegeid,rpt.usage_counter,br.privilegecategory,br.privilegename from istrpt.rptfuncusage rpt left join smview.BRPRIVILEGE br
on br.subsystemname='RPT'
and rpt.privilegeid=br.privilegeid where rpt.privilegeid like 'RPT%'";

        public List<BrprivilegeClickCount> BrprivilegeList { get; set; }
        DB2DataCatcher<BrprivilegeClickCount> BrCatcher { get; set; }
        private void Initialize()
        {
            BrCatcher = new DB2DataCatcher<BrprivilegeClickCount>("",sql);
            var list = BrCatcher.GetEntities().EntityList;
            BrprivilegeList = list.Any() ? list.ToList() : new List<BrprivilegeClickCount>();
        }


    }
}