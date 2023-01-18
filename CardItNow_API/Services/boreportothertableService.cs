
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nTireBO;
using nTireBO.Models;
using SunSmartnTireProducts.Helpers;
//////using FluentDateTime;
//////using FluentDate;
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
using SunSmartnTireProducts.Models;

namespace nTireBO.Services
{
    public class boreportothertableService : IboreportothertableService
    {
        private readonly IConfiguration Configuration;
        private readonly boreportothertableContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IboreportothertableService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public boreportothertableService(boreportothertableContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/boreportothertable
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_boreportothertables()
        {
        _logger.LogInfo("Getting into Get_boreportothertables() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.othertableid) as pkcol,othertableid as value,tablename as label from GetTable(NULL::public.boreportothertables,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_boreportothertables(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_othertableid(int othertableid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_othertableid(int othertableid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_othertableid = new { @cid = cid,@uid=uid ,@othertableid = othertableid  };
            var SQL = "select pk_encode(othertableid) as pkcol,othertableid as value,tablename as label,* from GetTable(NULL::public.boreportothertables,@cid) where othertableid = @othertableid";
var result = connection.Query<dynamic>(SQL, parameters_othertableid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_othertableid(int othertableid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_boreportothertable(string sid)
{
        _logger.LogInfo("Getting into  Get_boreportothertable(string sid) api");
int id = Helper.GetId(sid);
  return  Get_boreportothertable(id);
}
        // GET: boreportothertable/5
//gets the screen record
        public  dynamic Get_boreportothertable(int id)
        {
        _logger.LogInfo("Getting into Get_boreportothertable(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vjointype ="jointype";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vjointype =vjointype};
var SQL = @"select pk_encode(a.othertableid) as pkcol,a.othertableid as pk,a.*,
                              j.configtext as jointypedesc
 from GetTable(NULL::public.boreportothertables,@cid) a 
 left join boconfigvalues j on a.jointype=j.configkey and @vjointype=j.param
 where a.othertableid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_boreportothertable = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'boreportothertables'";
var boreportothertable_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { boreportothertable=obj_boreportothertable,boreportothertable_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_boreportothertable(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.othertableid) as pkcol,a.othertableid as pk,* ,othertableid as value,tablename as label  from GetTable(NULL::public.boreportothertables,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by tablename";
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
string vjointype ="jointype";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vjointype =vjointype};
            var SQL = @"select pk_encode(a.othertableid) as pkcol,a.othertableid as pk,a.*,
                              j.configtext as jointypedesc from GetTable(NULL::public.boreportothertables,@cid) a 
 left join boconfigvalues j on a.jointype=j.configkey and @vjointype=j.param";
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
        public  dynamic Save_boreportothertable(string token,boreportothertable obj_boreportothertable)
        {
        _logger.LogInfo("Saving: Save_boreportothertable(string token,boreportothertable obj_boreportothertable) ");
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
                //boreportothertable table
                if (obj_boreportothertable.othertableid == 0 || obj_boreportothertable.othertableid == null || obj_boreportothertable.othertableid<0)
{
if(obj_boreportothertable.status=="" || obj_boreportothertable.status==null)obj_boreportothertable.status="A";
obj_boreportothertable.createdby=uid;
obj_boreportothertable.createddate=DateTime.Now;
                    _context.boreportothertables.Add((dynamic)obj_boreportothertable);
querytype=1;
}
                else
{
obj_boreportothertable.updatedby=uid;
obj_boreportothertable.updateddate=DateTime.Now;
                    _context.Entry(obj_boreportothertable).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_boreportothertable).Property("createdby").IsModified = false;
                    _context.Entry(obj_boreportothertable).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api boreportothertables ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_boreportothertable,"boreportothertables", 0,obj_boreportothertable.othertableid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_boreportothertable( (int)obj_boreportothertable.othertableid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_boreportothertable(string token,boreportothertable obj_boreportothertable) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: boreportothertable/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
boreportothertable obj_boreportothertable = _context.boreportothertables.Find(id);
_context.boreportothertables.Remove(obj_boreportothertable);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api boreportothertables ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool boreportothertable_Exists(int id)
        {
        try{
            return _context.boreportothertables.Count(e => e.othertableid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:boreportothertable_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

