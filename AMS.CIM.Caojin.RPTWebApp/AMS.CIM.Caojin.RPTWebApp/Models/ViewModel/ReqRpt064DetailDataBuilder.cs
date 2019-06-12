using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt064DetailDataBuilder
    {
        public ReqRpt064DetailDataBuilder(string gno, string cno, string ctype, string startDate, string endDate)
        {
            sql = string.Format(@" select cm.gno,cm.cno,cd.ctype, cm.ctitle,CM.MFLD_ID,CM.DCSPEC_ID,CM.ITEM_NM,CM.CName,CM.Sample_Size,CM.Max_Points,
    CD.UPL_Value,cd.usl_value,cd.ucl_value,CD.UWL_Value,cd.target_value,cd.mean_value,cd.lsl_value,cd.lcl_value,cd.lpl_value,cd.lwl_value,
    t1.mean_value act_mean_value, t1.value_list,t1.time_list,t2.from_time,t2.to_time from
    spcview.spccm cm
    left join spcview.spcgm2 gm2
    on cm.gno=gm2.gno
    and cm.collection_type=gm2.collection_type
    left join spcview.spccd cd
    on cm.gno=cd.gno
    and cm.cno=cd.cno
    and cm.collection_type=cd.collection_type
    left join (
    select gno,cno,ctype,avg(point_value) mean_value,listagg(point_value,'|')  value_list,listagg(tx_datetime,'|') time_list from spcview.spc_data 
    where tx_datetime between '{0} 00:00:00' and '{1} 23:59:59' 
    and collection_type=1 
    and LTRIM(RTRIM(HIDE_FLAG))=''
    group by gno,cno,ctype
    ) t1
    on t1.gno=cm.gno
    and t1.cno=cm.cno
    and t1.ctype=cd.ctype
    left join (
     select gno,cno,ctype,min(tx_datetime) as from_time,max(tx_datetime) as to_time from spcview.spc_data where collection_type='1' group by gno,cno,ctype
    ) t2
    on t2.gno=cm.gno
    and t2.cno=cm.cno
    and t2.ctype=cd.ctype
    where cm.gno={2}
    and cm.cno={3}
    and cd.ctype={4}
", startDate,endDate,gno,cno,ctype);
            Initialize();
        }

        readonly string sql = "";

        DB2DataCatcher<Report64_DetailModel> DetailCatcher { get; set; }

        public ReqRpt064DetailModel DetailModel { get; set; } = new ReqRpt064DetailModel();

        void Initialize()
        {
            DetailCatcher = new DB2DataCatcher<Report64_DetailModel>("",sql);
            var list = DetailCatcher.GetEntities().EntityList;
            if (!list.Any()) throw new Exception("DB未查找到该Chart");
            var detail = list.First();
            DetailModel.ValueList =detail.Act_Mean_Value.HasValue? detail.Value_List.Split('|').Select(s=>Convert.ToDouble(s).ToString()).ToList():new List<string>();
            DetailModel.TimeList =detail.Act_Mean_Value.HasValue? detail.Time_List.Split('|').Select(s=> { var arry = s.Split('-');return arry[0] + "-" + arry[1] + "-" + arry[2] + " " + arry[3]; }).ToList():new List<string>();
            DetailModel.SetModel.ChartName = detail.Cname;
            DetailModel.SetModel.ChartTitle = detail.Ctitle;
            DetailModel.SetModel.ChartType = GetChartType(detail.Ctype);
            DetailModel.SetModel.DcID = detail.Mfld_ID;
            DetailModel.SetModel.DcSpecID = detail.DcSpec_ID;
            DetailModel.SetModel.From = detail.From_Time.HasValue?detail.From_Time.Value.ToString("yyyy-MM-dd HH:mm:ss"):"";
            DetailModel.SetModel.To = detail.To_Time.HasValue ? detail.To_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            DetailModel.SetModel.ItemName = detail.Item_NM;
            DetailModel.SetModel.Lcl = TranslateNullableValue(detail.Lcl_Value);
            DetailModel.SetModel.Lpl =TranslateNullableValue( detail.Lpl_Value);
            DetailModel.SetModel.Lsl =TranslateNullableValue( detail.Lsl_Value);
            DetailModel.SetModel.Lwl =TranslateNullableValue( detail.Lwl_Value);
            DetailModel.SetModel.MaxPoint = detail.Max_Points.ToString();
            DetailModel.SetModel.Mean =TranslateNullableValue( detail.Mean_Value);
            DetailModel.SetModel.SampleSize = detail.Sample_Size.ToString();
            DetailModel.SetModel.Target = TranslateNullableValue(detail.Target_Value);
            DetailModel.SetModel.Ucl = TranslateNullableValue(detail.Ucl_Value);
            DetailModel.SetModel.Upl = TranslateNullableValue(detail.Upl_Value);
            DetailModel.SetModel.Usl = TranslateNullableValue(detail.Usl_Value);
            DetailModel.SetModel.Uwl = TranslateNullableValue(detail.Uwl_Value);
            DetailModel.ProcessModel.Lsl = detail.Lsl_Value == 1E-37 ? null : detail.Lsl_Value;
            DetailModel.ProcessModel.Lcl = detail.Lcl_Value == 1E-37 ? null : detail.Lcl_Value;
            DetailModel.ProcessModel.Usl = detail.Usl_Value == 1E-37 ? null : detail.Usl_Value;
            DetailModel.ProcessModel.Ucl = detail.Ucl_Value == 1E-37 ? null : detail.Ucl_Value;
            DetailModel.ProcessModel.Target = detail.Target_Value == 1E-37 ? null : detail.Target_Value;
            DetailModel.ProcessModel.Mean = detail.Act_Mean_Value;
            DetailModel.SetProcessCalcProperty();
            DetailModel.SetStaticCalcProperty();
        }

        private string GetChartType(int type)
        {
            string ChartType = "";
            switch (type)
            {
                case 1:
                    ChartType = "Mean";
                    break;
                case 2:
                    ChartType = "Range";
                    break;
                case 3:
                    ChartType = "Sigma";
                    break;
                case 4:
                    ChartType = "Uniformity";
                    break;
                case 10:
                    ChartType = "Single";
                    break;
                case 11:
                    ChartType = "Moving Range";
                    break;
                case 20:
                    ChartType = "Count";
                    break;
                default:
                    ChartType = "Undefined";
                    break;
            }
            return ChartType;
        }

        private string TranslateNullableValue(double? dValue)
        {
            if ((!dValue.HasValue) || dValue == 1E-37) return "";
            return dValue.ToString();
        }
    }
}