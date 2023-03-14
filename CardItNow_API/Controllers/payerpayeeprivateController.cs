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
        [Route("SavePayerPayeePrivate")]
        public dynamic Save_payerpayeeprivate(payerpayeeprivate model)
        {
            var result = _payerpayeeprivateService.Save_payerpayeprivate(model);
            return Ok(result);
        }

        [HttpGet]
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

        //payee detail based on business registration number brn

        [HttpPost]
        [Route("MandatoryPayee")]
        public dynamic MandatoryPayee(payerpayeeprivate model)
        {
            try
            {
                string brn = model.brn;
                //var list_uid = getList_uid().Result.Result;
                var result = _payerpayeeprivateService.MandatoryPayee(brn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }


        //payerpayeeprivate/GetpayeeList/{customerid}
        [HttpGet]
        [Route("GetpayeeList/{customerid}")]
        public dynamic Get_payeeList(int customerid)
        {
            var result = _payerpayeeprivateService.GetallPayee(customerid);
            return result;
        }



        //Get_payeetranscdetail/GetpayeeList/{customerid}
        [HttpGet]
        [Route("Get_payeetranscdetail/{customerid}")]
        public dynamic Get_payeetranscdetail(int customerid)
        {
            var result = _payerpayeeprivateService.GetallPayeetranscdetail(customerid);
            return result;
        }




        //payerpayeeprivate/Get_payeebankdetail/{payeeid}
        [HttpGet]
        [Route("Get_payeebankdetail/{payeeid}")]
        public dynamic Get_payeebankdetail(int payeeid)
        {
            var result = _payerpayeeprivateService.GetallPayeebankdetail(payeeid);
            return result;
        }


    }
}
