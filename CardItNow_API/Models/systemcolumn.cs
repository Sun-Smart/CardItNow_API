using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;

namespace nTireBO.Models
{
    public class systemcolumn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? syscolumnid { get; set; }
        [Column(TypeName = "string")]
        public string tablename { get; set; }
        [Column(TypeName = "string")]
        public string columnname { get; set; }
        [Column(TypeName = "bool")]
        public bool? pk { get; set; }
        [Column(TypeName = "bool")]
        public bool? fk { get; set; }
        [Column(TypeName = "string")]
        public string fktablename { get; set; }
        [Column(TypeName = "string")]
        public string fkidentityid { get; set; }
        [Column(TypeName = "string")]
        public string fkdescription { get; set; }
        [Column(TypeName = "string")]
        public string fkwhere { get; set; }
        [Column(TypeName = "bool")]
        public bool? canshow { get; set; }
        [Column(TypeName = "string")]
        public string reportid { get; set; }
        [Column(TypeName = "string")]
        public string helptext { get; set; }
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
    public class systemcolumnView
    {
public systemcolumn data{ get; set; }

    }
}

