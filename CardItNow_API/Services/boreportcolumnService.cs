
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
    public class boreportcolumnService : IboreportcolumnService
    {
        private readonly IConfiguration Configuration;
        private readonly boreportcolumnContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IboreportcolumnService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        public boreportcolumnService(boreportcolumnContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;
            _context = context;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
            cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
        }

        // GET: service/boreportcolumn
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_boreportcolumns()
        {
            _logger.LogInfo("Getting into Get_boreportcolumns() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select pk_encode(a.reportcolumnid) as pkcol,reportcolumnid as value,tablealias as label from GetTable(NULL::public.boreportcolumns,@cid) a  WHERE  a.status='A' ORDER BY sequence";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_boreportcolumns(): {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetListBy_reportcolumnid(int reportcolumnid)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_reportcolumnid(int reportcolumnid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_reportcolumnid = new { @cid = cid, @uid = uid, @reportcolumnid = reportcolumnid };
                    var SQL = "select pk_encode(reportcolumnid) as pkcol,reportcolumnid as value,tablealias as label,* from GetTable(NULL::public.boreportcolumns,@cid) where reportcolumnid = @reportcolumnid";
                    var result = connection.Query<dynamic>(SQL, parameters_reportcolumnid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_reportcolumnid(int reportcolumnid) \r\n {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_boreportcolumn(string sid)
        {
            _logger.LogInfo("Getting into  Get_boreportcolumn(string sid) api");
            int id = Helper.GetId(sid);
            return Get_boreportcolumn(id);
        }
        // GET: boreportcolumn/5
        //gets the screen record
        public dynamic Get_boreportcolumn(int id)
        {
            _logger.LogInfo("Getting into Get_boreportcolumn(int id) api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";
                    string vdatatype = "datatype";
                    string vfiltertype = "filtertype";

                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus, @vdatatype = vdatatype, @vfiltertype = vfiltertype };
                    var SQL = @"select pk_encode(a.reportcolumnid) as pkcol,a.reportcolumnid as pk,a.*,
                              d.configtext as datatypedesc,
                              re.configtext as filtertypedesc
 from GetTable(NULL::public.boreportcolumns,@cid) a 
 left join boconfigvalues d on a.datatype=d.configkey and @vdatatype=d.param
 left join boconfigvalues re on a.filtertype=re.configkey and @vfiltertype=re.param
 where a.reportcolumnid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_boreportcolumn = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'boreportcolumns'";
                    var boreportcolumn_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { boreportcolumn = obj_boreportcolumn, boreportcolumn_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_boreportcolumn(int id)\r\n {ex}");
                throw ex;
            }
        }

        public IEnumerable<Object> GetList(string condition = "")
        {
            try
            {
                _logger.LogInfo("Getting into  GetList(string condition) api");

                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters = new { @cid = cid, @uid = uid, @key = condition };
                    var SQL = @"select  pk_encode(a.reportcolumnid) as pkcol,a.reportcolumnid as pk,* ,reportcolumnid as value,tablealias as label  from GetTable(NULL::public.boreportcolumns,@cid) a ";
                    if (condition != "") SQL += " and " + condition;
                    SQL += " order by sequence";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetFullList()
        {
            try
            {
                _logger.LogInfo("Getting into  GetFullList() api");

                int id = 0;
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    string wStatus = "NormalStatus";
                    string vdatatype = "datatype";
                    string vfiltertype = "filtertype";
                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus, @vdatatype = vdatatype, @vfiltertype = vfiltertype };
                    var SQL = @"select pk_encode(a.reportcolumnid) as pkcol,a.reportcolumnid as pk,a.*,
                              d.configtext as datatypedesc,
                              re.configtext as filtertypedesc from GetTable(NULL::public.boreportcolumns,@cid) a 
 left join boconfigvalues d on a.datatype=d.configkey and @vdatatype=d.param
 left join boconfigvalues re on a.filtertype=re.configkey and @vfiltertype=re.param";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
                throw ex;
            }
        }
        //saving of record
        public dynamic Save_boreportcolumn(string token, boreportcolumn obj_boreportcolumn)
        {
            _logger.LogInfo("Saving: Save_boreportcolumn(string token,boreportcolumn obj_boreportcolumn) ");
            try
            {
                string serr = "";
                int querytype = 0;
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                //connection.Open();
                //using var transaction = connection.BeginTransaction();
                //_context.Database.UseTransaction(transaction);
                //boreportcolumn table
                if (obj_boreportcolumn.reportcolumnid == 0 || obj_boreportcolumn.reportcolumnid == null || obj_boreportcolumn.reportcolumnid < 0)
                {
                    if (obj_boreportcolumn.status == "" || obj_boreportcolumn.status == null) obj_boreportcolumn.status = "A";
                    obj_boreportcolumn.createdby = uid;
                    obj_boreportcolumn.createddate = DateTime.Now;
                    _context.boreportcolumns.Add((dynamic)obj_boreportcolumn);
                    querytype = 1;
                }
                else
                {
                    obj_boreportcolumn.updatedby = uid;
                    obj_boreportcolumn.updateddate = DateTime.Now;
                    _context.Entry(obj_boreportcolumn).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_boreportcolumn).Property("createdby").IsModified = false;
                    _context.Entry(obj_boreportcolumn).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api boreportcolumns ");
                _context.SaveChanges();


                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                Helper.AfterExecute(token, querytype, obj_boreportcolumn, "boreportcolumns", 0, obj_boreportcolumn.reportcolumnid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_boreportcolumn((int)obj_boreportcolumn.reportcolumnid);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_boreportcolumn(string token,boreportcolumn obj_boreportcolumn) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: boreportcolumn/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                {
                    _logger.LogInfo("Getting into Delete(int id) api");
                    boreportcolumn obj_boreportcolumn = _context.boreportcolumns.Find(id);
                    _context.boreportcolumns.Remove(obj_boreportcolumn);
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _logger.LogInfo("remove api boreportcolumns ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Delete(int id) \r\n{ex}");
                throw ex;
            }
        }

        private bool boreportcolumn_Exists(int id)
        {
            try
            {
                return _context.boreportcolumns.Count(e => e.reportcolumnid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:boreportcolumn_Exists(int id) {ex}");
                return false;
            }
        }
    }
}

