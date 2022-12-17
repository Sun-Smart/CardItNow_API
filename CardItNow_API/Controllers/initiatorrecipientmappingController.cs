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
    public class initiatorrecipientmappingController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IinitiatorrecipientmappingService _initiatorrecipientmappingService;
        private readonly IcustomermasterService _customermasterService;

        public initiatorrecipientmappingController(IHttpContextAccessor objhttpContextAccessor, IinitiatorrecipientmappingService obj_initiatorrecipientmappingService, IcustomermasterService obj_customermasterService, ILoggerManager logger)
        {
            _initiatorrecipientmappingService = obj_initiatorrecipientmappingService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _initiatorrecipientmappingService = obj_initiatorrecipientmappingService;
            _customermasterService = obj_customermasterService;
        }

        // GET: api/initiatorrecipientmapping
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_initiatorrecipientmappings()
        {
            try
            {
                var result = _initiatorrecipientmappingService.Get_initiatorrecipientmappings();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_initiatorrecipientmappings()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_initiatorrecipientmappings " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/initiatorrecipientmapping/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _initiatorrecipientmappingService.GetFullList();
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
                var result = _initiatorrecipientmappingService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("mappingid/{mappingid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_mappingid(int mappingid)
        {
            try
            {
                var result = _initiatorrecipientmappingService.GetListBy_mappingid(mappingid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_mappingid(int mappingid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_mappingid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<initiatorrecipientmapping>> Get_initiatorrecipientmapping(string sid)
        {
            try
            {
                var result = _initiatorrecipientmappingService.Get_initiatorrecipientmapping(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_initiatorrecipientmapping(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_initiatorrecipientmapping(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/initiatorrecipientmapping/5
        [HttpGet("{id}")]
        public async Task<ActionResult<initiatorrecipientmapping>> Get_initiatorrecipientmapping(int id)
        {
            try
            {
                var result = _initiatorrecipientmappingService.Get_initiatorrecipientmapping(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_initiatorrecipientmapping(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_initiatorrecipientmapping(int id) " + ex.Message + "  " + ex.InnerException?.Message);
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

        // POST: api/initiatorrecipientmapping
        [HttpPost]
        public async Task<ActionResult<initiatorrecipientmapping>> Post_initiatorrecipientmapping()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                initiatorrecipientmappingView obj_initiatorrecipientmapping = JsonConvert.DeserializeObject<initiatorrecipientmappingView>(Request.Form["formData"]);
                var result = _initiatorrecipientmappingService.Save_initiatorrecipientmapping(token, obj_initiatorrecipientmapping.data);
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

        // DELETE: api/initiatorrecipientmapping/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<initiatorrecipientmapping>> Delete(int id)
        {
            try
            {
                var result = _initiatorrecipientmappingService.Delete(id);
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