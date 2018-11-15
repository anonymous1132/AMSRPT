using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt024LotDetailTableViewModel
    {
        public ReqRpt024LotDetailTableViewModel(ReqRpt024LotDetailQueryPostViewModel postViewModel)
        {
            TableViewModel =new ReqRpt024TableViewModel(postViewModel.SelectedDepartment);
            Department = postViewModel.Department;
            Category = postViewModel.Category;
            GetList();
        }

        private ReqRpt024TableViewModel TableViewModel { get; set; }

        private DB2DataCatcher<FHLot_DetailModel> dB2Data = new DB2DataCatcher<FHLot_DetailModel>("REPORT24_LOT_Detail");

        private string Department { get; set; }

        private string Category { get; set; }

        private string LotIDFilter { get; set; } = "";

        public List<FHLot_DetailModel> entities { get; set; } = new List<FHLot_DetailModel>();

        private void GetList()
        {
            GetFiterByDepartment();
            if (string.IsNullOrEmpty(LotIDFilter)) { throw new Exception("没有找到lot"); }
            dB2Data.Conditions = string.Format("where Lot_ID in ({0})",LotIDFilter);
            var db2 = dB2Data.GetEntities();
            entities = db2.EntityList.OrderBy(o=>o.Lot_ID).ThenBy(o=>o.Claim_Time).ToList();
        }

        private void GetFiterByDepartment()
        {
            List<FRLot_ScrapModel> filter = new List<FRLot_ScrapModel>();

            if (Department == "BadWafer")
            {
                TableViewModel.GetRawEntities().Where(w => w.Key.Substring(0, 2) == "WC").Select(s=>s.Value).ToList().ForEach(f=>filter= filter.Union(f).ToList());
            }
            else if (Department == "Total")
            {
                TableViewModel.GetRawEntities().Select(s => s.Value).ToList().ForEach(f=>filter=filter.Union(f).ToList());
            }
            else
            {
                TableViewModel.GetRawEntities().Where(w=>w.Key==Department).Select(s =>s.Value).ToList().ForEach(f =>filter= filter.Union(f).ToList());
            }

            if (Category == "Prod")
            {
                LotIDFilter = string.Join(",", filter.Where(w => w.Lot_Type == "Production").Select(s => string.Format("'{0}'",s.Lot_ID)));
            }
            if (Category == "SL")
            {
                LotIDFilter = string.Join(",",filter.Where(w=>w.Sub_Lot_Type== "Fab1 Shoot Loop").Select(s=> string.Format("'{0}'", s.Lot_ID)));
            }
            if (Category == "PS")
            {
                //sem的规则还没有定，所以先等于producion
                LotIDFilter = string.Join(",", filter.Where(w => w.Lot_Type == "Production").Select(s => string.Format("'{0}'", s.Lot_ID)));
            }
            if (Category == "NPW")
            {
                LotIDFilter = string.Join(",", filter.Where(w => w.Sub_Lot_Type != "Fab1 Shoot Loop" && w.Lot_Type != "Production").Select(s=> string.Format("'{0}'", s.Lot_ID)));
            }
            if (Category == "SEM")
            {

            }
            if (Category == "SN")
            {
                LotIDFilter = string.Join(",", filter.Where(w => w.Lot_Type != "Production").Select(s => string.Format("'{0}'", s.Lot_ID)));
            }

        }
    }
}