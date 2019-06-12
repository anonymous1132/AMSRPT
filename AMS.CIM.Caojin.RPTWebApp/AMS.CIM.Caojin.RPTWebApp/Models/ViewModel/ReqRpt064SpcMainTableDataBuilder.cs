using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt064SpcMainTableDataBuilder
    {
        /// <summary>
        /// SPC Report MainTable 数据工厂,查询起始日期的00:00:00至结束日期的23:59:59数据
        /// </summary>
        /// <param name="startDate">查询的起始日期，eg:2019-4-27</param>
        /// <param name="endDate">查询的结束日期，eg:2019-5-27</param>
        public ReqRpt064SpcMainTableDataBuilder(string startDate, string endDate)
        {
            sql = string.Format(@"
select cm.gno,
cm.cno,
gm2.dept_id,
cm.eqp_id,
cm.ctitle,
cd.usl_value,
cd.ucl_value,
cd.target_value,
cd.lsl_value,
cd.lcl_value,
cd.ctype,
t1.mean_value,
t1.value_list 
from
    spcview.spccm cm
    left join spcview.spcgm2 gm2
    on cm.gno=gm2.gno
    and cm.collection_type=gm2.collection_type
    left join spcview.spccd cd
    on cm.gno=cd.gno
    and cm.cno=cd.cno
    and cm.collection_type=cd.collection_type
    left join (
    select gno,cno,ctype,avg(point_value) mean_value,listagg(point_value,'|')  value_list  from spcview.spc_data
    where tx_datetime between '{0} 00:00:00' and '{1} 23:59:59'
    and collection_type=1
    and LTRIM(RTRIM(HIDE_FLAG))=''
    group by gno,cno,ctype
    ) t1
    on t1.gno=cm.gno
    and t1.cno=cm.cno
    and t1.ctype=cd.ctype
    ", startDate, endDate);
            SpcCatcher = new DB2DataCatcher<Report64_SPCModel>("", sql);
            Initialize();
        }

        readonly string sql = "";

        DB2DataCatcher<Report64_SPCModel> SpcCatcher { get; set; }

        public List<ReqRpt064SpcMainTableEntity> TableEntities { get; set; } = new List<ReqRpt064SpcMainTableEntity>();

        private void Initialize()
        {
            var list = SpcCatcher.GetEntities().EntityList;
            foreach (var l in list)
            {
                var entity = new ReqRpt064SpcMainTableEntity
                {
                    Department = l.Dept_ID,
                    Gno = l.Gno,
                    Cno = l.Cno,
                    EqpID = l.Eqp_ID,
                    ChartTitle = l.Ctitle,
                    Ucl = l.GetUcl(),
                    Usl = l.GetUsl(),
                    Target = l.GetTarget(),
                    Lsl = l.GetLsl(),
                    Lcl=l.GetLcl(),
                    Mean=l.GetMean(),
                    Sigma=l.GetStrSigma(),
                    Ca=l.GetStrCa(),
                    Cp=l.GetStrCp(),
                    Cpk=l.GetStrCpk()
                };
                entity.SetChartType(l.Ctype);
                TableEntities.Add(entity);
            }
        }

    }
}