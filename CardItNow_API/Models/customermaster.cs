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
        public string password { get; set; }

        [Column(TypeName = "string")]
        public string tpin { get; set; }
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
        public DateTime? createddate { get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate { get; set; }

        [Column(TypeName = "string")]
        public string otp { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]
        public string geoid { get; set; }

        [Column(TypeName = "bool")]
        public bool customervisible { get; set; }

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

    public class processdocument
    {
        public string email { get; set; }
        public int documenttype { get; set; }
        public string document { get; set; }
        public string documentid { get; set; }
        public string selfie { get; set; }
    }

    public class ProfileInformationUpdate
    {
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string nickname { get; set; }
        public string mobile { get; set; }
        public DateTime dateofbirth { get; set; }
        public string address { get; set; }
        public int geoid { get; set; }
        public int cityid { get; set; }
        public string postalcode { get; set; }
        public DateTime idissuedate { get; set; }
        public DateTime idexpirydate { get; set; }

    }
    public class customermasterView
    {
        public customermaster data { get; set; }
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

    public class customerauth
    {
        public string email { get; set; }
    }

    public class customerlist
    {
        public string email { get; set; }
        public string accescode { get; set; }
    }

    public class suspendAccount
    {
        public int customerid { get; set; }
        //public string customername { get; set; }
        public string passcode { get; set; }

    }
}

