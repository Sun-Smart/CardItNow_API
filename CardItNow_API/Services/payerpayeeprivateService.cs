using carditnow.Models;
using CardItNow.interfaces;
using CardItNow.Models;
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using Org.BouncyCastle.Asn1.Ocsp;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CardItNow.Services
{
    public class payerpayeeprivateService : IpayerpayeeprivateService
    {
        private readonly IConfiguration Configuration;
        private readonly payerpayeeprivateContext _context;
        private readonly customermasterContext _cus_context;
        private readonly customerdetailContext _cd_context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        public payerpayeeprivateService(IConfiguration configuration, ILoggerManager logger, customermasterContext cus_context, customerdetailContext cd_context, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;
            _logger = logger;
            _cus_context = cus_context;
            _cd_context = cd_context;
            this.httpContextAccessor = objhttpContextAccessor;
            if (httpContextAccessor.HttpContext.User.Claims.Any())
            {
                //cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                // uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";

                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            }
        }


        public dynamic Save_payerpayeprivate(payerpayeeprivate obj_payerpayeeprivate)
        {
            if (obj_payerpayeeprivate.customerid != null)
            {
                int maxid = 0;
                string newValueString = string.Empty;
                string geoids = obj_payerpayeeprivate.geocode;
                int geid = 0;
                geid = Convert.ToInt32(geoids);
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
                        //if ((obj_payerpayeeprivate.customerid != null) &&
                        //    (!string.IsNullOrEmpty(obj_payerpayeeprivate.Type)))
                        //{


                        var customers = _cus_context.customermasters.Where(x => x.email == obj_payerpayeeprivate.email);
                        if (customers != null && customers.Any())
                        {
                            var dbEntry = _cus_context.customermasters.Find(customers.FirstOrDefault().customerid);

                            if (dbEntry != null)
                            {

                                var parameters_customerid1 = new { @cid = cid, @uid = uid, @email = obj_payerpayeeprivate.email };
                                var SQL1 = "  select pk_encode(m.customerid) as pkcol,m.customerid,m.uid,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                                var result1 = connection.Query<dynamic>(SQL1, parameters_customerid1);
                                var obj_cutomerid1 = result1.FirstOrDefault();
                                custid = obj_cutomerid1.customerid;
                                uidd = obj_cutomerid1.uid;

                                NpgsqlCommand upd_customermaster = new NpgsqlCommand("update customermasters set mode=@mode,type=@type,customervisible=@customervisible where customerid=@customerid", connection);
                                upd_customermaster.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                upd_customermaster.Parameters.AddWithValue("@mode", obj_payerpayeeprivate.type);
                                upd_customermaster.Parameters.AddWithValue("@type", obj_payerpayeeprivate.customertype);
                                upd_customermaster.Parameters.AddWithValue("@customervisible", obj_payerpayeeprivate.visibletoall);
                                int result_upd = upd_customermaster.ExecuteNonQuery();




                                NpgsqlCommand check_payer1 = new NpgsqlCommand("select count(*)  from payerpayeemapping where customerid = '" + obj_payerpayeeprivate.customerid + "' and uid = '" + obj_payerpayeeprivate.uid + "' and payeeuid = '" + uidd + "'", connection);
                                var output_result1 = check_payer1.ExecuteScalar().ToString();
                                if ((int.Parse(output_result1) == 0) || (output_result1 == null))
                                {





                                    NpgsqlCommand check_payer = new NpgsqlCommand("select count(*)  from PayerPayeePrivate where customerid = '" + obj_payerpayeeprivate.customerid + "' and bankname = '" + obj_payerpayeeprivate.bankname + "' and bankaccountnumber = '" + obj_payerpayeeprivate.bankaccountnumber + "'", connection);
                                    var output_result = check_payer.ExecuteScalar().ToString();


                                    if ((int.Parse(output_result) == 0) || (output_result == null))
                                    {

                                        var parameters_cus = new { @cid = cid, @uid = uid, @customerid = obj_payerpayeeprivate.customerid };
                                        var SQL_cus = "  select  email,concat(firstname,'',lastname) as cusname from customermasters where customerid=@customerid";
                                        var resultcus = connection.Query<dynamic>(SQL_cus, parameters_cus);
                                        var obj_cus = resultcus.FirstOrDefault();
                                        cusemail = obj_cus.email;
                                        cusname = obj_cus.cusname;








                                        NpgsqlCommand inst_payerpayeeprivate = new NpgsqlCommand("INSERT INTO public.payerpayeeprivate(customerid, uid, type, firstname, lastname, email, mobile, geocode, city, pincode, bankaccountnumber, brn, bankname, iban, accountname, status, createdby, createddate,middlename,accounttype)VALUES(@customerid,@uid,@type,@firstname,@lastname,@email,@mobile,@geocode,@city,@pincode,@bankaccountnumber,@brn,@bankname,@iban,@accountname,@status,@createdby,@createddate,@middlename,@accounttype)", connection);
                                        // inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", custid);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@uid", uidd);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@type", obj_payerpayeeprivate.type);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@firstname", obj_payerpayeeprivate.businessname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@lastname", obj_payerpayeeprivate.ContactName);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@email", obj_payerpayeeprivate.email);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@mobile", obj_payerpayeeprivate.mobile);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@geocode", obj_payerpayeeprivate.geocode);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@city", obj_payerpayeeprivate.city);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@pincode", obj_payerpayeeprivate.pincode);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@bankaccountnumber", obj_payerpayeeprivate.bankaccountnumber);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@brn", obj_payerpayeeprivate.brn);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@bankname", obj_payerpayeeprivate.bankname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@iban", obj_payerpayeeprivate.iban);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@accountname", obj_payerpayeeprivate.accountname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@status", 'A');
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@middlename", obj_payerpayeeprivate.middlename);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@accounttype", obj_payerpayeeprivate.accounttype);
                                        //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                        int resultpayerpayeeprivate = inst_payerpayeeprivate.ExecuteNonQuery();


                                        NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into payerpayeemapping(customerid, uid, payeeuid, status) values(@customerid, @uid, @payeeuid, @status)", connection);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@payeeuid", uidd);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                        int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();



                                        if (resultpayerpayeeprivate > 0 && resultpayerpayeemapping > 0)
                                        {


                                            string subject2 = "Payee details are verified";
                                            StringBuilder sb2 = new StringBuilder();
                                            SmtpClient smtp2 = new SmtpClient();
                                            smtp2.EnableSsl = true;
                                            smtp2.UseDefaultCredentials = false;
                                            sb2.Append("Hi " + cusname + ",");
                                            sb2.Append("<br/>");
                                            sb2.Append("<br/>");
                                            sb2.Append("Congratulations,we have verified your payee details and you are ready to make payment.Go to start new payment on your APP or click here https://demo.herbie.ai/carditnow/#/ ");
                                            //sb2.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            //sb.Append("");
                                            //sb.Append("</b>");
                                            sb2.Append("<br/>");
                                            sb2.Append("<br/>");
                                            sb2.Append("Cheers,");
                                            sb2.Append("<br/>");
                                            sb2.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb2.ToString(), cusemail, cusname, subject2);



                                            string subject3 = "Successfully registered as a payee";
                                            StringBuilder sb3 = new StringBuilder();
                                            SmtpClient smtp3 = new SmtpClient();
                                            smtp3.EnableSsl = true;
                                            smtp3.UseDefaultCredentials = false;
                                            sb3.Append("Hi " + obj_payerpayeeprivate.ContactName + ",");
                                            sb3.Append("<br/>");
                                            sb3.Append("<br/>");
                                            sb3.Append("Congratulations you have successfully registered as a payee by " + cusname + " .Yoy have need to verify your account.Kindly click here https://demo.herbie.ai/carditnow/#/ ");
                                            //sb3.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            sb3.Append("to download the app");
                                            //sb.Append("</b>");
                                            sb3.Append("<br/>");
                                            sb3.Append("<br/>");
                                            sb3.Append("Cheers,");
                                            sb3.Append("<br/>");
                                            sb3.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb3.ToString(), obj_payerpayeeprivate.email, obj_payerpayeeprivate.ContactName, subject3);
                                            return "Success";
                                        }


                                    }
                                    else
                                    {
                                        NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into payerpayeemapping(customerid, uid, payeeuid, status) values(@customerid, @uid, @payeeuid, @status)", connection);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@payeeuid", uidd);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                        int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();
                                        if (resultpayerpayeemapping > 0)
                                        {
                                            string subject = "Payee details are verified";
                                            StringBuilder sb = new StringBuilder();
                                            SmtpClient smtp = new SmtpClient();
                                            smtp.EnableSsl = true;
                                            smtp.UseDefaultCredentials = false;
                                            sb.Append("Hi " + cusname + ",");
                                            sb.Append("<br/>");
                                            sb.Append("<br/>");
                                            sb.Append("Congratulations,we have verifiedyour payee details and you are ready to make payment.Go to start new payment on your APP or click here https://demo.herbie.ai/carditnow/#/");
                                            // sb.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            //sb.Append("");
                                            //sb.Append("</b>");
                                            sb.Append("<br/>");
                                            sb.Append("<br/>");
                                            sb.Append("Cheers,");
                                            sb.Append("<br/>");
                                            sb.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb.ToString(), cusemail, cusname, subject);



                                            string subject1 = "Successfully registered as a payee";
                                            StringBuilder sb1 = new StringBuilder();
                                            SmtpClient smtp1 = new SmtpClient();
                                            smtp1.EnableSsl = true;
                                            smtp1.UseDefaultCredentials = false;
                                            sb1.Append("Hi " + obj_payerpayeeprivate.ContactName + ",");
                                            sb1.Append("<br/>");
                                            sb1.Append("<br/>");
                                            sb1.Append("Congratulations you have successfully registered as a payee by " + obj_payerpayeeprivate.ContactName + " .Yoy have need to verify your account.Kindly click here https://demo.herbie.ai/carditnow/#/");
                                            // sb1.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            sb1.Append("to download the app");
                                            //sb.Append("</b>");
                                            sb1.Append("<br/>");
                                            sb1.Append("<br/>");
                                            sb1.Append("Cheers,");
                                            sb1.Append("<br/>");
                                            sb1.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb1.ToString(), obj_payerpayeeprivate.email, obj_payerpayeeprivate.ContactName, subject1);

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
                                cus_master.email = obj_payerpayeeprivate.email;
                                cus_master.createdby = 0;
                                cus_master.mode = obj_payerpayeeprivate.type;
                                //cus_master.uid = "P" + DateTime.Now.Second.ToString();
                                cus_master.uid = newValueString;
                                cus_master.type = obj_payerpayeeprivate.customertype;
                                cus_master.status = "N";
                                cus_master.mobile = obj_payerpayeeprivate.mobile;
                                cus_master.createddate = DateTime.Now;
                                cus_master.customervisible = obj_payerpayeeprivate.visibletoall;
                                // cus_master.otp = TACNo.ToString();
                                cus_master.customervisible = false;
                                _cus_context.customermasters.Add(cus_master);
                                _cus_context.SaveChanges();



                                var parameters_customerid1 = new { @cid = cid, @uid = uid, @email = obj_payerpayeeprivate.email };
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




                                NpgsqlCommand check_payer1 = new NpgsqlCommand("select count(*)  from payerpayeemapping where customerid = '" + obj_payerpayeeprivate.customerid + "' and uid = '" + obj_payerpayeeprivate.uid + "' and payeeuid = '" + uidd + "'", connection);
                                var output_result1 = check_payer1.ExecuteScalar().ToString();
                                if ((int.Parse(output_result1) == 0) || (output_result1 == null))
                                {





                                    NpgsqlCommand check_payer = new NpgsqlCommand("select count(*)  from PayerPayeePrivate where customerid = '" + obj_payerpayeeprivate.customerid + "' and bankname = '" + obj_payerpayeeprivate.bankname + "' and bankaccountnumber = '" + obj_payerpayeeprivate.bankaccountnumber + "'", connection);
                                    var output_result = check_payer.ExecuteScalar().ToString();


                                    if ((int.Parse(output_result) == 0) || (output_result == null))
                                    {

                                        var parameters_cus = new { @cid = cid, @uid = uid, @customerid = obj_payerpayeeprivate.customerid };
                                        var SQL_cus = "  select  email,concat(firstname,'',lastname) as cusname from customermasters where customerid=@customerid";
                                        var resultcus = connection.Query<dynamic>(SQL_cus, parameters_cus);
                                        var obj_cus = resultcus.FirstOrDefault();
                                        cusemail = obj_cus.email;
                                        cusname = obj_cus.cusname;








                                        NpgsqlCommand inst_payerpayeeprivate = new NpgsqlCommand("INSERT INTO public.payerpayeeprivate(customerid, uid, type, firstname, lastname, email, mobile, geocode, city, pincode, bankaccountnumber, brn, bankname, iban, accountname, status, createdby, createddate,middlename,accounttype)VALUES(@customerid,@uid,@type,@firstname,@lastname,@email,@mobile,@geocode,@city,@pincode,@bankaccountnumber,@brn,@bankname,@iban,@accountname,@status,@createdby,@createddate,@middlename,@accounttype)", connection);
                                        // inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", custid);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@uid", uidd);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@type", obj_payerpayeeprivate.type);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@firstname", obj_payerpayeeprivate.businessname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@lastname", obj_payerpayeeprivate.ContactName);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@email", obj_payerpayeeprivate.email);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@mobile", obj_payerpayeeprivate.mobile);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@geocode", obj_payerpayeeprivate.geocode);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@city", obj_payerpayeeprivate.city);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@pincode", obj_payerpayeeprivate.pincode);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@bankaccountnumber", obj_payerpayeeprivate.bankaccountnumber);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@brn", obj_payerpayeeprivate.brn);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@bankname", obj_payerpayeeprivate.bankname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@iban", obj_payerpayeeprivate.iban);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@accountname", obj_payerpayeeprivate.accountname);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@status", 'A');
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@middlename", obj_payerpayeeprivate.middlename);
                                        inst_payerpayeeprivate.Parameters.AddWithValue("@accounttype", obj_payerpayeeprivate.accounttype);
                                        //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                        int resultpayerpayeeprivate = inst_payerpayeeprivate.ExecuteNonQuery();


                                        NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into payerpayeemapping(customerid, uid, payeeuid, status) values(@customerid, @uid, @payeeuid, @status)", connection);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@payeeuid", uidd);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                        int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();



                                        if (resultpayerpayeeprivate > 0 && resultpayerpayeemapping > 0)
                                        {


                                            string subject2 = "Payee details are verified";
                                            StringBuilder sb2 = new StringBuilder();
                                            SmtpClient smtp2 = new SmtpClient();
                                            smtp2.EnableSsl = true;
                                            smtp2.UseDefaultCredentials = false;
                                            sb2.Append("Hi " + cusname + ",");
                                            sb2.Append("<br/>");
                                            sb2.Append("<br/>");
                                            sb2.Append("Congratulations,we have verified your payee details and you are ready to make payment.Go to start new payment on your APP or click here https://demo.herbie.ai/carditnow/#/ ");
                                            //sb2.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            //sb.Append("");
                                            //sb.Append("</b>");
                                            sb2.Append("<br/>");
                                            sb2.Append("<br/>");
                                            sb2.Append("Cheers,");
                                            sb2.Append("<br/>");
                                            sb2.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb2.ToString(), cusemail, cusname, subject2);



                                            string subject3 = "Successfully registered as a payee";
                                            StringBuilder sb3 = new StringBuilder();
                                            SmtpClient smtp3 = new SmtpClient();
                                            smtp3.EnableSsl = true;
                                            smtp3.UseDefaultCredentials = false;
                                            sb3.Append("Hi " + obj_payerpayeeprivate.ContactName + ",");
                                            sb3.Append("<br/>");
                                            sb3.Append("<br/>");
                                            sb3.Append("Congratulations you have successfully registered as a payee by " + cusname + " .Yoy have need to verify your account.Kindly click here https://demo.herbie.ai/carditnow/#/ ");
                                            //sb3.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            sb3.Append("to download the app");
                                            //sb.Append("</b>");
                                            sb3.Append("<br/>");
                                            sb3.Append("<br/>");
                                            sb3.Append("Cheers,");
                                            sb3.Append("<br/>");
                                            sb3.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb3.ToString(), obj_payerpayeeprivate.email, obj_payerpayeeprivate.ContactName, subject3);
                                            return "Success";
                                        }


                                    }
                                    else
                                    {
                                        NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into payerpayeemapping(customerid, uid, payeeuid, status) values(@customerid, @uid, @payeeuid, @status)", connection);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@payeeuid", uidd);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                        inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                        //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                        //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                        int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();
                                        if (resultpayerpayeemapping > 0)
                                        {
                                            string subject = "Payee details are verified";
                                            StringBuilder sb = new StringBuilder();
                                            SmtpClient smtp = new SmtpClient();
                                            smtp.EnableSsl = true;
                                            smtp.UseDefaultCredentials = false;
                                            sb.Append("Hi " + cusname + ",");
                                            sb.Append("<br/>");
                                            sb.Append("<br/>");
                                            sb.Append("Congratulations,we have verifiedyour payee details and you are ready to make payment.Go to start new payment on your APP or click here https://demo.herbie.ai/carditnow/#/");
                                            // sb.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            //sb.Append("");
                                            //sb.Append("</b>");
                                            sb.Append("<br/>");
                                            sb.Append("<br/>");
                                            sb.Append("Cheers,");
                                            sb.Append("<br/>");
                                            sb.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb.ToString(), cusemail, cusname, subject);



                                            string subject1 = "Successfully registered as a payee";
                                            StringBuilder sb1 = new StringBuilder();
                                            SmtpClient smtp1 = new SmtpClient();
                                            smtp1.EnableSsl = true;
                                            smtp1.UseDefaultCredentials = false;
                                            sb1.Append("Hi " + obj_payerpayeeprivate.ContactName + ",");
                                            sb1.Append("<br/>");
                                            sb1.Append("<br/>");
                                            sb1.Append("Congratulations you have successfully registered as a payee by " + obj_payerpayeeprivate.ContactName + " .Yoy have need to verify your account.Kindly click here https://demo.herbie.ai/carditnow/#/");
                                            // sb1.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                            sb1.Append("to download the app");
                                            //sb.Append("</b>");
                                            sb1.Append("<br/>");
                                            sb1.Append("<br/>");
                                            sb1.Append("Cheers,");
                                            sb1.Append("<br/>");
                                            sb1.Append("Carditnow");
                                            // SendEmail(email, subject, sb.ToString());
                                            Helper.Email(sb1.ToString(), obj_payerpayeeprivate.email, obj_payerpayeeprivate.ContactName, subject1);

                                            return "Success";
                                        }

                                    }

                                }
                                else
                                {
                                    return "The Given Payee already availbale in your My payee list ";
                                }



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
                            cus_master.email = obj_payerpayeeprivate.email;
                            cus_master.createdby = 0;
                            cus_master.mode = obj_payerpayeeprivate.type;
                            //cus_master.uid = "P" + DateTime.Now.Second.ToString();
                            cus_master.uid = newValueString;
                            cus_master.type = obj_payerpayeeprivate.customertype;
                            cus_master.status = "N";
                            cus_master.mobile = obj_payerpayeeprivate.mobile;
                            cus_master.createddate = DateTime.Now;
                            cus_master.customervisible = obj_payerpayeeprivate.visibletoall;
                            // cus_master.otp = TACNo.ToString();
                            cus_master.customervisible = false;
                            _cus_context.customermasters.Add(cus_master);
                            _cus_context.SaveChanges();



                            var parameters_customerid1 = new { @cid = cid, @uid = uid, @email = obj_payerpayeeprivate.email };
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





                            NpgsqlCommand check_payer1 = new NpgsqlCommand("select count(*)  from payerpayeemapping where customerid = '" + obj_payerpayeeprivate.customerid + "' and uid = '" + obj_payerpayeeprivate.uid + "' and payeeuid = '" + uidd + "'", connection);
                            var output_result1 = check_payer1.ExecuteScalar().ToString();
                            if ((int.Parse(output_result1) == 0) || (output_result1 == null))
                            {





                                NpgsqlCommand check_payer = new NpgsqlCommand("select count(*)  from PayerPayeePrivate where customerid = '" + obj_payerpayeeprivate.customerid + "' and bankname = '" + obj_payerpayeeprivate.bankname + "' and bankaccountnumber = '" + obj_payerpayeeprivate.bankaccountnumber + "'", connection);
                                var output_result = check_payer.ExecuteScalar().ToString();


                                if ((int.Parse(output_result) == 0) || (output_result == null))
                                {

                                    var parameters_cus = new { @cid = cid, @uid = uid, @customerid = obj_payerpayeeprivate.customerid };
                                    var SQL_cus = "  select  email,concat(firstname,'',lastname) as cusname from customermasters where customerid=@customerid";
                                    var resultcus = connection.Query<dynamic>(SQL_cus, parameters_cus);
                                    var obj_cus = resultcus.FirstOrDefault();
                                    cusemail = obj_cus.email;
                                    cusname = obj_cus.cusname;








                                    NpgsqlCommand inst_payerpayeeprivate = new NpgsqlCommand("INSERT INTO public.payerpayeeprivate(customerid, uid, type, firstname, lastname, email, mobile, geocode, city, pincode, bankaccountnumber, brn, bankname, iban, accountname, status, createdby, createddate,middlename,accounttype)VALUES(@customerid,@uid,@type,@firstname,@lastname,@email,@mobile,@geocode,@city,@pincode,@bankaccountnumber,@brn,@bankname,@iban,@accountname,@status,@createdby,@createddate,@middlename,@accounttype)", connection);
                                    // inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", custid);
                                    //inst_payerpayeeprivate.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@uid", uidd);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@type", obj_payerpayeeprivate.type);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@firstname", obj_payerpayeeprivate.businessname);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@lastname", obj_payerpayeeprivate.ContactName);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@email", obj_payerpayeeprivate.email);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@mobile", obj_payerpayeeprivate.mobile);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@geocode", obj_payerpayeeprivate.geocode);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@city", obj_payerpayeeprivate.city);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@pincode", obj_payerpayeeprivate.pincode);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@bankaccountnumber", obj_payerpayeeprivate.bankaccountnumber);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@brn", obj_payerpayeeprivate.brn);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@bankname", obj_payerpayeeprivate.bankname);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@iban", obj_payerpayeeprivate.iban);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@accountname", obj_payerpayeeprivate.accountname);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@status", 'A');
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@createddate", DateTime.Now);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@middlename", obj_payerpayeeprivate.middlename);
                                    inst_payerpayeeprivate.Parameters.AddWithValue("@accounttype", obj_payerpayeeprivate.accounttype);
                                    //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                    //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                    int resultpayerpayeeprivate = inst_payerpayeeprivate.ExecuteNonQuery();


                                    NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into payerpayeemapping(customerid, uid, payeeuid, status) values(@customerid, @uid, @payeeuid, @status)", connection);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@payeeuid", uidd);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                    inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                    //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                    //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                    int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();



                                    if (resultpayerpayeeprivate > 0 && resultpayerpayeemapping > 0)
                                    {


                                        string subject2 = "Payee details are verified";
                                        StringBuilder sb2 = new StringBuilder();
                                        SmtpClient smtp2 = new SmtpClient();
                                        smtp2.EnableSsl = true;
                                        smtp2.UseDefaultCredentials = false;
                                        sb2.Append("Hi " + cusname + ",");
                                        sb2.Append("<br/>");
                                        sb2.Append("<br/>");
                                        sb2.Append("Congratulations,we have verified your payee details and you are ready to make payment.Go to start new payment on your APP or click here https://demo.herbie.ai/carditnow/#/ ");
                                        //sb2.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                        //sb.Append("");
                                        //sb.Append("</b>");
                                        sb2.Append("<br/>");
                                        sb2.Append("<br/>");
                                        sb2.Append("Cheers,");
                                        sb2.Append("<br/>");
                                        sb2.Append("Carditnow");
                                        // SendEmail(email, subject, sb.ToString());
                                        Helper.Email(sb2.ToString(), cusemail, cusname, subject2);



                                        string subject3 = "Successfully registered as a payee";
                                        StringBuilder sb3 = new StringBuilder();
                                        SmtpClient smtp3 = new SmtpClient();
                                        smtp3.EnableSsl = true;
                                        smtp3.UseDefaultCredentials = false;
                                        sb3.Append("Hi " + obj_payerpayeeprivate.ContactName + ",");
                                        sb3.Append("<br/>");
                                        sb3.Append("<br/>");
                                        sb3.Append("Congratulations you have successfully registered as a payee by " + cusname + " .Yoy have need to verify your account.Kindly click here https://demo.herbie.ai/carditnow/#/ ");
                                        //sb3.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                        sb3.Append("to download the app");
                                        //sb.Append("</b>");
                                        sb3.Append("<br/>");
                                        sb3.Append("<br/>");
                                        sb3.Append("Cheers,");
                                        sb3.Append("<br/>");
                                        sb3.Append("Carditnow");
                                        // SendEmail(email, subject, sb.ToString());
                                        Helper.Email(sb3.ToString(), obj_payerpayeeprivate.email, obj_payerpayeeprivate.ContactName, subject3);
                                        return "Success";
                                    }


                                }
                                else
                                {
                                    NpgsqlCommand inst_payerpayeemapping = new NpgsqlCommand("insert into payerpayeemapping(customerid, uid, payeeuid, status) values(@customerid, @uid, @payeeuid, @status)", connection);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@uid", obj_payerpayeeprivate.uid);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@payeeuid", uidd);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@status", 'A');
                                    inst_payerpayeemapping.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                    inst_payerpayeemapping.Parameters.AddWithValue("@createddate", DateTime.Now);
                                    //  inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype", obj_payerpayeeprivate.documnettype);
                                    //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                    int resultpayerpayeemapping = inst_payerpayeemapping.ExecuteNonQuery();
                                    if (resultpayerpayeemapping > 0)
                                    {
                                        string subject = "Payee details are verified";
                                        StringBuilder sb = new StringBuilder();
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.EnableSsl = true;
                                        smtp.UseDefaultCredentials = false;
                                        sb.Append("Hi " + cusname + ",");
                                        sb.Append("<br/>");
                                        sb.Append("<br/>");
                                        sb.Append("Congratulations,we have verifiedyour payee details and you are ready to make payment.Go to start new payment on your APP or click here https://demo.herbie.ai/carditnow/#/");
                                        // sb.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                        //sb.Append("");
                                        //sb.Append("</b>");
                                        sb.Append("<br/>");
                                        sb.Append("<br/>");
                                        sb.Append("Cheers,");
                                        sb.Append("<br/>");
                                        sb.Append("Carditnow");
                                        // SendEmail(email, subject, sb.ToString());
                                        Helper.Email(sb.ToString(), cusemail, cusname, subject);



                                        string subject1 = "Successfully registered as a payee";
                                        StringBuilder sb1 = new StringBuilder();
                                        SmtpClient smtp1 = new SmtpClient();
                                        smtp1.EnableSsl = true;
                                        smtp1.UseDefaultCredentials = false;
                                        sb1.Append("Hi " + obj_payerpayeeprivate.ContactName + ",");
                                        sb1.Append("<br/>");
                                        sb1.Append("<br/>");
                                        sb1.Append("Congratulations you have successfully registered as a payee by " + obj_payerpayeeprivate.ContactName + " .Yoy have need to verify your account.Kindly click here https://demo.herbie.ai/carditnow/#/");
                                        // sb1.Append("<a href=https://demo.herbie.ai/myskillstree/#/login>");
                                        sb1.Append("to download the app");
                                        //sb.Append("</b>");
                                        sb1.Append("<br/>");
                                        sb1.Append("<br/>");
                                        sb1.Append("Cheers,");
                                        sb1.Append("<br/>");
                                        sb1.Append("Carditnow");
                                        // SendEmail(email, subject, sb.ToString());
                                        Helper.Email(sb1.ToString(), obj_payerpayeeprivate.email, obj_payerpayeeprivate.ContactName, subject1);

                                        return "Success";
                                    }

                                }

                            }
                            else
                            {
                                return "The Given Payee already availbale in your My payee list ";
                            }
                        }
                       // }
                       // else
                       // {
                            //return "Document type/ customer ID missing  ";
                       // }
                        connection.Close();
                        connection.Dispose();
                        return null;// result;
                    }

                   


                }
                catch (Exception ex)
                {
                    _logger.LogError($"Service : Save_payerpayeePrivate(): {ex}");

                }
            }
            else { return "Customer id not avilable"; }
            return null;
        }

        public dynamic Get_rawresult()
        {
            var result = new
            {
                Owner = "ANTONIN LAVYS. INGLES, Jr.and/or MARY ROSE C.INGLES",
                Resident = "GERALDINE Q.GALINATO",
                Address="Deelling house,Lot6,Block20,Royal South,Town Homes,",
                StartDate="20-05-2007",
                EndDate="20-05-2008",
                Rent="6500.00"

            };
            return JsonConvert.SerializeObject(result);
        }

        public dynamic MaskedNumber(string source)
        {
            StringBuilder sb = new StringBuilder(source);

            const int skipLeft = 6;
            const int skipRight = 4;

            int left = -1;

            for (int i = 0, c = 0; i < sb.Length; ++i)
            {
                if (Char.IsDigit(sb[i]))
                {
                    c += 1;

                    if (c > skipLeft)
                    {
                        left = i;

                        break;
                    }
                }
            }

            for (int i = sb.Length - 1, c = 0; i >= left; --i)
                if (Char.IsDigit(sb[i]))
                {
                    c += 1;

                    if (c > skipRight)
                        sb[i] = 'X';
                }

            return sb.ToString();
        }






        public dynamic MaskedNumber1(string source)
        {
            StringBuilder sb = new StringBuilder(source);

            const int skipLeft = 0;
            const int skipRight = 4;

            int left = -1;

            for (int i = 0, c = 0; i < sb.Length; ++i)
            {
                if (Char.IsDigit(sb[i]))
                {
                    c += 1;

                    if (c > skipLeft)
                    {
                        left = i;

                        break;
                    }
                }
            }

            for (int i = sb.Length - 1, c = 0; i >= left; --i)
                if (Char.IsDigit(sb[i]))
                {
                    c += 1;

                    if (c > skipRight)
                        sb[i] = 'X';
                }

            return sb.ToString();
        }




        //public dynamic Save_payerpayeprivateDocument(payerpayeeprivate obj_payerpayeeprivate)
        //{
        //    if (obj_payerpayeeprivate.customerid != null)
        //    {
        //        _logger.LogInfo("Getting into Get_avatarmasters() api");
        //        try
        //        {
        //            using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        //            {
        //                connection.Open();                     


        //                NpgsqlCommand inst_payerpayeeprivate = new NpgsqlCommand("insert into initiatorrecipientprivates(customerid,firstname,email,mobile,bankaccountnumber,bankname,iban,accountname,status,createdby,createddate) values(@customerid,@firstname,@email,@mobile,@bankaccountnumber,@bankname,@iban,@accountname,@status,@createdby,@createddate)", connection);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@firstname", obj_payerpayeeprivate.firstname);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@email", obj_payerpayeeprivate.email);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@mobile", obj_payerpayeeprivate.mobile);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@bankaccountnumber", obj_payerpayeeprivate.accountnumber);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@bankname", obj_payerpayeeprivate.bankname);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@iban", obj_payerpayeeprivate.swiftcode);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@accountname", obj_payerpayeeprivate.firstname);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@status", 'A');
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
        //                inst_payerpayeeprivate.Parameters.AddWithValue("@createddate", DateTime.Now);
        //                int result = inst_payerpayeeprivate.ExecuteNonQuery();
        //                if (result > 0)
        //                {
        //                    return "Success";
        //                }


        //                connection.Close();
        //                connection.Dispose();


        //                connection.Close();
        //                connection.Dispose();
        //                return null;// result;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError($"Service : Save_payerpayeePrivateDocument(): {ex}");

        //        }
        //    }
        //    else { return "Customer id not avilable"; }
        //    return null;
        //}




        public dynamic MandatoryPayee(string brn)
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

                    var parameters = new { @cid = cid, @uid = uid, @brn = brn, @wStatus = wStatus };
                    var SQL = @"select * from payerpayeeprivate where brn=@brn";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_MandatoryPayee = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { MandatoryPayee = obj_MandatoryPayee, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customerpaymode(int id)\r\n {ex}");
                throw ex;
            }
        }




        public dynamic GetallPayee(int customerid)
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

                    var parameters = new { @cid = cid, @uid = uid, @customerid = customerid, @wStatus = wStatus };
                    var SQL = @" select distinct(m.customerid),m.* from payerpayeemapping p left join customermasters m on 1=1 and m.uid in (select payeeuid from payerpayeemapping where customerid=@customerid)";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_MandatoryPayee = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { AllPayee = result, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customerpaymode(int id)\r\n {ex}");
                throw ex;
            }
        }





        public dynamic GetallPayeetranscdetail(int customerid)
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

                    var parameters = new { @cid = cid, @uid = uid, @customerid = customerid, @wStatus = wStatus };
                    var SQL = @" select pk_encode(m.verification_id) as pkcol,m.verification_id,m.documenttype,m.purpose,m.amount,
