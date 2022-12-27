using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Models
{
    public class CustomerRegister
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [Column(TypeName = "string")]
        public string mode { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string modedesc { get; set; }
    }
}
