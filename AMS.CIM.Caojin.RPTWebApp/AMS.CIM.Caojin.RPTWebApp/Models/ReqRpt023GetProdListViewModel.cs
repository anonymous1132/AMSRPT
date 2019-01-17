using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt023GetProdListViewModel
    {
        public ReqRpt023GetProdListViewModel(string lotType)
        {
            LotType = lotType.Split(',').ToList();
            Initialize();
        }

        public List<string> Products { get; set; } = new List<string>();

        private List<string> LotType { get; set; }



        private void Initialize()
        {
            if (LotType.Count == 0 ) return;
            string sql =string.Format("select distinct prodspec_id from mmview.frlot where lot_type in ('{0}') order by prodspec_id", string.Join("','",LotType));
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData(sql);
            foreach (DataRow dr in db2.dt.Rows)
            {
                Products.Add(dr[0].ToString());
            }
        }
    }
}