using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class bodynamicform
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }
        [Column(TypeName = "int")]
        public int? tableid { get; set; }
        [Column(TypeName = "string")]
        public string tableiddesc { get; set; }
        [Column(TypeName = "string")]
        public string conditionfield { get; set; }
        [Column(TypeName = "string")]
        public string conditionvalue { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? formid { get; set; }
        [Column(TypeName = "string")]
        public string formname { get; set; }
        [Column(TypeName = "string")]
        public string formtype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string formtypedesc { get; set; }
        [Column(TypeName = "string")]
        public string formhtml { get; set; }
        [Column(TypeName = "int")]
        public int? cols { get; set; }
        [Column(TypeName = "string")]
        public string templatehtml { get; set; }
        [Column(TypeName = "bool")]
        public bool? hasattachments { get; set; }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [NotMapped]
        public virtual ICollection<bodynamicformdetail> bodynamicformdetails { get; set; }
        [NotMapped]
        public string Deleted_bodynamicformdetail_IDs { get; set; }

    }
}

