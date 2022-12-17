using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;

namespace nTireBO.Models
{
    public class boconfigvalue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? configid { get; set; }
        [Column(TypeName = "string")]
        public string param { get; set; }
        [Column(TypeName = "string")]
        public string configkey { get; set; }
        [Column(TypeName = "string")]
        public string configtext { get; set; }
        [Column(TypeName = "int")]
        public int? orderno { get; set; }
        [Column(TypeName = "string")]
        public string htmlcode { get; set; }
        [Column(TypeName = "string")]
        public string param1 { get; set; }
        [Column(TypeName = "string")]
        public string param2 { get; set; }
        [Column(TypeName = "string")]
        public string helptext { get; set; }
        [Column(TypeName = "string")]
        public string flag { get; set; }
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
    public class boconfigvalueView
    {
public boconfigvalue data{ get; set; }

    }
}

