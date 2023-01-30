using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class boworkflowmaster
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? workflowmasterid { get; set; }
        [Column(TypeName = "string")]
        public string description { get; set; }
        [Column(TypeName = "string")]
        public string menucode { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string menucodedesc { get; set; }
        [Column(TypeName = "string")]
        public string tablecode { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string tablecodedesc { get; set; }
        [Column(TypeName = "string")]
        public string workflowhtml { get; set; }
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
        //public virtual ICollection<boworkflow> boworkflows { get; set; }
       // [NotMapped]
        public string Deleted_boworkflow_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [NotMapped]
        public virtual ICollection<boworkflowstep> boworkflowsteps { get; set; }
        [NotMapped]
        public string Deleted_boworkflowstep_IDs { get; set; }

    }
}

