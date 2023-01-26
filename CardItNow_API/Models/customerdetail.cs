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
    public class customerdetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? customerdetailid { get; set; }
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string customeriddesc { get; set; }
        [Column(TypeName = "string")]
        public string type { get; set; }
        [Column(TypeName = "string")]
        public string uid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string uiddesc { get; set; }
        [Column(TypeName = "string")]
        public string address { get; set; }
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
        public string postalcode { get; set; }
        [Column(TypeName = "int")]
        public int? identificationdocumenttype { get; set; }
        [Column(TypeName = "string")]
        public string idnumber { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? idissuedate { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? idexpirydate { get; set; }
        [Column(TypeName = "string")]
        public string livestockphoto { get; set; }
        [Column(TypeName = "string")]
        public string divmode { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string divmodedesc { get; set; }
        [Column(TypeName = "string")]
        public string divref { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? divsubmissionon { get; set; }
        [Column(TypeName = "string")]
        public string divstatus { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string divstatusdesc { get; set; }
        [Column(TypeName = "string")]
        public string divremarks { get; set; }
        [Column(TypeName = "string")]
        public string amlcheckstatus { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string amlcheckstatusdesc { get; set; }
        [Column(TypeName = "string")]
        public string amlremarks { get; set; }
        [Column(TypeName = "jsonb")]
        public string customfield { get; set; }
        [Column(TypeName = "jsonb")]
        public string attachment { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate { get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate { get; set; }
    }
    public class customerdetailView
    {
        public customerdetail data { get; set; }

    }

    public class ProcessOCRResposeView
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string dob { get; set; }
        public string Issuedate { get; set; }
        public string Expirydate { get; set; }
        public string Email { get; set; }
        public string Documentno { get; set; }
        //public string Referanceno { get; set; }

    }
}

