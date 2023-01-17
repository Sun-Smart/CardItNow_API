
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
    public class customerpaymodeService : IcustomerpaymodeService
    {
        private readonly IConfiguration Configuration;
        private readonly customerpaymodeContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomerpaymodeService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public customerpaymodeService(customerpaymodeContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;
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
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            }
        }

        // GET: service/customerpaymode
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customerpaymodes()
        {
            _logger.LogInfo("Getting into Get_customerpaymodes() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select pk_encode(a.payid) as pkcol,payid as value,uid as label from GetTable(NULL::public.customerpaymodes,@cid) a  WHERE  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_customerpaymodes(): {ex}");
                throw ex;
            }
            return null;
        }
        public IEnumerable<Object> GetListBy_payid(int payid)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_payid(int payid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_payid = new { @cid = cid, @uid = uid, @payid = payid };
                    var SQL = "select pk_encode(payid) as pkcol,payid as value,uid as label,* from GetTable(NULL::public.customerpaymodes,@cid) where payid = @payid";
                    var result = connection.Query<dynamic>(SQL, parameters_payid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_payid(int payid) \r\n {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_customerpaymode(string sid)
        {
            _logger.LogInfo("Getting into  Get_customerpaymode(string sid) api");
            int id = Helper.GetId(sid);
            return Get_customerpaymode(id);
        }
        // GET: customerpaymode/5
        //gets the screen record
        public dynamic Get_customerpaymode(int id)
        {
            _logger.LogInfo("Getting into Get_customerpaymode(int id) api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";

                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
                    var SQL = @"select pk_encode(a.payid) as pkcol,a.payid as pk,a.*,
u.email as uiddesc
 from GetTable(NULL::public.customerpaymodes,@cid) a 
 left join customermasters u on a.uid=u.uid
 where a.customerid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_customerpaymode = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { customerpaymode = obj_customerpaymode, customerpaymode_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customerpaymode(int id)\r\n {ex}");
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
                    var SQL = @"select  pk_encode(a.payid) as pkcol,a.payid as pk,* ,payid as value,uid as label  from GetTable(NULL::public.customerpaymodes,@cid) a ";
                    if (condition != "") SQL += " and " + condition;
                    SQL += " order by uid";
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
                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
                    var SQL = @"select pk_encode(a.payid) as pkcol,a.payid as pk,a.*,
u.email as uiddesc from GetTable(NULL::public.customerpaymodes,@cid) a 
 left join customermasters u on a.uid=u.uid";
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
        public dynamic Save_customerpaymode(string token, customerpaymode obj_customerpaymode)
        {
            _logger.LogInfo("Saving: Save_customerpaymode(string token,customerpaymode obj_customerpaymode) ");
            try
            {
                string serr = "";
                int querytype = 0;
                if (obj_customerpaymode.cardnumber != null)
                {
                    var parameterscardnumber = new { @cid = cid, @uid = uid, @cardnumber = obj_customerpaymode.cardnumber, @payid = obj_customerpaymode.payid };
                    if (Helper.Count("select count(*) from customerpaymodes where  and cardnumber =  @cardnumber and (@payid == 0 ||  @payid == null ||  @payid < 0 || payid!=  @payid)", parameterscardnumber) > 0) serr += "cardnumber is unique\r\n";
                }
                if (obj_customerpaymode.cardname != null)
                {
                    var parameterscardname = new { @cid = cid, @uid = uid, @cardname = obj_customerpaymode.cardname, @payid = obj_customerpaymode.payid };
                    if (Helper.Count("select count(*) from customerpaymodes where  and cardname =  @cardname and (@payid == 0 ||  @payid == null ||  @payid < 0 || payid!=  @payid)", parameterscardname) > 0) serr += "cardname is unique\r\n";
                }
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                //connection.Open();
                //using var transaction = connection.BeginTransaction();
                //_context.Database.UseTransaction(transaction);
                //customerpaymode table
                if (obj_customerpaymode.payid == 0 || obj_customerpaymode.payid == null || obj_customerpaymode.payid < 0)
                {
                    if (obj_customerpaymode.status == "" || obj_customerpaymode.status == null) obj_customerpaymode.status = "A";
                    //obj_customerpaymode.companyid=cid;
                    obj_customerpaymode.createdby = uid;
                    obj_customerpaymode.createddate = DateTime.Now;
                    _context.customerpaymodes.Add((dynamic)obj_customerpaymode);
                    querytype = 1;
                }
                else
                {
                    //obj_customerpaymode.companyid=cid;
                    obj_customerpaymode.updatedby = uid;
                    obj_customerpaymode.updateddate = DateTime.Now;
                    _context.Entry(obj_customerpaymode).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customerpaymode).Property("createdby").IsModified = false;
                    _context.Entry(obj_customerpaymode).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api customerpaymodes ");
                _context.SaveChanges();


                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                Helper.AfterExecute(token, querytype, obj_customerpaymode, "customerpaymodes", 0, obj_customerpaymode.payid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_customerpaymode((int)obj_customerpaymode.payid);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customerpaymode(string token,customerpaymode obj_customerpaymode) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customerpaymode/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                {
                    _logger.LogInfo("Getting into Delete(int id) api");
                    customerpaymode obj_customerpaymode = _context.customerpaymodes.Find(id);
                    _context.customerpaymodes.Remove(obj_customerpaymode);
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _logger.LogInfo("remove api customerpaymodes ");
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
        private bool customerpaymode_Exists(int id)
        {
            try
            {
                return _context.customerpaymodes.Count(e => e.payid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:customerpaymode_Exists(int id) {ex}");
                return false;
            }
        }

        public dynamic SaveCutomerCardDeatils(customerpaymode obj_customerpaymode)
        {
            try
            {
                _logger.LogInfo("Getting into SendOTP(string email) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    var get_cuscode = @"select cardnumber from customerpaymodes where customerid='" + obj_customerpaymode.customerid + "' ";
                    var result = connection.ExecuteScalar(get_cuscode, connection);
                    if (result==null)
                    {
                        if (!string.IsNullOrEmpty(obj_customerpaymode.cardnumber))
                        {
                            NpgsqlCommand customer_card = new NpgsqlCommand("insert into customerpaymodes (customerid,uid,cardnumber,cardname,expirydate,bankname,ibannumber,status,createdby,createddate,updatedby,updateddate)  values(@customerid,@uid,@cardnumber,@cardname,@expirydate,@bankname," +
                                "@ibannumber,@status,@createdby,@createddate,@updatedby,@updateddate)", connection);
                            customer_card.Parameters.AddWithValue("@customerid", obj_customerpaymode.customerid);
                            customer_card.Parameters.AddWithValue("@uid", obj_customerpaymode.uid);
                            customer_card.Parameters.AddWithValue("@cardnumber", obj_customerpaymode.cardnumber);
                            customer_card.Parameters.AddWithValue("@cardname", obj_customerpaymode.cardname);
                            customer_card.Parameters.AddWithValue("@expirydate", obj_customerpaymode.expirydate);
                            customer_card.Parameters.AddWithValue("@bankname", obj_customerpaymode.bankname);
                            customer_card.Parameters.AddWithValue("@ibannumber", obj_customerpaymode.ibannumber);
                            customer_card.Parameters.AddWithValue("@status", "A");
                            customer_card.Parameters.AddWithValue("@createdby", obj_customerpaymode.customerid);
                            customer_card.Parameters.AddWithValue("@createddate", DateTime.Now);
                            customer_card.Parameters.AddWithValue("@updatedby", 0);
                            customer_card.Parameters.AddWithValue("@updateddate", DateTime.Now);
                            var output = customer_card.ExecuteNonQuery();
                            if (output > 0)
                            {
                                return "Success";
                            }
                            else
                            {
                                return "fail";
                            }
                        }
                        else
                        {
                            return "Card no missing";
                        }
                    }
                    else
                    {
                        if (result.ToString() != (obj_customerpaymode.cardnumber).ToString())
                        {
                            return "Card already added";
                        }
                        //return "CardNumber missing";
                    }
                    connection.Close();
                    connection.Dispose();
                    return (result.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            return null;
        }
    }
}

