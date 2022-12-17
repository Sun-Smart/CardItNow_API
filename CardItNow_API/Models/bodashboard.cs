using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;
using nTireBO.Models;

namespace nTireBO.Models
{
    public class bodashboard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? dashboardid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string dashboardiddesc { get; set; }
        [Column(TypeName = "string")]
        public string dashboardname { get; set; }
        [Column(TypeName = "int")]
        public int? rows { get; set; }
        [Column(TypeName = "int")]
        public int? cols { get; set; }
        [Column(TypeName = "string")]
        public string design { get; set; }
[Column(TypeName = "jsonb")]
        public string remarks { get; set; }
        [Column(TypeName = "int")]
        public int? userid { get; set; }
        [Column(TypeName = "int")]
        public int? module { get; set; }
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
    public class bodashboardView
    {
public bodashboard data{ get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nTireBO.Models.bodashboarddetail> bodashboarddetails { get; set; }
        [NotMapped]
        public string Deleted_bodashboarddetail_IDs { get; set; }

    }
}

