using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTWebApp.Models;
using AMS.CIM.Caojin.RPTLibrary.Models;
using Caojin.Common;
using System.Data;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt025TableViewModel
    {
        public ReqRpt025TableViewModel(ReqRpt025PostModel postModel)
        {
            SelectedFrom = DateTime.ParseExact(postModel.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            if (postModel.FromCategory == "day") { SelectedFrom= SelectedFrom.AddHours(8); } else { SelectedFrom = SelectedFrom.AddHours(20); }

            SelectedTo = DateTime.ParseExact(postModel.ToDate, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            if (postModel.ToCategory == "day") {SelectedTo= SelectedTo.AddHours(8); } else { SelectedTo = SelectedTo.AddHours(20); }

            Initialize();
        }

        public DateTime SelectedFrom { get; set; }

        public string strSelecedFrom { get { return SelectedFrom.ToString("yyyy-MM-dd HH:mm:ss"); } }

        public DateTime SelectedTo { get; set; }

        public string strSelectedTo { get { return SelectedTo.ToString("yyyy-MM-dd HH:mm:ss"); } }

        public DateTime ShowTime { get { return DateTime.Now; } }

        public string StrShowTime { get { return ShowTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        public List<ReqRpt025TableEntity> TableEntities { get; set; } = new List<ReqRpt025TableEntity>();

        private void Initialize()
        {
            //DB2Helper db2 = new DB2Helper();

            //string sql = "select d_thesystemkey from smview.fbprod where prodcat_ident = 'Production' and mainpd_id = 'APAA'";

            //db2.GetSomeData(sql);

            //foreach (DataRow dr in db2.dt.Rows)
            //{
            //    TableEntities.Add(new ReqRpt025TableEntity(dr[0].ToString()));
            //}

            var productQuerier = new ReqRptCommonProductQuerier(6);
            foreach (var prod in productQuerier.Prods)
            {
                TableEntities.Add(new ReqRpt025TableEntity(prod));
            }

            DB2DataCatcher<RPT_RealTime_Lin> db2Catcher = new DB2DataCatcher<RPT_RealTime_Lin>("ISTRPT.RPT_RealTime_Lin");
            db2Catcher.Conditions = string.Format("where start_time>='{0}' and start_time <'{1}' ", strSelecedFrom, strSelectedTo);
            var list = db2Catcher.GetEntities().EntityList;
            foreach (var item in TableEntities)
            {
                var rowList = list.Where(w => w.Product_ID == item.ProductID);
                var rowList_Fab = rowList.Where(w => w.PartName == "FAB");
                var rowList_Wat = rowList.Where(w=>w.PartName=="WAT");
                if (rowList_Fab.Count() > 0)
                {
                    item.FAB_PassQty = rowList_Fab.Sum(w => w.MoveQty);
                    item.FAB_ScrapQty = rowList_Fab.Sum(w=>w.ScrappedQty);
                }
                if (rowList_Wat.Count() > 0)
                {
                    item.WAT_PassQty = rowList_Wat.Sum(s=>s.MoveQty);
                    item.WAT_ScrapQty = rowList_Wat.Sum(s=>s.ScrappedQty);
                }
            }
        }
    }
}