
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
    public class customerdocumentverificationService : IcustomerdocumentverificationService
    {
        private readonly IConfiguration Configuration;
        private readonly customerdocumentverificationContext _context;
        private readonly customerdetailContext _context_cd;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomerdocumentverificationService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public customerdocumentverificationService(customerdocumentverificationContext context, customerdetailContext contextdb, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
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




        public dynamic Save_customerdocumentverification(string token, customerdocumentverification obj_customermaster1)
        {
            _logger.LogInfo("Saving: Save_customerdocumentverification(string token,customerdocumentverification obj_customermaster1) ");
            try
            {
                string serr = "";
                int querytype = 0;
                string email = string.Empty;
                string cusname = string.Empty;
                string value = string.Empty;
                string purpose = string.Empty;
                string payee_email = string.Empty;
                string payeename = string.Empty;
                string Reason = obj_customermaster1.remarks;
                var res = "";

                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }


                if (obj_customermaster1.verification_id == 0 || obj_customermaster1.verification_id == null || obj_customermaster1.verification_id < 0)
                {
                    if (obj_customermaster1.status == "" || obj_customermaster1.status == null) obj_customermaster1.status = "P";
                    //obj_customermaster1.companyid=cid;
                    obj_customermaster1.createdby = uid;
                    obj_customermaster1.createddate =DateTime.Now;
                    _context.customer_document_verification.Add((dynamic)obj_customermaster1);
                    querytype = 1;
                }
                else
                {
                    //obj_customermaster1.companyid=cid;
                    obj_customermaster1.updatedby = uid;
                    obj_customermaster1.updateddate = DateTime.Now;
                    _context.Entry(obj_customermaster1).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_customermaster1).Property("createdby").IsModified = false;
                    _context.Entry(obj_customermaster1).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api customermasters ");
                _context.SaveChanges();




                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_customerid = new { @cid = cid, @uid = uid, @customerid = obj_customermaster1.customer_id };
                    var SQL = "select email,concat(firstname,'',lastname) as cusname from customermasters where customerid=@customerid";
                    var result = connection.Query<dynamic>(SQL, parameters_customerid);
                    var obj_cutomerid = result.FirstOrDefault();
                    email = obj_cutomerid.email;
                    cusname = obj_cutomerid.cusname;




                    var parameters_payee = new { @cid = cid, @uid = uid, @customerid = obj_customermaster1.payeeid };
                    var SQL_payee = "select email,concat(firstname,'',lastname) as cusname from customermasters where customerid=@customerid";
                    var resultpayee = connection.Query<dynamic>(SQL_payee, parameters_payee);
                    var objpayee = resultpayee.FirstOrDefault();
                    payee_email = obj_cutomerid.email;
                    payeename = obj_cutomerid.cusname;



                    var parameters_purpose = new { @cid = cid, @uid = uid, @purpose=obj_customermaster1.purpose };
                    var SQL_purpose = "select  masterdataid as value,masterdatadescription as description from masterdatatypes inner join masterdatas on datatypeid = masterdatatypeid where masterdatatypename = 'Rent Types' and masterdatatypeid = 12 and masterdataid = @purpose order by masterdatadescription";
                    var result_purpose = connection.Query<dynamic>(SQL_purpose, parameters_purpose);
                    var obj_purpose = result_purpose.FirstOrDefault();
                    value = obj_cutomerid.value;
                    purpose = obj_cutomerid.description;



                }


                if (obj_customermaster1.status == "A")
                {
                    string subject = "Your Purpose '"+ purpose+ "' Details for Payee '"+ payeename + "' have been verified ";
                    StringBuilder sb = new StringBuilder();
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    sb.Append("Hey '"+ cusname + "',");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Congratulations. We have verified Your Purpose '" + purpose + "' Details for Payee '" + payeename + "' and you are ready to make payment. Go to Start New Payment on your App or click here {weblink to Start New Payment}.");
                    sb.Append("<b>");
                    //sb.Append(TACNo);
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
                }
                if (obj_customermaster1.status == "R")
                {
                    string subject = "Your Purpose '" + purpose + "' Details for Payee '" + payeename + "' have been verified ";
                    StringBuilder sb = new StringBuilder();
                    SmtpClient smtp = new SmtpClient();
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    sb.Append("Hey '" + cusname + "',");
                    sb.Append("<br/>");
                    sb.Append("<br/>");
                    sb.Append("Sorry! We have Rejected Your Purpose '" + purpose + "' Details for Payee '" + payeename + "'.");
                    sb.Append("<br/>");
                    sb.Append("Reason :'"+ Reason+"' ");
                    sb.Append("<br/>");
                    sb.Append("Cheers,");
                    sb.Append("<br/>");
                    sb.Append("Carditnow");
                    // SendEmail(email, subject, sb.ToString());
                    //Helper.SendEmail();
                    //return "Success";

                    Helper.Email(sb.ToString(), email, cusname, subject);
                }

                if (obj_customermaster1.purpose=="34" || obj_customermaster1.purpose == "35"|| obj_customermaster1.purpose == "36")
                {
                    res = Get_customermaster((int)obj_customermaster1.customer_id);
                }
                if (obj_customermaster1.purpose == "37" || obj_customermaster1.purpose == "38")
                {
                    res = Get_customermaster1((int)obj_customermaster1.customer_id);
                }


             //  var res = Get_customermaster((int)obj_customermaster1.customer_id);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customerdocumentverification(string token,customerdocumentverification obj_customermaster1) \r\n{ex}");
                throw ex;
            }
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


                

                var result1 = new
                {
                    status = "success",
                    data = "",/* Application-specific data would go here. */
                    // OTP = TACNo.ToString(),//SHY
                    customerid = custmid,//SHY
                    geoid = geoids,
                    message = "succesfully save record" /* Or optional success message */
                };
                return JsonConvert.SerializeObject(result1);
                // }
                //else
                //{
                //    var result1 = new
                //    {
                //        status = "success",
                //        data = "",/* Application-specific data would go here. */
                //        OTP = TACNo.ToString(),
                //        customerid = custmid,
                //        geoid = geoids,
                //        message = "succesfully save record" /* Or optional success message */
                //    };
               
            

                    //return "Failed";
                }

            

            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetUserEmail_validat(string email) \r\n {ex}");
                throw ex;
            }
            //return "success";
        }






        public dynamic Get_customermasterdocument(string sid)
        {
            _logger.LogInfo("Getting into  Get_customermaster(string sid) api");
            int id = Helper.GetId(sid);
            return Get_customermasterdocument(id);
        }
        // GET: customermaster/5
        //gets the screen record
        public dynamic Get_customermasterdocument(int id)
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


                    var SQL = @"select pk_encode(m.verification_id) as pkcol,m.verification_id,m.documenttype,m.purpose,m.amount,case when m.status='P' then 'Pending' when m.status='A' then 'Approved' when m.status='R' then 'Rejected' end as status,m.uploadfilename,m.uploadpath,
