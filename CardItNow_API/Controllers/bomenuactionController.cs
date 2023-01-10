
//using nTireBO.Services;
using carditnow.Models;
using carditnow.Services;
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

namespace carditnow.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class bomenuactionController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IbomenuactionService _bomenuactionService;
        private readonly IboconfigvalueService _boconfigvalueService;
        public bomenuactionController(IHttpContextAccessor objhttpContextAccessor, IbomenuactionService obj_bomenuactionService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _bomenuactionService = obj_bomenuactionService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _bomenuactionService = obj_bomenuactionService;
            _boconfigvalueService = obj_boconfigvalueService;
        }

        // GET: api/bomenuaction
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_bomenuactions()
        {
            try
            {
                var result = _bomenuactionService.Get_bomenuactions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/bomenuaction/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _bomenuactionService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("actionid/{actionid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_actionid(int actionid)
        {
            try
            {
                var result = _bomenuactionService.GetListBy_actionid(actionid);
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
        public async Task<ActionResult<bomenuaction>> Get_bomenuaction(string sid)
        {
            try
            {
                var result = _bomenuactionService.Get_bomenuaction(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/bomenuaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bomenuaction>> Get_bomenuaction(int id)
        {
            try
            {
                var result = _bomenuactionService.Get_bomenuaction(id);
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
                var list_rowselecttype = getList_rowselecttype().Result.Result;
                var list_actiontype = getList_actiontype().Result.Result;
                var result = (new { list_rowselecttype, list_actiontype });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/bomenuaction
        [HttpPost]
        //saving of record
        public async Task<ActionResult<bomenuaction>> Post_bomenuaction(bomenuaction obj_bomenuaction)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _bomenuactionService.Save_bomenuaction(token, obj_bomenuaction);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_rowselecttype")]
        public async Task<ActionResult<dynamic>> getList_rowselecttype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("rowselecttype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_actiontype")]
        public async Task<ActionResult<dynamic>> getList_actiontype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("actiontype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/bomenuaction/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<bomenuaction>> Delete(int id)
        {
            try
            {
                var result = _bomenuactionService.Delete(id);
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
