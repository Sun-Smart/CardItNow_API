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
        public commonService(IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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
                    NpgsqlCommand inst_cd = new NpgsqlCommand("select  masterdatadescription from masterdatatypes inner join masterdatas on datatypeid = masterdatatypeid  where masterdatatypename = 'Purpose Of Payment' order by masterdatadescription", connection);
                    NpgsqlDataAdapter sda = new NpgsqlDataAdapter(inst_cd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            sList.Add(new purposeList
                            {
                                purpose = dr["masterdatadescription"].ToString()
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
                _logger.LogInfo("Getting into Save social api Type Login");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand CheckExists_social_media = new NpgsqlCommand("select count(email) from customermasters where email='" + model.email + "'", connection);
                    var exists_result = CheckExists_social_media.ExecuteScalar().ToString();
                    if (int.Parse(exists_result) == 0)
                    {
                        NpgsqlCommand inst_social_media = new NpgsqlCommand("insert into customermasters(mode,uid,type,firstname,lastname,email,mobile,googleid,facebookid,status,createddate,createdby) values(@mode,@uid,@type,@firstname,@lastname,@email,@mobile,@googleid,@facebookid,@status,@createddate,@createdby)", connection);
                        inst_social_media.Parameters.AddWithValue("@mode", "R");
                        inst_social_media.Parameters.AddWithValue("@uid", "P" + DateTime.Now.Day);
                        inst_social_media.Parameters.AddWithValue("@type", "C");
                        inst_social_media.Parameters.AddWithValue("@firstname", model.firstname);
                        inst_social_media.Parameters.AddWithValue("@lastname", model.lastname);
                        inst_social_media.Parameters.AddWithValue("@email", model.email);
                        inst_social_media.Parameters.AddWithValue("@mobile", "000000");
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
                        inst_social_media.Parameters.AddWithValue("@status", "P");
                        inst_social_media.Parameters.AddWithValue("@createddate", DateTime.Now);
                        inst_social_media.Parameters.AddWithValue("@createdby", 1);
                        var result = inst_social_media.ExecuteNonQuery().ToString();
                        if (int.Parse(result) > 0)
                        {
                            var result1 = new
                            {
                                status = "success",
                                data = "null",/* Application-specific data would go here. */
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

    }
}