concat(c.firstname,'',c.lastname) as customername,concat(cu.firstname,'',cu.lastname) as payeename,m.* from customer_document_verification m left join customermasters c on c.customerid=m.customer_id left join customermasters cu on cu.customerid=m.payeeid where m.verification_id=@id";


                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_customermaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customer_document_verification'";
                    var customermaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { customerdocumentverification = obj_customermaster, customermaster_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customermaster(int id)\r\n {ex}");
                throw ex;
            }
        }



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


                    var SQL = @"select pk_encode(m.verification_id) as pkcol,m.verification_id,m.documenttype,m.pro_address,m.purpose,m.amount as monthlyrent
,case when m.status='P' then 'Pending' when m.status='A' then 'Approved' when m.status='R' then 'Rejected' end as status,m.uploadfilename,m.uploadpath,
concat(c.firstname,'',c.lastname) as customername,concat(cu.firstname,'',cu.lastname) as payeename from customer_document_verification m left join customermasters c on c.customerid=m.customer_id left join customermasters cu on cu.customerid=m.payeeid where m.customer_id=@id";


                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_customermaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customer_document_verification'";
                    var customermaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { customerdocumentverification = obj_customermaster, customermaster_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customermaster(int id)\r\n {ex}");
                throw ex;
            }
        }



        public dynamic Get_customermaster1(int id)
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
                  

                    var SQL = @"select pk_encode(m.verification_id) as pkcol,m.verification_id,m.documenttype,m.invoicenumb,m.invocedate,m.purpose,m.amount
,case when m.status='P' then 'Pending' when m.status='A' then 'Approved' when m.status='R' then 'Rejected' end as status,m.uploadfilename,m.uploadpath,
concat(c.firstname,'',c.lastname) as customername,concat(cu.firstname,'',cu.lastname) as payeename from customer_document_verification m left join customermasters c on c.customerid=m.customer_id left join customermasters cu on cu.customerid=m.payeeid where m.customer_id=@id";


                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_customermaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customer_document_verification'";
                    var customermaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { customerdocumentverification = obj_customermaster, customermaster_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customermaster(int id)\r\n {ex}");
                throw ex;
            }
        }





    }
}

