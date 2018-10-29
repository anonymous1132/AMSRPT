using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMS.CIM.Caojin.RPTLibrary.Models
{
    [Table("EQP_UPm_018")]
    public class EQP_UPm_018Entity
    {
        [Key]
        public int EntityID { get; set; }
        
        [Required]
        public string EqpID { get; set; }

        public string EqpType { get; set; }

        public double PRDMin { get; set; }

        public double SBYMin { get; set; }

        public double NSTMin { get; set; }

        public double SDTMin { get; set; }

        public double ENGMin { get; set; }
        
        public double UDTMin { get; set; }

        public double PRDTestMin { get; set; }

        public double PMMin { get; set; }

        public DateTime Date { get; set; }

        public int Passqty { get; set; }

        public int ReworkQty { get; set; }

        public int EffMove { get; set; }
    }
}
