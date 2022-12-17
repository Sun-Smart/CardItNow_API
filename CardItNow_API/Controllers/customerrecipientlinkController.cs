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
    public class customerrecipientlinkController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomerrecipientlinkService _customerrecipientlinkService;
        private readonly IcustomermasterService _customermasterService;

        public customerrecipientlinkController(IHttpContextAccessor objhttpContextAccessor, IcustomerrecipientlinkService obj_customerrecipientlinkService, IcustomermasterService obj_customermasterService, ILoggerManager logger)
        {
            _customerrecipientlinkService = obj_customerrecipientlinkService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _customerrecipientlinkService = obj_customerrecipientlinkService;
            _customermasterService = obj_customermasterService;
        }

        // GET: api/customerrecipientlink
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_customerrecipientlinks()
        {
            try
            {
                var result = _customerrecipientlinkService.Get_customerrecipientlinks();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customerrecipientlinks()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerrecipientlinks " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/customerrecipientlink/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _customerrecipientlinkService.GetFullList();
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
                var result = _customerrecipientlinkService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("linkid/{linkid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_linkid(int linkid)
        {
            try
            {
                var result = _customerrecipientlinkService.GetListBy_linkid(linkid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_linkid(int linkid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_linkid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customerrecipientlink>> Get_customerrecipientlink(string sid)
        {
            try
            {
                var result = _customerrecipientlinkService.Get_customerrecipientlink(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customerrecipientlink(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerrecipientlink(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/customerrecipientlink/5
        [HttpGet("{id}")]
        public async Task<ActionResult<customerrecipientlink>> Get_customerrecipientlink(int id)
        {
            try
            {
                var result = _customerrecipientlinkService.Get_customerrecipientlink(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_customerrecipientlink(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerrecipientlink(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_customerid = getList_customerid().Result.Result;
                var list_uid = getList_uid().Result.Result;
                var list_recipientuid = getList_recipientuid().Result.Result;
                var result = (new { list_customerid, list_uid, list_recipientuid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/customerrecipientlink
        [HttpPost]
        public async Task<ActionResult<customerrecipientlink>> Post_customerrecipientlink()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                customerrecipientlinkView obj_customerrecipientlink = JsonConvert.DeserializeObject<customerrecipientlinkView>(Request.Form["formData"]);
                var result = _customerrecipientlinkService.Save_customerrecipientlink(token, obj_customerrecipientlink.data);
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

        // DELETE: api/customerrecipientlink/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customerrecipientlink>> Delete(int id)
        {
            try
            {
                var result = _customerrecipientlinkService.Delete(id);
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