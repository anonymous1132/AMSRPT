using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt004TableGroupEntity
    {
        public List<ReqRpt004TableEntity> TableEntities { get; set; } = new List<ReqRpt004TableEntity>();

        public int EffectiveSteps { get; set; } = 0;

        public string ProductID { get; set; }

        private double TotalTurn { get { return TableEntities.Sum(s => s.WIP) == 0 ? 0 : TableEntities.Sum(s => s.Move) / TableEntities.Sum(s => s.WIP); } }

        public string StrTotalTurn { get { return TotalTurn.ToString("0.00"); } }
    }
}