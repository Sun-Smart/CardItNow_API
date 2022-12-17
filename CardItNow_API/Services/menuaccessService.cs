
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
    public class menuaccessService : ImenuaccessService
    {
        private readonly IConfiguration Configuration;
        private readonly menuaccessContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ImenuaccessService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public menuaccessService(menuaccessContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/menuaccess
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_menuaccesses()
        {
        _logger.LogInfo("Getting into Get_menuaccesses() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.menuaccessid) as pkcol,menuaccessid as value,status as label from GetTable(NULL::public.menuaccesses,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_menuaccesses(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_menuaccessid(int menuaccessid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_menuaccessid(int menuaccessid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_menuaccessid = new { @cid = cid,@uid=uid ,@menuaccessid = menuaccessid  };
            var SQL = "select pk_encode(menuaccessid) as pkcol,menuaccessid as value,status as label,* from GetTable(NULL::public.menuaccesses,@cid) where menuaccessid = @menuaccessid";
var result = connection.Query<dynamic>(SQL, parameters_menuaccessid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_menuaccessid(int menuaccessid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_menuaccess(string sid)
{
        _logger.LogInfo("Getting into  Get_menuaccess(string sid) api");
int id = Helper.GetId(sid);
  return  Get_menuaccess(id);
}
        // GET: menuaccess/5
//gets the screen record
        public  dynamic Get_menuaccess(int id)
        {
        _logger.LogInfo("Getting into Get_menuaccess(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.menuaccessid) as pkcol,a.menuaccessid as pk,a.*,
m.menuname as menuiddesc,
r.roledescription as roleiddesc
 from GetTable(NULL::public.menuaccesses,@cid) a 
 left join menumasters m on a.menuid=m.menuid
 left join userrolemasters r on a.roleid=r.roleid
 where a.menuaccessid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_menuaccess = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'menuaccesses'";
var menuaccess_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { menuaccess=obj_menuaccess,menuaccess_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_menuaccess(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.menuaccessid) as pkcol,a.menuaccessid as pk,* ,menuaccessid as value,status as label  from GetTable(NULL::public.menuaccesses,@cid) a ";
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
            var SQL = @"select pk_encode(a.menuaccessid) as pkcol,a.menuaccessid as pk,a.*,
m.menuname as menuiddesc,
r.roledescription as roleiddesc from GetTable(NULL::public.menuaccesses,@cid) a 
 left join menumasters m on a.menuid=m.menuid
 left join userrolemasters r on a.roleid=r.roleid";
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
        public  dynamic Save_menuaccess(string token,menuaccess obj_menuaccess)
        {
        _logger.LogInfo("Saving: Save_menuaccess(string token,menuaccess obj_menuaccess) ");
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
                //menuaccess table
                if (obj_menuaccess.menuaccessid == 0 || obj_menuaccess.menuaccessid == null || obj_menuaccess.menuaccessid<0)
{
if(obj_menuaccess.status=="" || obj_menuaccess.status==null)obj_menuaccess.status="A";
//obj_menuaccess.companyid=cid;
obj_menuaccess.createdby=uid;
obj_menuaccess.createddate=DateTime.Now;
                    _context.menuaccesses.Add((dynamic)obj_menuaccess);
querytype=1;
}
                else
{
//obj_menuaccess.companyid=cid;
obj_menuaccess.updatedby=uid;
obj_menuaccess.updateddate=DateTime.Now;
                    _context.Entry(obj_menuaccess).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_menuaccess).Property("createdby").IsModified = false;
                    _context.Entry(obj_menuaccess).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api menuaccesses ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_menuaccess,"menuaccesses", 0,obj_menuaccess.menuaccessid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_menuaccess( (int)obj_menuaccess.menuaccessid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_menuaccess(string token,menuaccess obj_menuaccess) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: menuaccess/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
menuaccess obj_menuaccess = _context.menuaccesses.Find(id);
_context.menuaccesses.Remove(obj_menuaccess);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api menuaccesses ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool menuaccess_Exists(int id)
        {
        try{
            return _context.menuaccesses.Count(e => e.menuaccessid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:menuaccess_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

