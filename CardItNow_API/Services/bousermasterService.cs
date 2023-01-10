
using carditnow.Models;
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SunSmartnTireProducts.Helpers;
using SunSmartnTireProducts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace carditnow.Services
{
    public class bousermasterService : IbousermasterService
    {
        private readonly bousermasterContext _context;
        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IbousermasterService _service;
        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public bousermasterService(ILoggerManager logger)
        {
            _logger = logger;
        }


        public bousermasterService(bousermasterContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {
            _context = context;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
            cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
        }

        // GET: service/bousermaster
        //Used in filling of dropdowns, search, next,prev. List checkbox in the column property need to be selected
        public dynamic Get_bousermasters()
        {
            _logger.LogInfo("Getting into get api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    var parameters = new { @cid = cid };
                    string SQL = "select pk_encode(a.userid) as pkcol,userid as value,username as label from bousermasters a  WHERE a.companyid=@cid and  a.status='A'";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    connection.Close();
                    connection.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
            return null;
        }


        public IEnumerable<Object> GetList(string key)
        {
            try
            {
                _logger.LogInfo("Getting into param/{key} GetList api");

                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters = new { @cid = cid, @key = key };
                    var SQL = "select pk_encode(a.userid) as pkcol,*,userid as value,username as label  from bousermasters a  where  a.companyid=@cid  and a.status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters);


                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetListBy_userid(int userid)
        {
            try
            {
                _logger.LogInfo("Getting into userid/{userid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_userid = new { @cid = cid, @userid = userid };
                    var SQL = "select pk_encode(userid) as pkcol,* from bousermasters where userid = @userid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_userid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        public IEnumerable<Object> GetListBy_emailid(string emailid)
        {
            try
            {
                 cid = 1;
                _logger.LogInfo("Getting into emailid/{emailid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_emailid = new { @cid = cid, @emailid = emailid };
                    var SQL = "select pk_encode(userid) as pkcol,* from bousermasters where emailid = @emailid and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_emailid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //dec152022
        public IEnumerable<Object> GetListBy_mobilenum(string mobilenum)
        {
            try
            {
                cid = 1;
                _logger.LogInfo("Getting into emailid/{emailid} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_emailid = new { @cid = cid, @mobilenum = mobilenum };
                    var SQL = "select pk_encode(userid) as pkcol,* from bousermasters where  mobilenumber like '%" + mobilenum + "' and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_emailid);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }



        public IEnumerable<Object> GetListBy_sourcereference(int sourcereference)
        {
            try
            {
                _logger.LogInfo("Getting into sourcereference/{sourcereference} api");
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    var parameters_sourcereference = new { @cid = cid, @sourcereference = sourcereference };
                    var SQL = "select pk_encode(userid) as pkcol,* from bousermasters where sourcereference = @sourcereference and  companyid=@cid  and status='A' ";
                    var result = connection.Query<dynamic>(SQL, parameters_sourcereference);

                    connection.Close();
                    connection.Dispose();
                    return (result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }
        //used in getting the record. parameter is encrypted id  
        public dynamic Get_bousermaster(string sid)
        {
            _logger.LogInfo("Getting into e/{sid} api");
            int id = Helper.GetId(sid);
            return Get_bousermaster(id);
        }
        // GET: bousermaster/5
        //gets the screen record
        public dynamic Get_bousermaster(int id)
        {
            _logger.LogInfo("Getting into {id} api");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {

                    //all visible & hiding of fields are to be controlled with these variables.Must visible, Must hide fields are used 
                    ArrayList visiblelist = new ArrayList();
                    ArrayList hidelist = new ArrayList();


                    string sdepartmentid = "qghhe";
                    var pdepartmentid = new { @sdepartmentid = sdepartmentid };
                    string SQLdepartmentid = "select datatypeid from bomasterdatatypes where code=@sdepartmentid";
                    var rdepartmentid = connection.Query<dynamic>(SQLdepartmentid, pdepartmentid);
                    int? departmentid = rdepartmentid.FirstOrDefault().datatypeid;
                    string sdefaultlanguage = "jc2s3";
                    var pdefaultlanguage = new { @sdefaultlanguage = sdefaultlanguage };
                    string SQLdefaultlanguage = "select datatypeid from bomasterdatatypes where code=@sdefaultlanguage";
                    var rdefaultlanguage = connection.Query<dynamic>(SQLdefaultlanguage, pdefaultlanguage);
                    int? defaultlanguage = rdefaultlanguage.FirstOrDefault().datatypeid;
                    string seducationid = "uugg5";
                    var peducationid = new { @seducationid = seducationid };
                    string SQLeducationid = "select datatypeid from bomasterdatatypes where code=@seducationid";
                    var reducationid = connection.Query<dynamic>(SQLeducationid, peducationid);
                    int? educationid = reducationid.FirstOrDefault().datatypeid;
                    string sdesignation = "qbteo";
                    var pdesignation = new { @sdesignation = sdesignation };
                    string SQLdesignation = "select datatypeid from bomasterdatatypes where code=@sdesignation";
                    var rdesignation = connection.Query<dynamic>(SQLdesignation, pdesignation);
                    int? designation = rdesignation.FirstOrDefault().datatypeid;
                    string wStatus = "NormalStatus";
                    string vapprovalleveltype1 = "approvalleveltype1";
                    string vapprovalleveltype2 = "approvalleveltype2";
                    string vapprovalleveltype4 = "approvalleveltype4";
                    string vapprovalleveltype5 = "approvalleveltype5";
                    string vbloodgroup = "bloodgroup";
                    string vgender = "gender";
                    string vmaritalstatus = "maritalstatus";
                    string vnationality = "nationality";
                    string vreligion = "religion";
                    string vapprovalleveltype3 = "approvalleveltype3";

                    var parameters = new { @cid = cid, @id = id, @wStatus = wStatus, @vapprovalleveltype1 = vapprovalleveltype1, @vapprovalleveltype2 = vapprovalleveltype2, @vapprovalleveltype4 = vapprovalleveltype4, @vapprovalleveltype5 = vapprovalleveltype5, @vbloodgroup = vbloodgroup, @vgender = vgender, @vmaritalstatus = vmaritalstatus, @vnationality = vnationality, @vreligion = vreligion, @vapprovalleveltype3 = vapprovalleveltype3, @departmentid = departmentid, @defaultlanguage = defaultlanguage, @educationid = educationid, @designation = designation };
                    var SQL = @"select pk_encode(a.userid) as pkcol,a.*,
d.masterdatadescription as departmentiddesc,
u3.username as approvallevel3desc,
u4.username as approvallevel4desc,
u5.username as approvallevel5desc,
ub.username as approvalleveldesc,
uc.username as approvallevel1desc,
ud.username as approvallevel2desc,
ua.username as reportingtodesc,
b.branchname as branchiddesc,
l.masterdatadescription as defaultlanguagedesc,
r.userrole as userroleiddesc,
c.name as cityiddesc,
ia.name as countryiddesc,
i.userrole as roledesc,
e.masterdatadescription as educationiddesc,
s.name as stateiddesc,
sa.masterdatadescription as designationdesc,
                              pl.configtext as approvalleveltype1desc,
                              re.configtext as approvalleveltype2desc,
                              ue.configtext as approvalleveltype4desc,
                              ea.configtext as approvalleveltype5desc,
                              eg.configtext as bloodgroupdesc,
                              es.configtext as genderdesc,
                              au.configtext as maritalstatusdesc,
                              ms.configtext as nationalitydesc,
                              rl.configtext as religiondesc,
                              zto.configtext as approvalleveltype3desc
 from bousermasters a 
 left join bomasterdatas d on  d.companyid=@cid  and a.departmentid=d.masterdataid and @departmentid=d.masterdatatypeid
 left join bousermasters u3 on  u3.companyid=@cid  and a.approvallevel3=u3.userid
 left join bousermasters u4 on  u4.companyid=@cid  and a.approvallevel4=u4.userid
 left join bousermasters u5 on  u5.companyid=@cid  and a.approvallevel5=u5.userid
 left join bousermasters ub on  ub.companyid=@cid  and a.approvallevel=ub.userid
 left join bousermasters uc on  uc.companyid=@cid  and a.approvallevel1=uc.userid
 left join bousermasters ud on  ud.companyid=@cid  and a.approvallevel2=ud.userid
 left join bousermasters ua on  ua.companyid=@cid  and a.reportingto=ua.userid
 left join bobranchmasters b on  b.companyid=@cid  and a.branchid=b.branchid
 left join bomasterdatas l on  l.companyid=@cid  and a.defaultlanguage=l.masterdataid and @defaultlanguage=l.masterdatatypeid
 left join bouserrolemasters r on  r.companyid=@cid  and a.userroleid=r.userroleid
 left join bocities c on a.cityid=c.cityid
 left join bocountries ia on a.countryid=ia.countryid
 left join bouserrolemasters i on  i.companyid=@cid  and a.role=i.userroleid
 left join bomasterdatas e on  e.companyid=@cid  and a.educationid=e.masterdataid and @educationid=e.masterdatatypeid
 left join bostates s on a.stateid=s.stateid
 left join bomasterdatas sa on  sa.companyid=@cid  and a.designation=sa.masterdataid and @designation=sa.masterdatatypeid
 left join boconfigvalues pl on a.approvalleveltype1=pl.configkey and @vapprovalleveltype1=pl.param
 left join boconfigvalues re on a.approvalleveltype2=re.configkey and @vapprovalleveltype2=re.param
 left join boconfigvalues ue on a.approvalleveltype4=ue.configkey and @vapprovalleveltype4=ue.param
 left join boconfigvalues ea on a.approvalleveltype5=ea.configkey and @vapprovalleveltype5=ea.param
 left join boconfigvalues eg on a.bloodgroup=eg.configkey and @vbloodgroup=eg.param
 left join boconfigvalues es on a.gender=es.configkey and @vgender=es.param
 left join boconfigvalues au on a.maritalstatus=au.configkey and @vmaritalstatus=au.param
 left join boconfigvalues ms on a.nationality=ms.configkey and @vnationality=ms.param
 left join boconfigvalues rl on a.religion=rl.configkey and @vreligion=rl.param
 left join boconfigvalues zto on a.approvalleveltype3=zto.configkey and @vapprovalleveltype3=zto.param
 where  a.companyid=@cid  and a.userid=@id";
                    var result = connection.Query<dynamic>(SQL, parameters);
                    var obj_bousermaster = result.FirstOrDefault();
                    var SQLmenuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title, a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bousermasters'";
                    var bousermaster_menuactions = connection.Query<dynamic>(SQLmenuactions, parameters);
                    FormProperty formproperty = new FormProperty();
                    formproperty.edit = true;


                    //Child table values
                    ArrayList bousermenuaccesses_visiblelist = new ArrayList();
                    ArrayList bousermenuaccesses_hidelist = new ArrayList();


                    var SQLbousermenuaccesses = @" select   pk_encode(o.menuid) as pkcol, a.usermenuaccessid as usermenuaccessid, o.menuid, o.menudescription, o.menuurl, m.menudescription as parentid
 from bomenumasters o  left outer join bousermenuaccesses a on o.menuid=a.menuid and  a.userid= @id  and  a.companyid=@cid  left join bomenumasters m on  o.parentid=m.menuid ";
                    var parameters_bousermenuaccesses = new { @cid = cid, @id = id };
                    var r_bousermenuaccesses = connection.Query<dynamic>(SQLbousermenuaccesses, parameters_bousermenuaccesses);
                    var SQL_bousermenuaccess_menuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title,a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bousermenuaccesses'";
                    var bousermenuaccess_menuactions = connection.Query<dynamic>(SQL_bousermenuaccess_menuactions, parameters);
                    //Child table values
                    ArrayList bouserbranchaccesses_visiblelist = new ArrayList();
                    ArrayList bouserbranchaccesses_hidelist = new ArrayList();


                    var SQLbouserbranchaccesses = @" select   pk_encode(o.branchid) as pkcol, a.accessid as accessid, o.branchid, o.branchcode, o.branchname
 from bobranchmasters o  left outer join bouserbranchaccesses a on o.branchid=a.branchid and  a.userid= @id  and  a.companyid=@cid 
 WHERE  o.companyid=@cid ";
                    var parameters_bouserbranchaccesses = new { @cid = cid, @id = id };
                    var r_bouserbranchaccesses = connection.Query<dynamic>(SQLbouserbranchaccesses, parameters_bouserbranchaccesses);
                    var SQL_bouserbranchaccess_menuactions = @"select actionid as name,'html' as type,'<i style=""width: 10px"" class=""' || actionicon || '""></i>' as title,a.* from bomenumasters m, bomenuactions a where m.menuid = a.menuid and m.actionkey = 'bouserbranchaccesses'";
                    var bouserbranchaccess_menuactions = connection.Query<dynamic>(SQL_bouserbranchaccess_menuactions, parameters);
                    connection.Close();
                    connection.Dispose();
                    return (new { bousermaster = obj_bousermaster, bousermaster_menuactions, bousermenuaccesses = r_bousermenuaccesses, bousermenuaccess_menuactions, bouserbranchaccesses = r_bouserbranchaccesses, bouserbranchaccess_menuactions, formproperty, visiblelist, hidelist, bousermenuaccesses_visiblelist, bousermenuaccesses_hidelist, bouserbranchaccesses_visiblelist, bouserbranchaccesses_hidelist });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        //saving of record
        public dynamic Save_bousermaster(string token, bousermastercontext obj_bousermaster)
        {
            _logger.LogInfo("Saving");
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    string serr = "";
                    int querytype = 0;
                    if (obj_bousermaster.emailid != null)
                    {
                        var parametersemailid = new { @cid = cid, @emailid = obj_bousermaster.emailid, @userid = obj_bousermaster.userid };
                        if (Helper.Count("select count(*) from bousermasters where  companyid=@cid  and emailid =  @emailid and (@userid = 0 or  @userid = null or  @userid < 0 or userid!=  @userid)", parametersemailid) > 0) serr += "emailid is unique\r\n";
                    }
                    if (obj_bousermaster.mobilenumber != null)
                    {
                        var parametersmobilenumber = new { @cid = cid, @mobilenumber = obj_bousermaster.mobilenumber, @userid = obj_bousermaster.userid };
                        if (Helper.Count("select count(*) from bousermasters where  companyid=@cid  and mobilenumber =  @mobilenumber and (@userid = 0 or  @userid = null or  @userid < 0 or userid!=  @userid)", parametersmobilenumber) > 0) serr += "mobilenumber is unique\r\n";
                    }
                    if (obj_bousermaster.usercode != null)
                    {
                        var parametersusercode = new { @cid = cid, @usercode = obj_bousermaster.usercode, @userid = obj_bousermaster.userid };
                        if (Helper.Count("select count(*) from bousermasters where  companyid=@cid  and usercode =  @usercode and (@userid = 0 or  @userid = null or  @userid < 0 or userid!=  @userid)", parametersusercode) > 0) serr += "usercode is unique\r\n";
                    }

                    if (serr != "")
                    {
                        _logger.LogError($"Something went wrong: {serr}");
                        throw new Exception(serr);
                    }

                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    //bousermaster table
                    if (obj_bousermaster.userid == 0 || obj_bousermaster.userid == null || obj_bousermaster.userid < 0)
                    {
                        if (obj_bousermaster.status == "" || obj_bousermaster.status == null) obj_bousermaster.status = "A";
                        obj_bousermaster.companyid = cid;
                        obj_bousermaster.sourcefield = "bousermasters";
                        obj_bousermaster.sourcereference = obj_bousermaster.userid;
                        obj_bousermaster.createdby = uid;
                        obj_bousermaster.createddate = DateTime.Now;
                        _context.bousermasters.Add((dynamic)obj_bousermaster);
                        querytype = 1;
                    }
                    else
                    {
                        obj_bousermaster.companyid = cid;
                        obj_bousermaster.sourcefield = "bousermasters";
                        obj_bousermaster.sourcereference = obj_bousermaster.userid;
                        obj_bousermaster.updatedby = uid;
                        obj_bousermaster.updateddate = DateTime.Now;
                        _context.Entry(obj_bousermaster).State = EntityState.Modified;
                        //when IsModified = false, it will not update these fields.so old values will be retained
                        _context.Entry(obj_bousermaster).Property("createdby").IsModified = false;
                        _context.Entry(obj_bousermaster).Property("createddate").IsModified = false;
                        querytype = 2;
                    }
                    _logger.LogInfo("saving api bousermasters ");
                    _context.SaveChanges();
                    var options_bousermenuaccess = new DbContextOptionsBuilder<bousermenuaccessContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_bousermenuaccess = new bousermenuaccessContext(options_bousermenuaccess);
                    //_context_bousermenuaccess.Database.UseTransaction(transaction);
                    //bousermenuaccesses table
                    if (obj_bousermaster.bousermenuaccesses != null)
                    {
                        foreach (var item in obj_bousermaster.bousermenuaccesses)
                        {
                            if (item != null)
                            {
                                if (item.usermenuaccessid == 0 || item.usermenuaccessid == null || item.usermenuaccessid < 0)
                                {
                                    item.userid = obj_bousermaster.userid;
                                    if (item.status == "" || item.status == null) item.status = "A";
                                    item.companyid = cid;
                                    item.createdby = uid;
                                    item.createddate = DateTime.Now;
                                    _context_bousermenuaccess.bousermenuaccesses.Add(item);
                                }
                                else
                                {
                                    item.userid = obj_bousermaster.userid;
                                    item.companyid = cid;
                                    item.updatedby = uid;
                                    item.updateddate = DateTime.Now;
                                    _context_bousermenuaccess.Entry(item).State = EntityState.Modified;
                                    //transaction.Commit();
                                }
                            }
                        }
                    }

                    //Delete for bousermenuaccesses
                    if (obj_bousermaster.Deleted_bousermenuaccess_IDs != null)
                    {
                        foreach (var id in obj_bousermaster.Deleted_bousermenuaccess_IDs.Split(',').Where(x => x != ""))
                        {
                            bousermenuaccess obj = _context_bousermenuaccess.bousermenuaccesses.Find(Convert.ToInt32(id));
                            if (obj != null) _context_bousermenuaccess.bousermenuaccesses.Remove(obj);
                        }
                        _context_bousermenuaccess.SaveChanges();
                    }
                    var options_bouserbranchaccess = new DbContextOptionsBuilder<bouserbranchaccessContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_bouserbranchaccess = new bouserbranchaccessContext(options_bouserbranchaccess);
                    //_context_bouserbranchaccess.Database.UseTransaction(transaction);
                    //bouserbranchaccesses table
                    if (obj_bousermaster.bouserbranchaccesses != null)
                    {
                        foreach (var item in obj_bousermaster.bouserbranchaccesses)
                        {
                            if (item != null)
                            {
                                if (item.accessid == 0 || item.accessid == null || item.accessid < 0)
                                {
                                    item.userid = obj_bousermaster.userid;
                                    if (item.status == "" || item.status == null) item.status = "A";
                                    item.companyid = cid;
                                    item.createdby = uid;
                                    item.createddate = DateTime.Now;
                                    _context_bouserbranchaccess.bouserbranchaccesses.Add(item);
                                }
                                else
                                {
                                    item.userid = obj_bousermaster.userid;
                                    item.companyid = cid;
                                    item.updatedby = uid;
                                    item.updateddate = DateTime.Now;
                                    _context_bouserbranchaccess.Entry(item).State = EntityState.Modified;
                                    //transaction.Commit();
                                }
                            }
                        }
                    }

                    //Delete for bouserbranchaccesses
                    if (obj_bousermaster.Deleted_bouserbranchaccess_IDs != null)
                    {
                        foreach (var id in obj_bousermaster.Deleted_bouserbranchaccess_IDs.Split(',').Where(x => x != ""))
                        {
                            bouserbranchaccess obj = _context_bouserbranchaccess.bouserbranchaccesses.Find(Convert.ToInt32(id));
                            if (obj != null) _context_bouserbranchaccess.bouserbranchaccesses.Remove(obj);
                        }
                        _context_bouserbranchaccess.SaveChanges();
                    }


                    //to generate serial key - select serialkey option for that column
                    //the procedure to call after insert/update/delete - configure in systemtables 

                    Helper.AfterExecute(token, querytype, obj_bousermaster, "bousermasters", obj_bousermaster.companyid, obj_bousermaster.userid, "", null, _logger);


                    //After saving, send the whole record to the front end. What saved will be shown in the screen
                    var res = Get_bousermaster((int)obj_bousermaster.userid);
                    return (res);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        // DELETE: bousermaster/5
        //delete process
        public dynamic Delete(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    _logger.LogInfo("Getting into delete api");
                    dynamic obj_bousermaster = Get_bousermaster(id);
                    connection.Open();
                    //using var transaction = connection.BeginTransaction();
                    //_context.Database.UseTransaction(transaction);
                    var options_bousermenuaccess = new DbContextOptionsBuilder<bousermenuaccessContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_bousermenuaccess = new bousermenuaccessContext(options_bousermenuaccess);
                    //_context_bousermenuaccess.Database.UseTransaction(transaction);
                    if (obj_bousermaster.bousermenuaccesses != null)
                    {
                        foreach (var item in obj_bousermaster.bousermenuaccesses)
                        {
                            _context_bousermenuaccess.bousermenuaccesses.Remove(_context_bousermenuaccess.bousermenuaccesses.Find(item.usermenuaccessid));
                        }
                        _context_bousermenuaccess.SaveChanges();
                    }

                    var options_bouserbranchaccess = new DbContextOptionsBuilder<bouserbranchaccessContext>()
                                    .UseNpgsql(Helper.Connectionstring)
                                    .Options;
                    using var _context_bouserbranchaccess = new bouserbranchaccessContext(options_bouserbranchaccess);
                    //_context_bouserbranchaccess.Database.UseTransaction(transaction);
                    if (obj_bousermaster.bouserbranchaccesses != null)
                    {
                        foreach (var item in obj_bousermaster.bouserbranchaccesses)
                        {
                            _context_bouserbranchaccess.bouserbranchaccesses.Remove(_context_bouserbranchaccess.bouserbranchaccesses.Find(item.accessid));
                        }
                        _context_bouserbranchaccess.SaveChanges();
                    }

                    _context.bousermasters.Remove(_context.bousermasters.Find(id));
                    _logger.LogInfo("remove api bousermasters ");
                    _context.SaveChanges();
                    //           transaction.Commit();

                    return (obj_bousermaster);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                throw ex;
            }
        }

        private bool bousermaster_Exists(int id)
        {
            try
            {
                return _context.bousermasters.Count(e => e.userid == id) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return false;
            }
        }

    }
}

