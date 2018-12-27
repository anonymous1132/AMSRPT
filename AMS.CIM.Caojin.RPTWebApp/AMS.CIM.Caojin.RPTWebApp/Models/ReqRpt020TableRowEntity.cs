using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt020TableRowEntity
    {
        public string EqpType { get; set; }

        public double UpmTarget { get; set; } = 0;

        public double UpmActual { get; set; } = 0;

        public double UpmDelta { get { return UpmActual - UpmTarget; } }

        public double UumTarget { get; set; } = 0;

        public double UumActual { get; set; } = 0;

        public double UumDelta { get { return UumActual - UumTarget; } }

        public double PassTarget { get; set; } = 0;

        public double PassActual { get; set; } = 0;

        public double PassDelta { get { return PassTarget == 0 ? 0 : (PassActual - PassTarget) / PassTarget; } }

        public double RwkTarget { get; set; } = 0;

        public double RwkActual { get; set; } = 0;

        public double RwkDelta { get { return RwkTarget == 0 ? 0 : (RwkActual - RwkTarget) / RwkTarget; } }

        public double EffTarget { get; set; } = 0;

        public double EffActual { get; set; } = 0;

        public double EffDelta { get { return EffTarget == 0 ? 0 : (EffActual - EffTarget) / EffTarget; } }

        public double ThrTarget { get; set; } = 0;

        public double ThrActual { get; set; } = 0;

        public double ThrDelta { get { return ThrTarget == 0 ? 0 : (ThrActual - ThrTarget) / ThrTarget; } }
    }
}