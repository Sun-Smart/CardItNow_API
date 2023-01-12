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
    [Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class customerpaymodeController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomerpaymodeService _customerpaymodeService;
        private readonly IcustomermasterService _customermasterService;

        public customerpaymodeController(IHttpContextAccessor objhttpContextAccessor, IcustomerpaymodeService obj_customerpaymodeService, IcustomermasterService obj_customermasterService, ILoggerManager logger)
        {
            _customerpaymodeService = obj_customerpaymodeService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _customerpaymodeService = obj_customerpaymodeService;
            _customermasterService = obj_customermasterService;
        }

        // GET: api/customerpaymode
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_customerpaymodes()
        {
            try
            {
                var result = _customerpaymodeService.Get_customerpaymodes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customerpaymodes()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerpaymodes " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/customerpaymode/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _customerpaymodeService.GetFullList();
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
                var result = _customerpaymodeService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("payid/{payid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_payid(int payid)
        {
            try
            {
                var result = _customerpaymodeService.GetListBy_payid(payid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_payid(int payid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_payid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customerpaymode>> Get_customerpaymode(string sid)
        {
            try
            {
                var result = _customerpaymodeService.Get_customerpaymode(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customerpaymode(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerpaymode(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/customerpaymode/5
        [HttpGet("{id}")]
        public async Task<ActionResult<customerpaymode>> Get_customerpaymode(int id)
        {
            try
            {
                var result = _customerpaymodeService.Get_customerpaymode(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_customerpaymode(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerpaymode(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_uid = getList_uid().Result.Result;
                var result = (new { list_uid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/customerpaymode
        [HttpPost]
        
        public async Task<ActionResult<customerpaymode>> Post_customerpaymode()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                customerpaymodeView obj_customerpaymode = JsonConvert.DeserializeObject<customerpaymodeView>(Request.Form["formData"]);
                var result = _customerpaymodeService.Save_customerpaymode(token, obj_customerpaymode.data);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                if (Request.Form.Files != null)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        Helper.Upload(file);
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Save api {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        [Route("SaveCutomerCardDeatils")]
        public dynamic SaveCutomerCardDeatils(customerpaymode obj_customerpaymode)
        {
            //string token = Request.Headers["Authorization"].ToString();
            try
            {
                //obj_customerpaymode = JsonConvert.DeserializeObject<customerpaymode>(Request.Form["formData"]);
                var result = _customerpaymodeService.SaveCutomerCardDeatils(obj_customerpaymode);
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

        [HttpGet]
        [Route("getList_uid")]
        public async Task<ActionResult<dynamic>> getList_uid()
        {
            try
            {
                string strCondition = "";
                var result = _customermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_uid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/customerpaymode/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customerpaymode>> Delete(int id)
        {
            try
            {
                var result = _customerpaymodeService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Delete(int id) {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Delete " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }
    }
}