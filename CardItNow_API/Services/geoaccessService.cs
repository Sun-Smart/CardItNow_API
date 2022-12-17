
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
    public class geoaccessService : IgeoaccessService
    {
        private readonly IConfiguration Configuration;
        private readonly geoaccessContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IgeoaccessService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public geoaccessService(geoaccessContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/geoaccess
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_geoaccesses()
        {
        _logger.LogInfo("Getting into Get_geoaccesses() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.geoaccessid) as pkcol,geoaccessid as value,status as label from GetTable(NULL::public.geoaccesses,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_geoaccesses(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_geoaccessid(int geoaccessid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_geoaccessid(int geoaccessid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_geoaccessid = new { @cid = cid,@uid=uid ,@geoaccessid = geoaccessid  };
            var SQL = "select pk_encode(geoaccessid) as pkcol,geoaccessid as value,status as label,* from GetTable(NULL::public.geoaccesses,@cid) where geoaccessid = @geoaccessid";
var result = connection.Query<dynamic>(SQL, parameters_geoaccessid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_geoaccessid(int geoaccessid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_geoaccess(string sid)
{
        _logger.LogInfo("Getting into  Get_geoaccess(string sid) api");
int id = Helper.GetId(sid);
  return  Get_geoaccess(id);
}
        // GET: geoaccess/5
//gets the screen record
        public  dynamic Get_geoaccess(int id)
        {
        _logger.LogInfo("Getting into Get_geoaccess(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.geoaccessid) as pkcol,a.geoaccessid as pk,a.*,
g.geoname as geoiddesc,
u.email as useriddesc
 from GetTable(NULL::public.geoaccesses,@cid) a 
 left join geographymasters g on a.geoid=g.geoid
 left join usermasters u on a.userid=u.userid
 where a.geoaccessid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_geoaccess = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'geoaccesses'";
var geoaccess_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { geoaccess=obj_geoaccess,geoaccess_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_geoaccess(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.geoaccessid) as pkcol,a.geoaccessid as pk,* ,geoaccessid as value,status as label  from GetTable(NULL::public.geoaccesses,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by status";
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
            var SQL = @"select pk_encode(a.geoaccessid) as pkcol,a.geoaccessid as pk,a.*,
g.geoname as geoiddesc,
u.email as useriddesc from GetTable(NULL::public.geoaccesses,@cid) a 
 left join geographymasters g on a.geoid=g.geoid
 left join usermasters u on a.userid=u.userid";
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
        public  dynamic Save_geoaccess(string token,geoaccess obj_geoaccess)
        {
        _logger.LogInfo("Saving: Save_geoaccess(string token,geoaccess obj_geoaccess) ");
            try
            {
                string serr = "";
int querytype=0;
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //geoaccess table
                if (obj_geoaccess.geoaccessid == 0 || obj_geoaccess.geoaccessid == null || obj_geoaccess.geoaccessid<0)
{
if(obj_geoaccess.status=="" || obj_geoaccess.status==null)obj_geoaccess.status="A";
//obj_geoaccess.companyid=cid;
obj_geoaccess.createdby=uid;
obj_geoaccess.createddate=DateTime.Now;
                    _context.geoaccesses.Add((dynamic)obj_geoaccess);
querytype=1;
}
                else
{
//obj_geoaccess.companyid=cid;
obj_geoaccess.updatedby=uid;
obj_geoaccess.updateddate=DateTime.Now;
                    _context.Entry(obj_geoaccess).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_geoaccess).Property("createdby").IsModified = false;
                    _context.Entry(obj_geoaccess).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api geoaccesses ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_geoaccess,"geoaccesses", 0,obj_geoaccess.geoaccessid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_geoaccess( (int)obj_geoaccess.geoaccessid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_geoaccess(string token,geoaccess obj_geoaccess) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: geoaccess/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
geoaccess obj_geoaccess = _context.geoaccesses.Find(id);
_context.geoaccesses.Remove(obj_geoaccess);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api geoaccesses ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool geoaccess_Exists(int id)
        {
        try{
            return _context.geoaccesses.Count(e => e.geoaccessid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:geoaccess_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

