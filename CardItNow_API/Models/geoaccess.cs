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
    public class geoaccess
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? geoaccessid { get; set; }
        [Column(TypeName = "int")]
        public int? geoid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string geoiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? userid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string useriddesc { get; set; }
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
    public class geoaccessView
    {
public geoaccess data{ get; set; }

    }
}

