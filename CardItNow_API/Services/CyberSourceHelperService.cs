using CardItNow.ViewModel;
using CyberSource.Api;
using CyberSource.Model;
using System;
using System.Collections.Generic;

namespace CardItNow.Services
{
    public class CyberSourceHelperService
    {
        public CSCreateCustomerResponseViewModel CreateCustomer(CSCreateCustomerRequestViewModel model)
        {
            CSCreateCustomerResponseViewModel response = new CSCreateCustomerResponseViewModel();
          //  string buyerInformationMerchantCustomerID = "Your customer identifier";
          //  string buyerInformationEmail = "test@cybs.com";
            Tmsv2customersBuyerInformation buyerInformation = new Tmsv2customersBuyerInformation(
                MerchantCustomerID: model.MerchantCustomerID,
                Email: model.CustomerEmail
           );

           // string clientReferenceInformationCode = "TC50171_3";
            Tmsv2customersClientReferenceInformation clientReferenceInformation = new Tmsv2customersClientReferenceInformation(
                Code: model.ClientReferenceInformationCode
           );


            List<Tmsv2customersMerchantDefinedInformation> merchantDefinedInformation = new List<Tmsv2customersMerchantDefinedInformation>();
            string merchantDefinedInformationName1 = model.MerchantDefinedInformation_Name;
            string merchantDefinedInformationValue1 = model.MerchantDefinedInformation_Value;
            merchantDefinedInformation.Add(new Tmsv2customersMerchantDefinedInformation(
                Name: merchantDefinedInformationName1,
                Value: merchantDefinedInformationValue1
           ));

            var requestObj = new PostCustomerRequest(
                BuyerInformation: buyerInformation,
                ClientReferenceInformation: clientReferenceInformation,
                MerchantDefinedInformation: merchantDefinedInformation
           );

            try
            {
                var configDictionary = CyberSourceConfiguration.GetConfiguration();
                var clientConfig = new CyberSource.Client.Configuration(merchConfigDictObj: configDictionary);

                var apiInstance = new CustomerApi(clientConfig);
                TmsV2CustomersResponse result = apiInstance.PostCustomer(requestObj);
                response.CustomerReferenceId= result.Id;
                response.ClientReferenceInformationCode = result.ClientReferenceInformation.Code;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception on calling the API : " + e.Message);
                return null;
            }
        }
        public CSCreateInstrumentIdentifierCardResponseViewModel CreateInstrumentIdentifierCard(CSCreateInstrumentIdentifierCardRequestViewModel model)
        {
            CSCreateInstrumentIdentifierCardResponseViewModel response = new CSCreateInstrumentIdentifierCardResponseViewModel();
            string profileid = null; // "93B32398-AD51-4CC2-A682-EA3E93614EB1";
           // string cardNumber = "6071870000961364";// "411111111111111";
            Tmsv2customersEmbeddedDefaultPaymentInstrumentEmbeddedInstrumentIdentifierCard card = new Tmsv2customersEmbeddedDefaultPaymentInstrumentEmbeddedInstrumentIdentifierCard(
                Number: model.CardNo
                //ExpirationMonth: model.expierymonth.ToString(),//shy feb20
                //ExpirationYear: model.expieryyear.ToString(),
                //SecurityCode: model.securitycode.ToString()



           );

            var requestObj = new PostInstrumentIdentifierRequest(
                Card: card
           );

            try
            {
                var configDictionary = CyberSourceConfiguration.GetConfiguration();
                var clientConfig = new CyberSource.Client.Configuration(merchConfigDictObj: configDictionary);

                var apiInstance = new InstrumentIdentifierApi(clientConfig);
                Tmsv2customersEmbeddedDefaultPaymentInstrumentEmbeddedInstrumentIdentifier result = apiInstance.PostInstrumentIdentifier(requestObj, profileid);
                response.State = result.State;
                response.CardNoMasked = result.Card.Number;
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception on calling the API : " + e.Message);
                return null;
            }
        }
    }
}
