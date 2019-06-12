using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRptCommonProductQuerier
    {
        public ReqRptCommonProductQuerier(int type)
        {
            this.type = type;
            Initialize();
        }
        public List<string> Prods { get; set; } = new List<string>();
        private int type { get; set; }

        private void Initialize()
        {
            string sql = "select prodspec_id from mmview.fvprodspec ";
            switch (type)
            {
                case 0:
                    sql += "where prodcat_id='Dummy'";
                    break;
                case 1:
                    sql += "where prodcat_id='Equipment Monitor'";
                    break;
                case 2:
                    sql += "where prodcat_id='Process Monitor'";
                    break;
                case 3:
                    sql += "where prodcat_id='Production'";
                    break;
                case 4:
                    sql += "where prodcat_id='Raw'";
                    break;
                case 5:
                    sql += "where prodcat_id='Recycle'";
                    break;
                case 6:
                    sql += "where prodcat_id='Production' and prodspec_id not like 'SL%'";
                    break;
                default:
                    break;
            }
            DB2Helper dB2 = new DB2Helper();
            dB2.GetSomeData(sql);
            for (var i = 0; i < dB2.dt.Rows.Count; i++)
            {
                Prods.Add(dB2.dt.Rows[i][0].ToString());
            }
        }
    }
}