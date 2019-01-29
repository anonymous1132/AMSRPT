using System;
using System.Collections.Generic;
using System.Linq;
using AMS.CIM.Caojin.RPTLibrary.Models;
using AMS.CIM.Caojin.RPTLibrary;
using System.Data.Entity;

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
        private readonly Tuple<DateTime, DateTime> _maxAndMinDate;
        private Tuple<DateTime, DateTime> MaxAndMinDate
        {
            get
            {
                if (_maxAndMinDate != null)
                {
                    return _maxAndMinDate;
                }
                else
                {
                    using (RPTContext db = new RPTContext())
                    {
                        return Tuple.Create<DateTime, DateTime>(db.EQP_UPm_018.Max(m=>m.Date), db.EQP_UPm_018.Min(m=>m.Date));
                    }
                }
            }
        }

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
                //EndDay = StartDay.AddDays(DateTime.DaysInMonth(FromYear,FromMonth)-1);
               EndDay = Convert.ToDateTime(ToYear.ToString() + "/" + ToMonth.ToString() + "/" + DateTime.DaysInMonth(ToYear,ToMonth));
                while (StartDay < EndDay)
                {
                    Dates.Add(StartDay.ToString("yyyy-MM"));
                    StartDay= StartDay.AddMonths(1);
                }
            }
            else if (Type == "frame")
            {
                StartDay = MaxAndMinDate.Item2;
                EndDay= MaxAndMinDate.Item1;
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
            using (RPTContext db = new RPTContext())
            {
                var list = db.EQP_UPm_018.Where(w => w.EqpID == EqpID && DbFunctions.TruncateTime(w.Date) >= firstDay && DbFunctions.TruncateTime(w.Date) <= lastDay).ToList();
                double pr = list.Sum(s => s.PRDMin);
                double ns = list.Sum(s => s.NSTMin);
                double sb = list.Sum(s => s.SBYMin);
                double sd = list.Sum(s => s.SDTMin);
                double ud = list.Sum(s => s.UDTMin);
                double en = list.Sum(s => s.ENGMin);
                double ot = pr + sb + sd + ud + en;
                double ut = pr + sb;
                double pdtest = list.Sum(s=>s.PRDTestMin);
                double pm = list.Sum(s=>s.PMMin);
                if (ot + ns > 0)
                {
                    entity.UPm = ot == 0 ? 0 : (pr + sb) / ot;
                    entity.UUm = ut == 0 ? 0 : pr / ut;
                    entity.SD = ot == 0 ? 0 : sd / ot;
                    entity.UD = ot == 0 ? 0 : ud / ot;
                    entity.PRDHour = pr / 60;
                    entity.SBYHour = sb / 60;
                    entity.SDTHour = sd / 60;
                    entity.UDTHour = ud / 60;
                    entity.NSTHour = ns / 60;
                    entity.ENGHour = en / 60;
                    entity.PRDTestHour = pdtest / 60;
                    entity.PMHour = pm / 60;
                }
                return entity;
            }
 
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
                        data.Date = date;
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
                        data.Date = date;
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
                        data.Date = date;
                        entity.Datas.Add(data);
                    }
                    entities.Add(entity);
                }
            }

        }

       
    }
}