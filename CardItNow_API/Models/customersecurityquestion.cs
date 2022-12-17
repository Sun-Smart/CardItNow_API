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
    public class customersecurityquestion
    {
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string customeriddesc { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? securityquestionid { get; set; }
        [Column(TypeName = "int")]
        public int? questionid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string questioniddesc { get; set; }
        [Column(TypeName = "string")]
        public string answer { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? deletedon { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate{ get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate{ get; set; }
    }
    public class customersecurityquestionView
    {
public customersecurityquestion data{ get; set; }

    }
}

