
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
    public class citymasterService : IcitymasterService
    {
        private readonly IConfiguration Configuration;
        private readonly citymasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcitymasterService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public citymasterService(citymasterContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/citymaster
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_citymasters()
        {
        _logger.LogInfo("Getting into Get_citymasters() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.cityid) as pkcol,cityid as value,cityname as label from GetTable(NULL::public.citymasters,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_citymasters(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_cityid(int cityid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_cityid(int cityid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_cityid = new { @cid = cid,@uid=uid ,@cityid = cityid  };
            var SQL = "select pk_encode(cityid) as pkcol,cityid as value,cityname as label,* from GetTable(NULL::public.citymasters,@cid) where cityid = @cityid";
var result = connection.Query<dynamic>(SQL, parameters_cityid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_cityid(int cityid) \r\n {ex}");
            throw ex;
        }
        }
        public  IEnumerable<Object> GetListBy_geoid(int geoid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_geoid(int geoid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_geoid = new { @cid = cid,@uid=uid ,@geoid = geoid  };
            var SQL = "select pk_encode(cityid) as pkcol,cityid as value,cityname as label,* from GetTable(NULL::public.citymasters,@cid) where geoid = @geoid";
var result = connection.Query<dynamic>(SQL, parameters_geoid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_geoid(int geoid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_citymaster(string sid)
{
        _logger.LogInfo("Getting into  Get_citymaster(string sid) api");
int id = Helper.GetId(sid);
  return  Get_citymaster(id);
}
        // GET: citymaster/5
//gets the screen record
        public  dynamic Get_citymaster(int id)
        {
        _logger.LogInfo("Getting into Get_citymaster(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.cityid) as pkcol,a.cityid as pk,a.*,
g.geoname as geoiddesc
 from GetTable(NULL::public.citymasters,@cid) a 
 left join geographymasters g on a.geoid=g.geoid
 where a.cityid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_citymaster = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'citymasters'";
var citymaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { citymaster=obj_citymaster,citymaster_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_citymaster(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.cityid) as pkcol,a.cityid as pk,* ,cityid as value,cityname as label  from GetTable(NULL::public.citymasters,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by cityname";
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
            var SQL = @"select pk_encode(a.cityid) as pkcol,a.cityid as pk,a.*,
g.geoname as geoiddesc from GetTable(NULL::public.citymasters,@cid) a 
 left join geographymasters g on a.geoid=g.geoid";
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
        public  dynamic Save_citymaster(string token,citymaster obj_citymaster)
        {
        _logger.LogInfo("Saving: Save_citymaster(string token,citymaster obj_citymaster) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_citymaster.cityname!=null )
{
var parameterscityname =new {@cid=cid,@uid=uid, @cityname = obj_citymaster.cityname,@cityid=obj_citymaster.cityid };
                    if(Helper.Count("select count(*) from citymasters where  and cityname =  @cityname and (@cityid == 0 ||  @cityid == null ||  @cityid < 0 || cityid!=  @cityid)",parameterscityname)> 0) serr +="cityname is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //citymaster table
                if (obj_citymaster.cityid == 0 || obj_citymaster.cityid == null || obj_citymaster.cityid<0)
{
if(obj_citymaster.status=="" || obj_citymaster.status==null)obj_citymaster.status="A";
//obj_citymaster.companyid=cid;
obj_citymaster.createdby=uid;
obj_citymaster.createddate=DateTime.Now;
                    _context.citymasters.Add((dynamic)obj_citymaster);
querytype=1;
}
                else
{
//obj_citymaster.companyid=cid;
obj_citymaster.updatedby=uid;
obj_citymaster.updateddate=DateTime.Now;
                    _context.Entry(obj_citymaster).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_citymaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_citymaster).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api citymasters ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_citymaster,"citymasters", 0,obj_citymaster.cityid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_citymaster( (int)obj_citymaster.cityid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_citymaster(string token,citymaster obj_citymaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: citymaster/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
citymaster obj_citymaster = _context.citymasters.Find(id);
_context.citymasters.Remove(obj_citymaster);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api citymasters ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool citymaster_Exists(int id)
        {
        try{
            return _context.citymasters.Count(e => e.cityid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:citymaster_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

