using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Newtonsoft.Json;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    public abstract class DBUpdater
    {
        protected string DirPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        protected abstract string LastTimeKeepFileName { get; set; }
        protected DateTime StartTime { get; set; }
        protected string sqlStartTime { get { return StartTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff"); } }
        protected DateTime EndTime { get; set; }
        protected string sqlEndTime { get { return EndTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff"); } }
        protected void RewriteLastUpdateTimeToFile()
        {
            string json = JsonConvert.SerializeObject(new { LastRunTime = EndTime.ToString("yyyy-MM-dd HH.mm.ss.ffffff") });
            System.IO.File.WriteAllText(DirPath + LastTimeKeepFileName, json);
        }
        protected DateTime GetLastUpdateTimeFromFile()
        {
            DateTime dt = DateTime.MinValue;
            string json = System.IO.File.ReadAllText(DirPath + LastTimeKeepFileName);
            DateTime.TryParseExact(JsonConvert.DeserializeObject<CycleTimeRunningConfig>(json).LastRunTime, "yyyy-MM-dd HH.mm.ss.ffffff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
            return dt;
        }
        protected DB2Oper Db2 { get; set; }
    }
}
