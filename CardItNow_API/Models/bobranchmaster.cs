using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class bobranchmaster
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? branchid { get; set; }
        [Column(TypeName = "string")]
        public string branchcode { get; set; }
        [Column(TypeName = "string")]
        public string branchname { get; set; }
        [Column(TypeName = "string")]
        public string thumbnail { get; set; }
        [Column(TypeName = "string")]
        public string address1 { get; set; }
        [Column(TypeName = "string")]
        public string address2 { get; set; }
        [Column(TypeName = "int")]
        public int? countryid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string countryiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? stateid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string stateiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? cityid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string cityiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? locationid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string locationiddesc { get; set; }
        [Column(TypeName = "string")]
        public string pin { get; set; }
        [Column(TypeName = "string")]
        public string latlong { get; set; }
        [Column(TypeName = "TimeSpan")]
        public TimeSpan? starttime { get; set; }
        [Column(TypeName = "TimeSpan")]
        public TimeSpan? endtime { get; set; }
        [Column(TypeName = "int")]
        public int? weekoff1 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string weekoff1desc { get; set; }
        [Column(TypeName = "int")]
        public int? weekoff2 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string weekoff2desc { get; set; }
        [Column(TypeName = "string")]
        public string remarks { get; set; }
        [Column(TypeName = "int")]
        public int? totalregions { get; set; }
        [Column(TypeName = "int")]
        public int? accounts { get; set; }
        [Column(TypeName = "int")]
        public int? salespeople { get; set; }
        [Column(TypeName = "string")]
        public string resourceallocation { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string resourceallocationdesc { get; set; }
        [Column(TypeName = "string")]
        public string growthopportunity { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string growthopportunitydesc { get; set; }
        [Column(TypeName = "int")]
        public int? salesdirector { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string salesdirectordesc { get; set; }
        [Column(TypeName = "int")]
        public int? customersuccessdirector { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string customersuccessdirectordesc { get; set; }
        [Column(TypeName = "jsonb")]
        public string customfield { get; set; }
        [Column(TypeName = "jsonb")]
        public string attachment { get; set; }
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
        //public virtual ICollection<bobranchholiday> bobranchholidays { get; set; }
        //[NotMapped]
        public string Deleted_bobranchholiday_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [NotMapped]
        public virtual ICollection<bouserbranchaccess> bouserbranchaccesses { get; set; }
        [NotMapped]
        public string Deleted_bouserbranchaccess_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [NotMapped]
        //public virtual ICollection<bobranchlocation> bobranchlocations { get; set; }
        //[NotMapped]
        public string Deleted_bobranchlocation_IDs { get; set; }

    }
}

