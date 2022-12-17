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
    public class geographymasterController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IgeographymasterService _geographymasterService;
        private readonly IcitymasterService _citymasterService;
        private readonly IgeoaccessService _geoaccessService;

        public geographymasterController(IHttpContextAccessor objhttpContextAccessor, IgeographymasterService obj_geographymasterService, IcitymasterService obj_citymasterService, IgeoaccessService obj_geoaccessService, ILoggerManager logger)
        {
            _geographymasterService = obj_geographymasterService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _geographymasterService = obj_geographymasterService;
            _citymasterService = obj_citymasterService;
            _geoaccessService = obj_geoaccessService;
        }

        // GET: api/geographymaster
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_geographymasters()
        {
            try
            {
                var result = _geographymasterService.Get_geographymasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_geographymasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_geographymasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/geographymaster/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _geographymasterService.GetFullList();
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
                var result = _geographymasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("geoid/{geoid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_geoid(int geoid)
        {
            try
            {
                var result = _geographymasterService.GetListBy_geoid(geoid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_geoid(int geoid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_geoid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<geographymaster>> Get_geographymaster(string sid)
        {
            try
            {
                var result = _geographymasterService.Get_geographymaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_geographymaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_geographymaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/geographymaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<geographymaster>> Get_geographymaster(int id)
        {
            try
            {
                var result = _geographymasterService.Get_geographymaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_geographymaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_geographymaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
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

        // POST: api/geographymaster
        [HttpPost]
        public async Task<ActionResult<geographymaster>> Post_geographymaster()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                geographymasterView obj_geographymaster = JsonConvert.DeserializeObject<geographymasterView>(Request.Form["formData"]);
                var result = _geographymasterService.Save_geographymaster(token, obj_geographymaster.data);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                if (obj_geographymaster.citymasters != null && obj_geographymaster.citymasters.Count > 0)
                {
                    foreach (var obj in obj_geographymaster.citymasters)
                    {
                        if (obj.cityid == null)
                        {
                            obj.geoid = result.geographymaster.geoid;
                            _citymasterService.Save_citymaster(token, obj);
                        }
                    }
                }
                if (obj_geographymaster.Deleted_citymaster_IDs != null && obj_geographymaster.Deleted_citymaster_IDs != "")
                {
                    string[] ids = obj_geographymaster.Deleted_citymaster_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _citymasterService.Delete(int.Parse(id));
                        }
                    }
                }
                if (obj_geographymaster.geoaccesses != null && obj_geographymaster.geoaccesses.Count > 0)
                {
                    foreach (var obj in obj_geographymaster.geoaccesses)
                    {
                        if (obj.geoaccessid == null)
                        {
                            obj.geoid = result.geographymaster.geoid;
                            _geoaccessService.Save_geoaccess(token, obj);
                        }
                    }
                }
                if (obj_geographymaster.Deleted_geoaccess_IDs != null && obj_geographymaster.Deleted_geoaccess_IDs != "")
                {
                    string[] ids = obj_geographymaster.Deleted_geoaccess_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _geoaccessService.Delete(int.Parse(id));
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

        // DELETE: api/geographymaster/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<geographymaster>> Delete(int id)
        {
            try
            {
                var result = _geographymasterService.Delete(id);
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