using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class systemtabletemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? tabledetailid { get; set; }
        [Column(TypeName = "int")]
        public int? tableid { get; set; }
        [Column(TypeName = "int")]
        public int? userroleid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string userroleiddesc { get; set; }
        [Column(TypeName = "string")]
        public string viewhtml { get; set; }
        [Column(TypeName = "string")]
        public string templatehtml { get; set; }
        [Column(TypeName = "string")]
        public string visiblefields { get; set; }
        [Column(TypeName = "string")]
        public string hidefields { get; set; }
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

