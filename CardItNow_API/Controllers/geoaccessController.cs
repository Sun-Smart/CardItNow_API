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
    //[Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class geoaccessController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IgeoaccessService _geoaccessService;
        private readonly IgeographymasterService _geographymasterService;
        private readonly IusermasterService _usermasterService;

        public geoaccessController(IHttpContextAccessor objhttpContextAccessor, IgeoaccessService obj_geoaccessService, IgeographymasterService obj_geographymasterService, IusermasterService obj_usermasterService, ILoggerManager logger)
        {
            _geoaccessService = obj_geoaccessService;
            _logger = logger;
            if (objhttpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
                _geoaccessService = obj_geoaccessService;
                _geographymasterService = obj_geographymasterService;
                _usermasterService = obj_usermasterService;
            }
        }

        // GET: api/geoaccess
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_geoaccesses()
        {
            try
            {
                var result = _geoaccessService.Get_geoaccesses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_geoaccesses()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_geoaccesses " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/geoaccess/5
        //[AllowAnonymous]
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _geoaccessService.GetFullList();
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
                var result = _geoaccessService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("geoaccessid/{geoaccessid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_geoaccessid(int geoaccessid)
        {
            try
            {
                var result = _geoaccessService.GetListBy_geoaccessid(geoaccessid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_geoaccessid(int geoaccessid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_geoaccessid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<geoaccess>> Get_geoaccess(string sid)
        {
            try
            {
                var result = _geoaccessService.Get_geoaccess(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_geoaccess(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_geoaccess(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/geoaccess/5
        [HttpGet("{id}")]
        public async Task<ActionResult<geoaccess>> Get_geoaccess(int id)
        {
            try
            {
                var result = _geoaccessService.Get_geoaccess(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_geoaccess(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_geoaccess(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_geoid = getList_geoid().Result.Result;
                var list_userid = getList_userid().Result.Result;
                var result = (new { list_geoid, list_userid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/geoaccess
        [HttpPost]
        public async Task<ActionResult<geoaccess>> Post_geoaccess()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                geoaccessView obj_geoaccess = JsonConvert.DeserializeObject<geoaccessView>(Request.Form["formData"]);
                var result = _geoaccessService.Save_geoaccess(token, obj_geoaccess.data);
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
        [Route("getList_userid")]
        public async Task<ActionResult<dynamic>> getList_userid()
        {
            try
            {
                string strCondition = "";
                var result = _usermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_userid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/geoaccess/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<geoaccess>> Delete(int id)
        {
            try
            {
                var result = _geoaccessService.Delete(id);
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