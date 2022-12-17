
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
    public class customerrecipientlinkService : IcustomerrecipientlinkService
    {
        private readonly IConfiguration Configuration;
        private readonly customerrecipientlinkContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomerrecipientlinkService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public customerrecipientlinkService(customerrecipientlinkContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/customerrecipientlink
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customerrecipientlinks()
        {
        _logger.LogInfo("Getting into Get_customerrecipientlinks() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.linkid) as pkcol,linkid as value,uid as label from GetTable(NULL::public.customerrecipientlinks,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_customerrecipientlinks(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_linkid(int linkid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_linkid(int linkid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_linkid = new { @cid = cid,@uid=uid ,@linkid = linkid  };
            var SQL = "select pk_encode(linkid) as pkcol,linkid as value,uid as label,* from GetTable(NULL::public.customerrecipientlinks,@cid) where linkid = @linkid";
var result = connection.Query<dynamic>(SQL, parameters_linkid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_linkid(int linkid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_customerrecipientlink(string sid)
{
        _logger.LogInfo("Getting into  Get_customerrecipientlink(string sid) api");
int id = Helper.GetId(sid);
  return  Get_customerrecipientlink(id);
}
        // GET: customerrecipientlink/5
//gets the screen record
        public  dynamic Get_customerrecipientlink(int id)
        {
        _logger.LogInfo("Getting into Get_customerrecipientlink(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.linkid) as pkcol,a.linkid as pk,a.*,
u.email as customeriddesc,
u1.email as uiddesc,
u2.email as recipientuiddesc
 from GetTable(NULL::public.customerrecipientlinks,@cid) a 
 left join customermasters u on a.customerid=u.customerid
 left join customermasters u1 on a.uid=u1.uid
 left join customermasters u2 on a.recipientuid=u2.uid
 where a.linkid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_customerrecipientlink = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerrecipientlinks'";
var customerrecipientlink_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { customerrecipientlink=obj_customerrecipientlink,customerrecipientlink_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_customerrecipientlink(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.linkid) as pkcol,a.linkid as pk,* ,linkid as value,uid as label  from GetTable(NULL::public.customerrecipientlinks,@cid) a ";
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
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
            var SQL = @"select pk_encode(a.linkid) as pkcol,a.linkid as pk,a.*,
u.email as customeriddesc,
u.email as uiddesc,
u.email as recipientuiddesc from GetTable(NULL::public.customerrecipientlinks,@cid) a 
 left join customermasters u on a.customerid=u.customerid
 left join customermasters u on a.uid=u.uid
 left join customermasters u on a.recipientuid=u.uid";
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
        public  dynamic Save_customerrecipientlink(string token,customerrecipientlink obj_customerrecipientlink)
        {
        _logger.LogInfo("Saving: Save_customerrecipientlink(string token,customerrecipientlink obj_customerrecipientlink) ");
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
                //customerrecipientlink table
                if (obj_customerrecipientlink.linkid == 0 || obj_customerrecipientlink.linkid == null || obj_customerrecipientlink.linkid<0)
{
if(obj_customerrecipientlink.status=="" || obj_customerrecipientlink.status==null)obj_customerrecipientlink.status="A";
//obj_customerrecipientlink.companyid=cid;
obj_customerrecipientlink.createdby=uid;
obj_customerrecipientlink.createddate=DateTime.Now;
                    _context.customerrecipientlinks.Add((dynamic)obj_customerrecipientlink);
querytype=1;
}
                else
{
//obj_customerrecipientlink.companyid=cid;
obj_customerrecipientlink.updatedby=uid;
obj_customerrecipientlink.updateddate=DateTime.Now;
                    _context.Entry(obj_customerrecipientlink).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customerrecipientlink).Property("createdby").IsModified = false;
                    _context.Entry(obj_customerrecipientlink).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api customerrecipientlinks ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_customerrecipientlink,"customerrecipientlinks", 0,obj_customerrecipientlink.linkid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_customerrecipientlink( (int)obj_customerrecipientlink.linkid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_customerrecipientlink(string token,customerrecipientlink obj_customerrecipientlink) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customerrecipientlink/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
customerrecipientlink obj_customerrecipientlink = _context.customerrecipientlinks.Find(id);
_context.customerrecipientlinks.Remove(obj_customerrecipientlink);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api customerrecipientlinks ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool customerrecipientlink_Exists(int id)
        {
        try{
            return _context.customerrecipientlinks.Count(e => e.linkid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:customerrecipientlink_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

