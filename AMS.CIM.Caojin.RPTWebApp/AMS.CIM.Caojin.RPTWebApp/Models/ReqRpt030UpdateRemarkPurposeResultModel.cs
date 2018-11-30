using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt030UpdateRemarkPurposeResultModel
    {
        public ReqRpt030UpdateRemarkPurposeResultModel(ReqRpt030UpdateRemarkPurposePostModel postModel)
        {
            LotID = postModel.LotID;
            Remark = postModel.Remark;
            Purpose = postModel.Purpose;
            Initialize();
        }

        private string LotID { get; set; }

        private string Remark { get; set; }

        private string Purpose { get; set; }

        public int Message { get; set; }

        private void Initialize()
        {
            List<string> list_lot = Regex.Split(LotID, "%#", RegexOptions.IgnoreCase).ToList();
            List<string> list_remark = Regex.Split(Remark, "%#", RegexOptions.IgnoreCase).ToList();
            List<string>list_purpose= Regex.Split(Purpose, "%#", RegexOptions.IgnoreCase).ToList();
            if (list_lot.Count <= 0)
            {
                new Exception("没有要更新的Lot");
            }
            List<string> sqlList = new List<string>();
            for (int i = 0; i < list_lot.Count; i++)
            {
                string remark = string.IsNullOrEmpty(list_remark[i])?"":list_remark[i];
                string purpose = string.IsNullOrEmpty(list_purpose[i]) ? "" : list_purpose[i];
                sqlList.Add(string.Format("update istrpt.rpt_wip_special_lot set remark='{0}',purpose='{1}' where lot_id='{2}'", remark, purpose, list_lot[i]));
            }
            DB2Helper db2 = new DB2Helper();
            Message= db2.UpdateBatchCommand(sqlList);
        }
    }
}