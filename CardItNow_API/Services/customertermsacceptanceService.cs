
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
using nTireBO.Models;
using carditnow.Services;
using Microsoft.SqlServer.Management.Smo;
using System.Net.Mail;
using Xamarin.Essentials;
using System.Net;

namespace carditnow.Services
{
    public class customertermsacceptanceService : IcustomertermsacceptanceService
    {
        private readonly IConfiguration Configuration;
        private readonly customertermsacceptanceContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomertermsacceptanceService _service;
        private ItokenService _tokenservice;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public customertermsacceptanceService(customertermsacceptanceContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor, ItokenService tokenservice)
        {
            Configuration = configuration;
            _context = context;
            _logger = logger;
            _tokenservice = tokenservice;
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

        // GET: service/customertermsacceptance
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customertermsacceptances()
        {
            _logger.LogInfo("Getting into Get_customertermsacceptances() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select pk_encode(a.customertermid) as pkcol,customertermid as value,status as label from GetTable(NULL::public.customertermsacceptances,@cid) a  WHERE  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_customertermsacceptances(): {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetListBy_customertermid(int customertermid)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_customertermid(int customertermid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_customertermid = new { @cid = cid, @uid = uid, @customertermid = customertermid };
                    var SQL = "select pk_encode(customertermid) as pkcol,customertermid as value,status as label,* from GetTable(NULL::public.customertermsacceptances,@cid) where customertermid = @customertermid";
                    var result = connection.Query<dynamic>(SQL, parameters_customertermid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_customertermid(int customertermid) \r\n {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_customertermsacceptance(string sid)
        {
            _logger.LogInfo("Getting into  Get_customertermsacceptance(string sid) api");
            int id = Helper.GetId(sid);
            return Get_customertermsacceptance(id);
        }
        // GET: customertermsacceptance/5
        //gets the screen record
        public dynamic Get_customertermsacceptance(int id)
        {
            var tokenString = "";
            _logger.LogInfo("Getting into Get_customertermsacceptance(int id) api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";

                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
                    var SQL = @"select pk_encode(a.customertermid) as pkcol,a.customertermid as pk,a.*,
u.email as customeriddesc,
t.termdetails as termiddesc,a.customerid,u.email as email,u.password as password
 from GetTable(NULL::public.customertermsacceptances,@cid) a 
 left join customermasters u on a.customerid=u.customerid
 left join termsmasters t on a.termid=t.termid
 where a.customertermid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_customertermsacceptance = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customertermsacceptances'";
                    var customertermsacceptance_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;

                    //shy
                    int cus = obj_customertermsacceptance.customerid;
                    string email = obj_customertermsacceptance.email;
                    string pass = obj_customertermsacceptance.password;
                    if (obj_customertermsacceptance.email.ToString() != null && obj_customertermsacceptance.email.ToString() != "" && obj_customertermsacceptance.password.ToString() != "" && obj_customertermsacceptance.password.ToString() != null)
                    {
                        LoginModel lo = new LoginModel();
                        UserCredential u = new UserCredential();

                        lo.email = obj_customertermsacceptance.email;
                        lo.Password = obj_customertermsacceptance.password;
                        lo.host = "";
                        var user = _tokenservice.Authenticate(lo);
                        if (user != null)
                        {
                            tokenString = _tokenservice.BuildToken(user);
                        }

                     

                    }






                        connection.Close();
                    connection.Dispose();
                    return (new { customertermsacceptance = obj_customertermsacceptance, token= tokenString,customertermsacceptance_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customertermsacceptance(int id)\r\n {ex}");
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
                    var SQL = @"select  pk_encode(a.customertermid) as pkcol,a.customertermid as pk,* ,customertermid as value,status as label  from GetTable(NULL::public.customertermsacceptances,@cid) a ";
                    if (condition != "") SQL += " and " + condition;
                    SQL += " order by status";
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
                    var SQL = @"select pk_encode(a.customertermid) as pkcol,a.customertermid as pk,a.*,
u.email as customeriddesc,
t.termdetails as termiddesc from GetTable(NULL::public.customertermsacceptances,@cid) a 
 left join customermasters u on a.customerid=u.customerid
 left join termsmasters t on a.termid=t.termid";
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
        public dynamic Save_customertermsacceptance(string token, customertermsacceptance obj_customertermsacceptance)
        {
            _logger.LogInfo("Saving: Save_customertermsacceptance(string token,customertermsacceptance obj_customertermsacceptance) ");
            try
            {
                string serr = "";
                int querytype = 0;
                string email = string.Empty;
                string cusname = string.Empty;
                string fromname = "hi5@carditnow.com";
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                //connection.Open();
                //using var transaction = connection.BeginTransaction();
                //_context.Database.UseTransaction(transaction);
                //customertermsacceptance table
                if (obj_customertermsacceptance.customertermid == 0 || obj_customertermsacceptance.customertermid == null || obj_customertermsacceptance.customertermid < 0)
                {
                    if (obj_customertermsacceptance.status == "" || obj_customertermsacceptance.status == null) obj_customertermsacceptance.status = "A";
                    //obj_customertermsacceptance.companyid=cid;
                    obj_customertermsacceptance.createdby = uid;
                    obj_customertermsacceptance.createddate = DateTime.Now;
                    _context.customertermsacceptances.Add((dynamic)obj_customertermsacceptance);
                    querytype = 1;
                }
                else
                {
                    //obj_customertermsacceptance.companyid=cid;
                    obj_customertermsacceptance.updatedby = uid;
                    obj_customertermsacceptance.updateddate = DateTime.Now;
                    _context.Entry(obj_customertermsacceptance).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customertermsacceptance).Property("createdby").IsModified = false;
                    _context.Entry(obj_customertermsacceptance).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api customertermsacceptances ");
                _context.SaveChanges();


                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                //Helper.AfterExecute(token, querytype, obj_customertermsacceptance, "customertermsacceptances", 0, obj_customertermsacceptance.customertermid, "", null, _logger);

                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_customerid = new { @cid = cid, @uid = uid, @customerid = obj_customertermsacceptance.customerid };
                    var SQL = "  select  email,concat(firstname,'',lastname) as cusname from customermasters where customerid=@customerid";
                    var result = connection.Query<dynamic>(SQL, parameters_customerid);
                    var obj_cutomerid = result.FirstOrDefault();
                    email = obj_cutomerid.email;
                    cusname = obj_cutomerid.cusname;
                
                    connection.Close(); 
                }



                string subject = "Welcome Greetings";
                StringBuilder sb = new StringBuilder();
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                sb.Append("Dear "+ cusname + ",");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Congratulations you have successfully onboard with CARDINOW ");
                //sb.Append("<b>");
                //sb.Append("");
                //sb.Append("</b>");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Cheers,");
                sb.Append("<br/>");
                sb.Append("Carditnow");
               // SendEmail(email, subject, sb.ToString());
                Helper.Email(sb.ToString(), email, cusname, subject);

                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_customertermsacceptance((int)obj_customertermsacceptance.customertermid);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customertermsacceptance(string token,customertermsacceptance obj_customertermsacceptance) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customertermsacceptance/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                {
                    _logger.LogInfo("Getting into Delete(int id) api");
                    customertermsacceptance obj_customertermsacceptance = _context.customertermsacceptances.Find(id);
                    _context.customertermsacceptances.Remove(obj_customertermsacceptance);
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _logger.LogInfo("remove api customertermsacceptances ");
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

        private bool customertermsacceptance_Exists(int id)
        {
            try
            {
                return _context.customertermsacceptances.Count(e => e.customertermid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:customertermsacceptance_Exists(int id) {ex}");
                return false;
            }
        }

        public dynamic customeracceptancetermscondition(Customeracceptanceterms model)
        {
            _logger.LogInfo("Getting into Forgot Passcode(string email) api");
            using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                NpgsqlCommand CheckExists_customer = new NpgsqlCommand("select max(termid) from customertermsacceptances where customerid='" + model.customerid + "'", connection);
                NpgsqlDataAdapter get_customer = new NpgsqlDataAdapter(CheckExists_customer);
                DataTable dt = new DataTable();
                get_customer.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        var t = dt.Rows[0][0].ToString();
                        if (int.Parse(t) < model.termid)
                        {

                            NpgsqlCommand update_loginPass_Code = new NpgsqlCommand("update customertermsacceptances set termid='" + model.termid + "',dateofacceptance='" + model.dateofacceptance + "',status='A',updatedby='"+model.customerid+"',updateddate='"+model.dateofacceptance+"' where customerid='" + model.customerid + "'", connection);
                            var update_result = update_loginPass_Code.ExecuteNonQuery().ToString();
                            if (int.Parse(update_result) > 0)
                            {
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "Temrs and Conditions updated successfully" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                        else
                        {
                            var result1 = new
                            {
                                status = "fail",
                                data = "",   /* Application-specific data would go here. */
                                message = "Old passcode and New passcode same" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }
                    }
                    else
                    {
                        NpgsqlCommand update_terms = new NpgsqlCommand("insert into customertermsacceptances(termid,version,customerid,dateofacceptance,status,createdby,createddate) values(@termid,@version,@customerid,@dateofacceptance,@status,@createdby,@createddate)", connection);
                        update_terms.Parameters.AddWithValue("@termid",model.termid);
                        update_terms.Parameters.AddWithValue("@version",0);
                        update_terms.Parameters.AddWithValue("@customerid",model.customerid);
                        update_terms.Parameters.AddWithValue("@dateofacceptance",model.dateofacceptance);
                        update_terms.Parameters.AddWithValue("@status","A");
                        update_terms.Parameters.AddWithValue("@createdby",model.customerid);
                        update_terms.Parameters.AddWithValue("@createddate",model.dateofacceptance);
                        var insert_result = update_terms.ExecuteNonQuery().ToString();
                        if (int.Parse(insert_result) > 0)
                        {
                            var result1 = new
                            {
                                status = "success",
                                data = "",   /* Application-specific data would go here. */
                                message = "Accept terms updated successfully" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }
                    }
                }
            }
            return null;
        }



        public void SendEmail(string toemail, string subject, string htmlString)
        {
            //string _fromemail = @"support@myskillstree.com";
            // string _password = @"SupMyST123";//

            //  string _fromemail = @"support@sunsmartglobal.com";
            //  string _password = @"ecqsufegzoucluji";

            string _fromemail = @"pprakash@sunsmartglobal.com";
            string _password = @"dtrhxbwaorkbtyia";

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_fromemail);
                message.To.Add(new MailAddress(toemail));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_fromemail, _password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                smtp.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  SendEmail \r\n {ex}");
                throw ex;
            }
        }

    }
}

