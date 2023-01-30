
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using SunSmartnTireProducts.Helpers;
using SunSmartnTireProducts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace nTireBiz.Services
{
    public class boworkflowstepService : IboworkflowstepService
    {
        private readonly boworkflowstepContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IboworkflowstepService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public boworkflowstepService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public boworkflowstepService(boworkflowstepContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/boworkflowstep
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_boworkflowsteps()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.workflowstepid) as pkcol,workflowstepid as value,stepname as label from boworkflowsteps a  WHERE a.companyid=@cid and  a.status='A'";
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
                    var SQL = "select pk_encode(a.workflowstepid) as pkcol,*,workflowstepid as value,stepname as label  from boworkflowsteps a  where  a.companyid=@cid  and a.status='A' ";
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
        public IEnumerable<Object> GetListBy_workflowstepid(int workflowstepid)
        {
            try
            {
                _logger.LogInfo("Getting into workflowstepid/{workflowstepid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_workflowstepid = new { @cid = cid, @workflowstepid = workflowstepid };
                    var SQL = "select pk_encode(workflowstepid) as pkcol,* from boworkflowsteps where workflowstepid = @workflowstepid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_workflowstepid);

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
        public dynamic Get_boworkflowstep(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_boworkflowstep(id);
        }
        // GET: boworkflowstep/5
        //gets the screen record
        public dynamic Get_boworkflowstep(int id)
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
                    string vworkflowtask = "workflowtask";
                    string vworkflowuserfieldtype = "workflowuserfieldtype";

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus, @vworkflowtask = vworkflowtask, @vworkflowuserfieldtype = vworkflowuserfieldtype };
                    var SQL = @"select pk_encode(a.workflowstepid) as pkcol,a.*,
df.tableiddesc as customfieldiddesc,
                              wt.configtext as taskdesc,
ws.stepname as yesstepdesc,
ns.stepname as nostepdesc,
pi.stepname as parentiddesc,
                              ft.configtext as workflowuserfieldtypedesc
 from boworkflowsteps a 
 left join bodynamicforms df on  df.companyid=@cid  and a.customfieldid=df.formid
 left join boconfigvalues wt on a.task=wt.configkey and @vworkflowtask=wt.param
 left join boworkflowsteps ws on  ws.companyid=@cid  and a.yesstep=ws.workflowstepid
 left join boworkflowsteps ns on  ns.companyid=@cid  and a.nostep=ns.workflowstepid
 left join boworkflowsteps pi on  pi.companyid=@cid  and a.parentid=pi.workflowstepid
 left join boconfigvalues ft on a.workflowuserfieldtype=ft.configkey and @vworkflowuserfieldtype=ft.param
 where  a.companyid=@cid  and a.workflowstepid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_boworkflowstep = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'boworkflowsteps'";
                    var boworkflowstep_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { boworkflowstep = obj_boworkflowstep, boworkflowstep_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_boworkflowstep(string token, boworkflowstep obj_boworkflowstep)
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
                    //boworkflowstep table
                    if (obj_boworkflowstep.workflowstepid == 0 || obj_boworkflowstep.workflowstepid == null || obj_boworkflowstep.workflowstepid < 0)
                    {
                        if (obj_boworkflowstep.status == "" || obj_boworkflowstep.status == null) obj_boworkflowstep.status = "A";
                        obj_boworkflowstep.companyid = cid;
                        obj_boworkflowstep.createdby = uid;
                        obj_boworkflowstep.createddate = DateTime.Now;
                        if (obj_boworkflowstep.approver != null && obj_boworkflowstep.approver != "null" && obj_boworkflowstep.approver != "") obj_boworkflowstep.approver = (JsonConvert.DeserializeObject(obj_boworkflowstep.approver)).ToString();
                        if (obj_boworkflowstep.escalationuser != null && obj_boworkflowstep.escalationuser != "null" && obj_boworkflowstep.escalationuser != "") obj_boworkflowstep.escalationuser = (JsonConvert.DeserializeObject(obj_boworkflowstep.escalationuser)).ToString();
                        if (obj_boworkflowstep.cc != null && obj_boworkflowstep.cc != "null" && obj_boworkflowstep.cc != "") obj_boworkflowstep.cc = (JsonConvert.DeserializeObject(obj_boworkflowstep.cc)).ToString();
                        _context.boworkflowsteps.Add((dynamic)obj_boworkflowstep);
                        querytype = 1;
                    }
                    else
                    {
                        obj_boworkflowstep.companyid = cid;
                        obj_boworkflowstep.updatedby = uid;
                        obj_boworkflowstep.updateddate = DateTime.Now;
                        if (obj_boworkflowstep.approver != null && obj_boworkflowstep.approver != "null" && obj_boworkflowstep.approver != "") obj_boworkflowstep.approver = (JsonConvert.DeserializeObject(obj_boworkflowstep.approver)).ToString();
                        if (obj_boworkflowstep.escalationuser != null && obj_boworkflowstep.escalationuser != "null" && obj_boworkflowstep.escalationuser != "") obj_boworkflowstep.escalationuser = (JsonConvert.DeserializeObject(obj_boworkflowstep.escalationuser)).ToString();
                        if (obj_boworkflowstep.cc != null && obj_boworkflowstep.cc != "null" && obj_boworkflowstep.cc != "") obj_boworkflowstep.cc = (JsonConvert.DeserializeObject(obj_boworkflowstep.cc)).ToString();
                        _context.Entry(obj_boworkflowstep).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_boworkflowstep).Property("createdby").IsModified = false;
                        _context.Entry(obj_boworkflowstep).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api boworkflowsteps ");
                    _context.SaveChanges();


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_boworkflowstep, "boworkflowsteps", obj_boworkflowstep.companyid, obj_boworkflowstep.workflowstepid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_boworkflowstep((int)obj_boworkflowstep.workflowstepid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: boworkflowstep/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_boworkflowstep = Get_boworkflowstep(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _context.boworkflowsteps.Remove(_context.boworkflowsteps.Find(id));
                    _logger.LogInfo("remove api boworkflowsteps ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_boworkflowstep);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool boworkflowstep_Exists(int id)
        {
            try
            {
                return _context.boworkflowsteps.Count(e => e.workflowstepid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }
    }
}

