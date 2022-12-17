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
    public class transactionitemdetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? transactionitemdetailid { get; set; }
        [Column(TypeName = "int")]
        public int? transactiondetailid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string transactiondetailiddesc { get; set; }
        [Column(TypeName = "int")]
        public int? transactionid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string transactioniddesc { get; set; }
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
        [Column(TypeName = "int")]
        public int? recipientid { get; set; }
        [Column(TypeName = "int")]
        public int? payid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string payiddesc { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? av { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? period { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? basic { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? dp { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? netbasic { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? sef { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? sdp { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? netsef { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? total { get; set; }
[Column(TypeName = "jsonb")]
        public string customfield { get; set; }
[Column(TypeName = "jsonb")]
        public string attachment { get; set; }
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
    public class transactionitemdetailView
    {
public transactionitemdetail data{ get; set; }

    }
}

