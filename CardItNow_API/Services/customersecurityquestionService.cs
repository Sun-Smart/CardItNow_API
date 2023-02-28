
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

namespace carditnow.Services
{
    public class customersecurityquestionService : IcustomersecurityquestionService
    {
        private readonly IConfiguration Configuration;
        private readonly customersecurityquestionContext _context;
        private readonly customersecurityquestionshistoryContext _history_context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IcustomersecurityquestionService _service;
int cid=0;
int uid=0;
string uname="";
string uidemail="";




        public customersecurityquestionService(customersecurityquestionContext context, customersecurityquestionshistoryContext history_context,IConfiguration configuration, ILoggerManager logger,  IHttpContextAccessor objhttpContextAccessor )
        {
Configuration = configuration;
            _context = context;
            _logger = logger;
            _history_context = history_context;
            this.httpContextAccessor = objhttpContextAccessor;
            if (httpContextAccessor.HttpContext.User.Claims.Any())
            {
              //  cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
              //  uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                //if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                //if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
            }
        }

        // GET: service/customersecurityquestion
//Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_customersecurityquestions()
        {
        _logger.LogInfo("Getting into Get_customersecurityquestions() api");
        try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            
        var parameters = new { @cid = cid,@uid=uid };
    string SQL = "select pk_encode(a.securityquestionid) as pkcol,securityquestionid as value,answer as label from GetTable(NULL::public.customersecurityquestions,@cid) a  WHERE  a.status='A'";
        var result = connection.Query<dynamic>(SQL, parameters);
            connection.Close();
            connection.Dispose();
            return result;
        }
        } catch (Exception ex) {
            _logger.LogError($"Service : Get_customersecurityquestions(): {ex}");
            throw ex;
        }
        return null;
        }


        public  IEnumerable<Object> GetListBy_securityquestionid(int securityquestionid)
        {
        try{
        _logger.LogInfo("Getting into  GetListBy_securityquestionid(int securityquestionid) api");
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        var parameters_securityquestionid = new { @cid = cid,@uid=uid ,@securityquestionid = securityquestionid  };
            var SQL = "select pk_encode(securityquestionid) as pkcol,securityquestionid as value,answer as label,* from GetTable(NULL::public.customersecurityquestions,@cid) where securityquestionid = @securityquestionid";
var result = connection.Query<dynamic>(SQL, parameters_securityquestionid);

            connection.Close();
            connection.Dispose();
            return (result);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service:  GetListBy_securityquestionid(int securityquestionid) \r\n {ex}");
            throw ex;
        }
        }
//used in getting the record. parameter is encrypted id  
public  dynamic Get_customersecurityquestion(string sid)
{
        _logger.LogInfo("Getting into  Get_customersecurityquestion(string sid) api");
int id = Helper.GetId(sid);
  return  Get_customersecurityquestion(id);
}
        // GET: customersecurityquestion/5
