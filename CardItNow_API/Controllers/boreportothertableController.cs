
//using nTireBO.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class boreportothertableController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IboreportothertableService _boreportothertableService;
        private readonly IboconfigvalueService _boconfigvalueService;
        public boreportothertableController(IHttpContextAccessor objhttpContextAccessor, IboreportothertableService obj_boreportothertableService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _boreportothertableService = obj_boreportothertableService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _boreportothertableService = obj_boreportothertableService;
            _boconfigvalueService = obj_boconfigvalueService;
        }

        // GET: api/boreportothertable
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_boreportothertables()
        {
            try
            {
                var result = _boreportothertableService.Get_boreportothertables();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/boreportothertable/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _boreportothertableService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("othertableid/{othertableid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_othertableid(int othertableid)
        {
            try
            {
                var result = _boreportothertableService.GetListBy_othertableid(othertableid);
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
        public async Task<ActionResult<boreportothertable>> Get_boreportothertable(string sid)
        {
            try
            {
                var result = _boreportothertableService.Get_boreportothertable(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/boreportothertable/5
        [HttpGet("{id}")]
        public async Task<ActionResult<boreportothertable>> Get_boreportothertable(int id)
        {
            try
            {
                var result = _boreportothertableService.Get_boreportothertable(id);
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
                var list_jointype = getList_jointype().Result.Result;
                var result = (new { list_jointype });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/boreportothertable
        [HttpPost]
        //saving of record
        public async Task<ActionResult<boreportothertable>> Post_boreportothertable(boreportothertable obj_boreportothertable)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _boreportothertableService.Save_boreportothertable(token, obj_boreportothertable);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_jointype")]
        public async Task<ActionResult<dynamic>> getList_jointype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("jointype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/boreportothertable/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<boreportothertable>> Delete(int id)
        {
            try
            {
                var result = _boreportothertableService.Delete(id);
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
