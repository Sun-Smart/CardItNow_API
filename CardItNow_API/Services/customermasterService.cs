
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
    public class customermasterService : IcustomermasterService
    {
        private readonly IConfiguration Configuration;
        private readonly customermasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomermasterService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public customermasterService(customermasterContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/customermaster
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customermasters()
        {
        _logger.LogInfo("Getting into Get_customermasters() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.customerid) as pkcol,customerid as value,uid as label from GetTable(NULL::public.customermasters,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_customermasters(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_customerid(int customerid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_customerid(int customerid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_customerid = new { @cid = cid,@uid=uid ,@customerid = customerid  };
            var SQL = "select pk_encode(customerid) as pkcol,customerid as value,uid as label,* from GetTable(NULL::public.customermasters,@cid) where customerid = @customerid";
var result = connection.Query<dynamic>(SQL, parameters_customerid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_customerid(int customerid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_customermaster(string sid)
{
        _logger.LogInfo("Getting into  Get_customermaster(string sid) api");
int id = Helper.GetId(sid);
  return  Get_customermaster(id);
}
        // GET: customermaster/5
//gets the screen record
        public  dynamic Get_customermaster(int id)
        {
        _logger.LogInfo("Getting into Get_customermaster(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vmode ="mode";
string vcustomermastertype ="customermastertype";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vmode =vmode,@vcustomermastertype =vcustomermastertype};
var SQL = @"select pk_encode(a.customerid) as pkcol,a.customerid as pk,a.*,
                              d.configtext as modedesc,
                              t.configtext as typedesc,
s.avatarname as defaultavatardesc
 from GetTable(NULL::public.customermasters,@cid) a 
 left join boconfigvalues d on a.mode=d.configkey and @vmode=d.param
 left join boconfigvalues t on a.type=t.configkey and @vcustomermastertype=t.param
 left join avatarmasters s on a.defaultavatar=s.avatarid
 where a.customerid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_customermaster = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customermasters'";
var customermaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { customermaster=obj_customermaster,customermaster_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_customermaster(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.customerid) as pkcol,a.customerid as pk,* ,customerid as value,uid as label  from GetTable(NULL::public.customermasters,@cid) a ";
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
string vmode ="mode";
string vcustomermastertype ="customermastertype";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vmode =vmode,@vcustomermastertype =vcustomermastertype};
            var SQL = @"select pk_encode(a.customerid) as pkcol,a.customerid as pk,a.*,
                              d.configtext as modedesc,
                              t.configtext as typedesc,
s.avatarname as defaultavatardesc from GetTable(NULL::public.customermasters,@cid) a 
 left join boconfigvalues d on a.mode=d.configkey and @vmode=d.param
 left join boconfigvalues t on a.type=t.configkey and @vcustomermastertype=t.param
 left join avatarmasters s on a.defaultavatar=s.avatarid";
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
        public  dynamic Save_customermaster(string token,customermaster obj_customermaster)
        {
        _logger.LogInfo("Saving: Save_customermaster(string token,customermaster obj_customermaster) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_customermaster.uid!=null )
{
var parametersuid =new {@cid=cid,@uid=uid, @cuid = obj_customermaster.uid,@customerid=obj_customermaster.customerid };
                    if(Helper.Count("select count(*) from customermasters where  and uid =  @cuid and (@customerid == 0 ||  @customerid == null ||  @customerid < 0 || customerid!=  @customerid)",parametersuid)> 0) serr +="uid is unique\r\n";
}
if( obj_customermaster.email!=null )
{
var parametersemail =new {@cid=cid,@uid=uid, @email = obj_customermaster.email,@customerid=obj_customermaster.customerid };
                    if(Helper.Count("select count(*) from customermasters where  and email =  @email and (@customerid == 0 ||  @customerid == null ||  @customerid < 0 || customerid!=  @customerid)",parametersemail)> 0) serr +="email is unique\r\n";
}
if( obj_customermaster.mobile!=null )
{
var parametersmobile =new {@cid=cid,@uid=uid, @mobile = obj_customermaster.mobile,@customerid=obj_customermaster.customerid };
                    if(Helper.Count("select count(*) from customermasters where  and mobile =  @mobile and (@customerid == 0 ||  @customerid == null ||  @customerid < 0 || customerid!=  @customerid)",parametersmobile)> 0) serr +="mobile is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //customermaster table
                if (obj_customermaster.customerid == 0 || obj_customermaster.customerid == null || obj_customermaster.customerid<0)
{
if(obj_customermaster.status=="" || obj_customermaster.status==null)obj_customermaster.status="A";
//obj_customermaster.companyid=cid;
obj_customermaster.createdby=uid;
obj_customermaster.createddate=DateTime.Now;
                    _context.customermasters.Add((dynamic)obj_customermaster);
querytype=1;
}
                else
{
//obj_customermaster.companyid=cid;
obj_customermaster.updatedby=uid;
obj_customermaster.updateddate=DateTime.Now;
                    _context.Entry(obj_customermaster).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customermaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_customermaster).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api customermasters ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_customermaster,"customermasters", 0,obj_customermaster.customerid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_customermaster( (int)obj_customermaster.customerid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_customermaster(string token,customermaster obj_customermaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customermaster/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
customermaster obj_customermaster = _context.customermasters.Find(id);
_context.customermasters.Remove(obj_customermaster);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api customermasters ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool customermaster_Exists(int id)
        {
        try{
            return _context.customermasters.Count(e => e.customerid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:customermaster_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

