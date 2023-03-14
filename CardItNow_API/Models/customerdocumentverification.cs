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
    public class customerdocumentverification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? verification_id { get; set; }

        [Column(TypeName = "int")]
        public int customer_id { get; set; }


        [Column(TypeName = "string")]
        public string documentnumber { get; set; }
    
        [Column(TypeName = "string")]
        public string documenttype { get; set; }

        [Column(TypeName = "int")]
        public int payeeid { get; set; }

        [Column(TypeName = "string")]
        public string payeeuid { get; set; }
        [Column(TypeName = "string")]
        public string purpose { get; set; }
        [Column(TypeName = "string")]
        public string pro_address { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime startdate { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime enddate { get; set; }


        [Column(TypeName = "string")]
        public string invoicenumb { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime invocedate { get; set; }



        [Column(TypeName = "string")]
        public string uploadfilename { get; set; }

        [Column(TypeName = "string")]
        public string uploadpath { get; set; }


        [Column(TypeName = "string")]
        public string status { get; set; }



        [Column(TypeName = "string")]
        public string remarks { get; set; }
        
        [Column(TypeName = "int")]

        public int? createdby { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime? createddate { get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? amount { get; set; }




    }

   
   
}

