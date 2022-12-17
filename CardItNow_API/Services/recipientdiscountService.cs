
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
    public class recipientdiscountService : IrecipientdiscountService
    {
        private readonly IConfiguration Configuration;
        private readonly recipientdiscountContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IrecipientdiscountService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public recipientdiscountService(recipientdiscountContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/recipientdiscount
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_recipientdiscounts()
        {
        _logger.LogInfo("Getting into Get_recipientdiscounts() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.discountid) as pkcol,discountid as value,contractnumber as label from GetTable(NULL::public.recipientdiscounts,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_recipientdiscounts(): {ex}");
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
            var SQL = "select pk_encode(discountid) as pkcol,discountid as value,contractnumber as label,* from GetTable(NULL::public.recipientdiscounts,@cid) where discountid = @discountid";
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
public  dynamic Get_recipientdiscount(string sid)
{
        _logger.LogInfo("Getting into  Get_recipientdiscount(string sid) api");
int id = Helper.GetId(sid);
  return  Get_recipientdiscount(id);
}
        // GET: recipientdiscount/5
//gets the screen record
        public  dynamic Get_recipientdiscount(int id)
        {
        _logger.LogInfo("Getting into Get_recipientdiscount(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.discountid) as pkcol,a.discountid as pk,a.*,
i.email as initiatoruiddesc,
r.email as recipientuiddesc
 from GetTable(NULL::public.recipientdiscounts,@cid) a 
 left join customermasters i on a.initiatoruid=i.uid
 left join customermasters r on a.recipientuid=r.uid
 where a.discountid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_recipientdiscount = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'recipientdiscounts'";
var recipientdiscount_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { recipientdiscount=obj_recipientdiscount,recipientdiscount_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_recipientdiscount(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.discountid) as pkcol,a.discountid as pk,* ,discountid as value,contractnumber as label  from GetTable(NULL::public.recipientdiscounts,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by contractnumber";
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
i.email as initiatoruiddesc,
r.email as recipientuiddesc from GetTable(NULL::public.recipientdiscounts,@cid) a 
 left join customermasters i on a.initiatoruid=i.uid
 left join customermasters r on a.recipientuid=r.uid";
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
        public  dynamic Save_recipientdiscount(string token,recipientdiscount obj_recipientdiscount)
        {
        _logger.LogInfo("Saving: Save_recipientdiscount(string token,recipientdiscount obj_recipientdiscount) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_recipientdiscount.contractnumber!=null )
{
var parameterscontractnumber =new {@cid=cid,@uid=uid, @contractnumber = obj_recipientdiscount.contractnumber,@discountid=obj_recipientdiscount.discountid };
                    if(Helper.Count("select count(*) from recipientdiscounts where  and contractnumber =  @contractnumber and (@discountid == 0 ||  @discountid == null ||  @discountid < 0 || discountid!=  @discountid)",parameterscontractnumber)> 0) serr +="contractnumber is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //recipientdiscount table
                if (obj_recipientdiscount.discountid == 0 || obj_recipientdiscount.discountid == null || obj_recipientdiscount.discountid<0)
{
if(obj_recipientdiscount.status=="" || obj_recipientdiscount.status==null)obj_recipientdiscount.status="A";
//obj_recipientdiscount.companyid=cid;
obj_recipientdiscount.createdby=uid;
obj_recipientdiscount.createddate=DateTime.Now;
                    _context.recipientdiscounts.Add((dynamic)obj_recipientdiscount);
querytype=1;
}
                else
{
//obj_recipientdiscount.companyid=cid;
obj_recipientdiscount.updatedby=uid;
obj_recipientdiscount.updateddate=DateTime.Now;
                    _context.Entry(obj_recipientdiscount).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_recipientdiscount).Property("createdby").IsModified = false;
                    _context.Entry(obj_recipientdiscount).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api recipientdiscounts ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_recipientdiscount,"recipientdiscounts", 0,obj_recipientdiscount.discountid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_recipientdiscount( (int)obj_recipientdiscount.discountid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_recipientdiscount(string token,recipientdiscount obj_recipientdiscount) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: recipientdiscount/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
recipientdiscount obj_recipientdiscount = _context.recipientdiscounts.Find(id);
_context.recipientdiscounts.Remove(obj_recipientdiscount);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api recipientdiscounts ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool recipientdiscount_Exists(int id)
        {
        try{
            return _context.recipientdiscounts.Count(e => e.discountid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:recipientdiscount_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

