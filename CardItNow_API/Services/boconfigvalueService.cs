
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
////////using FluentDateTime;
////////using FluentDate;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;
using Npgsql;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections;
using System.Text;
using LoggerService;
using nTireBO.Services;

namespace nTireBO.Services
{
    public class boconfigvalueService : IboconfigvalueService
    {
        private readonly boconfigvalueContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IboconfigvalueService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public boconfigvalueService(boconfigvalueContext context, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
        {
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
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email").Value.ToString();
            }
        }

        // GET: service/boconfigvalue
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_boconfigvalues()
        {
        _logger.LogInfo("Getting into Get_boconfigvalues() api");
        try{
        using (var connection = new NpgsqlConnection(Helper.Connectionstring))
        {
            
        var parameters = new { @cid = cid };
    string SQL = "select pk_encode(a.configid) as pkcol,configkey as value,configtext as label from boconfigvalues a  WHERE  a.status='A' ORDER BY orderno";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_boconfigvalues(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_configid(int configid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_configid(int configid) api");
        using (var connection = new NpgsqlConnection(Helper.Connectionstring))
        {
        var parameters_configid = new { @cid = cid ,@configid = configid  };
            var SQL = "select pk_encode(configid) as pkcol,configkey as value,configtext as label,* from boconfigvalues where configid = @configid";
var result = connection.Query<dynamic>(SQL, parameters_configid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_configid(int configid) \r\n {ex}");
            throw ex;
        }
        }
        public  IEnumerable<Object> GetListBy_param(string param)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_param(string param) api");
        using (var connection = new NpgsqlConnection(Helper.Connectionstring))
        {
        var parameters_param = new { @cid = cid ,@param = param  };
            var SQL = "select pk_encode(configid) as pkcol,configkey as value,configtext as label,* from boconfigvalues where param = @param";
var result = connection.Query<dynamic>(SQL, parameters_param);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_param(string param) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_boconfigvalue(string sid)
{
        _logger.LogInfo("Getting into  Get_boconfigvalue(string sid) api");
int id = Helper.GetId(sid);
  return  Get_boconfigvalue(id);
}
        // GET: boconfigvalue/5
//gets the screen record
        public  dynamic Get_boconfigvalue(int id)
        {
        _logger.LogInfo("Getting into Get_boconfigvalue(int id) api");
try{
        using (var connection = new NpgsqlConnection(Helper.Connectionstring))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.configid) as pkcol,a.configid as pk,a.*
 from boconfigvalues a 
 where a.configid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_boconfigvalue = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'boconfigvalues'";
var boconfigvalue_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { boconfigvalue=obj_boconfigvalue,boconfigvalue_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_boconfigvalue(int id)\r\n {ex}");
throw ex;
}
        }

        public  IEnumerable<Object> GetList(string key)
        {
        try{
        _logger.LogInfo("Getting into  GetList(string key) api");

        using (var connection = new NpgsqlConnection(Helper.Connectionstring))
        {
        var parameters = new { @cid = cid,@key=key  };
            var SQL = @"select  pk_encode(a.configid) as pkcol,a.configid as pk,* ,configkey as value,configtext as label  from boconfigvalues a  where  coalesce(param,'') = @key ";
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
        using (var connection = new NpgsqlConnection(Helper.Connectionstring))
        {
string wStatus = "NormalStatus";
var parameters = new { @cid=cid,@id=id,@wStatus=wStatus};
            var SQL = @"select pk_encode(a.configid) as pkcol,a.configid as pk,a.* from boconfigvalues a ";
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
        public  dynamic Save_boconfigvalue(string token,boconfigvalue obj_boconfigvalue)
        {
        _logger.LogInfo("Saving: Save_boconfigvalue(string token,boconfigvalue obj_boconfigvalue) ");
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
                //boconfigvalue table
                if (obj_boconfigvalue.configid == 0 || obj_boconfigvalue.configid == null || obj_boconfigvalue.configid<0)
{
if(obj_boconfigvalue.status=="" || obj_boconfigvalue.status==null)obj_boconfigvalue.status="A";
obj_boconfigvalue.createdby=uid;
obj_boconfigvalue.createddate=DateTime.Now;
                    _context.boconfigvalues.Add((dynamic)obj_boconfigvalue);
querytype=1;
}
                else
{
obj_boconfigvalue.updatedby=uid;
obj_boconfigvalue.updateddate=DateTime.Now;
                    _context.Entry(obj_boconfigvalue).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_boconfigvalue).Property("createdby").IsModified = false;
                    _context.Entry(obj_boconfigvalue).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api boconfigvalues ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_boconfigvalue,"boconfigvalues", 0,obj_boconfigvalue.configid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_boconfigvalue( (int)obj_boconfigvalue.configid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_boconfigvalue(string token,boconfigvalue obj_boconfigvalue) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: boconfigvalue/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
boconfigvalue obj_boconfigvalue = _context.boconfigvalues.Find(id);
_context.boconfigvalues.Remove(obj_boconfigvalue);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api boconfigvalues ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool boconfigvalue_Exists(int id)
        {
        try{
            return _context.boconfigvalues.Count(e => e.configid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:boconfigvalue_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

