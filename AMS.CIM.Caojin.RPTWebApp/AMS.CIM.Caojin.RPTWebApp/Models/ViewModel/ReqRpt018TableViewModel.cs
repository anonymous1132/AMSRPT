using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018TableViewModel
    {
        public ReqRpt018TableViewModel(ReqRpt018PostViewModel postViewModel)
        {
            Type = postViewModel.type;
            GetEqIDs(postViewModel.selectedeqpid);
            Frame = postViewModel.frame;
            GetDateList(postViewModel.from, postViewModel.to);
            selectedeqptype = postViewModel.eqptype;
            GetEntitiesByType();
        }
        public List<ReqRpt018TableEntity> entities=new List<ReqRpt018TableEntity>();

        public List<string> Dates = new List<string>();
        public List<string> EqpIDs;
        private DateTime StartDay;
        private DateTime EndDay;
        private string Type;
        private string Frame;
        private string selectedeqptype;
        public string querycontent
        {
            get { return selectedeqptype + " " + Dates.FirstOrDefault() + "~ " + Dates.LastOrDefault() + " " + " UPm(%);UUm(%);SD(%);UD(%)"; }
        }

        private void GetDateList(string From, string To)
        {
            if (Type == "date")
            {
                StartDay = Convert.ToDateTime(From);
                EndDay = Convert.ToDateTime(To);
                while (StartDay <= EndDay)
                {
                    Dates.Add(StartDay.ToString("yyyyMMdd"));
                    StartDay= StartDay.AddDays(1);
                }
            }
            else if (Type == "week")
            {
                string result = System.Text.RegularExpressions.Regex.Replace(From, @"[^0-9]+", "");
                int FromYear = Convert.ToInt16(result.Substring(0, 4));
                int FromWeek = Convert.ToInt16(result.Substring(4));
                result = System.Text.RegularExpressions.Regex.Replace(To, @"[^0-9]+", "");
                int ToYear = Convert.ToInt16(result.Substring(0, 4));
                int ToWeek = Convert.ToInt16(result.Substring(4));
                StartDay = GetFirstEndDayOfWeek(FromYear, FromWeek, System.Globalization.CultureInfo.GetCultureInfo("zh-cn")).Item1;
                EndDay = GetFirstEndDayOfWeek(ToYear, ToWeek, System.Globalization.CultureInfo.GetCultureInfo("zh-cn")).Item2;
                while (StartDay < EndDay)
                {
                    Dates.Add(StartDay.Year.ToString() + "WK" + GetWeekOfYear(StartDay).ToString());
                    StartDay= StartDay.AddDays(7);
                }
            }
            else if (Type == "month")
            {
                string result = System.Text.RegularExpressions.Regex.Replace(From, @"[^0-9]+", "");
                int FromYear = Convert.ToInt16(result.Substring(0, 4));
                int FromMonth = Convert.ToInt16(result.Substring(4));
                result = System.Text.RegularExpressions.Regex.Replace(To, @"[^0-9]+", "");
                int ToYear = Convert.ToInt16(result.Substring(0, 4));
                int ToMonth = Convert.ToInt16(result.Substring(4));
                StartDay = Convert.ToDateTime(FromYear.ToString() + "/" + FromMonth.ToString() + "/" + 1.ToString());
                EndDay = Convert.ToDateTime(ToYear.ToString() + "/" + (ToMonth+1).ToString() + "/" + 1.ToString()).AddDays(-1);
                while (StartDay < EndDay)
                {
                    Dates.Add(StartDay.ToString("yyyy-MM"));
                    StartDay= StartDay.AddMonths(1);
                }
            }
            else if (Type == "frame")
            {
                StartDay = ShareDataEntity.GetSingleEntity().Rpt018.Rpt018GroupModel.ReqRpt018Models.Min(m => m.SomeDay);
                EndDay= ShareDataEntity.GetSingleEntity().Rpt018.Rpt018GroupModel.ReqRpt018Models.Max(m => m.SomeDay);
                if (Frame == "date")
                {
                    while (StartDay <= EndDay)
                    {
                        Dates.Add(StartDay.ToString("yyyyMMdd"));
                        StartDay= StartDay.AddDays(1);
                    }
                }
                else if (Frame == "week")
                {
                    int FromYear = StartDay.Year;
                    int FromWeek = GetWeekOfYear(StartDay);
                    int ToYear = EndDay.Year;
                    int ToWeek= GetWeekOfYear(StartDay);
                    StartDay = GetFirstEndDayOfWeek(FromYear, FromWeek, System.Globalization.CultureInfo.GetCultureInfo("zh-cn")).Item1;

                    while (StartDay <= EndDay)
                    {
                        Dates.Add(StartDay.Year.ToString() + "WK" + GetWeekOfYear(StartDay).ToString());
                        StartDay= StartDay.AddDays(7);
                    }
                }
                else if (Frame == "month")
                {
                    int FromYear = StartDay.Year;
                    int FromMonth = StartDay.Month;
                    int ToYear = EndDay.Year;
                    int ToMonth = EndDay.Month;
                    StartDay= Convert.ToDateTime(FromYear.ToString() + "/" + FromMonth.ToString() + "/" + 1.ToString());
                    while (StartDay <= EndDay)
                    {
                        Dates.Add(StartDay.ToString("yyyy-MM"));
                        StartDay= StartDay.AddMonths(1);
                    }
                }
            }
        }

        private void GetEqIDs(string strEqp)
        {
            EqpIDs = strEqp.Split(',').ToList();
        }

        public static Tuple<DateTime, DateTime> GetFirstEndDayOfWeek(int year, int weekNumber, System.Globalization.CultureInfo culture)
        {
            System.Globalization.Calendar calendar = culture.Calendar;
            DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
            DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber - 1);
            DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

            while (targetDay.DayOfWeek != firstDayOfWeek)
            {
                targetDay = targetDay.AddDays(-1);
            }

            return Tuple.Create<DateTime, DateTime>(targetDay, targetDay.AddDays(6));
        }

        public static int GetWeekOfYear(DateTime dt)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        private ReqRpt018DataBase GetEntityByDayRange(string EqpID, DateTime firstDay, DateTime lastDay)
        {
            ReqRpt018DataBase entity = new ReqRpt018DataBase();
            var ssd = ShareDataEntity.GetSingleEntity().Rpt018.Rpt018GroupModel.ReqRpt018Models;
            var list= ShareDataEntity.GetSingleEntity().Rpt018.Rpt018GroupModel.ReqRpt018Models.Where(w => w.EqpID == EqpID && w.SomeDay.Date >= firstDay && w.SomeDay.Date <= lastDay).ToList();
            double pr = list.Sum(s=>s.PRDTimeSpan);
            double ns = list.Sum(s=>s.NSTTimeSpan);
            double sb = list.Sum(s=>s.SBYTimeSpan);
            double sd = list.Sum(s=>s.SDTTimeSpan);
            double ud = list.Sum(s=>s.UDTTimeSpan);
            double en = list.Sum(s=>s.ENGTimeSpan);
            double ot = pr + sb + sd + ud + en;
            double ut = pr + sb;
            if(ot+ns>0)
            {
                entity.UPm = ot == 0 ? 0 : (pr + sb) / ot;
                entity.UUm = ut == 0 ? 0 : pr / ut;
                entity.SD = ot == 0?0: sd / ot;
                entity.UD = ot == 0?0:ud / ot;
            }

            return entity;   
        }

        private void GetEntitiesByType()
        {
            if (Type == "date"||(Type=="frame"&&Frame=="date"))
            {
                foreach (string id in EqpIDs)
                {
                    ReqRpt018TableEntity entity = new ReqRpt018TableEntity();
                    entity.EqpID = id;
                    foreach (string date in Dates)
                    {
                        DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                        ReqRpt018DataBase data = GetEntityByDayRange(id,dt,dt);
                        entity.Datas.Add(data);
                    }
                    entities.Add(entity);
                }
            }
            if (Type == "week"|| (Type == "frame" && Frame == "week"))
            {
                foreach (string id in EqpIDs)
                {
                    ReqRpt018TableEntity entity = new ReqRpt018TableEntity();
                    entity.EqpID = id;
                    foreach (string date in Dates)
                    {
                        int year = Convert.ToInt16(date.Substring(0,4));
                        int week = Convert.ToInt16(date.Substring(6));
                        Tuple<DateTime, DateTime> dts = GetFirstEndDayOfWeek(year, week, System.Globalization.CultureInfo.GetCultureInfo("zh-cn"));
                        DateTime sdt = dts.Item1;
                        DateTime edt = dts.Item2;
                        ReqRpt018DataBase data = GetEntityByDayRange(id, sdt, edt);
                        entity.Datas.Add(data);
                    }
                    entities.Add(entity);
                }
            }
            if (Type == "month" || (Type == "frame" && Frame == "month"))
            {
                foreach (string id in EqpIDs)
                {
                    ReqRpt018TableEntity entity = new ReqRpt018TableEntity();
                    entity.EqpID = id;
                    foreach (string date in Dates)
                    {
                        DateTime sdt = Convert.ToDateTime(date.Replace('-','/')+"/01");
                        DateTime edt = sdt.AddMonths(1).AddDays(-1);
                        ReqRpt018DataBase data = GetEntityByDayRange(id, sdt, edt);
                        entity.Datas.Add(data);
                    }
                    entities.Add(entity);
                }
            }

        }

       
    }
}