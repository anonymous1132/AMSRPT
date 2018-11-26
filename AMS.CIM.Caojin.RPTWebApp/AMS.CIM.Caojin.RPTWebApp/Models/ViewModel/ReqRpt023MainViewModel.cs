using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;
using System.Data;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt023MainViewModel
    {
        public ReqRpt023MainViewModel()
        {
            Initialize();
        }

        public Dictionary<string, string> DepartmentOptions { get { return ReqRpt024MainViewModel.Departments; } }

        public List<string> LotType=new List<string>();

        //public List<string> Product { get; set; } = new List<string>();

        private void Initialize()
        {
            DB2Helper db2 = new DB2Helper();
            string sql = "select distinct lot_type from istrpt.report24_lot_wafer_qty order by lot_type ";
            db2.GetSomeData(sql);
            foreach (DataRow dr in db2.dt.Rows)
            {
                LotType.Add(dr[0].ToString());
            }

            //sql = "select distinct prodspec_id from mmview.frlot order by prodspec_id";
            //db2.GetSomeData(sql);
            //foreach (DataRow dr in db2.dt.Rows)
            //{
            //    Product.Add(dr[0].ToString());
            //}
        }
    }
}