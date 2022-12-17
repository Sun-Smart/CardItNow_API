
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nTireBO;
using nTireBO.Models;
using SunSmartnTireProducts.Helpers;
//////using FluentDateTime;
//////using FluentDate;
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

namespace nTireBO.Services
{
    public class boreportdetailService : IboreportdetailService
    {
        private readonly IConfiguration Configuration;
        private readonly boreportdetailContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IboreportdetailService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public boreportdetailService(boreportdetailContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/boreportdetail
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_boreportdetails()
        {
        _logger.LogInfo("Getting into Get_boreportdetails() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.reportdetailid) as pkcol,reportdetailid as value,tablename as label from GetTable(NULL::public.boreportdetails,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_boreportdetails(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_reportdetailid(int reportdetailid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_reportdetailid(int reportdetailid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_reportdetailid = new { @cid = cid,@uid=uid ,@reportdetailid = reportdetailid  };
            var SQL = "select pk_encode(reportdetailid) as pkcol,reportdetailid as value,tablename as label,* from GetTable(NULL::public.boreportdetails,@cid) where reportdetailid = @reportdetailid";
var result = connection.Query<dynamic>(SQL, parameters_reportdetailid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_reportdetailid(int reportdetailid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_boreportdetail(string sid)
{
        _logger.LogInfo("Getting into  Get_boreportdetail(string sid) api");
int id = Helper.GetId(sid);
  return  Get_boreportdetail(id);
}
        // GET: boreportdetail/5
//gets the screen record
        public  dynamic Get_boreportdetail(int id)
        {
        _logger.LogInfo("Getting into Get_boreportdetail(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vseparator ="separator";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vseparator =vseparator};
var SQL = @"select pk_encode(a.reportdetailid) as pkcol,a.reportdetailid as pk,a.*,
                              j.configtext as separatordesc
 from GetTable(NULL::public.boreportdetails,@cid) a 
 left join boconfigvalues j on a.separator=j.configkey and @vseparator=j.param
 where a.reportdetailid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_boreportdetail = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'boreportdetails'";
var boreportdetail_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { boreportdetail=obj_boreportdetail,boreportdetail_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_boreportdetail(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.reportdetailid) as pkcol,a.reportdetailid as pk,* ,reportdetailid as value,tablename as label  from GetTable(NULL::public.boreportdetails,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by tablename";
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
string vseparator ="separator";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vseparator =vseparator};
            var SQL = @"select pk_encode(a.reportdetailid) as pkcol,a.reportdetailid as pk,a.*,
                              j.configtext as separatordesc from GetTable(NULL::public.boreportdetails,@cid) a 
 left join boconfigvalues j on a.separator=j.configkey and @vseparator=j.param";
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
        public  dynamic Save_boreportdetail(string token,boreportdetail obj_boreportdetail)
        {
        _logger.LogInfo("Saving: Save_boreportdetail(string token,boreportdetail obj_boreportdetail) ");
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
                //boreportdetail table
                if (obj_boreportdetail.reportdetailid == 0 || obj_boreportdetail.reportdetailid == null || obj_boreportdetail.reportdetailid<0)
{
if(obj_boreportdetail.status=="" || obj_boreportdetail.status==null)obj_boreportdetail.status="A";
obj_boreportdetail.createdby=uid;
obj_boreportdetail.createddate=DateTime.Now;
                    _context.boreportdetails.Add((dynamic)obj_boreportdetail);
querytype=1;
}
                else
{
obj_boreportdetail.updatedby=uid;
obj_boreportdetail.updateddate=DateTime.Now;
                    _context.Entry(obj_boreportdetail).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_boreportdetail).Property("createdby").IsModified = false;
                    _context.Entry(obj_boreportdetail).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api boreportdetails ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_boreportdetail,"boreportdetails", 0,obj_boreportdetail.reportdetailid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_boreportdetail( (int)obj_boreportdetail.reportdetailid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_boreportdetail(string token,boreportdetail obj_boreportdetail) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: boreportdetail/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
boreportdetail obj_boreportdetail = _context.boreportdetails.Find(id);
_context.boreportdetails.Remove(obj_boreportdetail);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api boreportdetails ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool boreportdetail_Exists(int id)
        {
        try{
            return _context.boreportdetails.Count(e => e.reportdetailid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:boreportdetail_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

