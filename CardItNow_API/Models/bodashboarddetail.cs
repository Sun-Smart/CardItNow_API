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
    public class bodashboarddetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? dashboarddetailid { get; set; }
        [Column(TypeName = "int")]
        public int? dashboardid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string dashboardiddesc { get; set; }
        [Column(TypeName = "string")]
        public string dashboardname { get; set; }
        [Column(TypeName = "string")]
        public string title { get; set; }
        [Column(TypeName = "int")]
        public int? row { get; set; }
        [Column(TypeName = "int")]
        public int? col { get; set; }
        [Column(TypeName = "string")]
        public string charttype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string charttypedesc { get; set; }
        [Column(TypeName = "string")]
        public string tablename { get; set; }
        [Column(TypeName = "string")]
        public string recordname { get; set; }
        [Column(TypeName = "string")]
        public string parameter { get; set; }
        [Column(TypeName = "string")]
        public string name { get; set; }
        [Column(TypeName = "string")]
        public string value { get; set; }
        [Column(TypeName = "string")]
        public string parameter1variable { get; set; }
        [Column(TypeName = "int")]
        public int? parameter1type { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string parameter1typedesc { get; set; }
        [Column(TypeName = "string")]
        public string parameter1datetype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string parameter1datetypedesc { get; set; }
        [Column(TypeName = "string")]
        public string parameter2variable { get; set; }
        [Column(TypeName = "int")]
        public int? parameter2type { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string parameter2typedesc { get; set; }
        [Column(TypeName = "string")]
        public string parameter2datetype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string parameter2datetypedesc { get; set; }
        [Column(TypeName = "string")]
        public string parameter3variable { get; set; }
        [Column(TypeName = "int")]
        public int? parameter3type { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string parameter3typedesc { get; set; }
        [Column(TypeName = "string")]
        public string parameter3datetype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string parameter3datetypedesc { get; set; }
        [Column(TypeName = "string")]
        public string backgroundcolor { get; set; }
        [Column(TypeName = "string")]
        public string hoverbackgroundcolor { get; set; }
        [Column(TypeName = "string")]
        public string bordercolor { get; set; }
        [Column(TypeName = "int")]
        public int? menuid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string menuiddesc { get; set; }
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
    public class bodashboarddetailView
    {
public bodashboarddetail data{ get; set; }

    }
}

