using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Models
{
    public class payerpayeeprivate
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? PrivateID { get; set; }

        [Column(TypeName = "int")]
        public int? customerid { get; set; }

        //[Column(TypeName = "string")]
        //public string UID { get; set; }

        [Column(TypeName = "string")]
        public string type { get; set; }

        [Column(TypeName = "string")]
        public string firstname { get; set; }

        [Column(TypeName = "string")]
        public string lastname { get; set; }

        [Column(TypeName = "string")]
        public string email { get; set; }

        [Column(TypeName = "string")]
        public string mobile { get; set; }

        [Column(TypeName = "string")]
        public string geocode { get; set; }


        [Column(TypeName = "string")]
        public string uid { get; set; }

        [Column(TypeName = "int")]
        public int city { get; set; }

        [Column(TypeName = "string")]
        public string pincode { get; set; }

        [Column(TypeName = "string")]
        public string bankaccountnumber { get; set; }

        [Column(TypeName = "string")]
        public string brn { get; set; }
        [Column(TypeName = "string")]
        public string bankname { get; set; }
          

        [Column(TypeName = "string")]
        public string iban { get; set; }

        [Column (TypeName ="string")]
        public string accountname { get; set; }
        [Column(TypeName ="string")]
        public string status { get; set; }

        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate { get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate { get; set; }


        [NotMapped]
        [Column(TypeName = "string")]
        public string customertype { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]
        public string businessname { get; set; }

        [NotMapped]
        [Column(TypeName = "string")]
        public string ContactName { get; set; }


        [NotMapped]
        [Column(TypeName = "boolean")]
        public Boolean visibletoall { get; set; }



    }
}
