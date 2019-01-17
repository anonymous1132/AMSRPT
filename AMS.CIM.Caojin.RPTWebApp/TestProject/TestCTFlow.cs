using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace TestProject
{
    public class TestCTFlow
    {
        public static void Test()
        {
            RPTFlowSumTranslator translator = new RPTFlowSumTranslator();
            Console.WriteLine("OK");
        }

        public static void TestCT()
        {
            RPTCycleTimeTranslator translator = new RPTCycleTimeTranslator();
            Console.WriteLine("OK");
        }
    }
}
