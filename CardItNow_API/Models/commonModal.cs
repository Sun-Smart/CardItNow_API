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
    }

    public class forgotpass
    {
        public string email { get; set; }
        public string customerid { get; set; }
        public string otp { get; set; }
        public string tpin { get; set; }
    }

}
