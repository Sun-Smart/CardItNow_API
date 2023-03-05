
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
using System.IO;

namespace carditnow.Services
{
    public class transactionmasterService : ItransactionmasterService
    {
        private readonly IConfiguration Configuration;
        private readonly transactionmasterContext _context;
        public readonly transactiondetailContext _td_context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ItransactionmasterService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public transactionmasterService(transactionmasterContext context,transactiondetailContext td_context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
        {
Configuration = configuration;
            _context = context;
            _logger = logger;
            _td_context = td_context;
            this.httpContextAccessor = objhttpContextAccessor;
        cid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
        uid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
        uname = "";
        uidemail = "";
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")!=null)uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid")!=null)uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
        }

        // GET: service/transactionmaster
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_transactionmasters()
        {
        _logger.LogInfo("Getting into Get_transactionmasters() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.transactionid) as pkcol,transactionid as value,uid as label from GetTable(NULL::public.transactionmasters,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_transactionmasters(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_transactionid(int transactionid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_transactionid(int transactionid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_transactionid = new { @cid = cid,@uid=uid ,@transactionid = transactionid  };
            var SQL = "select pk_encode(transactionid) as pkcol,transactionid as value,uid as label,* from GetTable(NULL::public.transactionmasters,@cid) where transactionid = @transactionid";
var result = connection.Query<dynamic>(SQL, parameters_transactionid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_transactionid(int transactionid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_transactionmaster(string sid)
{
        _logger.LogInfo("Getting into  Get_transactionmaster(string sid) api");
int id = Helper.GetId(sid);
  return  Get_transactionmaster(id);
}
        // GET: transactionmaster/5
//gets the screen record
        public  dynamic Get_transactionmaster(int id)
        {
        _logger.LogInfo("Getting into Get_transactionmaster(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";
string vtransactiontype ="transactiontype";
string vpaytype ="paytype";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vtransactiontype =vtransactiontype,@vpaytype =vpaytype};
var SQL = @"select pk_encode(a.transactionid) as pkcol,a.transactionid as pk,a.*,
u.email as uiddesc,
r.email as recipientuiddesc,

                              y.configtext as transactiontypedesc,
                              pt.configtext as paytypedesc,
d.cardname || '  ' || d.cardnumber as payiddesc
 from GetTable(NULL::public.transactionmasters,@cid) a 
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 
 left join boconfigvalues y on a.transactiontype=y.configkey and @vtransactiontype=y.param
 left join boconfigvalues pt on a.paytype=pt.configkey and @vpaytype=pt.param
 left join customerpaymodes d on a.payid=d.payid
 where a.transactionid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_transactionmaster = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'transactionmasters'";
var transactionmaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { transactionmaster=obj_transactionmaster,transactionmaster_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_transactionmaster(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.transactionid) as pkcol,a.transactionid as pk,* ,transactionid as value,uid as label  from GetTable(NULL::public.transactionmasters,@cid) a ";
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
string vtransactiontype ="transactiontype";
string vpaytype ="paytype";
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus,@vtransactiontype =vtransactiontype,@vpaytype =vpaytype};
            var SQL = @"select pk_encode(a.transactionid) as pkcol,a.transactionid as pk,a.*,
u.email as uiddesc,
r.email as recipientuiddesc,

                              y.configtext as transactiontypedesc,
                              pt.configtext as paytypedesc,
d.cardname || '  ' || d.cardnumber as payiddesc from GetTable(NULL::public.transactionmasters,@cid) a 
 left join customermasters u on a.uid=u.uid
 left join customermasters r on a.recipientuid=r.uid
 
 left join boconfigvalues y on a.transactiontype=y.configkey and @vtransactiontype=y.param
 left join boconfigvalues pt on a.paytype=pt.configkey and @vpaytype=pt.param
 left join customerpaymodes d on a.payid=d.payid";
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
        public  dynamic Save_transactionmaster(string token,transactionmaster obj_transactionmaster)
        {
            int transactionid = 0;
        _logger.LogInfo("Saving: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) ");
            try
            {
                string serr = "";
int querytype=0;
if( obj_transactionmaster.documentnumber!=null )
{
var parametersdocumentnumber =new {@cid=cid,@uid=uid, @documentnumber = obj_transactionmaster.documentnumber,@transactionid=obj_transactionmaster.transactionid };
                    if(Helper.Count("select count(*) from transactionmasters where   documentnumber =  @documentnumber ",parametersdocumentnumber)> 0) serr +="documentnumber is unique\r\n";
}

                //and (@transactionid == 0 ||  @transactionid == null ||  @transactionid < 0 || transactionid!=  @transactionid
                if (serr!="")
                {
            _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                    
                    //connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                //transactionmaster table
                if (obj_transactionmaster.transactionid == 0 || obj_transactionmaster.transactionid == null || obj_transactionmaster.transactionid<0)
{
//if(obj_transactionmaster.status=="" || obj_transactionmaster.status==null)
obj_transactionmaster.status="I";
//obj_transactionmaster.companyid=cid;
obj_transactionmaster.createdby=uid;
obj_transactionmaster.createddate=DateTime.Now;
                    _context.transactionmasters.Add((dynamic)obj_transactionmaster);
querytype=1;
}
                else
{
//obj_transactionmaster.companyid=cid;
obj_transactionmaster.updatedby=uid;
obj_transactionmaster.updateddate=DateTime.Now;
                    _context.Entry(obj_transactionmaster).State = EntityState.Modified;
//when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_transactionmaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_transactionmaster).Property("createddate").IsModified = false;
querytype=2;
}
        _logger.LogInfo("saving api transactionmasters ");
                _context.SaveChanges();

                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_customerid = new { @cid = cid, @uid = uid };
                    var SQL = "select max(transactionid) as transactionid from transactionmasters";
                    var result = connection.Query<dynamic>(SQL, parameters_customerid);
                    var obj_cutomerid = result.FirstOrDefault();
                    transactionid = obj_cutomerid.transactionid;
                }

                    var cus_trans = new transactiondetail();
                cus_trans.transactionid = Convert.ToInt32(transactionid);
                cus_trans.uid = obj_transactionmaster.uid;
                cus_trans.recipientid = obj_transactionmaster.recipientid;
                cus_trans.recipientuid = obj_transactionmaster.recipientuid;
                cus_trans.payid = obj_transactionmaster.payid;
                cus_trans.transactiondate = DateTime.Now;
                cus_trans.status = "I";
                cus_trans.transactionamount = obj_transactionmaster.payamount;
                _td_context.Add(cus_trans);
                _td_context.SaveChanges();


                //_context.customermasters.Add(cus_master);

                Helper.AfterExecute(token,querytype,obj_transactionmaster,"transactionmasters", 0,obj_transactionmaster.transactionid,"",null, _logger);


//After saving, send the whole record to the front end. What saved will be shown in the screen
var res= Get_transactionmaster( (int)obj_transactionmaster.transactionid);
return (res);
            }
            catch (Exception ex)
            {

            _logger.LogError($"Service: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) \r\n{ex}");
                throw ex;
            }
        }


        //saving new method

        public dynamic Save_transactionmaster1(string token, transactionmaster obj_transactionmaster)
        {
            int transactionid = 0;
            _logger.LogInfo("Saving: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) ");
            try
            {
                var res = "";
                string serr = "";
                int querytype = 0;
                if (obj_transactionmaster.documentnumber != null)
                {
                    var parametersdocumentnumber = new { @cid = cid, @uid = uid, @documentnumber = obj_transactionmaster.documentnumber, @transactionid = obj_transactionmaster.transactionid };
                    if (Helper.Count("select count(*) from transactionmasters where   documentnumber =  @documentnumber ", parametersdocumentnumber) > 0) serr += "documentnumber is unique\r\n";
                }

                //and (@transactionid == 0 ||  @transactionid == null ||  @transactionid < 0 || transactionid!=  @transactionid
                if (serr != "")
                {
                    //_logger.LogError($"Validation error-save: {serr}");
                    //throw new Exception(serr);

                    res = serr;
                }

                else
                {


                    if (obj_transactionmaster.transactionid == 0 || obj_transactionmaster.transactionid == null || obj_transactionmaster.transactionid < 0)
                    {
                        //if(obj_transactionmaster.status=="" || obj_transactionmaster.status==null)
                        obj_transactionmaster.status = "I";
                        //obj_transactionmaster.companyid=cid;
                        obj_transactionmaster.createdby = uid;
                        obj_transactionmaster.createddate = DateTime.Now;
                        _context.transactionmasters.Add((dynamic)obj_transactionmaster);
                        querytype = 1;
                    }
                    else
                    {
                        //obj_transactionmaster.companyid=cid;
                        obj_transactionmaster.updatedby = uid;
                        obj_transactionmaster.updateddate = DateTime.Now;
                        _context.Entry(obj_transactionmaster).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_transactionmaster).Property("createdby").IsModified = false;
                        _context.Entry(obj_transactionmaster).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api transactionmasters ");
                    _context.SaveChanges();

                    using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                    {
                        var parameters_customerid = new { @cid = cid, @uid = uid };
                        var SQL = "select max(transactionid) as transactionid from transactionmasters";
                        var result = connection.Query<dynamic>(SQL, parameters_customerid);
                        var obj_cutomerid = result.FirstOrDefault();
                        transactionid = obj_cutomerid.transactionid;
                    }

                    var cus_trans = new transactiondetail();
                    cus_trans.transactionid = Convert.ToInt32(transactionid);
                    cus_trans.uid = obj_transactionmaster.uid;
                    cus_trans.recipientid = obj_transactionmaster.recipientid;
                    cus_trans.recipientuid = obj_transactionmaster.recipientuid;
                    cus_trans.payid = obj_transactionmaster.payid;
                    cus_trans.transactiondate = DateTime.Now;
                    cus_trans.status = "I";
                    cus_trans.transactionamount = obj_transactionmaster.payamount;
                    _td_context.Add(cus_trans);
                    _td_context.SaveChanges();


                    //_context.customermasters.Add(cus_master);

                    Helper.AfterExecute(token, querytype, obj_transactionmaster, "transactionmasters", 0, obj_transactionmaster.transactionid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    res = Get_transactionmaster((int)obj_transactionmaster.transactionid);
                }
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: transactionmaster/5
        //delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
transactionmaster obj_transactionmaster = _context.transactionmasters.Find(id);
_context.transactionmasters.Remove(obj_transactionmaster);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api transactionmasters ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }

        private bool transactionmaster_Exists(int id)
        {
        try{
            return _context.transactionmasters.Count(e => e.transactionid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:transactionmaster_Exists(int id) {ex}");
            return false;
        }
        }



        public dynamic Get_purpose()
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select * from masterdatas where masterdatatypeid=11 and status='A'";
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


        public dynamic Get_payee()
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select * from customermasters where type='P' and status='A'";
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




        public dynamic transactiondocument(string token, transactionmaster obj_transactionmaster)
        {
            _logger.LogInfo("Saving: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) ");
            try
            {
                string serr = "";
                int querytype = 0;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://textract.extrieve.in/api/newjob");
                    HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;


                }



                    var res = "";
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) \r\n{ex}");
                throw ex;
            }
        }






        public dynamic newjob()
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                string jobidd = string.Empty;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://textract.extrieve.in/api/newjob");
                    HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
                    string jobid= response.StatusCode.ToString();
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    JObject Jobj = JsonConvert.DeserializeObject<JObject>(responseString);

                    if (Jobj.Count > 0)
                    {
                         jobidd = Jobj["jobid"].ToString();
                        
                    }

                            return jobidd;
                }
               
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_purpose(): {ex}");
                throw ex;
            }
            return null;
        }





        public async  Task<dynamic> jobdoc(jobdoc model)
        {
            _logger.LogInfo("Getting into Get_purpose() api");
            try
            {
                string jobidd = string.Empty;
                string docid = string.Empty;
                string uploaded = string.Empty;
                string processed = string.Empty;
                string analysed = string.Empty;

               // using (var client = new HttpClient())
                //{
                    //client.BaseAddress = new Uri("https://textract.extrieve.in/api/jobdoc");
                    //HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
                    //string jobid = response.StatusCode.ToString();
                    //var responseString = response.Content.ReadAsStringAsync().Result;
                    //JObject Jobj = JsonConvert.DeserializeObject<JObject>(responseString);




                    byte[] imageBytes33 = Convert.FromBase64String(model.uploadDoc);

                    string url = "https://textract.extrieve.in/api/jobdoc";
                    // string content = "splicerapi_jobid='"+model.jobid+ "'&uploadDoc='"+model.uploadDoc+"'";
                    HttpClientHandler handler = new HttpClientHandler();
                   // HttpClient httpClient = new HttpClient(handler);
                    HttpClient httpClient = new HttpClient(handler);
                    httpClient.DefaultRequestHeaders.Add("splicerapi_jobid", model.jobid);
                    MemoryStream memory = new MemoryStream(imageBytes33);
                    HttpContent content = new StreamContent(memory);

                    HttpResponseMessage res = await httpClient.PostAsync(url, content);

                    Task.Delay(100).Wait();
                    //var res=httpClient.PostAsync(url, content);

                    var responseString = res.Content.ReadAsStringAsync().Result;
                    JObject Jobj = JsonConvert.DeserializeObject<JObject>(responseString);

                    if (Jobj.Count > 0)
                    {
                        jobidd = Jobj["jobid"].ToString();
                        docid = Jobj["docid"].ToString();

                    }

                    do
                    {
                    HttpClientHandler handler1 = new HttpClientHandler();
                    HttpClient httpClient2 = new HttpClient(handler1);
                    httpClient2.BaseAddress = new Uri("https://textract.extrieve.in/api/status?jobid=" + jobidd + "&docid=" + docid + "");
                        HttpResponseMessage response = httpClient2.GetAsync(httpClient2.BaseAddress).Result;
                        string jobid = response.StatusCode.ToString();
                        var responseString1 = response.Content.ReadAsStringAsync().Result;
                        JObject Jobj1 = JsonConvert.DeserializeObject<JObject>(responseString1);

                        //JObject obj_parent2 = Jobj1.GetValue("")[0] as JObject;

                        foreach (KeyValuePair<string, JToken> item in Jobj1)
                        {
                            JProperty p2 = Jobj1.Property(item.Key);

                            if (item.Key == "status")
                            {
                                string st = item.Value.ToString();
                            }
                            if (item.Key == "documents")
                            {
                                string doc = item.Value.ToString();
                            }


                                if (item.Key == "documents")
                            {
                                var documentss = item.Value.ToString();

                                JArray array = JArray.Parse(documentss);
                                JArray jsonArray = JArray.Parse(documentss);

                                foreach (JObject contentd in array.Children<JObject>())
                                {
                                    foreach (JProperty prop in contentd.Properties())

                                    {
                                        string Name = prop.Name.ToString().Trim();
                                        string Value = prop.Value.ToString().Trim();

                                        if (Name == "uploaded")
                                        {
                                            uploaded = Value.ToString();

                                        }

                                        if (Name == "processed")
                                        {
                                            processed = item.Value.ToString();
                                        }
                                        if (Name == "analysed")
                                        {
                                            analysed = item.Value.ToString();
                                        }



                                    }



                                }
                            }
                        }
                    }

                    while (uploaded != "True" && processed != "True" && analysed != "True");
               // }

                    var client2 = new HttpClient();

                    client2.BaseAddress = new Uri("https://textract.extrieve.in/api/analysis?jobid=" + jobidd + "&docid=" + docid + "");
                    HttpResponseMessage response2 = client2.GetAsync(client2.BaseAddress).Result;
                    string jobid2 = response2.StatusCode.ToString();
                    var responseString2 = response2.Content.ReadAsStringAsync().Result;
                    JObject Jobj2 = JsonConvert.DeserializeObject<JObject>(responseString2);

                    return Jobj2;
                


            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_purpose(): {ex}");
                throw ex;
            }
            return null;
        }





        public dynamic billamountcalculation(string token, transactionmaster obj_transactionmaster)
        {
            _logger.LogInfo("Saving: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) ");
            try
            {
                string result = string.Empty;
                string serr = "";
                int querytype = 0;
               // string billamount = string.Empty;
                double billamount;
                int bamount = 0;
                string convtype = string.Empty;
                string percentage = string.Empty;
                //var CC_carditnowfee = "";
                //var NB_carditnowfee = "";
                int CC_carditnowfee = 0;
                int NB_carditnowfee = 0;
                int CC_totalamount = 0;
                int NB_totalamount = 0;
                double CC_carditnowfee1=0.0;
                double CC_totalamount1 = 0.0;
                double NB_carditnowfee1 = 0.0;
                double NB_totalamount1 = 0.0;
                billamountclass obj_class = new billamountclass();

                DataTable dataTable = new DataTable();
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                 billamount = obj_transactionmaster.billamount;
                bamount = Convert.ToInt32(billamount);

                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select convtype,percentage,max(effectivefrom) from  carditconvfee where effectivefrom <= NOW() and status='A'group by convtype,percentage ";

                    NpgsqlCommand cmd = new NpgsqlCommand(SQL, connection);
                    var reader = cmd.ExecuteReader();
                    System.Data.DataTable results = new System.Data.DataTable();
                    results.Load(reader);
                    if (results.Rows.Count > 0)
                    {
                        for (int i = 0; i < results.Rows.Count; i++)
                        {
                            convtype = results.Rows[i]["convtype"].ToString();
                            percentage = results.Rows[i]["percentage"].ToString();

                            if (convtype == "CC")
                            {
                                int per = Convert.ToInt32(percentage);

                                var value = ((double)bamount * per) / 100;
                                // CC_carditnowfee = Convert.ToInt32(Math.Round(value, 0));
                                CC_carditnowfee = (int)((double)(value));
                                CC_carditnowfee1 = value;
                                CC_totalamount1 = (double)billamount + value;


                            }
                            if (convtype == "NB")
                            {
                                int per = Convert.ToInt32(percentage);

                                var value = ((double)bamount * per) / 100;
                                NB_carditnowfee = Convert.ToInt32(value);
                                NB_totalamount = bamount + NB_carditnowfee;
                                NB_carditnowfee1 = value;
                                NB_totalamount1 = (double)billamount + value;

                            }

                        }




                        obj_class.billamount = bamount.ToString();
                        obj_class.CC_carditnowfee = CC_carditnowfee1.ToString();
                        obj_class.CC_totalamount = CC_totalamount1.ToString();
                        obj_class.NB_carditnowfee = NB_carditnowfee1.ToString();
                        obj_class.NB_totalamount = NB_totalamount1.ToString();
                        obj_class.paystatus = "Paying";
                        obj_class.feereason = "";
                    }


                }

                dynamic resvalue = JsonConvert.SerializeObject(obj_class);
                //dataTable.Rows.Add(obj_class);
                //result = DataTableToJSONWithStringBuilder(dataTable);

                //            string creditApplicationJson = JsonConvert.SerializeObject(
                //new
                //{
                //    jsonCreditApplication = obj_class
                //});

                return (resvalue);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_transactionmaster(string token,transactionmaster obj_transactionmaster) \r\n{ex}");
                throw ex;
            }
        }


        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString().Trim() + "\":" + "\"" + table.Rows[i][j].ToString().Trim() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString().Trim() + "\":" + "\"" + table.Rows[i][j].ToString().Trim() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

    }
}

