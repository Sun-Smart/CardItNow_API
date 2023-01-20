using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class columnvisibility
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? columnvisibilityid { get; set; }

        [Column(TypeName = "int")]
        public int? company { get; set; }

        [Column(TypeName = "string")]
        public string tablename { get; set; }

        [Column(TypeName = "string")]
        public string columnname { get; set; }

        [Column(TypeName = "int")]
        public int? usertypeid { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]
        public string usertypeidDesc { get; set; }

        [Column(TypeName = "bool")]
        public bool? show { get; set; }

        [Column(TypeName = "bool")]
        public bool? hide { get; set; }

        [Column(TypeName = "int")]
        public int? createdby { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime? createddate { get; set; }

        [Column(TypeName = "int")]
        public int? updatedby { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime? updateddate { get; set; }

        //[NotMapped]
        //public UserCredential SessionUser { get; set; }
    }
}