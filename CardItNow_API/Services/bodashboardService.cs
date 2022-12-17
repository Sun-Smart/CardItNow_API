
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

namespace nTireBO.Services
{
    public class bodashboardService : IbodashboardService
    {
        private readonly IConfiguration Configuration;
        private readonly bodashboardContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbodashboardService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public bodashboardService(bodashboardContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/bodashboard
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_bodashboards()
        {
        _logger.LogInfo("Getting into Get_bodashboards() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.dashboardid) as pkcol,dashboardid as value,dashboardname as label from GetTable(NULL::public.bodashboards,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_bodashboards(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_dashboardid(int dashboardid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_dashboardid(int dashboardid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_dashboardid = new { @cid = cid,@uid=uid ,@dashboardid = dashboardid  };
            var SQL = "select pk_encode(dashboardid) as pkcol,dashboardid as value,dashboardname as label,* from GetTable(NULL::public.bodashboards,@cid) where dashboardid = @dashboardid";
var result = connection.Query<dynamic>(SQL, parameters_dashboardid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_dashboardid(int dashboardid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_bodashboard(string sid)
{
        _logger.LogInfo("Getting into  Get_bodashboard(string sid) api");
int id = Helper.GetId(sid);
  return  Get_bodashboard(id);
}
        // GET: bodashboard/5
//gets the screen record
        public  dynamic Get_bodashboard(int id)
        {
        _logger.LogInfo("Getting into Get_bodashboard(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.dashboardid) as pkcol,a.dashboardid as pk,a.*,
db.dashboardname as dashboardiddesc
 from GetTable(NULL::public.bodashboards,@cid) a 
 left join bodashboards db on a.dashboardid=db.dashboardid
 where a.dashboardid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_bodashboard = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bodashboards'";
var bodashboard_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { bodashboard=obj_bodashboard,bodashboard_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_bodashboard(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.dashboardid) as pkcol,a.dashboardid as pk,* ,dashboardid as value,dashboardname as label  from GetTable(NULL::public.bodashboards,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by dashboardname";
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
            var SQL = @"select pk_encode(a.dashboardid) as pkcol,a.dashboardid as pk,a.*,
db.dashboardname as dashboardiddesc from GetTable(NULL::public.bodashboards,@cid) a 
 left join bodashboards db on a.dashboardid=db.dashboardid";
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
        public  dynamic Save_bodashboard(string token,bodashboard obj_bodashboard)
        {
        _logger.LogInfo("Saving: Save_bodashboard(string token,bodashboard obj_bodashboard) ");
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
                //bodashboard table
                if (obj_bodashboard.dashboardid == 0 || obj_bodashboard.dashboardid == null || obj_bodashboard.dashboardid<0)
{
if(obj_bodashboard.status=="" || obj_bodashboard.status==null)obj_bodashboard.status="A";
obj_bodashboard.createdby=uid;
obj_bodashboard.createddate=DateTime.Now;
                    _context.bodashboards.Add((dynamic)obj_bodashboard);
querytype=1;
}
                else
{
obj_bodashboard.updatedby=uid;
obj_bodashboard.updateddate=DateTime.Now;
                    _context.Entry(obj_bodashboard).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_bodashboard).Property("createdby").IsModified = false;
                    _context.Entry(obj_bodashboard).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api bodashboards ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_bodashboard,"bodashboards", 0,obj_bodashboard.dashboardid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_bodashboard( (int)obj_bodashboard.dashboardid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_bodashboard(string token,bodashboard obj_bodashboard) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: bodashboard/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
bodashboard obj_bodashboard = _context.bodashboards.Find(id);
_context.bodashboards.Remove(obj_bodashboard);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api bodashboards ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool bodashboard_Exists(int id)
        {
        try{
            return _context.bodashboards.Count(e => e.dashboardid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:bodashboard_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

