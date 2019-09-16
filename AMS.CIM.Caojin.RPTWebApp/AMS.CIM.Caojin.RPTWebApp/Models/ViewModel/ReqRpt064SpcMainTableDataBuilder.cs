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
            /*
            sql = string.Format(@"
select cm.gno,
cm.cno,
gm2.dept_id,
gm2.gname,
cm.eqp_id,
cm.ctitle,
--gm1.product_id,
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
   -- left join spcview.spcgm1 gm1
   -- on cm.gno=gm1.gno
   -- and cm.collection_type=gm1.collection_type
   -- and gm1.sub_gno=1
    left join spcview.spcgm2 gm2
    on cm.gno=gm2.gno
    and cm.collection_type=gm2.collection_type
    left join spcview.spccd cd
    on cm.gno=cd.gno
    and cm.cno=cd.cno
    and cm.collection_type=cd.collection_type
    left join (
    select gno,cno,ctype,avg(point_value) mean_value,listagg(cast( point_value as varchar(30000)),'|')  value_list  from spcview.spc_data
    where tx_datetime between '{0} 00:00:00' and '{1} 23:59:59'
    and collection_type=1
    and LTRIM(RTRIM(HIDE_FLAG))=''
    group by gno,cno,ctype
    ) t1
    on t1.gno=cm.gno
    and t1.cno=cm.cno
    and t1.ctype=cd.ctype
    ", startDate, endDate);
    */
            sql = string.Format(@"with tb as
(
select gno,cno,ctype,point_value,LSL_Value,USL_Value from spcview.spc_data 
    where tx_datetime between '{0} 00:00:00' and '{1} 23:59:59' 
    and collection_type=1 
    and LTRIM(RTRIM(HIDE_FLAG))=''
),
t1 as (
select gno,cno,ctype,avg(point_value) mean_value,listagg(cast( point_value as varchar(30000)),'|')  value_list  from tb group by gno,cno,ctype
),
t2 as (
select gno,cno,ctype, count(*) as oos from tb
where (point_value < LSL_Value and LSL_Value !=1.0E-37)
or (point_value>USL_Value and USL_Value!=1.0E-37)
group by gno,cno,ctype
)
select cm.gno,
cm.cno,
gm2.dept_id,
gm2.gname,
cm.eqp_id,
cm.ctitle,
--gm1.product_id,
cd.usl_value,
cd.ucl_value,
cd.target_value,
cd.lsl_value,
cd.lcl_value,
cd.ctype,
t3.mean_value,
t3.value_list ,
t3.oos
from
    spcview.spccm cm
   -- left join spcview.spcgm1 gm1
   -- on cm.gno=gm1.gno
   -- and cm.collection_type=gm1.collection_type
   -- and gm1.sub_gno=1
    left join spcview.spcgm2 gm2
    on cm.gno=gm2.gno
    and cm.collection_type=gm2.collection_type
    left join spcview.spccd cd
    on cm.gno=cd.gno
    and cm.cno=cd.cno
    and cm.collection_type=cd.collection_type
    left join (
        select t1.*,t2.oos from t1 left join t2 on t1.gno=t2.gno and t1.cno=t2.cno and t1.ctype=t2.ctype
    ) t3
    on t3.gno=cm.gno
    and t3.cno=cm.cno
    and t3.ctype=cd.ctype", startDate, endDate);
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
                    Cpk=l.GetStrCpk(),
                    Gname=l.Gname,
                    OOSRate=l.GetOOSRate()
                   // Prod=l.Product_ID
                };
                entity.SetChartType(l.Ctype);
                TableEntities.Add(entity);
            }
        }

    }
}

/*改进方向：假如以后30000也不够一个月的数据量，则可以考虑以下sql，筛选出1500条以下的data，用当前方法，超过1500条的数据单独拉出来计算
select gno,cno,ctype,avg(point_value) mean_value,listagg(cast( point_value as varchar(30000)),'|')  value_list from spcview.spc_data
    where tx_datetime between '2019-7-1 00:00:00' and '2019-7-29 23:59:59'
    and collection_type=1
    and LTRIM(RTRIM(HIDE_FLAG))=''
    and (gno,cno,ctype) in ( select gno,cno,ctype from spcview.spc_data    where tx_datetime between '2019-7-1 00:00:00' and '2019-7-29 23:59:59'
    and collection_type=1
    and LTRIM(RTRIM(HIDE_FLAG))=''
    group by gno,cno,ctype having count(*)<=1500)
    group by gno,cno,ctype 
 */
