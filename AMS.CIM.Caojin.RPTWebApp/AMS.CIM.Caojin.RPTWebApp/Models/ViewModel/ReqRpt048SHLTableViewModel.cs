using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;


namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt048SHLTableViewModel
    {
        public ReqRpt048SHLTableViewModel(string Priorities)
        {
            this.Priorities = Priorities;
            Initialize();
        }

        public List<ReqRpt048SHLEntity> SHLEntities { get; set; } = new List<ReqRpt048SHLEntity>();

        public List<ReqRpt048SHLEntity> FilterdSHLEntities { get { return SHLEntities.Where(w => Priorities.Contains(w.Priority.ToString())).ToList(); } }

        public List<ReqRpt048ProductQuotaEntity> ProductQuotaEntities { get; set; } = new List<ReqRpt048ProductQuotaEntity>();

        public List<ReqRpt048DepartmentQuotaEntity> DepartmentQuotaEntities { get; set; } = new List<ReqRpt048DepartmentQuotaEntity>();

        public List<ReqRpt048ProjectQuotaEntity> ProjectQuotaEntities { get; set; } = new List<ReqRpt048ProjectQuotaEntity>();

        private string Priorities { get; set; }

        public string QueryConditions { get { return string.Format("Priority: {0}",Priorities); } }

        //For Test Stage（已经转正了。。。）
        private readonly DB2DataCatcher<SHLLotModel> LotCatcher = new DB2DataCatcher<SHLLotModel>("ISTRPT.Report48_Lot_Test");
        //For Prod Stage
        // private readonly DB2DataCatcher<SHLLotModel> LotCatcher = new DB2DataCatcher<SHLLotModel>("ISTRPT.Report48_Lot");

        //SHLV2 private readonly DB2DataCatcher<SHLFHOpehsModel> OpehsCatcher = new DB2DataCatcher<SHLFHOpehsModel>("MMVIEW.FHOPEHS");
        private readonly DB2DataCatcher<SHLFHOpehsModel> OpehsCatcher = new DB2DataCatcher<SHLFHOpehsModel>("istrpt.report48_lot_history"); //SHLV2

        private readonly DB2DataCatcher<FVCast_LotModel> FoupCatcher = new DB2DataCatcher<FVCast_LotModel>("ISTRPT.FVCAST_LOT");

        private readonly DB2DataCatcher<SHLFRPDModel> PDCatcher = new DB2DataCatcher<SHLFRPDModel>("MMVIEW.FRPD");

        private readonly DB2DataCatcher<Report_Lot_OwnerDepartment_Mapping> OwnerDeptCatcher = new DB2DataCatcher<Report_Lot_OwnerDepartment_Mapping>("ISTRPT.Report_Lot_OwnerDepartment_Mapping");

        private readonly DB2DataCatcher<Rpt_Lot_Quota_Mapping> QuotaCatcher = new DB2DataCatcher<Rpt_Lot_Quota_Mapping>("ISTRPT.Report_lot_quota_mapping");

        private readonly DB2DataCatcher<SHLSTDTimeFlowModel> FlowCTCatcher = new DB2DataCatcher<SHLSTDTimeFlowModel>("ISTRPT.report48_flow_ct");

        private readonly DB2DataCatcher<Rpt_Quota_Define> QuotaDefineCatcher = new DB2DataCatcher<Rpt_Quota_Define>("ISTRPT.Rpt_Quota_Define");

        private readonly DB2DataCatcher<FRCodeModel> CodeCatcher = new DB2DataCatcher<FRCodeModel>("MMVIEW.FRCODE");

        private readonly DB2DataCatcher<FREQP_CurStatus> EqpCatcher = new DB2DataCatcher<FREQP_CurStatus>("MMVIEW.FREQP");

        #region DBDatas

        //LotModel集合
        IEnumerable<SHLLotModel> lotList ;
        //LotModel集合中的Lot_ID集合
        IEnumerable<string> lot_list;
        //LotModel集合中的Lot_Family_ID集合
        IEnumerable<string> lot_family_list;
        //LotModel集合中的split_lot_id集合
        IEnumerable<string> lot_split_list;
        //lot_id,lot_family_id,split_lot_id去重后集合
        IEnumerable<string> lot_all_list;
        //FHOpehsModel集合
        IList<SHLFHOpehsModel> opeList;
        //FoupModel集合
        IList<FVCast_LotModel> castList;
        //FRPDModel集合
        IList<SHLFRPDModel> pdList;
        //FREQP集合
        IList<FREQP_CurStatus> eqpList;
        //Report_Lot_OwnerDepartment_Mapping集合
        IList<Report_Lot_OwnerDepartment_Mapping> ownerDeptList;
        //Rpt_Lot_Quota_Mapping集合
        IList<Rpt_Lot_Quota_Mapping> quotaList;
        //SHLSTDTimeFlowModel集合
        IEnumerable<SHLSTDTimeFlowModel> ctList;
        //watPD集合
        IEnumerable<SHLFRPDModel> watPds;
        //lot_list对应的当前所在站点集合
        IEnumerable<SHLFHOpehsModel> curOpeList;
        //所有Department集合
        IList<FRCodeModel> deptList;
        //Rpt_Quota_Define集合
        IList<Rpt_Quota_Define> quotaDefineList;
        IEnumerable<SHLLotModel> SHLLotList { get { return lotList.Where(w => w.Priority_Class == 1); } }
        IEnumerable<SHLLotModel> HLLotList { get { return lotList.Where(w => w.Priority_Class == 2); } }
        #endregion


        private void Initialize()
        {
            DBQuery();

            foreach (var lot in lotList)
            {
                GetSHLEntity(lot);      
            }

            GetProductEntity();

            GetDepartmentEntity();

            GetProjectEntity();

            GetNormalEntity();
        }

        private void DBQuery()
        {
            // LotQuery
             LotCatcher.Conditions = string.Format("where Priority_Class in (1,2)");
            lotList = LotCatcher.GetEntities().EntityList;
            if (!lotList.Any()) throw new Exception(string.Format("当前没有Priority 为{0}的Super Hot Lot", Priorities));
            lot_list = lotList.Select(s => s.Lot_ID);
             lot_family_list = lotList.Select(s => s.Lot_Family_ID).Distinct();
             lot_split_list = lotList.Select(s => s.Split_Lot_ID).Distinct();
             lot_all_list = lot_list.Union(lot_family_list).Union(lot_split_list).Distinct();
            string lots = string.Join("','", lot_list);
            string lot_families = string.Join("','", lot_family_list);
            string lot_splits = string.Join("','", lot_split_list);
            string lots_all = string.Join("','", lot_all_list);
            //Ope History Query
            OpehsCatcher.Conditions = string.Format("where lot_id in ('{0}') and ope_pass_count > 0 order by claim_time desc", lots_all);
            opeList = OpehsCatcher.GetEntities().EntityList;
            //CastQuery
            FoupCatcher.Conditions = string.Format("where Lot_ID in ('{0}')", lots);
            castList = FoupCatcher.GetEntities().EntityList;
            //PDQuery
             curOpeList = opeList.GroupBy(g => g.Lot_ID).Select(s => new SHLFHOpehsModel() { Lot_ID = s.Key, Claim_Time = s.Max(m => m.Claim_Time),PD_ID= opeList.Where(w => w.Claim_Time == s.Max(m => m.Claim_Time)).Last().PD_ID,Ope_No= opeList.Where(w => w.Claim_Time == s.Max(m => m.Claim_Time)).Last().Ope_No });
            PDCatcher.Conditions = string.Format("where pd_id in ('{0}') or department='I'", string.Join("','", curOpeList.Select(s => s.PD_ID).Distinct()));
             pdList = PDCatcher.GetEntities().EntityList;
            //Owner Department Query
            OwnerDeptCatcher.Conditions = string.Format("where lot_id in ('{0}')", lots);
            ownerDeptList = OwnerDeptCatcher.GetEntities().EntityList;
            //Quota Query
            QuotaCatcher.Conditions = string.Format("where lot_id in ('{0}')", lot_families);
            quotaList = QuotaCatcher.GetEntities().EntityList;
            //Cycle Time Query
            FlowCTCatcher.Conditions = string.Format("where prodspec_id in ('{0}') order by prodspec_id,ope_no", string.Join("','", lotList.Select(s => s.ProdSpec_ID).Distinct()));
            ctList = FlowCTCatcher.GetEntities().EntityList;
            //All Department Query
            CodeCatcher.Conditions = "where category_id='Department'";
            deptList = CodeCatcher.GetEntities().EntityList;
            //QuotaDefine Query
            //QuotaDefineCatcher.Conditions = "";
            quotaDefineList = QuotaDefineCatcher.GetEntities().EntityList;
            EqpCatcher.Conditions = string.Format("where eqp_type !=''");
            eqpList = EqpCatcher.GetEntities().EntityList;
            watPds = pdList.Where(w => w.Department == "I");
        }

        private void GetSHLEntity(SHLLotModel lot)
        {
            var cast = castList.Where(w => w.Lot_ID == lot.Lot_ID).FirstOrDefault()??new FVCast_LotModel();
            var lot_pd =curOpeList.Where(w => w.Lot_ID == lot.Lot_ID).FirstOrDefault()??new SHLFHOpehsModel();
            //当前pd
            var pd = pdList.Where(w => w.PD_ID == lot_pd.PD_ID).FirstOrDefault()??new SHLFRPDModel();
            var owner = ownerDeptList.Where(w => w.Lot_ID == lot.Lot_ID).FirstOrDefault()??new Report_Lot_OwnerDepartment_Mapping();
            var quota = quotaList.Where(w => w.Lot_ID == lot.Lot_Family_ID).FirstOrDefault()??new Rpt_Lot_Quota_Mapping();
            var opes = opeList.Where(w => w.Lot_ID == lot.Lot_ID);
            var opes_family = opeList.Where(w => w.Lot_ID == lot.Lot_Family_ID);
            //var opes_withsplits = string.IsNullOrEmpty(lot.Split_Lot_ID)?opes:opes.Union(opeList.Where(w => w.Lot_ID == lot.Split_Lot_ID));
            var inOpeList = opes.Where(w => w.Ope_Category == "Ope_Complete" || w.Ope_Category == "STB" || w.Ope_Category.Contains("Locate"));
            //进站时间:如果lot_id对应操作记录能查找到，则在记录中取，否则进入函数查找
            var inTime =inOpeList.Any()?inOpeList.First().Claim_Time: GetComeinTime(lot);
            //第一站（如果是子批，则对应family lot的第一站）
            var firstpd = opes_family.Where(w => w.Ope_Category == "Ope_Complete" || w.Ope_Category == "STB").LastOrDefault();
            var ctList_prod = ctList.Where(w => w.ProdSpec_ID == lot.ProdSpec_ID);
            var today = DateTime.Now.Date.AddHours(8);
            //获取剩余站点
            var remainPds = ctList_prod.SkipWhile(s => s.Ope_No!=lot_pd.Ope_No);
            double m = lot.Priority_Class == 1 ? 1.3 : 1.6;
            //总 cycle time
            double ctValue_total = ctList_prod.Sum(s => s.PD_STD_Proc_Time_Min) * m;
            //剩余cycle time
            double ctValue_Remain = remainPds.Sum(s => s.PD_STD_Proc_Time_Min)*m;
            int test = remainPds.Count();
            //当前站点到wat站点的cycle time
            double ctValue_wat = remainPds.TakeWhile(t => !watPds.Select(s => s.PD_ID).Contains(t.PD_ID)).Sum(s => s.PD_STD_Proc_Time_Min)*m;
            var dept = deptList.Where(w => w.Code_ID == pd.Department);
            DateTime baseTime;
            if (remainPds.Count() == 0){ baseTime = DateTime.Now; }
            else
            {
                baseTime = inTime.AddMinutes(remainPds.First().PD_STD_Cycle_Time_Min) > DateTime.Now ? inTime : DateTime.Now;
            }
            var shl = new ReqRpt048SHLEntity
            {
                LotID = lot.Lot_ID,
                FoupID = cast.Cast_ID,
                Location = cast.Eqp_ID,
                Status = cast.Xfer_State,
                Priority = lot.Priority_Class,
                Department = dept.Any() ? dept.First().Description : pd.Department,
                QuotaOwner = owner.DisplayOwner,
                QuotaType = quota.Quota_Type,
                Project = quota.Project_Desc,
                Purpose = quota.Purpose,
                Qty = lot.Qty,
                OpeNo=lot_pd.Ope_No,
                Description = pd.Ope_Name,
                LotStates = lot.Lot_Hold_State,
                ProcessStates = lot.Lot_Process_State,
                WaferStart = lot.Created_Time,
                YSDT = opes.Where(w => w.Claim_Time < today && w.Claim_Time > today.AddDays(-1)).Select(s=> s.PD_ID).Distinct().Count(),
                Remark = lot.Hold_Claim_Memo,
                WAT = baseTime.AddMinutes(ctValue_wat),
                WaferOut = baseTime.AddMinutes(ctValue_Remain),
                TargetWaferOut = firstpd.Claim_Time.AddMinutes(ctValue_total),
                ProductID = lot.ProdSpec_ID,
                QuotaDept=owner.Description
            };
            var stages = opes.Where(w => w.Priority_Class != lot.Priority_Class);
            shl.PriChgStage = stages.Any() ? stages.First().Stage_ID : "";
            var types = ctList_prod.Where(w => w.Ope_No == lot_pd.Ope_No);
            shl.EqpType =types.Count()>0?types .First().Eqp_Type:"";
            var eqps = eqpList.Where(w => w.Eqp_Type == shl.EqpType);
            int total = eqps.Count();
            int ava = eqps.Where(w => w.E10_State == "PRD" || w.E10_State == "SBY").Count();
            shl.EqpType += string.Format(" {2}/{0}/{1}",ava,total-ava,total);
            SHLEntities.Add(shl);
        }

        private DateTime GetComeinTime(SHLLotModel lot)
        {
            string lotID = lot.Lot_ID;
            string subLotID =string.Join("", lotID.TakeWhile(t=>t!='.'));
            DB2DataCatcher<SHLFHOpehsModel> opehsCatcher = new DB2DataCatcher<SHLFHOpehsModel>("MMVIEW.FHOPEHS") { Conditions=string.Format("where lot_id like '{0}.%' order by claim_time",subLotID)};
            var list = opehsCatcher.GetEntities().EntityList;
            var sub_list = list.TakeWhile(w => !(w.Lot_ID == lot.Lot_ID && w.Ope_Category == "Split"));
            var motherLotOpe = sub_list.Where(w => w.Lot_ID == lot.Lot_ID && (w.Ope_Category == "Ope_Complete" || w.Ope_Category == "STB" || w.Ope_Category.Contains("Locate")));
            if (motherLotOpe.Any())
            {
                return motherLotOpe.Last().Claim_Time;
            }
            else
            {
                //如果母批也是split。。。
                return GetComeinTimeCaseMotherNoIn(lot,list);
            }
        }

        private DateTime GetComeinTimeCaseMotherNoIn(SHLLotModel lot,IList<SHLFHOpehsModel>opes)
        {
            DB2DataCatcher<SHLFRLot_Family> familyCatcher = new DB2DataCatcher<SHLFRLot_Family>("MMVIEW.FRLot") { Conditions=string.Format( "where lot_family_id='{0}'",lot.Lot_Family_ID)};
            var familyList = familyCatcher.GetEntities().EntityList;
            string split_lot = lot.Split_Lot_ID;
            while (!string.IsNullOrEmpty(split_lot))
            {
                var nextSplit= familyList.Where(w => w.Lot_ID == split_lot).First();
                var sub_list = opes.TakeWhile(w => !(w.Lot_ID == nextSplit.Lot_ID && w.Ope_Category == "Split"));
                var motherLotOpe = sub_list.Where(w => w.Lot_ID == nextSplit.Lot_ID && (w.Ope_Category == "Ope_Complete" || w.Ope_Category == "STB" || w.Ope_Category.Contains("Locate")));
                if (motherLotOpe.Any())
                {
                    return motherLotOpe.Last().Claim_Time;
                }
                split_lot = nextSplit.Split_Lot_ID;
            }
            throw new Exception("ReqRpt048SHLTableViewModel:GetComeinTimeCaseMotherNoIn:History Not Found Enter PD record!");
        }

        private void GetProductEntity()
        {
            var prodList = lotList.Select(s => s.ProdSpec_ID).Distinct();
            foreach(string prod in prodList)
            {
                var entity = new ReqRpt048ProductQuotaEntity
                {
                    ProductID = prod
                };
                var list = lotList.Where(w => w.ProdSpec_ID == prod);
                entity.SHL = list.Where(w=> w.Priority_Class == 1).Select(s=>s.Lot_Family_ID).Distinct().Count();
                entity.HL = lotList.Where(w => w.Priority_Class == 2).Select(s=>s.Lot_Family_ID).Distinct().Count();
                ProductQuotaEntities.Add(entity);
            }
        }

        private void GetDepartmentEntity()
        {

            var quotaMap_project = quotaList.Where(w => w.Quota_Type == 1);
            foreach (var dept in deptList)
            {
                var entity = new ReqRpt048DepartmentQuotaEntity { Department=dept.Description};
                //部门所有的shl、hl
                var lots = SHLEntities.Where(w => w.QuotaDept == dept.Description);
                //部门定义的quota
                var quotaDefine = quotaDefineList.Where(w => w.Department ==dept.Description);
                //部门普通shl quota
                var quotaDefine_normal = quotaDefine.Where(w => w.Quota_Type == 0);
                //部门project shl quota
                var quotaDefine_project = quotaDefine.Where(w => w.Quota_Type == 1);
                
                var shls = lots.Where(w => w.Priority == 1).Select(s => s.LotID.Split('.')[0]).Distinct(); //shl
                var hls = lots.Where(w => w.Priority == 2).Select(s => s.LotID.Split('.')[0]).Distinct();  //hl
                int allSHLUsed = shls.Count();
                int allHLUsed = hls.Count();
                entity.NormalHLQuota = quotaDefine_normal.Sum(s => s.Quota_HL);
                entity.NormalSHLQuota= quotaDefine_normal.Sum(s => s.Quota_SHL);
                entity.ProjectSHLQuota = quotaDefine_project.Sum(s=>s.Quota_SHL);
                entity.ProjectHLQuota = quotaDefine_project.Sum(s => s.Quota_HL);
                entity.ProjectSHLUsed = shls.Intersect(quotaMap_project.Select(s=>s.Lot_ID.Split('.')[0])).Count();
                entity.ProjectHLUsed = hls.Intersect(quotaMap_project.Select(s=>s.Lot_ID.Split('.')[0])).Count();
                entity.NormalSHLUsed = allSHLUsed - entity.ProjectSHLUsed;
                entity.NormalHLUsed = allHLUsed - entity.ProjectHLUsed;
                DepartmentQuotaEntities.Add(entity);
            }
        }

        private void GetProjectEntity()
        {
            var quota_project = quotaDefineList.Where(w=>w.Quota_Type==1);
            foreach (var define in quota_project)
            {
                var entity = new ReqRpt048ProjectQuotaEntity { Department=define.Department,Project=define.Project_Desc,QuotaSHL=define.Quota_SHL,QuotaHL=define.Quota_HL,Purpose=define.Purpose,QuotaType=define.Quota_Type};
                var shls = SHLEntities.Where(w => w.QuotaDept == define.Department && w.Project == define.Project_Desc);
                entity.UsedHL = shls.Where(w=>w.Priority==2).Select(s => s.LotID.Split('.')[0]).Count();
               entity.UsedSHL = shls.Where(w => w.Priority == 1).Select(s => s.LotID.Split('.')[0]).Count();
                ProjectQuotaEntities.Add(entity);
            }
        }

        private void GetNormalEntity()
        {
            var quota_normal = quotaDefineList.Where(w => w.Quota_Type == 0);
            foreach (var define in quota_normal)
            {
                var entity = new ReqRpt048ProjectQuotaEntity { Department = define.Department, Project = "", QuotaSHL = define.Quota_SHL, QuotaHL = define.Quota_HL, Purpose = define.Purpose, QuotaType = define.Quota_Type };
                var shls = SHLEntities.Where(w => w.QuotaDept == define.Department && w.QuotaType==define.Quota_Type);
                entity.UsedHL = shls.Where(w => w.Priority == 2).Select(s => s.LotID.Split('.')[0]).Count();
                entity.UsedSHL = shls.Where(w => w.Priority == 1).Select(s => s.LotID.Split('.')[0]).Count();
                ProjectQuotaEntities.Add(entity);
            }
        }
    }
}