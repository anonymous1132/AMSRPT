using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using System.Data;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt030MainViewModel
    {
        public List<string> Lot_ID { get; set; } = new List<string>();

        public ReqRpt030MainViewModel()
        {
            DB2Helper db2 = new DB2Helper();
            db2.GetSomeData("select lot_id from istrpt.rpt_wip_special_lot");
            foreach (DataRow dr in db2.dt.Rows)
            {
                Lot_ID.Add(dr.ToString());
            }
        }
    }
}