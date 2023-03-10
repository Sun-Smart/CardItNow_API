
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
using MailKit.Security;
using System.Net.Mail;
using MimeKit;
using System.Net;

namespace carditnow.Services
{
    public class customermasterService : IcustomermasterService
    {
        private readonly IConfiguration Configuration;
        private readonly customermasterContext _context;
        private readonly customerdetailContext _context_cd;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomermasterService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public customermasterService(customermasterContext context, customerdetailContext contextdb, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;
            _context = context;
            _context_cd = contextdb;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
            if (httpContextAccessor.HttpContext.User.Claims.Any())
            {
                //cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                //uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            }
        }

        // GET: service/customermaster
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customermasters()
        {
            _logger.LogInfo("Getting into Get_customermasters() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select pk_encode(a.customerid) as pkcol,customerid as value,uid as label from GetTable(NULL::public.customermasters,@cid) a  WHERE  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_customermasters(): {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetListBy_customerid(int customerid)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_customerid(int customerid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_customerid = new { @cid = cid, @uid = uid, @customerid = customerid };
                    var SQL = "select pk_encode(customerid) as pkcol,customerid as value,uid as label,* from GetTable(NULL::public.customermasters,@cid) where customerid = @customerid";
                    var result = connection.Query<dynamic>(SQL, parameters_customerid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_customerid(int customerid) \r\n {ex}");
                throw ex;
            }
        }

        //Generate Random number

        string token = "";
        int strOTP = 0;
        int fromuser = 0;
        int touser = 0;
        string tousername = string.Empty;
        string fromemailuser = "", toemailuser = "";
        private static Random random = new Random();
        //public static string RandomString()
        //{
        //    int strOTP = 0;
        //    const string chars = "0123456789";       
        //    strOTP = Convert.ToInt32(RandomString(6));

        //    int length = strOTP.ToString().Length;
        //    if (length <= 5)
        //    {
        //        goto Loop;

        //    }

        //}
        public decimal GetRandomNumber()
        {
            Random objRandom = new Random();
            int intValue = objRandom.Next(100000, 999999);
            return intValue;
        }

        public string SendOTP(string email)
        {
            //GetRandomNumber();
            bool OTPUpdated = false;
            int custid = 0;
            string custmid = string.Empty;
            int geoid = 0;
            string geoids = string.Empty;
            try
            {
               

                _logger.LogInfo("Getting into SendOTP(string email) api");

                decimal TACNo = GetRandomNumber();

                var customers = _context.customermasters.Where(x => x.email == email);
                if (customers != null && customers.Any())
                {
                    var dbEntry = _context.customermasters.Find(customers.FirstOrDefault().customerid);

                    if (dbEntry != null)
                    {
                        //dbEntry.otp = Convert.ToString(TACNo);
                        //dbEntry.updateddate = DateTime.Now;
                        //dbEntry.updatedby = 0;
                        //dbEntry.mode = "R";
                        //dbEntry.type = "c";
                        //dbEntry.status = "N";
                        //dbEntry.mobile = "000000";
                        //OTPUpdated = _context.SaveChanges() > 0;
                        return "Your account already register";
                    }

                }
                else
                {
                    var cus_master = new customermaster();
                    cus_master.email = email;
                    cus_master.createdby = 0;
                    cus_master.mode = "R";
                    cus_master.uid = "P" + DateTime.Now.Second.ToString();
                    cus_master.type = "c";
                    cus_master.status = "N";
                    cus_master.mobile = "000000";
                    cus_master.createddate = DateTime.Now;
                    cus_master.otp = TACNo.ToString();
                    _context.customermasters.Add(cus_master);
                    OTPUpdated = _context.SaveChanges() > 0;
                    //return "Success";
                }

                if (OTPUpdated)
                {
                    string subject = "CarditNow Registration OTP ";
                    StringBuilder sb = new StringBuilder();
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    sb.Append("Dear Customer,");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Your OTP to Continue the Registration: ");
                    sb.Append("<b>");
                    sb.Append(TACNo);
                    sb.Append("</b>");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Regards,");
                    sb.Append("<br/>");
                    sb.Append("SunSmart Global");
                    SendEmail(email, subject, sb.ToString());
                    //Helper.SendEmail();

                    
                    //return "Success";

                    //shy

                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {
                        var parameters_customerid = new { @cid = cid, @uid = uid, @email = email };
                        var SQL = "  select pk_encode(m.customerid) as pkcol,m.customerid,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                        var result = connection.Query<dynamic>(SQL, parameters_customerid);
                        var obj_cutomerid = result.FirstOrDefault();
                        custid = obj_cutomerid.customerid;
                        geoid = obj_cutomerid.geoid;
                        custmid = custid.ToString();
                        geoids = geoid.ToString();
                        connection.Close();
                    }

                    //end


                    var result1 = new
                    {
                        status = "success",
                        data = "",/* Application-specific data would go here. */
                        OTP = TACNo.ToString(),//SHY
                        customerid = custmid,//SHY
                        geoid= geoids,
                        message = "succesfully save record" /* Or optional success message */
                    };
                    return JsonConvert.SerializeObject(result1);
                }
                else
                {
                    var result1 = new
                    {
                        status = "success",
                        data = "",/* Application-specific data would go here. */
                        OTP = TACNo.ToString(),
                        customerid= custmid,
                        geoid= geoids,
                    message = "succesfully save record" /* Or optional success message */
                    };
                    return JsonConvert.SerializeObject(result1);


                    //return "Failed";
                }


                #region
                //using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                //{
                //    var parameters_customeremail = new { @cid = cid, @uid = uid, @customeremail = email }; 
                //    Helper.SendEmail("Login OTP", token, fromuser, touser, fromemailuser, toemailuser, strOTP.ToString(), _logger);

                //    connection.Close();
                //    connection.Dispose();
                //    //return (result);
                //    return "send";
                //}
                #endregion
            }

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            //return "success";
        }




        //new 16/2 shy
        public string SendOTP1(string email,string geoid)
        {
            //GetRandomNumber();
            bool OTPUpdated = false;
            int custid = 0;
            string custmid = string.Empty;
            int geonumber = 0;
            int geid = 0;
            string geoids = string.Empty;
            geid = Convert.ToInt32(geoid);
            int maxid = 0;
            string newValueString = string.Empty;
            string uidd = string.Empty;
            string cusname = string.Empty;
            try
            {


                _logger.LogInfo("Getting into SendOTP(string email) api");

                decimal TACNo = GetRandomNumber();

                var customers = _context.customermasters.Where(x => x.email == email);
                if (customers != null && customers.Any())
                {
                    var dbEntry = _context.customermasters.Find(customers.FirstOrDefault().customerid);

                    if (dbEntry != null)
                    {
                        //dbEntry.otp = Convert.ToString(TACNo);
                        //dbEntry.updateddate = DateTime.Now;
                        //dbEntry.updatedby = 0;
                        //dbEntry.mode = "R";
                        //dbEntry.type = "c";
                        //dbEntry.status = "N";
                        //dbEntry.mobile = "000000";
                        //OTPUpdated = _context.SaveChanges() > 0;
                        return "Your account already register";
                    }

                }
                else
                {

                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {
                        var parameters_customerid = new { @cid = cid, @uid = uid, @email = email };
                        var SQL = "select max(customerid) as maxid from customermasters";
                        var result = connection.Query<dynamic>(SQL, parameters_customerid);
                        var obj_cutomerid = result.FirstOrDefault();
                        maxid = obj_cutomerid.maxid;
                        maxid = maxid + 1;
                    }


                    if (geid==1)
                    {
                         newValueString ="U"+ maxid.ToString().PadLeft(8, '0');
                    }
                    if (geid==2)
                    {
                         newValueString ="P"+ maxid.ToString().PadLeft(8, '0');
                    }

                    // convert back to string with leading zero's
                    //string newValueString = maxid.ToString().PadLeft(8, '0');



                    var cus_master = new customermaster();
                    var cus_detail = new customerdetail();
                    cus_master.email = email;
                    cus_master.createdby = 0;
                    cus_master.mode = "I";
                    //cus_master.uid = "P" + DateTime.Now.Second.ToString();
                    cus_master.uid = newValueString;
                    cus_master.type = "I";
                    cus_master.status = "N";
                    cus_master.mobile = "000000";
                    cus_master.createddate = DateTime.Now;
                    cus_master.otp = TACNo.ToString();
                    cus_master.customervisible = false;
                    _context.customermasters.Add(cus_master);
                    OTPUpdated = _context.SaveChanges() > 0;
                    //cus_detail.geoid = geid;
                    //_context_cd.customerdetails.Add(cus_detail);
                    //_context_cd.SaveChanges();
                    //return "Success";
                }

                if (OTPUpdated)
                {
                    string subject = "CarditNow Registration OTP ";
                    StringBuilder sb = new StringBuilder();
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    sb.Append("Dear Customer,");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Your OTP to Continue the Registration: ");
                    sb.Append("<b>");
                    sb.Append(TACNo);
                    sb.Append("</b>");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Cheers,");
                    sb.Append("<br/>");
                    sb.Append("Carditnow");
                   // SendEmail(email, subject, sb.ToString());
                    //Helper.SendEmail();
                    //return "Success";

                    Helper.Email(sb.ToString(), email, cusname, subject);

                    //shy

                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {
                        var parameters_customerid = new { @cid = cid, @uid = uid, @email = email };
                        var SQL = "  select pk_encode(m.customerid) as pkcol,m.customerid,m.uid,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                        var result = connection.Query<dynamic>(SQL, parameters_customerid);
                        var obj_cutomerid = result.FirstOrDefault();
                        custid = obj_cutomerid.customerid;
                        uidd =obj_cutomerid.uid;
                        geonumber = obj_cutomerid.geoid;
                        custmid = custid.ToString();
                        geoids = geonumber.ToString();
                        connection.Close();
                    }
                    var cus_detail = new customerdetail();
                    cus_detail.geoid = geid;
                    cus_detail.customerid = custid;
                    cus_detail.uid = uidd.ToString();
                    _context_cd.customerdetails.Add(cus_detail);
                    _context_cd.SaveChanges();


                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {
                        var parameters_customerid = new { @cid = cid, @uid = uid, @email = email };
                        var SQL = "  select pk_encode(m.customerid) as pkcol,m.customerid,m.uid,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                        var result = connection.Query<dynamic>(SQL, parameters_customerid);
                        var obj_cutomerid = result.FirstOrDefault();
                        custid = obj_cutomerid.customerid;
                        uidd =obj_cutomerid.uid;
                        geonumber = obj_cutomerid.geoid;
                        custmid = custid.ToString();
                        geoids = geonumber.ToString();
                        connection.Close();
                    }



                    //end


                    var result1 = new
                    {
                        status = "success",
                        data = "",/* Application-specific data would go here. */
                        OTP = TACNo.ToString(),//SHY
                        customerid = custmid,//SHY
                        geoid = geoids,
                        message = "succesfully save record" /* Or optional success message */
                    };
                    return JsonConvert.SerializeObject(result1);
                }
                else
                {
                    var result1 = new
                    {
                        status = "success",
                        data = "",/* Application-specific data would go here. */
                        OTP = TACNo.ToString(),
                        customerid = custmid,
                        geoid = geoids,
                        message = "succesfully save record" /* Or optional success message */
                    };
                    return JsonConvert.SerializeObject(result1);


                    //return "Failed";
                }


                #region
                //using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                //{
                //    var parameters_customeremail = new { @cid = cid, @uid = uid, @customeremail = email }; 
                //    Helper.SendEmail("Login OTP", token, fromuser, touser, fromemailuser, toemailuser, strOTP.ToString(), _logger);

                //    connection.Close();
                //    connection.Dispose();
                //    //return (result);
                //    return "send";
                //}
                #endregion
            }

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            //return "success";
        }

        public string ReSendOTP(string email)
        {
            //GetRandomNumber();
            bool OTPUpdated = false;
            int custid = 0;
            string custmid = string.Empty;
            int geoid = 0;
            string geoids = string.Empty;
            int querytype = 0;
            string cusname = string.Empty;
            try
            {


                _logger.LogInfo("Getting into SendOTP(string email) api");

                decimal TACNo = GetRandomNumber();

                var customers = _context.customermasters.Where(x => x.email == email);
                if (customers != null && customers.Any())
                {

                    //customermaster cus_master = new customermaster();
                    ////cus_master.email = email;
                    ////cus_master.mode = "I";
                    ////cus_master.uid = "P" + DateTime.Now.Second.ToString();
                    ////cus_master.type = "I";
                    ////cus_master.status = "N";
                    ////cus_master.mobile = "000000";
                    //cus_master.updateddate = DateTime.Now;
                    //cus_master.otp = TACNo.ToString();
                    ////_context.customermasters.Add(cus_master);

                    //_context.Entry(cus_master).Property("email").IsModified = false;
                    //_context.Entry(cus_master).Property("mode").IsModified = false;
                    //_context.Entry(cus_master).Property("uid").IsModified = false;
                    //_context.Entry(cus_master).Property("type").IsModified = false;
                    //_context.Entry(cus_master).Property("status").IsModified = false;
                    //_context.Entry(cus_master).Property("mobile").IsModified = false;
                    //_context.Entry(cus_master).Property("otp").CurrentValue = TACNo.ToString();
                    ////cus_master.otp = TACNo.ToString();
                    //querytype = 2;
                    //_context.SaveChanges();



                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {
                        NpgsqlCommand customer_card = new NpgsqlCommand("update customermasters set otp=@otp where email=@email", connection);
                        connection.Open();
                    customer_card.Parameters.AddWithValue("@otp", TACNo.ToString());
                    customer_card.Parameters.AddWithValue("@email", email);
                    var output = customer_card.ExecuteNonQuery();







                    string subject = "CarditNow Registration OTP ";
                    StringBuilder sb = new StringBuilder();
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    sb.Append("Dear Customer,");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Your OTP to Continue the Registration: ");
                    sb.Append("<b>");
                    sb.Append(TACNo);
                    sb.Append("</b>");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Regards,");
                    sb.Append("<br/>");
                    sb.Append("SunSmart Global");
                    //SendEmail(email, subject, sb.ToString());
                        //Helper.SendEmail();
                        //return "Success";


                        Helper.Email(sb.ToString(), email, cusname, subject);
                        //shy

                        //using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                        //{
                        var parameters_customerid = new { @cid = cid, @uid = uid, @email = email };
                        var SQL = "  select pk_encode(m.customerid) as pkcol,m.customerid,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                        var result = connection.Query<dynamic>(SQL, parameters_customerid);
                        var obj_cutomerid = result.FirstOrDefault();
                        custid = obj_cutomerid.customerid;
                        geoid = obj_cutomerid.geoid;
                        custmid = custid.ToString();
                        geoids = geoid.ToString();
                        connection.Close();
                    }

                    //end


                    var result1 = new
                    {
                        status = "success",
                        data = "",/* Application-specific data would go here. */
                        OTP = TACNo.ToString(),//SHY
                        customerid = custmid,//SHY
                        geoid = geoids,
                        message = "succesfully save record" /* Or optional success message */
                    };
                    return JsonConvert.SerializeObject(result1);
                }
                else
                {
                    var result1 = new
                    {
                        status = "success",
                        data = "",/* Application-specific data would go here. */
                        OTP = "",
                        customerid = "",
                        geoid = "",
                        message = "Not a registred MailID" /* Or optional success message */
                    };
                    return JsonConvert.SerializeObject(result1);


                    //return "Failed";
                }


                #region
                //using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                //{
                //    var parameters_customeremail = new { @cid = cid, @uid = uid, @customeremail = email }; 
                //    Helper.SendEmail("Login OTP", token, fromuser, touser, fromemailuser, toemailuser, strOTP.ToString(), _logger);

                //    connection.Close();
                //    connection.Dispose();
                //    //return (result);
                //    return "send";
                //}
                #endregion
            }

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            //return "success";
        }



        public string PasswordSet(string email, string password)
        {
            try
            {
                _logger.LogInfo("Getting into SendOTP(string email) api");
                var customers = _context.customermasters.Where(x => x.email == email);
                if (customers != null && customers.Any())
                {
                    var dbEntry = _context.customermasters.Find(customers.FirstOrDefault().customerid);
                    if (dbEntry != null)
                    {
                        dbEntry.password = password.ToString();
                        dbEntry.tpin = password.ToString();
                        dbEntry.updateddate = DateTime.Now;
                        dbEntry.updatedby = 0;
                        dbEntry.mode = "R";
                        dbEntry.type = "c";
                        dbEntry.status = "N";
                        dbEntry.mobile = "000000";
                        _context.SaveChanges();
                        //return "Success";
                    }

                }
                else
                {
                    var cus_master = new customermaster();
                    cus_master.email = email;
                    cus_master.createdby = 0;
                    cus_master.mode = "R";
                    cus_master.uid = "P" + DateTime.Now.Second.ToString();
                    cus_master.type = "c";
                    cus_master.status = "N";
                    cus_master.mobile = "000000";
                    cus_master.createddate = DateTime.Now;
                    cus_master.password = password.ToString();
                    cus_master.tpin = password.ToString();
                    _context.customermasters.Add(cus_master);
                    //OTPUpdated = _context.SaveChanges() > 0;
                    _context.SaveChanges();
                    //return "Success";
                }
                //return "Success";
                var result1 = new
                {
                    status = "success",
                    data = "",   /* Application-specific data would go here. */
                    message = "succesfully save record" /* Or optional success message */
                };
                return JsonConvert.SerializeObject(result1);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }

        }

        public string SetPinConfig(string email, string pin)
        {
            try
            {
                _logger.LogInfo("Getting into Set passcode api");
                var customers = _context.customermasters.Where(x => x.email == email);
                if (customers != null && customers.Any())
                {
                    var dbEntry = _context.customermasters.Find(customers.FirstOrDefault().customerid);
                    if (dbEntry != null)
                    {
                        dbEntry.tpin = pin.ToString();
                        dbEntry.password = pin.ToString();
                        dbEntry.updateddate = DateTime.Now;
                        dbEntry.updatedby = 0;
                        dbEntry.mobile = "000000";
                        _context.SaveChanges();
                        //return "Success";
                    }

                }
                else
                {
                    var cus_master = new customermaster();
                    cus_master.email = email;
                    cus_master.createdby = 0;
                    cus_master.mode = "R";
                    cus_master.uid = "P" + DateTime.Now.Second.ToString();
                    cus_master.type = "c";
                    cus_master.mobile = "000000";
                    cus_master.createddate = DateTime.Now;
                    cus_master.tpin = pin.ToString();
                    cus_master.password = pin.ToString();
                    _context.customermasters.Add(cus_master);
                    //OTPUpdated = _context.SaveChanges() > 0;
                    _context.SaveChanges();
                    //return "Success";
                }
                //return "Success";
                var result1 = new
                {
                    status = "success",
                    data = "",   /* Application-specific data would go here. */
                    message = "succesfully save record" /* Or optional success message */
                };
                return JsonConvert.SerializeObject(result1);
                #region
                //using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                //{
                //    var parameters_customeremail = new { @cid = cid, @uid = uid, @customeremail = email }; 
                //    Helper.SendEmail("Login OTP", token, fromuser, touser, fromemailuser, toemailuser, strOTP.ToString(), _logger);

                //    connection.Close();
                //    connection.Dispose();
                //    //return (result);
                //    return "send";
                //}
                #endregion
            }

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            //return "success";
        }


        public string ProcessDocument(processdocument model)
        {
            try
            {
                _logger.LogInfo("Getting into ProcessDocument api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    var get_cuscode = @"select customerid from customermasters where email='" + model.email + "' ";
                    var result = connection.ExecuteScalar(get_cuscode, connection);
                    if ((result != null) || (Convert.ToInt32(result) > 0))
                    {
                        var get_customerdetails = @"select * from customerdetails where customerid='" + result + "'";
                        var result_customerdetails = connection.ExecuteScalar(get_customerdetails, connection);
                        if ((result_customerdetails != null) || (Convert.ToInt32(result_customerdetails) > 0))
                        {
                            //var update_customerdocumnet = @"update customerdetails set identificationdocumenttype='"+ doucumenttype + "',idnumber='"+ documentid + "'where customerdetailid='" + result_customerdetails + "' ";
                            //var result_updatedocument = connection.Query(update_customerdocumnet);

                            NpgsqlCommand inst_cd = new NpgsqlCommand("update customerdetails set identificationdocumenttype=@identificationdocumenttype,idnumber=@idnumber,updatedby=@updatedby,updateddate=@updateddate,livestockphoto=@document", connection);
                            inst_cd.Parameters.AddWithValue("@customerdetailid", result_customerdetails);
                            inst_cd.Parameters.AddWithValue("@identificationdocumenttype", model.documenttype);
                            inst_cd.Parameters.AddWithValue("@idnumber", model.documentid);
                            inst_cd.Parameters.AddWithValue("@document", model.document);
                            inst_cd.Parameters.AddWithValue("@updatedby", result_customerdetails);
                            inst_cd.Parameters.AddWithValue("@updateddate", DateTime.Now);
                            var output = inst_cd.ExecuteNonQuery();
                            if (output > 0)
                            {
                                //return "Success";
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "succesfully save record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                            else
                            {
                                //return "fail";
                                var result1 = new
                                {
                                    status = "fail",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "not succesfully record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                        else
                        {

                            //var Insert_customerdocument = "insert into customerdetails values";
                            NpgsqlCommand inst_cd = new NpgsqlCommand("insert into customerdetails (customerid,type,uid,identificationdocumenttype,idnumber,createdby,createddate) values(@customerid,@type,@uid,@identificationdocumenttype,@idnumber,@createdby,@createddate)", connection);
                            inst_cd.Parameters.AddWithValue("@customerid", result);
                            inst_cd.Parameters.AddWithValue("@type", 1);
                            inst_cd.Parameters.AddWithValue("@uid", 2);
                            inst_cd.Parameters.AddWithValue("@identificationdocumenttype", model.documenttype);
                            inst_cd.Parameters.AddWithValue("@idnumber", model.documentid);
                            inst_cd.Parameters.AddWithValue("@createdby", result);
                            inst_cd.Parameters.AddWithValue("@createddate", DateTime.Now);
                            var output = inst_cd.ExecuteNonQuery();
                            if (output > 0)
                            {
                                //return "Success";
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "succesfully save record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                            else
                            {
                                //return "fail";
                                var result1 = new
                                {
                                    status = "fail",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "not succesfully record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                    }
                    else
                    {
                        return "Customer not register in carditnow";
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


        public dynamic UpdateProfileInformation(ProfileInformationUpdate model)
        {
            try
            {

                _logger.LogInfo("Getting into UpdateProfileInformation api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                   
                    connection.Open();
                    var get_cuscode = @"select customerid from customermasters where email='" + model.email + "' ";
                    var result = connection.ExecuteScalar(get_cuscode, connection);
                    if ((result != null) || (Convert.ToInt32(result) > 0))
                    {

                        NpgsqlCommand update_cus = new NpgsqlCommand(@"update customermasters set firstname=@firstname,lastname=@lastname,mobile=@mobile,dob=@dob,nickname=@nickname where email=@email", connection);
                        update_cus.CommandType = CommandType.Text;
                        update_cus.Parameters.AddWithValue("@email", model.email);
                        update_cus.Parameters.AddWithValue("@firstname", model.firstname);
                        update_cus.Parameters.AddWithValue("@lastname", model.lastname);
                        update_cus.Parameters.AddWithValue("@mobile", model.mobile);
                        update_cus.Parameters.AddWithValue("@dob", model.dateofbirth);
                        update_cus.Parameters.AddWithValue("@nickname", model.nickname);
                        int rowAfftect = update_cus.ExecuteNonQuery();
                        var get_customerdetails = @"select * from customerdetails where customerid='" + result + "'";
                        var result_customerdetails = connection.ExecuteScalar(get_customerdetails, connection);
                        if ((result_customerdetails != null) || (Convert.ToInt32(result_customerdetails) > 0))
                        {
                            NpgsqlCommand update_cusdetails = new NpgsqlCommand(@"update customerdetails set address=@address,geoid=@geoid,cityid=@cityid,postalcode=@postalcode,
                             idissuedate=@idissudate,idexpirydate=@idexpirydate,createdby=@createdby,
                            createddate=@createddate,nickname=@nickname where customerid=@customerid", connection);
                            update_cus.CommandType = CommandType.Text;
                            update_cus.Parameters.AddWithValue("@customerid", result);
                            update_cus.Parameters.AddWithValue("@address", model.address);
                           // update_cus.Parameters.AddWithValue("@geoid", 1);
                            update_cus.Parameters.AddWithValue("@geoid", model.geoid);//shy
                            //update_cus.Parameters.AddWithValue("@cityid", 2);
                            update_cus.Parameters.AddWithValue("@cityid", model.cityid);//shy
                            update_cus.Parameters.AddWithValue("@postalcode", model.postalcode);
                            update_cus.Parameters.AddWithValue("@idissudate", model.idissuedate);
                            update_cus.Parameters.AddWithValue("@idexpirydate", model.idexpirydate);
                            update_cus.Parameters.AddWithValue("@nickname", model.nickname);
                            update_cus.Parameters.AddWithValue("@createdby", result);
                            update_cus.Parameters.AddWithValue("@createddate", DateTime.Now);
                            //connection.Open();
                            int customerdetailsUpdate = update_cus.ExecuteNonQuery();
                            if (customerdetailsUpdate > 0)
                            {
                                //return "Success";
                                var result1 = new
                                {
                                    status = "success",
                                    data = "null",/* Application-specific data would go here. */
                                    message = "succesfully saved record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                            else
                            {
                                //return "fail";
                                var result1 = new
                                {
                                    status = "fail",
                                    data = "null",/* Application-specific data would go here. */
                                    message = "not succesfully saved record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                        else
                        {

                            //var Insert_customerdocument = "insert into customerdetails values";
                            NpgsqlCommand inst_cd = new NpgsqlCommand("insert into customerdetails (customerid,address,geoid,cityid,postalcode,idissuedate,idexpirydate) values(@customerid,@address,@geoid,@cityid,@postalcode,@idissudate,@idexpirydate)", connection);
                            inst_cd.Parameters.AddWithValue("@customerid", result);
                            inst_cd.Parameters.AddWithValue("@address", model.address);
                            // update_cus.Parameters.AddWithValue("@geoid", 1);
                            inst_cd.Parameters.AddWithValue("@geoid", model.geoid);//shy
                            //update_cus.Parameters.AddWithValue("@cityid", 2);
                            inst_cd.Parameters.AddWithValue("@cityid", model.cityid);//shy
                            inst_cd.Parameters.AddWithValue("@postalcode", model.postalcode);
                            inst_cd.Parameters.AddWithValue("@idissudate", model.idissuedate);
                            inst_cd.Parameters.AddWithValue("@idexpirydate", model.idexpirydate);
                            //update_cus.Parameters.AddWithValue("@nickname", nickname);
                            inst_cd.Parameters.AddWithValue("@createdby", result);
                            inst_cd.Parameters.AddWithValue("@createddate", DateTime.Now);
                            var output = inst_cd.ExecuteNonQuery();
                            if (output > 0)
                            {
                                //return "Success";
                                var result1 = new
                                {
                                    status = "success",
                                    data = "null",/* Application-specific data would go here. */
                                    message = "succesfully saved record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                            else
                            {
                                //return "fail";
                                var result1 = new
                                {
                                    status = "fail",
                                    data = "null",/* Application-specific data would go here. */
                                    message = "not succesfully saved record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                    }
                    else
                    {
                        return "Customer not register in carditnow";
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

        //used in getting the record. parameter is encrypted id
        public dynamic Get_customermaster(string sid)
        {
            _logger.LogInfo("Getting into  Get_customermaster(string sid) api");
            int id = Helper.GetId(sid);
            return Get_customermaster(id);
        }
        // GET: customermaster/5
        //gets the screen record
        public dynamic Get_customermaster(int id)
        {
            _logger.LogInfo("Getting into Get_customermaster(int id) api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();
                    string wStatus = "NormalStatus";
                    string vmode = "mode";
                    string vcustomermastertype = "customermastertype";
                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus, @vmode = vmode, @vcustomermastertype = vcustomermastertype };
                    //var SQL = @"select pk_encode(a.customerid) as pkcol,a.customerid as pk,a.*,
                    //          d.configtext as modedesc,
                    //          t.configtext as typedesc,
                    //          s.avatarname as defaultavatardesc
                    //         from GetTable(NULL::public.customermasters,@cid) a 
                    //         left join boconfigvalues d on a.mode=d.configkey and @vmode=d.param
                    //         left join boconfigvalues t on a.type=t.configkey and                       @vcustomermastertype=t.param
                    //         left join avatarmasters s on a.defaultavatar=s.avatarid
                    //         where a.customerid=@id";


                    var SQL = @"select pk_encode(a.customerid) as pkcol,a.customerid as pk,a.*,
                              d.configtext as modedesc,
                              t.configtext as typedesc,
                              s.avatarname as defaultavatardesc,
							  de.address,g.geoname as country,
							  c.cityname as city
                             from GetTable(NULL::public.customermasters,@cid) a 
							 left join customerdetails de on a.customerid=de.customerid
                             left join boconfigvalues d on a.mode=d.configkey and @vmode=d.param
                             left join boconfigvalues t on a.type=t.configkey and
							 @vcustomermastertype=t.param
							 left join geographymasters g on g.geoid=de.geoid
							 left join citymasters c on c.cityid=de.cityid and c.geoid=g.geoid
                             left join avatarmasters s on a.defaultavatar=s.avatarid
                             where a.customerid=@id";


                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_customermaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customermasters'";
                    var customermaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { customermaster = obj_customermaster, customermaster_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customermaster(int id)\r\n {ex}");
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
                    var SQL = @"select  pk_encode(a.customerid) as pkcol,a.customerid as pk,* ,customerid as value,uid as label  from GetTable(NULL::public.customermasters,@cid) a ";
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
                    string vmode = "mode";
                    string vcustomermastertype = "customermastertype";
                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus, @vmode = vmode, @vcustomermastertype = vcustomermastertype };
                    var SQL = @"select pk_encode(a.customerid) as pkcol,a.customerid as pk,a.*,
                              d.configtext as modedesc,
                              t.configtext as typedesc,
s.avatarname as defaultavatardesc from GetTable(NULL::public.customermasters,@cid) a 
 left join boconfigvalues d on a.mode=d.configkey and @vmode=d.param
 left join boconfigvalues t on a.type=t.configkey and @vcustomermastertype=t.param
 left join avatarmasters s on a.defaultavatar=s.avatarid";
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
        public dynamic Save_customermaster(string token, customermaster obj_customermaster)
        {
            _logger.LogInfo("Saving: Save_customermaster(string token,customermaster obj_customermaster) ");
            try
            {
                string serr = "";
                int querytype = 0;
                //if (obj_customermaster.uid != null)
                //{
                //    var parametersuid = new { @cid = cid, @uid = uid, @cuid = obj_customermaster.uid, @customerid = obj_customermaster.customerid };
                //    if (Helper.Count("select count(*) from customermasters where   uid =  @cuid and (@customerid = 0 ||  @customerid = null ||  @customerid < 0 )", parametersuid) > 0) serr += "uid is unique\r\n";
                //}
                if (obj_customermaster.email != null)
                {
                    var parametersemail = new { @cid = cid, @uid = uid, @email = obj_customermaster.email, @customerid = obj_customermaster.customerid };
                    if (Helper.Count("select count(*) from customermasters where   email =  @email ", parametersemail) > 0) serr += "email is unique\r\n";

                    //and (@customerid = 0 ||  @customerid = null ||  @customerid < 0 )
                }
                if (obj_customermaster.mobile != null)
                {
                    var parametersmobile = new { @cid = cid, @uid = uid, @mobile = obj_customermaster.mobile, @customerid = obj_customermaster.customerid };
                    if (Helper.Count("select count(*) from customermasters where   mobile =  @mobile ", parametersmobile) > 0) serr += "mobile is unique\r\n";
                }
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                //connection.Open();
                //using var transaction = connection.BeginTransaction();
                //_context.Database.UseTransaction(transaction);
                //customermaster table


                //using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                //{
                //    var parameters_customerid = new { @cid = cid, @uid = uid, @email = email };
                //    var SQL = "select max(customerid) as maxid from customermasters";
                //    var result = connection.Query<dynamic>(SQL, parameters_customerid);
                //    var obj_cutomerid = result.FirstOrDefault();
                //    maxid = obj_cutomerid.maxid;
                //    maxid = maxid + 1;
                //}


                //if (geid == 1)
                //{
                //    newValueString = "U" + maxid.ToString().PadLeft(8, '0');
                //}
                //if (geid == 2)
                //{
                //    newValueString = "P" + maxid.ToString().PadLeft(8, '0');
                //}




                if (obj_customermaster.customerid == 0 || obj_customermaster.customerid == null || obj_customermaster.customerid < 0)
                {
                    if (obj_customermaster.status == "" || obj_customermaster.status == null) obj_customermaster.status = "A";
                    //obj_customermaster.companyid=cid;
                    obj_customermaster.createdby = uid;
                    obj_customermaster.createddate = DateTime.Now;
                    _context.customermasters.Add((dynamic)obj_customermaster);
                    querytype = 1;
                }
                else
                {
                    //obj_customermaster.companyid=cid;
                    obj_customermaster.updatedby = uid;
                    obj_customermaster.updateddate = DateTime.Now;
                    _context.Entry(obj_customermaster).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customermaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_customermaster).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api customermasters ");
                _context.SaveChanges();





                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                Helper.AfterExecute(token, querytype, obj_customermaster, "customermasters", 0, obj_customermaster.customerid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_customermaster((int)obj_customermaster.customerid);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customermaster(string token,customermaster obj_customermaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: customermaster/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                {
                    _logger.LogInfo("Getting into Delete(int id) api");
                    customermaster obj_customermaster = _context.customermasters.Find(id);
                    _context.customermasters.Remove(obj_customermaster);
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _logger.LogInfo("remove api customermasters ");
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

        private bool customermaster_Exists(int id)
        {
            try
            {
                return _context.customermasters.Count(e => e.customerid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:customermaster_Exists(int id) {ex}");
                return false;
            }
        }

        public dynamic Customerauth(customerauth model)
        {
            var sList = new List<customerlist>();
            try
            {
                _logger.LogInfo("Getting into Get Document Type api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand inst_cd = new NpgsqlCommand("select email,password from customermasters where email ='" + model.email + "' ", connection);
                    NpgsqlDataAdapter sda = new NpgsqlDataAdapter(inst_cd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            sList.Add(new customerlist
                            {
                                email = dr["email"].ToString(),
                                accescode = dr["password"].ToString()

                            });

                        }
                        var result1 = new
                        {
                            status = "success",
                            data = sList,   /* Application-specific data would go here. */
                            message = "succesfully get record" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }
                    else
                    {
                        var result1 = new
                        {
                            status = "fail",
                            data = sList,   /* Application-specific data would go here. */
                            message = "record not available" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }
                    connection.Close();
                    connection.Dispose();
                }
                //return sList;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            //return sList;
            return null;
        }

        public dynamic account_Suspend(suspendAccount model)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    var get_cuscode = @"select password from customermasters where customerid='" + model.customerid + "' ";
                    var result = connection.ExecuteScalar(get_cuscode, connection);
                    if(!string.IsNullOrEmpty(result.ToString()))
                    { 
                        if(model.passcode==result.ToString())
                        {
                            NpgsqlCommand update_customer_status = new NpgsqlCommand("update customermasters set status='S' where customerid='" + model.customerid + "'", connection);
                            var updateresult = update_customer_status.ExecuteNonQuery().ToString();
                            if(int.Parse(updateresult)>0)
                            {
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",/* Application-specific data would go here. */
                                    message = "Account suspended succesfully." /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                        else
                        {
                            var result1 = new
                            {
                                status = "fail",
                                data = "",/* Application-specific data would go here. */
                                message = "The customer passcode missmatch." /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }

                    }
                    else
                    {
                        var result1 = new
                        {
                            status = "fail",
                            data = "null",/* Application-specific data would go here. */
                            message = "Customer id is required." /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }

                }
            }
            catch(Exception ex)
            { }

                return null;
        }

        public dynamic account_Suspend_Reactive(suspendAccount model)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    var get_cuscode = @"select status from customermasters where customerid='" + model.customerid + "' ";
                    var result = connection.ExecuteScalar(get_cuscode, connection);
                    if (!string.IsNullOrEmpty(result.ToString()))
                    {
                        if (result.ToString()=="S")
                        {
                            NpgsqlCommand update_customer_status = new NpgsqlCommand("update customermasters set status='A' where customerid='" + model.customerid + "'", connection);
                            var updateresult = update_customer_status.ExecuteNonQuery().ToString();
                            if (int.Parse(updateresult) > 0)
                            {
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",/* Application-specific data would go here. */
                                    message = "Account suspended succesfully." /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                        else
                        {
                            var result1 = new
                            {
                                status = "fail",
                                data = "",/* Application-specific data would go here. */
                                message = "The customer account not suspended,please contact CarditNow tech team." /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }

                    }
                    else
                    {
                        var result1 = new
                        {
                            status = "fail",
                            data = "null",/* Application-specific data would go here. */
                            message = "Customer id is required." /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }

                }
            }
            catch (Exception ex)
            { }

            return null;
        }


        //shy

        public IEnumerable<Object> GetListdocument_bygeoid(string geoid)
        {
            try
            {
                dynamic result = "";
                _logger.LogInfo("Getting into  GetListdocument_bygeoid(string geoid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    if (geoid=="1")
                    {
                        var parameters_customerdetailid = new { @cid = cid, @uid = uid, @geoid = geoid };

                        var SQL = "select pk_encode(masterdataid)as pkcol,masterdataid as key,masterdatadescription as value from masterdatas where masterdatatypeid=9";
                         result = connection.Query<dynamic>(SQL, parameters_customerdetailid);
                        connection.Close();
                        connection.Dispose();
                        //return (result);
                    }
                    if (geoid == "2")
                    {
                        var parameters_customerdetailid = new { @cid = cid, @uid = uid, @geoid = geoid };

                        var SQL = "select pk_encode(masterdataid)as pkcol,masterdataid as key,masterdatadescription as value from masterdatas where masterdatatypeid=8";
                         result = connection.Query<dynamic>(SQL, parameters_customerdetailid);
                        connection.Close();
                        connection.Dispose();
                       // return (result);

                    }
                    return (result);



                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListdocument_bygeoid(string geoid) \r\n {ex}");
                throw ex;
            }
        }

        //end



        public IEnumerable<Object> GetsecurityQuestions()
        {
            try
            {
                _logger.LogInfo("Getting into  GetsecurityQuestions() api");

                int id = 0;
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    string wStatus = "NormalStatus";
                    string vmode = "mode";
                    string vcustomermastertype = "customermastertype";
                    var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus, @vmode = vmode, @vcustomermastertype = vcustomermastertype };
                    var SQL = @"select masterdataid as Questionid,masterdatadescription as Question from masterdatas where masterdatatypeid=1 and status='A'";
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







        public IEnumerable<Object> Getproviencedetail(int geoid)
        {
            try
            {
                _logger.LogInfo("Getting into  Getproviencedetail(int geoid) api");

                int id = 0;
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    string wStatus = "NormalStatus";
                    string vmode = "mode";
                    string vcustomermastertype = "customermastertype";
                    var parameters = new { @cid = cid, @uid = uid,@geoid=geoid };
                    var SQL = @"select pk_encode(a.provienceid) as pkcol,provienceid as value,name as label from GetTable(NULL::public.proviencemaster,@cid) a  WHERE a.geoid=@geoid and a.status='A'";
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


    }
}

