
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
    public class bomenuactionService : IbomenuactionService
    {
        private readonly bomenuactionContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbomenuactionService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public bomenuactionService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public bomenuactionService(bomenuactionContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/bomenuaction
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_bomenuactions()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.actionid) as pkcol,actionid as value,'' as label from bomenuactions a  WHERE  a.status='A'";
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
                    var SQL = "select pk_encode(a.actionid) as pkcol,*,actionid as value,'' as label  from bomenuactions a where a.status='A'";
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
        public IEnumerable<Object> GetListBy_actionid(int actionid)
        {
            try
            {
                _logger.LogInfo("Getting into actionid/{actionid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_actionid = new { @cid = cid, @actionid = actionid };
                    var SQL = "select pk_encode(actionid) as pkcol,* from bomenuactions where actionid = @actionid";
                    var result = connection.Query<dynamic>(SQL, parameters_actionid);

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


        public IEnumerable<Object> GetListBy_actioname(string actioname)
        {
            try
            {
                _logger.LogInfo("Getting into actionid/{actionid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_actionid = new { @cid = cid, @actionname = actioname };
                    var SQL = "select pk_encode(actionid) as pkcol,* from bomenuactions where actionname = @actionname";
                    var result = connection.Query<dynamic>(SQL, parameters_actionid);

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
        public dynamic Get_bomenuaction(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_bomenuaction(id);
        }
        // GET: bomenuaction/5
        //gets the screen record
        public dynamic Get_bomenuaction(int id)
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
                    string vactiontype = "actiontype";
                    string vrowselecttype = "rowselecttype";

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus, @vactiontype = vactiontype, @vrowselecttype = vrowselecttype };
                    var SQL = @"select pk_encode(a.actionid) as pkcol,a.*,
                              zat.configtext as actiontypedesc,
                              rs.configtext as rowselecttypedesc
 from bomenuactions a 
 left join boconfigvalues zat on a.actiontype=zat.configkey and @vactiontype=zat.param
 left join boconfigvalues rs on a.rowselecttype=rs.configkey and @vrowselecttype=rs.param
 where a.actionid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_bomenuaction = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bomenuactions'";
                    var bomenuaction_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { bomenuaction = obj_bomenuaction, bomenuaction_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_bomenuaction(string token, bomenuaction obj_bomenuaction)
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
                    //bomenuaction table
                    if (obj_bomenuaction.actionid == 0 || obj_bomenuaction.actionid == null || obj_bomenuaction.actionid < 0)
                    {
                        if (obj_bomenuaction.status == "" || obj_bomenuaction.status == null) obj_bomenuaction.status = "A";
                        obj_bomenuaction.createdby = uid;
                        obj_bomenuaction.createddate = DateTime.Now;
                        _context.bomenuactions.Add((dynamic)obj_bomenuaction);
                        querytype = 1;
                    }
                    else
                    {
                        obj_bomenuaction.updatedby = uid;
                        obj_bomenuaction.updateddate = DateTime.Now;
                        _context.Entry(obj_bomenuaction).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_bomenuaction).Property("createdby").IsModified = false;
                        _context.Entry(obj_bomenuaction).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api bomenuactions ");
                    _context.SaveChanges();


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_bomenuaction, "bomenuactions", 0, obj_bomenuaction.actionid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_bomenuaction((int)obj_bomenuaction.actionid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: bomenuaction/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_bomenuaction = Get_bomenuaction(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _context.bomenuactions.Remove(_context.bomenuactions.Find(id));
                    _logger.LogInfo("remove api bomenuactions ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_bomenuaction);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool bomenuaction_Exists(int id)
        {
            try
            {
                return _context.bomenuactions.Count(e => e.actionid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }
    }
}

