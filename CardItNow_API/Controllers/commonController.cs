using carditnow.Controllers;
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
    public class commonController : baseController
    {

        private ILoggerManager _logger;
        private readonly IcommonService _commonService;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        public commonController(IHttpContextAccessor objhttpContextAccessor,IcommonService commonservices, ILoggerManager logger) : base(logger)
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
            return Ok(result);
        }
    }
}
