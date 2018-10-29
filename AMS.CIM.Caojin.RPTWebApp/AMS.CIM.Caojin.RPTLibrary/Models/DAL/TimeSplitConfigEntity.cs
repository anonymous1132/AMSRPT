using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    [Table("TimeSplitConfig")]
    public class TimeSplitConfigEntity
    {
        [Key]
        public int EntityID { get; set; }

        [Required]
        public string RptID { get; set; }

        [Required]
        public string TimeValue { get; set; }
    }
}
