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
    public class customersecurityquestionController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomersecurityquestionService _customersecurityquestionService;
        private readonly IcustomermasterService _customermasterService;
        private readonly ImasterdataService _masterdataService;

        public customersecurityquestionController(IHttpContextAccessor objhttpContextAccessor, IcustomersecurityquestionService obj_customersecurityquestionService, IcustomermasterService obj_customermasterService, ImasterdataService obj_masterdataService, ILoggerManager logger)
        {
            _customersecurityquestionService = obj_customersecurityquestionService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _customersecurityquestionService = obj_customersecurityquestionService;
            _customermasterService = obj_customermasterService;
            _masterdataService = obj_masterdataService;
        }

        // GET: api/customersecurityquestion
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_customersecurityquestions()
        {
            try
            {
                var result = _customersecurityquestionService.Get_customersecurityquestions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customersecurityquestions()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customersecurityquestions " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/customersecurityquestion/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _customersecurityquestionService.GetFullList();
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
                var result = _customersecurityquestionService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("securityquestionid/{securityquestionid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_securityquestionid(int securityquestionid)
        {
            try
            {
                var result = _customersecurityquestionService.GetListBy_securityquestionid(securityquestionid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_securityquestionid(int securityquestionid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_securityquestionid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customersecurityquestion>> Get_customersecurityquestion(string sid)
        {
            try
            {
                var result = _customersecurityquestionService.Get_customersecurityquestion(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customersecurityquestion(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customersecurityquestion(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/customersecurityquestion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<customersecurityquestion>> Get_customersecurityquestion(int id)
        {
            try
            {
                var result = _customersecurityquestionService.Get_customersecurityquestion(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_customersecurityquestion(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customersecurityquestion(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_customerid = getList_customerid().Result.Result;
                var list_questionid = getList_questionid().Result.Result;
                var result = (new { list_customerid, list_questionid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/customersecurityquestion
        [HttpPost]
        public async Task<ActionResult<customersecurityquestion>> Post_customersecurityquestion()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                customersecurityquestionView obj_customersecurityquestion = JsonConvert.DeserializeObject<customersecurityquestionView>(Request.Form["formData"]);
                var result = _customersecurityquestionService.Save_customersecurityquestion(token, obj_customersecurityquestion.data);
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
        [Route("getList_customerid")]
        public async Task<ActionResult<dynamic>> getList_customerid()
        {
            try
            {
                string strCondition = "";
                var result = _customermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_customerid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_questionid")]
        public async Task<ActionResult<dynamic>> getList_questionid()
        {
            try
            {
                string strCondition = "";
                var result = _masterdataService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_questionid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/customersecurityquestion/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customersecurityquestion>> Delete(int id)
        {
            try
            {
                var result = _customersecurityquestionService.Delete(id);
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