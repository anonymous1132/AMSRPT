using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTWebApp.Models;

namespace TestProject
{
    public class TestDb2
    {
        public static void Test()
        {
            ShareDataEntity.GetSingleEntity();
            ReqRpt018GroupModel groupModel = new ReqRpt018GroupModel();
            groupModel.GetData();
            Console.WriteLine(groupModel.ReqRpt018Models.Count);
        }
    }
}
