
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
    public class customersecurityquestionshistoryService : IcustomersecurityquestionshistoryService
    {
        private readonly IConfiguration Configuration;
        private readonly customersecurityquestionshistoryContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomersecurityquestionshistoryService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public customersecurityquestionshistoryService(customersecurityquestionshistoryContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
        {
Configuration = configuration;
            _context = context;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
            if (httpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            }
        }

        // GET: service/customersecurityquestionshistory
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customersecurityquestionshistories()
        {
        _logger.LogInfo("Getting into Get_customersecurityquestionshistories() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.historyid) as pkcol,historyid as value,oldanswer as label from GetTable(NULL::public.customersecurityquestionshistories,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_customersecurityquestionshistories(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_historyid(int historyid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_historyid(int historyid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_historyid = new { @cid = cid,@uid=uid ,@historyid = historyid  };
            var SQL = "select pk_encode(historyid) as pkcol,historyid as value,oldanswer as label,* from GetTable(NULL::public.customersecurityquestionshistories,@cid) where historyid = @historyid";
var result = connection.Query<dynamic>(SQL, parameters_historyid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_historyid(int historyid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_customersecurityquestionshistory(string sid)
{
        _logger.LogInfo("Getting into  Get_customersecurityquestionshistory(string sid) api");
int id = Helper.GetId(sid);
  return  Get_customersecurityquestionshistory(id);
}
        // GET: customersecurityquestionshistory/5
//gets the screen record
        public  dynamic Get_customersecurityquestionshistory(int id)
        {
        _logger.LogInfo("Getting into Get_customersecurityquestionshistory(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.historyid) as pkcol,a.historyid as pk,a.*,
u.email as customeriddesc,
q.masterdatadescription as securityquestioniddesc
 from GetTable(NULL::public.customersecurityquestionshistories,@cid) a 
 left join customermasters u on a.customerid=u.customerid
 left join masterdatas q on a.securityquestionid=q.masterdataid
 where a.historyid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_customersecurityquestionshistory = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customersecurityquestionshistories'";
var customersecurityquestionshistory_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { customersecurityquestionshistory=obj_customersecurityquestionshistory,customersecurityquestionshistory_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_customersecurityquestionshistory(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.historyid) as pkcol,a.historyid as pk,* ,historyid as value,oldanswer as label  from GetTable(NULL::public.customersecurityquestionshistories,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by oldanswer";
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
            var SQL = @"select pk_encode(a.historyid) as pkcol,a.historyid as pk,a.*,
u.email as customeriddesc,
q.masterdatadescription as securityquestioniddesc from GetTable(NULL::public.customersecurityquestionshistories,@cid) a 
 left join customermasters u on a.customerid=u.customerid
 left join masterdatas q on a.securityquestionid=q.masterdataid";
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
        public  dynamic Save_customersecurityquestionshistory(string token,customersecurityquestionshistory obj_customersecurityquestionshistory)
        {
        _logger.LogInfo("Saving: Save_customersecurityquestionshistory(string token,customersecurityquestionshistory obj_customersecurityquestionshistory) ");
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
                //customersecurityquestionshistory table
                if (obj_customersecurityquestionshistory.historyid == 0 || obj_customersecurityquestionshistory.historyid == null || obj_customersecurityquestionshistory.historyid<0)
{
if(obj_customersecurityquestionshistory.status=="" || obj_customersecurityquestionshistory.status==null)obj_customersecurityquestionshistory.status="A";
//obj_customersecurityquestionshistory.companyid=cid;
obj_customersecurityquestionshistory.createdby=uid;
obj_customersecurityquestionshistory.createddate=DateTime.Now;
                    _context.customersecurityquestionshistories.Add((dynamic)obj_customersecurityquestionshistory);
querytype=1;
}
                else
{
//obj_customersecurityquestionshistory.companyid=cid;
obj_customersecurityquestionshistory.updatedby=uid;
obj_customersecurityquestionshistory.updateddate=DateTime.Now;
                    _context.Entry(obj_customersecurityquestionshistory).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customersecurityquestionshistory).Property("createdby").IsModified = false;
                    _context.Entry(obj_customersecurityquestionshistory).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api customersecurityquestionshistories ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_customersecurityquestionshistory,"customersecurityquestionshistories", 0,obj_customersecurityquestionshistory.historyid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_customersecurityquestionshistory( (int)obj_customersecurityquestionshistory.historyid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_customersecurityquestionshistory(string token,customersecurityquestionshistory obj_customersecurityquestionshistory) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customersecurityquestionshistory/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
customersecurityquestionshistory obj_customersecurityquestionshistory = _context.customersecurityquestionshistories.Find(id);
_context.customersecurityquestionshistories.Remove(obj_customersecurityquestionshistory);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api customersecurityquestionshistories ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool customersecurityquestionshistory_Exists(int id)
        {
        try{
            return _context.customersecurityquestionshistories.Count(e => e.historyid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:customersecurityquestionshistory_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

