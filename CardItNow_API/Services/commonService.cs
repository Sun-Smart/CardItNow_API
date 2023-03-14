using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CardItNow.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using carditnow.Models;
using Dapper;
using System.Collections;
using SunSmartnTireProducts.Helpers;

namespace CardItNow.Services
{
    public class commonService : IcommonService
    {
        private readonly IConfiguration Configuration;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly customerdetailContext _context_cd;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        public commonService(IConfiguration configuration, customerdetailContext contextdb, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;

            _logger = logger;
            _context_cd = contextdb;
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

        public dynamic Getdocumenttype()
        {
            var sList = new List<IndividualDocument>();
            try
            {
                _logger.LogInfo("Getting into Get Document Type api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand inst_cd = new NpgsqlCommand("select  masterdatadescription from masterdatatypes inner join masterdatas on datatypeid = masterdatatypeid  where masterdatatypename = 'Document Type'", connection);
                    NpgsqlDataAdapter sda = new NpgsqlDataAdapter(inst_cd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //dt.AsEnumerable().Select(x => new IndividualDocument
                        //{
                        //    documnettype = (string)x["masterdatadescription"]
                        //}).ToList();

                        foreach (DataRow dr in dt.Rows)
                        {
                            sList.Add(new IndividualDocument
                            {
                                documnettype = dr["masterdatadescription"].ToString()
                            });
                        }
                    }
                    connection.Close();
                    connection.Dispose();
                }
                return sList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            return sList;
        }

        public dynamic GetBankList()
        {
            var sList = new List<BankList>();
            try
            {
                _logger.LogInfo("Getting into Get Document Type api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand inst_cd = new NpgsqlCommand("select  masterdatadescription from masterdatatypes inner join masterdatas on datatypeid = masterdatatypeid  where masterdatatypename = 'Bank Details' order by masterdatadescription", connection);
                    NpgsqlDataAdapter sda = new NpgsqlDataAdapter(inst_cd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //dt.AsEnumerable().Select(x => new IndividualDocument
                        //{
                        //    documnettype = (string)x["masterdatadescription"]
                        //}).ToList();

                        foreach (DataRow dr in dt.Rows)
                        {
                            sList.Add(new BankList
                            {
                                bankname = dr["masterdatadescription"].ToString()
                            });
                        }
                    }
                    connection.Close();
                    connection.Dispose();
                }
                return sList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            return sList;
        }

        public dynamic GetPurposeList()
        {
            var sList = new List<purposeList>();
            try
            {
                _logger.LogInfo("Getting into Get Document Type api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();

                    var parameters = new { @cid = cid, @uid = uid };
                        string inst_cd = @"select  masterdataid as value,masterdatadescription as description from masterdatatypes inner join masterdatas on datatypeid = masterdatatypeid where masterdatatypename = 'Purpose Of Payment' and masterdatatypeid = 7 order by masterdatadescription";
                    var result = connection.Query<dynamic>(inst_cd, parameters);
                    connection.Close();
                    connection.Dispose();
                    return new { payment_puposelist = result };
                    //ramesh
                    //NpgsqlDataAdapter sda = new NpgsqlDataAdapter(inst_cd);
                    //DataTable dt = new DataTable();
                    //sda.Fill(dt);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        sList.Add(new purposeList
                    //        {
                    //            purpose = dr["masterdatadescription"].ToString()
                    //        });
                    //    }
                    //}
                    //connection.Close();
                    //connection.Dispose();
                    //end
                }
                return sList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            return sList;
        }




        public dynamic GetPurposeList_hr()
        {
            var sList = new List<purposeList>();
            try
            {
                _logger.LogInfo("Getting into Get Document Type api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    var parameters = new { @cid = cid, @uid = uid };


                    string inst_cd = @"select  masterdataid as value,masterdatadescription as description from masterdatatypes inner join masterdatas on datatypeid = masterdatatypeid where masterdatatypename = 'Rent Types' and masterdatatypeid = 12 order by masterdatadescription";
                    var result = connection.Query<dynamic>(inst_cd, parameters);
                    connection.Close();
                    connection.Dispose();
                    return new { houserent_puposelist = result };
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            return sList;
        }




        public dynamic Sociallogin(sociallogin model)
        {

            try
            {
                _logger.LogInfo("Getting into Get Document Type api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand inst_cd = new NpgsqlCommand("select count(email) from customermasters where email='" + model.email + "'", connection);
                    var result = inst_cd.ExecuteScalar().ToString();
                    if (int.Parse(result) > 0)
                        return "Not available";
                    else
                        return "Available";

                    connection.Close();
                    connection.Dispose();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            return null;
        }

        public dynamic SaveSocial(Savesocial model)
        {
            try
            {
                int custid = 0;
                string custmid = string.Empty;
                int geonumber = 0;
                int geid =model.geoid;
                string geoids = string.Empty;
                int maxid = 0;
                string newValueString = string.Empty;
                string cusname = string.Empty;
                // geid = Convert.ToInt32(geoid);

                _logger.LogInfo("Getting into Save social api Type Login");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();

                    NpgsqlCommand CheckExists_social_media = new NpgsqlCommand("select count(email) from customermasters where email='" + model.email + "'", connection);
                    var exists_result = CheckExists_social_media.ExecuteScalar().ToString();
                    if (int.Parse(exists_result) == 0)
                    {
                        //shy
                        var parameters_customeridmax = new { @cid = cid, @uid = uid };
                        var SQLmax = "select max(customerid) as maxid from customermasters";
                        var resultmax = connection.Query<dynamic>(SQLmax, parameters_customeridmax);
                        var obj_cutomeridmax = resultmax.FirstOrDefault();
                        maxid = obj_cutomeridmax.maxid;
                        maxid = maxid + 1;



                        if (geid == 1)
                        {
                            newValueString = "U" + maxid.ToString().PadLeft(8, '0');
                        }
                        if (geid == 2)
                        {
                            newValueString = "P" + maxid.ToString().PadLeft(8, '0');
                        }



                        NpgsqlCommand inst_social_media = new NpgsqlCommand("insert into customermasters(mode,uid,type,firstname,lastname,email,mobile,googleid,facebookid,status,createddate,createdby,customervisible) values(@mode,@uid,@type,@firstname,@lastname,@email,@mobile,@googleid,@facebookid,@status,@createddate,@createdby,@customervisible)", connection);
                       // inst_social_media.Parameters.AddWithValue("@mode", "R");
                        inst_social_media.Parameters.AddWithValue("@mode", "I");
                       // inst_social_media.Parameters.AddWithValue("@uid", "P" + DateTime.Now.Day);
                        inst_social_media.Parameters.AddWithValue("@uid",newValueString);
                        inst_social_media.Parameters.AddWithValue("@type", "I");
                       // inst_social_media.Parameters.AddWithValue("@type", "C");
                        inst_social_media.Parameters.AddWithValue("@firstname", model.firstname);
                        inst_social_media.Parameters.AddWithValue("@lastname", model.lastname);
                        inst_social_media.Parameters.AddWithValue("@email", model.email);
                       // inst_social_media.Parameters.AddWithValue("@mobile", "000000");
                        inst_social_media.Parameters.AddWithValue("@mobile", model.mobile);
                        if (model.mediatype == "Google")
                        {
                            inst_social_media.Parameters.AddWithValue("@googleid", model.socialid);
                        }
                        else
                        {
                            inst_social_media.Parameters.AddWithValue("@googleid", "0");
                        }
                        if (model.mediatype == "Facebook")
                        {
                            inst_social_media.Parameters.AddWithValue("@facebookid", model.socialid);
                        }
                        else
                        {
                            inst_social_media.Parameters.AddWithValue("@facebookid", "0");
                        }
                        inst_social_media.Parameters.AddWithValue("@status", "N");
                        inst_social_media.Parameters.AddWithValue("@createddate", DateTime.Now);
                        inst_social_media.Parameters.AddWithValue("@createdby", 1);
                        inst_social_media.Parameters.AddWithValue("@customervisible", false);
                        var result = inst_social_media.ExecuteNonQuery().ToString();

                       
                            var parameters_customerid = new { @cid = cid, @uid = uid, @email = model.email };
                            var SQL1 = "  select pk_encode(m.customerid) as pkcol,m.customerid,m.customerid,concat(m.firstname,'',m.lastname) as cusname,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                            var result2 = connection.Query<dynamic>(SQL1, parameters_customerid);
                            var obj_cutomerid = result2.FirstOrDefault();
                            custid = obj_cutomerid.customerid;
                            geonumber = obj_cutomerid.geoid;
                            custmid = custid.ToString();
                            geoids = geonumber.ToString();
                        cusname = obj_cutomerid.cusname;


                        var cus_detail = new customerdetail();
                        cus_detail.geoid = model.geoid;
                        cus_detail.customerid = custid;
                        _context_cd.customerdetails.Add(cus_detail);
                        _context_cd.SaveChanges();


                        var parameters_customerid2 = new { @cid = cid, @uid = uid, @email = model.email };
                        var SQL2 = "  select pk_encode(m.customerid) as pkcol,m.customerid,concat(m.firstname,'',m.lastname) as cusname,case when d.geoid is null then 0 else d.geoid end as geoid from customermasters m  left join customerdetails d on d.customerid = m.customerid where m.email = @email";
                        var result3 = connection.Query<dynamic>(SQL2, parameters_customerid);
                        var obj_cutomerid2 = result3.FirstOrDefault();
                        custid = obj_cutomerid2.customerid;
                        geonumber = obj_cutomerid2.geoid;
                        custmid = custid.ToString();
                        geoids = geonumber.ToString();
                        cusname = obj_cutomerid.cusname;

                        if (int.Parse(result) > 0)
                        {

                            string subject = "Welcome Greetings";
                            StringBuilder sb = new StringBuilder();
                            SmtpClient smtp = new SmtpClient();
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            sb.Append("Dear " + cusname + ",");
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
                            Helper.Email(sb.ToString(), model.email, cusname, subject);















                            var result1 = new
                            {
                                status = "success",
                                data = "null",
                                customerid= custmid,
                                geoid= geoids,/* Application-specific data would go here. */
                                message = "Succesfully saved record" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }
                        //
                        else
                        {
                            //return "Filed";
                            var result_fail = new
                            {
                                status = "Fail",
                                data = "",/* Application-specific data would go here. */
                                message = "Unexcepted Error" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result_fail);
                        }

                    }
                    else
                    {
                        #region Begin
                        //NpgsqlCommand inst_social_media = new NpgsqlCommand("update  customermasters SET mode=@mode,uid=@uid,type=@type,firstname=@firstname,lastname=@lastname,email=@email,mobile=@mobile,googleid=@googleid,facebookid=@facebookid,status=@status,createddate=@createddate,createdby=@createdby where email='" + model.email + "'", connection);
                        //inst_social_media.Parameters.AddWithValue("@mode", "R");
                        //inst_social_media.Parameters.AddWithValue("@uid", "P" + DateTime.Now.Day);
                        //inst_social_media.Parameters.AddWithValue("@type", "C");
                        //inst_social_media.Parameters.AddWithValue("@firstname", model.firstname);
                        //inst_social_media.Parameters.AddWithValue("@lastname", model.lastname);
                        //inst_social_media.Parameters.AddWithValue("@email", model.email);
                        //inst_social_media.Parameters.AddWithValue("@mobile", "000000");
                        //if (model.mediatype == "Google")
                        //{
                        //    inst_social_media.Parameters.AddWithValue("@googleid", model.socialid);
                        //}
                        //else
                        //{
                        //    inst_social_media.Parameters.AddWithValue("@googleid", "0");
                        //}
                        //if (model.mediatype == "Facebook")
                        //{
                        //    inst_social_media.Parameters.AddWithValue("@facebookid", model.socialid);
                        //}
                        //else
                        //{
                        //    inst_social_media.Parameters.AddWithValue("@facebookid", "0");
                        //}
                        //inst_social_media.Parameters.AddWithValue("@status", "P");
                        //inst_social_media.Parameters.AddWithValue("@createddate", DateTime.Now);
                        //inst_social_media.Parameters.AddWithValue("@createdby", 1);
                        //var result = inst_social_media.ExecuteNonQuery().ToString();
                        //if (int.Parse(result) > 0)
                        //    return "Success";
                        //else
                        //    return "Filed";
                        #endregion

                        var result = new
                        {
                            status = "Failed",
                            data = "",/* Application-specific data would go here. */
                            message = "Email id already register in this app" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result);
                    }
                    connection.Close();
                    connection.Dispose();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
            }
            return null;
        }

        public decimal GetRandomNumber()
        {
            Random objRandom = new Random();
            int intValue = objRandom.Next(100000, 999999);
            return intValue;
        }

        public string forgotpass(Savesocial model)
        {
            try
            {
                int custid = 0;
                string cusname = string.Empty;

                _logger.LogInfo("Getting into Forgot Passcode(string email) api");

                decimal TACNo = GetRandomNumber();
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand CheckExists_social_media = new NpgsqlCommand("select count(email) from customermasters where email='" + model.email + "'", connection);
                    var exists_result = CheckExists_social_media.ExecuteScalar().ToString();
                    if (int.Parse(exists_result) > 0)
                    {
                        NpgsqlCommand update_otp = new NpgsqlCommand("update customermasters set otp='" + TACNo + "'where email='" + model.email + "'", connection);
                        var update_result = update_otp.ExecuteNonQuery().ToString();


                        var parameters_customerid = new { @cid = cid, @uid = uid,@email = model.email };
                        var SQL = "select email,concat(firstname,'',lastname) as cusname from customermasters where email=@email";
                        var result = connection.Query<dynamic>(SQL, parameters_customerid);
                        var obj_cutomerid = result.FirstOrDefault();
                       // email = obj_cutomerid.email;
                        cusname = obj_cutomerid.cusname;




                        if (int.Parse(update_result) > 0)
                        {
                            string subject = "CarditNow Forgot passcode OTP ";
                            StringBuilder sb = new StringBuilder();
                            SmtpClient smtp = new SmtpClient();
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            sb.Append("Dear Customer,");
                            sb.Append("<br/>");
                            sb.Append("<br/>");
                            sb.Append("Your forgot passcode OTP to send your Registration email account,Please visit your register email : ");
                            sb.Append("<b>");
                            sb.Append(TACNo);
                            sb.Append("</b>");
                            sb.Append("<br/>");
                            sb.Append("<br/>");
                            sb.Append("Regards,");
                            sb.Append("<br/>");
                            sb.Append("SunSmart Global");
                            //SendEmail(model.email, subject, sb.ToString());

                            Helper.Email(sb.ToString(), model.email, cusname, subject);


                            var parameters_customerid1 = new { @cid = cid, @uid = uid, @email = model.email };
                            var SQL1 = "  select m.customerid,m.uid from customermasters m where m.email=@email ";
                            var result_cus = connection.Query<dynamic>(SQL1, parameters_customerid1);
                            var obj_cutomerid1 = result_cus.FirstOrDefault();
                            custid = obj_cutomerid1.customerid;




                            //Helper.SendEmail();
                            //return "Success";
                            var result1 = new
                            {
                                status = "success",
                                data = "",
                                customerid= custid,/* Application-specific data would go here. */
                                message = "OTP has been send to register email id" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }
                    }
                    else
                    {
                        var result1 = new
                        {
                            status = "fail",
                            data = "",
                            customerid = "",/* Application-specific data would go here. */
                            message = "Forgot email ID not avilable in CarditNow App" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            return null;
        }

        public void SendEmail(string toemail, string subject, string htmlString)
        {
            //string _fromemail = @"support@myskillstree.com";
            // string _password = @"SupMyST123";//

           // string _fromemail = @"support@sunsmartglobal.com";
           // string _password = @"ecqsufegzoucluji";

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

        public string otpvalidate(verify_otp model)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand CheckExists_customer = new NpgsqlCommand("select otp from customermasters where email='" +model.email + "'", connection);
                    NpgsqlDataAdapter get_customer = new NpgsqlDataAdapter(CheckExists_customer);
                    DataTable dt = new DataTable();
                    get_customer.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {
                            var t = dt.Rows[0][0].ToString();
                            if (t == model.otp)
                            {
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "OTP verified successfully" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                            else
                            {
                                {
                                    var result1 = new
                                    {
                                        status = "fail",
                                        data = "",   /* Application-specific data would go here. */
                                        message = "OTP missmatch" /* Or optional success message */
                                    };
                                    return JsonConvert.SerializeObject(result1);
                                }
                            }

                            connection.Close();
                            connection.Dispose();
                        }
                    }
                }
            }
            catch
            { }
            return null;
        }

        public string ChangePass(changepasscode model)
        {
            try
            {

                _logger.LogInfo("Getting into Forgot Passcode(string email) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand CheckExists_customer = new NpgsqlCommand("select password,tpin from customermasters where email='" + model.email + "'", connection);
                    NpgsqlDataAdapter get_customer = new NpgsqlDataAdapter(CheckExists_customer);
                    DataTable dt = new DataTable();
                    get_customer.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {
                            var t = dt.Rows[0][0].ToString();
                            if (t != model.pin)
                            {

                                NpgsqlCommand update_loginPass_Code = new NpgsqlCommand("update customermasters set password='" + model.pin + "',tpin='" + model.pin + "' where email='" + model.email + "'", connection);
                                var update_result = update_loginPass_Code.ExecuteNonQuery().ToString();
                                if (int.Parse(update_result) > 0)
                                {
                                    var result1 = new
                                    {
                                        status = "success",
                                        data = "",   /* Application-specific data would go here. */
                                        message = "Passcode Changed successfully" /* Or optional success message */
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
                            NpgsqlCommand update_loginPass_Code = new NpgsqlCommand("update customermasters set password='" + model.pin + "',tpin='" + model.pin + "' where email='" + model.email + "'", connection);
                            var update_result = update_loginPass_Code.ExecuteNonQuery().ToString();
                            if (int.Parse(update_result) > 0)
                            {
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "Passcode updated successfully" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            return null;
        }

        public dynamic Customerauth(customerauth model)
        {
            throw new NotImplementedException();
        }




        //privacy clause


        public dynamic GetPrivacyclause()
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

                    var parameters = new { @cid = cid, @uid = uid,  @wStatus = wStatus };
                    var SQL = @"select pk_encode(masterdataid) as pkcol,masterdataid,masterdatadescription as privacyclause from masterdatas where masterdatatypeid=10";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_privacypolicy = result.FirstOrDefault();
                    //var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    //var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { Privacypolicy = obj_privacypolicy, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customerpaymode(int id)\r\n {ex}");
                throw ex;
            }
        }



        //LGU duplication validation
        public string duplicatetransactionvalidation(duplicatetransactionvalidation model)
        {
            try
            {
               // var result1 = "";

                _logger.LogInfo("Getting into Forgot Passcode(string email) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();

                    var parameters_customerid = new { @cid = cid, @uid = uid, @customerid = model.customerid, @municipality=model.municipality,@purpose=model.purpose,@startdate=model.startdate,@enddate=model.enddate,@billamount=model.billamount};
                    //var SQL = "  select * from transactionmasters where uid=(select uid from customermasters where customerid=@customerid) and recipientuid = (select uid from customermasters where customerid = @municipality) and recipientid = @municipality and transactiontype = @purpose and pin = @pin and startdate = @startdate and expirydate = @enddate and contractamount = @billamount";
                    //var result = connection.Query<dynamic>(SQL, parameters_customerid);



                    NpgsqlCommand CheckExists_customer = new NpgsqlCommand(" select * from transactionmasters where uid=(select uid from customermasters where customerid='"+ model.customerid + "') and recipientuid = (select uid from customermasters where customerid = '"+ model.customerid + "') and recipientid = '"+model.municipality+"' and transactiontype = '"+model.purpose+"' and pin = '"+model.pin+"' and startdate = '"+model.startdate+"' and expirydate = '"+model.enddate+"' and contractamount = '"+model.billamount+"'", connection);
                    NpgsqlDataAdapter get_customer = new NpgsqlDataAdapter(CheckExists_customer);
                    DataTable dt = new DataTable();
                    get_customer.Fill(dt);
                    if (dt.Rows.Count==0)
                    {
                               
                                    var result1 = new
                                    {
                                        status = "success",
                                        data = "",   /* Application-specific data would go here. */
                                        message = "Proceed for futher action" /* Or optional success message */
                                    };
                                    return JsonConvert.SerializeObject(result1);
                              
                            }
                            else
                            {
                                var result1 = new
                                {
                                    status = "fail",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "This invoice has been paid" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                      
                                
                            }
                        }
                   

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            return null;
        }


        //Home rent duplication

        public string duplicatetransactionvalidation_hr(duplicatetransactionvalidation model)
        {
            try
            {
                // var result1 = "";

                _logger.LogInfo("Getting into Forgot Passcode(string email) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();

                    if (model.purpose == "Maintenance" || model.purpose == "Renovation")
                    {

                        NpgsqlCommand CheckExists_customer = new NpgsqlCommand(" select * from transactionmasters where uid=(select uid from customermasters where customerid='" + model.customerid + "') and recipientuid = '" + model.recipientuid + "' and recipientid = '" + model.recipientid + "' and transactiontype = '" + model.purpose + "' and pin = '" + model.pin + "' and startdate = '" + model.startdate + "' and expirydate = '" + model.enddate + "' and contractamount = '" + model.billamount + "'", connection);
                        NpgsqlDataAdapter get_customer = new NpgsqlDataAdapter(CheckExists_customer);
                        DataTable dt = new DataTable();
                        get_customer.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {

                            var result1 = new
                            {
                                status = "success",
                                data = "",   /* Application-specific data would go here. */
                                message = "Proceed for futher action" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);

                        }
                        else
                        {
                            var result1 = new
                            {
                                status = "fail",
                                data = "",   /* Application-specific data would go here. */
                                message = "This invoice has been paid" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }
                    }
                    else
                    {



                        NpgsqlCommand CheckExists_customer = new NpgsqlCommand(" select * from transactionmasters where uid=(select uid from customermasters where customerid='" + model.customerid + "') and recipientuid = '" + model.recipientuid + "' and recipientid = '" + model.recipientid + "' and transactiontype = '" + model.purpose + "'  and startdate = '" + model.startdate + "' and expirydate = '" + model.enddate + "' and contractamount = '" + model.billamount + "'", connection);
                        NpgsqlDataAdapter get_customer = new NpgsqlDataAdapter(CheckExists_customer);
                        DataTable dt = new DataTable();
                        get_customer.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {

                            var result1 = new
                            {
                                status = "success",
                                data = "",   /* Application-specific data would go here. */
                                message = "Proceed for futher action" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);

                        }
                        else
                        {
                            var result1 = new
                            {
                                status = "fail",
                                data = "",   /* Application-specific data would go here. */
                                message = "This invoice has been paid" /* Or optional success message */
                            };
                            return JsonConvert.SerializeObject(result1);
                        }
                    }

                }
            }


            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            return null;
        }


        public dynamic get_recenttransaction(dashboard model)
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid,@customerid=model.customerid };
                    string SQL = " select m.*,d.transactiondate,concat(c.firstname,'',c.lastname) as customername, m.recipientname as paidto,case when m.transactiontype = 'P' then 'Property Tax' when m.transactiontype = 'I' then 'Invoice' when m.transactiontype = 'R' then 'Rent' end as purpose,d.transactionconfirmnumber as TXNID from transactionmasters m left join transactiondetails d on m.transactionid = d.transactionid left join customermasters c on c.uid = d.uid and c.uid = m.uid  and c.customerid = m.recipientid where m.uid = (select uid from customermasters where customerid = @customerid) order by m.transactionid desc limit 4";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_purpose(): {ex}");
                throw ex;
            }
            return null;
        }

        public dynamic get_allrecenttransaction(dashboard model)
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid, @customerid = model.customerid };
                    string SQL = " select m.*,d.transactiondate,concat(c.firstname,'',c.lastname) as customername, m.recipientname as paidto,case when m.transactiontype = 'P' then 'Property Tax' when m.transactiontype = 'I' then 'Invoice' when m.transactiontype = 'R' then 'Rent' end as purpose,d.transactionconfirmnumber as TXNID from transactionmasters m left join transactiondetails d on m.transactionid = d.transactionid left join customermasters c on c.uid = d.uid and c.uid = m.uid  and c.customerid = m.recipientid where m.uid = (select uid from customermasters where customerid = @customerid) order by m.transactionid desc ";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_purpose(): {ex}");
                throw ex;
            }
            return null;
        }

        public dynamic get_moneyspenddetails(dashboard model)
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid, @customerid = model.customerid };
                    //string SQL = "select sum(m.payamount) as moneycount,extract(month from d.transactiondate) as month,to_char(d.transactiondate, 'month') as monthname from transactionmasters m left join transactiondetails d on m.transactionid = d.transactionid where m.uid = (select uid from customermasters where customerid = @customerid) and d.transactiondate > CURRENT_DATE - INTERVAL '6 months' group by d.transactiondate";


                    string SQL = "select  (select sum(m.payamount) as moneycount from transactionmasters m  where m.uid = (select uid from customermasters where customerid = @customerid)) as moneyd,extract(month from d.transactiondate) as month,trim(to_char(d.transactiondate, 'month')) as monthname from transactionmasters m left join transactiondetails d on m.transactionid = d.transactionid where m.uid = (select uid from customermasters where customerid = @customerid) and d.transactiondate > CURRENT_DATE - INTERVAL '6 months' group by d.transactiondate ";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_purpose(): {ex}");
                throw ex;
            }
            return null;
        }


        public dynamic get_transactionoverview(dashboard model)
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid, @customerid = model.customerid };
                  


                    string SQL = "select m.payamount as totalamount,m.contractamount as billamount,m.carditconvfee as ConvenienceFee,d.transactionconfirmnumber as transactionid,case when m.transactiontype = 'P' then 'Property Tax' when m.transactiontype = 'I' then 'Invoice' when m.transactiontype = 'R' then 'Rent' end as purpose,concat('Trnx ID',':', d.transactionconfirmnumber) as mailsubject,'pingme@carditnow.com' as toemail,m.* from transactionmasters m left join transactiondetails d on m.transactionid = d.transactionid where m.uid = (select uid from customermasters where customerid = @customerid)";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_purpose(): {ex}");
                throw ex;
            }
            return null;
        }

        //get LGU customers

        public dynamic GetLGUcustomers()
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

                    var parameters = new { @cid = cid, @uid = uid, @wStatus = wStatus };
                    var SQL = @"select * from customermasters where mode='LG' and type='B' and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_privacypolicy = result.FirstOrDefault();
                    //var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    //var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { LGUcustomers = result, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customerpaymode(int id)\r\n {ex}");
                throw ex;
            }
        }


        public dynamic GetHomerentcustomers()
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

                    var parameters = new { @cid = cid, @uid = uid, @wStatus = wStatus };
                    var SQL = @"select * from customermasters where mode='PP' or mode='M' or mode='IL' and type='B' and status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_privacypolicy = result.FirstOrDefault();
                    //var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customerpaymodes'";
                    //var customerpaymode_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { Homerentcustomers = result, formproperty, visiblelist, hidelist });
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
