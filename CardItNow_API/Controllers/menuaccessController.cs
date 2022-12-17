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
    public class menuaccessController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ImenuaccessService _menuaccessService;
        private readonly ImenumasterService _menumasterService;
        private readonly IuserrolemasterService _userrolemasterService;

        public menuaccessController(IHttpContextAccessor objhttpContextAccessor, ImenuaccessService obj_menuaccessService, ImenumasterService obj_menumasterService, IuserrolemasterService obj_userrolemasterService, ILoggerManager logger)
        {
            _menuaccessService = obj_menuaccessService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _menuaccessService = obj_menuaccessService;
            _menumasterService = obj_menumasterService;
            _userrolemasterService = obj_userrolemasterService;
        }

        // GET: api/menuaccess
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_menuaccesses()
        {
            try
            {
                var result = _menuaccessService.Get_menuaccesses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_menuaccesses()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_menuaccesses " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/menuaccess/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _menuaccessService.GetFullList();
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
                var result = _menuaccessService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("menuaccessid/{menuaccessid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_menuaccessid(int menuaccessid)
        {
            try
            {
                var result = _menuaccessService.GetListBy_menuaccessid(menuaccessid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_menuaccessid(int menuaccessid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_menuaccessid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<menuaccess>> Get_menuaccess(string sid)
        {
            try
            {
                var result = _menuaccessService.Get_menuaccess(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_menuaccess(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_menuaccess(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/menuaccess/5
        [HttpGet("{id}")]
        public async Task<ActionResult<menuaccess>> Get_menuaccess(int id)
        {
            try
            {
                var result = _menuaccessService.Get_menuaccess(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_menuaccess(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_menuaccess(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_menuid = getList_menuid().Result.Result;
                var list_roleid = getList_roleid().Result.Result;
                var result = (new { list_menuid, list_roleid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/menuaccess
        [HttpPost]
        public async Task<ActionResult<menuaccess>> Post_menuaccess()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                menuaccessView obj_menuaccess = JsonConvert.DeserializeObject<menuaccessView>(Request.Form["formData"]);
                var result = _menuaccessService.Save_menuaccess(token, obj_menuaccess.data);
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
        [Route("getList_menuid")]
        public async Task<ActionResult<dynamic>> getList_menuid()
        {
            try
            {
                string strCondition = "";
                var result = _menumasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_menuid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
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

        // DELETE: api/menuaccess/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<menuaccess>> Delete(int id)
        {
            try
            {
                var result = _menuaccessService.Delete(id);
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