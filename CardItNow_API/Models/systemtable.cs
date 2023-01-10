using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace carditnow.Models

{
    public class systemtable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? tableid { get; set; }
        [Column(TypeName = "string")]
        public string tablecode { get; set; }
        [Column(TypeName = "string")]
        public string tablename { get; set; }
        [Column(TypeName = "string")]
        public string insertaction { get; set; }
        [Column(TypeName = "string")]
        public string updateaction { get; set; }
        [Column(TypeName = "string")]
        public string deleteaction { get; set; }
        [Column(TypeName = "bool")]
        public bool? workflow { get; set; }
        [Column(TypeName = "string")]
        public string remindercolorcode { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string remindercolorcodedesc { get; set; }
        [Column(TypeName = "string")]
        public string reminderpriority { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string reminderprioritydesc { get; set; }
        [Column(TypeName = "string")]
        public string remindericon { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string remindericondesc { get; set; }
        [Column(TypeName = "string")]
        public string documentadminusers { get; set; }
        [Column(TypeName = "string")]
        public string documentsecurity { get; set; }
        [Column(TypeName = "string")]
        public string attachmentcategory { get; set; }
        [Column(TypeName = "bool")]
        public bool? noattachmentdelete { get; set; }
        [Column(TypeName = "bool")]
        public bool? audittrailenabled { get; set; }
        [Column(TypeName = "bool")]
        public bool? audittrailview { get; set; }
        [Column(TypeName = "string")]
        public string audittrailfields { get; set; }
        [Column(TypeName = "bool")]
        public bool? versionmaintenance { get; set; }
        [Column(TypeName = "bool")]
        public bool? documentcontrolenabled { get; set; }
        [Column(TypeName = "bool")]
        public bool? documentsharingenabled { get; set; }
        [Column(TypeName = "string")]
        public string fieldstyles { get; set; }
        [Column(TypeName = "string")]
        public string notifyusersoncreation { get; set; }
        [Column(TypeName = "string")]
        public string notifyusersonupdation { get; set; }
        [Column(TypeName = "string")]
        public string notifyusersondeletion { get; set; }
        [Column(TypeName = "string")]
        public string notifyusersonviewing { get; set; }
        [Column(TypeName = "string")]
        public string recordaccesscondition { get; set; }
        [Column(TypeName = "string")]
        public string recordnoaccesscondition { get; set; }
        [Column(TypeName = "string")]
        public string folderview { get; set; }
        [Column(TypeName = "string")]
        public string metatagfields { get; set; }
        [Column(TypeName = "bool")]
        public bool? digitalsignature { get; set; }
        [Column(TypeName = "string")]
        public string viewhtml { get; set; }
        [Column(TypeName = "string")]
        public string templatehtml { get; set; }
        [Column(TypeName = "string")]
        public string helptext { get; set; }
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
        public virtual ICollection<systemtabletemplate> systemtabletemplates { get; set; }
        [NotMapped]
        public string Deleted_systemtabletemplate_IDs { get; set; }

    }
}

