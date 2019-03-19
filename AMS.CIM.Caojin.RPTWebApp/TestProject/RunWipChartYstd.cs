using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Newtonsoft.Json;
using Caojin.Common;

namespace TestProject
{
    public class RunWipChartYstd
    {
        public static void Run()
        {

            string DirPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string Db2ConfigFileName = "\\App\\db2Config.json";
            string jsonObj = System.IO.File.ReadAllText(DirPath + Db2ConfigFileName);
            Db2ConnObj conn = JsonConvert.DeserializeObject<Db2ConnObj>(jsonObj);
            ReqRpt028Runner runner = new ReqRpt028Runner(conn);
        }
    }
}
