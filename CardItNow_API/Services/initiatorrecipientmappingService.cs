
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
    public class initiatorrecipientmappingService : IinitiatorrecipientmappingService
    {
        private readonly IConfiguration Configuration;
        private readonly initiatorrecipientmappingContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IinitiatorrecipientmappingService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public initiatorrecipientmappingService(initiatorrecipientmappingContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/initiatorrecipientmapping
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_initiatorrecipientmappings()
        {
        _logger.LogInfo("Getting into Get_initiatorrecipientmappings() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.mappingid) as pkcol,mappingid as value,uid as label from GetTable(NULL::public.initiatorrecipientmappings,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_initiatorrecipientmappings(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_mappingid(int mappingid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_mappingid(int mappingid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_mappingid = new { @cid = cid,@uid=uid ,@mappingid = mappingid  };
            var SQL = "select pk_encode(mappingid) as pkcol,mappingid as value,uid as label,* from GetTable(NULL::public.initiatorrecipientmappings,@cid) where mappingid = @mappingid";
var result = connection.Query<dynamic>(SQL, parameters_mappingid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_mappingid(int mappingid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_initiatorrecipientmapping(string sid)
{
        _logger.LogInfo("Getting into  Get_initiatorrecipientmapping(string sid) api");
int id = Helper.GetId(sid);
  return  Get_initiatorrecipientmapping(id);
}
        // GET: initiatorrecipientmapping/5
//gets the screen record
        public  dynamic Get_initiatorrecipientmapping(int id)
        {
        _logger.LogInfo("Getting into Get_initiatorrecipientmapping(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.mappingid) as pkcol,a.mappingid as pk,a.*,
r.email as customeriddesc,
i.email as uiddesc,
r1.email as recipientuiddesc
 from GetTable(NULL::public.initiatorrecipientmappings,@cid) a 
 left join customermasters r on a.customerid=r.customerid
 left join customermasters i on a.uid=i.uid
 left join customermasters  r1 on   a.recipientuid=r1.uid
 where a.mappingid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_initiatorrecipientmapping = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'initiatorrecipientmappings'";
var initiatorrecipientmapping_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { initiatorrecipientmapping=obj_initiatorrecipientmapping,initiatorrecipientmapping_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_initiatorrecipientmapping(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.mappingid) as pkcol,a.mappingid as pk,* ,mappingid as value,uid as label  from GetTable(NULL::public.initiatorrecipientmappings,@cid) a ";
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
            var SQL = @"select pk_encode(a.mappingid) as pkcol,a.mappingid as pk,a.*,
r.email as customeriddesc,
i.email as uiddesc,
r.email as recipientuiddesc from GetTable(NULL::public.initiatorrecipientmappings,@cid) a 
 left join customermasters r on a.customerid=r.customerid
 left join customermasters i on a.uid=i.uid
 left join  r on  r.companyid=@cid  and a.recipientuid=r.uid";
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
        public  dynamic Save_initiatorrecipientmapping(string token,initiatorrecipientmapping obj_initiatorrecipientmapping)
        {
        _logger.LogInfo("Saving: Save_initiatorrecipientmapping(string token,initiatorrecipientmapping obj_initiatorrecipientmapping) ");
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
                //initiatorrecipientmapping table
                if (obj_initiatorrecipientmapping.mappingid == 0 || obj_initiatorrecipientmapping.mappingid == null || obj_initiatorrecipientmapping.mappingid<0)
{
if(obj_initiatorrecipientmapping.status=="" || obj_initiatorrecipientmapping.status==null)obj_initiatorrecipientmapping.status="A";
//obj_initiatorrecipientmapping.companyid=cid;
obj_initiatorrecipientmapping.createdby=uid;
obj_initiatorrecipientmapping.createddate=DateTime.Now;
                    _context.initiatorrecipientmappings.Add((dynamic)obj_initiatorrecipientmapping);
querytype=1;
}
                else
{
//obj_initiatorrecipientmapping.companyid=cid;
obj_initiatorrecipientmapping.updatedby=uid;
obj_initiatorrecipientmapping.updateddate=DateTime.Now;
                    _context.Entry(obj_initiatorrecipientmapping).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_initiatorrecipientmapping).Property("createdby").IsModified = false;
                    _context.Entry(obj_initiatorrecipientmapping).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api initiatorrecipientmappings ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_initiatorrecipientmapping,"initiatorrecipientmappings", 0,obj_initiatorrecipientmapping.mappingid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_initiatorrecipientmapping( (int)obj_initiatorrecipientmapping.mappingid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_initiatorrecipientmapping(string token,initiatorrecipientmapping obj_initiatorrecipientmapping) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: initiatorrecipientmapping/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
initiatorrecipientmapping obj_initiatorrecipientmapping = _context.initiatorrecipientmappings.Find(id);
_context.initiatorrecipientmappings.Remove(obj_initiatorrecipientmapping);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api initiatorrecipientmappings ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool initiatorrecipientmapping_Exists(int id)
        {
        try{
            return _context.initiatorrecipientmappings.Count(e => e.mappingid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:initiatorrecipientmapping_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

