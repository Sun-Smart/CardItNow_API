using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using nTireBO.Models;
using SunSmartnTireProducts.Helpers;

//using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;
using System.Data;

/*using System.Text.Json;
using System.Text.Json.Serialization;*/

using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Services
{
    //[Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class ReportViewerController : ControllerBase
    {
        private readonly ReportViewerContext _context;

        private readonly ILogger _logger;
        private readonly IConfiguration Configuration;
        private readonly IboreportService _boreportService;
        private readonly IReportViewerService _reportviewerService;

        private int cid = 1;
        private int uid = 4;
        private string uname = "";
        private string uidemail = "";
        private int? sessionuserid = 4;
        private int? sessionbranchid = 1;
        private int? userroleid = 1;
        private int? finyearid = 1;

        //
        public ReportViewerController(IHttpContextAccessor objhttpContextAccessor, ReportViewerContext context, ILogger<ReportViewerController> logger, IReportViewerService reportviewerService, IConfiguration configuration, IboreportService objreportService)
        {
            _context = context;
            _logger = logger;
            Configuration = configuration;
            _boreportService = objreportService;
            _reportviewerService = reportviewerService;

            uname = "";
            uidemail = "";
            if (objhttpContextAccessor.HttpContext.User.Claims.Count() > 0 && objhttpContextAccessor.HttpContext.User.Claims.Count() > 0 && objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (objhttpContextAccessor.HttpContext.User.Claims.Count() > 0 && objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email").Value.ToString();

            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid") != null) cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());

            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid") != null) sessionuserid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());

            if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userroleid") != null) userroleid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userroleid").Value.ToString());

            //if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "finyearid") != null) finyearid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "finyearid").Value.ToString());
        }

        // [HttpGet("{id}/{fkname?}/{fk?}")]
        //public async Task<ActionResult<boreport>> GetReport( int id=0, string fkname="", string fk="")
        [HttpPost]
        ////[GzipCompression]
        [Route("saveview")]
        public async Task<ActionResult<bool>> saveview(saveview param)
        {
            try
            {
                //var obj="{\"view\":\""+param.view+"\",\"filters\":\""+param.filters+"\"}";

                var result = from a in _context.boreports.Where(f => (f.reportid ?? 0) == param.reportid)
                             select a;
                var objboreport = result.FirstOrDefault();
                JArray jArr = new JArray();
                if (objboreport.filters != null) jArr = (JArray)JsonConvert.DeserializeObject(objboreport.filters);
                jArr.Add(JsonConvert.SerializeObject(param));
                objboreport.filters = JsonConvert.SerializeObject(jArr);
                _context.Entry(objboreport).Property(x => x.filters).IsModified = true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            return null;
        }

        [HttpPost]
        ////[GzipCompression]
        [Route("upload")]
        public async Task<ActionResult<boreport>> uploaddata(Upload param)
        {
            var result = from a in _context.boreports.Where(f => (f.reportid ?? 0) == param.reportid)
                         select new
                         {
                             maintablename = a.maintablename,
                             pk = a.pk
                         };
            var objboreport = result.FirstOrDefault();
            List<UpdateData> tbl = (List<UpdateData>)JsonConvert.DeserializeObject(param.data, typeof(List<UpdateData>));
            UpdateData objrow = null;
            string SQL = "";
            for (int i = 0; i < tbl.Count; i++)
            {
                objrow = tbl[i];
                SQL = SQL + "update " + objboreport.maintablename + " m set " + objrow.field + "='" + objrow.val + "' where " + objboreport.pk + "=" + objrow.pk + ";";
            }

            if (false && SQL != "")
            {
                using (NpgsqlConnection dbConn = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    dbConn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(SQL, dbConn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    dbConn.Close();
                }
            }
            string ret = "Uploaded";
            return Ok(new { ret });
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> Postboreport(ReportParam param)
        {
            dynamic ret = _reportviewerService.RunReport(param);
            return Ok(ret);
        }

        [HttpGet]
        ////[GzipCompression]
        [Route("function/{name}")]
        public async Task<ActionResult<IEnumerable<Object>>> CallFunction(string name, params string[] values)
        {
            var budget = 100;
            var used = 200;
            var available = 300;

            dynamic obj = new { budget, used, available };

            return Ok(new { obj });
        }

        [HttpPost]
        ////[GzipCompression]
        [Route("sequence")]
        public async Task<ActionResult<IEnumerable<Object>>> sequence(SequenceView data)
        {
            return null;
        }

        [HttpPost]
        ////[GzipCompression]
        [Route("delete")]
        public async Task<ActionResult<IEnumerable<Object>>> delete(dynamic data)
        {
            return null;
        }

        [HttpPost]
        ////[GzipCompression]
        [Route("email")]
        public async Task<ActionResult<IEnumerable<Object>>> Email(dynamic data)
        {
            return null;
        }

        [HttpPost]
        ////[GzipCompression]
        [Route("runprocedure")]
        public async Task<ActionResult<IEnumerable<Object>>> ProcessAction(WorkFlowAction action)
        {
            return null;
        }
    }
}