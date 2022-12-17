
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
    public class transactiondetailService : ItransactiondetailService
    {
        private readonly IConfiguration Configuration;
        private readonly transactiondetailContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ItransactiondetailService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public transactiondetailService(transactiondetailContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/transactiondetail
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_transactiondetails()
        {
        _logger.LogInfo("Getting into Get_transactiondetails() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.transactiondetailid) as pkcol,transactiondetailid as value,uid as label from GetTable(NULL::public.transactiondetails,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_transactiondetails(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_transactiondetailid(int transactiondetailid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_transactiondetailid(int transactiondetailid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_transactiondetailid = new { @cid = cid,@uid=uid ,@transactiondetailid = transactiondetailid  };
            var SQL = "select pk_encode(transactiondetailid) as pkcol,transactiondetailid as value,uid as label,* from GetTable(NULL::public.transactiondetails,@cid) where transactiondetailid = @transactiondetailid";
var result = connection.Query<dynamic>(SQL, parameters_transactiondetailid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_transactiondetailid(int transactiondetailid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_transactiondetail(string sid)
{
        _logger.LogInfo("Getting into  Get_transactiondetail(string sid) api");
int id = Helper.GetId(sid);
  return  Get_transactiondetail(id);
}
        // GET: transactiondetail/5
//gets the screen record
        public  dynamic Get_transactiondetail(int id)
        {
        _logger.LogInfo("Getting into Get_transactiondetail(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.transactiondetailid) as pkcol,a.transactiondetailid as pk,a.*,
t.documentnumber as transactioniddesc,
p.email as privateiddesc,
u.email as uiddesc,
r.email as recipientuiddesc,
p.cardname || '  ' || p.cardnumber as payiddesc
 from GetTable(NULL::public.transactiondetails,@cid) a 
 left join transactionmasters t on a.transactionid=t.transactionid
 left join initiatorrecipientprivates p on a.privateid=p.privateid
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 left join customerpaymodes p on a.payid=p.payid
 where a.transactiondetailid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_transactiondetail = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'transactiondetails'";
var transactiondetail_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { transactiondetail=obj_transactiondetail,transactiondetail_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_transactiondetail(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.transactiondetailid) as pkcol,a.transactiondetailid as pk,* ,transactiondetailid as value,uid as label  from GetTable(NULL::public.transactiondetails,@cid) a ";
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
            var SQL = @"select pk_encode(a.transactiondetailid) as pkcol,a.transactiondetailid as pk,a.*,
t.documentnumber as transactioniddesc,
p.email as privateiddesc,
u.email as uiddesc,
r.email as recipientuiddesc,
p.cardname || '  ' || p.cardnumber as payiddesc from GetTable(NULL::public.transactiondetails,@cid) a 
 left join transactionmasters t on a.transactionid=t.transactionid
 left join initiatorrecipientprivates p on a.privateid=p.privateid
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 left join customerpaymodes p on a.payid=p.payid";
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
        public  dynamic Save_transactiondetail(string token,transactiondetail obj_transactiondetail)
        {
        _logger.LogInfo("Saving: Save_transactiondetail(string token,transactiondetail obj_transactiondetail) ");
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
                //transactiondetail table
                if (obj_transactiondetail.transactiondetailid == 0 || obj_transactiondetail.transactiondetailid == null || obj_transactiondetail.transactiondetailid<0)
{
if(obj_transactiondetail.status=="" || obj_transactiondetail.status==null)obj_transactiondetail.status="A";
//obj_transactiondetail.companyid=cid;
obj_transactiondetail.createdby=uid;
obj_transactiondetail.createddate=DateTime.Now;
                    _context.transactiondetails.Add((dynamic)obj_transactiondetail);
querytype=1;
}
                else
{
//obj_transactiondetail.companyid=cid;
obj_transactiondetail.updatedby=uid;
obj_transactiondetail.updateddate=DateTime.Now;
                    _context.Entry(obj_transactiondetail).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_transactiondetail).Property("createdby").IsModified = false;
                    _context.Entry(obj_transactiondetail).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api transactiondetails ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_transactiondetail,"transactiondetails", 0,obj_transactiondetail.transactiondetailid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_transactiondetail( (int)obj_transactiondetail.transactiondetailid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_transactiondetail(string token,transactiondetail obj_transactiondetail) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: transactiondetail/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
transactiondetail obj_transactiondetail = _context.transactiondetails.Find(id);
_context.transactiondetails.Remove(obj_transactiondetail);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api transactiondetails ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool transactiondetail_Exists(int id)
        {
        try{
            return _context.transactiondetails.Count(e => e.transactiondetailid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:transactiondetail_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

