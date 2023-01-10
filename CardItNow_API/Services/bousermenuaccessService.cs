
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
    public class bousermenuaccessService : IbousermenuaccessService
    {
        private readonly bousermenuaccessContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbousermenuaccessService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public bousermenuaccessService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public bousermenuaccessService(bousermenuaccessContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/bousermenuaccess
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_bousermenuaccesses()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.usermenuaccessid) as pkcol,usermenuaccessid as value,menudescription as label from bousermenuaccesses a  WHERE a.companyid=@cid and  a.status='A'";
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
                    var SQL = "select pk_encode(a.usermenuaccessid) as pkcol,*,usermenuaccessid as value,menudescription as label  from bousermenuaccesses a  where  a.companyid=@cid  and a.status='A' ";
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
        public IEnumerable<Object> GetListBy_usermenuaccessid(int usermenuaccessid)
        {
            try
            {
                _logger.LogInfo("Getting into usermenuaccessid/{usermenuaccessid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_usermenuaccessid = new { @cid = cid, @usermenuaccessid = usermenuaccessid };
                    var SQL = "select pk_encode(usermenuaccessid) as pkcol,* from bousermenuaccesses where usermenuaccessid = @usermenuaccessid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_usermenuaccessid);

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
        public dynamic Get_bousermenuaccess(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_bousermenuaccess(id);
        }
        // GET: bousermenuaccess/5
        //gets the screen record
        public dynamic Get_bousermenuaccess(int id)
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
                    var SQL = @"select pk_encode(a.usermenuaccessid) as pkcol,a.*
 from bousermenuaccesses a 
 where  a.companyid=@cid  and a.usermenuaccessid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_bousermenuaccess = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bousermenuaccesses'";
                    var bousermenuaccess_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { bousermenuaccess = obj_bousermenuaccess, bousermenuaccess_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_bousermenuaccess(string token, bousermenuaccess obj_bousermenuaccess)
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
                    //bousermenuaccess table
                    if (obj_bousermenuaccess.usermenuaccessid == 0 || obj_bousermenuaccess.usermenuaccessid == null || obj_bousermenuaccess.usermenuaccessid < 0)
                    {
                        if (obj_bousermenuaccess.status == "" || obj_bousermenuaccess.status == null) obj_bousermenuaccess.status = "A";
                        obj_bousermenuaccess.companyid = cid;
                        obj_bousermenuaccess.createdby = uid;
                        obj_bousermenuaccess.createddate = DateTime.Now;
                        _context.bousermenuaccesses.Add((dynamic)obj_bousermenuaccess);
                        querytype = 1;
                    }
                    else
                    {
                        obj_bousermenuaccess.companyid = cid;
                        obj_bousermenuaccess.updatedby = uid;
                        obj_bousermenuaccess.updateddate = DateTime.Now;
                        _context.Entry(obj_bousermenuaccess).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_bousermenuaccess).Property("createdby").IsModified = false;
                        _context.Entry(obj_bousermenuaccess).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api bousermenuaccesses ");
                    _context.SaveChanges();


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_bousermenuaccess, "bousermenuaccesses", obj_bousermenuaccess.companyid, obj_bousermenuaccess.usermenuaccessid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_bousermenuaccess((int)obj_bousermenuaccess.usermenuaccessid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: bousermenuaccess/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_bousermenuaccess = Get_bousermenuaccess(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _context.bousermenuaccesses.Remove(_context.bousermenuaccesses.Find(id));
                    _logger.LogInfo("remove api bousermenuaccesses ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_bousermenuaccess);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool bousermenuaccess_Exists(int id)
        {
            try
            {
                return _context.bousermenuaccesses.Count(e => e.usermenuaccessid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }
    }
}

