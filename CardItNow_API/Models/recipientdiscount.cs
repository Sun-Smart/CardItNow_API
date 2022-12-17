using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;

namespace carditnow.Models
{
    public class recipientdiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? discountid { get; set; }
        [Column(TypeName = "string")]
        public string recipientuid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string recipientuiddesc { get; set; }
        [Column(TypeName = "string")]
        public string initiatoruid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string initiatoruiddesc { get; set; }
        [Column(TypeName = "string")]
        public string contractnumber { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? discountpercentage { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate{ get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate{ get; set; }
    }
    public class recipientdiscountView
    {
public recipientdiscount data{ get; set; }

    }
}

