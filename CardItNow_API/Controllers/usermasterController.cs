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
    public class usermasterController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IusermasterService _usermasterService;
        private readonly IuserrolemasterService _userrolemasterService;
        private readonly IgeographymasterService _geographymasterService;

        //private readonly IuserrolemasterService _userrolemasterService;
        public usermasterController(IHttpContextAccessor objhttpContextAccessor, IusermasterService obj_usermasterService, IuserrolemasterService obj_userrolemasterService, IgeographymasterService obj_geographymasterService, ILoggerManager logger)
        {
            _usermasterService = obj_usermasterService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _usermasterService = obj_usermasterService;
            // _userrolemasterService =obj_userrolemasterService;
            _geographymasterService = obj_geographymasterService;
            _userrolemasterService = obj_userrolemasterService;
        }

        // GET: api/usermaster
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_usermasters()
        {
            try
            {
                var result = _usermasterService.Get_usermasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_usermasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_usermasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/usermaster/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _usermasterService.GetFullList();
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
                var result = _usermasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("userid/{userid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_userid(int userid)
        {
            try
            {
                var result = _usermasterService.GetListBy_userid(userid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_userid(int userid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_userid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<usermaster>> Get_usermaster(string sid)
        {
            try
            {
                var result = _usermasterService.Get_usermaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_usermaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_usermaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/usermaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<usermaster>> Get_usermaster(int id)
        {
            try
            {
                var result = _usermasterService.Get_usermaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_usermaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_usermaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_roleid = getList_roleid().Result.Result;
                var list_basegeoid = getList_basegeoid().Result.Result;
                var result = (new { list_roleid, list_basegeoid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/usermaster
        [HttpPost]
        public async Task<ActionResult<usermaster>> Post_usermaster(usermaster obj_usermaster)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
               // usermaster obj_usermaster = JsonConvert.DeserializeObject<usermaster>(Request.Form["data"]);
                var result = _usermasterService.Save_usermaster(token, obj_usermaster);
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
        [Route("getList_roleid")]
        public async Task<ActionResult<dynamic>> getList_roleid()
        {
            try
            {
                string strCondition = "";
                var result = _userrolemasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_roleid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_basegeoid")]
        public async Task<ActionResult<dynamic>> getList_basegeoid()
        {
            try
            {
                string strCondition = "";
                var result = _geographymasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_basegeoid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/usermaster/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<usermaster>> Delete(int id)
        {
            try
            {
                var result = _usermasterService.Delete(id);
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