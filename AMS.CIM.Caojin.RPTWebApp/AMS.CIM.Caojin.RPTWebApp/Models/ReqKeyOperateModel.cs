using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqKeyOperateModel
    {
        public bool CheckKey(string project,string key)
        {
            string res = "";
            string sql =string.Format( "select pass from istrpt.rpt_project_key where projectname ='{0}'",project);
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData(sql);
            if (db2.dt.Rows.Count <= 0) return false;
             res = db2.dt.Rows[0][0].ToString();
            if (res == MD5jiami.MD5Encrypt(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateKey(string project,string newKey)
        {
            string md5Key = MD5jiami.MD5Encrypt(newKey);
            string sql =string.Format( "update  istrpt.rpt_project_key set pass='{0}' where projectname='{1}'",md5Key,project);
            try
            {
                DB2Helper db2 = new DB2Helper();
                db2.GetSomeData(sql);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}