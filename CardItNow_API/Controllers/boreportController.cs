using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nTireBO.Models;
using nTireBO.Services;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace nTireBO.Controllers
{
    [Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class boreportController : ControllerBase
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly nTireBO.Services.IboreportService _boreportService;
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly nTireBO.Services.IbodashboardService _bodashboardService;
        private readonly nTireBO.Services.IboreportdetailService _boreportdetailService;
        private readonly nTireBO.Services.IboreportothertableService _boreportothertableService;
        private readonly nTireBO.Services.IboreportcolumnService _boreportcolumnService;

        public boreportController(IHttpContextAccessor objhttpContextAccessor, IboreportService obj_boreportService, IboconfigvalueService obj_boconfigvalueService, nTireBO.Services.IbodashboardService obj_bodashboardService, nTireBO.Services.IboreportdetailService obj_boreportdetailService, nTireBO.Services.IboreportothertableService obj_boreportothertableService, nTireBO.Services.IboreportcolumnService obj_boreportcolumnService, ILoggerManager logger)
        {
            _boreportService = obj_boreportService;
            _logger = logger;
            cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            _boreportService = obj_boreportService;
            _boconfigvalueService = obj_boconfigvalueService;
            _bodashboardService = obj_bodashboardService;
            _boreportdetailService = obj_boreportdetailService;
            _boreportothertableService = obj_boreportothertableService;
            _boreportcolumnService = obj_boreportcolumnService;
        }

        // GET: api/boreport
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_boreports()
        {
            try
            {
                var result = _boreportService.Get_boreports();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_boreports()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_boreports " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/boreport/5
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _boreportService.GetFullList();
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
                var result = _boreportService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("reportid/{reportid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_reportid(int reportid)
        {
            try
            {
                var result = _boreportService.GetListBy_reportid(reportid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_reportid(int reportid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_reportid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("reportcode/{reportcode}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_reportcode(string reportcode)
        {
            try
            {
                var result = _boreportService.GetListBy_reportcode(reportcode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_reportcode(string reportcode)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_reportcode " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet("e/{sid}")]
        public async Task<ActionResult<boreport>> Get_boreport(string sid)
        {
            try
            {
                var result = _boreportService.Get_boreport(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_boreport(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_boreport(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/boreport/5
        [HttpGet("{id}")]
        public async Task<ActionResult<boreport>> Get_boreport(int id)
        {
            try
            {
                var result = _boreportService.Get_boreport(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_boreport(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_boreport(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_reportmodule = getList_reportmodule().Result.Result;
                var list_reporttype = getList_reporttype().Result.Result;
                var list_sidefiltertype = getList_sidefiltertype().Result.Result;
                var list_datefiltertype = getList_datefiltertype().Result.Result;
                var list_groupbyrelationship = getList_groupbyrelationship().Result.Result;
                var list_jointype = getList_jointype().Result.Result;
                var list_reportoutputtype = getList_reportoutputtype().Result.Result;
                var list_viewhtmltype = getList_viewhtmltype().Result.Result;
                var list_workflowhtmltype = getList_workflowhtmltype().Result.Result;
                var list_recordtype = getList_recordtype().Result.Result;
                var list_dashboardid = getList_dashboardid().Result.Result;
                var list_schedule = getList_schedule().Result.Result;
                var result = (new { list_reportmodule, list_reporttype, list_sidefiltertype, list_datefiltertype, list_groupbyrelationship, list_jointype, list_reportoutputtype, list_viewhtmltype, list_workflowhtmltype, list_recordtype, list_dashboardid, list_schedule });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/boreport
        [HttpPost]
        public async Task<ActionResult<boreport>> Post_boreport()
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                boreportView obj_boreport = JsonConvert.DeserializeObject<boreportView>(Request.Form["formData"]);
                var result = _boreportService.Save_boreport(token, obj_boreport.data);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                if (obj_boreport.boreportdetails != null && obj_boreport.boreportdetails.Count > 0)
                {
                    foreach (var obj in obj_boreport.boreportdetails)
                    {
                        if (obj.reportdetailid == null)
                        {
                            obj.reportid = result.boreport.reportid;
                            _boreportdetailService.Save_boreportdetail(token, obj);
                        }
                    }
                }
                if (obj_boreport.Deleted_boreportdetail_IDs != null && obj_boreport.Deleted_boreportdetail_IDs != "")
                {
                    string[] ids = obj_boreport.Deleted_boreportdetail_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _boreportdetailService.Delete(int.Parse(id));
                        }
                    }
                }
                if (obj_boreport.boreportothertables != null && obj_boreport.boreportothertables.Count > 0)
                {
                    foreach (var obj in obj_boreport.boreportothertables)
                    {
                        if (obj.othertableid == null)
                        {
                            obj.reportid = result.boreport.reportid;
                            _boreportothertableService.Save_boreportothertable(token, obj);
                        }
                    }
                }
                if (obj_boreport.Deleted_boreportothertable_IDs != null && obj_boreport.Deleted_boreportothertable_IDs != "")
                {
                    string[] ids = obj_boreport.Deleted_boreportothertable_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _boreportothertableService.Delete(int.Parse(id));
                        }
                    }
                }
                if (obj_boreport.boreportcolumns != null && obj_boreport.boreportcolumns.Count > 0)
                {
                    foreach (var obj in obj_boreport.boreportcolumns)
                    {
                        if (obj.reportcolumnid == null)
                        {
                            obj.reportid = result.boreport.reportid;
                            _boreportcolumnService.Save_boreportcolumn(token, obj);
                        }
                    }
                }
                if (obj_boreport.Deleted_boreportcolumn_IDs != null && obj_boreport.Deleted_boreportcolumn_IDs != "")
                {
                    string[] ids = obj_boreport.Deleted_boreportcolumn_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _boreportcolumnService.Delete(int.Parse(id));
                        }
                    }
                }
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
        [Route("getList_reportmodule")]
        public async Task<ActionResult<dynamic>> getList_reportmodule()
        {
            try
            {
                var result = _boconfigvalueService.GetList("reportmodule");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_reportmodule() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_reporttype")]
        public async Task<ActionResult<dynamic>> getList_reporttype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("reporttype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_reporttype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_sidefiltertype")]
        public async Task<ActionResult<dynamic>> getList_sidefiltertype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("sidefiltertype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_sidefiltertype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_datefiltertype")]
        public async Task<ActionResult<dynamic>> getList_datefiltertype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("datefiltertype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_datefiltertype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_groupbyrelationship")]
        public async Task<ActionResult<dynamic>> getList_groupbyrelationship()
        {
            try
            {
                var result = _boconfigvalueService.GetList("groupbyrelationship");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_groupbyrelationship() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
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
                _logger.LogError($"Controller: getList_jointype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_reportoutputtype")]
        public async Task<ActionResult<dynamic>> getList_reportoutputtype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("reportoutputtype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_reportoutputtype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_viewhtmltype")]
        public async Task<ActionResult<dynamic>> getList_viewhtmltype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("viewhtmltype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_viewhtmltype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_workflowhtmltype")]
        public async Task<ActionResult<dynamic>> getList_workflowhtmltype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("workflowhtmltype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_workflowhtmltype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_recordtype")]
        public async Task<ActionResult<dynamic>> getList_recordtype()
        {
            try
            {
                var result = _boconfigvalueService.GetList("recordtype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_recordtype() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_dashboardid")]
        public async Task<ActionResult<dynamic>> getList_dashboardid()
        {
            try
            {
                string strCondition = "";
                var result = _bodashboardService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_dashboardid() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_schedule")]
        public async Task<ActionResult<dynamic>> getList_schedule()
        {
            try
            {
                var result = _boconfigvalueService.GetList("schedule");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_schedule() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/boreport/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<boreport>> Delete(int id)
        {
            try
            {
                var result = _boreportService.Delete(id);
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