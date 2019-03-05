using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048LotQuotaMappingHandler
    {
        public ReqRpt048LotQuotaMappingHandler(string lot,string project)
        {
            LotID = lot;
            Project = project;
            Initialize();
        }

        private string LotID { get; set; }

        private string Project { get; set; }

        public bool Success { get; set; } = false;

       // public string Msg { get; set; }

        //DB2DataCatcher<Rpt_Lot_Quota_Mapping> map { get; set; } = new DB2DataCatcher<Rpt_Lot_Quota_Mapping>("ISTRPT.Rpt_Lot_Quota_Mapping");

        private void Initialize()
        {
            if (string.IsNullOrEmpty(LotID))
            {
                throw new Exception("LotID是空字符");
            }
            string sql = "";
            var db2 = new DB2Helper();
            if (string.IsNullOrEmpty(Project))
            {
                sql = string.Format("delete from ISTRPT.Rpt_Lot_Quota_Mapping where lot_id='{0}';", LotID);
                db2.GetSomeData(sql);
            }
            else
            {
                sql = string.Format("delete from ISTRPT.Rpt_Lot_Quota_Mapping where lot_id='{0}' and project_desc='{1}'", LotID, Project);
                db2.GetSomeData(sql);
                sql = string.Format("insert into ISTRPT.Rpt_Lot_Quota_Mapping (lot_id,quota_type,project_desc) values ('{0}',1,'{1}')", LotID, Project);
                db2.GetSomeData(sql);
            }
            Success = true;
        }
    }
}