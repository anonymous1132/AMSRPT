using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace TestProject
{
    public class TestUUm
    {
        public static void Test()
        {
            ReqRpt018GroupModelTest model = new ReqRpt018GroupModelTest();
            model.TestGetData();
            Console.WriteLine("Date:{0};EQP:{1};TotalMins:{2}", model.Entity.Date,model.Entity.EqpID,model.Entity.PRDMin+model.Entity.SBYMin+model.Entity.SDTMin+model.Entity.UDTMin+model.Entity.NSTMin+model.Entity.ENGMin);

            Console.WriteLine("{0}-{1}-{2}-{3}", model.Entity.PRDMin, model.Entity.SBYMin, model.Entity.ENGMin, model.Entity.SDTMin);
        }
    }
}
