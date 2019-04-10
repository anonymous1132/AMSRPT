using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt028ChartEntity
    {
        public string Stage { get; set; }

        public string OpeNo { get; set; }

        public string Step { get; set; }

        public List<ReqRpt028LotEntity> LotEntities { get; set; } = new List<ReqRpt028LotEntity>();

        public double RemainCT { get; set; }

        public int Wip { get; set; }

        public int GetCurWip()
        { return LotEntities.Any() ? LotEntities.Sum(s => s.Qty) : 0; }
    }
}