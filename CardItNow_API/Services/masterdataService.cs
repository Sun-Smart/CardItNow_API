
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
    public class masterdataService : ImasterdataService
    {
        private readonly IConfiguration Configuration;
        private readonly masterdataContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ImasterdataService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public masterdataService(masterdataContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/masterdata
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_masterdatas()
        {
        _logger.LogInfo("Getting into Get_masterdatas() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.masterdataid) as pkcol,masterdataid as value,masterdatadescription as label from GetTable(NULL::public.masterdatas,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_masterdatas(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_masterdataid(int masterdataid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_masterdataid(int masterdataid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_masterdataid = new { @cid = cid,@uid=uid ,@masterdataid = masterdataid  };
            var SQL = "select pk_encode(masterdataid) as pkcol,masterdataid as value,masterdatadescription as label,* from GetTable(NULL::public.masterdatas,@cid) where masterdataid = @masterdataid";
var result = connection.Query<dynamic>(SQL, parameters_masterdataid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_masterdataid(int masterdataid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_masterdata(string sid)
{
        _logger.LogInfo("Getting into  Get_masterdata(string sid) api");
int id = Helper.GetId(sid);
  return  Get_masterdata(id);
}
        // GET: masterdata/5
//gets the screen record
        public  dynamic Get_masterdata(int id)
        {
        _logger.LogInfo("Getting into Get_masterdata(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.masterdataid) as pkcol,a.masterdataid as pk,a.*,
d.masterdatatypename as masterdatatypeiddesc
 from GetTable(NULL::public.masterdatas,@cid) a 
 left join masterdatatypes d on a.masterdatatypeid=d.datatypeid
 where a.masterdataid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_masterdata = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'masterdatas'";
var masterdata_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { masterdata=obj_masterdata,masterdata_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_masterdata(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.masterdataid) as pkcol,a.masterdataid as pk,* ,masterdataid as value,masterdatadescription as label  from GetTable(NULL::public.masterdatas,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by masterdatadescription";
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
            var SQL = @"select pk_encode(a.masterdataid) as pkcol,a.masterdataid as pk,a.*,
d.masterdatatypename as masterdatatypeiddesc from GetTable(NULL::public.masterdatas,@cid) a 
 left join masterdatatypes d on a.masterdatatypeid=d.datatypeid";
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
        public  dynamic Save_masterdata(string token,masterdata obj_masterdata)
        {
        _logger.LogInfo("Saving: Save_masterdata(string token,masterdata obj_masterdata) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_masterdata.masterdatadescription!=null )
{
var parametersmasterdatadescription =new {@cid=cid,@uid=uid, @masterdatadescription = obj_masterdata.masterdatadescription,@masterdataid=obj_masterdata.masterdataid };
                    if(Helper.Count("select count(*) from masterdatas where  and masterdatadescription =  @masterdatadescription and (@masterdataid == 0 ||  @masterdataid == null ||  @masterdataid < 0 || masterdataid!=  @masterdataid)",parametersmasterdatadescription)> 0) serr +="masterdatadescription is unique\r\n";
}
                if(serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //masterdata table
                if (obj_masterdata.masterdataid == 0 || obj_masterdata.masterdataid == null || obj_masterdata.masterdataid<0)
{
if(obj_masterdata.status=="" || obj_masterdata.status==null)obj_masterdata.status="A";
//obj_masterdata.companyid=cid;
obj_masterdata.createdby=uid;
obj_masterdata.createddate=DateTime.Now;
                    _context.masterdatas.Add((dynamic)obj_masterdata);
querytype=1;
}
                else
{
//obj_masterdata.companyid=cid;
obj_masterdata.updatedby=uid;
obj_masterdata.updateddate=DateTime.Now;
                    _context.Entry(obj_masterdata).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_masterdata).Property("createdby").IsModified = false;
                    _context.Entry(obj_masterdata).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api masterdatas ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_masterdata,"masterdatas", 0,obj_masterdata.masterdataid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_masterdata( (int)obj_masterdata.masterdataid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_masterdata(string token,masterdata obj_masterdata) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: masterdata/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
masterdata obj_masterdata = _context.masterdatas.Find(id);
_context.masterdatas.Remove(obj_masterdata);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api masterdatas ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool masterdata_Exists(int id)
        {
        try{
            return _context.masterdatas.Count(e => e.masterdataid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:masterdata_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

