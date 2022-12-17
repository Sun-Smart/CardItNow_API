using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;
using nTireBO.Models;

namespace nTireBO.Models
{
    public class boreport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? reportid { get; set; }
        [Column(TypeName = "string")]
        public string reportcode { get; set; }
        [Column(TypeName = "string")]
        public string reportname { get; set; }
        [Column(TypeName = "string")]
        public string reportmodule { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string reportmoduledesc { get; set; }
        [Column(TypeName = "string")]
        public string actionkey { get; set; }
        [Column(TypeName = "string")]
        public string reporttype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string reporttypedesc { get; set; }
        [Column(TypeName = "string")]
        public string columns { get; set; }
        [Column(TypeName = "bool")]
        public bool? sidefilter { get; set; }
        [Column(TypeName = "string")]
        public string sidefiltertype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string sidefiltertypedesc { get; set; }
        [Column(TypeName = "string")]
        public string sidefilters { get; set; }
        [Column(TypeName = "string")]
        public string maintablename { get; set; }
        [Column(TypeName = "string")]
        public string maintablealias { get; set; }
        [Column(TypeName = "string")]
        public string maintableidentityfield { get; set; }
        [Column(TypeName = "string")]
        public string pk { get; set; }
        [Column(TypeName = "string")]
        public string query { get; set; }
        [Column(TypeName = "string")]
        public string postquery { get; set; }
        [Column(TypeName = "string")]
        public string wherecondition { get; set; }
        [Column(TypeName = "bool")]
        public bool? cardtype { get; set; }
        [Column(TypeName = "string")]
        public string html { get; set; }
        [Column(TypeName = "bool")]
        public bool? calendar { get; set; }
        [Column(TypeName = "bool")]
        public bool? kanbanview { get; set; }
        [Column(TypeName = "string")]
        public string kanbankey { get; set; }
        [Column(TypeName = "bool")]
        public bool? datefilter { get; set; }
        [Column(TypeName = "string")]
        public string datefiltercolumnname { get; set; }
        [Column(TypeName = "string")]
        public string datefiltertype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string datefiltertypedesc { get; set; }
        [Column(TypeName = "string")]
        public string groupby { get; set; }
        [Column(TypeName = "string")]
        public string groupbytext { get; set; }
        [Column(TypeName = "string")]
        public string groupby2 { get; set; }
        [Column(TypeName = "string")]
        public string groupby2text { get; set; }
        [Column(TypeName = "string")]
        public string groupbyrelationship { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string groupbyrelationshipdesc { get; set; }
        [Column(TypeName = "string")]
        public string sortby1 { get; set; }
        [Column(TypeName = "string")]
        public string sortby2 { get; set; }
        [Column(TypeName = "string")]
        public string sortby3 { get; set; }
        [Column(TypeName = "string")]
        public string parentid { get; set; }
        [Column(TypeName = "string")]
        public string parentdescription { get; set; }
        [Column(TypeName = "string")]
        public string detailtablename { get; set; }
        [Column(TypeName = "string")]
        public string detailtablealias { get; set; }
        [Column(TypeName = "string")]
        public string jointype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string jointypedesc { get; set; }
        [Column(TypeName = "string")]
        public string detailtableidentityfield { get; set; }
        [Column(TypeName = "string")]
        public string detailtablefk { get; set; }
        [Column(TypeName = "bool")]
        public bool? detailtableconcatenate { get; set; }
        [Column(TypeName = "string")]
        public string detailtableheader { get; set; }
        [Column(TypeName = "string")]
        public string detailtablefooter { get; set; }
        [Column(TypeName = "string")]
        public string detailtablequery { get; set; }
        [Column(TypeName = "string")]
        public string masterdetailwhere { get; set; }
        [Column(TypeName = "int")]
        public int? numrows { get; set; }
        [Column(TypeName = "string")]
        public string reportoutputtype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string reportoutputtypedesc { get; set; }
        [Column(TypeName = "bool")]
        public bool? noheader { get; set; }
        [Column(TypeName = "string")]
        public string header { get; set; }
        [Column(TypeName = "string")]
        public string footer { get; set; }
        [Column(TypeName = "string")]
        public string headerquery { get; set; }
        [Column(TypeName = "string")]
        public string footerquery { get; set; }
        [Column(TypeName = "string")]
        public string headerquery1 { get; set; }
        [Column(TypeName = "string")]
        public string footerquery1 { get; set; }
        [Column(TypeName = "string")]
        public string headerquery2 { get; set; }
        [Column(TypeName = "string")]
        public string footerquery2 { get; set; }
        [Column(TypeName = "string")]
        public string headerquery3 { get; set; }
        [Column(TypeName = "string")]
        public string footerquery3 { get; set; }
        [Column(TypeName = "string")]
        public string headerquery4 { get; set; }
        [Column(TypeName = "string")]
        public string footerquery4 { get; set; }
        [Column(TypeName = "string")]
        public string headerquery5 { get; set; }
        [Column(TypeName = "string")]
        public string footerquery5 { get; set; }
        [Column(TypeName = "string")]
        public string header1 { get; set; }
        [Column(TypeName = "string")]
        public string footer1 { get; set; }
        [Column(TypeName = "string")]
        public string header2 { get; set; }
        [Column(TypeName = "string")]
        public string footer2 { get; set; }
        [Column(TypeName = "string")]
        public string header3 { get; set; }
        [Column(TypeName = "string")]
        public string footer3 { get; set; }
        [Column(TypeName = "string")]
        public string header4 { get; set; }
        [Column(TypeName = "string")]
        public string footer4 { get; set; }
        [Column(TypeName = "string")]
        public string header5 { get; set; }
        [Column(TypeName = "string")]
        public string footer5 { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "string")]
        public string css { get; set; }
        [Column(TypeName = "string")]
        public string viewhtmltype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string viewhtmltypedesc { get; set; }
        [Column(TypeName = "string")]
        public string viewhtml { get; set; }
        [Column(TypeName = "string")]
        public string viewcss { get; set; }
        [Column(TypeName = "string")]
        public string reporthtml { get; set; }
        [Column(TypeName = "string")]
        public string workflowhtmltype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string workflowhtmltypedesc { get; set; }
        [Column(TypeName = "string")]
        public string workflowhtml { get; set; }
        [Column(TypeName = "string")]
        public string component { get; set; }
        [Column(TypeName = "string")]
        public string alternateview { get; set; }
        [Column(TypeName = "string")]
        public string recordtype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string recordtypedesc { get; set; }
        [Column(TypeName = "string")]
        public string userfield { get; set; }
        [Column(TypeName = "string")]
        public string employeefield { get; set; }
        [Column(TypeName = "string")]
        public string userfiltertype { get; set; }
        [Column(TypeName = "string")]
        public string rolefield { get; set; }
        [Column(TypeName = "int")]
        public int? dashboardid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string dashboardiddesc { get; set; }
        [Column(TypeName = "string")]
        public string tableheader { get; set; }
        [Column(TypeName = "string")]
        public string reportjsondata { get; set; }
        [Column(TypeName = "string")]
        public string helptext { get; set; }
[Column(TypeName = "jsonb")]
        public string filters { get; set; }
[Column(TypeName = "jsonb")]
        public string filtercolumns { get; set; }
        [Column(TypeName = "string")]
        public string groupbyfooter { get; set; }
        [Column(TypeName = "string")]
        public string email { get; set; }
        [Column(TypeName = "string")]
        public string schedule { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string scheduledesc { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? nextschedule { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate{ get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate{ get; set; }
    }
    public class boreportView
    {
public boreport data{ get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nTireBO.Models.boreportdetail> boreportdetails { get; set; }
        [NotMapped]
        public string Deleted_boreportdetail_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nTireBO.Models.boreportothertable> boreportothertables { get; set; }
        [NotMapped]
        public string Deleted_boreportothertable_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nTireBO.Models.boreportcolumn> boreportcolumns { get; set; }
        [NotMapped]
        public string Deleted_boreportcolumn_IDs { get; set; }

    }
}

