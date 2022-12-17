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
    public class carditchargesdiscountController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcarditchargesdiscountService _carditchargesdiscountService;
        private readonly IcustomermasterService _customermasterService;

        public carditchargesdiscountController(IHttpContextAccessor objhttpContextAccessor, IcarditchargesdiscountService obj_carditchargesdiscountService, IcustomermasterService obj_customermasterService, ILoggerManager logger)
        {
            _carditchargesdiscountService = obj_carditchargesdiscountService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _carditchargesdiscountService = obj_carditchargesdiscountService;
            _customermasterService = obj_customermasterService;
        }

        // GET: api/carditchargesdiscount
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_carditchargesdiscounts()
        {
            try
            {
                var result = _carditchargesdiscountService.Get_carditchargesdiscounts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_carditchargesdiscounts()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_carditchargesdiscounts " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/carditchargesdiscount/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _carditchargesdiscountService.GetFullList();
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
                var result = _carditchargesdiscountService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("discountid/{discountid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_discountid(int discountid)
        {
            try
            {
                var result = _carditchargesdiscountService.GetListBy_discountid(discountid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_discountid(int discountid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_discountid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<carditchargesdiscount>> Get_carditchargesdiscount(string sid)
        {
            try
            {
                var result = _carditchargesdiscountService.Get_carditchargesdiscount(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_carditchargesdiscount(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_carditchargesdiscount(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/carditchargesdiscount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<carditchargesdiscount>> Get_carditchargesdiscount(int id)
        {
            try
            {
                var result = _carditchargesdiscountService.Get_carditchargesdiscount(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_carditchargesdiscount(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_carditchargesdiscount(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_recipientuid = getList_recipientuid().Result.Result;
                var result = (new { list_recipientuid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/carditchargesdiscount
        [HttpPost]
        public async Task<ActionResult<carditchargesdiscount>> Post_carditchargesdiscount()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                carditchargesdiscountView obj_carditchargesdiscount = JsonConvert.DeserializeObject<carditchargesdiscountView>(Request.Form["formData"]);
                var result = _carditchargesdiscountService.Save_carditchargesdiscount(token, obj_carditchargesdiscount.data);
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

        // DELETE: api/carditchargesdiscount/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<carditchargesdiscount>> Delete(int id)
        {
            try
            {
                var result = _carditchargesdiscountService.Delete(id);
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