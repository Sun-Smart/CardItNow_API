
using carditnow.Models;
using carditnow.Services;
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SunSmartnTireProducts.Helpers;
using SunSmartnTireProducts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace carditnow.Services
{
    public class systemtableService : IsystemtableService
    {
        private readonly systemtableContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IsystemtableService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public systemtableService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public systemtableService(systemtableContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
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

        // GET: service/systemtable
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_systemtables()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.tableid) as pkcol,tableid as value,'' as label from systemtables a  WHERE  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetList(string key)
        {
            try
            {
                _logger.LogInfo("Getting into param/{key} GetList api");

                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters = new { @cid = cid, @key = key };
                    var SQL = "select pk_encode(a.tableid) as pkcol,*,tableid as value,'' as label  from systemtables a ";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetListBy_tableid(int tableid)
        {
            try
            {
                _logger.LogInfo("Getting into tableid/{tableid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_tableid = new { @cid = cid, @tableid = tableid };
                    var SQL = "select pk_encode(tableid) as pkcol,* from systemtables where tableid = @tableid";
                    var result = connection.Query<dynamic>(SQL, parameters_tableid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetListBy_tablename(string tablename)
        {
            try
            {
                _logger.LogInfo("Getting into tablename/{tablename} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_tablename = new { @cid = cid, @tablename = tablename };
                    var SQL = "select pk_encode(tableid) as pkcol,* from systemtables where tablename = @tablename";
                    var result = connection.Query<dynamic>(SQL, parameters_tablename);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_systemtable(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_systemtable(id);
        }
        // GET: systemtable/5
        //gets the screen record
        public dynamic Get_systemtable(int id)
        {
            _logger.LogInfo("Getting into {id} api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";
                    string vcolor = "color";
                    string vpriority = "priority";
                    string vicon = "icon";

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus, @vcolor = vcolor, @vpriority = vpriority, @vicon = vicon };
                    var SQL = @"select pk_encode(a.tableid) as pkcol,a.*,
                              o.configtext as remindercolorcodedesc,
                              p.configtext as reminderprioritydesc,
                              i.configtext as remindericondesc
 from systemtables a 
 left join boconfigvalues o on a.remindercolorcode=o.configkey and @vcolor=o.param
 left join boconfigvalues p on a.reminderpriority=p.configkey and @vpriority=p.param
 left join boconfigvalues i on a.remindericon=i.configkey and @vicon=i.param
 where a.tableid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_systemtable = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'systemtables'";
                    var systemtable_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    //Child table values
                    ArrayList systemtabletemplates_visiblelist = new ArrayList();
                    ArrayList systemtabletemplates_hidelist = new ArrayList();


                    var SQLsystemtabletemplates = @" select   pk_encode(a.tabledetailid) as pkcol,a.*,r.userrole as userroleiddesc
 from systemtabletemplates a  left join bouserrolemasters r on a.userroleid=r.userroleid and  r.companyid=@cid 
 WHERE  a.tableid = @id ";
                    var parameters_systemtabletemplates = new { @cid = cid, @id = id };
                    var r_systemtabletemplates = connection.Query<dynamic>(SQLsystemtabletemplates, parameters_systemtabletemplates);
                    var SQL_systemtabletemplate_menuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title,a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'systemtabletemplates'";
                    var systemtabletemplate_menuactions = connection.Query<dynamic>(SQL_systemtabletemplate_menuactions, parameters);
                    connection.Close();
                    connection.Dispose();
                    return (new { systemtable = obj_systemtable, systemtable_menuactions, systemtabletemplates = r_systemtabletemplates, systemtabletemplate_menuactions, formproperty, visiblelist, hidelist, systemtabletemplates_visiblelist, systemtabletemplates_hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_systemtable(string token, systemtable obj_systemtable)
        {
            _logger.LogInfo("Saving");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    string serr = "";
                    int querytype = 0;
                    if (serr != "")
                    {
                        _logger.LogError($"Something went wrong: {serr}");
                        throw new Exception(serr);
                    }

                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    //systemtable table
                    if (obj_systemtable.tableid == 0 || obj_systemtable.tableid == null || obj_systemtable.tableid < 0)
                    {
                        if (obj_systemtable.status == "" || obj_systemtable.status == null) obj_systemtable.status = "A";
                        obj_systemtable.createdby = uid;
                        obj_systemtable.createddate = DateTime.Now;
                        _context.systemtables.Add((dynamic)obj_systemtable);
                        querytype = 1;
                    }
                    else
                    {
                        obj_systemtable.updatedby = uid;
                        obj_systemtable.updateddate = DateTime.Now;
                        _context.Entry(obj_systemtable).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_systemtable).Property("createdby").IsModified = false;
                        _context.Entry(obj_systemtable).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api systemtables ");
                    _context.SaveChanges();
                    var options_systemtabletemplate = new DbContextOptionsBuilder<systemtabletemplateContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_systemtabletemplate = new systemtabletemplateContext(options_systemtabletemplate);
                    //_context_systemtabletemplate.Database.UseTransaction(transaction);
                    //systemtabletemplates table
                    if (obj_systemtable.systemtabletemplates != null)
                    {
                        foreach (var item in obj_systemtable.systemtabletemplates)
                        {
                            if (item != null)
                            {
                                if (item.tabledetailid == 0 || item.tabledetailid == null || item.tabledetailid < 0)
                                {
                                    item.tableid = obj_systemtable.tableid;
                                    if (item.status == "" || item.status == null) item.status = "A";
                                    item.createdby = uid;
                                    item.createddate = DateTime.Now;
                                    _context_systemtabletemplate.systemtabletemplates.Add(item);
                                }
                                else
                                {
                                    item.tableid = obj_systemtable.tableid;
                                    item.updatedby = uid;
                                    item.updateddate = DateTime.Now;
                                    _context_systemtabletemplate.Entry(item).State = EntityState.Modified;
                                    //transaction.Commit();
                                }
                            }
                        }
                    }

                    //Delete for systemtabletemplates
                    if (obj_systemtable.Deleted_systemtabletemplate_IDs != null)
                    {
                        foreach (var id in obj_systemtable.Deleted_systemtabletemplate_IDs.Split(',').Where(x => x != ""))
                        {
                            systemtabletemplate obj = _context_systemtabletemplate.systemtabletemplates.Find(Convert.ToInt32(id));
                            if (obj != null) _context_systemtabletemplate.systemtabletemplates.Remove(obj);
                        }
                        _context_systemtabletemplate.SaveChanges();
                    }


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_systemtable, "systemtables", 0, obj_systemtable.tableid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_systemtable((int)obj_systemtable.tableid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: systemtable/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_systemtable = Get_systemtable(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    var options_systemtabletemplate = new DbContextOptionsBuilder<systemtabletemplateContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_systemtabletemplate = new systemtabletemplateContext(options_systemtabletemplate);
                    //_context_systemtabletemplate.Database.UseTransaction(transaction);
                    if (obj_systemtable.systemtabletemplates != null)
                    {
                        foreach (var item in obj_systemtable.systemtabletemplates)
                        {
                            _context_systemtabletemplate.systemtabletemplates.Remove(_context_systemtabletemplate.systemtabletemplates.Find(item.tabledetailid));
                        }
                        _context_systemtabletemplate.SaveChanges();
                    }

                    _context.systemtables.Remove(_context.systemtables.Find(id));
                    _logger.LogInfo("remove api systemtables ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_systemtable);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool systemtable_Exists(int id)
        {
            try
            {
                return _context.systemtables.Count(e => e.tableid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }
    }
}

