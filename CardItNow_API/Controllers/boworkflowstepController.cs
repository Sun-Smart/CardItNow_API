
//using nTireBO.Services;
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
    public class boworkflowstepController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IboworkflowstepService _boworkflowstepService;
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly IbodynamicformService _bodynamicformService;
        public boworkflowstepController(IHttpContextAccessor objhttpContextAccessor, IboworkflowstepService obj_boworkflowstepService, IboconfigvalueService obj_boconfigvalueService, IbodynamicformService obj_bodynamicformService, ILoggerManager logger)
        {
            _boworkflowstepService = obj_boworkflowstepService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _boworkflowstepService = obj_boworkflowstepService;
            _boconfigvalueService = obj_boconfigvalueService;
            _bodynamicformService = obj_bodynamicformService;
        }

        // GET: api/boworkflowstep
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_boworkflowsteps()
        {
            try
            {
                var result = _boworkflowstepService.Get_boworkflowsteps();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/boworkflowstep/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _boworkflowstepService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("workflowstepid/{workflowstepid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_workflowstepid(int workflowstepid)
        {
            try
            {
                var result = _boworkflowstepService.GetListBy_workflowstepid(workflowstepid);
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
        public async Task<ActionResult<boworkflowstep>> Get_boworkflowstep(string sid)
        {
            try
            {
                var result = _boworkflowstepService.Get_boworkflowstep(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/boworkflowstep/5
        [HttpGet("{id}")]
        public async Task<ActionResult<boworkflowstep>> Get_boworkflowstep(int id)
        {
            try
            {
                var result = _boworkflowstepService.Get_boworkflowstep(id);
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
                var list_task = getList_task().Result.Result;
                var list_yesstep = getList_yesstep().Result.Result;
                var list_nostep = getList_nostep().Result.Result;
                var list_workflowuserfieldtype = getList_workflowuserfieldtype().Result.Result;
                var list_parentid = getList_parentid().Result.Result;
                var list_customfieldid = getList_customfieldid().Result.Result;
                var result = (new { list_task, list_yesstep, list_nostep, list_workflowuserfieldtype, list_parentid, list_customfieldid });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/boworkflowstep
        [HttpPost]
        //saving of record
        public async Task<ActionResult<boworkflowstep>> Post_boworkflowstep(boworkflowstep obj_boworkflowstep)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _boworkflowstepService.Save_boworkflowstep(token, obj_boworkflowstep);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_task")]
        public async Task<ActionResult<dynamic>> getList_task()
        {
            try
            {
                var result = _boconfigvalueService.GetList("workflowtask");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_yesstep")]
        public async Task<ActionResult<dynamic>> getList_yesstep()
        {
            try
            {
                var result = _boworkflowstepService.Get_boworkflowsteps();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_nostep")]
        public async Task<ActionResult<dynamic>> getList_nostep()
        {
            try
            {
                var result = _boworkflowstepService.Get_boworkflowsteps();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_workflowuserfieldtype")]
        public async Task<ActionResult<dynamic>> getList_workflowuserfieldtype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("workflowuserfieldtype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_parentid")]
        public async Task<ActionResult<dynamic>> getList_parentid()
        {
            try
            {
                var result = _boworkflowstepService.Get_boworkflowsteps();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getList_customfieldid")]
        public async Task<ActionResult<dynamic>> getList_customfieldid()
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

        // DELETE: api/boworkflowstep/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<boworkflowstep>> Delete(int id)
        {
            try
            {
                var result = _boworkflowstepService.Delete(id);
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
