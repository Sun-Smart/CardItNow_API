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
    public class menuaccess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? menuaccessid { get; set; }
        [Column(TypeName = "int")]
        public int? menuid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string menuiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? roleid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string roleiddesc { get; set; }
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
    public class menuaccessView
    {
public menuaccess data{ get; set; }

    }
}

