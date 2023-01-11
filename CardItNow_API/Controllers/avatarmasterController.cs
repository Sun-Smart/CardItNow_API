using carditnow.Models;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace carditnow.Controllers
{
    //[Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class avatarmasterController : baseController
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IavatarmasterService _avatarmasterService;


        
        public avatarmasterController(IHttpContextAccessor objhttpContextAccessor, IavatarmasterService obj_avatarmasterService, ILoggerManager logger):base(logger)
        {
            
            _avatarmasterService = obj_avatarmasterService;
            _logger = logger;
            //cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            //uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _avatarmasterService = obj_avatarmasterService;
        }


        // GET: api/avatarmaster
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_avatarmasters()
        {
            try
            {
                var result = _avatarmasterService.Get_avatarmasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_avatarmasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_avatarmasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }


        // PUT: api/avatarmaster/5
        //[Authorize]
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _avatarmasterService.GetFullList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [Authorize]
        [HttpGet]
        //Dump of the table.If param field exists, filter by param
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _avatarmasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("avatarid/{avatarid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_avatarid(int avatarid)
        {
            try
            {
                var result = _avatarmasterService.GetListBy_avatarid(avatarid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_avatarid(int avatarid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_avatarid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("GetImages")]
        public dynamic GetImages()
        {
                var result = _avatarmasterService.GetImages();
                return Ok(result);
        }

        [HttpPost]
        [Route("UploadSelfiJson")]
        public dynamic UploadSelfiJson([FromBody]JsonMobile model)
        {
            string sname = JsonConvert.SerializeObject(model);
            var result =_avatarmasterService.UploadSelfiJson(sname);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("e/{sid}")]
        public async Task<ActionResult<avatarmaster>> Get_avatarmaster(string sid)
        {
            try
            {
                var result = _avatarmasterService.Get_avatarmaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_avatarmaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_avatarmaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/avatarmaster/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<avatarmaster>> Get_avatarmaster(int id)
        {
            try
            {
                var result = _avatarmasterService.Get_avatarmaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_avatarmaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_avatarmaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        //[Authorize]
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

        // POST: api/avatarmaster
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<avatarmaster>> Post_avatarmaster()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                avatarmasterView obj_avatarmaster = JsonConvert.DeserializeObject<avatarmasterView>(Request.Form["formData"]);
                var result = _avatarmasterService.Save_avatarmaster(token, obj_avatarmaster.data);
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

        // DELETE: api/avatarmaster/5
        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<avatarmaster>> Delete(int id)
        {
            try
            {
                var result = _avatarmasterService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Delete(int id) {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Delete " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        [Route("UploadSelfi")]
        public async Task<ActionResult<string>> UploadSelfi([FromForm] avatarUploadRequestViewModel model)
        {
            try
            {
                if (model.ImageFile.Length > 0)
                {

                    string result = await _avatarmasterService.UploadSelfi(model);
                    if (!string.IsNullOrEmpty(result))
                        return Ok(result);
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,"Image file is Required");
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex, "UploadSelfi");
            }
        }
    }
}