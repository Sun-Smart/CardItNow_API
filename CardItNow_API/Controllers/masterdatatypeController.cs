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
    public class masterdatatypeController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ImasterdatatypeService _masterdatatypeService;
        private readonly ImasterdataService _masterdataService;

        public masterdatatypeController(IHttpContextAccessor objhttpContextAccessor, ImasterdatatypeService obj_masterdatatypeService, ImasterdataService obj_masterdataService, ILoggerManager logger)
        {
            _masterdatatypeService = obj_masterdatatypeService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _masterdatatypeService = obj_masterdatatypeService;
            _masterdataService = obj_masterdataService;
        }

        // GET: api/masterdatatype
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_masterdatatypes()
        {
            try
            {
                var result = _masterdatatypeService.Get_masterdatatypes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_masterdatatypes()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_masterdatatypes " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/masterdatatype/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _masterdatatypeService.GetFullList();
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
                var result = _masterdatatypeService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("datatypeid/{datatypeid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_datatypeid(int datatypeid)
        {
            try
            {
                var result = _masterdatatypeService.GetListBy_datatypeid(datatypeid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_datatypeid(int datatypeid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_datatypeid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<masterdatatype>> Get_masterdatatype(string sid)
        {
            try
            {
                var result = _masterdatatypeService.Get_masterdatatype(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_masterdatatype(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_masterdatatype(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/masterdatatype/5
        [HttpGet("{id}")]
        public async Task<ActionResult<masterdatatype>> Get_masterdatatype(int id)
        {
            try
            {
                var result = _masterdatatypeService.Get_masterdatatype(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_masterdatatype(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_masterdatatype(int id) " + ex.Message + "  " + ex.InnerException?.Message);
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

        // POST: api/masterdatatype
        [HttpPost]
        public async Task<ActionResult<masterdatatype>> Post_masterdatatype()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                masterdatatypeView obj_masterdatatype = JsonConvert.DeserializeObject<masterdatatypeView>(Request.Form["formData"]);
                var result = _masterdatatypeService.Save_masterdatatype(token, obj_masterdatatype.data);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                if (obj_masterdatatype.masterdatas != null && obj_masterdatatype.masterdatas.Count > 0)
                {
                    foreach (var obj in obj_masterdatatype.masterdatas)
                    {
                        if (obj.masterdataid == null)
                        {
                            obj.masterdatatypeid = result.masterdatatype.masterdatatypeid;
                            _masterdataService.Save_masterdata(token, obj);
                        }
                    }
                }
                if (obj_masterdatatype.Deleted_masterdata_IDs != null && obj_masterdatatype.Deleted_masterdata_IDs != "")
                {
                    string[] ids = obj_masterdatatype.Deleted_masterdata_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _masterdataService.Delete(int.Parse(id));
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

        // DELETE: api/masterdatatype/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<masterdatatype>> Delete(int id)
        {
            try
            {
                var result = _masterdatatypeService.Delete(id);
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