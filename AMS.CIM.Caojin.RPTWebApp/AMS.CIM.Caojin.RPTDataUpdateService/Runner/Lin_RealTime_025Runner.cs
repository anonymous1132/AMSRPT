using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTDataUpdateService.Runner
{
    public class Lin_RealTime_025Runner
    {
        public static void Run()
        {
            try
            {
                ReqRpt025Translator reqRpt025Translator = new ReqRpt025Translator();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("符合条件的数据"))
                    LogHelper.InfoLog(ex.Message);
                else
                    LogHelper.ErrorLog(ex);
            }
        }
    }
}
