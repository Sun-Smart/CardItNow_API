
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nTireBO;
using carditnow.Models;
using nTireBO.Models;
using SunSmartnTireProducts.Helpers;
//using FluentDateTime;
//using FluentDate;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections;
using System.Text;
using LoggerService;
using nTireBO.Services;
using carditnow.Services;

namespace carditnow.Services
{
    public class termsmasterService : ItermsmasterService
    {
        private readonly IConfiguration Configuration;
        private readonly termsmasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ItermsmasterService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public termsmasterService(termsmasterContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;
            _context = context;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
            if (httpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            }
        }

        // GET: service/termsmaster
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected

        public dynamic Get_termsmasters()
        {
            _logger.LogInfo("Getting into Get_termsmasters() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = 1, @uid = 0 };
                    string SQL = "select pk_encode(a.termid) as pkcol,max(termid) as value,termdetails as discription,status as label from GetTable(NULL::public.termsmasters,@cid) a  WHERE  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_termsmasters(): {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetListBy_termid(int termid)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_termid(int termid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_termid = new { @cid = cid, @uid = uid, @termid = termid };
                    var SQL = "select pk_encode(termid) as pkcol,termid as value,status as label,* from GetTable(NULL::public.termsmasters,@cid) where termid = @termid";
                    var result = connection.Query<dynamic>(SQL, parameters_termid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_termid(int termid) \r\n {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_termsmaster(string sid)
        {
            _logger.LogInfo("Getting into  Get_termsmaster(string sid) api");
            int id = Helper.GetId(sid);
            return Get_termsmaster(id);
        }
        // GET: termsmaster/5
        //gets the screen record
        public dynamic Get_termsmaster(int id)
        {
            _logger.LogInfo("Getting into Get_termsmaster(int id) api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";

                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
                    var SQL = @"select pk_encode(a.termid) as pkcol,a.termid as pk,a.*
 from GetTable(NULL::public.termsmasters,@cid) a 
 where a.termid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_termsmaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'termsmasters'";
                    var termsmaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { termsmaster = obj_termsmaster, termsmaster_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_termsmaster(int id)\r\n {ex}");
                throw ex;
            }
        }

        public IEnumerable<Object> GetList(string condition = "")
        {
            try
            {
                _logger.LogInfo("Getting into  GetList(string condition) api");

                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters = new { @cid = cid, @uid = uid, @key = condition };
                    var SQL = @"select  pk_encode(a.termid) as pkcol,a.termid as pk,* ,termid as value,status as label  from GetTable(NULL::public.termsmasters,@cid) a ";
                    if (condition != "") SQL += " and " + condition;
                    SQL += " order by status";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> Get_NoAuthFullList()
        {
            try
            {
                // _logger.LogInfo("Getting into  GetFullList() api");

                int id = 0;
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //var parameters = new { @cid=0,@uid=0};
                    var SQL = @"select termid,termdetails,currentversion,status from termsmasters where status='A'";
                    //var result = connection.Query<dynamic>(SQL, parameters);
                    var result1 = connection.Query(SQL);
                    connection.Close();
                    connection.Dispose();
                    return (result1);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
                throw ex;
            }
        }

        public IEnumerable<Object> GetFullList()
        {
            try
            {
                _logger.LogInfo("Getting into  GetFullList() api");

                int id = 0;
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    string wStatus = "NormalStatus";
                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
                    var SQL = @"select pk_encode(a.termid) as pkcol,a.termid as pk,a.* from GetTable(NULL::public.termsmasters,@cid) a ";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
                throw ex;
            }
        }


        //saving of record
        public dynamic Save_termsmaster(string token, termsmaster obj_termsmaster)
        {
            _logger.LogInfo("Saving: Save_termsmaster(string token,termsmaster obj_termsmaster) ");
            try
            {
                string serr = "";
                int querytype = 0;
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                //connection.Open();
                //using var transaction = connection.BeginTransaction();
                //_context.Database.UseTransaction(transaction);
                //termsmaster table
                if (obj_termsmaster.termid == 0 || obj_termsmaster.termid == null || obj_termsmaster.termid < 0)
                {
                    if (obj_termsmaster.status == "" || obj_termsmaster.status == null) obj_termsmaster.status = "A";
                    //obj_termsmaster.companyid=cid;
                    obj_termsmaster.createdby = uid;
                    obj_termsmaster.createddate = DateTime.Now;
                    _context.termsmasters.Add((dynamic)obj_termsmaster);
                    querytype = 1;
                }
                else
                {
                    //obj_termsmaster.companyid=cid;
                    obj_termsmaster.updatedby = uid;
                    obj_termsmaster.updateddate = DateTime.Now;
                    _context.Entry(obj_termsmaster).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_termsmaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_termsmaster).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api termsmasters ");
                _context.SaveChanges();


                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                Helper.AfterExecute(token, querytype, obj_termsmaster, "termsmasters", 0, obj_termsmaster.termid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_termsmaster((int)obj_termsmaster.termid);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_termsmaster(string token,termsmaster obj_termsmaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: termsmaster/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                {
                    _logger.LogInfo("Getting into Delete(int id) api");
                    termsmaster obj_termsmaster = _context.termsmasters.Find(id);
                    _context.termsmasters.Remove(obj_termsmaster);
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _logger.LogInfo("remove api termsmasters ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Delete(int id) \r\n{ex}");
                throw ex;
            }
        }

        private bool termsmaster_Exists(int id)
        {
            try
            {
                return _context.termsmasters.Count(e => e.termid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:termsmaster_Exists(int id) {ex}");
                return false;
            }
        }
    }
}

