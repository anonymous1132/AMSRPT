using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt024TableViewModel
    {
        public ReqRpt024TableViewModel(ReqRpt024PostViewModel postViewModel)
        {
            departments = postViewModel.selectedDepartment;
            GetList();
        }

        public ReqRpt024TableViewModel(string selectedDepartmentName)
        {
            var list = selectedDepartmentName.Split(',').ToList();
           var departList=  ReqRpt024MainViewModel.Departments.Where(w => list.Contains(w.Key)).Select(s => s.Value);
            departments = string.Join(",",departList);
            GetList();
        }

        private string departments { get; set; }

        private DB2DataCatcher<FRLot_ScrapModel> dB2Data = new DB2DataCatcher<FRLot_ScrapModel>("ISTRPT.REPORT24_LOT_WAFER_QTY");
        /// <summary>
        /// 视图模型
        /// </summary>
        public List<ReqRpt024TableEntity> entities = new List<ReqRpt024TableEntity>();

        public ReqRpt024TableEntity TotalEntity = new ReqRpt024TableEntity() { Department = "Total" };

        private Dictionary<string , List<FRLot_ScrapModel>> rawEntityList=new Dictionary<string, List<FRLot_ScrapModel>>();

        public Dictionary<string, List<FRLot_ScrapModel>> GetRawEntities()
        {
            return rawEntityList;
        }

        //页面上显示的Department列
        public List<string> ItemDepartments { get { return entities.Select(s => s.Department).ToList(); } } 

        private void GetList()
        {
            if (string.IsNullOrEmpty(departments)) throw new Exception("没有选择Department");
            var list = departments.Split(',').ToList();
            ReqRpt024TableEntity BadWaferEntity = new ReqRpt024TableEntity() { Department="BadWafer"};
            if (list.Where(w => w.Substring(0, 2) == "WC").Count() > 0) { entities.Add(BadWaferEntity); }
            dB2Data.Conditions = "where lot_finished_state !='SCRAPPED'";
            var db2= dB2Data.GetEntities();
            foreach (string item in list)
            {
                if (item.Substring(0, 2) == "WC")
                {
                    var Badfilter = db2.EntityList.Where(w => w.Bank_ID == item);
                    rawEntityList.Add(item,Badfilter.ToList());
                    int prod_lot=Badfilter.Where(w => w.Lot_Type == "Production").Count();
                    int prod_pcs=prod_lot==0?0:Badfilter.Where(w => w.Lot_Type == "Production").Sum(s => s.QTY);
                    int sl_lot=Badfilter.Where(w => w.Sub_Lot_Type == "Fab1 Shoot Loop").Count();
                    int sl_pcs=sl_lot==0?0:Badfilter.Where(w => w.Sub_Lot_Type == "Fab1 Shoot Loop").Sum(s => s.QTY);
                    int npw_lot=Badfilter.Where(w => w.Sub_Lot_Type != "Fab1 Shoot Loop" && w.Lot_Type != "Production").Count();
                    int npw_pcs=npw_lot==0?0:Badfilter.Where(w => w.Sub_Lot_Type != "Fab1 Shoot Loop" && w.Lot_Type != "Production").Sum(s => s.QTY);
                    int sem_lot = 0;
                    int sem_pcs = 0;
                    BadWaferEntity.Prod_Lot += prod_lot;
                    BadWaferEntity.Prod_Pcs += prod_pcs;
                    BadWaferEntity.Sem_Lot += sem_lot;
                    BadWaferEntity.Sem_Pcs += sem_pcs;
                    BadWaferEntity.Npw_Lot += npw_lot;
                    BadWaferEntity.Npw_Pcs += npw_pcs;
                    BadWaferEntity.Sl_Lot += sl_lot;
                    BadWaferEntity.Sl_Pcs += sl_pcs;
                    continue;
                }

                ReqRpt024TableEntity entity = new ReqRpt024TableEntity()
                {
                    Department = ReqRpt024MainViewModel.Departments.Where(w => w.Value == item).FirstOrDefault().Key
                };
                var filter = db2.EntityList.Where(w => w.Bank_ID == item);
                rawEntityList.Add(item,filter.ToList());
                entity.Prod_Lot = filter.Where(w => w.Lot_Type == "Production").Count();
                entity.Prod_Pcs =entity.Prod_Lot==0?0: filter.Where(w => w.Lot_Type == "Production").Sum(s => s.QTY);
                entity.Sl_Lot = filter.Where(w => w.Sub_Lot_Type== "Fab1 Shoot Loop").Count();
                entity.Sl_Pcs =entity.Sl_Lot==0?0: filter.Where(w => w.Sub_Lot_Type == "Fab1 Shoot Loop").Sum(s=>s.QTY);
                entity.Npw_Lot = filter.Where(w => w.Sub_Lot_Type != "Fab1 Shoot Loop" && w.Lot_Type != "Production").Count();
                entity.Npw_Pcs =entity.Npw_Lot==0?0: filter.Where(w => w.Sub_Lot_Type != "Fab1 Shoot Loop" && w.Lot_Type != "Production").Sum(s => s.QTY);
                //sem定义还没有，待修改
                entity.Sem_Lot = 0;
                entity.Sem_Pcs = 0;
                entities.Add(entity);
            }

            TotalEntity.Prod_Lot = entities.Sum(s => s.Prod_Lot);
            TotalEntity.Prod_Pcs = entities.Sum(s=>s.Prod_Pcs);
            TotalEntity.Sem_Lot = entities.Sum(s => s.Sem_Lot);
            TotalEntity.Sem_Pcs = entities.Sum(s=>s.Sem_Pcs);
            TotalEntity.Sl_Lot = entities.Sum(s=>s.Sl_Lot);
            TotalEntity.Sl_Pcs = entities.Sum(s=>s.Sl_Pcs);
            TotalEntity.Npw_Lot = entities.Sum(s=>s.Npw_Lot);
            TotalEntity.Npw_Pcs = entities.Sum(s=>s.Npw_Pcs);
        }
    }
}