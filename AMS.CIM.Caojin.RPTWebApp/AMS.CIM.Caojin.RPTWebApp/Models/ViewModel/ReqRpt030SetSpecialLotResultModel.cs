using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using System.Data;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt030SetSpecialLotResultModel
    {
        public ReqRpt030SetSpecialLotResultModel(ReqRpt030SetSpecialLotPostModel postModel)
        {
            LotID = postModel.LotID.Split('\n').Select(s=>s.Trim()).Where(w=> (!string.IsNullOrEmpty(w))&&w.Substring(0,1)!="#").Distinct().ToList();
            Mode = postModel.Mode;
            ImportToDB2();
        }

        private List<string> LotID { get; set; }

        private string Mode { get; set; }

        public bool AllIn { get { return ErrorID.Count+RepeatID.Count == 0 ? true : false; } } 

        public List<string> ErrorID { get; set; } = new List<string>();

        public List<string> RepeatID { get; set; } = new List<string>();

        private string FormatLotIDCondition { get { if (LotID.Count == 0) throw new Exception("LotID没有元素"); return string.Join("','",LotID); } }

        private string FormatLotIDCondition2 { get { if (LotID.Count == 0) throw new Exception("LotID没有元素"); return string.Join("'),('", LotID); } }


        public void ImportToDB2()
        {
            List<string> FRLots = new List<string>();
            DB2Helper db2 = new DB2Helper();
            string sql =string.Format("select lot_id from mmview.frlot where lot_id in ('{0}')",FormatLotIDCondition) ;
            db2.GetSomeData(sql);
            foreach (DataRow dr in db2.dt.Rows)
            {
                FRLots.Add(dr[0].ToString());
            }
            foreach (string lot in LotID)
            {
                if (!FRLots.Contains(lot))
                {
                    ErrorID.Add(lot);
                }
            }
            LotID = FRLots;
            if (Mode == "OverRide")
            {
                sql = string.Format("insert into ISTRPT.RPT_WIP_SPECIAL_LOT (LOT_ID) values ('{0}')", FormatLotIDCondition2);
                sql = string.Format("delete from ISTRPT.RPT_WIP_SPECIAL_LOT;{0}", sql);
                db2.GetSomeData(sql);
            }
            else if (Mode == "AddMore")
            {
                foreach (string lot in LotID)
                {
                    try
                    {
                        sql = string.Format("insert into ISTRPT.RPT_WIP_SPECIAL_LOT (LOT_ID) values ('{0}')", lot);
                        db2.GetSomeData(sql);
                    }
                    catch (Exception)
                    {
                        RepeatID.Add(lot);
                    }
                }
            }
        }
    }
}