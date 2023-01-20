using carditnow.Controllers;
using CardItNow.interfaces;
using CardItNow.Models;
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
    public class payerpayeeprivateController : baseController
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IpayerpayeeprivateService _payerpayeeprivateService;

        public payerpayeeprivateController(IHttpContextAccessor objhttpContextAccessor, IpayerpayeeprivateService obj_payerpayeeprivateService, ILoggerManager logger) : base(logger)
        {

            _payerpayeeprivateService = obj_payerpayeeprivateService;
            _logger = logger;
            //cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            //uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            
        }


        [HttpPost]
        //[Route("SavePayerPayeePrivate")]
        public dynamic Save_payerpayeeprivate(payerpayeeprivate model)
        {
            var result = _payerpayeeprivateService.Save_payerpayeprivate(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetRawResult")]
        public dynamic Get_rawresult()
        {
            var result = _payerpayeeprivateService.Get_rawresult();
            return result;
        }

        [HttpGet]
        [Route("GetCardNo")]
        public dynamic MaskedNumber(string source)
        {
            var result = _payerpayeeprivateService.MaskedNumber(source);
            return result;
        }
    }
}
