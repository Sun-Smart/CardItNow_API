
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
    public class userrolemasterService : IuserrolemasterService
    {
        private readonly IConfiguration Configuration;
        private readonly userrolemasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IuserrolemasterService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public userrolemasterService(userrolemasterContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
        {
Configuration = configuration;
            _context = context;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
        cid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
        uid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
        uname = "";
        uidemail = "";
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")!=null)uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid")!=null)uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
        }

        // GET: service/userrolemaster
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_userrolemasters()
        {
        _logger.LogInfo("Getting into Get_userrolemasters() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.roleid) as pkcol,roleid as value,roledescription as label from GetTable(NULL::public.userrolemasters,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_userrolemasters(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_roleid(int roleid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_roleid(int roleid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_roleid = new { @cid = cid,@uid=uid ,@roleid = roleid  };
            var SQL = "select pk_encode(roleid) as pkcol,roleid as value,roledescription as label,* from GetTable(NULL::public.userrolemasters,@cid) where roleid = @roleid";
var result = connection.Query<dynamic>(SQL, parameters_roleid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_roleid(int roleid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_userrolemaster(string sid)
{
        _logger.LogInfo("Getting into  Get_userrolemaster(string sid) api");
int id = Helper.GetId(sid);
  return  Get_userrolemaster(id);
}
        // GET: userrolemaster/5
//gets the screen record
        public  dynamic Get_userrolemaster(int id)
        {
        _logger.LogInfo("Getting into Get_userrolemaster(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.roleid) as pkcol,a.roleid as pk,a.*
 from GetTable(NULL::public.userrolemasters,@cid) a 
 where a.roleid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_userrolemaster = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'userrolemasters'";
var userrolemaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { userrolemaster=obj_userrolemaster,userrolemaster_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_userrolemaster(int id)\r\n {ex}");
throw ex;
}
        }

        public  IEnumerable<Object> GetList(string condition="")
        {
        try{
        _logger.LogInfo("Getting into  GetList(string condition) api");

        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters = new { @cid = cid,@uid=uid,@key=condition  };
            var SQL = @"select  pk_encode(a.roleid) as pkcol,a.roleid as pk,* ,roleid as value,roledescription as label  from GetTable(NULL::public.userrolemasters,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by roledescription";
var result = connection.Query<dynamic>(SQL, parameters);


            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
            throw ex;
        }
        }
        public  IEnumerable<Object> GetFullList()
        {
        try{
        _logger.LogInfo("Getting into  GetFullList() api");

int id=0;
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
string wStatus = "NormalStatus";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
            var SQL = @"select pk_encode(a.roleid) as pkcol,a.roleid as pk,a.* from GetTable(NULL::public.userrolemasters,@cid) a ";
var result = connection.Query<dynamic>(SQL, parameters);


            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
            throw ex;
        }
        }
//saving of record
        public  dynamic Save_userrolemaster(string token,userrolemaster obj_userrolemaster)
        {
        _logger.LogInfo("Saving: Save_userrolemaster(string token,userrolemaster obj_userrolemaster) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_userrolemaster.roledescription!=null )
{
var parametersroledescription =new {@cid=cid,@uid=uid, @roledescription = obj_userrolemaster.roledescription,@roleid=obj_userrolemaster.roleid };
                    if(Helper.Count("select count(*) from userrolemasters where  and roledescription =  @roledescription and (@roleid == 0 ||  @roleid == null ||  @roleid < 0 || roleid!=  @roleid)",parametersroledescription)> 0) serr +="roledescription is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //userrolemaster table
                if (obj_userrolemaster.roleid == 0 || obj_userrolemaster.roleid == null || obj_userrolemaster.roleid<0)
{
if(obj_userrolemaster.status=="" || obj_userrolemaster.status==null)obj_userrolemaster.status="A";
//obj_userrolemaster.companyid=cid;
obj_userrolemaster.createdby=uid;
obj_userrolemaster.createddate=DateTime.Now;
                    _context.userrolemasters.Add((dynamic)obj_userrolemaster);
querytype=1;
}
                else
{
//obj_userrolemaster.companyid=cid;
obj_userrolemaster.updatedby=uid;
obj_userrolemaster.updateddate=DateTime.Now;
                    _context.Entry(obj_userrolemaster).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_userrolemaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_userrolemaster).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api userrolemasters ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_userrolemaster,"userrolemasters", 0,obj_userrolemaster.roleid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_userrolemaster( (int)obj_userrolemaster.roleid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_userrolemaster(string token,userrolemaster obj_userrolemaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: userrolemaster/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
userrolemaster obj_userrolemaster = _context.userrolemasters.Find(id);
_context.userrolemasters.Remove(obj_userrolemaster);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api userrolemasters ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool userrolemaster_Exists(int id)
        {
        try{
            return _context.userrolemasters.Count(e => e.roleid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:userrolemaster_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

