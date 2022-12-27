using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nTireBO.Models;
using nTireBO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace nTireBO.Controllers
{
    [Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class boconfigvalueController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IboconfigvalueService _boconfigvalueService;

        public boconfigvalueController(IHttpContextAccessor objhttpContextAccessor, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _boconfigvalueService = obj_boconfigvalueService;
            _logger = logger;
            if (objhttpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email").Value.ToString();
                _boconfigvalueService = obj_boconfigvalueService;
            }
        }

        // GET: api/boconfigvalue
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_boconfigvalues()
        {
            try
            {
                var result = _boconfigvalueService.Get_boconfigvalues();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_boconfigvalues()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_boconfigvalues " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/boconfigvalue/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _boconfigvalueService.GetFullList();
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
                var result = _boconfigvalueService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("configid/{configid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_configid(int configid)
        {
            try
            {
                var result = _boconfigvalueService.GetListBy_configid(configid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_configid(int configid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_configid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        /*
        [HttpGet]
        [Route("param/{param}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_param(string param)
        {
        try
        {
        var result= _boconfigvalueService.GetListBy_param(param);
        return Ok(result);
        } catch (Exception ex) {
            _logger.LogError($"Controller: GetListBy_param(string param)\r\n{ex}");
            return StatusCode(StatusCodes.Status417ExpectationFailed,"GetListBy_param " + ex.Message + "  " + ex.InnerException?.Message);
        }
        }
        */

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<boconfigvalue>> Get_boconfigvalue(string sid)
        {
            try
            {
                var result = _boconfigvalueService.Get_boconfigvalue(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_boconfigvalue(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_boconfigvalue(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/boconfigvalue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<boconfigvalue>> Get_boconfigvalue(int id)
        {
            try
            {
                var result = _boconfigvalueService.Get_boconfigvalue(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_boconfigvalue(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_boconfigvalue(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var result = (new { });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/boconfigvalue
        [HttpPost]
        public async Task<ActionResult<boconfigvalue>> Post_boconfigvalue(boconfigvalueView obj_boconfigvalue)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _boconfigvalueService.Save_boconfigvalue(token, obj_boconfigvalue.data);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Save api {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/boconfigvalue/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<boconfigvalue>> Delete(int id)
        {
            try
            {
                var result = _boconfigvalueService.Delete(id);
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