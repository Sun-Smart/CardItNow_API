using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace carditnow.Models
{
    public class avatarmaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? avatarid { get; set; }
        [Column(TypeName = "int")]
        public int? orderid { get; set; }
        [Column(TypeName = "string")]
        public string avatarname { get; set; }
        [Column(TypeName = "string")]
        public string avatarurl { get; set; }
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

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
    public class avatarmasterView
    {
        public avatarmaster data{ get; set; }

    }
    public class avatarUploadRequestViewModel
    {
        public IFormFile ImageFile { get; set; }
    }
    public class avatarUploadRequestViewModelsMobile
    {
        public string ImageFile { get; set; }
        public string customerid { get; set; }
    }

    public class JsonMobile
    {
        public string ImageFile { get; set; }
        public string customerid { get; set; }
    }
    public class avatarUploadList
    {
        public int? avatarid { get; set; }        
        public string avatarname { get; set; }        
        public string avatarurl { get; set; }
    }
}

