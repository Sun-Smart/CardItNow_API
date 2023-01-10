
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
    public class botemplateController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IbotemplateService _botemplateService;
        private readonly IboconfigvalueService _boconfigvalueService;
        public botemplateController(IHttpContextAccessor objhttpContextAccessor, IbotemplateService obj_botemplateService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _botemplateService = obj_botemplateService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _botemplateService = obj_botemplateService;
            _boconfigvalueService = obj_boconfigvalueService;
        }

        // GET: api/botemplate
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_botemplates()
        {
            try
            {
                var result = _botemplateService.Get_botemplates();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/botemplate/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _botemplateService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("templateid/{templateid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_templateid(int templateid)
        {
            try
            {
                var result = _botemplateService.GetListBy_templateid(templateid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("templatecode/{templatecode}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_templatecode(string templatecode)
        {
            try
            {
                var result = _botemplateService.GetListBy_templatecode(templatecode);
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
        public async Task<ActionResult<botemplate>> Get_botemplate(string sid)
        {
            try
            {
                var result = _botemplateService.Get_botemplate(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/botemplate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<botemplate>> Get_botemplate(int id)
        {
            try
            {
                var result = _botemplateService.Get_botemplate(id);
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
                var list_templatetype = getList_templatetype().Result.Result;
                var result = (new { list_templatetype });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/botemplate
        [HttpPost]
        //saving of record
        public async Task<ActionResult<botemplate>> Post_botemplate(botemplate obj_botemplate)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _botemplateService.Save_botemplate(token, obj_botemplate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_templatetype")]
        public async Task<ActionResult<dynamic>> getList_templatetype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("templatetype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/botemplate/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<botemplate>> Delete(int id)
        {
            try
            {
                var result = _botemplateService.Delete(id);
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
