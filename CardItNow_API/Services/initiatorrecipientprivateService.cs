
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
    public class initiatorrecipientprivateService : IinitiatorrecipientprivateService
    {
        private readonly IConfiguration Configuration;
        private readonly initiatorrecipientprivateContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IinitiatorrecipientprivateService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public initiatorrecipientprivateService(initiatorrecipientprivateContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/initiatorrecipientprivate
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_initiatorrecipientprivates()
        {
        _logger.LogInfo("Getting into Get_initiatorrecipientprivates() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.privateid) as pkcol,privateid as value,uid as label from GetTable(NULL::public.initiatorrecipientprivates,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_initiatorrecipientprivates(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_privateid(int privateid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_privateid(int privateid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_privateid = new { @cid = cid,@uid=uid ,@privateid = privateid  };
            var SQL = "select pk_encode(privateid) as pkcol,privateid as value,uid as label,* from GetTable(NULL::public.initiatorrecipientprivates,@cid) where privateid = @privateid";
var result = connection.Query<dynamic>(SQL, parameters_privateid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_privateid(int privateid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_initiatorrecipientprivate(string sid)
{
        _logger.LogInfo("Getting into  Get_initiatorrecipientprivate(string sid) api");
int id = Helper.GetId(sid);
  return  Get_initiatorrecipientprivate(id);
}
        // GET: initiatorrecipientprivate/5
//gets the screen record
        public  dynamic Get_initiatorrecipientprivate(int id)
        {
        _logger.LogInfo("Getting into Get_initiatorrecipientprivate(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vcustomermastertype ="customermastertype";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vcustomermastertype =vcustomermastertype};
var SQL = @"select pk_encode(a.privateid) as pkcol,a.privateid as pk,a.*,
i.email as uiddesc,
s.cityname as cityiddesc,
                              y.configtext as typedesc,
u.email as customeriddesc,
g.geoname as geoiddesc
 from GetTable(NULL::public.initiatorrecipientprivates,@cid) a 
 left join customermasters i on a.uid=i.uid
 left join citymasters s on a.cityid=s.cityid
 left join boconfigvalues y on a.type=y.configkey and @vcustomermastertype=y.param
 left join customermasters u on a.customerid=u.customerid
 left join geographymasters g on a.geoid=g.geoid
 where a.privateid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_initiatorrecipientprivate = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'initiatorrecipientprivates'";
var initiatorrecipientprivate_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { initiatorrecipientprivate=obj_initiatorrecipientprivate,initiatorrecipientprivate_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_initiatorrecipientprivate(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.privateid) as pkcol,a.privateid as pk,* ,privateid as value,uid as label  from GetTable(NULL::public.initiatorrecipientprivates,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by uid";
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
string vcustomermastertype ="customermastertype";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vcustomermastertype =vcustomermastertype};
            var SQL = @"select pk_encode(a.privateid) as pkcol,a.privateid as pk,a.*,
i.email as uiddesc,
s.cityname as cityiddesc,
                              y.configtext as typedesc,
u.email as customeriddesc,
g.geoname as geoiddesc from GetTable(NULL::public.initiatorrecipientprivates,@cid) a 
 left join customermasters i on a.uid=i.uid
 left join citymasters s on a.cityid=s.cityid
 left join boconfigvalues y on a.type=y.configkey and @vcustomermastertype=y.param
 left join customermasters u on a.customerid=u.customerid
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
        public  dynamic Save_initiatorrecipientprivate(string token,initiatorrecipientprivate obj_initiatorrecipientprivate)
        {
        _logger.LogInfo("Saving: Save_initiatorrecipientprivate(string token,initiatorrecipientprivate obj_initiatorrecipientprivate) ");
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
                //initiatorrecipientprivate table
                if (obj_initiatorrecipientprivate.privateid == 0 || obj_initiatorrecipientprivate.privateid == null || obj_initiatorrecipientprivate.privateid<0)
{
if(obj_initiatorrecipientprivate.status=="" || obj_initiatorrecipientprivate.status==null)obj_initiatorrecipientprivate.status="A";
//obj_initiatorrecipientprivate.companyid=cid;
obj_initiatorrecipientprivate.createdby=uid;
obj_initiatorrecipientprivate.createddate=DateTime.Now;
                    _context.initiatorrecipientprivates.Add((dynamic)obj_initiatorrecipientprivate);
querytype=1;
}
                else
{
//obj_initiatorrecipientprivate.companyid=cid;
obj_initiatorrecipientprivate.updatedby=uid;
obj_initiatorrecipientprivate.updateddate=DateTime.Now;
                    _context.Entry(obj_initiatorrecipientprivate).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_initiatorrecipientprivate).Property("createdby").IsModified = false;
                    _context.Entry(obj_initiatorrecipientprivate).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api initiatorrecipientprivates ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_initiatorrecipientprivate,"initiatorrecipientprivates", 0,obj_initiatorrecipientprivate.privateid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_initiatorrecipientprivate( (int)obj_initiatorrecipientprivate.privateid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_initiatorrecipientprivate(string token,initiatorrecipientprivate obj_initiatorrecipientprivate) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: initiatorrecipientprivate/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
initiatorrecipientprivate obj_initiatorrecipientprivate = _context.initiatorrecipientprivates.Find(id);
_context.initiatorrecipientprivates.Remove(obj_initiatorrecipientprivate);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api initiatorrecipientprivates ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool initiatorrecipientprivate_Exists(int id)
        {
        try{
            return _context.initiatorrecipientprivates.Count(e => e.privateid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:initiatorrecipientprivate_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

