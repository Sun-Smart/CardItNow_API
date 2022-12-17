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
    public class transactiondetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? transactiondetailid { get; set; }
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
        public int recipientid { get; set; }
        [Column(TypeName = "int")]
        public int? payid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string payiddesc { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? transactiondate { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? transactionamount { get; set; }
        [Column(TypeName = "string")]
        public string remarks { get; set; }
        [Column(TypeName = "string")]
        public string acquirername { get; set; }
        [Column(TypeName = "string")]
        public string transactionconfirmnumber { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? processedon { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? processedamount { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? acquirercharges { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? amountrecipient { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? carditcharges { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? recipientprocessdate { get; set; }
        [Column(TypeName = "string")]
        public string recipientprocesscode { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? carditprocessdate { get; set; }
        [Column(TypeName = "string")]
        public string carditprocesscode { get; set; }
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
    public class transactiondetailView
    {
public transactiondetail data{ get; set; }

    }
}

