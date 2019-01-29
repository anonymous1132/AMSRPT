using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt036EqpEntity
    {
        public string EqpID { get; set; }

        public string CurState { get; set; }

        public List<ReqRpt036EqpHistoryEntity> HistoryEntities { get; set; } = new List<ReqRpt036EqpHistoryEntity>();

        public double PR { get {
                if (HistoryEntities.Count == 0) return 0;
                var Pr = HistoryEntities.Where(w => w.E10State == "PRD").Sum(s=>s.DurationSecond);
                return Pr;
            } }

        public double SB { get {
                if (HistoryEntities.Count == 0) return 0;
                var Sb= HistoryEntities.Where(w => w.E10State == "SBY").Sum(s => s.DurationSecond);
                return Sb;
            } }

        public double EN { get {
                if (HistoryEntities.Count == 0) return 0;
                var En = HistoryEntities.Where(w => w.E10State == "ENG").Sum(s => s.DurationSecond);
                return En;
            } }

        public double SD { get {
                if (HistoryEntities.Count == 0) return 0;
                var Sd = HistoryEntities.Where(w => w.E10State == "SDT").Sum(s => s.DurationSecond);
                return Sd;
            } }

        public double UD
        {
            get {
                if (HistoryEntities.Count == 0) return 0;
                var Ud = HistoryEntities.Where(w => w.E10State == "UDT").Sum(s => s.DurationSecond);
                return Ud;
            }
        }

        public double NS {
            get {
                if (HistoryEntities.Count == 0) return 0;
                var Ns = HistoryEntities.Where(w => w.E10State == "NST").Sum(s => s.DurationSecond);
                return Ns;
            }
        }

        public double PT {
            get {
                if (HistoryEntities.Count == 0) return 0;
                var Pt = HistoryEntities.Where(w => w.EqpState=="4600").Sum(s => s.DurationSecond);
                return Pt;
            }
        }

        public double PM {
            get {
                if (HistoryEntities.Count == 0) return 0;
                var Pm = HistoryEntities.Where(w => w.EqpState == "4100" || w.EqpState == "4110"|| w.EqpState == "4300"|| w.EqpState == "4310").Sum(s => s.DurationSecond);
                return Pm;
            }
        }

        public double UP {
            get {
                return PR + SB + EN;
            }
        }

        public Dictionary<string, double> Dic_DetailState { get; set; } = new Dictionary<string, double>();

    }
}