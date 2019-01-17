using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt030DeleteSpecialLotResultModel
    {
        public ReqRpt030DeleteSpecialLotResultModel(string LotID)
        {
            LotID = LotID.Replace("'","");
            LotID = LotID.Replace("\"", "");
            List<string> list = LotID.Split(',').ToList();
            string sql = string.Format("delete from ISTRPT.RPT_WIP_SPECIAL_LOT where LOT_ID in ('{0}')",string.Join("','",list));
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData(sql);
        }


    }
}