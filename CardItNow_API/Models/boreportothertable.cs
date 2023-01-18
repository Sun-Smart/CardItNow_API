using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class boreportothertable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? othertableid { get; set; }
        [Column(TypeName = "int")]
        public int? reportid { get; set; }
        [Column(TypeName = "string")]
        public string tablename { get; set; }
        [Column(TypeName = "string")]
        public string tablealias { get; set; }
        [Column(TypeName = "string")]
        public string jointype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string jointypedesc { get; set; }
        [Column(TypeName = "string")]
        public string wherecondition { get; set; }
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

