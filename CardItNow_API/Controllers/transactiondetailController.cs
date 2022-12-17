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
    public class transactiondetailController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ItransactiondetailService _transactiondetailService;
        private readonly ItransactionmasterService _transactionmasterService;
        private readonly IcustomermasterService _customermasterService;
        private readonly IcustomerpaymodeService _customerpaymodeService;

        public transactiondetailController(IHttpContextAccessor objhttpContextAccessor, ItransactiondetailService obj_transactiondetailService, ItransactionmasterService obj_transactionmasterService, IcustomermasterService obj_customermasterService, IcustomerpaymodeService obj_customerpaymodeService, ILoggerManager logger)
        {
            _transactiondetailService = obj_transactiondetailService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _transactiondetailService = obj_transactiondetailService;
            _transactionmasterService = obj_transactionmasterService;
            _customermasterService = obj_customermasterService;
            _customerpaymodeService = obj_customerpaymodeService;
        }

        // GET: api/transactiondetail
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_transactiondetails()
        {
            try
            {
                var result = _transactiondetailService.Get_transactiondetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_transactiondetails()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactiondetails " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/transactiondetail/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _transactiondetailService.GetFullList();
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
                var result = _transactiondetailService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("transactiondetailid/{transactiondetailid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_transactiondetailid(int transactiondetailid)
        {
            try
            {
                var result = _transactiondetailService.GetListBy_transactiondetailid(transactiondetailid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_transactiondetailid(int transactiondetailid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_transactiondetailid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<transactiondetail>> Get_transactiondetail(string sid)
        {
            try
            {
                var result = _transactiondetailService.Get_transactiondetail(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_transactiondetail(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactiondetail(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/transactiondetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<transactiondetail>> Get_transactiondetail(int id)
        {
            try
            {
                var result = _transactiondetailService.Get_transactiondetail(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_transactiondetail(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactiondetail(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_transactionid = getList_transactionid().Result.Result;
                var list_uid = getList_uid().Result.Result;
                var list_recipientuid = getList_recipientuid().Result.Result;
                var list_payid = getList_payid().Result.Result;
                var result = (new { list_transactionid, list_uid, list_recipientuid, list_payid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/transactiondetail
        [HttpPost]
        public async Task<ActionResult<transactiondetail>> Post_transactiondetail()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                transactiondetailView obj_transactiondetail = JsonConvert.DeserializeObject<transactiondetailView>(Request.Form["formData"]);
                var result = _transactiondetailService.Save_transactiondetail(token, obj_transactiondetail.data);
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

        [HttpGet]
        [Route("getList_transactionid")]
        public async Task<ActionResult<dynamic>> getList_transactionid()
        {
            try
            {
                string strCondition = "";
                var result = _transactionmasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_transactionid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
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

        [HttpGet]
        [Route("getList_recipientuid")]
        public async Task<ActionResult<dynamic>> getList_recipientuid()
        {
            try
            {
                string strCondition = "";
                var result = _customermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_recipientuid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_payid")]
        public async Task<ActionResult<dynamic>> getList_payid()
        {
            try
            {
                string strCondition = "";
                var result = _customerpaymodeService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_payid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/transactiondetail/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<transactiondetail>> Delete(int id)
        {
            try
            {
                var result = _transactiondetailService.Delete(id);
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