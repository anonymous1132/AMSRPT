using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary.Models;
namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt014TableViewModel
    {
        public ReqRpt014TableViewModel(ReqRpt014PostModel postModel)
        {
            StartTime= DateTime.ParseExact(postModel.StartDate, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            if (postModel.StartCategory == "day") StartTime = StartTime.Add(StartOfDay); else StartTime = StartTime.Add(EndOfDay);
            EndTime= DateTime.ParseExact(postModel.EndDate, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            if (postModel.EndCategory == "day") EndTime = EndTime.Add(StartOfDay); else EndTime = EndTime.Add(EndOfDay);
            ProductID = postModel.ProductID;
            Initialize();
        }

        private readonly TimeSpan StartOfDay = TimeSpan.FromHours(8);

        private readonly TimeSpan EndOfDay = TimeSpan.FromHours(20);

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string ProductID { get; set; }

        public List<ReqRpt014DepartmentTableModel> DepartmentModels { get; set; } = new List<ReqRpt014DepartmentTableModel>();

        public List<ReqRpt014ProductTableModel> ProductModels { get; set; } = new List<ReqRpt014ProductTableModel>();

        private DB2DataCatcher<FHOPEHS_HoldReason> Db2HoldReason { get; set; } = new DB2DataCatcher<FHOPEHS_HoldReason>("MMVIEW.FHOPEHS");

        private DB2DataCatcher<FRCodeModel> DepartmentCatcher { get; set; } = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE") { Conditions= "where category_id ='Department'" };

        private DB2DataCatcher<FRPD> PDCatcher { get; set; } = new DB2DataCatcher<FRPD>("MMVIEW.FRPD");

        private void Initialize()
        {

            List<string> Product_List = ProductID.Split(',').ToList();
            Db2HoldReason.Conditions = string.Format("where lot_type='Production' and prodspec_id in ('{2}')  and claim_time between '{0}' and '{1}'", StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"),string.Join("','",Product_List));
            var Models = Db2HoldReason.GetEntities().EntityList;
            //if (Models.Count == 0) return;
            //lotCount
            int lotCount = Models.Select(s => s.Lot_ID).Distinct().Count();
            //所有Hold数据
            var HoldModels = Models.Where(w => w.Hold_Type != "" && w.Ope_Category.Substring(w.Ope_Category.Length - 4) == "Hold");
            //5码的Hold数据
            var HoldModels_5 = HoldModels.Where(w => w.Hold_Reason_Code.Length == 5);
            //4码的hold数据
            var HoldModels_4 = HoldModels.Where(w => w.Hold_Reason_Code.Length == 4);
            List<FRPD> PD_List = new List<FRPD>();
            if (HoldModels_4.Count() > 0)
            {
                PDCatcher.Conditions = string.Format("where pd_id in ('{0}')", string.Join("','", HoldModels_4.Select(s => s.PD_ID)));
                 PD_List = PDCatcher.GetEntities().EntityList.ToList();
            }
            var Departments = DepartmentCatcher.GetEntities().EntityList;

            foreach (var department in Departments)
            {
                //对每个部门计算该部门对应的hold list
                var list5 = HoldModels_5.Where(w => w.Hold_Reason_Code.Substring(0, 1) == department.Code_ID);
                var list4 = HoldModels_4.Where(w => PD_List.Where(p => p.Department == department.Code_ID).DefaultIfEmpty().Select(s => s.PD_ID).Contains(w.PD_ID));
                var list = list4.Union(list5);
                List<ReqRpt014DepartmentTableEntity> l = new List<ReqRpt014DepartmentTableEntity>();
                if (list.Count()==0)  //如果没有元素，则不进行查找
                {
                    DepartmentModels.Add(new ReqRpt014DepartmentTableModel(l) { Department=department.Description});
                    continue;
                }
                //如果存在元素，则进行下去
                var ReasonList = list.GroupBy(g => g.Hold_Reason_Code).Select(s=>new { Hold_Reason_Code=s.Key,Hold_Count=s.Count()});
                foreach (var item in ReasonList)
                {
                    ReqRpt014DepartmentTableEntity tableEntity = new ReqRpt014DepartmentTableEntity()
                    { HoldCode=item.Hold_Reason_Code,HoldRate=item.Hold_Count/lotCount,LotCount=list.Where(w=>w.Hold_Reason_Code==item.Hold_Reason_Code).Select(s=>s.Lot_ID).Distinct().Count()};
                    l.Add(tableEntity);
                }
                DepartmentModels.Add(new ReqRpt014DepartmentTableModel(l) { Department=department.Description});
            }

            //by product部分
            foreach (string prod in Product_List)
            {
                var list = HoldModels.Where(s => s.ProdSpec_ID == prod);
                List<ReqRpt014ProductTableEntity> l = new List<ReqRpt014ProductTableEntity>();

                if (list is null)  //如果没有元素，则不进行查找
                {
                    ProductModels.Add(new ReqRpt014ProductTableModel(l) { ProductID=prod});
                    continue;
                }
                //如果存在元素，则进行下去
                var ReasonList = list.GroupBy(g => g.Hold_Reason_Code).Select(s=>new { Hold_Reason_Code=s.Key,HoldCount=s.Count()});
                foreach (var item in ReasonList)
                {
                    ReqRpt014ProductTableEntity tableEntity = new ReqRpt014ProductTableEntity()
                    { HoldCode = item.Hold_Reason_Code, HoldRate = item.HoldCount / lotCount, LotCount = list.Where(w => w.Hold_Reason_Code == item.Hold_Reason_Code).Select(s => s.Lot_ID).Distinct().Count() };
                    l.Add(tableEntity);
                }
                ProductModels.Add(new ReqRpt014ProductTableModel(l) { ProductID=prod});
            }
        }


    }
}