using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nTireBO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using SunSmartnTireProducts.Helpers;
using CardItNow.interfaces;
using CardItNow.Models;

namespace CardItNow.Controllers
{
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class verifyotpController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomermasterService _customermasterService;        
        //private readonly IverifyOTPService _otpverifyservice;
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly IavatarmasterService _avatarmasterService;
        private readonly IcustomerdetailService _customerdetailService;
        private readonly IcustomertermsacceptanceService _customertermsacceptanceService;
        private readonly IcustomerpaymodeService _customerpaymodeService;
        private readonly IcustomersecurityquestionService _customersecurityquestionService;
        private readonly IcustomersecurityquestionshistoryService _customersecurityquestionshistoryService;
        private readonly IverifyotpService _Iverifyotpservice;

        public verifyotpController(IHttpContextAccessor objhttpContextAccessor, IavatarmasterService obj_avatarmasterService, IverifyotpService obj_verifyotpService, ILoggerManager logger)
        {
            _avatarmasterService = obj_avatarmasterService;
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
                _avatarmasterService = obj_avatarmasterService;
            }
        }

        [HttpPost("Verify")]        
        public ActionResult<string> Verify(string email,string otp )
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(otp))
                {
                    var result = _Iverifyotpservice.VerifyOTP(email,otp);
                    return Ok(result);
                }
                else
                { return Ok("Email / OTP missing"); }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GeSendOTP(string email)\r\n{ex}");
                //return StatusCode(StatusCodes.Status417ExpectationFailed, "GetSendOTP " + ex.Message + "  " + ex.InnerException?.Message);               
                //return "Fail";

            }
            return "fail";
        }
    }
}
