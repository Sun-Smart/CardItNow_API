
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
    public class transactionmasterService : ItransactionmasterService
    {
        private readonly IConfiguration Configuration;
        private readonly transactionmasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ItransactionmasterService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public transactionmasterService(transactionmasterContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/transactionmaster
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_transactionmasters()
        {
        _logger.LogInfo("Getting into Get_transactionmasters() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.transactionid) as pkcol,transactionid as value,uid as label from GetTable(NULL::public.transactionmasters,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_transactionmasters(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_transactionid(int transactionid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_transactionid(int transactionid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_transactionid = new { @cid = cid,@uid=uid ,@transactionid = transactionid  };
            var SQL = "select pk_encode(transactionid) as pkcol,transactionid as value,uid as label,* from GetTable(NULL::public.transactionmasters,@cid) where transactionid = @transactionid";
var result = connection.Query<dynamic>(SQL, parameters_transactionid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_transactionid(int transactionid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_transactionmaster(string sid)
{
        _logger.LogInfo("Getting into  Get_transactionmaster(string sid) api");
int id = Helper.GetId(sid);
  return  Get_transactionmaster(id);
}
        // GET: transactionmaster/5
//gets the screen record
        public  dynamic Get_transactionmaster(int id)
        {
        _logger.LogInfo("Getting into Get_transactionmaster(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vtransactiontype ="transactiontype";
string vpaytype ="paytype";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vtransactiontype =vtransactiontype,@vpaytype =vpaytype};
var SQL = @"select pk_encode(a.transactionid) as pkcol,a.transactionid as pk,a.*,
u.email as uiddesc,
r.email as recipientuiddesc,

                              y.configtext as transactiontypedesc,
                              pt.configtext as paytypedesc,
d.cardname || '  ' || d.cardnumber as payiddesc
 from GetTable(NULL::public.transactionmasters,@cid) a 
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 
 left join boconfigvalues y on a.transactiontype=y.configkey and @vtransactiontype=y.param
 left join boconfigvalues pt on a.paytype=pt.configkey and @vpaytype=pt.param
 left join customerpaymodes d on a.payid=d.payid
 where a.transactionid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_transactionmaster = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'transactionmasters'";
var transactionmaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { transactionmaster=obj_transactionmaster,transactionmaster_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_transactionmaster(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.transactionid) as pkcol,a.transactionid as pk,* ,transactionid as value,uid as label  from GetTable(NULL::public.transactionmasters,@cid) a ";
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
string vtransactiontype ="transactiontype";
string vpaytype ="paytype";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vtransactiontype =vtransactiontype,@vpaytype =vpaytype};
            var SQL = @"select pk_encode(a.transactionid) as pkcol,a.transactionid as pk,a.*,
u.email as uiddesc,
r.email as recipientuiddesc,

                              y.configtext as transactiontypedesc,
                              pt.configtext as paytypedesc,
d.cardname || '  ' || d.cardnumber as payiddesc from GetTable(NULL::public.transactionmasters,@cid) a 
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 
 left join boconfigvalues y on a.transactiontype=y.configkey and @vtransactiontype=y.param
 left join boconfigvalues pt on a.paytype=pt.configkey and @vpaytype=pt.param
 left join customerpaymodes d on a.payid=d.payid";
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
        public  dynamic Save_transactionmaster(string token,transactionmaster obj_transactionmaster)
        {
        _logger.LogInfo("Saving: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_transactionmaster.documentnumber!=null )
{
var parametersdocumentnumber =new {@cid=cid,@uid=uid, @documentnumber = obj_transactionmaster.documentnumber,@transactionid=obj_transactionmaster.transactionid };
                    if(Helper.Count("select count(*) from transactionmasters where  and documentnumber =  @documentnumber and (@transactionid == 0 ||  @transactionid == null ||  @transactionid < 0 || transactionid!=  @transactionid)",parametersdocumentnumber)> 0) serr +="documentnumber is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //transactionmaster table
                if (obj_transactionmaster.transactionid == 0 || obj_transactionmaster.transactionid == null || obj_transactionmaster.transactionid<0)
{
if(obj_transactionmaster.status=="" || obj_transactionmaster.status==null)obj_transactionmaster.status="A";
//obj_transactionmaster.companyid=cid;
obj_transactionmaster.createdby=uid;
obj_transactionmaster.createddate=DateTime.Now;
                    _context.transactionmasters.Add((dynamic)obj_transactionmaster);
querytype=1;
}
                else
{
//obj_transactionmaster.companyid=cid;
obj_transactionmaster.updatedby=uid;
obj_transactionmaster.updateddate=DateTime.Now;
                    _context.Entry(obj_transactionmaster).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_transactionmaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_transactionmaster).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api transactionmasters ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_transactionmaster,"transactionmasters", 0,obj_transactionmaster.transactionid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_transactionmaster( (int)obj_transactionmaster.transactionid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: transactionmaster/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
transactionmaster obj_transactionmaster = _context.transactionmasters.Find(id);
_context.transactionmasters.Remove(obj_transactionmaster);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api transactionmasters ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool transactionmaster_Exists(int id)
        {
        try{
            return _context.transactionmasters.Count(e => e.transactionid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:transactionmaster_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

