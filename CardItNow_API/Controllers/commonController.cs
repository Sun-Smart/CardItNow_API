using carditnow.Controllers;
using CardItNow.Models;
using CardItNow.Services;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public commonController(IHttpContextAccessor objhttpContextAccessor, IcommonService commonservices, ILoggerManager logger) : base(logger)
        {
            _logger = logger;
            _commonService = commonservices;
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

        [HttpGet]
        [Route("GetPurposeList")]
        public dynamic GetPurposeList()
        {
            var result = _commonService.GetPurposeList();
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
    }
}
