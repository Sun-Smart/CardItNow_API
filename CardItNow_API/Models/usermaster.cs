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
    public class usermaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? userid { get; set; }
        [Column(TypeName = "string")]
        public string username { get; set; }
        [Column(TypeName = "int")]
        public int? roleid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string roleiddesc { get; set; }
        [Column(TypeName = "string")]
        public string email { get; set; }
        [Column(TypeName = "string")]
        public string emailpassword { get; set; }
        [Column(TypeName = "int")]
        public int? mobile { get; set; }
        [Column(TypeName = "int")]
        public int? basegeoid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string basegeoiddesc { get; set; }
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
    public class usermasterView
    {
        public usermaster data{ get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userrolemaster> userrolemasters { get; set; }
        [NotMapped]
        public string Deleted_userrolemaster_IDs { get; set; }

    }
}

