
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
    public class transactionitemdetailService : ItransactionitemdetailService
    {
        private readonly IConfiguration Configuration;
        private readonly transactionitemdetailContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ItransactionitemdetailService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public transactionitemdetailService(transactionitemdetailContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/transactionitemdetail
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_transactionitemdetails()
        {
        _logger.LogInfo("Getting into Get_transactionitemdetails() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.transactionitemdetailid) as pkcol,transactionitemdetailid as value,uid as label from GetTable(NULL::public.transactionitemdetails,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_transactionitemdetails(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_transactionitemdetailid(int transactionitemdetailid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_transactionitemdetailid(int transactionitemdetailid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_transactionitemdetailid = new { @cid = cid,@uid=uid ,@transactionitemdetailid = transactionitemdetailid  };
            var SQL = "select pk_encode(transactionitemdetailid) as pkcol,transactionitemdetailid as value,uid as label,* from GetTable(NULL::public.transactionitemdetails,@cid) where transactionitemdetailid = @transactionitemdetailid";
var result = connection.Query<dynamic>(SQL, parameters_transactionitemdetailid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_transactionitemdetailid(int transactionitemdetailid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_transactionitemdetail(string sid)
{
        _logger.LogInfo("Getting into  Get_transactionitemdetail(string sid) api");
int id = Helper.GetId(sid);
  return  Get_transactionitemdetail(id);
}
        // GET: transactionitemdetail/5
//gets the screen record
        public  dynamic Get_transactionitemdetail(int id)
        {
        _logger.LogInfo("Getting into Get_transactionitemdetail(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.transactionitemdetailid) as pkcol,a.transactionitemdetailid as pk,a.*,
tm.documentnumber as transactioniddesc,
u.email as uiddesc,
r.email as recipientuiddesc,
d.cardname || '  ' || d.cardnumber as payiddesc,
t.acquirername as transactiondetailiddesc,
v.email as privateiddesc
 from GetTable(NULL::public.transactionitemdetails,@cid) a 
 left join transactionmasters tm on a.transactionid=tm.transactionid
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 left join customerpaymodes d on a.payid=d.payid
 left join transactiondetails t on a.transactiondetailid=t.transactiondetailid
 left join initiatorrecipientprivates v on a.privateid=v.privateid
 where a.transactionitemdetailid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_transactionitemdetail = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'transactionitemdetails'";
var transactionitemdetail_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { transactionitemdetail=obj_transactionitemdetail,transactionitemdetail_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_transactionitemdetail(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.transactionitemdetailid) as pkcol,a.transactionitemdetailid as pk,* ,transactionitemdetailid as value,uid as label  from GetTable(NULL::public.transactionitemdetails,@cid) a ";
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
            var SQL = @"select pk_encode(a.transactionitemdetailid) as pkcol,a.transactionitemdetailid as pk,a.*,
tm.documentnumber as transactioniddesc,
u.email as uiddesc,
r.email as recipientuiddesc,
d.cardname || '  ' || d.cardnumber as payiddesc,
t.acquirername as transactiondetailiddesc,
v.email as privateiddesc from GetTable(NULL::public.transactionitemdetails,@cid) a 
 left join transactionmasters tm on a.transactionid=tm.transactionid
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 left join customerpaymodes d on a.payid=d.payid
 left join transactiondetails t on a.transactiondetailid=t.transactiondetailid
 left join initiatorrecipientprivates v on a.privateid=v.privateid";
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
        public  dynamic Save_transactionitemdetail(string token,transactionitemdetail obj_transactionitemdetail)
        {
        _logger.LogInfo("Saving: Save_transactionitemdetail(string token,transactionitemdetail obj_transactionitemdetail) ");
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
                //transactionitemdetail table
                if (obj_transactionitemdetail.transactionitemdetailid == 0 || obj_transactionitemdetail.transactionitemdetailid == null || obj_transactionitemdetail.transactionitemdetailid<0)
{
if(obj_transactionitemdetail.status=="" || obj_transactionitemdetail.status==null)obj_transactionitemdetail.status="A";
//obj_transactionitemdetail.companyid=cid;
obj_transactionitemdetail.createdby=uid;
obj_transactionitemdetail.createddate=DateTime.Now;
                    _context.transactionitemdetails.Add((dynamic)obj_transactionitemdetail);
querytype=1;
}
                else
{
//obj_transactionitemdetail.companyid=cid;
obj_transactionitemdetail.updatedby=uid;
obj_transactionitemdetail.updateddate=DateTime.Now;
                    _context.Entry(obj_transactionitemdetail).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_transactionitemdetail).Property("createdby").IsModified = false;
                    _context.Entry(obj_transactionitemdetail).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api transactionitemdetails ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_transactionitemdetail,"transactionitemdetails", 0,obj_transactionitemdetail.transactionitemdetailid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_transactionitemdetail( (int)obj_transactionitemdetail.transactionitemdetailid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_transactionitemdetail(string token,transactionitemdetail obj_transactionitemdetail) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: transactionitemdetail/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
transactionitemdetail obj_transactionitemdetail = _context.transactionitemdetails.Find(id);
_context.transactionitemdetails.Remove(obj_transactionitemdetail);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api transactionitemdetails ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool transactionitemdetail_Exists(int id)
        {
        try{
            return _context.transactionitemdetails.Count(e => e.transactionitemdetailid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:transactionitemdetail_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

