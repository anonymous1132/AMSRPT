using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;
using System.Data;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt023TableViewModel
    {
        public ReqRpt023TableViewModel(ReqRpt023PostViewModel postViewModel)
        {
            RecivePostModel = postViewModel;
            Initialize();
        }

        private ReqRpt023PostViewModel RecivePostModel;

        public Dictionary<string, string> Reason_Desc { get; set; } = new Dictionary<string, string>();

        private DB2DataCatcher<ScrapSummarizedModel> dB2Data = new DB2DataCatcher<ScrapSummarizedModel>("REPORT23_SCRAPPED_QTY");

        public List<ReqRpt023TableEntity> Entities { get; set; } = new List<ReqRpt023TableEntity>();

        public ReqRpt023TableEntity TotalEntity = new ReqRpt023TableEntity() { Department = "Total" };

        private Dictionary<string, List<ScrapSummarizedModel>> rawEntityList = new Dictionary<string, List<ScrapSummarizedModel>>();

        public Dictionary<string, List<ScrapSummarizedModel>> GetRawEntities()
        {
            return rawEntityList;
        }

        //页面上显示的Department列
        public List<string> ItemDepartments { get { return Entities.Select(s => s.Department).ToList(); } }

        //选中的LotTypes
        public List<string> LotTypes { get { return RecivePostModel.LotTypes.Split(',').ToList(); } }

        //选中的Production
        public string Production { get { return RecivePostModel.Product; } }

        //选中的From
        private DateTime From { get { return DateTime.ParseExact(RecivePostModel.FromDateTime, "yyyy-MM-ddTHH:mm", System.Globalization.CultureInfo.CurrentCulture); } }
        public string strFrom {get { return From.ToString("yyyy-MM-dd HH:mm:ss"); } }
        //选中的To
        private DateTime To { get { return DateTime.ParseExact(RecivePostModel.ToDateTime, "yyyy-MM-ddTHH:mm", System.Globalization.CultureInfo.CurrentCulture); } }
        public string strTo { get { return To.ToString("yyyy-MM-dd HH:mm:ss"); } }
        //获取格式化好的Reasons_Pcs
        private Dictionary<string, int> GetNewReasons_Pcs()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (string key in Reason_Desc.Keys)
            {
                dic.Add(key,0);
            }
            return dic;
        }

        private void Initialize()
        {
            if (string.IsNullOrEmpty(RecivePostModel.LotTypes)) throw new Exception("没有选择LotType");
            if (string.IsNullOrEmpty(RecivePostModel.Departments)) throw new Exception("没有选择Department");
            if (From > To) throw new Exception("时间范围错误");
            DB2Helper db2 = new DB2Helper();
            string sql = "select code_id, code_id || '(' || description || ')' as reason from mmview.frcode where category_id ='WaferScrap'";
            db2.GetSomeData(sql);
            foreach (DataRow dr in db2.dt.Rows)
            {
                Reason_Desc.Add(dr["code_id"].ToString(), dr["reason"].ToString());
            }

            var list = RecivePostModel.Departments.Split(',').ToList();
            ReqRpt023TableEntity BadWaferEntity = new ReqRpt023TableEntity() { Department = "BadWafer" ,Reasons_Pcs=GetNewReasons_Pcs()};
            if (list.Where(w => w.Substring(0, 2) == "WC").Count() > 0) { Entities.Add(BadWaferEntity); }
            dB2Data.Conditions =string.Format("where scrap_time between '{3}' and '{4}' and  bank_id in('{0}') and lot_type in ('{1}') and prodspec_id like '%{2}%'",string.Join("','",list),string.Join("','",LotTypes),Production,strFrom,strTo) ;
            var db2Entity = dB2Data.GetEntities();
            foreach (string item in list)
            {
                //BadWafer处理
                if (item.Substring(0, 2) == "WC")
                {
                    var Badfilter = db2Entity.EntityList.Where(w => w.Bank_ID == item);
                    foreach (string str in Reason_Desc.Keys)
                    {
                        BadWaferEntity.Reasons_Pcs[str] += Badfilter.Where(w=>w.Reason_Code==str).Sum(s=>s.Qty);
                    }
                    continue;
                }
                //非badwafer
                ReqRpt023TableEntity entity = new ReqRpt023TableEntity()
                {
                    Department = ReqRpt024MainViewModel.Departments.Where(w => w.Value == item).FirstOrDefault().Key,
                    Reasons_Pcs = GetNewReasons_Pcs()
                };
                var filter = db2Entity.EntityList.Where(w => w.Bank_ID == item);
                foreach (string str in Reason_Desc.Keys)
                {
                    entity.Reasons_Pcs[str] = filter.Where(w => w.Reason_Code == str).Sum(s => s.Qty);
                }
                Entities.Add(entity);
            }

            TotalEntity.Reasons_Pcs = GetNewReasons_Pcs();
            foreach (string str in Reason_Desc.Keys)
            {
                TotalEntity.Reasons_Pcs[str] = Entities.Sum(s => s.Reasons_Pcs[str]);
            }

        }


    }
}