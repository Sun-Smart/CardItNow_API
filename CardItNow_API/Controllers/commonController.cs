using carditnow.Controllers;
using carditnow.Services;
using CardItNow.Models;
using CardItNow.Services;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Controllers
{
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class commonController : baseController
    {


        private ILoggerManager _logger;
        private readonly IcommonService _commonService;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IgeographymasterService _geographymasterService;
        private readonly IcitymasterService _citymasterService;
        private readonly IcustomermasterService _customermasterService;
        public commonController(IHttpContextAccessor objhttpContextAccessor, IcommonService commonservices, IgeographymasterService obj_geographymasterService, IcitymasterService obj_citymasterService, IcustomermasterService obj_customermasterService, ILoggerManager logger) : base(logger)
        {
            _logger = logger;
            _commonService = commonservices;
            _geographymasterService = obj_geographymasterService;
            _citymasterService = obj_citymasterService;
            _customermasterService = obj_customermasterService;
            //cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            //uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();

        }
        [HttpGet]
        [Route("Getdocumenttype")]
        public dynamic Getdocumenttype()
        {
            var result = _commonService.Getdocumenttype();
            return result;
        }

        [HttpGet]
        [Route("GetBankList")]
        public dynamic GetBankList()
        {
            var result = _commonService.GetBankList();
            return result;
        }
        //Purpose Of Payment
        [HttpGet]
        [Route("GetPurposeList")]
        public dynamic GetPurposeList()
        {
            var result = _commonService.GetPurposeList();
            return result;
        }
        //purpose of house rent
        [HttpGet]
        [Route("GetPurposeList_hr")]
        public dynamic GetPurposeList_hr()
        {
            var result = _commonService.GetPurposeList_hr();
            return result;
        }

        [HttpGet]
        [Route("Sociallogin")]
        public dynamic Sociallogin(sociallogin model)
        {
            var result = _commonService.Sociallogin(model);
            return result;
        }
        [HttpPost]
        [Route("SaveSocialMedia")]
        public dynamic SaveSocial(Savesocial model)
        {
            var result = _commonService.SaveSocial(model);
            return result;

        }

        [HttpPost]
        [Route("ForgotPasscode")]
        public dynamic forgotpass(Savesocial model)
        {
            var result = _commonService.forgotpass(model);
            return result;

        }

        [HttpPost]
        [Route("Changepass")]
        public dynamic changepasscode(changepasscode model)
        {
            var result = _commonService.ChangePass(model);
            return result;

        }
        [HttpPost]
        [Route("ForgotOTPvalidate")]
        public string otpvalidate(verify_otp model)
        {
            if (!string.IsNullOrEmpty(model.email))
            {
                var result = _commonService.otpvalidate(model);
                return result;
            }
            else
            {
                var result1 = new
                {
                    status = "fail",
                    data = "",   /* Application-specific data would go here. */
                    message = "Register email id required" /* Or optional success message */
                };
                return JsonConvert.SerializeObject(result1);
            }
           
        }


        //get privacy clause

        [HttpGet]
        [Route("GetPrivacyclause")]
        public dynamic GetPrivacyclause()
        {
            var result = _commonService.GetPrivacyclause();
            return result;
        }

        //duplicate validation for LGU
        [HttpPost]
        [Route("duplicatetransactionvalidation")]
        public dynamic duplicatetransactionvalidation(duplicatetransactionvalidation model)
        {
            var result = _commonService.duplicatetransactionvalidation(model);
            return result;

        }


        //duplicate validation for Home rent

        [HttpPost]
        [Route("duplicatetransactionvalidation_hr")]
        public dynamic duplicatetransactionvalidation_hr(duplicatetransactionvalidation model)
        {
            var result = _commonService.duplicatetransactionvalidation_hr(model);
            return result;

        }

        //dashboard

        [HttpPost]
        [Route("dashboard_recenttransaction")]
        public dynamic dashboard_recenttransaction(dashboard model)
        {
            var result = _commonService.get_recenttransaction(model);
            return result;

        }


        [HttpPost]
        [Route("dashboard_allrecenttransaction")]
        public dynamic dashboard_allrecenttransaction(dashboard model)
        {
            var result = _commonService.get_allrecenttransaction(model);
            return result;

        }


        [HttpPost]
        [Route("dashboard_moneyspenddetails")]
        public dynamic dashboard_moneyspenddetails(dashboard model)
        {
            var result = _commonService.get_moneyspenddetails(model);
            return result;

        }



        [HttpPost]
        [Route("dashboard_transactionoverview")]
        public dynamic dashboard_transactionoverview(dashboard model)
        {
            var result = _commonService.get_transactionoverview(model);
            return result;

        }

        [HttpGet]
        [Route("GetLGUcustomers")]
        public dynamic GetLGUcustomers()
        {
            var result = _commonService.GetLGUcustomers();
            return result;
        }


        [HttpGet]
        [Route("GetHomerentcustomers")]
        public dynamic GetHomerentcustomers()
        {
            var result = _commonService.GetHomerentcustomers();
            return result;
        }




        //get geodetails
        [HttpGet]
        [Route("Getgeodetails")]
        public async Task<ActionResult<IEnumerable<Object>>> Getgeodetails()
        {
            try
            {
                var result = _geographymasterService.Get_geographymasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }


        //get provience based on geoid
        [HttpGet]
        [Route("Getproviencedeatail/{geoid}")]
        public async Task<ActionResult<IEnumerable<Object>>> Getproviencedeatail(int geoid)
        {
            try
            {
                var result = _customermasterService.Getproviencedetail(geoid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }





        //get city based on geoid and provience

        [HttpGet]
        [Route("Getcitydetail/{geoid}/{provienceid}")]
        public async Task<ActionResult<IEnumerable<Object>>> Getcitydetail(int geoid, int provienceid)
        {
            try
            {
                var result = _citymasterService.GetListBy_geoid2(geoid, provienceid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_geoid(int geoid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_geoid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }



    }
}
