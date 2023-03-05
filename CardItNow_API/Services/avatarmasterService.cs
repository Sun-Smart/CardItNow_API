
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
using Microsoft.AspNetCore.Hosting;

namespace carditnow.Services
{
    public class avatarmasterService : IavatarmasterService
    {
        private readonly IConfiguration Configuration;
        private readonly avatarmasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IavatarmasterService _service;
        public static IWebHostEnvironment _environment;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";
        public avatarmasterService(avatarmasterContext context, IConfiguration configuration, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
            _context = context;
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

        // GET: service/avatarmaster
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_avatarmasters()
        {
            _logger.LogInfo("Getting into Get_avatarmasters() api");
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {

                    var parameters = new { @cid = cid, @uid = uid };
                    string SQL = "select pk_encode(a.avatarid) as pkcol,avatarid as value,avatarname as label,avatarurl as aurl from GetTable(NULL::public.avatarmasters,@cid) a  WHERE  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    //return result;
                    
                        var result1 = new
                        {
                            status = "success",
                            data = result,   /* Application-specific data would go here. */
                            message = "succesfully get record" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service : Get_avatarmasters(): {ex}");

            }
            return null;
        }
        public IEnumerable<Object> GetListBy_avatarid(int avatarid)
        {
            try
            {
                _logger.LogInfo("Getting into  GetListBy_avatarid(int avatarid) api");
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    var parameters_avatarid = new { @cid = cid, @uid = uid, @avatarid = avatarid };
                    var SQL = "select pk_encode(avatarid) as pkcol,avatarid as value,avatarname as label,* from GetTable(NULL::public.avatarmasters,@cid) where avatarid = @avatarid";
                    var result = connection.Query<dynamic>(SQL, parameters_avatarid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:  GetListBy_avatarid(int avatarid) \r\n {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_avatarmaster(string sid)
        {
            _logger.LogInfo("Getting into  Get_avatarmaster(string sid) api");
            int id = Helper.GetId(sid);
            return Get_avatarmaster(id);
        }
        // GET: avatarmaster/5
        //gets the screen record
        //       public dynamic Get_avatarmaster(int id)
        //       {
        //           _logger.LogInfo("Getting into Get_avatarmaster(int id) api");
        //           try
        //           {
        //               using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        //               {

        //                   //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
        //                   ArrayList visiblelist = new ArrayList();
        //                   ArrayList hidelist = new ArrayList();


        //                   string wStatus = "NormalStatus";

        //                   var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
        //                   var SQL = @"select pk_encode(a.avatarid) as pkcol,a.avatarid as pk,a.*
        //from GetTable(NULL::public.avatarmasters,@cid) a 
        //where a.avatarid=@id";
        //                   var result = connection.Query<dynamic>(SQL, parameters);
        //                   var obj_avatarmaster = result.FirstOrDefault();
        //                   var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'avatarmasters'";
        //                   var avatarmaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
        //                   FormProperty formproperty = new FormProperty();
        //                   formproperty.edit = true;


        //                   connection.Close();
        //                   connection.Dispose();
        //                   return (new { avatarmaster = obj_avatarmaster, avatarmaster_menuactions, formproperty, visiblelist, hidelist });
        //               }
        //           }
        //           catch (Exception ex)
        //           {
        //               _logger.LogError($"Service: Get_avatarmaster(int id)\r\n {ex}");
        //               throw ex;
        //           }
        //       }

        //       public IEnumerable<Object> GetList(string condition = "")
        //       {
        //           try
        //           {
        //               _logger.LogInfo("Getting into  GetList(string condition) api");

        //               using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        //               {
        //                   var parameters = new { @cid = cid, @uid = uid, @key = condition };
        //                   var SQL = @"select  pk_encode(a.avatarid) as pkcol,a.avatarid as pk,* ,avatarid as value,avatarname as label  from GetTable(NULL::public.avatarmasters,@cid) a ";
        //                   if (condition != "") SQL += " and " + condition;
        //                   SQL += " order by avatarname";
        //                   var result = connection.Query<dynamic>(SQL, parameters);


        //                   connection.Close();
        //                   connection.Dispose();
        //                   return (result);
        //               }
        //           }
        //           catch (Exception ex)
        //           {
        //               _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
        //               throw ex;
        //           }
        //       }
        //       public IEnumerable<Object> GetFullList()
        //       {
        //           try
        //           {
        //               _logger.LogInfo("Getting into  GetFullList() api");

        //               int id = 0;
        //               using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
        //               {
        //                   string wStatus = "NormalStatus";
        //                   var parameters = new { @cid = cid, @uid = uid, @id = id, @wStatus = wStatus };
        //                   var SQL = @"select pk_encode(a.avatarid) as pkcol,a.avatarid as pk,a.* from GetTable(NULL::public.avatarmasters,@cid) a ";
        //                   var result = connection.Query<dynamic>(SQL, parameters);


        //                   connection.Close();
        //                   connection.Dispose();
        //                   return (result);
        //               }
        //           }
        //           catch (Exception ex)
        //           {
        //               _logger.LogError($"Service: GetList(string key) api \r\n {ex}");
        //               throw ex;
        //           }
        //       }


        //       public IEnumerable<avatarmaster> getlist_Test(avatarmaster model)
        //       {
        //           var result = _context.avatarmasters.Where(x => x.avatarid == model.avatarid);

        //           result.Where(x => x.avatarname.Contains("dada"));

        //           return result;

        //       }
        //       public void getlist_Test(avatarmaster model)
        //       {
        //           var result=_context.avatarmasters.Where(x => x.avatarid == model.avatarid);

        //           result.Where(x => x.avatarname.Contains("dada"));
        //           result.ToList();
        //       }
        //       public void TestList(List<avatarmaster> models)
        //       {
        //           foreach (var model in models)
        //           {
        //               if (model.avatarid == 0)
        //               {
        //                   //create 
        //                   _context.avatarmasters.Add(model);
        //               }
        //               else
        //               {
        //                   var dbObject = _context.avatarmasters.Find(model.avatarid);
        //                   if (dbObject != null)
        //                   {
        //                       dbObject.avatarname = model.avatarname;
        //                       dbObject.avatarurl = model.avatarurl;
        //                   }
        //               }
        //               //delete 
        //               //var dbObjecttodelete = _context.avatarmasters.Find(model.avatarid);
        //               //_context.avatarmasters.Remove(dbObjecttodelete);
        //               //_context.SaveChanges();
        //           }
        //           _context.SaveChanges();

        //       }


        //saving of record
        [Authorize]
        public dynamic Save_avatarmaster(string token, avatarmaster obj_avatarmaster)
        {
            _logger.LogInfo("Saving: Save_avatarmaster(string token,avatarmaster obj_avatarmaster) ");
            try
            {
                string serr = "";
                int querytype = 0;
                if (obj_avatarmaster.avatarname != null)
                {
                    var parametersavatarname = new { @cid = cid, @uid = uid, @avatarname = obj_avatarmaster.avatarname, @avatarid = obj_avatarmaster.avatarid };
                    if (Helper.Count("select count(*) from avatarmasters where  avatarname =  @avatarname ", parametersavatarname) > 0) serr += "avatarname is unique\r\n";

                    //and (@avatarid == 0 ||  @avatarid == null ||  @avatarid < 0 || avatarid!=  @avatarid)
                }
                if (obj_avatarmaster.avatarurl != null)
                {
                    var parametersavatarurl = new { @cid = cid, @uid = uid, @avatarurl = obj_avatarmaster.avatarurl, @avatarid = obj_avatarmaster.avatarid };
                    if (Helper.Count("select count(*) from avatarmasters where   avatarurl =  @avatarurl ", parametersavatarurl) > 0) serr += "avatarurl is unique\r\n";

                    //and (@avatarid == 0 ||  @avatarid == null ||  @avatarid < 0 || avatarid!=  @avatarid)
                }
                if (serr != "")
                {
                    _logger.LogError($"Validation error-save: {serr}");
                    throw new Exception(serr);
                }

                //connection.Open();
                //using var transaction = connection.BeginTransaction();
                //_context.Database.UseTransaction(transaction);
                //avatarmaster table
                if (obj_avatarmaster.avatarid == 0 || obj_avatarmaster.avatarid == null || obj_avatarmaster.avatarid < 0)
                {
                    if (obj_avatarmaster.status == "" || obj_avatarmaster.status == null) obj_avatarmaster.status = "A";
                    //obj_avatarmaster.companyid=cid;
                    obj_avatarmaster.createdby = uid;
                    obj_avatarmaster.createddate = DateTime.Now;
                    _context.avatarmasters.Add((dynamic)obj_avatarmaster);
                    querytype = 1;
                }
                else
                {
                    //obj_avatarmaster.companyid=cid;
                    obj_avatarmaster.updatedby = uid;
                    obj_avatarmaster.updateddate = DateTime.Now;
                    _context.Entry(obj_avatarmaster).State = EntityState.Modified;
                    //when IsModified = false, it will not update these fields.so old values will be retained
                    _context.Entry(obj_avatarmaster).Property("createdby").IsModified = false;
                    _context.Entry(obj_avatarmaster).Property("createddate").IsModified = false;
                    querytype = 2;
                }
                _logger.LogInfo("saving api avatarmasters ");
                _context.SaveChanges();


                //to generate serial key - select serialkey option for that column
                //the procedure to call after insert/update/delete - configure in systemtables 

                Helper.AfterExecute(token, querytype, obj_avatarmaster, "avatarmasters", 0, obj_avatarmaster.avatarid, "", null, _logger);


                //After saving, send the whole record to the front end. What saved will be shown in the screen
                var res = Get_avatarmaster((int)obj_avatarmaster.avatarid);
                return (res);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Service: Save_avatarmaster(string token,avatarmaster obj_avatarmaster) \r\n{ex}");
                throw ex;
            }
        }

        // DELETE: avatarmaster/5
        //delete process
        [Authorize]
        public dynamic Delete(int id)
        {
            try
            {
                {
                    _logger.LogInfo("Getting into Delete(int id) api");
                    avatarmaster obj_avatarmaster = _context.avatarmasters.Find(id);
                    _context.avatarmasters.Remove(obj_avatarmaster);
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    _logger.LogInfo("remove api avatarmasters ");
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

        private bool avatarmaster_Exists(int id)
        {
            try
            {
                return _context.avatarmasters.Count(e => e.avatarid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Service:avatarmaster_Exists(int id) {ex}");
                return false;
            }
        }

        public IEnumerable<object> GetFullList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetList(string key)
        {
            throw new NotImplementedException();
        }

        public dynamic Get_avatarmaster(int id)
        {

            //shy start

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
                    var SQL = @"select * from avatarmasters where avatarid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_customermaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px""  class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'avatarmasters'";
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




            //end








            // throw new NotImplementedException();  ramesh
        }
        [AllowAnonymous]
        public async Task<string> UploadSelfi(avatarUploadRequestViewModel objfile)
        {

            try
            {
                //string sSharePath = @"D:\Production\Application\CardItNow\CarditNowImages";
                string sSharePath = @"E:\";
                string sHostPath = @"https://demo.herbie.ai:80/CarditNowImages";
                if (!Directory.Exists(sSharePath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(sSharePath + "\\uploads\\");
                }
                using (FileStream fileStream = File.Create(sSharePath + "\\uploads\\" + objfile.ImageFile.FileName))
                {

                    objfile.ImageFile.CopyTo(fileStream);
                    fileStream.Flush();
                    var file_path = fileStream.Name;
                    var img_name = objfile.ImageFile.FileName;
                    if (!string.IsNullOrEmpty(img_name))
                    {
                        sHostPath = sHostPath + "\\uploads\\" + objfile.ImageFile.FileName;
                        using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                        {
                            connection.Open();
                            NpgsqlCommand inst_cd = new NpgsqlCommand("insert into avatarmasters (orderid,avatarname,avatarurl,status,createdby,createddate,updatedby,updateddate) values(@orderid,@avatarname,@avatarurl,@status,@createdby,@createddate,@updatedby,@updateddate)", connection);
                            inst_cd.Parameters.AddWithValue("@orderid", 0);
                            inst_cd.Parameters.AddWithValue("@avatarname", img_name);
                            inst_cd.Parameters.AddWithValue("@avatarurl", sHostPath);
                            inst_cd.Parameters.AddWithValue("@status", 'A');
                            inst_cd.Parameters.AddWithValue("@createdby", cid);
                            inst_cd.Parameters.AddWithValue("@createddate", DateTime.Now);
                            inst_cd.Parameters.AddWithValue("@updatedby", cid);
                            inst_cd.Parameters.AddWithValue("@updateddate", DateTime.Now);
                            var output = inst_cd.ExecuteNonQuery();
                            if (output > 0)
                            {
                                //return "Success";
                                var result1 = new
                                {
                                    status = "success",
                                    data = "",   /* Application-specific data would go here. */
                                    message = "succesfully get record" /* Or optional success message */
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
                                    message = "succesfully get record" /* Or optional success message */
                                };
                                return JsonConvert.SerializeObject(result1);
                            }
                        }

                    }
                    return objfile.ImageFile.FileName;
                }
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public dynamic UploadSelfiJson(string SJsonVal)
        {

            try
            {
                string val = SJsonVal;
                avatarUploadRequestViewModelsMobile mavatarUploadRequestViewModelsMobile = JsonConvert.DeserializeObject<avatarUploadRequestViewModelsMobile>(val);
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand inst_cd = new NpgsqlCommand("update customermasters set customerphoto=@customerphoto where customerid=@customerid", connection);
                    inst_cd.Parameters.AddWithValue("@customerphoto", mavatarUploadRequestViewModelsMobile.ImageFile);
                    inst_cd.Parameters.AddWithValue("@customerid", Convert.ToInt32(mavatarUploadRequestViewModelsMobile.customerid));                    
                    //inst_cd.Parameters.AddWithValue("@updatedby", cid);
                    //inst_cd.Parameters.AddWithValue("@updateddate", DateTime.Now);
                    var output = inst_cd.ExecuteNonQuery();
                    if (output > 0)
                    {
                        //return  "Success";
                        var result1 = new
                        {
                            status = "success",
                            data = "",   /* Application-specific data would go here. */
                            message = "succesfully get record" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }
                    else
                    {
                        //return "fail";
                        var result1 = new
                        {
                            status = "success",
                            data = "",   /* Application-specific data would go here. */
                            message = "succesfully get record" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log error
                return null;
            }
            return null;
        }

        public dynamic GetImages()
        {
            var sList = new List<avatarUploadList>();
            //string sSharePath = @"D:\Production\Application\CardItNow\CarditNowImages";
            //string sHostPath = @"https://demo.herbie.ai:80/CarditNowImages";
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    connection.Open();
                    NpgsqlCommand inst_cd = new NpgsqlCommand("select orderid,avatarname,avatarurl from avatarmasters", connection);
                    //inst_cd.Parameters.AddWithValue("@orderid", 0);
                    //inst_cd.Parameters.AddWithValue("@avatarname", img_name);
                    NpgsqlDataAdapter sda = new NpgsqlDataAdapter(inst_cd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            sList.Add(new avatarUploadList
                            {
                                avatarid = Convert.ToInt32(dr["orderid"].ToString()),
                                avatarname = dr["avatarname"].ToString(),
                                avatarurl = dr["avatarurl"].ToString()

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
                            data = "",   /* Application-specific data would go here. */
                            message = "not succesfully get record" /* Or optional success message */
                        };
                        return JsonConvert.SerializeObject(result1);
                    }
                }

            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

    }
}

