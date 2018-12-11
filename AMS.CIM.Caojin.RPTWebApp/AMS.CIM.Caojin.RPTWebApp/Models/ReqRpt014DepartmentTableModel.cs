using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt014DepartmentTableModel
    {
        public ReqRpt014DepartmentTableModel(List<ReqRpt014DepartmentTableEntity>lists)
        {
            while (lists.Count < 3)
            {
                lists.Add(new ReqRpt014DepartmentTableEntity());
            }
            DepartmentTableEntities = lists.OrderByDescending(o=>o.HoldRate).ToList();
        }

        public string Department { get; set; }

        private List<ReqRpt014DepartmentTableEntity> DepartmentTableEntities { get; set; } = new List<ReqRpt014DepartmentTableEntity>();

        public ReqRpt014DepartmentTableEntity FirstEntity { get { return DepartmentTableEntities.ElementAt(0); } }

        public ReqRpt014DepartmentTableEntity SecondEntity { get { return DepartmentTableEntities.ElementAt(1); } }

        public ReqRpt014DepartmentTableEntity ThirdEntity { get { return DepartmentTableEntities.ElementAt(2); } }

        public double DepartmentSum { get { return DepartmentTableEntities.Sum(s => s.HoldRate); } }

        public double Top3Sum { get { return FirstEntity.HoldRate + SecondEntity.HoldRate + ThirdEntity.HoldRate; } }

        public string StrTop3Sum { get { return (Top3Sum * 100).ToString("0.00") + "%"; } }

        public string StrDepartmentSum { get { return (DepartmentSum * 100).ToString("0.00") + "%"; } }
    }
}