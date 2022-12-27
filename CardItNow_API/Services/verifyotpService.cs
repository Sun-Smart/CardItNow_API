using carditnow.Models;
using carditnow.Services;
using CardItNow.interfaces;
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Services
{
    public class verifyotpService:IverifyotpService
    {
        private readonly IConfiguration Configuration;
        private readonly customermasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomermasterService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";

        public verifyotpService(customermasterContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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

        public string VerifyOTP(string email, string otp)
        {
            _logger.LogInfo("Getting into VerifyOTP() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters = new { @email = email, @otp = otp };
                    string SQL = "select otp from customermasters where email=@email and status='N'";            
                    var result = connection.ExecuteScalar<string>(SQL, parameters);
                    if (result != null)
                    {
                        if (result.ToString()== otp)
                        {
                            string sql_update = "update customermasters set status='A' where email=@email and otp=@otp ";
                            var result_update = connection.Query<dynamic>(sql_update,parameters);
                            //if (result_update >= 1)
                            //{
                            if (result_update.Count() >= 0)
                            {
                                //return (new { data = result_update, count = result_update.Count() });
                                return "Success";
                            }
                            else { return "Record missmatch or already validate this email"; }
                            //}
                        }
                        else { return "not match"; }
                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_customermasters(): {ex}");
                throw ex;
            }
            return "Fail";
        }
    }
}
