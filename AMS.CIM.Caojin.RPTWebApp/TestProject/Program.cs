using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtilsLibrary.Utils;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            LogUtils.InfoLog("Start To Run WipChartYSTD");
            try
            {

                RunWipChartYstd.Run();
                LogUtils.InfoLog("Success!");
            }
            catch (Exception ex)
            {
                LogUtils.ErrorLog(ex);
                System.Threading.Thread.Sleep(5*1000);
            }
        }


    }
}
