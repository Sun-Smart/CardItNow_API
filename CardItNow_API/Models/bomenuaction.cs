using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace carditnow.Models
{
    public class bomenuaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? actionid { get; set; }
        [Column(TypeName = "int")]
        public int? menuid { get; set; }
        [Column(TypeName = "string")]
        public string description { get; set; }
        [Column(TypeName = "string")]
        public string rowselecttype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string rowselecttypedesc { get; set; }
        [Column(TypeName = "string")]
        public string actionicon { get; set; }
        [Column(TypeName = "string")]
        public string actiontype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string actiontypedesc { get; set; }
        [Column(TypeName = "string")]
        public string servicename { get; set; }
        [Column(TypeName = "string")]
        public string actionname { get; set; }
        [Column(TypeName = "string")]
        public string actioncondition { get; set; }
        [Column(TypeName = "bool")]
        public bool? actionbutton { get; set; }
        [Column(TypeName = "string")]
        public string actionbuttonlocation { get; set; }
        [Column(TypeName = "string")]
        public string actionhelp { get; set; }
        [Column(TypeName = "string")]
        public string actionrequestorfield { get; set; }
        [Column(TypeName = "string")]
        public string actionassigneduserfield { get; set; }
        [Column(TypeName = "string")]
        public string notificationtext { get; set; }
        [Column(TypeName = "string")]
        public string actionrequestoremailfield { get; set; }
        [Column(TypeName = "string")]
        public string actionassigneduseremailfield { get; set; }
        [Column(TypeName = "string")]
        public string actionstatus { get; set; }
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

