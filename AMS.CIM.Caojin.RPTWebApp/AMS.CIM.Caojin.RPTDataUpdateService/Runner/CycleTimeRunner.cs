using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTDataUpdateService.Runner
{
    public class CycleTimeRunner
    {
        public static void RunCT()
        {
            try
            {
                RPTCycleTimeTranslator translator = new RPTCycleTimeTranslator();
            }
            catch (Exception e)
            {
                LogHelper.ErrorLog(e);
            }
        }

        public static void RunFlow()
        {
            try
            {
                RPTFlowSumTranslator translator = new RPTFlowSumTranslator();
            }
            catch (Exception e)
            {
                LogHelper.ErrorLog(e);
            }
        }
    }
}
