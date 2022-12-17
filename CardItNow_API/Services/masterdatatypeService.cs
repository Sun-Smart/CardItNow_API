
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
    public class masterdatatypeService : ImasterdatatypeService
    {
        private readonly IConfiguration Configuration;
        private readonly masterdatatypeContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ImasterdatatypeService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public masterdatatypeService(masterdatatypeContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/masterdatatype
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_masterdatatypes()
        {
        _logger.LogInfo("Getting into Get_masterdatatypes() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.datatypeid) as pkcol,datatypeid as value,masterdatatypename as label from GetTable(NULL::public.masterdatatypes,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_masterdatatypes(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_datatypeid(int datatypeid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_datatypeid(int datatypeid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_datatypeid = new { @cid = cid,@uid=uid ,@datatypeid = datatypeid  };
            var SQL = "select pk_encode(datatypeid) as pkcol,datatypeid as value,masterdatatypename as label,* from GetTable(NULL::public.masterdatatypes,@cid) where datatypeid = @datatypeid";
var result = connection.Query<dynamic>(SQL, parameters_datatypeid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_datatypeid(int datatypeid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_masterdatatype(string sid)
{
        _logger.LogInfo("Getting into  Get_masterdatatype(string sid) api");
int id = Helper.GetId(sid);
  return  Get_masterdatatype(id);
}
        // GET: masterdatatype/5
//gets the screen record
        public  dynamic Get_masterdatatype(int id)
        {
        _logger.LogInfo("Getting into Get_masterdatatype(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.datatypeid) as pkcol,a.datatypeid as pk,a.*
 from GetTable(NULL::public.masterdatatypes,@cid) a 
 where a.datatypeid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_masterdatatype = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'masterdatatypes'";
var masterdatatype_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { masterdatatype=obj_masterdatatype,masterdatatype_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_masterdatatype(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.datatypeid) as pkcol,a.datatypeid as pk,* ,datatypeid as value,masterdatatypename as label  from GetTable(NULL::public.masterdatatypes,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by masterdatatypename";
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
            var SQL = @"select pk_encode(a.datatypeid) as pkcol,a.datatypeid as pk,a.* from GetTable(NULL::public.masterdatatypes,@cid) a ";
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
        public  dynamic Save_masterdatatype(string token,masterdatatype obj_masterdatatype)
        {
        _logger.LogInfo("Saving: Save_masterdatatype(string token,masterdatatype obj_masterdatatype) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_masterdatatype.masterdatatypename!=null )
{
var parametersmasterdatatypename =new {@cid=cid,@uid=uid, @masterdatatypename = obj_masterdatatype.masterdatatypename,@datatypeid=obj_masterdatatype.datatypeid };
                    if(Helper.Count("select count(*) from masterdatatypes where  and masterdatatypename =  @masterdatatypename and (@datatypeid == 0 ||  @datatypeid == null ||  @datatypeid < 0 || datatypeid!=  @datatypeid)",parametersmasterdatatypename)> 0) serr +="masterdatatypename is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //masterdatatype table
                if (obj_masterdatatype.datatypeid == 0 || obj_masterdatatype.datatypeid == null || obj_masterdatatype.datatypeid<0)
{
if(obj_masterdatatype.status=="" || obj_masterdatatype.status==null)obj_masterdatatype.status="A";
//obj_masterdatatype.companyid=cid;
obj_masterdatatype.createdby=uid;
obj_masterdatatype.createddate=DateTime.Now;
                    _context.masterdatatypes.Add((dynamic)obj_masterdatatype);
querytype=1;
}
                else
{
//obj_masterdatatype.companyid=cid;
obj_masterdatatype.updatedby=uid;
obj_masterdatatype.updateddate=DateTime.Now;
                    _context.Entry(obj_masterdatatype).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_masterdatatype).Property("createdby").IsModified = false;
                    _context.Entry(obj_masterdatatype).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api masterdatatypes ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_masterdatatype,"masterdatatypes", 0,obj_masterdatatype.datatypeid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_masterdatatype( (int)obj_masterdatatype.datatypeid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_masterdatatype(string token,masterdatatype obj_masterdatatype) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: masterdatatype/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
masterdatatype obj_masterdatatype = _context.masterdatatypes.Find(id);
_context.masterdatatypes.Remove(obj_masterdatatype);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api masterdatatypes ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool masterdatatype_Exists(int id)
        {
        try{
            return _context.masterdatatypes.Count(e => e.datatypeid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:masterdatatype_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

