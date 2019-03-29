using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;
using Newtonsoft.Json;
using System.IO;


namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public abstract class DB2OperateBase
    {
        public DB2OperateBase()
        {
           
            var content = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/db2Config.json"));
            var conn = JsonConvert.DeserializeObject<Db2ConnObj>(content);
            DB2 = new DB2Oper(conn);

            Initial();
        }
        public DB2Oper DB2 { get; set; }

        public abstract void Initial();
        
    }
}