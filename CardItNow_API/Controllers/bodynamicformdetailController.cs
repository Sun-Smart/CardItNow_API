
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
    public class bodynamicformdetailController : ControllerBase
    {
        private ILoggerManager _logger;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        private readonly IbodynamicformdetailService _bodynamicformdetailService;
        private readonly IboconfigvalueService _boconfigvalueService;
        public bodynamicformdetailController(IHttpContextAccessor objhttpContextAccessor, IbodynamicformdetailService obj_bodynamicformdetailService, IboconfigvalueService obj_boconfigvalueService, ILoggerManager logger)
        {
            _bodynamicformdetailService = obj_bodynamicformdetailService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _bodynamicformdetailService = obj_bodynamicformdetailService;
            _boconfigvalueService = obj_boconfigvalueService;
        }

        // GET: api/bodynamicformdetail
        [HttpGet]
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public async Task<ActionResult<dynamic>> Get_bodynamicformdetails()
        {
            try
            {
                var result = _bodynamicformdetailService.Get_bodynamicformdetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // PUT: api/bodynamicformdetail/5
        [HttpGet]
        //Dump of the table.If param field exists, filter by param 
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _bodynamicformdetailService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("formdetailid/{formdetailid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_formdetailid(int formdetailid)
        {
            try
            {
                var result = _bodynamicformdetailService.GetListBy_formdetailid(formdetailid);
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
        public async Task<ActionResult<bodynamicformdetail>> Get_bodynamicformdetail(string sid)
        {
            try
            {
                var result = _bodynamicformdetailService.Get_bodynamicformdetail(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // GET: api/bodynamicformdetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<bodynamicformdetail>> Get_bodynamicformdetail(int id)
        {
            try
            {
                var result = _bodynamicformdetailService.Get_bodynamicformdetail(id);
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
                var list_controltype = getList_controltype().Result.Result;
                var result = (new { list_controltype });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // POST: api/bodynamicformdetail
        [HttpPost]
        //saving of record
        public async Task<ActionResult<bodynamicformdetail>> Post_bodynamicformdetail(bodynamicformdetail obj_bodynamicformdetail)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                var result = _bodynamicformdetailService.Save_bodynamicformdetail(token, obj_bodynamicformdetail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("getList_controltype")]
        public async Task<ActionResult<dynamic>> getList_controltype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("controltype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/bodynamicformdetail/5
        [HttpDelete]
        [Route("{id}")]
        //delete process
        public async Task<ActionResult<bodynamicformdetail>> Delete(int id)
        {
            try
            {
                var result = _bodynamicformdetailService.Delete(id);
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
