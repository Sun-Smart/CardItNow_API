
using carditnow.Models;
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SunSmartnTireProducts.Helpers;
using SunSmartnTireProducts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace carditnow.Services
{
    public class botemplateService : IbotemplateService
    {
        private readonly botemplateContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbotemplateService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public botemplateService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public botemplateService(botemplateContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            _context = context;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
            cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
        }

        // GET: service/botemplate
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_botemplates()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.templateid) as pkcol,templateid as value,templatecode as label from botemplates a  WHERE a.companyid=@cid and  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetList(string key)
        {
            try
            {
                _logger.LogInfo("Getting into param/{key} GetList api");

                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters = new { @cid = cid, @key = key };
                    var SQL = "select pk_encode(a.templateid) as pkcol,*,templateid as value,templatecode as label  from botemplates a  where  a.companyid=@cid  and a.status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetListBy_templateid(int templateid)
        {
            try
            {
                _logger.LogInfo("Getting into templateid/{templateid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_templateid = new { @cid = cid, @templateid = templateid };
                    var SQL = "select pk_encode(templateid) as pkcol,* from botemplates where templateid = @templateid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_templateid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetListBy_templatecode(string templatecode)
        {
            try
            {
                //if (templatecode.Contains('"'))
                //{
                //    templatecode.Replace('"', ' ').Trim();
                //}
                cid = 1;
                _logger.LogInfo("Getting into templatecode/{templatecode} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_templatecode = new { @cid = cid, @templatecode = templatecode };
                    var SQL = "select pk_encode(templateid) as pkcol,* from botemplates where templatecode =@templatecode and  companyid=@cid  and status='A' ";
                        var result = connection.Query<dynamic>(SQL, parameters_templatecode);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_botemplate(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_botemplate(id);
        }
        // GET: botemplate/5
        //gets the screen record
        public dynamic Get_botemplate(int id)
        {
            _logger.LogInfo("Getting into {id} api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";
                    string vtemplatetype = "templatetype";

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus, @vtemplatetype = vtemplatetype };
                    var SQL = @"select pk_encode(a.templateid) as pkcol,a.*,
                              t.configtext as templatetypedesc
 from botemplates a 
 left join boconfigvalues t on a.templatetype=t.configkey and @vtemplatetype=t.param
 where  a.companyid=@cid  and a.templateid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_botemplate = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'botemplates'";
                    var botemplate_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { botemplate = obj_botemplate, botemplate_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_botemplate(string token, botemplate obj_botemplate)
        {
            _logger.LogInfo("Saving");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    string serr = "";
                    int querytype = 0;
                    if (serr != "")
                    {
                        _logger.LogError($"Something went wrong: {serr}");
                        throw new Exception(serr);
                    }

                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    //botemplate table
                    if (obj_botemplate.templateid == 0 || obj_botemplate.templateid == null || obj_botemplate.templateid < 0)
                    {
                        if (obj_botemplate.status == "" || obj_botemplate.status == null) obj_botemplate.status = "A";
                        obj_botemplate.companyid = cid;
                        obj_botemplate.createdby = uid;
                        obj_botemplate.createddate = DateTime.Now;
                        _context.botemplates.Add((dynamic)obj_botemplate);
                        querytype = 1;
                    }
                    else
                    {
                        obj_botemplate.companyid = cid;
                        obj_botemplate.updatedby = uid;
                        obj_botemplate.updateddate = DateTime.Now;
                        _context.Entry(obj_botemplate).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_botemplate).Property("createdby").IsModified = false;
                        _context.Entry(obj_botemplate).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api botemplates ");
                    _context.SaveChanges();


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_botemplate, "botemplates", obj_botemplate.companyid, obj_botemplate.templateid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_botemplate((int)obj_botemplate.templateid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: botemplate/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_botemplate = Get_botemplate(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _context.botemplates.Remove(_context.botemplates.Find(id));
                    _logger.LogInfo("remove api botemplates ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_botemplate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool botemplate_Exists(int id)
        {
            try
            {
                return _context.botemplates.Count(e => e.templateid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }
    }
}

