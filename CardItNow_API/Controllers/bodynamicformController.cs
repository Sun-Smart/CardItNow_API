
//using nTireBO.Services;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nTireBiz.Services;
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
    public class bodynamicformController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IbodynamicformService _bodynamicformService;
        private readonly IsystemtableService _systemtableService;
        private readonly IboconfigvalueService _boconfigvalueService;
        public bodynamicformController(IHttpContextAccessor objhttpContextAccessor, IbodynamicformService obj_bodynamicformService, IsystemtableService obj_systemtableService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _bodynamicformService = obj_bodynamicformService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _bodynamicformService = obj_bodynamicformService;
            _systemtableService = obj_systemtableService;
            _boconfigvalueService = obj_boconfigvalueService;
        }

        // GET: api/bodynamicform
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_bodynamicforms()
        {
            try
            {
                var result = _bodynamicformService.Get_bodynamicforms();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/bodynamicform/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _bodynamicformService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("formid/{formid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_formid(int formid)
        {
            try
            {
                var result = _bodynamicformService.GetListBy_formid(formid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("tableiddesc/{tableiddesc}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_tableiddesc(string tableiddesc)
        {
            try
            {
                var result = _bodynamicformService.GetListBy_tableiddesc(tableiddesc);
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
        public async Task<ActionResult<bodynamicform>> Get_bodynamicform(string sid)
        {
            try
            {
                var result = _bodynamicformService.Get_bodynamicform(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/bodynamicform/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bodynamicform>> Get_bodynamicform(int id)
        {
            try
            {
                var result = _bodynamicformService.Get_bodynamicform(id);
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
                var list_tableid = getList_tableid().Result.Result;
                var list_formtype = getList_formtype().Result.Result;
                var result = (new { list_tableid, list_formtype });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/bodynamicform
        [HttpPost]
        //saving of record
        public async Task<ActionResult<bodynamicform>> Post_bodynamicform(bodynamicform obj_bodynamicform)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _bodynamicformService.Save_bodynamicform(token, obj_bodynamicform);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_tableid")]
        public async Task<ActionResult<dynamic>> getList_tableid()
        {
            try
            {
                var result = _systemtableService.Get_systemtables();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_formtype")]
        public async Task<ActionResult<dynamic>> getList_formtype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("formtype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/bodynamicform/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<bodynamicform>> Delete(int id)
        {
            try
            {
                var result = _bodynamicformService.Delete(id);
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
