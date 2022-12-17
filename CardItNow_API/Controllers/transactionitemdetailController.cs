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
    public class transactionitemdetailController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ItransactionitemdetailService _transactionitemdetailService;
        private readonly ItransactiondetailService _transactiondetailService;
        private readonly ItransactionmasterService _transactionmasterService;
        private readonly IcustomermasterService _customermasterService;
        private readonly IcustomerpaymodeService _customerpaymodeService;

        public transactionitemdetailController(IHttpContextAccessor objhttpContextAccessor, ItransactionitemdetailService obj_transactionitemdetailService, ItransactiondetailService obj_transactiondetailService, ItransactionmasterService obj_transactionmasterService, IcustomermasterService obj_customermasterService, IcustomerpaymodeService obj_customerpaymodeService, ILoggerManager logger)
        {
            _transactionitemdetailService = obj_transactionitemdetailService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _transactionitemdetailService = obj_transactionitemdetailService;
            _transactiondetailService = obj_transactiondetailService;
            _transactionmasterService = obj_transactionmasterService;
            _customermasterService = obj_customermasterService;
            _customerpaymodeService = obj_customerpaymodeService;
        }

        // GET: api/transactionitemdetail
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_transactionitemdetails()
        {
            try
            {
                var result = _transactionitemdetailService.Get_transactionitemdetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_transactionitemdetails()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactionitemdetails " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/transactionitemdetail/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _transactionitemdetailService.GetFullList();
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
                var result = _transactionitemdetailService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("transactionitemdetailid/{transactionitemdetailid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_transactionitemdetailid(int transactionitemdetailid)
        {
            try
            {
                var result = _transactionitemdetailService.GetListBy_transactionitemdetailid(transactionitemdetailid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_transactionitemdetailid(int transactionitemdetailid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_transactionitemdetailid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<transactionitemdetail>> Get_transactionitemdetail(string sid)
        {
            try
            {
                var result = _transactionitemdetailService.Get_transactionitemdetail(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_transactionitemdetail(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactionitemdetail(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/transactionitemdetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<transactionitemdetail>> Get_transactionitemdetail(int id)
        {
            try
            {
                var result = _transactionitemdetailService.Get_transactionitemdetail(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_transactionitemdetail(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactionitemdetail(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_transactiondetailid = getList_transactiondetailid().Result.Result;
                var list_transactionid = getList_transactionid().Result.Result;
                var list_uid = getList_uid().Result.Result;
                var list_recipientuid = getList_recipientuid().Result.Result;
                var list_payid = getList_payid().Result.Result;
                var result = (new { list_transactiondetailid, list_transactionid, list_uid, list_recipientuid, list_payid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/transactionitemdetail
        [HttpPost]
        public async Task<ActionResult<transactionitemdetail>> Post_transactionitemdetail()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                transactionitemdetailView obj_transactionitemdetail = JsonConvert.DeserializeObject<transactionitemdetailView>(Request.Form["formData"]);
                var result = _transactionitemdetailService.Save_transactionitemdetail(token, obj_transactionitemdetail.data);
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
        [Route("getList_transactiondetailid")]
        public async Task<ActionResult<dynamic>> getList_transactiondetailid()
        {
            try
            {
                string strCondition = "";
                var result = _transactiondetailService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_transactiondetailid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
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

        // DELETE: api/transactionitemdetail/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<transactionitemdetail>> Delete(int id)
        {
            try
            {
                var result = _transactionitemdetailService.Delete(id);
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