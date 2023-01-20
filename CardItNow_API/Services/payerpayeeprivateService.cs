using CardItNow.interfaces;
using CardItNow.Models;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Services
{
    public class payerpayeeprivateService : IpayerpayeeprivateService
    {
        private readonly IConfiguration Configuration;
        private readonly payerpayeeprivateContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        public payerpayeeprivateService(IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;
            _logger = logger;
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
                _logger.LogInfo("Getting into Get_avatarmasters() api");
                try
                {
                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {
                        
                        connection.Open();
                        if ((obj_payerpayeeprivate.customerid != null) &&
                            (!string.IsNullOrEmpty(obj_payerpayeeprivate.documnettype)))
                        {
                            NpgsqlCommand check_payer = new NpgsqlCommand("select count(*)  from initiatorrecipientprivates where customerid = '" + obj_payerpayeeprivate.customerid + "' and bankname = '" + obj_payerpayeeprivate.bankname + "' and bankaccountnumber = '" + obj_payerpayeeprivate.accountnumber + "'and documenttype='"+obj_payerpayeeprivate.documnettype+"'", connection);
                           var output_result = check_payer.ExecuteScalar().ToString();

                            if ((int.Parse(output_result) == 0 )||(output_result==null))
                            {
                                NpgsqlCommand inst_payerpayeeprivate = new NpgsqlCommand("insert into initiatorrecipientprivates(customerid,firstname,email,mobile,bankaccountnumber,bankname,iban,accountname,status,createdby,createddate,documenttype) values(@customerid,@firstname,@email,@mobile,@bankaccountnumber,@bankname,@iban,@accountname,@status,@createdby,@createddate,@documenttype)", connection);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@customerid", obj_payerpayeeprivate.customerid);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@firstname", obj_payerpayeeprivate.firstname);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@email", obj_payerpayeeprivate.email);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@mobile", obj_payerpayeeprivate.mobile);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@bankaccountnumber", obj_payerpayeeprivate.accountnumber);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@bankname", obj_payerpayeeprivate.bankname);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@iban", obj_payerpayeeprivate.swiftcode);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@accountname", obj_payerpayeeprivate.firstname);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@status", 'A');
                                inst_payerpayeeprivate.Parameters.AddWithValue("@createdby", obj_payerpayeeprivate.customerid);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@createddate", DateTime.Now);
                                inst_payerpayeeprivate.Parameters.AddWithValue("@documenttype",obj_payerpayeeprivate.documnettype);
                                //inst_payerpayeeprivate.Parameters.AddWithValue("@document",obj_payerpayeeprivate.documentvalue);
                                int result = inst_payerpayeeprivate.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    return "Success";
                                }
                            }
                            else
                            {
                                return "The Given Payee already availbale in your My payee list ";
                            }
                        }
                        else
                        {
                            return "Document type/ customer ID missing  ";
                        }


                        connection.Close();
                        connection.Dispose();


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
    }
}
