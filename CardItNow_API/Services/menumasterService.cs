
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
    public class menumasterService : ImenumasterService
    {
        private readonly IConfiguration Configuration;
        private readonly menumasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ImenumasterService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public menumasterService(menumasterContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/menumaster
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_menumasters()
        {
        _logger.LogInfo("Getting into Get_menumasters() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.menuid) as pkcol,menuid as value,menuname as label from GetTable(NULL::public.menumasters,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_menumasters(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_menuid(int menuid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_menuid(int menuid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_menuid = new { @cid = cid,@uid=uid ,@menuid = menuid  };
            var SQL = "select pk_encode(menuid) as pkcol,menuid as value,menuname as label,* from GetTable(NULL::public.menumasters,@cid) where menuid = @menuid";
var result = connection.Query<dynamic>(SQL, parameters_menuid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_menuid(int menuid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_menumaster(string sid)
{
        _logger.LogInfo("Getting into  Get_menumaster(string sid) api");
int id = Helper.GetId(sid);
  return  Get_menumaster(id);
}

        public IEnumerable<Object> Get_usermenumaster(dynamic param)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into Get_bousermenumaster (dynamic param) api");
                    int? sessionuserid = uid;
                    var parametersusermaster = new { @sessionuserid = sessionuserid, @param = param };
                    string SQLusermaster = "select * from usermasters a WHERE userid = @sessionuserid";
                    var usermasterresult = connection.Query<dynamic>(SQLusermaster, parametersusermaster).FirstOrDefault();
                    if (true || usermasterresult.userroleid == 6)
                    {
                        string SQLresult = @"select distinct * from menumasters where  status='A' order by menuname ";
                        var result = connection.Query<dynamic>(SQLresult, parametersusermaster).ToList();
                        connection.Close();
                        connection.Dispose();
                        return (result);
                    }
                    else
                    {
                        string SQLresult = @"select distinct * from bousermenuaccesses c where c.userid = @sessionuserid  order by parentid,orderno,menudescription";
                        var result = connection.Query<dynamic>(SQLresult, parametersusermaster).ToList();
                        connection.Close();
                        connection.Dispose();
                        return (result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:Get_bousermenumaster (dynamic param) \r\n {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetListBy_menuurl(string menuurl)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_menuurl(string menuurl) api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_menuurl = new { @cid = cid, @menuurl = menuurl };
                    var SQL = "select pk_encode(menuid) as pkcol,menuid as value,menudescription as label,* from bomenumasters where menuurl = @menuurl";
                    var result = connection.Query<dynamic>(SQL, parameters_menuurl);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_menuurl(string menuurl) \r\n {ex}");
                throw ex;
            }
        }
        // GET: menumaster/5
        //gets the screen record
        public  dynamic Get_menumaster(int id)
        {
        _logger.LogInfo("Getting into Get_menumaster(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.menuid) as pkcol,a.menuid as pk,a.*
 from GetTable(NULL::public.menumasters,@cid) a 
 where a.menuid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_menumaster = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'menumasters'";
var menumaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { menumaster=obj_menumaster,menumaster_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_menumaster(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.menuid) as pkcol,a.menuid as pk,* ,menuid as value,menuname as label  from GetTable(NULL::public.menumasters,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by menuname";
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
            var SQL = @"select pk_encode(a.menuid) as pkcol,a.menuid as pk,a.* from GetTable(NULL::public.menumasters,@cid) a ";
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
        public  dynamic Save_menumaster(string token,menumaster obj_menumaster)
        {
        _logger.LogInfo("Saving: Save_menumaster(string token,menumaster obj_menumaster) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_menumaster.menuname!=null )
{
var parametersmenuname =new {@cid=cid,@uid=uid, @menuname = obj_menumaster.menuname,@menuid=obj_menumaster.menuid };
                    if(Helper.Count("select count(*) from menumasters where  and menuname =  @menuname and (@menuid == 0 ||  @menuid == null ||  @menuid < 0 || menuid!=  @menuid)",parametersmenuname)> 0) serr +="menuname is unique\r\n";
}
if( obj_menumaster.menulocation!=null )
{
var parametersmenulocation =new {@cid=cid,@uid=uid, @menulocation = obj_menumaster.menulocation,@menuid=obj_menumaster.menuid };
                    if(Helper.Count("select count(*) from menumasters where  and menulocation =  @menulocation and (@menuid == 0 ||  @menuid == null ||  @menuid < 0 || menuid!=  @menuid)",parametersmenulocation)> 0) serr +="menulocation is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //menumaster table
                if (obj_menumaster.menuid == 0 || obj_menumaster.menuid == null || obj_menumaster.menuid<0)
{
if(obj_menumaster.status=="" || obj_menumaster.status==null)obj_menumaster.status="A";
//obj_menumaster.companyid=cid;
obj_menumaster.createdby=uid;
obj_menumaster.createddate=DateTime.Now;
                    _context.menumasters.Add((dynamic)obj_menumaster);
querytype=1;
}
                else
{
//obj_menumaster.companyid=cid;
obj_menumaster.updatedby=uid;
obj_menumaster.updateddate=DateTime.Now;
                    _context.Entry(obj_menumaster).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_menumaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_menumaster).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api menumasters ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_menumaster,"menumasters", 0,obj_menumaster.menuid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_menumaster( (int)obj_menumaster.menuid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_menumaster(string token,menumaster obj_menumaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: menumaster/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
menumaster obj_menumaster = _context.menumasters.Find(id);
_context.menumasters.Remove(obj_menumaster);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api menumasters ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool menumaster_Exists(int id)
        {
        try{
            return _context.menumasters.Count(e => e.menuid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:menumaster_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

