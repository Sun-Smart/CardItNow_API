using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class bodynamicformdetail
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }
        [Column(TypeName = "int")]
        public int? tableid { get; set; }
        [Column(TypeName = "string")]
        public string tableiddesc { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? formdetailid { get; set; }
        [Column(TypeName = "int")]
        public int? formid { get; set; }
        [Column(TypeName = "string")]
        public string fieldname { get; set; }
        [Column(TypeName = "string")]
        public string controltype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string controltypedesc { get; set; }
        [Column(TypeName = "bool")]
        public bool? required { get; set; }
        [Column(TypeName = "bool")]
        public bool? fk { get; set; }
        [Column(TypeName = "int")]
        public int? sequence { get; set; }
        [Column(TypeName = "string")]
        public string configurations { get; set; }
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

