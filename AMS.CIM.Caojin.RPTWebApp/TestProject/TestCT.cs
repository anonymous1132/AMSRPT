using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace TestProject
{
   public static  class TestCT
    {
        public static void RunCT()
        {

            
            RPTCycleTimeTranslator translator = new RPTCycleTimeTranslator();

        }

        public static void RunFlow()
        {

            RPTFlowSumTranslator translator = new RPTFlowSumTranslator();

        }
    }
}
