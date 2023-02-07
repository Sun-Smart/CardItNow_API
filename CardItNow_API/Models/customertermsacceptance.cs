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
    public class customertermsacceptance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? customertermid { get; set; }
        [Column(TypeName = "int")]
        public int? termid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string termiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? version { get; set; }
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string customeriddesc { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? dateofacceptance { get; set; }
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
    public class customertermsacceptanceView
    {
public customertermsacceptance data{ get; set; }

    }

    public class Customeracceptanceterms
    {
        public int? customerid { get; set; }
        public int? termid { get; set; }
        public int? version { get; set; }
        public DateTime dateofacceptance { get; set; }
        public string status { get; set; }
    }
}

