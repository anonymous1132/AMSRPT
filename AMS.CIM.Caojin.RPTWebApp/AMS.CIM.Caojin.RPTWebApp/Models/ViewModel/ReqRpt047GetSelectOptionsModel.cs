using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt047GetSelectOptionsModel
    {
        public ReqRpt047GetSelectOptionsModel()
        {
            Initialize();
        }

        public List<ReqRpt047DepartmentEntity> DepartmentEntities { get; set; } = new List<ReqRpt047DepartmentEntity>();

        public List<ReqRpt047EqpTypeEntity> EqpTypeEntities { get; set; } = new List<ReqRpt047EqpTypeEntity>();

        DB2DataCatcher<Rpt_Dept_EqpType_Mapping> DeptCatcher { get; set; } = new DB2DataCatcher<Rpt_Dept_EqpType_Mapping>("ISTRPT.Rpt_Dept_EqpType_Mapping") {
        Conditions="where department != ''"};

        DB2DataCatcher<Rpt_EqpType_EqpID_Mapping> EqpTypeCatcher { get; set; } = new DB2DataCatcher<Rpt_EqpType_EqpID_Mapping>("ISTRPT.Rpt_EqpType_EqpID_Mapping");

        void Initialize()
        {
          var depts=  DeptCatcher.GetEntities().EntityList;
            var types = EqpTypeCatcher.GetEntities().EntityList;
            foreach (var dept in depts)
            {
                DepartmentEntities.Add(new ReqRpt047DepartmentEntity() {
                    Department=dept.Department,
                    EqpTypes=dept.Types.Split(',').ToList()
                });
            }

            foreach (var type in types)
            {
                EqpTypeEntities.Add(new ReqRpt047EqpTypeEntity() {
                    EqpType=type.Eqp_Type,
                    Eqps=type.Eqps.Split(',').ToList()
                });
            }
        }
    }
}