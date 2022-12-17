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
    public class boreportcolumn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? reportcolumnid { get; set; }
        [Column(TypeName = "int")]
        public int? reportid { get; set; }
        [Column(TypeName = "string")]
        public string tablealias { get; set; }
        [Column(TypeName = "string")]
        public string field { get; set; }
        [Column(TypeName = "string")]
        public string header { get; set; }
        [Column(TypeName = "string")]
        public string columnalias { get; set; }
        [Column(TypeName = "bool")]
        public bool? hide { get; set; }
        [Column(TypeName = "bool")]
        public bool? derived { get; set; }
        [Column(TypeName = "string")]
        public string datatype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string datatypedesc { get; set; }
        [Column(TypeName = "bool")]
        public bool? fkfilter { get; set; }
        [Column(TypeName = "string")]
        public string filtertype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string filtertypedesc { get; set; }
        [Column(TypeName = "int")]
        public int? width { get; set; }
        [Column(TypeName = "bool")]
        public bool? nofilter { get; set; }
        [Column(TypeName = "bool")]
        public bool? groupby { get; set; }
        [Column(TypeName = "bool")]
        public bool? sum { get; set; }
        [Column(TypeName = "bool")]
        public bool? count { get; set; }
        [Column(TypeName = "string")]
        public string colhtml { get; set; }
        [Column(TypeName = "string")]
        public string poptitle { get; set; }
        [Column(TypeName = "bool")]
        public bool? link { get; set; }
        [Column(TypeName = "string")]
        public string linkurl { get; set; }
        [Column(TypeName = "bool")]
        public bool? service { get; set; }
        [Column(TypeName = "string")]
        public string servicename { get; set; }
        [Column(TypeName = "bool")]
        public bool? sp { get; set; }
        [Column(TypeName = "string")]
        public string spname { get; set; }
        [Column(TypeName = "string")]
        public string alert { get; set; }
        [Column(TypeName = "bool")]
        public bool? caps { get; set; }
        [Column(TypeName = "bool")]
        public bool? bold { get; set; }
        [Column(TypeName = "bool")]
        public bool? italic { get; set; }
        [Column(TypeName = "bool")]
        public bool? strikethrough { get; set; }
        [Column(TypeName = "string")]
        public string bgcolor { get; set; }
        [Column(TypeName = "string")]
        public string forecolor { get; set; }
        [Column(TypeName = "string")]
        public string conditionstyle { get; set; }
        [Column(TypeName = "string")]
        public string performancestatusvalues { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "bool")]
        public bool? notsortable { get; set; }
        [Column(TypeName = "int")]
        public int? sequence { get; set; }
        [Column(TypeName = "string")]
        public string sumcondition { get; set; }
        [Column(TypeName = "string")]
        public string countcondition { get; set; }
        [Column(TypeName = "int")]
        public int? min { get; set; }
        [Column(TypeName = "int")]
        public int? max { get; set; }
        [Column(TypeName = "int")]
        public int? maxchars { get; set; }
        [Column(TypeName = "string")]
        public string helptext { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate{ get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate{ get; set; }
    }
    public class boreportcolumnView
    {
public boreportcolumn data{ get; set; }

    }
}

