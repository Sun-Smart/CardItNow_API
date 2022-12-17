using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;

namespace carditnow.Models
{
    public class customermaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [Column(TypeName = "string")]
        public string mode { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string modedesc { get; set; }
        [Column(TypeName = "string")]
        public string uid { get; set; }
        [Column(TypeName = "string")]
        public string type { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string typedesc { get; set; }
        [Column(TypeName = "string")]
        public string firstname { get; set; }
        [Column(TypeName = "string")]
        public string lastname { get; set; }
        [Column(TypeName = "string")]
        public string email { get; set; }
        [Column(TypeName = "string")]
        public string mobile { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? dob { get; set; }
        [Column(TypeName = "string")]
        public string customerinterests { get; set; }
        [Column(TypeName = "int")]
        public int? defaultavatar { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string defaultavatardesc { get; set; }
        [Column(TypeName = "string")]
        public string customerphoto { get; set; }
        [Column(TypeName = "string")]
        public string googleid { get; set; }
        [Column(TypeName = "string")]
        public string facebookid { get; set; }
        [Column(TypeName = "int")]
        public int? lasttermsaccepted { get; set; }
[Column(TypeName = "jsonb")]
        public string customfield { get; set; }
[Column(TypeName = "jsonb")]
        public string attachment { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? deletionaccountrequestedon { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? autodeletedon { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? deleterevokedon { get; set; }

        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate{ get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate{ get; set; }
    }
    public class customermasterView
    {
public customermaster data{ get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customerdetail> customerdetails { get; set; }
        [NotMapped]
        public string Deleted_customerdetail_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customertermsacceptance> customertermsacceptances { get; set; }
        [NotMapped]
        public string Deleted_customertermsacceptance_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customerpaymode> customerpaymodes { get; set; }
        [NotMapped]
        public string Deleted_customerpaymode_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customersecurityquestion> customersecurityquestions { get; set; }
        [NotMapped]
        public string Deleted_customersecurityquestion_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<customersecurityquestionshistory> customersecurityquestionshistories { get; set; }
        [NotMapped]
        public string Deleted_customersecurityquestionshistory_IDs { get; set; }

    }
}

