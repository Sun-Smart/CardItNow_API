using carditnow.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunSmartnTireProducts.Models
{
    public class bousermastercontext
    {
        [Column(TypeName = "int")]
        public int? companyid { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? userid { get; set; }
        [Column(TypeName = "string")]
        public string sourcefield { get; set; }
        [Column(TypeName = "int")]
        public int? sourcereference { get; set; }
        [Column(TypeName = "int")]
        public int? userroleid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string userroleiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? branchid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string branchiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? departmentid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string departmentiddesc { get; set; }
        [Column(TypeName = "string")]
        public string usercode { get; set; }
        [Column(TypeName = "string")]
        public string username { get; set; }
        [Column(TypeName = "string")]
        public string shortname { get; set; }
        [Column(TypeName = "string")]
        public string bio { get; set; }
        [Column(TypeName = "string")]
        public string avatar { get; set; }
        [Column(TypeName = "int")]
        public int? designation { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string designationdesc { get; set; }
        [Column(TypeName = "int")]
        public int? reportingto { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string reportingtodesc { get; set; }
        [Column(TypeName = "int")]
        public int? role { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string roledesc { get; set; }
        [Column(TypeName = "string")]
        public string emailid { get; set; }
        [Column(TypeName = "string")]
        public string mobilenumber { get; set; }
        [Column(TypeName = "string")]
        public string password { get; set; }
        [Column(TypeName = "bool")]
        public bool? nextloginchangepassword { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? validityfrom { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? validityto { get; set; }
        [Column(TypeName = "int")]
        public int? educationid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string educationiddesc { get; set; }
        [Column(TypeName = "string")]
        public string usersignature { get; set; }
        [Column(TypeName = "string")]
        public string userphoto { get; set; }
        [Column(TypeName = "string")]
        public string thumbnail { get; set; }
        [Column(TypeName = "string")]
        public string emailpassword { get; set; }
        [Column(TypeName = "string")]
        public string emailsignature { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? dateofbirth { get; set; }
        [Column(TypeName = "string")]
        public string defaultpage { get; set; }
        [Column(TypeName = "int")]
        public int? defaultlanguage { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string defaultlanguagedesc { get; set; }
        [Column(TypeName = "string")]
        public string layoutpage { get; set; }
        [Column(TypeName = "string")]
        public string theme { get; set; }
        [Column(TypeName = "string")]
        public string gender { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string genderdesc { get; set; }
        [Column(TypeName = "string")]
        public string nationality { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string nationalitydesc { get; set; }
        [Column(TypeName = "string")]
        public string bloodgroup { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string bloodgroupdesc { get; set; }
        [Column(TypeName = "string")]
        public string religion { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string religiondesc { get; set; }
        [Column(TypeName = "string")]
        public string maritalstatus { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string maritalstatusdesc { get; set; }
        [Column(TypeName = "string")]
        public string referencenumber { get; set; }
        [Column(TypeName = "string")]
        public string address1 { get; set; }
        [Column(TypeName = "string")]
        public string address2 { get; set; }
        [Column(TypeName = "int")]
        public int? countryid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string countryiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? stateid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string stateiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? cityid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string cityiddesc { get; set; }
        [Column(TypeName = "string")]
        public string zipcode { get; set; }
        [Column(TypeName = "string")]
        public string emergencycontactperson { get; set; }
        [Column(TypeName = "string")]
        public string relationship { get; set; }
        [Column(TypeName = "string")]
        public string cpphonenumber { get; set; }
        [Column(TypeName = "bool")]
        public bool? emailnotifications { get; set; }
        [Column(TypeName = "bool")]
        public bool? whatsappnotifications { get; set; }
        [Column(TypeName = "bool")]
        public bool? employeespecificapproval { get; set; }
        [Column(TypeName = "bool")]
        public bool? autoapproval { get; set; }
        [Column(TypeName = "int")]
        public int? approvallevel { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvalleveldesc { get; set; }
        [Column(TypeName = "int")]
        public int? approvallevel1 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvallevel1desc { get; set; }
        [Column(TypeName = "int")]
        public int? approvallevel2 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvallevel2desc { get; set; }
        [Column(TypeName = "int")]
        public int? approvallevel3 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvallevel3desc { get; set; }
        [Column(TypeName = "int")]
        public int? approvallevel4 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvallevel4desc { get; set; }
        [Column(TypeName = "int")]
        public int? approvallevel5 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvallevel5desc { get; set; }
        [Column(TypeName = "string")]
        public string approvalleveltype1 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvalleveltype1desc { get; set; }
        [Column(TypeName = "string")]
        public string approvalleveltype2 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvalleveltype2desc { get; set; }
        [Column(TypeName = "string")]
        public string approvalleveltype3 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvalleveltype3desc { get; set; }
        [Column(TypeName = "string")]
        public string approvalleveltype4 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvalleveltype4desc { get; set; }
        [Column(TypeName = "string")]
        public string approvalleveltype5 { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string approvalleveltype5desc { get; set; }
        [Column(TypeName = "string")]
        public string twitter { get; set; }
        [Column(TypeName = "string")]
        public string facebook { get; set; }
        [Column(TypeName = "string")]
        public string linkedin { get; set; }
        [Column(TypeName = "string")]
        public string skype { get; set; }
        [Column(TypeName = "string")]
        public string googleplus { get; set; }
        [Column(TypeName = "jsonb")]
        public string customfield { get; set; }
        [Column(TypeName = "jsonb")]
        public string attachment { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "int")]
        public int? employeeid { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate { get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate { get; set; }

        [Column(TypeName = "string")]
        public string otpe { get; set; }
        [Column(TypeName = "string")]
        public string otpm { get; set; }
        [Column(TypeName = "string")]
        public string usertype { get; set; }
        [Column(TypeName = "string")]
        public string usercategory { get; set; }
        [Column(TypeName = "string")]
        public string specialcategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [NotMapped]
        public virtual ICollection<bousermenuaccess> bousermenuaccesses { get; set; }
        [NotMapped]
        public string Deleted_bousermenuaccess_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [NotMapped]
        public virtual ICollection<bouserbranchaccess> bouserbranchaccesses { get; set; }
        [NotMapped]
        public string Deleted_bouserbranchaccess_IDs { get; set; }

    }
}

