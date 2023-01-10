
//using nTireBO.Services;
using carditnow.Models;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class bousermenuaccessController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IbousermenuaccessService _bousermenuaccessService;
        public bousermenuaccessController(IHttpContextAccessor objhttpContextAccessor, IbousermenuaccessService obj_bousermenuaccessService, ILoggerManager logger)
        {
            _bousermenuaccessService = obj_bousermenuaccessService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _bousermenuaccessService = obj_bousermenuaccessService;
        }

        // GET: api/bousermenuaccess
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_bousermenuaccesses()
        {
            try
            {
                var result = _bousermenuaccessService.Get_bousermenuaccesses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/bousermenuaccess/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _bousermenuaccessService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("usermenuaccessid/{usermenuaccessid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_usermenuaccessid(int usermenuaccessid)
        {
            try
            {
                var result = _bousermenuaccessService.GetListBy_usermenuaccessid(usermenuaccessid);
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
        public async Task<ActionResult<bousermenuaccess>> Get_bousermenuaccess(string sid)
        {
            try
            {
                var result = _bousermenuaccessService.Get_bousermenuaccess(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/bousermenuaccess/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bousermenuaccess>> Get_bousermenuaccess(int id)
        {
            try
            {
                var result = _bousermenuaccessService.Get_bousermenuaccess(id);
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
                var result = (new { });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/bousermenuaccess
        [HttpPost]
        //saving of record
        public async Task<ActionResult<bousermenuaccess>> Post_bousermenuaccess(bousermenuaccess obj_bousermenuaccess)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _bousermenuaccessService.Save_bousermenuaccess(token, obj_bousermenuaccess);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // DELETE: api/bousermenuaccess/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<bousermenuaccess>> Delete(int id)
        {
            try
            {
                var result = _bousermenuaccessService.Delete(id);
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
