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

namespace CardItNow.Services
{
    public class commonService : IcommonService
    {
        private readonly IConfiguration Configuration;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        public commonService( IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            Configuration = configuration;

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
    }
}
