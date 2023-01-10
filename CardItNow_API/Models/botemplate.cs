using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace carditnow.Models
{
    public class botemplate
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? templateid { get; set; }
        [Column(TypeName = "string")]
        public string templatetype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string templatetypedesc { get; set; }
        [Column(TypeName = "string")]
        public string templatecode { get; set; }
        [Column(TypeName = "string")]
        public string templatetext { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate { get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate { get; set; }

    }
}