//gets the screen record
        public  dynamic Get_customersecurityquestion(int id)
        {
        _logger.LogInfo("Getting into Get_customersecurityquestion(int id) api");
try{
        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
        
//all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
ArrayList visiblelist=new ArrayList();
ArrayList hidelist=new ArrayList();


string wStatus = "NormalStatus";

var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
var SQL = @"select pk_encode(a.securityquestionid) as pkcol,a.securityquestionid as pk,a.*,
c.UID as customeriddesc,
d.masterdatadescription as questioniddesc
 from GetTable(NULL::public.customersecurityquestions,@cid) a 
 left join customermasters c on a.customerid=c.customerid
 left join masterdatas d on a.questionid=d.masterdataid
 where a.securityquestionid=@id";
var result = connection.Query<dynamic>(SQL, parameters);
var obj_customersecurityquestion = result.FirstOrDefault();
var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customersecurityquestions'";
var customersecurityquestion_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
FormProperty formproperty=new FormProperty();
formproperty.edit=true;


            connection.Close();
            connection.Dispose();
            return (new { customersecurityquestion=obj_customersecurityquestion,customersecurityquestion_menuactions,formproperty,visiblelist,hidelist });
}
}catch(Exception ex)
{
            _logger.LogError($"Service: Get_customersecurityquestion(int id)\r\n {ex}");
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
            var SQL = @"select  pk_encode(a.securityquestionid) as pkcol,a.securityquestionid as pk,* ,securityquestionid as value,answer as label  from GetTable(NULL::public.customersecurityquestions,@cid) a ";
if(condition!="")SQL+=" and "+condition;
SQL+=" order by answer";
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
var parameters = new { @cid=cid,@uid=uid,@id=id,@wStatus=wStatus};
            var SQL = @"select pk_encode(a.securityquestionid) as pkcol,a.securityquestionid as pk,a.*,
c.UID as customeriddesc,
d.masterdatadescription as questioniddesc from GetTable(NULL::public.customersecurityquestions,@cid) a 
 left join customermasters c on a.customerid=c.customerid
 left join masterdatas d on a.questionid=d.masterdataid";
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

       // public dynamic Save_customersecurityquestion(string token, customersecurityquestion obj_customersecurityquestion)
        public  dynamic Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion)
        {
        _logger.LogInfo("Saving: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) ");
            try
            {
                string serr = "";
                int querytype = 0;
                int historyid = 0;
                string oldanswer = string.Empty;
                //int securityquestionid1 = 0;
                //int customerid1 = 0;
                //int questionid1 = 0;
                //string answer1 = string.Empty;
                //string status1 = string.Empty;

                int countt1 = 0;

                customersecurityquestionshistory obj_customersecurityquestionshistory = new customersecurityquestionshistory();



                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    connection.Open();




                        if (obj_customersecurityquestion.securityquestionid == 0 || obj_customersecurityquestion.securityquestionid == null || obj_customersecurityquestion.securityquestionid < 0)



                           // if (securityquestionid1 == 0 || securityquestionid1 == null || securityquestionid1 < 0)

                            {
                            //if (obj_customersecurityquestion.status == "" || obj_customersecurityquestion.status == null)
                                
                                obj_customersecurityquestion.status = "A";
                            //obj_customersecurityquestion.customerid = customerid1;
                            //obj_customersecurityquestion.questionid = questionid1;
                            //obj_customersecurityquestion.answer = answer1;
                            //obj_customersecurityquestion.companyid=cid;
                            obj_customersecurityquestion.createdby = uid;
                            obj_customersecurityquestion.createddate = DateTime.Now;

                            _context.customersecurityquestions.Add((dynamic)obj_customersecurityquestion);

                            querytype = 1;
                        }
                        else
                        {
                            //obj_customersecurityquestion.companyid=cid;
                            obj_customersecurityquestion.updatedby = uid;
                            obj_customersecurityquestion.updateddate = DateTime.Now;
                            obj_customersecurityquestion.status = "A";
                            //obj_customersecurityquestion.customerid = customerid1;
                            //obj_customersecurityquestion.questionid = questionid1;
                            //obj_customersecurityquestion.answer = answer1;
                            _context.Entry(obj_customersecurityquestion).State = EntityState.Modified;
                            //when IsModified = false, it will not update these fields.so old values will be retained
                            _context.Entry(obj_customersecurityquestion).Property("createdby").IsModified = false;
                            _context.Entry(obj_customersecurityquestion).Property("createddate").IsModified = false;
                            querytype = 2;
                        }
                        _logger.LogInfo("saving api customersecurityquestions ");
                        _context.SaveChanges();


                        var parametersapllicantreg = new { @cid = cid, @customerid = obj_customersecurityquestion.customerid };
                        //if (Helper.Count("select count(*) as count from customersecurityquestionshistories where customerid=@customerid", parametersapllicantreg) <= 0)


                        string SQLeducationcategory = "select count(*) as count from customersecurityquestionshistories where  customerid=@customerid";
                        var rsecurityquestionid = connection.Query<dynamic>(SQLeducationcategory, parametersapllicantreg);
                        int countt = rsecurityquestionid.Count();

                        if (countt > 0)
                        {


                            var parametersapllicantreg1 = new { @cid = cid, @securityquestionid = obj_customersecurityquestion.securityquestionid, @customerid = obj_customersecurityquestion.customerid };

                            string SQLeducationcategory1 = "select count(*) as count from customersecurityquestionshistories where securityquestionid='"+ obj_customersecurityquestion.securityquestionid+"' and  customerid='"+ obj_customersecurityquestion.customerid+"'";
                            //var rsecurityquestionid1 = connection.Query<dynamic>(SQLeducationcategory1, parametersapllicantreg1);
                            //int countt1 = rsecurityquestionid1.Count();


                        NpgsqlCommand cmdcurs = new NpgsqlCommand(SQLeducationcategory1, connection);
                        var readercurs = cmdcurs.ExecuteReader();
                        System.Data.DataTable resultscurs = new System.Data.DataTable();
                        resultscurs.Load(readercurs);
                        for (int i = 0; i < resultscurs.Rows.Count; i++)
                        {
                            DataRow row1 = resultscurs.Rows[i];
                            countt1 = Convert.ToInt32(resultscurs.Rows[i]["count"]);
                        }



                            //if (Helper.Count("select count(*) as count from customersecurityquestionshistories where securityquestionid=@securityquestionid and  customerid=@customerid", parametersapllicantreg1) <= 0)
                            if (countt1 == 0)
                            {

                                // var parsecurityquestionid = new { @customerid = obj_customersecurityquestion.customerid };
                                string SQLques = "select * from customersecurityquestions where customerid='" + obj_customersecurityquestion.customerid + "'";
                                NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                                var readercurr = cmdcurr.ExecuteReader();
                                System.Data.DataTable resultscurr = new System.Data.DataTable();
                                resultscurr.Load(readercurr);
                                for (int i = 0; i < resultscurr.Rows.Count; i++)
                                {
                                    DataRow row1 = resultscurr.Rows[i];
                                    int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                    int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                    int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                    string answer = resultscurr.Rows[i]["answer"].ToString();






                                    /* if (obj_customersecurityquestionshistory.status == "" || obj_customersecurityquestionshistory.status == null) */
                                    obj_customersecurityquestionshistory.status = "A";
                                    //obj_customersecurityquestionshistory.companyid=cid;
                                    obj_customersecurityquestionshistory.historyid = null;
                                    obj_customersecurityquestionshistory.createdby = uid;
                                    obj_customersecurityquestionshistory.createddate = DateTime.Now;
                                    obj_customersecurityquestionshistory.newanswer = answer;
                                    obj_customersecurityquestionshistory.customerid = customerid;
                                obj_customersecurityquestionshistory.questionid = questionid;
                                    obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                    _history_context.customersecurityquestionshistories.Add((dynamic)obj_customersecurityquestionshistory);
                                    querytype = 1;
                                    _logger.LogInfo("saving api customersecurityquestionshistories ");
                                    _history_context.SaveChanges();
                                }
                            }

                            else
                            {
                                string SQLques = "select * from customersecurityquestions where customerid='" + obj_customersecurityquestion.customerid + "'";
                                NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                                var readercurr = cmdcurr.ExecuteReader();
                                System.Data.DataTable resultscurr = new System.Data.DataTable();
                                resultscurr.Load(readercurr);
                                for (int i = 0; i < resultscurr.Rows.Count; i++)
                                {
                                    DataRow row1 = resultscurr.Rows[i];
                                    int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                    int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                    int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                    string answer = resultscurr.Rows[i]["answer"].ToString();

                                    string SQLques1 = "select * from customersecurityquestionshistories where customerid='" + obj_customersecurityquestion.customerid + "'  and securityquestionid='" + securityquestionid + "'";
                                    NpgsqlCommand cmdcurr1 = new NpgsqlCommand(SQLques1, connection);
                                    var readercurr1 = cmdcurr1.ExecuteReader();
                                    System.Data.DataTable resultscurr1 = new System.Data.DataTable();
                                    resultscurr1.Load(readercurr1);
                                    for (int i1 = 0; i1 < resultscurr1.Rows.Count; i1++)
                                    {
                                        DataRow row2 = resultscurr1.Rows[i];
                                        historyid = Convert.ToInt32(resultscurr1.Rows[i]["historyid"]);
                                        oldanswer = resultscurr1.Rows[i]["newanswer"].ToString();
                                    }







                                    //obj_customersecurityquestionshistory.companyid=cid;
                                    obj_customersecurityquestionshistory.status = "A";
                                    obj_customersecurityquestionshistory.updatedby = uid;
                                    obj_customersecurityquestionshistory.historyid = historyid;
                                    obj_customersecurityquestionshistory.updateddate = DateTime.Now;
                                    obj_customersecurityquestionshistory.oldanswer = oldanswer;
                                    obj_customersecurityquestionshistory.newanswer = answer;
                                    obj_customersecurityquestionshistory.customerid = customerid;
                                    obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                    _history_context.Entry(obj_customersecurityquestionshistory).State = EntityState.Modified;
                                    //when IsModified = false, it will not update these fields.so old values will be retained
                                    _history_context.Entry(obj_customersecurityquestionshistory).Property("createdby").IsModified = false;
                                    _history_context.Entry(obj_customersecurityquestionshistory).Property("createddate").IsModified = false;

                                    querytype = 2;

                                    _logger.LogInfo("saving api customersecurityquestionshistories ");
                                    _history_context.SaveChanges();

                                }
                            }

                        }
                        else
                        {
                            string SQLques = "select * from customersecurityquestions where customerid='" + obj_customersecurityquestion.customerid + "'";
                            // var rsecurityquestionid = connection.Query<dynamic>(SQLeducationcategory, parsecurityquestionid);
                            //int countt = rsecurityquestionid.Count();
                            //System.Data.DataTable resultscurr = new System.Data.DataTable();
                            NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                            var readercurr = cmdcurr.ExecuteReader();
                            System.Data.DataTable resultscurr = new System.Data.DataTable();
                            resultscurr.Load(readercurr);
                            for (int i = 0; i < resultscurr.Rows.Count; i++)
                            {
                                DataRow row1 = resultscurr.Rows[i];
                                int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                string answer = resultscurr.Rows[i]["answer"].ToString();






                                /* if (obj_customersecurityquestionshistory.status == "" || obj_customersecurityquestionshistory.status == null) */
                                obj_customersecurityquestionshistory.status = "A";
                                //obj_customersecurityquestionshistory.companyid=cid;
                                //obj_customersecurityquestionshistory.historyid = null;
                                obj_customersecurityquestionshistory.createdby = uid;
                                obj_customersecurityquestionshistory.createddate = DateTime.Now;
                                obj_customersecurityquestionshistory.newanswer = answer;
                                obj_customersecurityquestionshistory.customerid = customerid;
                                obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                _history_context.customersecurityquestionshistories.Add((dynamic)obj_customersecurityquestionshistory);
                                querytype = 1;
                                _logger.LogInfo("saving api customersecurityquestionshistories ");
                                _history_context.SaveChanges();

                            }

                        }
                    }
                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_customersecurityquestion, "customersecurityquestions", 0, obj_customersecurityquestion.securityquestionid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_customersecurityquestion((int)obj_customersecurityquestion.securityquestionid);
                    return (res);
                }
            //}
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) \r\n{ex}");
                throw ex;
            }
        }




        //saving of multiple records


        public dynamic Save_customersecuritymultiquestions(string token,dynamic data)
        {
            _logger.LogInfo("Saving: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) ");
            try
            {
                string serr = "";
                int querytype = 0;
                int historyid = 0;
                string oldanswer = string.Empty;
                int securityquestionid1 = 0;
                int customerid1 = 0;
                int questionid1 = 0;
                string answer1 = string.Empty;
                string status1 = string.Empty;
                int count = 0;

                customersecurityquestionshistory obj_customersecurityquestionshistory = new customersecurityquestionshistory();



                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();


                    //JObject obj_parent = JsonConvert.DeserializeObject<JObject>(data);
                    //JObject obj_parent1 = obj_parent.GetValue("securityquestions")[0] as JObject;
                    //foreach (KeyValuePair<string, JToken> item in obj_parent1)
                    //{
                    //    JProperty p1 = obj_parent1.Property(item.Key);

                    //    if (item.Key == "securityquestionid")
                    //    {
                    //        securityquestionid1 = Convert.ToInt32(item.Value);
                    //    }
                    //    if (item.Key == "customerid")
                    //    {
                    //        customerid1 = Convert.ToInt32(item.Value);
                    //    }
                    //    if (item.Key == "questionid")
                    //    {
                    //        questionid1 = Convert.ToInt32(item.Value);
                    //    }
                    //    if (item.Key == "answer")
                    //    {
                    //        answer1 = item.Value.ToString();

                    //    }
                    //    if (item.Key == "status")
                    //    {
                    //        status1 = item.Value.ToString();
                    //    }





                        //if (obj_customersecurityquestion.securityquestionid == 0 || obj_customersecurityquestion.securityquestionid == null || obj_customersecurityquestion.securityquestionid < 0)



                     if (securityquestionid1 == 0 || securityquestionid1 == null || securityquestionid1 < 0)

                    {
                        //if (obj_customersecurityquestion.status == "" || obj_customersecurityquestion.status == null)

                        data.status = "A";
                            data.customerid = customerid1;
                            data.questionid = questionid1;
                            data.answer = answer1;
                            //obj_customersecurityquestion.companyid = cid;
                            data.createdby = uid;
                            data.createddate = DateTime.Now;

                        _context.customersecurityquestions.Add((dynamic)data);

                        querytype = 1;
                    }
                    else
                    {
                            //obj_customersecurityquestion.companyid=cid;
                            data.updatedby = uid;
                            data.updateddate = DateTime.Now;
                            data.status = "A";
                            data.customerid = customerid1;
                            data.questionid = questionid1;
                            data.answer = answer1;
                            _context.Entry(data).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(data).Property("createdby").IsModified = false;
                        _context.Entry(data).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api customersecurityquestions ");
                    _context.SaveChanges();


                    var parametersapllicantreg = new { @cid = cid, @customerid = customerid1 };
                    //if (Helper.Count("select count(*) as count from customersecurityquestionshistories where customerid=@customerid", parametersapllicantreg) <= 0)


                    string SQLeducationcategory = "select count(*) as count from customersecurityquestionshistories where  customerid=@customerid";
                    var rsecurityquestionid = connection.Query<dynamic>(SQLeducationcategory, parametersapllicantreg);
                    int countt = rsecurityquestionid.Count();

                    if (countt > 0)
                    {


                        var parametersapllicantreg1 = new { @cid = cid, @securityquestionid = securityquestionid1, @customerid = customerid1 };

                        string SQLeducationcategory1 = "select count(*) as count from customersecurityquestionshistories where securityquestionid=@securityquestionid and  customerid=@customerid";
                        var rsecurityquestionid1 = connection.Query<dynamic>(SQLeducationcategory1, parametersapllicantreg1);
                        int countt1 = rsecurityquestionid1.Count();



                        //if (Helper.Count("select count(*) as count from customersecurityquestionshistories where securityquestionid=@securityquestionid and  customerid=@customerid", parametersapllicantreg1) <= 0)
                        if (countt1 == 0)
                        {

                            // var parsecurityquestionid = new { @customerid = obj_customersecurityquestion.customerid };
                            string SQLques = "select * from customersecurityquestions where customerid='" + customerid1 + "'";
                            NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                            var readercurr = cmdcurr.ExecuteReader();
                            System.Data.DataTable resultscurr = new System.Data.DataTable();
                            resultscurr.Load(readercurr);
                            for (int i = 0; i < resultscurr.Rows.Count; i++)
                            {
                                DataRow row1 = resultscurr.Rows[i];
                                int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                string answer = resultscurr.Rows[i]["answer"].ToString();






                                /* if (obj_customersecurityquestionshistory.status == "" || obj_customersecurityquestionshistory.status == null) */
                                obj_customersecurityquestionshistory.status = "A";
                                //obj_customersecurityquestionshistory.companyid=cid;
                                obj_customersecurityquestionshistory.historyid = null;
                                obj_customersecurityquestionshistory.createdby = uid;
                                obj_customersecurityquestionshistory.createddate = DateTime.Now;
                                obj_customersecurityquestionshistory.oldanswer = answer;
                                obj_customersecurityquestionshistory.customerid = customerid;
                                obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                _history_context.customersecurityquestionshistories.Add((dynamic)obj_customersecurityquestionshistory);
                                querytype = 1;
                                _logger.LogInfo("saving api customersecurityquestionshistories ");
                                _history_context.SaveChanges();
                            }
                        }

                        else
                        {
                            string SQLques = "select * from customersecurityquestions where customerid='" + customerid1 + "'";
                            NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                            var readercurr = cmdcurr.ExecuteReader();
                            System.Data.DataTable resultscurr = new System.Data.DataTable();
                            resultscurr.Load(readercurr);
                            for (int i = 0; i < resultscurr.Rows.Count; i++)
                            {
                                DataRow row1 = resultscurr.Rows[i];
                                int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                string answer = resultscurr.Rows[i]["answer"].ToString();

                                string SQLques1 = "select * from customersecurityquestionshistories where customerid='" + customerid1 + "'  and securityquestionid='" + securityquestionid1 + "'";
                                NpgsqlCommand cmdcurr1 = new NpgsqlCommand(SQLques1, connection);
                                var readercurr1 = cmdcurr1.ExecuteReader();
                                System.Data.DataTable resultscurr1 = new System.Data.DataTable();
                                resultscurr1.Load(readercurr1);
                                for (int i1 = 0; i1 < resultscurr1.Rows.Count; i1++)
                                {
                                    DataRow row2 = resultscurr1.Rows[i];
                                    historyid = Convert.ToInt32(resultscurr1.Rows[i]["historyid"]);
                                    oldanswer = resultscurr1.Rows[i]["newanswer"].ToString();
                                }







                                //obj_customersecurityquestionshistory.companyid=cid;
                                obj_customersecurityquestionshistory.status = "A";
                                obj_customersecurityquestionshistory.updatedby = uid;
                                obj_customersecurityquestionshistory.historyid = historyid;
                                obj_customersecurityquestionshistory.updateddate = DateTime.Now;
                                obj_customersecurityquestionshistory.oldanswer = oldanswer;
                                obj_customersecurityquestionshistory.newanswer = answer;
                                obj_customersecurityquestionshistory.customerid = customerid;
                                obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                _history_context.Entry(obj_customersecurityquestionshistory).State = EntityState.Modified;
                                //when IsModified = false, it will not update these fields.so old values will be retained
                                _history_context.Entry(obj_customersecurityquestionshistory).Property("createdby").IsModified = false;
                                _history_context.Entry(obj_customersecurityquestionshistory).Property("createddate").IsModified = false;

                                querytype = 2;

                                _logger.LogInfo("saving api customersecurityquestionshistories ");
                                _history_context.SaveChanges();

                            }
                        }

                    }
                    else
                    {
                        string SQLques = "select * from customersecurityquestions where customerid='" + customerid1 + "'";
                        // var rsecurityquestionid = connection.Query<dynamic>(SQLeducationcategory, parsecurityquestionid);
                        //int countt = rsecurityquestionid.Count();
                        //System.Data.DataTable resultscurr = new System.Data.DataTable();
                        NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                        var readercurr = cmdcurr.ExecuteReader();
                        System.Data.DataTable resultscurr = new System.Data.DataTable();
                        resultscurr.Load(readercurr);
                        for (int i = 0; i < resultscurr.Rows.Count; i++)
                        {
                            DataRow row1 = resultscurr.Rows[i];
                            int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                            int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                            int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                            string answer = resultscurr.Rows[i]["answer"].ToString();






                            /* if (obj_customersecurityquestionshistory.status == "" || obj_customersecurityquestionshistory.status == null) */
                            obj_customersecurityquestionshistory.status = "A";
                            //obj_customersecurityquestionshistory.companyid=cid;
                            //obj_customersecurityquestionshistory.historyid = null;
                            obj_customersecurityquestionshistory.createdby = uid;
                            obj_customersecurityquestionshistory.createddate = DateTime.Now;
                            obj_customersecurityquestionshistory.oldanswer = answer;
                            obj_customersecurityquestionshistory.customerid = customerid;
                            obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                            _history_context.customersecurityquestionshistories.Add((dynamic)obj_customersecurityquestionshistory);
                            querytype = 1;
                            _logger.LogInfo("saving api customersecurityquestionshistories ");
                            _history_context.SaveChanges();

                        }

                    }
                }
                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                Helper.AfterExecute(token, querytype, data, "customersecurityquestions", 0, data.securityquestionid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_customersecurityquestion((int)data.securityquestionid);
                return (res);
            }
          //  }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) \r\n{ex}");
                throw ex;
            }
        }



        //multi question and answer in array



        public dynamic Save_customerallsecuritymultiquestions(dynamic data)
        {
            _logger.LogInfo("Saving: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) ");
            try
            {
                string serr = "";
                int querytype = 0;
                int historyid = 0;
                string oldanswer = string.Empty;
                int securityquestionid1 = 0;
                int customerid1 = 0;
                int questionid1 = 0;
                string answer1 = string.Empty;
                string status1 = string.Empty;
                int countt = 0;
                int count = 0;

                customersecurityquestionshistory obj_customersecurityquestionshistory = new customersecurityquestionshistory();
                customersecurityquestion obj_customersecurityquestion = new customersecurityquestion();


                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();


                    //JObject obj_parent = JsonConvert.DeserializeObject<JObject>(data);
                    //JObject obj_parent1 = obj_parent.GetValue("securityquestions")[0] as JObject;



                    JObject obj_parents = JsonConvert.DeserializeObject<JObject>(data.ToString());

                    JObject obj_parent2 = obj_parents.GetValue("securityquestions")[0] as JObject;


                    foreach (KeyValuePair<string, JToken> item in obj_parent2)
                    {
                        JProperty p2 = obj_parent2.Property(item.Key);



                        if (item.Key == "questiondetails")
                        {
                            var Vendor_details = item.Value.ToString();

                            JArray array = JArray.Parse(Vendor_details);
                            JArray jsonArray = JArray.Parse(Vendor_details);

                            foreach (JObject content in array.Children<JObject>())
                            {
                                foreach (JProperty prop in content.Properties())
                                {
                                    string Name = prop.Name.ToString().Trim();
                                    string Value = prop.Value.ToString().Trim();
                                    if (Name == "securityquestionid")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            securityquestionid1 = 0;
                                        }
                                        else
                                        {
                                            securityquestionid1 = Convert.ToInt32(Value);
                                        }

                                    }

                                    if (Name == "customerid")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            customerid1 = 0;
                                        }
                                        else
                                        {
                                            customerid1 = Convert.ToInt32(Value);
                                        }

                                    }
                                    if (Name == "questionid")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            questionid1 = 0;
                                        }
                                        else
                                        {
                                            questionid1 = Convert.ToInt32(Value);
                                        }

                                    }


                                    if (Name == "answer")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            answer1 = null;
                                        }
                                        else
                                        {
                                            answer1 = Value.ToString();
                                        }

                                    }

                                    if (Name == "status")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            status1 = null;
                                        }
                                        else
                                        {
                                            status1 = Value.ToString();
                                        }

                                    }

                                }







                                if (securityquestionid1 == 0 || securityquestionid1 == null || securityquestionid1 < 0)

                                {



                                    string SQLques = "select count(*) as count from customersecurityquestions where customerid='" + customerid1 + "' and questionid='" + questionid1 + "'";
                                    NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                                    var readercurr = cmdcurr.ExecuteReader();
                                    System.Data.DataTable resultscurr = new System.Data.DataTable();
                                    resultscurr.Load(readercurr);
                                    for (int i = 0; i < resultscurr.Rows.Count; i++)
                                    {
                                        DataRow row1 = resultscurr.Rows[i];
                                        count = Convert.ToInt32(resultscurr.Rows[i]["count"]);

                                    }


                                   //     var parametercusque = new { @cid = cid, @customerid = customerid1, @questionid = questionid1 };
                                   // string SQLcusque = "select count(*) as count from customersecurityquestionshistories where  customerid=@customerid and questionid=@questionid";
                                   // var rcusque = connection.Query<dynamic>(SQLcusque, parametercusque);
                                   // int count = rcusque.Count();
                                   //var cusdata = rcusque.FirstOrDefault();
                                   // var dd = cusdata("count");
                                        if (count == 0)
                                    {

                                        obj_customersecurityquestion.securityquestionid = null;
                                        obj_customersecurityquestion.status = "A";
                                        obj_customersecurityquestion.customerid = customerid1;
                                        obj_customersecurityquestion.questionid = questionid1;
                                        obj_customersecurityquestion.answer = answer1;
                                        //obj_customersecurityquestion.companyid = cid;
                                        obj_customersecurityquestion.createdby = uid;
                                        obj_customersecurityquestion.createddate = DateTime.Now;
                                        _context.customersecurityquestions.Add((dynamic)obj_customersecurityquestion);

                                        querytype = 1;

                                    }
                                   
                                }
                                else
                                {
                                    //obj_customersecurityquestion.companyid=cid;
                                    obj_customersecurityquestion.updatedby = uid;
                                    obj_customersecurityquestion.updateddate = DateTime.Now;
                                    obj_customersecurityquestion.status = "A";
                                    obj_customersecurityquestion.customerid = customerid1;
                                    obj_customersecurityquestion.questionid = questionid1;
                                    obj_customersecurityquestion.answer = answer1;
                                    _context.Entry(obj_customersecurityquestion).State = EntityState.Modified;
                                    //when IsModified = false, it will not update these fields.so old values will be retained
                                    _context.Entry(obj_customersecurityquestion).Property("createdby").IsModified = false;
                                    _context.Entry(obj_customersecurityquestion).Property("createddate").IsModified = false;
                                    querytype = 2;
                                }
                                    _logger.LogInfo("saving api customersecurityquestions ");
                                    _context.SaveChanges();


                                    var parametersapllicantreg = new { @cid = cid, @customerid = customerid1 };
                                    //if (Helper.Count("select count(*) as count from customersecurityquestionshistories where customerid=@customerid", parametersapllicantreg) <= 0)


                                    string SQLeducationcategory = "select count(*) as count from customersecurityquestionshistories where  customerid='"+ customerid1 + "' and questionid='"+questionid1+"'";




                                NpgsqlCommand cmdcurr2 = new NpgsqlCommand(SQLeducationcategory, connection);
                                var readercurr2 = cmdcurr2.ExecuteReader();
                                System.Data.DataTable resultscurr2 = new System.Data.DataTable();
                                resultscurr2.Load(readercurr2);
                                for (int i = 0; i < resultscurr2.Rows.Count; i++)
                                {
                                    DataRow row1 = resultscurr2.Rows[i];
                                    countt = Convert.ToInt32(resultscurr2.Rows[i]["count"]);

                                }
                                //var rsecurityquestionid = connection.Query<dynamic>(SQLeducationcategory, parametersapllicantreg);
                                //     countt = rsecurityquestionid.Count();

                                    if (countt > 0)
                                    {


                                        var parametersapllicantreg1 = new { @cid = cid, @securityquestionid = securityquestionid1, @customerid = customerid1 };

                                        string SQLeducationcategory1 = "select count(*) as count from customersecurityquestionshistories where securityquestionid=@securityquestionid and  customerid=@customerid";
                                        var rsecurityquestionid1 = connection.Query<dynamic>(SQLeducationcategory1, parametersapllicantreg1);
                                        int countt1 = rsecurityquestionid1.Count();



                                        //if (Helper.Count("select count(*) as count from customersecurityquestionshistories where securityquestionid=@securityquestionid and  customerid=@customerid", parametersapllicantreg1) <= 0)
                                        if (countt1 == 0)
                                        {

                                            // var parsecurityquestionid = new { @customerid = obj_customersecurityquestion.customerid };
                                            string SQLques = "select * from customersecurityquestions where customerid='" + customerid1 + "'";
                                            NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                                            var readercurr = cmdcurr.ExecuteReader();
                                            System.Data.DataTable resultscurr = new System.Data.DataTable();
                                            resultscurr.Load(readercurr);
                                            for (int i = 0; i < resultscurr.Rows.Count; i++)
                                            {
                                                DataRow row1 = resultscurr.Rows[i];
                                                int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                                int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                                int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                                string answer = resultscurr.Rows[i]["answer"].ToString();






                                                /* if (obj_customersecurityquestionshistory.status == "" || obj_customersecurityquestionshistory.status == null) */
                                                obj_customersecurityquestionshistory.status = "A";
                                                //obj_customersecurityquestionshistory.companyid=cid;
                                                obj_customersecurityquestionshistory.historyid = null;
                                                obj_customersecurityquestionshistory.createdby = uid;
                                                obj_customersecurityquestionshistory.createddate = DateTime.Now;
                                                obj_customersecurityquestionshistory.oldanswer = answer;
                                                obj_customersecurityquestionshistory.customerid = customerid;
                                                obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                                _history_context.customersecurityquestionshistories.Add((dynamic)obj_customersecurityquestionshistory);
                                                querytype = 1;
                                                _logger.LogInfo("saving api customersecurityquestionshistories ");
                                                _history_context.SaveChanges();
                                            }
                                        }

                                        else
                                        {
                                            string SQLques = "select * from customersecurityquestions where customerid='" + customerid1 + "'";
                                            NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                                            var readercurr = cmdcurr.ExecuteReader();
                                            System.Data.DataTable resultscurr = new System.Data.DataTable();
                                            resultscurr.Load(readercurr);
                                            for (int i = 0; i < resultscurr.Rows.Count; i++)
                                            {
                                                DataRow row1 = resultscurr.Rows[i];
                                                int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                                int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                                int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                                string answer = resultscurr.Rows[i]["answer"].ToString();

                                                string SQLques1 = "select * from customersecurityquestionshistories where customerid='" + customerid1 + "'  and securityquestionid='" + securityquestionid1 + "'";
                                                NpgsqlCommand cmdcurr1 = new NpgsqlCommand(SQLques1, connection);
                                                var readercurr1 = cmdcurr1.ExecuteReader();
                                                System.Data.DataTable resultscurr1 = new System.Data.DataTable();
                                                resultscurr1.Load(readercurr1);
                                                for (int i1 = 0; i1 < resultscurr1.Rows.Count; i1++)
                                                {
                                                    DataRow row2 = resultscurr1.Rows[i];
                                                    historyid = Convert.ToInt32(resultscurr1.Rows[i]["historyid"]);
                                                    oldanswer = resultscurr1.Rows[i]["newanswer"].ToString();
                                                }







                                                //obj_customersecurityquestionshistory.companyid=cid;
                                                obj_customersecurityquestionshistory.status = "A";
                                                obj_customersecurityquestionshistory.updatedby = uid;
                                                //obj_customersecurityquestionshistory.historyid = historyid;
                                                obj_customersecurityquestionshistory.updateddate = DateTime.Now;
                                                obj_customersecurityquestionshistory.oldanswer = oldanswer;
                                                obj_customersecurityquestionshistory.newanswer = answer;
                                                obj_customersecurityquestionshistory.customerid = customerid;
                                                obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                                _history_context.Entry(obj_customersecurityquestionshistory).State = EntityState.Modified;
                                            //when IsModified = false, it will not update these fields.so old values will be retained
                                            _history_context.Entry(obj_customersecurityquestionshistory).Property("historyid").IsModified = false;
                                            _history_context.Entry(obj_customersecurityquestionshistory).Property("createdby").IsModified = false;
                                                _history_context.Entry(obj_customersecurityquestionshistory).Property("createddate").IsModified = false;

                                                querytype = 2;

                                                _logger.LogInfo("saving api customersecurityquestionshistories ");
                                                _history_context.SaveChanges();

                                            }
                                        }

                                    }
                                    else
                                    {
                                        string SQLques = "select * from customersecurityquestions where customerid='" + customerid1 + "' and questionid='" + questionid1 + "'";
                                        // var rsecurityquestionid = connection.Query<dynamic>(SQLeducationcategory, parsecurityquestionid);
                                        //int countt = rsecurityquestionid.Count();
                                        //System.Data.DataTable resultscurr = new System.Data.DataTable();
                                        NpgsqlCommand cmdcurr = new NpgsqlCommand(SQLques, connection);
                                        var readercurr = cmdcurr.ExecuteReader();
                                        System.Data.DataTable resultscurr = new System.Data.DataTable();
                                        resultscurr.Load(readercurr);
                                        for (int i = 0; i < resultscurr.Rows.Count; i++)
                                        {
                                            DataRow row1 = resultscurr.Rows[i];
                                            int customerid = Convert.ToInt32(resultscurr.Rows[i]["customerid"]);
                                            int securityquestionid = Convert.ToInt32(resultscurr.Rows[i]["securityquestionid"]);
                                            int questionid = Convert.ToInt32(resultscurr.Rows[i]["questionid"]);
                                            string answer = resultscurr.Rows[i]["answer"].ToString();






                                            /* if (obj_customersecurityquestionshistory.status == "" || obj_customersecurityquestionshistory.status == null) */
                                            obj_customersecurityquestionshistory.status = "A";
                                            //obj_customersecurityquestionshistory.companyid=cid;
                                            obj_customersecurityquestionshistory.historyid = null;
                                            obj_customersecurityquestionshistory.createdby = uid;
                                            obj_customersecurityquestionshistory.createddate = DateTime.Now;
                                        obj_customersecurityquestionshistory.questionid = questionid;
                                            obj_customersecurityquestionshistory.newanswer = answer;
                                            obj_customersecurityquestionshistory.customerid = customerid;
                                            obj_customersecurityquestionshistory.securityquestionid = securityquestionid;
                                            _history_context.customersecurityquestionshistories.Add((dynamic)obj_customersecurityquestionshistory);
                                            querytype = 1;
                                            _logger.LogInfo("saving api customersecurityquestionshistories ");
                                            _history_context.SaveChanges();

                                        }

                                    }
                                }
                            }
                        }
                    var res = "inserted ";
                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

              //  Helper.AfterExecute("", querytype, data, "customersecurityquestions", 0, data.securityquestionid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                //var res = Get_customersecurityquestion((int)data.securityquestionid);
                return (res);
            }
           }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) \r\n{ex}");
                throw ex;
            }
        }


        // DELETE: customersecurityquestion/5
        //delete process
        public  dynamic Delete(int id)
        {
        try{
        {
        _logger.LogInfo("Getting into Delete(int id) api");
customersecurityquestion obj_customersecurityquestion = _context.customersecurityquestions.Find(id);
_context.customersecurityquestions.Remove(obj_customersecurityquestion);
//using var transaction = connection.BeginTransaction();
//_context.Database.UseTransaction(transaction);
        _logger.LogInfo("remove api customersecurityquestions ");
            _context.SaveChanges();
//           transaction.Commit();

            return (true);
        }
        } catch (Exception ex) {
            _logger.LogError($"Service: Delete(int id) \r\n{ex}");
            throw ex;
        }
        }




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



        public dynamic Get_customersecurityquestiondetail(int customerid)
        {
            _logger.LogInfo("Getting into Get_customersecurityquestiondetail(int customerid) api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string wStatus = "NormalStatus";

                    var parameters = new { @cid = cid, @uid = uid, @customerid = customerid, @wStatus = wStatus };
                    var SQL = @"select c.customerid,c.securityquestionid,c.questionid,m.masterdatadescription as Question from customersecurityquestions c left join masterdatas m on c.questionid=m.masterdataid where m.masterdatatypeid=1 and m.status='A' and c.customerid=@customerid";

                   // var SQL = @"select * from customersecurityquestions where customerid=@customerid";
                    var result = connection.Query<dynamic>(SQL, parameters);
                   // var obj_customersecurityquestion = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'customersecurityquestions'";
                    var customersecurityquestion_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    connection.Close();
                    connection.Dispose();
                    return (new { customersecurityquestiondetail = result, customersecurityquestion_menuactions, formproperty, visiblelist, hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service: Get_customersecurityquestion(int id)\r\n {ex}");
                throw ex;
            }
        }





        public dynamic securityquestioncheck(string token, customersecurityquestion obj_customersecurityquestion)
        {
            _logger.LogInfo("Saving: securityquestioncheck(string token,customersecurityquestion obj_customersecurityquestion) ");
            try
            {
                string serr = "";
                int querytype = 0;
                int historyid = 0;
                string oldanswer = string.Empty;
                //int securityquestionid1 = 0;
                //int customerid1 = 0;
                //int questionid1 = 0;
                //string answer1 = string.Empty;
                //string status1 = string.Empty;
                dynamic result = "";

                int countt1 = 0;

                customersecurityquestionshistory obj_customersecurityquestionshistory = new customersecurityquestionshistory();



                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    connection.Open();








                    string SQLeducationcategory1 = "select count(q.*) as count from customersecurityquestions q left join customersecurityquestionshistories h on q.customerid = h.customerid and q.questionid = h.questionid and q.answer = h.newanswer where q.customerid = '"+ obj_customersecurityquestion .customerid+ "' and q.questionid ='"+ obj_customersecurityquestion .questionid+ "'and lower(q.answer)= lower('"+ obj_customersecurityquestion .answer+ "')";



                    NpgsqlCommand cmdcurs = new NpgsqlCommand(SQLeducationcategory1, connection);
                    var readercurs = cmdcurs.ExecuteReader();
                    System.Data.DataTable resultscurs = new System.Data.DataTable();
                    resultscurs.Load(readercurs);
                    for (int i = 0; i < resultscurs.Rows.Count; i++)
                    {
                        DataRow row1 = resultscurs.Rows[i];
                        countt1 = Convert.ToInt32(resultscurs.Rows[i]["count"]);
                    }


                    if (countt1 > 0)
                    {

                        // var parameters = new { @cid = cid };
                        //string SQL = "select pk_encode(a.registrationid) as pkcol,registrationid as value,'' as label from bouserregistrations a  WHERE a.companyid=@cid and  a.status='A'";
                        //var result = connection.Query<dynamic>(SQL, parameters);


                        result = "true";
                    }

                    else
                    {
                        result = "false";
                    }
                    return (result);
                }
            }
            //}
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) \r\n{ex}");
                throw ex;
            }
        }
        public dynamic securityquestionscheck(dynamic data)
        {
            _logger.LogInfo("Saving: securityquestioncheck(string token,customersecurityquestion obj_customersecurityquestion) ");
            try
            {
                string serr = "";
                int querytype = 0;
                int historyid = 0;
                string oldanswer = string.Empty;
                int securityquestionid1 = 0;
                int customerid1 = 0;
                int questionid1 = 0;
                string answer1 = string.Empty;
                string status1 = string.Empty;
                int countt = 0;
                int count = 0;
                dynamic result = "";

                customersecurityquestionshistory obj_customersecurityquestionshistory = new customersecurityquestionshistory();
                customersecurityquestion obj_customersecurityquestion = new customersecurityquestion();


                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();


                    //JObject obj_parent = JsonConvert.DeserializeObject<JObject>(data);
                    //JObject obj_parent1 = obj_parent.GetValue("securityquestions")[0] as JObject;



                    JObject obj_parents = JsonConvert.DeserializeObject<JObject>(data.ToString());

                    JObject obj_parent2 = obj_parents.GetValue("securityquestions")[0] as JObject;


                    foreach (KeyValuePair<string, JToken> item in obj_parent2)
                    {
                        JProperty p2 = obj_parent2.Property(item.Key);



                        if (item.Key == "questiondetails")
                        {
                            var Vendor_details = item.Value.ToString();

                            JArray array = JArray.Parse(Vendor_details);
                            JArray jsonArray = JArray.Parse(Vendor_details);

                            foreach (JObject content in array.Children<JObject>())
                            {
                                foreach (JProperty prop in content.Properties())
                                {
                                    string Name = prop.Name.ToString().Trim();
                                    string Value = prop.Value.ToString().Trim();
                                    if (Name == "securityquestionid")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            securityquestionid1 = 0;
                                        }
                                        else
                                        {
                                            securityquestionid1 = Convert.ToInt32(Value);
                                        }

                                    }

                                    if (Name == "customerid")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            customerid1 = 0;
                                        }
                                        else
                                        {
                                            customerid1 = Convert.ToInt32(Value);
                                        }

                                    }
                                    if (Name == "questionid")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            questionid1 = 0;
                                        }
                                        else
                                        {
                                            questionid1 = Convert.ToInt32(Value);
                                        }

                                    }


                                    if (Name == "answer")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            answer1 = null;
                                        }
                                        else
                                        {
                                            answer1 = Value.ToString();
                                        }

                                    }

                                    if (Name == "status")
                                    {
                                        if (Value.ToString() == "")
                                        {
                                            status1 = null;
                                        }
                                        else
                                        {
                                            status1 = Value.ToString();
                                        }

                                    }

                                }








                                string SQLeducationcategory1 = "select count(q.*) as count from customersecurityquestions q left join customersecurityquestionshistories h on q.customerid = h.customerid and q.questionid = h.questionid and q.answer = h.newanswer where q.customerid = '" + customerid1 + "' and q.questionid ='" + questionid1 + "' and h.securityquestionid='" + securityquestionid1 + "' and lower(q.answer)= lower('" + answer1 + "')";



                                NpgsqlCommand cmdcurs = new NpgsqlCommand(SQLeducationcategory1, connection);
                                var readercurs = cmdcurs.ExecuteReader();
                                System.Data.DataTable resultscurs = new System.Data.DataTable();
                                resultscurs.Load(readercurs);
                                for (int i = 0; i < resultscurs.Rows.Count; i++)
                                {
                                    DataRow row1 = resultscurs.Rows[i];
                                    countt = Convert.ToInt32(resultscurs.Rows[i]["count"]);
                                }


                                if (countt > 0)
                                {

                                    // var parameters = new { @cid = cid };
                                    //string SQL = "select pk_encode(a.registrationid) as pkcol,registrationid as value,'' as label from bouserregistrations a  WHERE a.companyid=@cid and  a.status='A'";
                                    //var result = connection.Query<dynamic>(SQL, parameters);


                                    count = count + 1;
                                }

                              
                            }
                        }
                    }
                   
                    if (count>=3)
                                {
                                 result = "true";
                                 }

                                 else
                                {
                                     result = "false";
                                   }
                    return (result);
                    
                }
               
            }
            //}
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion) \r\n{ex}");
                throw ex;
            }
        }








        private bool customersecurityquestion_Exists(int id)
        {
        try{
            return _context.customersecurityquestions.Count(e => e.securityquestionid == id) > 0;
        } catch (Exception ex) {
            _logger.LogError($"Service:customersecurityquestion_Exists(int id) {ex}");
            return false;
        }
        }
    }
}

