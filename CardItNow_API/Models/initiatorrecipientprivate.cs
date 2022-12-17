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
    public class initiatorrecipientprivate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? privateid { get; set; }
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string customeriddesc { get; set; }
        [Column(TypeName = "string")]
        public string uid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string uiddesc { get; set; }
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
        [Column(TypeName = "int")]
        public int? geoid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string geoiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? cityid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string cityiddesc { get; set; }
        [Column(TypeName = "string")]
        public string pincode { get; set; }
        [Column(TypeName = "string")]
        public string bankaccountnumber { get; set; }
        [Column(TypeName = "string")]
        public string bankname { get; set; }
        [Column(TypeName = "string")]
        public string iban { get; set; }
        [Column(TypeName = "string")]
        public string accountname { get; set; }
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
    public class initiatorrecipientprivateView
    {
public initiatorrecipientprivate data{ get; set; }

    }
}

