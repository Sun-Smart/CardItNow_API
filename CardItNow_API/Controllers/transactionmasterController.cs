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
    [Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class transactionmasterController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly ItransactionmasterService _transactionmasterService;
        private readonly IcustomermasterService _customermasterService;
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly IcustomerpaymodeService _customerpaymodeService;
        private readonly ItransactiondetailService _transactiondetailService;

        public transactionmasterController(IHttpContextAccessor objhttpContextAccessor, ItransactionmasterService obj_transactionmasterService, IcustomermasterService obj_customermasterService, IboconfigvalueService obj_boconfigvalueService, IcustomerpaymodeService obj_customerpaymodeService, ItransactiondetailService obj_transactiondetailService, ILoggerManager logger)
        {
            _transactionmasterService = obj_transactionmasterService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _transactionmasterService = obj_transactionmasterService;
            _customermasterService = obj_customermasterService;
            _boconfigvalueService = obj_boconfigvalueService;
            _customerpaymodeService = obj_customerpaymodeService;
            _transactiondetailService = obj_transactiondetailService;
        }

        // GET: api/transactionmaster
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_transactionmasters()
        {
            try
            {
                var result = _transactionmasterService.Get_transactionmasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_transactionmasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactionmasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/transactionmaster/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _transactionmasterService.GetFullList();
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
                var result = _transactionmasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("transactionid/{transactionid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_transactionid(int transactionid)
        {
            try
            {
                var result = _transactionmasterService.GetListBy_transactionid(transactionid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_transactionid(int transactionid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_transactionid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<transactionmaster>> Get_transactionmaster(string sid)
        {
            try
            {
                var result = _transactionmasterService.Get_transactionmaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_transactionmaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactionmaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/transactionmaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<transactionmaster>> Get_transactionmaster(int id)
        {
            try
            {
                var result = _transactionmasterService.Get_transactionmaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_transactionmaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_transactionmaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_uid = getList_uid().Result.Result;
                var list_recipientuid = getList_recipientuid().Result.Result;
                var list_transactiontype = getList_transactiontype().Result.Result;
                var list_payid = getList_payid().Result.Result;
                var list_paytype = getList_paytype().Result.Result;
                var result = (new { list_uid, list_recipientuid, list_transactiontype, list_payid, list_paytype });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/transactionmaster
        [HttpPost]
        public async Task<ActionResult<transactionmaster>> Post_transactionmaster()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                transactionmasterView obj_transactionmaster = JsonConvert.DeserializeObject<transactionmasterView>(Request.Form["formData"]);
                var result = _transactionmasterService.Save_transactionmaster(token, obj_transactionmaster.data);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                if (obj_transactionmaster.transactiondetails != null && obj_transactionmaster.transactiondetails.Count > 0)
                {
                    foreach (var obj in obj_transactionmaster.transactiondetails)
                    {
                        if (obj.transactiondetailid == null)
                        {
                            obj.transactionid = result.transactionmaster.transactionid;
                            _transactiondetailService.Save_transactiondetail(token, obj);
                        }
                    }
                }
                if (obj_transactionmaster.Deleted_transactiondetail_IDs != null && obj_transactionmaster.Deleted_transactiondetail_IDs != "")
                {
                    string[] ids = obj_transactionmaster.Deleted_transactiondetail_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _transactiondetailService.Delete(int.Parse(id));
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
        [Route("getList_recipientuid")]
        public async Task<ActionResult<dynamic>> getList_recipientuid()
        {
            try
            {
                string strCondition = "";
                var result = _customermasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_recipientuid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_transactiontype")]
        public async Task<ActionResult<dynamic>> getList_transactiontype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("transactiontype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_transactiontype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_payid")]
        public async Task<ActionResult<dynamic>> getList_payid()
        {
            try
            {
                string strCondition = "";
                var result = _customerpaymodeService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_payid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_paytype")]
        public async Task<ActionResult<dynamic>> getList_paytype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("paytype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_paytype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/transactionmaster/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<transactionmaster>> Delete(int id)
        {
            try
            {
                var result = _transactionmasterService.Delete(id);
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