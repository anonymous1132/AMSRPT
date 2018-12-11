using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt014ProductTableModel
    {
        public ReqRpt014ProductTableModel(List<ReqRpt014ProductTableEntity>lists)
        {
            while (lists.Count < 3)
            {
                lists.Add(new ReqRpt014ProductTableEntity());
            }
            ProductTableEntities = lists;
        }
        public string ProductID { get; set; }

        private List<ReqRpt014ProductTableEntity> ProductTableEntities { get; set; } = new List<ReqRpt014ProductTableEntity>();

        public ReqRpt014ProductTableEntity FirstEntity { get { return ProductTableEntities.ElementAt(0); } }

        public ReqRpt014ProductTableEntity SecondEntity { get { return ProductTableEntities.ElementAt(1); } }

        public ReqRpt014ProductTableEntity ThirdEntity { get { return ProductTableEntities.ElementAt(2); } }

        public double ProductSum { get { return ProductTableEntities.Sum(s=>s.HoldRate); } }

        public double Top3Sum { get { return FirstEntity.HoldRate + SecondEntity.HoldRate + ThirdEntity.HoldRate; } }

        public string StrTop3Sum { get { return (Top3Sum * 100).ToString("0.00") + "%"; } }

        public string StrProductSum { get { return (ProductSum * 100).ToString("0.00") + "%"; } }
    }
}