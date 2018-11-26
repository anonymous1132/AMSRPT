using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt023LotDetailTableViewModel
    {
        public ReqRpt023LotDetailTableViewModel(ReqRpt023LotDetailQueryPostModel postModel)
        {
            var list=postModel.SelectedDepartment.Split(',').ToList();
            var departList = ReqRpt024MainViewModel.Departments.Where(w => list.Contains(w.Key)).Select(s => s.Value);
            ReqRpt023PostViewModel mainPost = new ReqRpt023PostViewModel()
            {
                Departments =string.Join(",",departList),
                LotTypes=postModel.SelectedLotType,
                Product=postModel.SelectedProduct,
                FromDateTime=postModel.SelectedFrom,
                ToDateTime=postModel.SelectedTo
            };
            TableViewModel = new ReqRpt023TableViewModel(mainPost);
            Department = postModel.Department;
            Reason = postModel.Reason;
            Initialize();
        }

        private string Department { get; set; }

        public string Reason { get; set; }

        private ReqRpt023TableViewModel TableViewModel;

        private DB2DataCatcher<FHLot_DetailModel> dB2Data = new DB2DataCatcher<FHLot_DetailModel>("REPORT24_LOT_Detail");

        public List<FHLot_DetailModel> entities { get; set; } = new List<FHLot_DetailModel>();

        private string LotIDFilter { get; set; } = "";

        private void GetFilter()
        {
            List<ScrapSummarizedModel> filter = new List<ScrapSummarizedModel>();
            if (Department == "BadWafer")
            {
                TableViewModel.GetRawEntities().Where(w => w.Key.Substring(0, 2) == "WC").Select(s => s.Value).ToList().ForEach(f => filter = filter.Union(f).ToList());
            }
            else if (Department == "Total")
            {
                TableViewModel.GetRawEntities().Select(s => s.Value).ToList().ForEach(f => filter = filter.Union(f).ToList());
            }
            else
            {
                TableViewModel.GetRawEntities().Where(w => w.Key == Department).Select(s => s.Value).ToList().ForEach(f => filter = filter.Union(f).ToList());
            }

            if (Reason == "Total")
            {
                LotIDFilter = string.Join(",", filter.Select(s => string.Format("'{0}'", s.Lot_ID)));
            }
            else
            {
                LotIDFilter = string.Join(",", filter.Where(w => w.Reason_Code == Reason).Select(s => string.Format("'{0}'", s.Lot_ID)));
            }
        }

        private void Initialize()
        {
            GetFilter();
            if (string.IsNullOrEmpty(LotIDFilter)) { throw new Exception("没有找到lot"); }
            dB2Data.Conditions = string.Format("where Lot_ID in ({0})", LotIDFilter);
            var db2 = dB2Data.GetEntities();
            //改为只取最后一笔
            //entities = db2.EntityList.OrderBy(o => o.Lot_ID).ThenBy(t=>t.Claim_Time).ToList();
            //新修改代码如下
            var group = db2.EntityList.GroupBy(g => g.Lot_ID).Select(s => new { Lot_ID = s.Key, Claim_Time = s.Max(item => item.Claim_Time) });
            foreach (var item in group)
            {
                FHLot_DetailModel model = db2.EntityList.Where(w => w.Lot_ID == item.Lot_ID && w.Claim_Time == item.Claim_Time).LastOrDefault();
                entities.Add(model);
            }
        }
    }
}