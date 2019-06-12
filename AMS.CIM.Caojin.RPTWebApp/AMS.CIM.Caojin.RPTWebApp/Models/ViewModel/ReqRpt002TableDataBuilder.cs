using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt002TableDataBuilder
    {
        public ReqRpt002TableDataBuilder(string date)
        {
            Date = DateTime.Parse(date);
            Initialze();
        }
        DateTime Date { get; set; }

        List<string> ModuleDepartments =new List<string>{"Photo","Diffusion","CVD","PVD","ETCH","WET","CMP","DCM","PIE","Production"};

        List<string> TestDepartment = new List<string> { "Device(WAT)","QRA"};

        List<string> BankDepartment = new List<string> { "Bank","OutSource"};

        private DB2DataCatcher<RPT_Dept_CT_By_Prod> DevCatcher { get; set; } = new DB2DataCatcher<RPT_Dept_CT_By_Prod>("ISTRPT.RPT_Dept_CT_By_Prod");

       ReqRpt002WipQuerier WipQuerier { get; set; }

       ReqRpt002MoveQuerier YstdMoveQuerier { get; set; }

       ReqRpt002MoveQuerier TdMoveQuerier { get; set; }

        ReqRpt002WaferOutQuerier WaferOutQuerier { get; set; }

        public List<ReqRpt002DepartmentTableRowEntity> DeptTableModuleRowEntities { get; set; } = new List<ReqRpt002DepartmentTableRowEntity>();

        public List<ReqRpt002DepartmentTableRowEntity> DeptTableTestRowEntities { get; set; } = new List<ReqRpt002DepartmentTableRowEntity>();

        public List<ReqRpt002DepartmentTableRowEntity> DeptTableBankRowEntities { get; set; } = new List<ReqRpt002DepartmentTableRowEntity>();

        public ReqRpt002DepartmentTableRowEntity ModuleTotal { get; set; } = new ReqRpt002DepartmentTableRowEntity();

        public ReqRpt002DepartmentTableRowEntity TestTotal { get; set; } = new ReqRpt002DepartmentTableRowEntity();

        public ReqRpt002DepartmentTableRowEntity BankTotal { get; set; } = new ReqRpt002DepartmentTableRowEntity();

        public ReqRpt002DepartmentTableRowEntity FabTotal { get; set; } = new ReqRpt002DepartmentTableRowEntity();

        public List<ReqRpt002WaferOutEntity> WaferOutEntities { get; set; } = new List<ReqRpt002WaferOutEntity>();

        public ReqRpt002WaferOutEntity WaferOutTotalEntity { get; set; } = new ReqRpt002WaferOutEntity();

        double CtTotal = 0;

        private void Initialze()
        {
            //今天的实时数据
            if (Date.AddHours(32) > DateTime.Now)
            {
                WipQuerier = new ReqRpt002WipQuerier(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                //查询历史数据
                WipQuerier = new ReqRpt002WipQuerier(Date.AddHours(32).ToString("yyyy-MM-dd HH:mm:ss"));

            }
            TdMoveQuerier = new ReqRpt002MoveQuerier(Date.ToString("yyyy-MM-dd"));
            YstdMoveQuerier = new ReqRpt002MoveQuerier(Date.AddDays(-1).ToString("yyyy-MM-dd"));
            WaferOutQuerier = new ReqRpt002WaferOutQuerier(Date.ToString("yyyy-MM-dd"));

            foreach (var dept in ModuleDepartments)
            {
                var entity = new ReqRpt002DepartmentTableRowEntity();
                SetRowEntity(entity,dept);
                DeptTableModuleRowEntities.Add(entity);
            }
            foreach (var dept in TestDepartment)
            {
                var entity = new ReqRpt002DepartmentTableRowEntity();
                SetRowEntity(entity,dept);
                DeptTableTestRowEntities.Add(entity);
            }
            foreach (var dept in BankDepartment)
            {
                var entity = new ReqRpt002DepartmentTableRowEntity();
                SetRowEntity(entity, dept);
                DeptTableBankRowEntities.Add(entity);
            }
            //ModuleTotal
            ModuleTotal.Department = "Module Total";
            SetTotalEntity(ModuleTotal,DeptTableModuleRowEntities);
            //TestTotal
            TestTotal.Department = "Test Total";
            SetTotalEntity(TestTotal,DeptTableTestRowEntities);
            //BankTotal
            DeptTableBankRowEntities[0].Department = "Normal Bank";
            BankTotal.Department = "Bank Total";
            SetTotalEntity(BankTotal,DeptTableBankRowEntities);
            //FabTotal
            FabTotal.Department = "Fab Total";
            SetTotalEntity(FabTotal,DeptTableModuleRowEntities.Union(DeptTableTestRowEntities).Union(DeptTableBankRowEntities).ToList());
            GetCtData();
            SetDevValue();
            WaferOutEntities = WaferOutQuerier.WaferOutEntities;
            WaferOutTotalEntity = WaferOutQuerier.WaferOutTotalEntity;
        }

        private void SetRowEntity(ReqRpt002DepartmentTableRowEntity entity,string dept)
        {
            entity.Department = dept;
            var wips = WipQuerier.WipEntities.Where(w => w.Department == dept);
            var moves = YstdMoveQuerier.MoveEntities.Where(w => w.Department == dept);
            var td_moves = TdMoveQuerier.MoveEntities.Where(w => w.Department == dept);
            if (wips.Count() > 0)
            {
                entity.WipLot = wips.First().Lots;
                entity.WipWafer = wips.First().Wafers;
                entity.HoldLot = wips.First().HoldLots;
                entity.HoldWafer = wips.First().HoldWafers;
                entity.HoldLotOverTime = wips.First().HoldLotOverTime;
            }
            if (moves.Count() > 0)
            {
                entity.YstdMoveTarget = moves.First().MoveTarget;
                entity.YstdMoveActual = moves.First().MoveValue;
                entity.YstdMovePercentage = entity.YstdMoveTarget==0?"": Math.Round(moves.First().Percentage * 100, 2).ToString() + "%";
                entity.YstdMoveTurnRate = moves.First().TurnRate==-1?"": Math.Round(moves.First().TurnRate * 100, 2).ToString() + "%";
                entity.YstdAvaWip = moves.First().AvaWip;
            }
            if (td_moves.Count() > 0)
            {
                entity.TdMoveTarget = td_moves.First().MoveTarget;
                entity.TdMoveActual = td_moves.First().MoveValue;
                entity.TdMovePercentage = entity.TdMoveTarget == 0 ? "" : Math.Round(td_moves.First().Percentage * 100, 2).ToString() + "%";
            }
        }

        private void SetTotalEntity(ReqRpt002DepartmentTableRowEntity entity, List<ReqRpt002DepartmentTableRowEntity>tableRowEntities)
        {
            entity.WipLot = tableRowEntities.Sum(s => s.WipLot);
            entity.WipWafer = tableRowEntities.Sum(s => s.WipWafer);
            entity.HoldLot = tableRowEntities.Sum(s => s.HoldLot);
            entity.HoldWafer = tableRowEntities.Sum(s => s.HoldWafer);
            entity.HoldLotOverTime = tableRowEntities.Sum(s => s.HoldLotOverTime);
            entity.YstdMoveTarget = tableRowEntities.Sum(s => s.YstdMoveTarget);
            entity.YstdMoveActual = tableRowEntities.Sum(s => s.YstdMoveActual);
            entity.YstdMovePercentage = entity.YstdMoveTarget == 0 ? "" : Math.Round(entity.YstdMoveActual * 100.0 / entity.YstdMoveTarget, 2).ToString() + "%";
            var module_avaWip = tableRowEntities.Sum(s => s.YstdAvaWip);
            entity.YstdMoveTurnRate = module_avaWip == 0 ? "" : Math.Round(tableRowEntities.Sum(s => s.YstdMoveActual) * 100.0 / module_avaWip, 2).ToString() + "%";
            entity.TdMoveActual = tableRowEntities.Sum(s => s.TdMoveActual);
            entity.TdMoveTarget = tableRowEntities.Sum(s => s.TdMoveTarget);
            entity.TdMovePercentage = entity.TdMoveTarget == 0 ? "" : Math.Round(entity.TdMoveActual * 100.0 / entity.TdMoveTarget, 2).ToString() + "%";
        }

        private void GetCtData()
        {
            var prodList = new ReqRptCommonProductQuerier(6).Prods;
            DevCatcher.Conditions = string.Format("where prodspec_id in ('{0}')", string.Join("','", prodList));
            DevCatcher.GetEntities();
            CtTotal = DevCatcher.entities.EntityList.Sum(s => s.Total_CT);
        }


        private void SetDevValue()
        {
            if (CtTotal == 0) return;
            foreach (var entity in DeptTableModuleRowEntities)
            {
                var percent = DevCatcher.entities.EntityList.Where(w => w.Department == entity.Department).Sum(s => s.Total_CT) / CtTotal;

                var stdWip = percent * FabTotal.WipLot;

                entity.WipDev =stdWip==0?"": Math.Round((entity.WipLot - stdWip)*100 / stdWip,2).ToString()+"%";
            }
            foreach (var entity in DeptTableTestRowEntities)
            {
                var percent = DevCatcher.entities.EntityList.Where(w => w.Department == entity.Department).Sum(s => s.Total_CT) / CtTotal;

                var stdWip = percent * FabTotal.WipLot;

                entity.WipDev = stdWip == 0 ? "" : Math.Round((entity.WipLot - stdWip) * 100 / stdWip, 2).ToString() + "%";
            }
        }
    }
}