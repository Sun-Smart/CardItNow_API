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
    public class menumasterController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ImenumasterService _menumasterService;
        private readonly ImenuaccessService _menuaccessService;

        public menumasterController(IHttpContextAccessor objhttpContextAccessor, ImenumasterService obj_menumasterService, ImenuaccessService obj_menuaccessService, ILoggerManager logger)
        {
            _menumasterService = obj_menumasterService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _menumasterService = obj_menumasterService;
            _menuaccessService = obj_menuaccessService;
        }

        [HttpGet]
        [Route("menuurl/{menuurl}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_menuurl(string menuurl)
        {
            try
            {
                var result = _menumasterService.GetListBy_menuurl(menuurl);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_menuurl(string menuurl)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_menuurl " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/bomenumaster
        [HttpGet]
        [Route("usermenumaster/{param}")]
        public async Task<ActionResult<IEnumerable<Object>>> Get_usermenumaster(int param)
        {
            try
            {
                var result = _menumasterService.Get_usermenumaster(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_bousermenumaster(int param) {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "bomenumaster bousermenumaster(int param) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/menumaster
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_menumasters()
        {
            try
            {
                var result = _menumasterService.Get_menumasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_menumasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_menumasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/menumaster/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _menumasterService.GetFullList();
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
                var result = _menumasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("menuid/{menuid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_menuid(int menuid)
        {
            try
            {
                var result = _menumasterService.GetListBy_menuid(menuid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_menuid(int menuid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_menuid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<menumaster>> Get_menumaster(string sid)
        {
            try
            {
                var result = _menumasterService.Get_menumaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_menumaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_menumaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/menumaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<menumaster>> Get_menumaster(int id)
        {
            try
            {
                var result = _menumasterService.Get_menumaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_menumaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_menumaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
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

        // POST: api/menumaster
        [HttpPost]
        public async Task<ActionResult<menumaster>> Post_menumaster(menumasterView obj_menumaster)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                //menumasterView obj_menumaster = JsonConvert.DeserializeObject<menumasterView>(Request.Form["formData"]);
                var result = _menumasterService.Save_menumaster(token, obj_menumaster.data);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                if (obj_menumaster.menuaccesses != null && obj_menumaster.menuaccesses.Count > 0)
                {
                    foreach (var obj in obj_menumaster.menuaccesses)
                    {
                        if (obj.menuaccessid == null)
                        {
                            obj.menuid = result.menumaster.menuid;
                            _menuaccessService.Save_menuaccess(token, obj);
                        }
                    }
                }
                if (obj_menumaster.Deleted_menuaccess_IDs != null && obj_menumaster.Deleted_menuaccess_IDs != "")
                {
                    string[] ids = obj_menumaster.Deleted_menuaccess_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _menuaccessService.Delete(int.Parse(id));
                        }
                    }
                }
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

        // DELETE: api/menumaster/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<menumaster>> Delete(int id)
        {
            try
            {
                var result = _menumasterService.Delete(id);
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