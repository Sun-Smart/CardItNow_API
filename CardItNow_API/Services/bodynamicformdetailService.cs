
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
using System.Linq;

namespace nTireBiz.Services
{
    public class bodynamicformdetailService : IbodynamicformdetailService
    {
        private readonly bodynamicformdetailContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbodynamicformdetailService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public bodynamicformdetailService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public bodynamicformdetailService(bodynamicformdetailContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        // GET: service/bodynamicformdetail
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_bodynamicformdetails()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.formdetailid) as pkcol,formdetailid as value,'' as label from bodynamicformdetails a  WHERE a.companyid=@cid and  a.status='A' ORDER BY sequence";
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
                    var SQL = "select pk_encode(a.formdetailid) as pkcol,*,formdetailid as value,'' as label  from bodynamicformdetails a  where  a.companyid=@cid  and a.status='A' ";
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
        public IEnumerable<Object> GetListBy_formdetailid(int formdetailid)
        {
            try
            {
                _logger.LogInfo("Getting into formdetailid/{formdetailid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_formdetailid = new { @cid = cid, @formdetailid = formdetailid };
                    var SQL = "select pk_encode(formdetailid) as pkcol,* from bodynamicformdetails where formdetailid = @formdetailid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_formdetailid);

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
        public dynamic Get_bodynamicformdetail(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_bodynamicformdetail(id);
        }
        // GET: bodynamicformdetail/5
        //gets the screen record
        public dynamic Get_bodynamicformdetail(int id)
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
                    string vcontroltype = "controltype";

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus, @vcontroltype = vcontroltype };
                    var SQL = @"select pk_encode(a.formdetailid) as pkcol,a.*,
                              t.configtext as controltypedesc
 from bodynamicformdetails a 
 left join boconfigvalues t on a.controltype=t.configkey and @vcontroltype=t.param
 where  a.companyid=@cid  and a.formdetailid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_bodynamicformdetail = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bodynamicformdetails'";
                    var bodynamicformdetail_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { bodynamicformdetail = obj_bodynamicformdetail, bodynamicformdetail_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_bodynamicformdetail(string token, bodynamicformdetail obj_bodynamicformdetail)
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
                    //bodynamicformdetail table
                    if (obj_bodynamicformdetail.formdetailid == 0 || obj_bodynamicformdetail.formdetailid == null || obj_bodynamicformdetail.formdetailid < 0)
                    {
                        if (obj_bodynamicformdetail.status == "" || obj_bodynamicformdetail.status == null) obj_bodynamicformdetail.status = "A";
                        obj_bodynamicformdetail.companyid = cid;
                        obj_bodynamicformdetail.createdby = uid;
                        obj_bodynamicformdetail.createddate = DateTime.Now;
                        _context.bodynamicformdetails.Add((dynamic)obj_bodynamicformdetail);
                        querytype = 1;
                    }
                    else
                    {
                        obj_bodynamicformdetail.companyid = cid;
                        obj_bodynamicformdetail.updatedby = uid;
                        obj_bodynamicformdetail.updateddate = DateTime.Now;
                        _context.Entry(obj_bodynamicformdetail).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_bodynamicformdetail).Property("createdby").IsModified = false;
                        _context.Entry(obj_bodynamicformdetail).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api bodynamicformdetails ");
                    _context.SaveChanges();


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_bodynamicformdetail, "bodynamicformdetails", obj_bodynamicformdetail.companyid, obj_bodynamicformdetail.formdetailid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_bodynamicformdetail((int)obj_bodynamicformdetail.formdetailid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: bodynamicformdetail/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_bodynamicformdetail = Get_bodynamicformdetail(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _context.bodynamicformdetails.Remove(_context.bodynamicformdetails.Find(id));
                    _logger.LogInfo("remove api bodynamicformdetails ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_bodynamicformdetail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool bodynamicformdetail_Exists(int id)
        {
            try
            {
                return _context.bodynamicformdetails.Count(e => e.formdetailid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }
    }
}

