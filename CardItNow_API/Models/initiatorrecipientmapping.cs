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
    public class initiatorrecipientmapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? mappingid { get; set; }
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
        public string recipientuid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string recipientuiddesc { get; set; }
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
    public class initiatorrecipientmappingView
    {
public initiatorrecipientmapping data{ get; set; }

    }
}

