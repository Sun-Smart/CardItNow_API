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
    public class userrolemasterController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IuserrolemasterService _userrolemasterService;

        public userrolemasterController(IHttpContextAccessor objhttpContextAccessor, IuserrolemasterService obj_userrolemasterService, ILoggerManager logger)
        {
            _userrolemasterService = obj_userrolemasterService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _userrolemasterService = obj_userrolemasterService;
        }

        // GET: api/userrolemaster
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_userrolemasters()
        {
            try
            {
                var result = _userrolemasterService.Get_userrolemasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_userrolemasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_userrolemasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/userrolemaster/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _userrolemasterService.GetFullList();
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
                var result = _userrolemasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("roleid/{roleid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_roleid(int roleid)
        {
            try
            {
                var result = _userrolemasterService.GetListBy_roleid(roleid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_roleid(int roleid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_roleid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<userrolemaster>> Get_userrolemaster(string sid)
        {
            try
            {
                var result = _userrolemasterService.Get_userrolemaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_userrolemaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_userrolemaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/userrolemaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<userrolemaster>> Get_userrolemaster(int id)
        {
            try
            {
                var result = _userrolemasterService.Get_userrolemaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_userrolemaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_userrolemaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
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

        // POST: api/userrolemaster
        [HttpPost]
        public async Task<ActionResult<userrolemaster>> Post_userrolemaster()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                userrolemasterView obj_userrolemaster = JsonConvert.DeserializeObject<userrolemasterView>(Request.Form["formData"]);
                var result = _userrolemasterService.Save_userrolemaster(token, obj_userrolemaster.data);
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

        // DELETE: api/userrolemaster/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<userrolemaster>> Delete(int id)
        {
            try
            {
                var result = _userrolemasterService.Delete(id);
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