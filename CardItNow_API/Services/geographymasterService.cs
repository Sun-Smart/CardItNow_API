
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
    public class geographymasterService : IgeographymasterService
    {
        private readonly IConfiguration Configuration;
        private readonly geographymasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IgeographymasterService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public geographymasterService(geographymasterContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/geographymaster
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_geographymasters()
        {
        _logger.LogInfo("Getting into Get_geographymasters() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.geoid) as pkcol,geoid as value,geoname as label from GetTable(NULL::public.geographymasters,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_geographymasters(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_geoid(int geoid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_geoid(int geoid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_geoid = new { @cid = cid,@uid=uid ,@geoid = geoid  };
            var SQL = "select pk_encode(geoid) as pkcol,geoid as value,geoname as label,* from GetTable(NULL::public.geographymasters,@cid) where geoid = @geoid";
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
public  dynamic Get_geographymaster(string sid)
{
        _logger.LogInfo("Getting into  Get_geographymaster(string sid) api");
int id = Helper.GetId(sid);
  return  Get_geographymaster(id);
}
        // GET: geographymaster/5
//gets the screen record
        public  dynamic Get_geographymaster(int id)
        {
        _logger.LogInfo("Getting into Get_geographymaster(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.geoid) as pkcol,a.geoid as pk,a.*
 from GetTable(NULL::public.geographymasters,@cid) a 
 where a.geoid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_geographymaster = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'geographymasters'";
var geographymaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { geographymaster=obj_geographymaster,geographymaster_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_geographymaster(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.geoid) as pkcol,a.geoid as pk,* ,geoid as value,geoname as label  from GetTable(NULL::public.geographymasters,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by geoname";
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
            var SQL = @"select pk_encode(a.geoid) as pkcol,a.geoid as pk,a.* from GetTable(NULL::public.geographymasters,@cid) a ";
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
        public  dynamic Save_geographymaster(string token,geographymaster obj_geographymaster)
        {
        _logger.LogInfo("Saving: Save_geographymaster(string token,geographymaster obj_geographymaster) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_geographymaster.geoname!=null )
{
var parametersgeoname =new {@cid=cid,@uid=uid, @geoname = obj_geographymaster.geoname,@geoid=obj_geographymaster.geoid };
                    if(Helper.Count("select count(*) from geographymasters where  and geoname =  @geoname and (@geoid == 0 ||  @geoid == null ||  @geoid < 0 || geoid!=  @geoid)",parametersgeoname)> 0) serr +="geoname is unique\r\n";
}
if( obj_geographymaster.geocode!=null )
{
var parametersgeocode =new {@cid=cid,@uid=uid, @geocode = obj_geographymaster.geocode,@geoid=obj_geographymaster.geoid };
                    if(Helper.Count("select count(*) from geographymasters where  and geocode =  @geocode and (@geoid == 0 ||  @geoid == null ||  @geoid < 0 || geoid!=  @geoid)",parametersgeocode)> 0) serr +="geocode is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //geographymaster table
                if (obj_geographymaster.geoid == 0 || obj_geographymaster.geoid == null || obj_geographymaster.geoid<0)
{
if(obj_geographymaster.status=="" || obj_geographymaster.status==null)obj_geographymaster.status="A";
//obj_geographymaster.companyid=cid;
obj_geographymaster.createdby=uid;
obj_geographymaster.createddate=DateTime.Now;
                    _context.geographymasters.Add((dynamic)obj_geographymaster);
querytype=1;
}
                else
{
//obj_geographymaster.companyid=cid;
obj_geographymaster.updatedby=uid;
obj_geographymaster.updateddate=DateTime.Now;
                    _context.Entry(obj_geographymaster).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_geographymaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_geographymaster).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api geographymasters ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_geographymaster,"geographymasters", 0,obj_geographymaster.geoid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_geographymaster( (int)obj_geographymaster.geoid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_geographymaster(string token,geographymaster obj_geographymaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: geographymaster/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
geographymaster obj_geographymaster = _context.geographymasters.Find(id);
_context.geographymasters.Remove(obj_geographymaster);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api geographymasters ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool geographymaster_Exists(int id)
        {
        try{
            return _context.geographymasters.Count(e => e.geoid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:geographymaster_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

