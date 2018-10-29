using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace TestProject
{
    public class TestEF
    {
        public static void Test()
        {
            using (var db = new RPTContext())
            {
                TimeSplitConfigEntity configEntity = new TimeSplitConfigEntity()
                {
                    RptID = "global",
                    TimeValue = "8:00:00"
                };
                db.TimeSplitConfig.Add(configEntity);
                db.SaveChanges();

                foreach (var item in db.TimeSplitConfig)
                {
                    Console.WriteLine("Name:" + item.RptID);
                    Console.WriteLine("Value:" + item.TimeValue);
                }
            }
        }

    }
}
