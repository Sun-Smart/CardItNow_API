namespace CardItNow.ViewModel
{
    public class CSCreateCustomerRequestViewModel
    {
        public string MerchantCustomerID { get; set; }
        public string CustomerEmail { get; set; }
        public string ClientReferenceInformationCode { get; set; }
        public string MerchantDefinedInformation_Name { get; set; }
        public string MerchantDefinedInformation_Value { get; set; }


    }
    public class CSCreateCustomerResponseViewModel
    {
        public string ClientReferenceInformationCode { get; set; }
        public string CustomerReferenceId { get; set; }
    }
}
