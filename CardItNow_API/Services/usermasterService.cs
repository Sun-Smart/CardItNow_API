
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
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Org.BouncyCastle.Crypto.Generators;
//using Jose.native;

namespace carditnow.Services
{
    public class usermasterService : IusermasterService
    {
        private readonly IConfiguration Configuration;
        private readonly usermasterContext _context;
        private readonly IuserrolemasterService _userrolemasterService;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IusermasterService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public usermasterService(usermasterContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/usermaster
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_usermasters()
        {
            _logger.LogInfo("Getting into Get_usermasters() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select pk_encode(a.userid) as pkcol,userid as value,username as label from GetTable(NULL::public.usermasters,@cid) a  WHERE  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_usermasters(): {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetListBy_userid(int userid)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_userid(int userid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_userid = new { @cid = cid, @uid = uid, @userid = userid };
                    var SQL = "select pk_encode(userid) as pkcol,userid as value,username as label,* from GetTable(NULL::public.usermasters,@cid) where userid = @userid";
                    var result = connection.Query<dynamic>(SQL, parameters_userid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_userid(int userid) \r\n {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_usermaster(string sid)
        {
            _logger.LogInfo("Getting into  Get_usermaster(string sid) api");
            int id = Helper.GetId(sid);
            return Get_usermaster(id);
        }
        // GET: usermaster/5
        //gets the screen record
        public dynamic Get_usermaster(int id)
        {
            _logger.LogInfo("Getting into Get_usermaster(int id) api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";

                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
                    var SQL = @"select pk_encode(a.userid) as pkcol,a.userid as pk,a.*,
r.roledescription as roleiddesc,
g.geoname as basegeoiddesc
 from usermasters a 
 left join userrolemasters r on a.roleid=r.roleid
 left join geographymasters g on a.basegeoid=g.geoid
 where a.userid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_usermaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'usermasters'";
                    var usermaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { usermaster = obj_usermaster, usermaster_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_usermaster(int id)\r\n {ex}");
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
                    var SQL = @"select  pk_encode(a.userid) as pkcol,a.userid as pk,* ,userid as value,username as label  from GetTable(NULL::public.usermasters,@cid) a ";
                    if (condition != "") SQL += " and " + condition;
                    SQL += " order by username";
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
                    var SQL = @"select pk_encode(a.userid) as pkcol,a.userid as pk,a.*,
r.roledescription as roleiddesc,
g.geoname as basegeoiddesc from GetTable(NULL::public.usermasters,@cid) a 
 left join userrolemasters r on a.roleid=r.roleid
 left join geographymasters g on a.basegeoid=g.geoid";
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
        public dynamic Save_usermaster(string token, usermaster obj_usermaster)
        {
            _logger.LogInfo("Saving: Save_usermaster(string token,usermaster obj_usermaster) ");
            try
            {
                string serr = "";
                int querytype = 0;
                if (obj_usermaster.mobile != null)
                {
                    var parametersmobile = new { @cid = cid, @uid = uid, @mobile = obj_usermaster.mobile, @userid = obj_usermaster.userid };
                    if (Helper.Count("select count(*) as count from usermasters where  mobile =  @mobile ", parametersmobile) > 0) serr += "mobile is unique\r\n";
                }
                //and (@userid == 0 ||  @userid == null ||  @userid < 0 || userid!=  @userid)
                if (obj_usermaster.email != null)
                {
                    var parametersemail = new { @cid = cid, @uid = uid, @email = obj_usermaster.email, @userid = obj_usermaster.userid };
                    if (Helper.Count("select count(*) as count from usermasters where  email =  @email ", parametersemail) > 0) serr += "email is unique\r\n";
                }
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                
                //connection.Open();
                //using var transaction = connection.BeginTransaction();
                //_context.Database.UseTransaction(transaction);
                //usermaster table
                if (obj_usermaster.userid == 0 || obj_usermaster.userid == null || obj_usermaster.userid < 0)
                {

                    var datasss = Encoding.Unicode.GetBytes("123456");

                    if (obj_usermaster.status == "" || obj_usermaster.status == null) obj_usermaster.status = "A";
                    //obj_usermaster.companyid=cid;
                    obj_usermaster.createdby = uid;
                   // obj_usermaster.emailpassword = crypt('1234', obj_usermaster.emailpassword);
                    obj_usermaster.createddate = DateTime.Now;
                   
                    _context.usermasters.Add((dynamic)obj_usermaster);
                    querytype = 1;
                }
                else
                {
                    //obj_usermaster.companyid=cid;
                    obj_usermaster.updatedby = uid;
                    obj_usermaster.updateddate = DateTime.Now;
                    _context.Entry(obj_usermaster).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_usermaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_usermaster).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api usermasters ");
                _context.SaveChanges();

                //start

                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    string wStatus = "NormalStatus";
                    var parameters = new { @cid = cid, @uid = uid, @email = obj_usermaster.email, @wStatus = wStatus };
                    var SQL = @"update usermasters set emailpassword=crypt('"+ obj_usermaster .emailpassword+ "',emailpassword) where email = @email ";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }


                if (obj_usermaster.userrolemasters != null && obj_usermaster.userrolemasters.Count > 0)
                {
                    foreach (var obj in obj_usermaster.userrolemasters)
                    {
                        if (obj.roleid == null)
                        {
                            //obj.userid = result.usermaster.userid;
                            _userrolemasterService.Save_userrolemaster(token, obj);
                        }
                    }
                }
                if (obj_usermaster.Deleted_userrolemaster_IDs != null && obj_usermaster.Deleted_userrolemaster_IDs != "")
                {
                    string[] ids = obj_usermaster.Deleted_userrolemaster_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _userrolemasterService.Delete(int.Parse(id));
                        }
                    }
                }
                //if (Request.Form.Files != null)
                //{
                //    foreach (var file in Request.Form.Files)
                //    {
                //        Helper.Upload(file);
                //    }
                //}




                //end






                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                Helper.AfterExecute(token, querytype, obj_usermaster, "usermasters", 0, obj_usermaster.userid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_usermaster((int)obj_usermaster.userid);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_usermaster(string token,usermaster obj_usermaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: usermaster/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                {
                    _logger.LogInfo("Getting into Delete(int id) api");
                    usermaster obj_usermaster = _context.usermasters.Find(id);
                    _context.usermasters.Remove(obj_usermaster);
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _logger.LogInfo("remove api usermasters ");
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

        private bool usermaster_Exists(int id)
        {
            try
            {
                return _context.usermasters.Count(e => e.userid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:usermaster_Exists(int id) {ex}");
                return false;
            }
        }


        //

       

    }
}

