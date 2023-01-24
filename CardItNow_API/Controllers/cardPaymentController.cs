using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nTireBO.Services;
using System;
using System.Linq;
using CardItNow.interfaces;
using CardItNow.Services;
using NLog;
using CardItNow.ViewModel;

namespace CardItNow.Controllers
{
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class cardPaymentController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomermasterService _customermasterService;        
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly IcustomerpaymodeService _customerpaymodeService;
        private readonly IverifyotpService _Iverifyotpservice;

        public cardPaymentController(IHttpContextAccessor objhttpContextAccessor, IavatarmasterService obj_avatarmasterService, IverifyotpService obj_verifyotpService, ILoggerManager logger)
        {
            _Iverifyotpservice = obj_verifyotpService;
            _logger = logger;
            if (objhttpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            }
        }

        [HttpPost("CreateCustomer")]        
        public ActionResult<string> CreateCustomer(CSCreateCustomerRequestViewModel cyberSourceCreateCustomerRequestViewModel)
        {
            try
            {
                //CSCreateCustomerRequestViewModel cyberSourceCreateCustomerRequestViewModel = new CSCreateCustomerRequestViewModel();
                //cyberSourceCreateCustomerRequestViewModel.MerchantCustomerID = "Your customer identifier";
                //cyberSourceCreateCustomerRequestViewModel.CustomerEmail = "test@cybs.com";
                //cyberSourceCreateCustomerRequestViewModel.ClientReferenceInformationCode = "TC50171_3";
                //cyberSourceCreateCustomerRequestViewModel.ClientReferenceInformationCode = "TC50171_3";
                //cyberSourceCreateCustomerRequestViewModel.MerchantDefinedInformation_Name = "data1";
                //cyberSourceCreateCustomerRequestViewModel.MerchantDefinedInformation_Value = "Your customer data";

                var result = new CyberSourceHelperService().CreateCustomer(cyberSourceCreateCustomerRequestViewModel);

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GeSendOTP(string email)\r\n{ex}");
            }
            return "fail";
        }

        [HttpPost("VerifyCard")]
        public ActionResult<string> VerifyCard(CSCreateInstrumentIdentifierCardRequestViewModel model)
        {
            try
            {
                //                CSCreateInstrumentIdentifierCardRequestViewModel identifierCardRequestViewModel = new CSCreateInstrumentIdentifierCardRequestViewModel();
                //identifierCardRequestViewModel.CardNo = "6071870000961364";
                var obj = new CyberSourceHelperService().CreateInstrumentIdentifierCard(model);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GeSendOTP(string email)\r\n{ex}");
            }
            return "fail";
        }
    }
}
