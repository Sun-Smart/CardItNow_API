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
    public class transactionmaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? transactionid { get; set; }
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
        [Column(TypeName = "string")]
        public string transactiontype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string transactiontypedesc { get; set; }
        [Column(TypeName = "string")]
        public string recipientname { get; set; }
        [Column(TypeName = "string")]
        public string documentnumber { get; set; }
        [Column(TypeName = "string")]
        public string additionaldocumentnumber { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? startdate { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? expirydate { get; set; }
        [Column(TypeName = "string")]
        public string address { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? billdate { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? contractamount { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? discount { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? carditconvfee { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? payamount { get; set; }
        [Column(TypeName = "int")]
        public int? payid { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string payiddesc { get; set; }
        [Column(TypeName = "string")]
        public string paytype { get; set; }
        [NotMapped]
        [Column(TypeName = "string")]
        public string paytypedesc { get; set; }
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
        //new
        [Column(TypeName = "string")]
        public string pin { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]
        public string uploadDoc { get; set; }
        //[NotMapped]
        //[Column(TypeName = "string")]
        //public string billamount { get; set; }

        [NotMapped]
        [Column(TypeName = "double")]
        public double billamount { get; set; }

        [NotMapped]
        [Column(TypeName = "int")]
        public int CC_carditnowfee { get; set; }


        [NotMapped]
        [Column(TypeName = "int")]
        public int NB_carditnowfee { get; set; }

        [NotMapped]
        [Column(TypeName = "int")]

        public int CC_totalamount { get; set; }


        [NotMapped]
        [Column(TypeName = "int")]
        public int NB_totalamount { get; set; }
    }
    public class transactionmasterView
    {
public transactionmaster data{ get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transactiondetail> transactiondetails { get; set; }
        [NotMapped]
        public string Deleted_transactiondetail_IDs { get; set; }

    }

    public class jobdoc
    {
        public string jobid { get; set; }

        public string uploadDoc { get; set; }
    }

    public class billamountclass
    {

        [NotMapped]
        [Column(TypeName = "string")]
        public string billamount { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]
        public string CC_carditnowfee { get; set; }

        //[NotMapped]
        //[Column(TypeName = "double")]
        //public double CC_carditnowfee1 { get; set; }


        [NotMapped]
        [Column(TypeName = "string")]
        public string NB_carditnowfee { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]

        public string CC_totalamount { get; set; }


        [NotMapped]
        [Column(TypeName = "string")]
        public string NB_totalamount { get; set; }

        public string paystatus { get; set; }

        public string feereason { get; set; }
    }
}

