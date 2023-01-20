using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class boworkflowstep
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? workflowstepid { get; set; }
        [Column(TypeName = "int")]
        public int? workflowmasterid { get; set; }
        [Column(TypeName = "int")]
        public int? stepno { get; set; }
        [Column(TypeName = "string")]
        public string stepname { get; set; }
        [Column(TypeName = "string")]
        public string tat { get; set; }
        [Column(TypeName = "string")]
        public string task { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string taskdesc { get; set; }
        [Column(TypeName = "string")]
        public string condition { get; set; }
        [Column(TypeName = "int")]
        public int? yesstep { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string yesstepdesc { get; set; }
        [Column(TypeName = "int")]
        public int? nostep { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string nostepdesc { get; set; }
        [Column(TypeName = "jsonb")]
        public string approver { get; set; }
        [Column(TypeName = "string")]
        public string workflowuserfieldtype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string workflowuserfieldtypedesc { get; set; }
        [Column(TypeName = "string")]
        public string workflowuserfieldname { get; set; }
        [Column(TypeName = "int")]
        public int? parentid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string parentiddesc { get; set; }
        [Column(TypeName = "bool")]
        public bool? noedittransaction { get; set; }
        [Column(TypeName = "bool")]
        public bool? autoapproval { get; set; }
        [Column(TypeName = "bool")]
        public bool? autodenial { get; set; }
        [Column(TypeName = "string")]
        public string waitduration { get; set; }
        [Column(TypeName = "string")]
        public string remainderduration { get; set; }
        [Column(TypeName = "jsonb")]
        public string escalationuser { get; set; }
        [Column(TypeName = "jsonb")]
        public string cc { get; set; }
        [Column(TypeName = "int")]
        public int? customfieldid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string customfieldiddesc { get; set; }
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
}

