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
    public class recipientdiscountController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IrecipientdiscountService _recipientdiscountService;
        private readonly IcustomermasterService _customermasterService;

        public recipientdiscountController(IHttpContextAccessor objhttpContextAccessor, IrecipientdiscountService obj_recipientdiscountService, IcustomermasterService obj_customermasterService, ILoggerManager logger)
        {
            _recipientdiscountService = obj_recipientdiscountService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _recipientdiscountService = obj_recipientdiscountService;
            _customermasterService = obj_customermasterService;
        }

        // GET: api/recipientdiscount
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_recipientdiscounts()
        {
            try
            {
                var result = _recipientdiscountService.Get_recipientdiscounts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_recipientdiscounts()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_recipientdiscounts " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/recipientdiscount/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _recipientdiscountService.GetFullList();
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
                var result = _recipientdiscountService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("discountid/{discountid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_discountid(int discountid)
        {
            try
            {
                var result = _recipientdiscountService.GetListBy_discountid(discountid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_discountid(int discountid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_discountid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<recipientdiscount>> Get_recipientdiscount(string sid)
        {
            try
            {
                var result = _recipientdiscountService.Get_recipientdiscount(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_recipientdiscount(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_recipientdiscount(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/recipientdiscount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<recipientdiscount>> Get_recipientdiscount(int id)
        {
            try
            {
                var result = _recipientdiscountService.Get_recipientdiscount(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_recipientdiscount(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_recipientdiscount(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_recipientuid = getList_recipientuid().Result.Result;
                var list_initiatoruid = getList_initiatoruid().Result.Result;
                var result = (new { list_recipientuid, list_initiatoruid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/recipientdiscount
        [HttpPost]
        public async Task<ActionResult<recipientdiscount>> Post_recipientdiscount()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                recipientdiscountView obj_recipientdiscount = JsonConvert.DeserializeObject<recipientdiscountView>(Request.Form["formData"]);
                var result = _recipientdiscountService.Save_recipientdiscount(token, obj_recipientdiscount.data);
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
        [Route("getList_initiatoruid")]
        public async Task<ActionResult<dynamic>> getList_initiatoruid()
        {
            try
            {
                string strCondition = "";
                var result = _customermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_initiatoruid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/recipientdiscount/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<recipientdiscount>> Delete(int id)
        {
            try
            {
                var result = _recipientdiscountService.Delete(id);
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