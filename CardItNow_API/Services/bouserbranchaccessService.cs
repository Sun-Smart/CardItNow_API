
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
    public class bouserbranchaccessService : IbouserbranchaccessService
    {
        private readonly bouserbranchaccessContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbouserbranchaccessService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public bouserbranchaccessService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public bouserbranchaccessService(bouserbranchaccessContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/bouserbranchaccess
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_bouserbranchaccesses()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.accessid) as pkcol,b.branchid as value,b.branchname as label from bouserbranchaccesses a  left join bobranchmasters b on a.branchid=b.branchid WHERE a.companyid=@cid and  a.status='A'";
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
                    var SQL = "select pk_encode(a.accessid) as pkcol,*,b.branchid as value,b.branchname as label  from bouserbranchaccesses a  left join bobranchmasters b on a.branchid=b.branchid where  a.companyid=@cid  and a.status='A' ";
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
        public IEnumerable<Object> GetListBy_accessid(int accessid)
        {
            try
            {
                _logger.LogInfo("Getting into accessid/{accessid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_accessid = new { @cid = cid, @accessid = accessid };
                    var SQL = "select pk_encode(accessid) as pkcol,* from bouserbranchaccesses where accessid = @accessid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_accessid);

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
        public dynamic Get_bouserbranchaccess(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_bouserbranchaccess(id);
        }
        // GET: bouserbranchaccess/5
        //gets the screen record
        public dynamic Get_bouserbranchaccess(int id)
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

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus };
                    var SQL = @"select pk_encode(a.accessid) as pkcol,a.*
 from bouserbranchaccesses a 
 where  a.companyid=@cid  and a.accessid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_bouserbranchaccess = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bouserbranchaccesses'";
                    var bouserbranchaccess_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { bouserbranchaccess = obj_bouserbranchaccess, bouserbranchaccess_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_bouserbranchaccess(string token, bouserbranchaccess obj_bouserbranchaccess)
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
                    //bouserbranchaccess table
                    if (obj_bouserbranchaccess.accessid == 0 || obj_bouserbranchaccess.accessid == null || obj_bouserbranchaccess.accessid < 0)
                    {
                        if (obj_bouserbranchaccess.status == "" || obj_bouserbranchaccess.status == null) obj_bouserbranchaccess.status = "A";
                        obj_bouserbranchaccess.companyid = cid;
                        obj_bouserbranchaccess.createdby = uid;
                        obj_bouserbranchaccess.createddate = DateTime.Now;
                        _context.bouserbranchaccesses.Add((dynamic)obj_bouserbranchaccess);
                        querytype = 1;
                    }
                    else
                    {
                        obj_bouserbranchaccess.companyid = cid;
                        obj_bouserbranchaccess.updatedby = uid;
                        obj_bouserbranchaccess.updateddate = DateTime.Now;
                        _context.Entry(obj_bouserbranchaccess).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_bouserbranchaccess).Property("createdby").IsModified = false;
                        _context.Entry(obj_bouserbranchaccess).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api bouserbranchaccesses ");
                    _context.SaveChanges();


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_bouserbranchaccess, "bouserbranchaccesses", obj_bouserbranchaccess.companyid, obj_bouserbranchaccess.accessid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_bouserbranchaccess((int)obj_bouserbranchaccess.accessid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: bouserbranchaccess/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_bouserbranchaccess = Get_bouserbranchaccess(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _context.bouserbranchaccesses.Remove(_context.bouserbranchaccesses.Find(id));
                    _logger.LogInfo("remove api bouserbranchaccesses ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_bouserbranchaccess);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool bouserbranchaccess_Exists(int id)
        {
            try
            {
                return _context.bouserbranchaccesses.Count(e => e.accessid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }

    }
}

