using carditnow.Models;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace carditnow.Controllers
{
    //[Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class customertermsacceptanceController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomertermsacceptanceService _customertermsacceptanceService;
        private readonly ItermsmasterService _termsmasterService;
        private readonly IcustomermasterService _customermasterService;

        public customertermsacceptanceController(IHttpContextAccessor objhttpContextAccessor, IcustomertermsacceptanceService obj_customertermsacceptanceService, ItermsmasterService obj_termsmasterService, IcustomermasterService obj_customermasterService, ILoggerManager logger)
        {
            _customertermsacceptanceService = obj_customertermsacceptanceService;
            _logger = logger;
            if (objhttpContextAccessor.HttpContext.User.Claims.Any())
            {
               // cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
               // uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
                _customertermsacceptanceService = obj_customertermsacceptanceService;
                _termsmasterService = obj_termsmasterService;
                _customermasterService = obj_customermasterService;
            }
        }

        // GET: api/customertermsacceptance
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_customertermsacceptances()
        {
            try
            {
                var result = _customertermsacceptanceService.Get_customertermsacceptances();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customertermsacceptances()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customertermsacceptances " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/customertermsacceptance/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _customertermsacceptanceService.GetFullList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        //Dump of the table.If param field exists, filter by param
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _customertermsacceptanceService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("customertermid/{customertermid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_customertermid(int customertermid)
        {
            try
            {
                var result = _customertermsacceptanceService.GetListBy_customertermid(customertermid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_customertermid(int customertermid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_customertermid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customertermsacceptance>> Get_customertermsacceptance(string sid)
        {
            try
            {
                var result = _customertermsacceptanceService.Get_customertermsacceptance(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customertermsacceptance(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customertermsacceptance(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/customertermsacceptance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<customertermsacceptance>> Get_customertermsacceptance(int id)
        {
            try
            {
                var result = _customertermsacceptanceService.Get_customertermsacceptance(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_customertermsacceptance(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customertermsacceptance(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_termid = getList_termid().Result.Result;
                var list_customerid = getList_customerid().Result.Result;
                var result = (new { list_termid, list_customerid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        //// POST: api/customertermsacceptance
        //[HttpPost]
        //public async Task<ActionResult<customertermsacceptance>> Post_customertermsacceptance()
        //{
        //    string token = Request.Headers["Authorization"].ToString();
        //    try
        //    {
        //        customertermsacceptanceView obj_customertermsacceptance = JsonConvert.DeserializeObject<customertermsacceptanceView>(Request.Form["formData"]);
        //        var result = _customertermsacceptanceService.Save_customertermsacceptance(token, obj_customertermsacceptance.data);
        //        HttpClient client = new HttpClient();
        //        client.DefaultRequestHeaders.Add("Authorization", token);
        //        if (Request.Form.Files != null)
        //        {
        //            foreach (var file in Request.Form.Files)
        //            {
        //                Helper.Upload(file);
        //            }
        //        }

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Controller:Save api {ex}");
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
        //    }
        //}




        // POST: api/customertermsacceptance
        [HttpPost]
        public async Task<ActionResult<customertermsacceptance>> Post_customertermsacceptance(customertermsacceptance obj_customertermsacceptance)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                //customertermsacceptanceView obj_customertermsacceptance = JsonConvert.DeserializeObject<customertermsacceptanceView>(Request.Form["formData"]);
                var result = _customertermsacceptanceService.Save_customertermsacceptance(token, obj_customertermsacceptance);


                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Save api {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }








        [HttpGet]
        [Route("getList_termid")]
        public async Task<ActionResult<dynamic>> getList_termid()
        {
            try
            {
                string strCondition = "";
                var result = _termsmasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_termid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_customerid")]
        public async Task<ActionResult<dynamic>> getList_customerid()
        {
            try
            {
                string strCondition = "";
                var result = _customermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_customerid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/customertermsacceptance/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customertermsacceptance>> Delete(int id)
        {
            try
            {
                var result = _customertermsacceptanceService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Delete(int id) {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Delete " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        [Route("CustomeracceptTerms")]
    
        public dynamic customeracceptancetermscondition(Customeracceptanceterms model)
        {
            var result = _customertermsacceptanceService.customeracceptancetermscondition(model);
            return result;

        }
    }
}