using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunSmartnTireProducts.Helpers;
using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBOWebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class customfieldconfigurationController : ControllerBase
    {
        private readonly customfieldconfigurationContext _context;

        public customfieldconfigurationController(customfieldconfigurationContext context)
        {
            _context = context;
        }

        // GET: api/customfieldconfiguration
        [HttpGet]
        [GzipCompression]
        public async Task<ActionResult<IEnumerable<Object>>> Getcustomfieldconfigurations()
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            var result = (from a in _context.customfieldconfigurations.Where(c => c.companyid == cid) select a).ToList(); ;
            return result;
        }

        // PUT: api/customfieldconfiguration/5
        // GET: api/customfieldconfiguration/5
        [HttpGet("table1/{id}")]
        public async Task<ActionResult<IEnumerable<Object>>> Getcustomfieldconfiguration(string id)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            //return await _context.customfieldconfigurations.ToListAsync();
            var visibilities = (from vi in _context.columnvisibilities.Where(vi => vi.tablename == "customfieldconfigurations") select new { vi.columnname, vi.show, vi.hide }).ToList();
            /*
                        var a1 = (from a in _context.customfieldconfigurations.Where(c => c.companyid==cid) where a.tablename == id
            join config in _context.boconfigvalues on new{X=a.status,X2=ConfigParam} equals new{X=config.configkey,X2=config.param} into config1
            from config2 in config1.DefaultIfEmpty()                          from f in _context.boconfigvalues.Where(f =>a.fieldtype == f.configkey && f.param=="fieldtype").DefaultIfEmpty()
                                      select new
                                      {
                                          customfieldid = a.customfieldid,
                                          company = a.company,
                                          tablename = a.tablename,
                                          fieldname = a.fieldname,
                                          fieldtype = a.fieldtype,
                                          fieldvalues = a.fieldvalues,
                                          labelname = a.labelname,
                                          sequence = a.sequence,
                                          status = a.status,
                                          fieldtypeDesc=f.configtext,
                                      });
            */
            string X2fieldtype = "fieldtype";
            string ConfigParam = "ApprovalStatus";
            var result = from a in _context.customfieldconfigurations.Where(c => c.companyid == cid)
                         where a.tablename == id
                         join config in _context.boconfigvalues on new { X = a.status, X2 = ConfigParam } equals new { X = config.configkey, X2 = config.param } into config1
                         from config2 in config1.DefaultIfEmpty()
                         join ff in _context.boconfigvalues on new { X = a.fieldtype, X2 = X2fieldtype } equals new { X = ff.configkey, X2 = ff.param } into fff
                         from f in fff.DefaultIfEmpty()
                         select new
                         {
                             customfieldid = a.customfieldid,
                             company = a.company,
                             tablename = a.tablename,
                             fieldname = a.fieldname,
                             fieldtype = a.fieldtype,
                             fieldvalues = a.fieldvalues,
                             labelname = a.labelname,
                             sequence = a.sequence,
                             status = a.status,
                             StatusDesc = config2.configtext,
                             fieldtypeDesc = f.configtext,
                         };
            /*
            var result1 = from a in result
            select new
            {
                                          customfieldid = a.customfieldid,
                                          company = a.company,
                                          tablename = a.tablename,
                                          fieldname = a.fieldname,
                                          fieldtype = a.fieldtype,
                                          fieldvalues = a.fieldvalues,
                                          labelname = a.labelname,
                                          sequence = a.sequence,
                                          status = a.status,
                                          fieldtypeDesc=a.fieldtypeDesc,
            };
            */

            return result.ToList();
        }

        [HttpPost]
        [GzipCompression]
        [Route("table")]
        public async Task<ActionResult<IEnumerable<bodynamicform>>> GetListBytable([FromBody] CustomFieldInfo customFieldInfo)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            int userroleid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userroleid").Value.ToString());
            int uid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            try
            {
                string key = customFieldInfo.key;
                string customformname = customFieldInfo.CustomFormName;
                string CustomFormField = customFieldInfo.CustomFormField;
                string CustomFormFieldValue = customFieldInfo.CustomFormFieldValue;
                string ConfigParam = "ApprovalStatus";
                var result = (from a in _context.bodynamicforms.Where(c => c.companyid == cid).Where(f => f.tableiddesc == key && (((f.conditionfield == "{{SESSIONUSERID}}") && (f.conditionvalue == uid.ToString())) || ((CustomFormField == "{{SESSIONUSERROLE}}") && (CustomFormFieldValue == userroleid.ToString())) || (customformname == "" || (f.formname ?? "") == customformname) && (CustomFormField == "" || (f.conditionfield ?? "") == CustomFormField) && (CustomFormFieldValue == "" || (f.conditionvalue ?? "") == CustomFormFieldValue)))
                              join config in _context.boconfigvalues on new { X = a.status, X2 = ConfigParam } equals new { X = config.configkey, X2 = config.param } into config1
                              from config2 in config1.DefaultIfEmpty()
                              join ff in _context.boconfigvalues on new { X = a.formtype } equals new { X = ff.configkey } into fff
                              from f in fff.DefaultIfEmpty()
                              join cc in _context.systemtables on new { X1 = a.tableid } equals new { X1 = cc.tableid } into ccc
                              from c in ccc.DefaultIfEmpty()

                              select new
                              {
                                  tableid = a.tableid,
                                  tableiddesc = a.tableiddesc,
                                  conditionfield = a.conditionfield,
                                  conditionvalue = a.conditionvalue,
                                  formid = a.formid,
                                  formname = a.formname,
                                  formtype = a.formtype,
                                  formhtml = a.formhtml,
                                  cols = a.cols,
                                  templatehtml = a.templatehtml,
                                  hasattachments = a.hasattachments,
                                  sequence = a.sequence,
                                  status = a.status,
                                  StatusDesc = config2.configtext,
                                  formtypedesc = f.configtext,
                              }).OrderBy(x => x.sequence).ToList();

                var bodynamicform = result.FirstOrDefault();

                List<string> formid = new List<string>();
                foreach (var frm in result)
                {
                    formid.Add(frm.formid.ToString());
                }
                if (bodynamicform != null)
                {
                    var bodynamicformdetail = (from a in _context.bodynamicformdetails.Where(c => c.companyid == cid)
                                               from t in _context.boconfigvalues.Where(t => a.controltype == t.configkey && t.param == "controltype").DefaultIfEmpty()
                                               where formid.Contains(a.formid.ToString())

                                               select new
                                               {
                                                   a.tableid,
                                                   a.tableiddesc,
                                                   a.formdetailid,
                                                   a.formid,
                                                   a.fieldname,
                                                   a.controltype,
                                                   a.required,
                                                   a.fk,
                                                   a.sequence,
                                                   a.configurations,
                                                   a.status,
                                                   controltypedesc = t.configtext,
                                               }).ToList().OrderBy(x => x.sequence).ToList();

                    return Ok(new { bodynamicform, bodynamicformdetail });
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            return null;
        }

        [HttpGet]
        [GzipCompression]
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            //return await _context.boconfigvalues.ToListAsync();
            var result = (from a in _context.customfieldconfigurations.Where(c => c.companyid == cid)
                              //                          where a.param == key
                          select a).ToList();

            return result;
        }

        // GET: api/customfieldconfiguration/5
        [HttpGet("{id}")]
        public async Task<ActionResult<customfieldconfiguration>> Getcustomfieldconfiguration(int id)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            string ConfigParam = "ApprovalStatus";
            var result = from a in _context.customfieldconfigurations.Where(c => c.companyid == cid).Where(f => (f.customfieldid ?? 0) == id)
                         join config in _context.boconfigvalues on new { X = a.status, X2 = ConfigParam } equals new { X = config.configkey, X2 = config.param } into config1
                         from config2 in config1.DefaultIfEmpty()
                         join ff in _context.boconfigvalues on new { X = a.fieldtype } equals new { X = ff.configkey } into fff
                         from f in fff.DefaultIfEmpty()

                         select new
                         {
                             customfieldid = a.customfieldid,
                             company = a.company,
                             tablename = a.tablename,
                             fieldname = a.fieldname,
                             fieldtype = a.fieldtype,
                             fieldvalues = a.fieldvalues,
                             labelname = a.labelname,
                             sequence = a.sequence,
                             status = a.status,
                             StatusDesc = config2.configtext,
                             fieldtypeDesc = f.configtext,
                         };
            var customfieldconfiguration = result.FirstOrDefault();

            return Ok(new { customfieldconfiguration });
        }

        // POST: api/customfieldconfiguration
        [HttpPost]
        [GzipCompression]
        public async Task<ActionResult<customfieldconfiguration>> Postcustomfieldconfiguration(customfieldconfiguration customfieldconfiguration)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString()); // customfieldconfiguration.SessionUser.companyid;
            try
            {
                string serr = "";

                if (serr != "")
                {
                    Exception ex = new Exception(serr);
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                //customfieldconfiguration table
                if (customfieldconfiguration.customfieldid == 0 || customfieldconfiguration.customfieldid == null || customfieldconfiguration.customfieldid < 0)
                {
                    if (customfieldconfiguration.status == "" || customfieldconfiguration.status == null) customfieldconfiguration.status = "P";
                    customfieldconfiguration.companyid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString()); //customfieldconfiguration.SessionUser.companyid;
                    customfieldconfiguration.createdby = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString()); //customfieldconfiguration.SessionUser.userid;
                    customfieldconfiguration.createddate = DateTime.Now;
                    _context.customfieldconfigurations.Add(customfieldconfiguration);
                }
                else
                {
                    customfieldconfiguration.companyid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString()); //customfieldconfiguration.SessionUser.companyid;
                    customfieldconfiguration.updatedby = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString()); //customfieldconfiguration.SessionUser.userid;
                    customfieldconfiguration.updateddate = DateTime.Now;
                    _context.Entry(customfieldconfiguration).State = EntityState.Modified;
                }
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/customfieldconfiguration/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customfieldconfiguration>> Delete(int id)
        {
            customfieldconfiguration customfieldconfiguration = _context.customfieldconfigurations.Find(id);
            _context.customfieldconfigurations.Remove(customfieldconfiguration);
            _context.SaveChanges();

            return Ok(customfieldconfiguration);
        }

        private bool customfieldconfigurationExists(int id)
        {
            return _context.customfieldconfigurations.Count(e => e.customfieldid == id) > 0;
        }
    }

    public class CustomFieldInfo
    {
        public string key;
        public string CustomFormName;
        public string CustomFormField;
        public string CustomFormFieldValue;
    }
}