using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using AMS.CIM.Caojin.RPTLibrary;
using System.Data.Entity;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt020TableViewModel
    {
        public ReqRpt020TableViewModel(ReqRpt020TablePostModel postModel)
        {
            PostModel = postModel;
            Initialize();
        }

        private readonly ReqRpt020TablePostModel PostModel;

        private DateTime D_StartDate { get; set; }

        private DateTime D_EndDate { get; set; }
        //sql中between startDate~endDate 8:00:00(包含首日与末日)
        private DateTime sqlStartDate { get { return D_StartDate; } }

        private DateTime sqlEndDate { get { return D_EndDate.AddHours(8); } }

        public string StartDate { get { return D_StartDate == DateTime.Now.Date ? "Today" : D_StartDate.ToString("yyyyMMdd"); } }

        public string EndDate { get { return D_EndDate == DateTime.Now.Date ? "Today" : D_EndDate.ToString("yyyyMMdd"); } }

        public string Conditions { get { return string.IsNullOrEmpty(PostModel.EqpTypes) ? "" : string.Format("EqpTypes:{0},From {1} To {2}", PostModel.EqpTypes, PostModel.StartDate, PostModel.EndDate); } }

        public List<ReqRpt020TBodyEntity> TbodyEntities { get; set; } = new List<ReqRpt020TBodyEntity>();

        private void GetDateFromPostModel()
        {

            switch (PostModel.StartDate.Length)
            {  //by date
                case 10:
                    D_StartDate = DateTime.Parse(PostModel.StartDate);
                    D_EndDate = DateTime.Parse(PostModel.EndDate);
                    break;
                //by month
                case 7:
                    D_StartDate = DateTime.Parse(PostModel.StartDate + "-01");
                    int eyear = Convert.ToInt16(PostModel.EndDate.Substring(0, 4));
                    int emonth = Convert.ToInt16(PostModel.EndDate.Substring(5, 2));
                    int eday = DateTime.DaysInMonth(eyear, emonth);
                    D_EndDate = DateTime.Parse(string.Format("{0}-{1}-{2}", eyear, emonth, eday));
                    break;
                //by week
                case 8:
                    int syear_w = Convert.ToInt16(PostModel.StartDate.Substring(0, 4));
                    int sweek_w = Convert.ToInt16(PostModel.StartDate.Substring(6, 2));
                    int eyear_w = Convert.ToInt16(PostModel.EndDate.Substring(0, 4));
                    int eweek_w = Convert.ToInt16(PostModel.EndDate.Substring(6, 2));
                    D_StartDate = TimeHelper.GetFirstEndDayOfWeek(syear_w, sweek_w, System.Globalization.CultureInfo.GetCultureInfo("zh-cn")).Item1;
                    D_EndDate = TimeHelper.GetFirstEndDayOfWeek(eyear_w, eweek_w, System.Globalization.CultureInfo.GetCultureInfo("zh-cn")).Item2;
                    break;
                default:
                    throw new Exception("DateTime Format Undefined!");
            }

            if (D_EndDate < D_StartDate)
            {
                throw new Exception("时间区间错误！");
            }
        }

        private void GetTbodyEntities()
        {
            var list = PostModel.EqpTypes.Split(',');
            //获取Actual值
            using (RPTContext db = new RPTContext())
            {
                var rawList = db.EQP_UPm_018.Where(w => DbFunctions.TruncateTime(w.Date) >= sqlStartDate && DbFunctions.TruncateTime(w.Date) <= sqlEndDate && list.Contains(w.EqpType));
                foreach (var item in list)
                {
                    ReqRpt020TBodyEntity bodyEntity = new ReqRpt020TBodyEntity();
                    bodyEntity.TotalRow.EqpType = item;
                    if (rawList.Count() > 0)
                    {
                        var rowList = rawList.Where(w => w.EqpType == item).GroupBy(g => g.EqpID).Select(s => new
                        {
                            EqpID = s.Key,
                            sd = s.Sum(su => su.SDTMin),
                            pr = s.Sum(su => su.PRDMin),
                            sb = s.Sum(su => su.SBYMin),
                            en = s.Sum(su => su.ENGMin),
                            nd = s.Sum(su => su.UDTMin),
                            rwk = s.Sum(su => su.ReworkQty),
                            pass = s.Sum(su => su.Passqty),
                            eff = s.Sum(su => su.EffMove)
                        });
                        foreach (var row in rowList)
                        {
                            ReqRpt020TableRowEntity rowEntity = new ReqRpt020TableRowEntity();
                            double ot = row.pr + row.sb + row.sd + row.nd + row.en;
                            double ut = row.pr + row.sb;
                            int days = (D_EndDate - D_StartDate).Days + 1;
                            rowEntity.EqpType = row.EqpID;
                            rowEntity.UpmActual = ot == 0 ? 0 : ut / ot;
                            rowEntity.UumActual = ut == 0 ? 0 : row.pr / ut;
                            rowEntity.RwkActual = row.rwk / days;
                            rowEntity.EffActual = row.eff / days;
                            rowEntity.PassActual = row.pass / days;
                            rowEntity.ThrActual = row.pr == 0 ? 0 : row.pass / row.pr;
                            bodyEntity.RowEntities.Add(rowEntity);
                        }
                        double ot_all = rowList.Sum(s => s.pr + s.sb + s.sd + s.nd + s.en);
                        double ut_all= rowList.Sum(s => s.pr + s.sb);
                        double pr_all = rowList.Sum(s=>s.pr);
                        bodyEntity.TotalRow.UpmActual = ot_all==0?0:ut_all/ot_all;
                        bodyEntity.TotalRow.UumActual = ut_all==0?0:pr_all/ut_all;
                        bodyEntity.TotalRow.RwkActual = bodyEntity.RowEntities.Average(a => a.RwkActual);
                        bodyEntity.TotalRow.PassActual = bodyEntity.RowEntities.Average(a => a.PassActual);
                        bodyEntity.TotalRow.EffActual = bodyEntity.RowEntities.Average(a => a.EffActual);
                        bodyEntity.TotalRow.ThrActual = pr_all == 0 ? 0 : rowList.Sum(s => s.pass) /pr_all;
                    }
                    TbodyEntities.Add(bodyEntity);
                }
            }
            //获取Target值
            string condition = string.Join("','", list);
            DB2DataCatcher<RPT_EQP_PERFM_TARGET> TargetCatcher = new DB2DataCatcher<RPT_EQP_PERFM_TARGET>("ISTRPT.RPT_EQP_PERFM_TARGET");
            TargetCatcher.Conditions = string.Format("where eqp_type in ('{0}')", condition);
            var tarList = TargetCatcher.GetEntities().EntityList;
            foreach (var item in tarList)
            {
                var body = TbodyEntities.Where(w => w.TotalRow.EqpType == item.Eqp_Type).First();
                body.TotalRow.UpmTarget = item.Upm_Target;
                body.TotalRow.UumTarget = item.Uum_Target;
                body.TotalRow.PassTarget = item.PassQty_Target;
                body.TotalRow.RwkTarget = item.Rework_Target;
                body.TotalRow.EffTarget = item.Eff_Target;
                body.TotalRow.ThrTarget = item.Wph;
                foreach (var row in body.RowEntities)
                {
                    row.UpmTarget = item.Upm_Target;
                    row.UumTarget = item.Uum_Target;
                    row.EffTarget = item.Eff_Target;
                    row.PassTarget = item.PassQty_Target;
                    row.RwkTarget = item.Rework_Target;
                    row.ThrTarget = item.Wph;
                }
            }
        }

        //Main Job
        private void Initialize()
        {
            GetDateFromPostModel();

            GetTbodyEntities();
        }

    }
}