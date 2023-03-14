using carditnow.Models;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nTireBO.Services;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace carditnow.Controllers
{

    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class customerdocumentverificationController : baseController
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        //private readonly IcustomermasterService _customermasterService;
        //private readonly IboconfigvalueService _boconfigvalueService;
        //private readonly IavatarmasterService _avatarmasterService;
        //private readonly IcustomerdetailService _customerdetailService;
        //private readonly IcustomertermsacceptanceService _customertermsacceptanceService;
        //private readonly IcustomerpaymodeService _customerpaymodeService;
        //private readonly IcustomersecurityquestionService _customersecurityquestionService;
        //private readonly IcustomersecurityquestionshistoryService _customersecurityquestionshistoryService;
        private readonly IcustomerdocumentverificationService _customerdocumentverificationService;

        public customerdocumentverificationController(IHttpContextAccessor objhttpContextAccessor, IcustomerdocumentverificationService obj_customerdocumentverificationService, ILoggerManager logger) : base(logger)
            //, IcustomermasterService obj_customermasterService, IboconfigvalueService obj_boconfigvalueService, IavatarmasterService obj_avatarmasterService, IcustomerdetailService obj_customerdetailService, IcustomertermsacceptanceService obj_customertermsacceptanceService, IcustomerpaymodeService obj_customerpaymodeService, IcustomersecurityquestionService obj_customersecurityquestionService, IcustomersecurityquestionshistoryService obj_customersecurityquestionshistoryService
        {
            //_customermasterService = obj_customermasterService;
            _customerdocumentverificationService = obj_customerdocumentverificationService;
            _logger = logger;
            if (objhttpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
                //_customermasterService = obj_customermasterService;
                //_boconfigvalueService = obj_boconfigvalueService;
                //_avatarmasterService = obj_avatarmasterService;
                //_customerdetailService = obj_customerdetailService;
                //_customertermsacceptanceService = obj_customertermsacceptanceService;
                //_customerpaymodeService = obj_customerpaymodeService;
                //_customersecurityquestionService = obj_customersecurityquestionService;
                //_customersecurityquestionshistoryService = obj_customersecurityquestionshistoryService;
            }
        }

       

        [HttpPost("SendOTP")]
        public async Task<ActionResult<IEnumerable<Object>>> SendOTP(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var result = "";
                   // var result = _customermasterService.SendOTP(email);
                    return Ok(result);
                }
                else
                {
                    return Content("fail");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Password Config(string email,string password)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Password Config " + ex.Message + "  " + ex.InnerException?.Message);
            }

        }





        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customerdocumentverification>> Get_customermaster(string sid)
        {
            try
            {
                var result = _customerdocumentverificationService.Get_customermasterdocument(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customermaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customermaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }






        [HttpPost]
        public async Task<ActionResult<customerdocumentverification>> Post_customermaster(customerdocumentverification obj_customermaster1)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
               // customermasterView obj_customermaster = JsonConvert.DeserializeObject<customermasterView>(Request.Form["formData"]);
                var result = _customerdocumentverificationService.Save_customerdocumentverification(token, obj_customermaster1);
                //HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Authorization", token);
                

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Save api {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

      


    }
}