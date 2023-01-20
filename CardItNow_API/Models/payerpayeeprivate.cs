using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Models
{
    public class payerpayeeprivate
    {
        [Column(TypeName = "int")]
        public int? customerid { get; set; }
        
        [Column(TypeName = "string")]
        public string firstname { get; set; }
        [Column(TypeName = "string")]
        public string email { get; set; }

        [Column(TypeName = "string")]
        public string businessregnumber { get; set; }

        [Column(TypeName = "string")]
        public string mobile { get; set; }

        [Column(TypeName = "string")]
        public string bankname { get; set; }

        [Column(TypeName = "string")]
        public string accountnumber { get; set; }

        [Column(TypeName = "string")]
        public string swiftcode { get; set; }

        [Column (TypeName ="string")]
        public string documnettype { get; set; }
        [Column(TypeName ="string")]
        public string documentvalue { get; set; }


    }
}
