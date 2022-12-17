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
    public class customersecurityquestionshistory
    {
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string customeriddesc { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? historyid { get; set; }
        [Column(TypeName = "int")]
        public int? securityquestionid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string securityquestioniddesc { get; set; }
        [Column(TypeName = "int")]
        public int? questionid { get; set; }
        [Column(TypeName = "string")]
        public string oldanswer { get; set; }
        [Column(TypeName = "string")]
        public string newanswer { get; set; }
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
    public class customersecurityquestionshistoryView
    {
public customersecurityquestionshistory data{ get; set; }

    }
}

