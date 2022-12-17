
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
    public class carditchargesdiscountService : IcarditchargesdiscountService
    {
        private readonly IConfiguration Configuration;
        private readonly carditchargesdiscountContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcarditchargesdiscountService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public carditchargesdiscountService(carditchargesdiscountContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/carditchargesdiscount
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_carditchargesdiscounts()
        {
        _logger.LogInfo("Getting into Get_carditchargesdiscounts() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.discountid) as pkcol,discountid as value,status as label from GetTable(NULL::public.carditchargesdiscounts,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_carditchargesdiscounts(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_discountid(int discountid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_discountid(int discountid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_discountid = new { @cid = cid,@uid=uid ,@discountid = discountid  };
            var SQL = "select pk_encode(discountid) as pkcol,discountid as value,status as label,* from GetTable(NULL::public.carditchargesdiscounts,@cid) where discountid = @discountid";
var result = connection.Query<dynamic>(SQL, parameters_discountid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_discountid(int discountid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_carditchargesdiscount(string sid)
{
        _logger.LogInfo("Getting into  Get_carditchargesdiscount(string sid) api");
int id = Helper.GetId(sid);
  return  Get_carditchargesdiscount(id);
}
        // GET: carditchargesdiscount/5
//gets the screen record
        public  dynamic Get_carditchargesdiscount(int id)
        {
        _logger.LogInfo("Getting into Get_carditchargesdiscount(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.discountid) as pkcol,a.discountid as pk,a.*,
u.email as recipientuiddesc
 from GetTable(NULL::public.carditchargesdiscounts,@cid) a 
 left join customermasters u on a.recipientuid=u.uid
 where a.discountid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_carditchargesdiscount = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'carditchargesdiscounts'";
var carditchargesdiscount_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { carditchargesdiscount=obj_carditchargesdiscount,carditchargesdiscount_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_carditchargesdiscount(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.discountid) as pkcol,a.discountid as pk,* ,discountid as value,status as label  from GetTable(NULL::public.carditchargesdiscounts,@cid) a ";
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
            var SQL = @"select pk_encode(a.discountid) as pkcol,a.discountid as pk,a.*,
u.email as recipientuiddesc from GetTable(NULL::public.carditchargesdiscounts,@cid) a 
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
        public  dynamic Save_carditchargesdiscount(string token,carditchargesdiscount obj_carditchargesdiscount)
        {
        _logger.LogInfo("Saving: Save_carditchargesdiscount(string token,carditchargesdiscount obj_carditchargesdiscount) ");
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
                //carditchargesdiscount table
                if (obj_carditchargesdiscount.discountid == 0 || obj_carditchargesdiscount.discountid == null || obj_carditchargesdiscount.discountid<0)
{
if(obj_carditchargesdiscount.status=="" || obj_carditchargesdiscount.status==null)obj_carditchargesdiscount.status="A";
//obj_carditchargesdiscount.companyid=cid;
obj_carditchargesdiscount.createdby=uid;
obj_carditchargesdiscount.createddate=DateTime.Now;
                    _context.carditchargesdiscounts.Add((dynamic)obj_carditchargesdiscount);
querytype=1;
}
                else
{
//obj_carditchargesdiscount.companyid=cid;
obj_carditchargesdiscount.updatedby=uid;
obj_carditchargesdiscount.updateddate=DateTime.Now;
                    _context.Entry(obj_carditchargesdiscount).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_carditchargesdiscount).Property("createdby").IsModified = false;
                    _context.Entry(obj_carditchargesdiscount).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api carditchargesdiscounts ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_carditchargesdiscount,"carditchargesdiscounts", 0,obj_carditchargesdiscount.discountid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_carditchargesdiscount( (int)obj_carditchargesdiscount.discountid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_carditchargesdiscount(string token,carditchargesdiscount obj_carditchargesdiscount) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: carditchargesdiscount/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
carditchargesdiscount obj_carditchargesdiscount = _context.carditchargesdiscounts.Find(id);
_context.carditchargesdiscounts.Remove(obj_carditchargesdiscount);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api carditchargesdiscounts ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool carditchargesdiscount_Exists(int id)
        {
        try{
            return _context.carditchargesdiscounts.Count(e => e.discountid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:carditchargesdiscount_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

