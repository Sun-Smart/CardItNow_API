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
    public class masterdataController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ImasterdataService _masterdataService;
        private readonly ImasterdatatypeService _masterdatatypeService;

        public masterdataController(IHttpContextAccessor objhttpContextAccessor, ImasterdataService obj_masterdataService, ImasterdatatypeService obj_masterdatatypeService, ILoggerManager logger)
        {
            _masterdataService = obj_masterdataService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _masterdataService = obj_masterdataService;
            _masterdatatypeService = obj_masterdatatypeService;
        }

        // GET: api/masterdata
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_masterdatas()
        {
            try
            {
                var result = _masterdataService.Get_masterdatas();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_masterdatas()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_masterdatas " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/masterdata/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _masterdataService.GetFullList();
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
                var result = _masterdataService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("masterdataid/{masterdataid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_masterdataid(int masterdataid)
        {
            try
            {
                var result = _masterdataService.GetListBy_masterdataid(masterdataid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_masterdataid(int masterdataid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_masterdataid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<masterdata>> Get_masterdata(string sid)
        {
            try
            {
                var result = _masterdataService.Get_masterdata(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_masterdata(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_masterdata(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/masterdata/5
        [HttpGet("{id}")]
        public async Task<ActionResult<masterdata>> Get_masterdata(int id)
        {
            try
            {
                var result = _masterdataService.Get_masterdata(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_masterdata(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_masterdata(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_masterdatatypeid = getList_masterdatatypeid().Result.Result;
                var result = (new { list_masterdatatypeid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/masterdata
        [HttpPost]
        public async Task<ActionResult<masterdata>> Post_masterdata()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                masterdataView obj_masterdata = JsonConvert.DeserializeObject<masterdataView>(Request.Form["formData"]);
                var result = _masterdataService.Save_masterdata(token, obj_masterdata.data);
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
        [Route("getList_masterdatatypeid")]
        public async Task<ActionResult<dynamic>> getList_masterdatatypeid()
        {
            try
            {
                string strCondition = "";
                var result = _masterdatatypeService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_masterdatatypeid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/masterdata/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<masterdata>> Delete(int id)
        {
            try
            {
                var result = _masterdataService.Delete(id);
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