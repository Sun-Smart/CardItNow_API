
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
using carditnow.Models;

namespace nTireBO.Services
{
    public class boreportService : IboreportService
    {
        private readonly IConfiguration Configuration;
        private readonly boreportContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IboreportService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public boreportService(boreportContext context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
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

        // GET: service/boreport
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_boreports()
        {
        _logger.LogInfo("Getting into Get_boreports() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.reportid) as pkcol,reportid as value,reportname as label from GetTable(NULL::public.boreports,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_boreports(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_reportid(int reportid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_reportid(int reportid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_reportid = new { @cid = cid,@uid=uid ,@reportid = reportid  };
            var SQL = "select pk_encode(reportid) as pkcol,reportid as value,reportname as label,* from GetTable(NULL::public.boreports,@cid) where reportid = @reportid";
var result = connection.Query<dynamic>(SQL, parameters_reportid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_reportid(int reportid) \r\n {ex}");
            throw ex;
        }
        }
        public  IEnumerable<Object> GetListBy_reportcode(string reportcode)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_reportcode(string reportcode) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_reportcode = new { @cid = cid,@uid=uid ,@reportcode = reportcode  };
            var SQL = "select pk_encode(reportid) as pkcol,reportid as value,reportname as label,* from GetTable(NULL::public.boreports,@cid) where reportcode = @reportcode";
var result = connection.Query<dynamic>(SQL, parameters_reportcode);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_reportcode(string reportcode) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_boreport(string sid)
{
        _logger.LogInfo("Getting into  Get_boreport(string sid) api");
int id = Helper.GetId(sid);
  return  Get_boreport(id);
}
        // GET: boreport/5
//gets the screen record
        public  dynamic Get_boreport(int id)
        {
        _logger.LogInfo("Getting into Get_boreport(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vschedule ="schedule";
string vgroupbyrelationship ="groupbyrelationship";
string vdatefiltertype ="datefiltertype";
string vjointype ="jointype";
string vrecordtype ="recordtype";
string vreportoutputtype ="reportoutputtype";
string vreporttype ="reporttype";
string vreportmodule ="reportmodule";
string vsidefiltertype ="sidefiltertype";
string vviewhtmltype ="viewhtmltype";
string vworkflowhtmltype ="workflowhtmltype";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vschedule =vschedule,@vgroupbyrelationship =vgroupbyrelationship,@vdatefiltertype =vdatefiltertype,@vjointype =vjointype,@vrecordtype =vrecordtype,@vreportoutputtype =vreportoutputtype,@vreporttype =vreporttype,@vreportmodule =vreportmodule,@vsidefiltertype =vsidefiltertype,@vviewhtmltype =vviewhtmltype,@vworkflowhtmltype =vworkflowhtmltype};
var SQL = @"select pk_encode(a.reportid) as pkcol,a.reportid as pk,a.*,
                              sh.configtext as scheduledesc,
db.dashboardname as dashboardiddesc,
                              yp.configtext as groupbyrelationshipdesc,
                              d.configtext as datefiltertypedesc,
                              j.configtext as jointypedesc,
                              r.configtext as recordtypedesc,
                              o.configtext as reportoutputtypedesc,
                              t.configtext as reporttypedesc,
                              rt.configtext as reportmoduledesc,
                              ps.configtext as sidefiltertypedesc,
                              oh.configtext as viewhtmltypedesc,
                              po.configtext as workflowhtmltypedesc
 from GetTable(NULL::public.boreports,@cid) a 
 left join boconfigvalues sh on a.schedule=sh.configkey and @vschedule=sh.param
 left join bodashboards db on a.dashboardid=db.dashboardid
 left join boconfigvalues yp on a.groupbyrelationship=yp.configkey and @vgroupbyrelationship=yp.param
 left join boconfigvalues d on a.datefiltertype=d.configkey and @vdatefiltertype=d.param
 left join boconfigvalues j on a.jointype=j.configkey and @vjointype=j.param
 left join boconfigvalues r on a.recordtype=r.configkey and @vrecordtype=r.param
 left join boconfigvalues o on a.reportoutputtype=o.configkey and @vreportoutputtype=o.param
 left join boconfigvalues t on a.reporttype=t.configkey and @vreporttype=t.param
 left join boconfigvalues rt on a.reportmodule=rt.configkey and @vreportmodule=rt.param
 left join boconfigvalues ps on a.sidefiltertype=ps.configkey and @vsidefiltertype=ps.param
 left join boconfigvalues oh on a.viewhtmltype=oh.configkey and @vviewhtmltype=oh.param
 left join boconfigvalues po on a.workflowhtmltype=po.configkey and @vworkflowhtmltype=po.param
 where a.reportid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_boreport = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'boreports'";
var boreport_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { boreport=obj_boreport,boreport_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_boreport(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.reportid) as pkcol,a.reportid as pk,* ,reportid as value,reportname as label  from GetTable(NULL::public.boreports,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by reportname";
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
string vschedule ="schedule";
string vgroupbyrelationship ="groupbyrelationship";
string vdatefiltertype ="datefiltertype";
string vjointype ="jointype";
string vrecordtype ="recordtype";
string vreportoutputtype ="reportoutputtype";
string vreporttype ="reporttype";
string vreportmodule ="reportmodule";
string vsidefiltertype ="sidefiltertype";
string vviewhtmltype ="viewhtmltype";
string vworkflowhtmltype ="workflowhtmltype";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vschedule =vschedule,@vgroupbyrelationship =vgroupbyrelationship,@vdatefiltertype =vdatefiltertype,@vjointype =vjointype,@vrecordtype =vrecordtype,@vreportoutputtype =vreportoutputtype,@vreporttype =vreporttype,@vreportmodule =vreportmodule,@vsidefiltertype =vsidefiltertype,@vviewhtmltype =vviewhtmltype,@vworkflowhtmltype =vworkflowhtmltype};
            var SQL = @"select pk_encode(a.reportid) as pkcol,a.reportid as pk,a.*,
                              sh.configtext as scheduledesc,
db.dashboardname as dashboardiddesc,
                              yp.configtext as groupbyrelationshipdesc,
                              d.configtext as datefiltertypedesc,
                              j.configtext as jointypedesc,
                              r.configtext as recordtypedesc,
                              o.configtext as reportoutputtypedesc,
                              t.configtext as reporttypedesc,
                              rt.configtext as reportmoduledesc,
                              ps.configtext as sidefiltertypedesc,
                              oh.configtext as viewhtmltypedesc,
                              po.configtext as workflowhtmltypedesc from GetTable(NULL::public.boreports,@cid) a 
 left join boconfigvalues sh on a.schedule=sh.configkey and @vschedule=sh.param
 left join bodashboards db on a.dashboardid=db.dashboardid
 left join boconfigvalues yp on a.groupbyrelationship=yp.configkey and @vgroupbyrelationship=yp.param
 left join boconfigvalues d on a.datefiltertype=d.configkey and @vdatefiltertype=d.param
 left join boconfigvalues j on a.jointype=j.configkey and @vjointype=j.param
 left join boconfigvalues r on a.recordtype=r.configkey and @vrecordtype=r.param
 left join boconfigvalues o on a.reportoutputtype=o.configkey and @vreportoutputtype=o.param
 left join boconfigvalues t on a.reporttype=t.configkey and @vreporttype=t.param
 left join boconfigvalues rt on a.reportmodule=rt.configkey and @vreportmodule=rt.param
 left join boconfigvalues ps on a.sidefiltertype=ps.configkey and @vsidefiltertype=ps.param
 left join boconfigvalues oh on a.viewhtmltype=oh.configkey and @vviewhtmltype=oh.param
 left join boconfigvalues po on a.workflowhtmltype=po.configkey and @vworkflowhtmltype=po.param";
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
        public  dynamic Save_boreport(string token,boreport obj_boreport)
        {
        _logger.LogInfo("Saving: Save_boreport(string token,boreport obj_boreport) ");
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
                //boreport table
                if (obj_boreport.reportid == 0 || obj_boreport.reportid == null || obj_boreport.reportid<0)
{
if(obj_boreport.status=="" || obj_boreport.status==null)obj_boreport.status="A";
obj_boreport.createdby=uid;
obj_boreport.createddate=DateTime.Now;
                    _context.boreports.Add((dynamic)obj_boreport);
querytype=1;
}
                else
{
obj_boreport.updatedby=uid;
obj_boreport.updateddate=DateTime.Now;
                    _context.Entry(obj_boreport).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_boreport).Property("createdby").IsModified = false;
                    _context.Entry(obj_boreport).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api boreports ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_boreport,"boreports", 0,obj_boreport.reportid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_boreport( (int)obj_boreport.reportid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_boreport(string token,boreport obj_boreport) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: boreport/5
//delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
boreport obj_boreport = _context.boreports.Find(id);
_context.boreports.Remove(obj_boreport);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api boreports ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool boreport_Exists(int id)
        {
        try{
            return _context.boreports.Count(e => e.reportid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:boreport_Exists(int id) {ex}");
            return false;
        }
        }

    }
}

