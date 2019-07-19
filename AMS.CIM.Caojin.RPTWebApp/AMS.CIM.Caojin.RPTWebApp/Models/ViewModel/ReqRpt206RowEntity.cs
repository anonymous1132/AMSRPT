using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt206RowEntity
    {
        public string RouteID { get; set; }

        public string OperID { get; set; }

        public string OpeNo { get; set; }

        public string SpecItem { get; set; }

        public string MeasItem { get; set; }

        public string LotID { get; set; }

        public string ProdID { get; set; }

        public List<double?> WaferValue { get; set; } = new List<double?>();

        private double? _ls = null;
        public double? LS { get { return _ls; } set { if (value == 0) _ls = null; else _ls = value; } }

        private double? _lc = null;
        public double? LC { get { return _lc; } set { if (value == 0) _lc = null; else _lc = value; } }

        private double? _target = null;
        public double? Target { get { return _target; } set { if (value == 0) _target = null; else _target = value; } }

        private double? _uc = null;
        public double? UC { get { return _uc; } set { if (value == 0) _uc = null; else _uc = value; } }

        private double? _us = null;
        public double? US { get { return _us; } set { if (value == 0) _us = null; else _us = value; } }

        public string OpeTime { get; set; }
    }
}