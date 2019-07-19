using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt209RowEntity
    {
        public string LotID { get; set; }

        public string ProdID { get; set; }

        public string FoupID { get; set; }

        public int Qty { get; set; }

        public string RouteID { get; set; }

        public string OperID { get; set; }

        public string EqpType { get; set; }

        public string EqpID { get; set; }

        public string RecipeID { get; set; }

        public string OpeNo { get; set; }

        public string SpecItem { get; set; }

        public string MeasItem { get; set; }

        public string WaferID { get; set; }

        public List<double?> SiteValue { get; set; } = new List<double?>();

        public double WaferMean { get; set; }

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

        public double? MeanRange { get; set; } = null;

        private double? _rangeUc = null;
        public double? RangeUC { get { return _rangeUc; } set { if (value == 0) _rangeUc = null; else _rangeUc = value; } }

        public string OpeTime { get; set; }
    }
}