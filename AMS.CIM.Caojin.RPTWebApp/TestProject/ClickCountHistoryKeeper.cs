using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;

namespace TestProject
{
    public class ClickCountHistoryKeeper
    {
        public static void Run()
        {
            string[] sql = { @"delete from istrpt.rpt_click_count_history where date=to_char(current date, 'yyyy-MM-dd')",
@"insert into istrpt.rpt_click_count_history  (
select privilegeid, usage_counter, to_char(current date, 'yyyy-MM-dd'), current timestamp from istrpt.rptfuncusage
)" };
            DB2Helper db2 = new DB2Helper();
            db2.UpdateBatchCommand(sql.ToList());

        }
    }
}
