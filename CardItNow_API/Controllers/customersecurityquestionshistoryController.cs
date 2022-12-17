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
    public class customersecurityquestionshistoryController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomersecurityquestionshistoryService _customersecurityquestionshistoryService;
        private readonly IcustomermasterService _customermasterService;
        private readonly ImasterdataService _masterdataService;

        public customersecurityquestionshistoryController(IHttpContextAccessor objhttpContextAccessor, IcustomersecurityquestionshistoryService obj_customersecurityquestionshistoryService, IcustomermasterService obj_customermasterService, ImasterdataService obj_masterdataService, ILoggerManager logger)
        {
            _customersecurityquestionshistoryService = obj_customersecurityquestionshistoryService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _customersecurityquestionshistoryService = obj_customersecurityquestionshistoryService;
            _customermasterService = obj_customermasterService;
            _masterdataService = obj_masterdataService;
        }

        // GET: api/customersecurityquestionshistory
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_customersecurityquestionshistories()
        {
            try
            {
                var result = _customersecurityquestionshistoryService.Get_customersecurityquestionshistories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customersecurityquestionshistories()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customersecurityquestionshistories " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/customersecurityquestionshistory/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _customersecurityquestionshistoryService.GetFullList();
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
                var result = _customersecurityquestionshistoryService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("historyid/{historyid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_historyid(int historyid)
        {
            try
            {
                var result = _customersecurityquestionshistoryService.GetListBy_historyid(historyid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_historyid(int historyid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_historyid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customersecurityquestionshistory>> Get_customersecurityquestionshistory(string sid)
        {
            try
            {
                var result = _customersecurityquestionshistoryService.Get_customersecurityquestionshistory(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customersecurityquestionshistory(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customersecurityquestionshistory(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/customersecurityquestionshistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<customersecurityquestionshistory>> Get_customersecurityquestionshistory(int id)
        {
            try
            {
                var result = _customersecurityquestionshistoryService.Get_customersecurityquestionshistory(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_customersecurityquestionshistory(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customersecurityquestionshistory(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_customerid = getList_customerid().Result.Result;
                var list_securityquestionid = getList_securityquestionid().Result.Result;
                var result = (new { list_customerid, list_securityquestionid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/customersecurityquestionshistory
        [HttpPost]
        public async Task<ActionResult<customersecurityquestionshistory>> Post_customersecurityquestionshistory()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                customersecurityquestionshistoryView obj_customersecurityquestionshistory = JsonConvert.DeserializeObject<customersecurityquestionshistoryView>(Request.Form["formData"]);
                var result = _customersecurityquestionshistoryService.Save_customersecurityquestionshistory(token, obj_customersecurityquestionshistory.data);
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
        [Route("getList_securityquestionid")]
        public async Task<ActionResult<dynamic>> getList_securityquestionid()
        {
            try
            {
                string strCondition = "";
                var result = _masterdataService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_securityquestionid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/customersecurityquestionshistory/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customersecurityquestionshistory>> Delete(int id)
        {
            try
            {
                var result = _customersecurityquestionshistoryService.Delete(id);
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