case when m.status='P' then 'Pending' 
when m.status='A' then 'Approved' 
when m.status='R' then 'Rejected' end as statusdesc,
m.uploadfilename,m.uploadpath,
concat(c.firstname,'',c.lastname) as customername,concat(cu.firstname,'',cu.lastname) as payeename,m.*,
case when m.status='P' then '' 
when m.status='A' then
   case when t.status='I' then 'Payment pending' 
   when t.status='A' then 'Payment Done'
   end
when m.status='R' then 'Rejected' end as paymentsataus
from customer_document_verification m
left join customermasters c on c.customerid=m.customer_id 
left join customermasters cu on cu.customerid=m.payeeid 
left join transactionmasters t on t.recipientid = m.payeeid and t.documentnumber = m.documentnumber where m.customer_id=@customerid)";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_MandatoryPayee = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { AllPayee = result, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customerpaymode(int id)\r\n {ex}");
                throw ex;
            }
        }







        public dynamic GetallPayeebankdetail(int payeeid)
        {
            _logger.LogInfo("Getting into Get_customerpaymode(int id) api");
            try
            {
                string bankaccountnumber = string.Empty;
                string mask_bankaccno = string.Empty;
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    //get bankaccountnumber

                    var parameters_cus = new { @cid = cid, @uid = uid, @customerid = payeeid};
                    var SQL_cus = " select bankaccountnumber from payerpayeeprivate where customerid=@customerid";
                    var resultcus = connection.Query<dynamic>(SQL_cus, parameters_cus);
                    var obj_cus = resultcus.FirstOrDefault();
                    bankaccountnumber = obj_cus.bankaccountnumber;
                    // cusname = obj_cus.cusname;

                    mask_bankaccno = MaskedNumber1(bankaccountnumber);



                    string wStatus = "NormalStatus";

                    var parameters = new { @cid = cid, @uid = uid, @customerid = payeeid, @wStatus = wStatus };
                    var SQL = @" select concat(firstname,'',lastname) as payeename,bankname from payerpayeeprivate where customerid=@customerid";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_MandatoryPayee = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { Payeedetail = result,maskedbankaccountnumber= mask_bankaccno, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customerpaymode(int id)\r\n {ex}");
                throw ex;
            }
        }


    }
}
