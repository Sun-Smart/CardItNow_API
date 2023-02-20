namespace CardItNow.ViewModel
{
    public class CSCreateInstrumentIdentifierCardRequestViewModel
    {
        public string CardNo { get; set; }
        //shy
        //public string expierymonth { get; set; }

        //public string expieryyear { get; set; }

        //public string securitycode { get; set; }

    }
    public class CSCreateInstrumentIdentifierCardResponseViewModel
    {
        public string State { get; set; }
        public string CardNoMasked { get; set; }

    }
}
