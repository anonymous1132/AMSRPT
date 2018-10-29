using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.CIM.Caojin.RPTLibrary.Models;

namespace AMS.CIM.Caojin.RPTLibrary
{
    public class RPTContext : DbContext
    {
        public RPTContext() : base("ACEDB")
        { }

        public DbSet<EQP_UPm_018Entity> EQP_UPm_018 { get; set; }

        public DbSet<TimeSplitConfigEntity> TimeSplitConfig { get; set; }

        public DbSet<EQPType_Department_MappingEntity> EQPType_Department_Mapping { get; set; }
    }
}