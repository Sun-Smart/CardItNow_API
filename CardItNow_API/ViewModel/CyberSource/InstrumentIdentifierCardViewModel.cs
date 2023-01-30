namespace CardItNow.ViewModel
{
    public class CSCreateInstrumentIdentifierCardRequestViewModel
    {
        public string CardNo { get; set; }

    }
    public class CSCreateInstrumentIdentifierCardResponseViewModel
    {
        public string State { get; set; }
        public string CardNoMasked { get; set; }

    }
}
