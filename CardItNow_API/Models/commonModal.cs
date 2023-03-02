using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardItNow.Models
{
    public class IndividualDocument
    {
        public string documnettype { get; set; }
       // public string documnet_files { get; set; }
    }

    public class BankList
    {
        public  string bankname { get; set; }
    }
    public class purposeList
    {
        public string purpose { get; set; }
    }
    public class sociallogin
    {
        public string email { get; set; }
    }
    public class Savesocial
    {
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string socialid { get; set; }
        public string mediatype { get; set; }
       // public string avatarURL { get; set; }
       public string mobile { get; set; }

        public int geoid { get; set; }
    }

    public class verify_otp
    {
        public string email { get; set; }
        public string otp { get; set; }
    }
    public class changepasscode
    {
        public string email { get; set; }
        //public int customerid { get; set; }
        public string pin { get; set; }
    }

    public class validatepass
    {
        public string email { get; set; }
        public string  passcode { get; set; }

    }

    public class duplicatetransactionvalidation
    {
        [Column(TypeName = "string")]
        //login customer
        public string uid { get; set; }
        //login customer
         public int customerid { get; set; }
        [Column(TypeName = "string")]
        //purpose i.e transaction type

        public string purpose { get; set; }
        //payee i.e muncipality
        public string recipientuid { get; set; }

        
        public int municipality { get; set; }

        [Column(TypeName = "int")]
        public int recipientid { get; set; }

        [Column(TypeName = "string")]

        //provience i.e address
        public string address { get; set; }


        public string province { get; set; }
        public string pin { get; set; }

       

        public string billamount { get; set; }


        public string contractamount { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime startdate { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime enddate { get; set; }
    }
}
