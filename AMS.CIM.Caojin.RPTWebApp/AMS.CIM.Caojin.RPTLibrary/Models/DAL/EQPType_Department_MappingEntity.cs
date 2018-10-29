using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    [Table("EQPType_Department_Mapping")]
    public class EQPType_Department_MappingEntity
    {
        [Key]
        public int EntityID { get; set; }

        [Required]
        public string EqpType { get; set; }

        public string Department { get; set; }
    }
}
