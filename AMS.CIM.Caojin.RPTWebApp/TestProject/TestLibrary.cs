using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace TestProject
{
    public class TestLibrary
    {
        public static void Test()
        {
            ShareDataEntity.GetSingleEntity();
            ShareDataEntity.GetSingleEntity().Rpt018 = new ReqRpt018ViewModel();
            ShareDataEntity.GetSingleEntity().Rpt018.GetData();
            Console.WriteLine("OK");
        }
    }
}
