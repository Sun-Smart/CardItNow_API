
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
    public class customersecurityquestionService : IcustomersecurityquestionService
    {
        private readonly IConfiguration Configuration;
        private readonly customersecurityquestionContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomersecurityquestionService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public customersecurityquestionService(customersecurityquestionContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/customersecurityquestion
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customersecurityquestions()
        {
        _logger.LogInfo("Getting into Get_customersecurityquestions() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.securityquestionid) as pkcol,securityquestionid as value,answer as label from GetTable(NULL::public.customersecurityquestions,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_customersecurityquestions(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_securityquestionid(int securityquestionid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_securityquestionid(int securityquestionid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_securityquestionid = new { @cid = cid,@uid=uid ,@securityquestionid = securityquestionid  };
            var SQL = "select pk_encode(securityquestionid) as pkcol,securityquestionid as value,answer as label,* from GetTable(NULL::public.customersecurityquestions,@cid) where securityquestionid = @securityquestionid";
var result = connection.Query<dynamic>(SQL, parameters_securityquestionid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_securityquestionid(int securityquestionid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_customersecurityquestion(string sid)
{
        _logger.LogInfo("Getting into  Get_customersecurityquestion(string sid) api");
int id = Helper.GetId(sid);
  return  Get_customersecurityquestion(id);
}
        // GET: customersecurityquestion/5
//gets the screen record
        public  dynamic Get_customersecurityquestion(int id)
        {
        _logger.LogInfo("Getting into Get_customersecurityquestion(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.securityquestionid) as pkcol,a.securityquestionid as pk,a.*,
c.UID as customeriddesc,
d.masterdatadescription as questioniddesc
 from GetTable(NULL::public.customersecurityquestions,@cid) a 
 left join customermasters c on a.customerid=c.customerid
 left join masterdatas d on a.questionid=d.masterdataid
 where a.securityquestionid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_customersecurityquestion = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customersecurityquestions'";
var customersecurityquestion_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { customersecurityquestion=obj_customersecurityquestion,customersecurityquestion_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_customersecurityquestion(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.securityquestionid) as pkcol,a.securityquestionid as pk,* ,securityquestionid as value,answer as label  from GetTable(NULL::public.customersecurityquestions,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by answer";
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
            var SQL = @"select pk_encode(a.securityquestionid) as pkcol,a.securityquestionid as pk,a.*,
c.UID as customeriddesc,
d.masterdatadescription as questioniddesc from GetTable(NULL::public.customersecurityquestions,@cid) a 
 left join customermasters c on a.customerid=c.customerid
 left join masterdatas d on a.questionid=d.masterdataid";
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
        public  dynamic Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion)
        {
        _logger.LogInfo("Saving: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) ");
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
                //customersecurityquestion table
                if (obj_customersecurityquestion.securityquestionid == 0 || obj_customersecurityquestion.securityquestionid == null || obj_customersecurityquestion.securityquestionid<0)
{
if(obj_customersecurityquestion.status=="" || obj_customersecurityquestion.status==null)obj_customersecurityquestion.status="A";
//obj_customersecurityquestion.companyid=cid;
obj_customersecurityquestion.createdby=uid;
obj_customersecurityquestion.createddate=DateTime.Now;
                    _context.customersecurityquestions.Add((dynamic)obj_customersecurityquestion);
querytype=1;
}
                else
{
//obj_customersecurityquestion.companyid=cid;
obj_customersecurityquestion.updatedby=uid;
obj_customersecurityquestion.updateddate=DateTime.Now;
                    _context.Entry(obj_customersecurityquestion).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customersecurityquestion).Property("createdby").IsModified = false;
                    _context.Entry(obj_customersecurityquestion).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api customersecurityquestions ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_customersecurityquestion,"customersecurityquestions", 0,obj_customersecurityquestion.securityquestionid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_customersecurityquestion( (int)obj_customersecurityquestion.securityquestionid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customersecurityquestion/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
customersecurityquestion obj_customersecurityquestion = _context.customersecurityquestions.Find(id);
_context.customersecurityquestions.Remove(obj_customersecurityquestion);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api customersecurityquestions ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool customersecurityquestion_Exists(int id)
        {
        try{
            return _context.customersecurityquestions.Count(e => e.securityquestionid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:customersecurityquestion_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

