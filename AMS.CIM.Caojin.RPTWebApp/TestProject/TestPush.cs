using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace TestProject
{
    public class TestPush
    {
        public static void Test()
        {
            DB2DataPusher<RPT_RealTime_Lin> Pusher = new DB2DataPusher<RPT_RealTime_Lin>("ISTRPT.RPT_RealTime_Lin");
            Pusher.entities.EntityList.Add(new RPT_RealTime_Lin() { Start_Time = DateTime.Now, Product_ID = "TestProd", PartName = "FAB" });
            Pusher.entities.EntityList.Add(new RPT_RealTime_Lin() { Start_Time = DateTime.Now.AddDays(-1), Product_ID = "TestProd", PartName = "FAB", MoveQty = 12 });
            Pusher.entities.EntityList.Add(new RPT_RealTime_Lin() { Start_Time = DateTime.Now.AddDays(-1), MoveQty = 25, Product_ID = "TestProd", PartName = "WAT" });
            Pusher.PushEntities();
            // ReqRpt025Translator translator = new ReqRpt025Translator();
        }
    }
}
