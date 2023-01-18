
//using nTireBO.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nTireBO.Models;
using nTireBO.Services;
using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBiz.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class boreportdetailController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IboreportdetailService _boreportdetailService;
        private readonly IboconfigvalueService _boconfigvalueService;
        public boreportdetailController(IHttpContextAccessor objhttpContextAccessor, IboreportdetailService obj_boreportdetailService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _boreportdetailService = obj_boreportdetailService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _boreportdetailService = obj_boreportdetailService;
            _boconfigvalueService = obj_boconfigvalueService;
        }

        // GET: api/boreportdetail
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_boreportdetails()
        {
            try
            {
                var result = _boreportdetailService.Get_boreportdetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/boreportdetail/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _boreportdetailService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("reportdetailid/{reportdetailid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_reportdetailid(int reportdetailid)
        {
            try
            {
                var result = _boreportdetailService.GetListBy_reportdetailid(reportdetailid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("e/{sid}")]
        //used in getting the record. parameter is encrypted id  
        public async Task<ActionResult<boreportdetail>> Get_boreportdetail(string sid)
        {
            try
            {
                var result = _boreportdetailService.Get_boreportdetail(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/boreportdetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<boreportdetail>> Get_boreportdetail(int id)
        {
            try
            {
                var result = _boreportdetailService.Get_boreportdetail(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_separator = getList_separator().Result.Result;
                var result = (new { list_separator });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/boreportdetail
        [HttpPost]
        //saving of record
        public async Task<ActionResult<boreportdetail>> Post_boreportdetail(boreportdetail obj_boreportdetail)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _boreportdetailService.Save_boreportdetail(token, obj_boreportdetail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_separator")]
        public async Task<ActionResult<dynamic>> getList_separator()
        {
            try
            {
                var result = _boconfigvalueService.GetList("separator");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/boreportdetail/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<boreportdetail>> Delete(int id)
        {
            try
            {
                var result = _boreportdetailService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
