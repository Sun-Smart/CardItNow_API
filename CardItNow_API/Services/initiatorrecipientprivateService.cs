
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
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Net;

namespace carditnow.Services
{
    public class initiatorrecipientprivateService : IinitiatorrecipientprivateService
    {
        private readonly IConfiguration Configuration;
        private readonly initiatorrecipientprivateContext _context;
        private readonly customermasterContext _cus_context;
        private readonly customerdetailContext _cd_context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IinitiatorrecipientprivateService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public initiatorrecipientprivateService(initiatorrecipientprivateContext context, customermasterContext cus_context, customerdetailContext cd_context, IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
        {
Configuration = configuration;
            _context = context;
            _logger = logger;
            _cus_context = cus_context;
            _cd_context = cd_context;
            this.httpContextAccessor = objhttpContextAccessor;
        cid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
        uid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
        uname = "";
        uidemail = "";
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")!=null)uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid")!=null)uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
        }

        // GET: service/initiatorrecipientprivate
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_initiatorrecipientprivates()
        {
        _logger.LogInfo("Getting into Get_initiatorrecipientprivates() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.privateid) as pkcol,privateid as value,uid as label from GetTable(NULL::public.initiatorrecipientprivates,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_initiatorrecipientprivates(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_privateid(int privateid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_privateid(int privateid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_privateid = new { @cid = cid,@uid=uid ,@privateid = privateid  };
            var SQL = "select pk_encode(privateid) as pkcol,privateid as value,uid as label,* from GetTable(NULL::public.initiatorrecipientprivates,@cid) where privateid = @privateid";
var result = connection.Query<dynamic>(SQL, parameters_privateid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_privateid(int privateid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_initiatorrecipientprivate(string sid)
{
        _logger.LogInfo("Getting into  Get_initiatorrecipientprivate(string sid) api");
int id = Helper.GetId(sid);
  return  Get_initiatorrecipientprivate(id);
}
        // GET: initiatorrecipientprivate/5
//gets the screen record
        public  dynamic Get_initiatorrecipientprivate(int id)
        {
        _logger.LogInfo("Getting into Get_initiatorrecipientprivate(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vcustomermastertype ="customermastertype";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vcustomermastertype =vcustomermastertype};
var SQL = @"select pk_encode(a.privateid) as pkcol,a.privateid as pk,a.*,
i.email as uiddesc,
s.cityname as cityiddesc,
                              y.configtext as typedesc,
u.email as customeriddesc,
g.geoname as geoiddesc
 from GetTable(NULL::public.initiatorrecipientprivates,@cid) a 
 left join customermasters i on a.uid=i.uid
 left join citymasters s on a.cityid=s.cityid
 left join boconfigvalues y on a.type=y.configkey and @vcustomermastertype=y.param
 left join customermasters u on a.customerid=u.customerid
 left join geographymasters g on a.geoid=g.geoid
 where a.privateid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_initiatorrecipientprivate = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'initiatorrecipientprivates'";
var initiatorrecipientprivate_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { initiatorrecipientprivate=obj_initiatorrecipientprivate,initiatorrecipientprivate_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_initiatorrecipientprivate(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.privateid) as pkcol,a.privateid as pk,* ,privateid as value,uid as label  from GetTable(NULL::public.initiatorrecipientprivates,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by uid";
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
string vcustomermastertype ="customermastertype";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vcustomermastertype =vcustomermastertype};
            var SQL = @"select pk_encode(a.privateid) as pkcol,a.privateid as pk,a.*,
i.email as uiddesc,
s.cityname as cityiddesc,
                              y.configtext as typedesc,
u.email as customeriddesc,
g.geoname as geoiddesc from GetTable(NULL::public.initiatorrecipientprivates,@cid) a 
 left join customermasters i on a.uid=i.uid
 left join citymasters s on a.cityid=s.cityid
 left join boconfigvalues y on a.type=y.configkey and @vcustomermastertype=y.param
 left join customermasters u on a.customerid=u.customerid
 left join geographymasters g on a.geoid=g.geoid";
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
        public  dynamic Save_initiatorrecipientprivate(string token,initiatorrecipientprivate obj_initiatorrecipientprivate)
        {
        _logger.LogInfo("Saving: Save_initiatorrecipientprivate(string token,initiatorrecipientprivate obj_initiatorrecipientprivate) ");
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
                //initiatorrecipientprivate table
                if (obj_initiatorrecipientprivate.privateid == 0 || obj_initiatorrecipientprivate.privateid == null || obj_initiatorrecipientprivate.privateid<0)
{
if(obj_initiatorrecipientprivate.status=="" || obj_initiatorrecipientprivate.status==null)obj_initiatorrecipientprivate.status="A";
//obj_initiatorrecipientprivate.companyid=cid;
obj_initiatorrecipientprivate.createdby=uid;
obj_initiatorrecipientprivate.createddate=DateTime.Now;
                    _context.initiatorrecipientprivates.Add((dynamic)obj_initiatorrecipientprivate);
querytype=1;
}
                else
{
//obj_initiatorrecipientprivate.companyid=cid;
obj_initiatorrecipientprivate.updatedby=uid;
obj_initiatorrecipientprivate.updateddate=DateTime.Now;
                    _context.Entry(obj_initiatorrecipientprivate).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_initiatorrecipientprivate).Property("createdby").IsModified = false;
                    _context.Entry(obj_initiatorrecipientprivate).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api initiatorrecipientprivates ");
                _context.SaveChanges();


//to generate serial key - select serialkey option for that column
//the procedure to call after insert/update/delete - configure in systemtables 

Helper.AfterExecute(token,querytype,obj_initiatorrecipientprivate,"initiatorrecipientprivates", 0,obj_initiatorrecipientprivate.privateid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_initiatorrecipientprivate( (int)obj_initiatorrecipientprivate.privateid);
return (res);
                
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_initiatorrecipientprivate(string token,initiatorrecipientprivate obj_initiatorrecipientprivate) \r\n{ex}");
                throw ex;
            }
        }




        public dynamic Save_initiatorrecipientprivate1(string token, initiatorrecipientprivate obj_initiatorrecipientprivate)
        {
            if (obj_initiatorrecipientprivate.customerid != null)
            {
                int maxid = 0;
                string newValueString = string.Empty;
                //string geoids = obj_initiatorrecipientprivate.geoid;
                int? geid = 0;
                geid = obj_initiatorrecipientprivate.geoid;
                bool OTPUpdated = false;
                int custid = 0;
                //int uidd = 0;
                string uidd = string.Empty;
                string cusemail = string.Empty;
                string cusname = string.Empty;
                _logger.LogInfo("Getting into Get_avatarmasters() api");
                try
                {
                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {

                        connection.Open();
                        //if ((obj_initiatorrecipientprivate.customerid != null) &&
                        //    (!string.IsNullOrEmpty(obj_initiatorrecipientprivate.Type)))
                        //{


                        var customers = _cus_context.customermasters.Where(x => x.email == obj_initiatorrecipientprivate.email);
                        if (customers != null && customers.Any())
                        {
                            var dbEntry = _cus_context.customermasters.Find(customers.FirstOrDefault().customerid);

                            if (dbEntry != null)
                            {

                                var parameters_customerid1 = new { @cid = cid, @uid = uid, @email = obj_initiatorrecipientprivate.email };
                                var SQL1 = "  select pk_encode(m.customerid) as pkcol,m.customerid,m.uid,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                                var result1 = connection.Query<dynamic>(SQL1, parameters_customerid1);
                                var obj_cutomerid1 = result1.FirstOrDefault();
                                custid = obj_cutomerid1.customerid;
                                uidd = obj_cutomerid1.uid;

                                NpgsqlCommand upd_customermaster = new NpgsqlCommand("update customermasters set mode=@mode,type=@type,customervisible=@customervisible where customerid=@customerid", connection);
                                upd_customermaster.Parameters.AddWithValue("@customerid", obj_initiatorrecipientprivate.customerid);
                                upd_customermaster.Parameters.AddWithValue("@mode", obj_initiatorrecipientprivate.type);
                                upd_customermaster.Parameters.AddWithValue("@type", obj_initiatorrecipientprivate.type);
                                upd_customermaster.Parameters.AddWithValue("@customervisible", obj_initiatorrecipientprivate.visibletoall);
                                int result_upd = upd_customermaster.ExecuteNonQuery();




                                NpgsqlCommand check_payer1 = new NpgsqlCommand("select count(*)  from initiatorrecipientmappings where customerid = '" + obj_initiatorrecipientprivate.customerid + "' and uid = '" + obj_initiatorrecipientprivate.uid + "' and recipientuid = '" + uidd + "'", connection);
                                var output_result1 = check_payer1.ExecuteScalar().ToString();
                                if ((int.Parse(output_result1) == 0) || (output_result1 == null))
                                {





                                    NpgsqlCommand check_payer = new NpgsqlCommand("select count(*)  from initiatorrecipientprivates where customerid = '" + obj_initiatorrecipientprivate.customerid + "' and bankname = '" + obj_initiatorrecipientprivate.bankname + "' and bankaccountnumber = '" + obj_initiatorrecipientprivate.bankaccountnumber + "'", connection);
                                    var output_result = check_payer.ExecuteScalar().ToString();


                                    if ((int.Parse(output_result) == 0) || (output_result == null))
                                    {

                                        var parameters_cus = new { @cid = cid, @uid = uid, @customerid = obj_initiatorrecipientprivate.customerid };
                                        var SQL_cus = "  select  email,concat(firstname,'',lastname) as cusname from customermasters where customerid=@customerid";
                                        var resultcus = connection.Query<dynamic>(SQL_cus, parameters_cus);
                                        var obj_cus = resultcus.FirstOrDefault();
                                        cusemail = obj_cus.email;
                                        cusname = obj_cus.cusname;








                                        NpgsqlCommand inst_payerpayeeprivate = new NpgsqlCommand("INSERT INTO public.initiatorrecipientprivates(privateid, customerid, uid, type, firstname, lastname, email, mobile, geoid, cityid, pincode, bankaccountnumber, bankname, iban, accountname, status, createdby, createddate)VALUES(@customerid,@uid,@type,@firstname,@lastname,@email,@mobile,@geocode,@city,@pincode,@bankaccountnumber,@bankname,@iban,@accountname,@status,@createdby,@createddate)", connection);
                                        // inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", obj_initiatorrecipientprivate.customerid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", custid);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@uid", obj_initiatorrecipientprivate.uid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@uid", uidd);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@type", obj_initiatorrecipientprivate.type);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@firstname", obj_initiatorrecipientprivate.firstname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@lastname", obj_initiatorrecipientprivate.lastname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@email", obj_initiatorrecipientprivate.email);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@mobile", obj_initiatorrecipientprivate.mobile);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@geocode", obj_initiatorrecipientprivate.geoid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@city", obj_initiatorrecipientprivate.cityid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@pincode", obj_initiatorrecipientprivate.pincode);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@bankaccountnumber", obj_initiatorrecipientprivate.bankaccountnumber);
                                        // inst_payerpayeeprivate.Parameters.AddWithValue("@brn", obj_initiatorrecipientprivate.b);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@bankname", obj_initiatorrecipientprivate.bankname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@iban", obj_initiatorrecipientprivate.iban);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@accountname", obj_initiatorrecipientprivate.accountname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@status", 'A');
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@createdby", obj_initiatorrecipientprivate.customerid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        // inst_payerpayeeprivate.Parameters.AddWithValue("@middlename", obj_initiatorrecipientprivate.middlename);
                                        // inst_payerpayeeprivate.Parameters.AddWithValue("@accounttype", obj_initiatorrecipientprivate.accounttype);
                                        //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_initiatorrecipientprivate.documnettype);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_initiatorrecipientprivate.documentvalue);
                                        int resultpayerpayeeprivate = inst_payerpayeeprivate.ExecuteNonQuery();


                                        //NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into initiatorrecipientmappings(customerid, uid, recipientuid, status) values(@customerid, @uid, @recipientuid, @status)", connection);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_initiatorrecipientprivate.customerid);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_initiatorrecipientprivate.uid);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@recipientuid", uidd);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_initiatorrecipientprivate.customerid);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        ////  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_initiatorrecipientprivate.documnettype);
                                        ////inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_initiatorrecipientprivate.documentvalue);
                                        //int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();






                                    }
                                    else
                                    {

                                        //NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into initiatorrecipientmappings(customerid, uid, recipientuid, status) values(@customerid, @uid, @recipientuid, @status)", connection);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_initiatorrecipientprivate.customerid);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_initiatorrecipientprivate.uid);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@recipientuid", uidd);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_initiatorrecipientprivate.customerid);
                                        //inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        ////  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_initiatorrecipientprivate.documnettype);
                                        ////inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_initiatorrecipientprivate.documentvalue);
                                        //int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();


                                        return "Success";
                                    }

                                }

                            }
                            else
                            {
                                return "The Given Payee already availbale in your My payee list ";
                            }





                        }
                        else
                        {

                            var parameters_customerid = new { @cid = cid, @uid = uid };
                            var SQL = "select max(customerid) as maxid from customermasters";
                            var result = connection.Query<dynamic>(SQL, parameters_customerid);
                            var obj_cutomerid = result.FirstOrDefault();
                            maxid = obj_cutomerid.maxid;
                            maxid = maxid + 1;



                            if (geid == 1)
                            {
                                newValueString = "U" + maxid.ToString().PadLeft(8, '0');
                            }
                            if (geid == 2)
                            {
                                newValueString = "P" + maxid.ToString().PadLeft(8, '0');
                            }

                            // convert back to string with leading zero's
                            //string newValueString = maxid.ToString().PadLeft(8, '0');



                            var cus_master = new customermaster();
                            var cus_detail = new customerdetail();
                            cus_master.email = obj_initiatorrecipientprivate.email;
                            cus_master.createdby = 0;
                            cus_master.mode = obj_initiatorrecipientprivate.type;
                            //cus_master.uid = "P" + DateTime.Now.Second.ToString();
                            cus_master.uid = newValueString;
                            cus_master.type = obj_initiatorrecipientprivate.type;
                            cus_master.status = "N";
                            cus_master.mobile = obj_initiatorrecipientprivate.mobile;
                            cus_master.createddate = DateTime.Now;
                            cus_master.customervisible = obj_initiatorrecipientprivate.visibletoall;
                            // cus_master.otp = TACNo.ToString();
                            cus_master.customervisible = false;
                            _cus_context.customermasters.Add(cus_master);
                            _cus_context.SaveChanges();



                            var parameters_customerid1 = new { @cid = cid, @uid = uid, @email = obj_initiatorrecipientprivate.email };
                            var SQL1 = "  select pk_encode(m.customerid) as pkcol,m.customerid,m.uid,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                            var result1 = connection.Query<dynamic>(SQL1, parameters_customerid1);
                            var obj_cutomerid1 = result1.FirstOrDefault();
                            custid = obj_cutomerid1.customerid;
                            uidd = obj_cutomerid1.uid;



                            //var cus_detail = new customerdetail();
                            cus_detail.geoid = geid;
                            cus_detail.customerid = custid;
                            cus_detail.uid = uidd.ToString();
                            _cd_context.customerdetails.Add(cus_detail);
                            _cd_context.SaveChanges();




                            NpgsqlCommand check_payer1 = new NpgsqlCommand("select count(*)  from initiatorrecipientmappings where customerid = '" + custid + "' and uid = '" + obj_initiatorrecipientprivate.uid + "' and recipientuid = '" + uidd + "'", connection);
                            var output_result1 = check_payer1.ExecuteScalar().ToString();
                            if ((int.Parse(output_result1) == 0) || (output_result1 == null))
                            {





                                NpgsqlCommand check_payer = new NpgsqlCommand("select count(*)  from initiatorrecipientprivates where customerid = '" + obj_initiatorrecipientprivate.customerid + "' and bankname = '" + obj_initiatorrecipientprivate.bankname + "' and bankaccountnumber = '" + obj_initiatorrecipientprivate.bankaccountnumber + "'", connection);
                                var output_result = check_payer.ExecuteScalar().ToString();


                                if ((int.Parse(output_result) == 0) || (output_result == null))
                                {

                                    var parameters_cus = new { @cid = cid, @uid = uid, @customerid = custid };
                                    var SQL_cus = "  select  email,concat(firstname,'',lastname) as cusname,email from customermasters where customerid=@customerid";
                                    var resultcus = connection.Query<dynamic>(SQL_cus, parameters_cus);
                                    var obj_cus = resultcus.FirstOrDefault();
                                    cusemail = obj_cus.email;
                                    cusname = obj_cus.cusname;









                                    NpgsqlCommand inst_payerpayeeprivate = new NpgsqlCommand("INSERT INTO public.initiatorrecipientprivates(customerid, uid, type, firstname, lastname, email, mobile, geoid, cityid, pincode, bankaccountnumber, bankname, iban, accountname, status, createdby, createddate)VALUES(@customerid,@uid,@type,@firstname,@lastname,@email,@mobile,@geocode,@city,@pincode,@bankaccountnumber,@bankname,@iban,@accountname,@status,@createdby,@createddate)", connection);
                                    // inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", obj_initiatorrecipientprivate.customerid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", custid);
                                    //inst_payerpayeeprivate.Parameters.AddWithValue("@uid", obj_initiatorrecipientprivate.uid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@uid", uidd);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@type", obj_initiatorrecipientprivate.type);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@firstname", obj_initiatorrecipientprivate.firstname);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@lastname", obj_initiatorrecipientprivate.lastname);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@email", obj_initiatorrecipientprivate.email);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@mobile", obj_initiatorrecipientprivate.mobile);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@geocode", obj_initiatorrecipientprivate.geoid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@city", obj_initiatorrecipientprivate.cityid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@pincode", obj_initiatorrecipientprivate.pincode);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@bankaccountnumber", obj_initiatorrecipientprivate.bankaccountnumber);
                                    // inst_payerpayeeprivate.Parameters.AddWithValue("@brn", obj_initiatorrecipientprivate.b);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@bankname", obj_initiatorrecipientprivate.bankname);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@iban", obj_initiatorrecipientprivate.iban);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@accountname", obj_initiatorrecipientprivate.accountname);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@status", 'A');
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@createdby", custid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@createddate", DateTime.Now);
                                    // inst_payerpayeeprivate.Parameters.AddWithValue("@middlename", obj_initiatorrecipientprivate.middlename);
                                    // inst_payerpayeeprivate.Parameters.AddWithValue("@accounttype", obj_initiatorrecipientprivate.accounttype);
                                    //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_initiatorrecipientprivate.documnettype);
                                    //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_initiatorrecipientprivate.documentvalue);
                                    int resultpayerpayeeprivate = inst_payerpayeeprivate.ExecuteNonQuery();


                                    //NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into initiatorrecipientmappings(customerid, uid, recipientuid, status) values(@customerid, @uid, @recipientuid, @status)", connection);
                                    //inst_payerpayeemapping.Parameters.AddWithValue("@customerid", custid);
                                    //inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_initiatorrecipientprivate.uid);
                                    //inst_payerpayeemapping.Parameters.AddWithValue("@recipientuid", uidd);
                                    //inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                    //inst_payerpayeemapping.Parameters.AddWithValue("@createdby", custid);
                                    //inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                    ////  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_initiatorrecipientprivate.documnettype);
                                    ////inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_initiatorrecipientprivate.documentvalue);
                                    //int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();








                                    string subject = cusname+" " +"Registration Confirmation Requested";
                                    StringBuilder sb = new StringBuilder();
                                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                                    smtp.EnableSsl = true;
                                    smtp.UseDefaultCredentials = false;
                                    sb.Append("Hiya" + cusname + ",");
                                    sb.Append("<br/>");
                                    sb.Append("<br/>");
                                    sb.Append("Congratulations we have registered you succesfully on CarditNow.Click here to verify your email and set passcode.");
                                    //sb.Append("<b>");
                                    //sb.Append("");
                                    //sb.Append("</b>");
                                    sb.Append("<br/>");
                                    sb.Append("<br/>");
                                    sb.Append("Cheers,");
                                    sb.Append("<br/>");
                                    sb.Append("Carditnow");
                                    // SendEmail(email, subject, sb.ToString());
                                    Helper.Email(sb.ToString(), obj_initiatorrecipientprivate.email, cusname, subject);


                                    return "Success";
                                }


                            }
                           
                        }











                        connection.Close();


                        }
            
                    return "Success";
                




                }
                catch (Exception ex)
                {
                    _logger.LogError($"Service : Save_payerpayeePrivate(): {ex}");

                }
            }
            else { return "Customer id not avilable"; }
            return null;
        }

        // DELETE: initiatorrecipientprivate/5
        //delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
initiatorrecipientprivate obj_initiatorrecipientprivate = _context.initiatorrecipientprivates.Find(id);
_context.initiatorrecipientprivates.Remove(obj_initiatorrecipientprivate);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api initiatorrecipientprivates ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool initiatorrecipientprivate_Exists(int id)
        {
        try{
            return _context.initiatorrecipientprivates.Count(e => e.privateid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:initiatorrecipientprivate_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

