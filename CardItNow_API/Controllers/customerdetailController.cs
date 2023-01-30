using carditnow.Models;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nTireBO.Services;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace carditnow.Controllers
{
   // [Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class customerdetailController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomerdetailService _customerdetailService;
        private readonly IcustomermasterService _customermasterService;
        private readonly IgeographymasterService _geographymasterService;
        private readonly IcitymasterService _citymasterService;
        private readonly IboconfigvalueService _boconfigvalueService;

        public customerdetailController(IHttpContextAccessor objhttpContextAccessor, IcustomerdetailService obj_customerdetailService, IcustomermasterService obj_customermasterService, IgeographymasterService obj_geographymasterService, IcitymasterService obj_citymasterService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _customerdetailService = obj_customerdetailService;
            _logger = logger;
            if (objhttpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
                _customerdetailService = obj_customerdetailService;
                _customermasterService = obj_customermasterService;
                _geographymasterService = obj_geographymasterService;
                _citymasterService = obj_citymasterService;
                _boconfigvalueService = obj_boconfigvalueService;
            }
        }

        // GET: api/customerdetail
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_customerdetails()
        {
            try
            {
                var result = _customerdetailService.Get_customerdetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customerdetails()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerdetails " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/customerdetail/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _customerdetailService.GetFullList();
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
                var result = _customerdetailService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("customerdetailid/{customerdetailid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_customerdetailid(int customerdetailid)
        {
            try
            {
                var result = _customerdetailService.GetListBy_customerdetailid(customerdetailid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_customerdetailid(int customerdetailid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_customerdetailid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customerdetail>> Get_customerdetail(string sid)
        {
            try
            {
                var result = _customerdetailService.Get_customerdetail(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customerdetail(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerdetail(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/customerdetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<customerdetail>> Get_customerdetail(int id)
        {
            try
            {
                var result = _customerdetailService.Get_customerdetail(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_customerdetail(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customerdetail(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_customerid = getList_customerid().Result.Result;
                var list_uid = getList_uid().Result.Result;
                var list_geoid = getList_geoid().Result.Result;
                var list_divmode = getList_divmode().Result.Result;
                var list_divstatus = getList_divstatus().Result.Result;
                var list_amlcheckstatus = getList_amlcheckstatus().Result.Result;
                var result = (new { list_customerid, list_uid, list_geoid, list_divmode, list_divstatus, list_amlcheckstatus });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }
        [HttpPost]
        [Route("ProcessOCR")]
        public async Task<ActionResult<ProcessOCRResposeView>> ProcessOCR(customerdetail model)
        {
            ProcessOCRResposeView response = new ProcessOCRResposeView();
            try
            {
                response = _customerdetailService.ProcessOCR(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Save api {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
            }

        }
        // POST: api/customerdetail
        [HttpPost]
        public async Task<ActionResult<customerdetail>> Post_customerdetail()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                customerdetailView obj_customerdetail = JsonConvert.DeserializeObject<customerdetailView>(Request.Form["formData"]);
                var result = _customerdetailService.Save_customerdetail(token, obj_customerdetail.data);
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
        [Route("getList_uid")]
        public async Task<ActionResult<dynamic>> getList_uid()
        {
            try
            {
                string strCondition = "";
                var result = _customermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_uid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
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
        [Route("getList_cityid/{geoid}")]
        public async Task<ActionResult<dynamic>> getList_cityid(int geoid)
        {
            try
            {
                var result = _citymasterService.GetListBy_geoid(geoid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_cityid(int geoid) {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_divmode")]
        public async Task<ActionResult<dynamic>> getList_divmode()
        {
            try
            {
                var result = _boconfigvalueService.GetList("divmode");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_divmode() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_divstatus")]
        public async Task<ActionResult<dynamic>> getList_divstatus()
        {
            try
            {
                var result = _boconfigvalueService.GetList("divstatus");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_divstatus() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_amlcheckstatus")]
        public async Task<ActionResult<dynamic>> getList_amlcheckstatus()
        {
            try
            {
                var result = _boconfigvalueService.GetList("amlcheckstatus");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_amlcheckstatus() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/customerdetail/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customerdetail>> Delete(int id)
        {
            try
            {
                var result = _customerdetailService.Delete(id);
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