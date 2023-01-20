using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class customfieldconfiguration
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? customfieldid { get; set; }

        [Column(TypeName = "int")]
        public int? company { get; set; }

        [Column(TypeName = "string")]
        public string tablename { get; set; }

        [Column(TypeName = "string")]
        public string fieldname { get; set; }

        [Column(TypeName = "string")]
        public string fieldtype { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]
        public string fieldtypeDesc { get; set; }

        [Column(TypeName = "string")]
        public string fieldvalues { get; set; }

        [Column(TypeName = "string")]
        public string labelname { get; set; }

        [Column(TypeName = "int")]
        public int? sequence { get; set; }

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

        //[NotMapped]
        //public UserCredential SessionUser { get; set; }
    }
}