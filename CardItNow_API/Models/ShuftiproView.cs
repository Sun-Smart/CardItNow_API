namespace carditnow.Models
{
    public class ShuftiproRequestView
    {
        public string reference { get; set; }
        public string callback_url { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public string verification_mode { get; set; }
      //  public string show_results { get; set; }
        public string manual_review { get; set; }
        public ShuftiproRequestView_document document { get; set; }
        public ShuftiproRequestView()
        {
            this.document = new ShuftiproRequestView_document();
           // this.show_results = "1";
            this.manual_review = "0";
            this.verification_mode = "any";
            this.document.supported_types = new string[] { "id_card", "driving_license", "passport"};
        }

    }
    public class ShuftiproRequestView_document
    {
        public string proof { get; set; }
        public string[] supported_types { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string issue_date { get; set; }
        public string expiry_date { get; set; }
        public string document_number { get; set; }
    }
    public class ShuftiproRequestView_name
    {
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
    }

    public class ShuftiproResponseView
    {
        public string reference { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public ShuftiproResponseView_data verification_data { get; set; }
        public ShuftiproResponseView_result verification_result { get; set; }

    }
    public class ShuftiproResponseView_data
    {
        public ShuftiproResponseView_data_document document { get; set; }
    }
    public class ShuftiproResponseView_result
    {
        public ShuftiproResponseView_result_document document { get; set; }
    }
    public class ShuftiproResponseView_data_document
    {
        public ShuftiproRequestView_name name { get; set; }
        public string dob { get; set; }
        public string expiry_date { get; set; }
        public string issue_date { get; set; }
        public string document_number { get; set; }

    }
    public class ShuftiproResponseView_result_document
    {
        public int name { get; set; }
        public int dob { get; set; }
        public string expiry_date { get; set; }
        public int document_number { get; set; }

    }

    public class  returnResult
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string dob { get; set; }
        public string expirtdate { get; set; }
        public string issuedate { get; set; }
        public string documentno { get; set; }

    }
   
}

