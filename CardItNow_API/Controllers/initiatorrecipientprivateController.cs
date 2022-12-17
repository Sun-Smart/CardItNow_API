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
    [Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class initiatorrecipientprivateController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IinitiatorrecipientprivateService _initiatorrecipientprivateService;
        private readonly IcustomermasterService _customermasterService;
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly IgeographymasterService _geographymasterService;
        private readonly IcitymasterService _citymasterService;

        public initiatorrecipientprivateController(IHttpContextAccessor objhttpContextAccessor, IinitiatorrecipientprivateService obj_initiatorrecipientprivateService, IcustomermasterService obj_customermasterService, IboconfigvalueService obj_boconfigvalueService, IgeographymasterService obj_geographymasterService, IcitymasterService obj_citymasterService, ILoggerManager logger)
        {
            _initiatorrecipientprivateService = obj_initiatorrecipientprivateService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _initiatorrecipientprivateService = obj_initiatorrecipientprivateService;
            _customermasterService = obj_customermasterService;
            _boconfigvalueService = obj_boconfigvalueService;
            _geographymasterService = obj_geographymasterService;
            _citymasterService = obj_citymasterService;
        }

        // GET: api/initiatorrecipientprivate
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_initiatorrecipientprivates()
        {
            try
            {
                var result = _initiatorrecipientprivateService.Get_initiatorrecipientprivates();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_initiatorrecipientprivates()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_initiatorrecipientprivates " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/initiatorrecipientprivate/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _initiatorrecipientprivateService.GetFullList();
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
                var result = _initiatorrecipientprivateService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("privateid/{privateid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_privateid(int privateid)
        {
            try
            {
                var result = _initiatorrecipientprivateService.GetListBy_privateid(privateid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_privateid(int privateid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_privateid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<initiatorrecipientprivate>> Get_initiatorrecipientprivate(string sid)
        {
            try
            {
                var result = _initiatorrecipientprivateService.Get_initiatorrecipientprivate(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_initiatorrecipientprivate(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_initiatorrecipientprivate(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/initiatorrecipientprivate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<initiatorrecipientprivate>> Get_initiatorrecipientprivate(int id)
        {
            try
            {
                var result = _initiatorrecipientprivateService.Get_initiatorrecipientprivate(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_initiatorrecipientprivate(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_initiatorrecipientprivate(int id) " + ex.Message + "  " + ex.InnerException?.Message);
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
                var list_type = getList_type().Result.Result;
                var list_geoid = getList_geoid().Result.Result;
                var result = (new { list_customerid, list_uid, list_type, list_geoid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/initiatorrecipientprivate
        [HttpPost]
        public async Task<ActionResult<initiatorrecipientprivate>> Post_initiatorrecipientprivate()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                initiatorrecipientprivateView obj_initiatorrecipientprivate = JsonConvert.DeserializeObject<initiatorrecipientprivateView>(Request.Form["formData"]);
                var result = _initiatorrecipientprivateService.Save_initiatorrecipientprivate(token, obj_initiatorrecipientprivate.data);
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
        [Route("getList_type")]
        public async Task<ActionResult<dynamic>> getList_type()
        {
            try
            {
                var result = _boconfigvalueService.GetList("customermastertype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_type() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_geoid")]
        public async Task<ActionResult<dynamic>> getList_geoid()
        {
            try
            {
                string strCondition = "";
                var result = _geographymasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_geoid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_cityid/{geoid}")]
        public async Task<ActionResult<dynamic>> getList_cityid(int geoid)
        {
            try
            {
                var result = _citymasterService.GetListBy_geoid(geoid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_cityid(int geoid) {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/initiatorrecipientprivate/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<initiatorrecipientprivate>> Delete(int id)
        {
            try
            {
                var result = _initiatorrecipientprivateService.Delete(id);
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