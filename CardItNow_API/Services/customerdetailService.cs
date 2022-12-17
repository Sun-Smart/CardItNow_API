
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
    public class customerdetailService : IcustomerdetailService
    {
        private readonly IConfiguration Configuration;
        private readonly customerdetailContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomerdetailService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public customerdetailService(customerdetailContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/customerdetail
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customerdetails()
        {
        _logger.LogInfo("Getting into Get_customerdetails() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.customerdetailid) as pkcol,customerdetailid as value,uid as label from GetTable(NULL::public.customerdetails,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_customerdetails(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_customerdetailid(int customerdetailid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_customerdetailid(int customerdetailid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_customerdetailid = new { @cid = cid,@uid=uid ,@customerdetailid = customerdetailid  };
            var SQL = "select pk_encode(customerdetailid) as pkcol,customerdetailid as value,uid as label,* from GetTable(NULL::public.customerdetails,@cid) where customerdetailid = @customerdetailid";
var result = connection.Query<dynamic>(SQL, parameters_customerdetailid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_customerdetailid(int customerdetailid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_customerdetail(string sid)
{
        _logger.LogInfo("Getting into  Get_customerdetail(string sid) api");
int id = Helper.GetId(sid);
  return  Get_customerdetail(id);
}
        // GET: customerdetail/5
//gets the screen record
        public  dynamic Get_customerdetail(int id)
        {
        _logger.LogInfo("Getting into Get_customerdetail(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vdivmode ="divmode";
string vdivstatus ="divstatus";
string vamlcheckstatus ="amlcheckstatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vdivmode =vdivmode,@vdivstatus =vdivstatus,@vamlcheckstatus =vamlcheckstatus};
var SQL = @"select pk_encode(a.customerdetailid) as pkcol,a.customerdetailid as pk,a.*,
u.email as customeriddesc,
i.email as uiddesc,
g.geoname as geoiddesc,
s.cityname as cityiddesc,
                              o.configtext as divmodedesc,
                              v.configtext as divstatusdesc,
                              l.configtext as amlcheckstatusdesc
 from GetTable(NULL::public.customerdetails,@cid) a 
 left join customermasters u on a.customerid=u. customerid
 left join customermasters i on a.uid=i.uid
 left join geographymasters g on a.geoid=g.geoid
 left join citymasters s on a.cityid=s.cityid
 left join boconfigvalues o on a.divmode=o.configkey and @vdivmode=o.param
 left join boconfigvalues v on a.divstatus=v.configkey and @vdivstatus=v.param
 left join boconfigvalues l on a.amlcheckstatus=l.configkey and @vamlcheckstatus=l.param
 where a.customerdetailid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_customerdetail = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerdetails'";
var customerdetail_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { customerdetail=obj_customerdetail,customerdetail_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_customerdetail(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.customerdetailid) as pkcol,a.customerdetailid as pk,* ,customerdetailid as value,uid as label  from GetTable(NULL::public.customerdetails,@cid) a ";
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
string vdivmode ="divmode";
string vdivstatus ="divstatus";
string vamlcheckstatus ="amlcheckstatus";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vdivmode =vdivmode,@vdivstatus =vdivstatus,@vamlcheckstatus =vamlcheckstatus};
            var SQL = @"select pk_encode(a.customerdetailid) as pkcol,a.customerdetailid as pk,a.*,
u.email as customeriddesc,
i.email as uiddesc,
g.geoname as geoiddesc,
s.cityname as cityiddesc,
                              o.configtext as divmodedesc,
                              v.configtext as divstatusdesc,
                              l.configtext as amlcheckstatusdesc from GetTable(NULL::public.customerdetails,@cid) a 
 left join customermasters u on a.customerid=u. customerid
 left join customermasters i on a.uid=i.uid
 left join geographymasters g on a.geoid=g.geoid
 left join citymasters s on a.cityid=s.cityid
 left join boconfigvalues o on a.divmode=o.configkey and @vdivmode=o.param
 left join boconfigvalues v on a.divstatus=v.configkey and @vdivstatus=v.param
 left join boconfigvalues l on a.amlcheckstatus=l.configkey and @vamlcheckstatus=l.param";
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
        public  dynamic Save_customerdetail(string token,customerdetail obj_customerdetail)
        {
        _logger.LogInfo("Saving: Save_customerdetail(string token,customerdetail obj_customerdetail) ");
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
                //customerdetail table
                if (obj_customerdetail.customerdetailid == 0 || obj_customerdetail.customerdetailid == null || obj_customerdetail.customerdetailid<0)
{
if(obj_customerdetail.status=="" || obj_customerdetail.status==null)obj_customerdetail.status="A";
//obj_customerdetail.companyid=cid;
obj_customerdetail.createdby=uid;
obj_customerdetail.createddate=DateTime.Now;
                    _context.customerdetails.Add((dynamic)obj_customerdetail);
querytype=1;
}
                else
{
//obj_customerdetail.companyid=cid;
obj_customerdetail.updatedby=uid;
obj_customerdetail.updateddate=DateTime.Now;
                    _context.Entry(obj_customerdetail).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customerdetail).Property("createdby").IsModified = false;
                    _context.Entry(obj_customerdetail).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api customerdetails ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_customerdetail,"customerdetails", 0,obj_customerdetail.customerdetailid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_customerdetail( (int)obj_customerdetail.customerdetailid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_customerdetail(string token,customerdetail obj_customerdetail) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customerdetail/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
customerdetail obj_customerdetail = _context.customerdetails.Find(id);
_context.customerdetails.Remove(obj_customerdetail);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api customerdetails ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool customerdetail_Exists(int id)
        {
        try{
            return _context.customerdetails.Count(e => e.customerdetailid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:customerdetail_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

