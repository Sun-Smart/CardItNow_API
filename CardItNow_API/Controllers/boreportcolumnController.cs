
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
using carditnow.Models;
using carditnow.Services;

namespace nTireBO.Controllers
{
    [Authorize]
    [Route("carditnowapi/[controller]")]

    [ApiController]
    public class boreportcolumnController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IboreportcolumnService _boreportcolumnService;
        private readonly IboconfigvalueService _boconfigvalueService;
        public boreportcolumnController(IHttpContextAccessor objhttpContextAccessor, IboreportcolumnService obj_boreportcolumnService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _boreportcolumnService = obj_boreportcolumnService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _boreportcolumnService = obj_boreportcolumnService;
            _boconfigvalueService = obj_boconfigvalueService;
        }

        // GET: api/boreportcolumn
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_boreportcolumns()
        {
            try
            {
                var result = _boreportcolumnService.Get_boreportcolumns();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/boreportcolumn/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _boreportcolumnService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("reportcolumnid/{reportcolumnid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_reportcolumnid(int reportcolumnid)
        {
            try
            {
                var result = _boreportcolumnService.GetListBy_reportcolumnid(reportcolumnid);
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
        public async Task<ActionResult<boreportcolumn>> Get_boreportcolumn(string sid)
        {
            try
            {
                var result = _boreportcolumnService.Get_boreportcolumn(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/boreportcolumn/5
        [HttpGet("{id}")]
        public async Task<ActionResult<boreportcolumn>> Get_boreportcolumn(int id)
        {
            try
            {
                var result = _boreportcolumnService.Get_boreportcolumn(id);
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
                var list_datatype = getList_datatype().Result.Result;
                var list_filtertype = getList_filtertype().Result.Result;
                var result = (new { list_datatype, list_filtertype });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/boreportcolumn
        [HttpPost]
        //saving of record
        public async Task<ActionResult<boreportcolumn>> Post_boreportcolumn(boreportcolumn obj_boreportcolumn)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _boreportcolumnService.Save_boreportcolumn(token, obj_boreportcolumn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_datatype")]
        public async Task<ActionResult<dynamic>> getList_datatype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("datatype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_filtertype")]
        public async Task<ActionResult<dynamic>> getList_filtertype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("filtertype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/boreportcolumn/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<boreportcolumn>> Delete(int id)
        {
            try
            {
                var result = _boreportcolumnService.Delete(id);
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
