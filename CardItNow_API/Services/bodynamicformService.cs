
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

namespace nTireBiz.Services
{
    public class bodynamicformService : IbodynamicformService
    {
        private readonly bodynamicformContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbodynamicformService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public bodynamicformService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public bodynamicformService(bodynamicformContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/bodynamicform
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_bodynamicforms()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.formid) as pkcol,formid as value,formname as label from bodynamicforms a  WHERE a.companyid=@cid and  a.status='A'";
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
                    var SQL = "select pk_encode(a.formid) as pkcol,*,formid as value,formname as label  from bodynamicforms a  where  a.companyid=@cid  and a.status='A' ";
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
        public IEnumerable<Object> GetListBy_formid(int formid)
        {
            try
            {
                _logger.LogInfo("Getting into formid/{formid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_formid = new { @cid = cid, @formid = formid };
                    var SQL = "select pk_encode(formid) as pkcol,* from bodynamicforms where formid = @formid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_formid);

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
        public IEnumerable<Object> GetListBy_tableiddesc(string tableiddesc)
        {
            try
            {
                _logger.LogInfo("Getting into tableiddesc/{tableiddesc} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_tableiddesc = new { @cid = cid, @tableiddesc = tableiddesc };
                    var SQL = "select pk_encode(formid) as pkcol,* from bodynamicforms where tableiddesc = @tableiddesc and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_tableiddesc);

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
        public dynamic Get_bodynamicform(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_bodynamicform(id);
        }
        // GET: bodynamicform/5
        //gets the screen record
        public dynamic Get_bodynamicform(int id)
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
                    string vformtype = "formtype";

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus, @vformtype = vformtype };
                    var SQL = @"select pk_encode(a.formid) as pkcol,a.*,
                              t.configtext as formtypedesc
 from bodynamicforms a 
 left join systemtables c on a.tableid=c.tableid
 left join boconfigvalues t on a.formtype=t.configkey and @vformtype=t.param
 where  a.companyid=@cid  and a.formid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_bodynamicform = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bodynamicforms'";
                    var bodynamicform_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    //Child table values
                    ArrayList bodynamicformdetails_visiblelist = new ArrayList();
                    ArrayList bodynamicformdetails_hidelist = new ArrayList();


                    var SQLbodynamicformdetails = @" select   pk_encode(a.formdetailid) as pkcol,a.*,t.configtext  as controltypedesc
 from bodynamicformdetails a  left join boconfigvalues t on a.controltype = t.configkey and t.param='controltype'
 WHERE  a.companyid=@cid  and a.status='A'  and  a.formid = @id  ORDER BY sequence";
                    var parameters_bodynamicformdetails = new { @cid = cid, @id = id };
                    var r_bodynamicformdetails = connection.Query<dynamic>(SQLbodynamicformdetails, parameters_bodynamicformdetails);
                    var SQL_bodynamicformdetail_menuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title,a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bodynamicformdetails'";
                    var bodynamicformdetail_menuactions = connection.Query<dynamic>(SQL_bodynamicformdetail_menuactions, parameters);
                    connection.Close();
                    connection.Dispose();
                    return (new { bodynamicform = obj_bodynamicform, bodynamicform_menuactions, bodynamicformdetails = r_bodynamicformdetails, bodynamicformdetail_menuactions, formproperty, visiblelist, hidelist, bodynamicformdetails_visiblelist, bodynamicformdetails_hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_bodynamicform(string token, bodynamicform obj_bodynamicform)
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
                    //bodynamicform table
                    if (obj_bodynamicform.formid == 0 || obj_bodynamicform.formid == null || obj_bodynamicform.formid < 0)
                    {
                        if (obj_bodynamicform.status == "" || obj_bodynamicform.status == null) obj_bodynamicform.status = "A";
                        obj_bodynamicform.companyid = cid;
                        obj_bodynamicform.createdby = uid;
                        obj_bodynamicform.createddate = DateTime.Now;
                        _context.bodynamicforms.Add((dynamic)obj_bodynamicform);
                        querytype = 1;
                    }
                    else
                    {
                        obj_bodynamicform.companyid = cid;
                        obj_bodynamicform.updatedby = uid;
                        obj_bodynamicform.updateddate = DateTime.Now;
                        _context.Entry(obj_bodynamicform).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_bodynamicform).Property("createdby").IsModified = false;
                        _context.Entry(obj_bodynamicform).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api bodynamicforms ");
                    _context.SaveChanges();
                    var options_bodynamicformdetail = new DbContextOptionsBuilder<bodynamicformdetailContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_bodynamicformdetail = new bodynamicformdetailContext(options_bodynamicformdetail);
                    //_context_bodynamicformdetail.Database.UseTransaction(transaction);
                    //bodynamicformdetails table
                    if (obj_bodynamicform.bodynamicformdetails != null)
                    {
                        int sequence = 1;
                        foreach (var item in obj_bodynamicform.bodynamicformdetails)
                        {
                            if (item != null)
                            {
                                item.sequence = sequence;
                                sequence++;
                                if (item.formdetailid == 0 || item.formdetailid == null || item.formdetailid < 0)
                                {
                                    item.formid = obj_bodynamicform.formid;
                                    if (item.status == "" || item.status == null) item.status = "A";
                                    item.companyid = cid;
                                    item.createdby = uid;
                                    item.createddate = DateTime.Now;
                                    _context_bodynamicformdetail.bodynamicformdetails.Add(item);
                                }
                                else
                                {
                                    item.formid = obj_bodynamicform.formid;
                                    item.companyid = cid;
                                    item.updatedby = uid;
                                    item.updateddate = DateTime.Now;
                                    _context_bodynamicformdetail.Entry(item).State = EntityState.Modified;
                                    //transaction.Commit();
                                }
                            }
                        }
                    }

                    //Delete for bodynamicformdetails
                    if (obj_bodynamicform.Deleted_bodynamicformdetail_IDs != null)
                    {
                        foreach (var id in obj_bodynamicform.Deleted_bodynamicformdetail_IDs.Split(',').Where(x => x != ""))
                        {
                            bodynamicformdetail obj = _context_bodynamicformdetail.bodynamicformdetails.Find(Convert.ToInt32(id));
                            if (obj != null) _context_bodynamicformdetail.bodynamicformdetails.Remove(obj);
                        }
                        _context_bodynamicformdetail.SaveChanges();
                    }


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_bodynamicform, "bodynamicforms", obj_bodynamicform.companyid, obj_bodynamicform.formid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_bodynamicform((int)obj_bodynamicform.formid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: bodynamicform/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_bodynamicform = Get_bodynamicform(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    var options_bodynamicformdetail = new DbContextOptionsBuilder<bodynamicformdetailContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_bodynamicformdetail = new bodynamicformdetailContext(options_bodynamicformdetail);
                    //_context_bodynamicformdetail.Database.UseTransaction(transaction);
                    if (obj_bodynamicform.bodynamicformdetails != null)
                    {
                        foreach (var item in obj_bodynamicform.bodynamicformdetails)
                        {
                            _context_bodynamicformdetail.bodynamicformdetails.Remove(_context_bodynamicformdetail.bodynamicformdetails.Find(item.formdetailid));
                        }
                        _context_bodynamicformdetail.SaveChanges();
                    }

                    _context.bodynamicforms.Remove(_context.bodynamicforms.Find(id));
                    _logger.LogInfo("remove api bodynamicforms ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_bodynamicform);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool bodynamicform_Exists(int id)
        {
            try
            {
                return _context.bodynamicforms.Count(e => e.formid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }
    }
}

