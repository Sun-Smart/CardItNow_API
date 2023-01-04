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
    public class termsmasterController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ItermsmasterService _termsmasterService;

        public termsmasterController(IHttpContextAccessor objhttpContextAccessor, ItermsmasterService obj_termsmasterService, ILoggerManager logger)
        {
            _termsmasterService = obj_termsmasterService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _termsmasterService = obj_termsmasterService;
        }

        // GET: api/termsmaster
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_termsmasters()
        {
            try
            {
                var result = _termsmasterService.Get_termsmasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_termsmasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_termsmasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        

        // PUT: api/termsmaster/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _termsmasterService.GetFullList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        //[HttpGet]
        //[Route("NoAuthFullList")]
        //public async Task<ActionResult<IEnumerable<Object>>> Get_NoAuthFullList()
        //{
        //    try
        //    {
        //        var result = _termsmasterService.Get_NoAuthFullList();
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Controller: GetFullList() \r\n{ex}");
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
        //    }
        //}

        [HttpGet]
        //Dump of the table.If param field exists, filter by param
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _termsmasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("termid/{termid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_termid(int termid)
        {
            try
            {
                var result = _termsmasterService.GetListBy_termid(termid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_termid(int termid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_termid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<termsmaster>> Get_termsmaster(string sid)
        {
            try
            {
                var result = _termsmasterService.Get_termsmaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_termsmaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_termsmaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/termsmaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<termsmaster>> Get_termsmaster(int id)
        {
            try
            {
                var result = _termsmasterService.Get_termsmaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_termsmaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_termsmaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
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

        // POST: api/termsmaster
        [HttpPost]
        public async Task<ActionResult<termsmaster>> Post_termsmaster()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                termsmasterView obj_termsmaster = JsonConvert.DeserializeObject<termsmasterView>(Request.Form["formData"]);
                var result = _termsmasterService.Save_termsmaster(token, obj_termsmaster.data);
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

        // DELETE: api/termsmaster/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<termsmaster>> Delete(int id)
        {
            try
            {
                var result = _termsmasterService.Delete(id);
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