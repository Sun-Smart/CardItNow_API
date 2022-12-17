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
    public class customerpaymode
    {
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [Column(TypeName = "string")]
        public string uid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string uiddesc { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? payid { get; set; }
        [Column(TypeName = "string")]
        public string cardnumber { get; set; }
        [Column(TypeName = "string")]
        public string cardname { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? expirydate { get; set; }
        [Column(TypeName = "string")]
        public string bankname { get; set; }
        [Column(TypeName = "string")]
        public string ibannumber { get; set; }
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
    public class customerpaymodeView
    {
public customerpaymode data{ get; set; }

    }
}

