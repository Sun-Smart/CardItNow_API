
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nTireBO;
using nTireBO.Models;
using SunSmartnTireProducts.Helpers;
////////using FluentDateTime;
////////using FluentDate;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;
using Npgsql;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections;
using System.Text;
using nTireBO.Services;
using Microsoft.Extensions.Configuration;
using LoggerService;
using System.Dynamic;

namespace nTireBO.Services
{
    public class ReportViewerService : IReportViewerService
    {
        private readonly ReportViewerContext _context;
        private readonly ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;
        private readonly IReportViewerService _service;

        int cid = 1;
        int uid = 4;
        string uname = "";
        string uidemail = "";
        int? sessionuserid = 4;
        int? sessionbranchid = 1;
        int? userroleid = 1;
        int? finyearid = 1;



        public ReportViewerService(ReportViewerContext context, ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor )
        {
            _context = context;
            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid")!=null) cid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid")!=null) uid=int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
        uname = "";
        uidemail = "";
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")!=null)uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
        if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email")!=null)uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email").Value.ToString();

            sessionuserid = uid;
        }

        public dynamic RunReport(ReportParam param)
        {
            /*
                      //  _logger.LogInfo(p);
                       // ReportParam param=new ReportParam();
                        _logger.LogInfo("s1");
                        //_logger.LogInfo(param.ToString());
                        _logger.LogInfo("s2");
                        _logger.LogInfo(ModelState.IsValid.ToString());
                        string sq=param.id.ToString();
                        _logger.LogInfo(sq);
                        _logger.LogInfo(param.SessionUser);
                        /*
                        foreach (PropertyInfo o in param.GetType().GetProperties())
                        {
                            if (o.GetValue(param) != null)
                                {
                            _logger.LogInfo(o.GetValue(param).ToString());
                                }
                        }
                        */
            string SQL = "";
            if (param.id == null) return null;
            bool bstatusfound = false;
            bool bpkfound = false;
            string reportcode = param.id;
            long id = 0;
            _logger.LogInfo("Final " + id.ToString());



            string sgroupby = "";
            _logger.LogInfo(id.ToString());
            _logger.LogInfo(sessionuserid.ToString());
            _logger.LogInfo(userroleid.ToString());
            _logger.LogInfo(finyearid.ToString());

            int? sessionempid = 0;
            string fkname = "";
            string fk = "";
            if (param.fkname != null && param.fkname != "") fkname = "m." + param.fkname;
            if (param.fk != null && param.fk != "") fk = param.fk;

            string fkname1 = "";
            string fk1 = "";
            if (param.fkname1 != null && param.fkname1 != "") fkname1 = "m." + param.fkname1;
            if (param.fk1 != null && param.fk1 != "") fk1 = param.fk1;

            string[] nocompanytables = { "recipientdiscounts", "customerpaymodes", "transactionmasters", "transactiondetails", "transactionitemdetails", "carditchargesdiscounts", "usermasters", "termsmasters", "initiatorrecipientprivates", "customerdetails", "customerrecipientlinks", "customertermsacceptances", "geoaccesses", "geographymasters", "userrolemasters", "citymasters", "menuaccesses", "menumasters", "masterdatas", "masterdatatypes", "customermasters", "customersecurityquestions", "customersecurityquestionshistories", "avatarmasters", "initiatorrecipientmappings", "systemtabletemplates", "autodetails", "automasters", "bocompanymasters", "systemtables", "bomenuactions", "boreportdetails", "boconfigvalues", "boconfigparametertypes", "boconfigparameters", "bomasterdatatypes", "bomenumasters", "botablemasterdetailmaps", "botables", "bocompanymasters", "botableconfigurations", "bostates", "bocountries", "bocities", "bolocations", "boreports", "bodashboards", "bodashboarddetails", "boreportcolumns", "systemcolumns", "systemcolumn", "boreportothertables" };
            //dapper System.Data.Common.DbConnection dbConn;
            dynamic dyboreport = new ExpandoObject();

            try
            {
                using (var dbConn = new NpgsqlConnection(Helper.Connectionstring))
                {


                    dynamic companysettingsresult = null;
                    //companysettingsresult = (from a in _context.bocompanysettings.Where(c => c.companyid == cid) select a).FirstOrDefault();

                    dynamic usermasterresult = null;
                    //if (sessionuserid != -97567) usermasterresult = (from a in _context.usermasters.Where(c => c.userid == sessionuserid) select a).FirstOrDefault();

                    //if (usermasterresult != null) sessionempid = usermasterresult.employeeid;

                    string ConfigParam = "ApprovalStatus";

                    _logger.LogInfo("step 0");

                    string wStatus = "NormalStatus";
                    string vrecordtype = "recordtype";
                    string vdatefiltertype = "datefiltertype";
                    string vjointype = "jointype";
                    string vreportoutputtype = "reportoutputtype";
                    string vreporttype = "reporttype";

                    var parameters = new { @cid = cid, @id = reportcode, @wStatus = wStatus, @vrecordtype = vrecordtype, @vdatefiltertype = vdatefiltertype, @vjointype = vjointype, @vreportoutputtype = vreportoutputtype, @vreporttype = vreporttype };
                    SQL = @"select pk_encode(a.reportid) as pkcol,a.*,
                              r.configtext as recordtypedesc,
                              d.configtext as datefiltertypedesc,
                              j.configtext as jointypedesc,
                              o.configtext as reportoutputtypedesc,
                              t.configtext as reporttypedesc
 from boreports a 
 left join boconfigvalues r on a.recordtype=r.configkey and @vrecordtype=r.param
 left join boconfigvalues d on a.datefiltertype=d.configkey and @vdatefiltertype=d.param
 left join boconfigvalues j on a.jointype=j.configkey and @vjointype=j.param
 left join boconfigvalues o on a.reportoutputtype=o.configkey and @vreportoutputtype=o.param
 left join boconfigvalues t on a.reporttype=t.configkey and @vreporttype=t.param
 where reportcode=@id";
                    IEnumerable<dynamic> result = dbConn.Query<dynamic>(SQL, parameters);

                    var objboreport = result.FirstOrDefault();
                    if (objboreport.pk == null || objboreport.pk == "") objboreport.pk = objboreport.maintablealias + "." + objboreport.maintableidentityfield;

                    id = objboreport.reportid;

                    //objboreport=objboreport;


                    if (objboreport.reportjsondata != null && objboreport.reportjsondata != "")
                    {
                        JArray jsonVal = JArray.Parse(objboreport.reportjsondata) as JArray;
                        dynamic datas = jsonVal;
                        dynamic reportcoljson;
                        var scoljson = "";
                        foreach (JProperty o in datas[0])
                        {
                            if (scoljson != "") scoljson += ",";

                            scoljson += "{\"reportid\":\"" + objboreport.reportid.ToString() + "\",\"tablealias\":\"m\",\"field\":\"" + o.Name + "\",\"header\":\"" + o.Name + "\",\"columnalias\":\"\",\"columnheader\":\"\",\"width\":\"\",\"datatype\":\"\",\"derived\":\"\",\"nofilter\":\"\",\"groupby\":\"\",\"sum\":\"\",\"count\":\"\",\"hide\":\"\",\"colhtml\":\"\",\"poptitle\":\"\",\"link\":\"\",\"linkurl\":\"\",\"service\":\"\",\"servicename\":\"\",\"sp\":\"\",\"spname\":\"\",\"alert\":\"\",\"maxchars\":\"\",\"caps\":\"\",\"bold\":\"\",\"italic\":\"\",\"strikethrough\":\"\",\"bgcolor\":\"\",\"forecolor\":\"\",\"status\":\"\",\"notsortable\":\"\",\"sequence\":\"\",\"sumcondition\":\"\",\"countcondition\":\"\",\"min\":\"\",\"max\":\"\",\"datatypeDesc\":\"\"}";
                        }
                        JArray jsoncol = JArray.Parse("[" + scoljson + "]") as JArray;
                        return (new { objboreport = objboreport, boreportcolumn = jsoncol, results = jsonVal });
                    }

                    _logger.LogInfo("step 1 {id} "+ id);

                    var SQLboreportcolumn = @" select pk_encode(a.reportcolumnid) as pkcol, a.*,d.configtext  as datatypedesc
 from boreportcolumns a  left join boconfigvalues d on a.datatype = d.configkey and d.param='datatype' 
 WHERE   a.reportid = @id 
 ORDER BY sequence";
                    var parametersboreportcolumn = new { @cid = cid, @id = id };
                    IEnumerable<dynamic> objboreportcolumn = dbConn.Query<dynamic>(SQLboreportcolumn, parametersboreportcolumn);


                    //.Where(a=> a.derived!=true)
                    _logger.LogInfo("step 2");
                    var SQLboreportothertable = @" select distinct  pk_encode(a.othertableid) as pkcol,a.*,j.configtext  as jointypedesc,coalesce(sequence,0) as sequence
 from boreportothertables a  left join boconfigvalues j on a.jointype = j.configkey and j.param='jointype'
 WHERE   a.reportid = @id order by coalesce(sequence,0)";
                    var parametersboreportothertable = new { @cid = cid, @id = id };
                    var boreportothertable = dbConn.Query<dynamic>(SQLboreportothertable, parametersboreportothertable);


                    var SQLboreportdetail = @" select distinct  pk_encode(a.reportdetailid) as pkcol,a.*,j.configtext  as separatordesc
 from boreportdetails a  left join boconfigvalues j on a.separator = j.configkey and j.param='separator'
 WHERE   a.reportid = @id ";
                    var parametersboreportdetail = new { @cid = cid, @id = id };
                    var objboreportdetail = dbConn.Query<dynamic>(SQLboreportdetail, parametersboreportdetail);

                    _logger.LogInfo("step 3");

                    var parameterworkflows = new { @tablename = objboreport.maintablename };
                    var SQLworkflow = "select count(*) as cnt from boworkflowmasters m, boworkflowsteps s where m.workflowmasterid=s.workflowmasterid and menuid= @tablename and pause!=true";
                    var objworkflow = dbConn.Query<dynamic>(SQLworkflow, parameterworkflows);
                    var rowWorkflow = objworkflow.FirstOrDefault();
                    bool isWorkFlow = rowWorkflow.cnt > 0 ? true : false;

                    var SQLsystemtable = "select tablename, statusfield from systemtables where tablename =@tablename";
                    var objsystemtable = dbConn.Query<dynamic>(SQLsystemtable, parameterworkflows);
                    var rowsystemtable = objsystemtable.FirstOrDefault();
                    string statusfield = "status";
                    string pkfield = objboreport.pk;
                    if (rowsystemtable!=null) statusfield = rowsystemtable.statusfield;
                    /*
                    HttpClient client = new HttpClient();

                    string strURL = "http://localhost:7002/ntireboapi/BOReport/c/" +cid+"/"+id;


                    var response = (client.GetAsync(strURL)).Result;
                    var responseOutput = response.Content.ReadAsStringAsync();
                    var Data = responseOutput.Result;
                    dynamic jArr = JsonConvert.DeserializeObject(Data);



                    BOReport  objboreport =null;
                    BOReportOtherTable[] boreportothertable = null;
                    BOReportColumn[] boreportcolumn = null;

                    int i = 0;
                    foreach(var obj in jArr)
                    {
                        if (i == 0) objboreport = (dynamic)obj;
                        if (i == 1) boreportcolumn = (dynamic)obj;
                        if (i == 2) boreportothertable = (dynamic)obj;

                        i++;
                    }
                    */

                    string kanbancols = "";
                    string kanbantbl = "";
                    string kanbanwhere = "";
                    string kanbantype = "";

                    ArrayList tblconfigslist = new ArrayList();

                    var parameterscolconfigs = new { @tablename = objboreport.maintablename };

                    IEnumerable<dynamic> colconfigs = dbConn.Query<dynamic>("select s.* from systemcolumns s , boreports r , boreportcolumns c where tablename = @tablename and fk = true and r.maintablename=tablename and r.reportid=c.reportid and c.field=s.columnname and coalesce(c.derived,false)=false", parameterscolconfigs);

                    //var colconfigs = from a in _context.systemcolumns.Where (c => c.tablename == objboreport.maintablename && c.fk == true).Distinct () select a;

                    tblconfigslist.Add(new TableInfo { Alias = objboreport.maintablealias, colconfigs = colconfigs });
                    ArrayList fkdatas = new ArrayList();

                    _logger.LogInfo("step 5");
                    //dapper dbConn = _context.Database.GetDbConnection ();
                    _logger.LogInfo("step 6");

                    SQL = "";//string
                    string cols = "";
                    bool bnofk = false;
                    string Tables = "";
                    //string status=".status='A'";
                    bool statuscheck = true;
                    if (param.status == "all") statuscheck = false;
                    string Where = "";
                    _logger.LogInfo("step 7");
                    if (objboreport.reporttype == "Q") SQL = "select * from (" + objboreport.query + ") m";

                    _logger.LogInfo(SQL);

                    if (objboreport.reporttype != "Q")
                    {
                        if(isWorkFlow)
                            Tables = objboreport.maintablename + " " ;
                        else
                        Tables = objboreport.maintablename + " " + objboreport.maintablealias;
                        if (!((IList)nocompanytables).Contains(objboreport.maintablename) && sessionuserid != -97567)
                        {
                            if (Where != "") Where += " and ";
                            //Where += (objboreport.maintablealias == "" ? "m" : objboreport.maintablealias) + ".companyid=" + cid;
                        }
                        if (objboreport.wherecondition != null && objboreport.wherecondition != "")
                        {
                            if (Where != "") Where += " and ";
                            Where += objboreport.wherecondition;
                        }
                        if (param.status != null && param.status != "")
                        {
                            if (Where != "" && statuscheck) Where += " and ";
                            Where += (statuscheck ? ((objboreport.maintablealias == "" ? "m" : objboreport.maintablealias) + ".status in(" + param.status + ")") : "");
                        }

                        if (param.pkvalue != null && param.pkvalue != 0)
                        {
                            if (Where != "") Where += " and ";
                            Where += objboreport.pk + "=" + param.pkvalue;
                        }
                    }

                    if (objboreport.reporttype == "MD")
                    {
                        string strJoinType = "inner join";
                        if (objboreport.jointype == "I") strJoinType = "  join ";
                        else if (objboreport.jointype == "L") strJoinType = " left  join ";
                        else if (objboreport.jointype == "R") strJoinType = " right  join ";

                        if (objboreport.detailtablequery != null && objboreport.detailtablequery != "")
                            Tables += " " + strJoinType + " (" + objboreport.detailtablequery + ") " + objboreport.detailtablealias;
                        else
                            Tables += " " + strJoinType + " " + objboreport.detailtablename + " " + objboreport.detailtablealias;

                        //if (fkwhere != "") Where += " and";
                        bool btablecondition = false;

                        if (!((IList)nocompanytables).Contains(objboreport.detailtablename) && sessionuserid != -97567)
                        {
                            if (!btablecondition) Tables += " on ";
                            //Tables += objboreport.detailtablealias + ".companyid=" + cid;
                            btablecondition = true;
                        }
                        if (objboreport.detailtablefk != null && objboreport.detailtablefk != "")
                        {
                            if (!btablecondition)
                                Tables += " on ";
                            else
                                Tables += " and ";

                            btablecondition = true;
                            Tables += objboreport.detailtablealias + "." + objboreport.detailtableidentityfield + "=" + objboreport.maintablealias + "." + objboreport.detailtablefk;
                        }

                        if (objboreport.masterdetailwhere != null && objboreport.masterdetailwhere != "")
                        {
                            if (!btablecondition)
                                Tables += " on ";
                            else
                                Tables += " and ";
                            btablecondition = true;
                            Tables += objboreport.masterdetailwhere;
                        }
                        if (!btablecondition)
                            Tables += " on 1=1";

                        Tables += (statuscheck ? (" and " + objboreport.detailtablealias + ".status='A'") : ""); //
                        parameterscolconfigs = new { @tablename = objboreport.detailtablename };
                        colconfigs = dbConn.Query<dynamic>("select * from systemcolumns where tablename = @tablename and fk = true", parameterscolconfigs);

                        //colconfigs = from a in _context.systemcolumns.Where (c => c.tablename == objboreport.detailtablename && c.fk == true).Distinct () select a;
                        tblconfigslist.Add(new TableInfo { Alias = objboreport.detailtablealias, colconfigs = colconfigs });

                    }
                    _logger.LogInfo("Step 4");
                    if (boreportothertable != null)
                    {
                        foreach (var othertbl in boreportothertable)
                        {
                            string strJoinType = "inner join";
                            if (othertbl.jointype == "I") strJoinType = " join ";
                            else if (othertbl.jointype == "L") strJoinType = " left  join ";
                            else if (othertbl.jointype == "R") strJoinType = " right  join ";

                            Tables += " " + strJoinType + " " + othertbl.tablename + " " + othertbl.tablealias;

                            //colconfigs = from a in _context.systemcolumns.Where (c => c.tablename == othertbl.tablename && c.fk == true).Distinct () select a;

                            parameterscolconfigs = new { @tablename = othertbl.tablename };
                            colconfigs = dbConn.Query<dynamic>("select * from systemcolumns where tablename = @tablename and fk = true", parameterscolconfigs);
                            tblconfigslist.Add(new TableInfo { Alias = othertbl.tablealias, colconfigs = colconfigs });

                            Tables += " on ";
                            if (!((IList)nocompanytables).Contains(othertbl.tablename) && sessionuserid != -97567)
                            {
                                //Tables += othertbl.tablealias + ".companyid=" + cid + " and ";
                            }
                            if (othertbl.wherecondition != "")
                            {
                                Tables += othertbl.wherecondition;
                            }
                            else
                            {
                                Tables += " 1=1 ";
                            }
                            Tables += (statuscheck ? (" and " + othertbl.tablealias + ".status='A'") : "");
                        }
                    }

                    _logger.LogInfo("Step 5");

                    if (objboreport.reporttype != "Q")
                    {
                        foreach (var row in objboreportcolumn)
                        {
                            if(row.field==statusfield)
                            {
                                bstatusfound = true;
                            }
                            if (row.field == pkfield)
                            {
                                bpkfound = true;
                            }
                            if (row.datatype == "SS" && row.field == objboreport.kanbankey)
                            {
                                kanbantype = row.datatype;
                            }
                            else if (row.datatype == "SS")
                            {
                                if (cols != "") cols += ",";
                                cols += "getuseraccess(" + row.tablealias + "." + row.field + ") as " + row.field + "desc";
                            }
                            else if (row.datatype == "M")
                            {
                                if (cols != "") cols += ",";
                                cols += "getcomments(" + row.tablealias + "." + row.field + ") as " + row.field + "desc";
                            }
                            if (row.derived != true)
                            {
                                bnofk = true;

                                if (cols != "") cols += ",";
                                if (row.groupby == true)
                                {
                                    if (sgroupby != "") sgroupby += ",";
                                    if (row.tablealias != "") sgroupby += row.tablealias + ".";
                                    sgroupby += row.field;
                                }
                                foreach (TableInfo Tblconfig in tblconfigslist)
                                {
                                    var alias = Tblconfig.Alias;
                                    var colconfigs1 = Tblconfig.colconfigs;
                                    if (alias == row.tablealias)
                                    {
                                        string fkwhere = "";
                                        foreach (var colconfig in colconfigs1)
                                        {

                                            if (colconfig.columnname == row.field && colconfig.fk == true)
                                            {

                                                if (row.fkfilter == true) fkdatas.Add(colconfig);


                                                bnofk = false;
                                                string scol = "";

                                                if (colconfig.fktablename == "bocountries")
                                                    scol = "'<img src=\"http://localhost:5002/MyResources/flag_placeholder.png\" width=\"30\" class=\"flag flag-' ||  t" + colconfig.syscolumnid + ".code || '\">'   " + " ||  t" + colconfig.syscolumnid + "." + colconfig.fkdescription;
                                                else if (colconfig.fktablename != "boconfigvalues" && colconfig.fktablename != "bomasterdatas" && colconfig.fktablename != "usermasters" && colconfig.fktablename != "hrmsemployees" && colconfig.fktablename != "bouserrolemasters" && colconfig.fktablename != "lmsmasters" && colconfig.fktablename != "camsassetmasters" && colconfig.fktablename != "erpproducts" && colconfig.fktablename != "erpitemmasters" && colconfig.fktablename != "bobranchmasters" && colconfig.fktablename != "prjprojectmasters" && colconfig.fktablename != "crmcustomermasters" && colconfig.fktablename != "legalcustomermasters" && colconfig.fktablename != "erpsuppliermasters")
                                                    scol = " t" + colconfig.syscolumnid + "." + colconfig.fkdescription;

                                                //else if(colconfig.fktablename=="usermasters" ||  colconfig.fktablename=="hrmsemployees" ||  colconfig.fktablename=="bouserrolemasters" ||  colconfig.fktablename=="lmsmasters" ||  colconfig.fktablename=="camsassetmasters" ||  colconfig.fktablename=="erpproducts" ||  colconfig.fktablename=="erpitemmasters" ||  colconfig.fktablename=="bobranchmasters" ||  colconfig.fktablename=="prjprojectmasters" ||  colconfig.fktablename=="crmcustomermasters" ||  colconfig.fktablename=="legalcustomermasters" ||  colconfig.fktablename=="erpsuppliermasters" )
                                                //scol = " COALESCE ('<img class=\"icon\" src=\"http://localhost:5002/MyResources/' || COALESCE (t" + colconfig.syscolumnid + ".thumbnail,'"+colconfig.fktablename+"thumbnail.png') || '\">'|| t" + colconfig.syscolumnid + "."+colconfig.fkdescription +" ,t" + colconfig.syscolumnid + "."+colconfig.fkdescription+")";     
                                                else if (colconfig.fktablename == "boconfigvalues")
                                                    scol = " case when t" + colconfig.syscolumnid + ".htmlcode is not null and t" + colconfig.syscolumnid + ".htmlcode!='' then    " + "t" + colconfig.syscolumnid + ".htmlcode  else t" + colconfig.syscolumnid + ".configtext end ";
                                                //scol = " case when t" + colconfig.syscolumnid + ".htmlcode is not null and t" + colconfig.syscolumnid + ".htmlcode!='' then   '<span style=\"' || "+"t" + colconfig.syscolumnid + ".htmlcode || '\">' || "+"t" + colconfig.syscolumnid + ".configtext || '</span>'  else t" + colconfig.syscolumnid + ".configtext end ";
                                                else if (colconfig.fktablename == "bomasterdatas")
                                                    scol = " case when t" + colconfig.syscolumnid + ".htmlcode is not null and t" + colconfig.syscolumnid + ".htmlcode!='' then    " + "t" + colconfig.syscolumnid + ".htmlcode  else t" + colconfig.syscolumnid + ".masterdatadescription end ";
                                                //scol = " case when t" + colconfig.syscolumnid + ".htmlcode is not null and t" + colconfig.syscolumnid + ".htmlcode!='' then   '<span style=\"' || "+"t" + colconfig.syscolumnid + ".htmlcode || '\">' || "+"t" + colconfig.syscolumnid + ".masterdatadescription || '</span>'  else t" + colconfig.syscolumnid + ".masterdatadescription end ";    
                                                //scol = " COALESCE (t" + colconfig.syscolumnid + ".htmlcode,t" + colconfig.syscolumnid + "."+colconfig.fkdescription+")";

                                                else
                                                    scol = " t" + colconfig.syscolumnid + "." + colconfig.fkdescription;

                                                if (row.maxchars != null && row.maxchars != 0) scol = "left(" + scol + "," + row.maxchars + ") ";
                                                cols += scol + " as " + (row.columnalias ?? row.field) + "desc,";
                                                if (colconfig.fktablename == "boconfigvalues")
                                                    cols += " t" + colconfig.syscolumnid + ".orderno as " + (row.columnalias ?? row.field) + "order,";

                                                Tables += " left  join " + colconfig.fktablename + " t" + colconfig.syscolumnid;
                                                if (fkwhere != "") fkwhere += " and";
                                                fkwhere += " " + "cast(" + row.tablealias + "." + row.field + " as varchar)=cast(" + "t" + colconfig.syscolumnid + "." + colconfig.fkidentityid + " as varchar)";

                                                if (colconfig.fkwhere != null && colconfig.fkwhere != "")
                                                {
                                                    if (fkwhere != "") fkwhere += " and ";
                                                    fkwhere += colconfig.fkwhere.Replace("{{a}}", "t" + colconfig.syscolumnid);
                                                }
                                                Tables += " on" + fkwhere;
                                                if (objboreport.kanbankey != null && objboreport.kanbankey != "" && objboreport.kanbankey == colconfig.columnname)
                                                {
                                                    //kanbantype = row.datatype;
                                                    kanbancols = colconfig.fkidentityid + " as configkey," + colconfig.fkdescription + " as configtext";
                                                    kanbantbl = colconfig.fktablename + " t";
                                                    string sfkwhere = "";
                                                    if (colconfig.fkwhere != null && colconfig.fkwhere != "") kanbanwhere = colconfig.fkwhere.Replace("{{a}}", "t");
                                                }



                                                break;
                                            }
                                        }
                                    }
                                }
                                /*if (objboreport.kanbankey != null && objboreport.kanbankey != "" && objboreport.kanbankey == colconfig.columnname)
                                {
                                    kanbancols = colconfig.fkidentityid + " as configkey," + colconfig.fkdescription + " as configtext";
                                    kanbantbl = colconfig.fktablename + " t";
                                    string sfkwhere = "";
                                    if (colconfig.fkwhere != null && colconfig.fkwhere != "") kanbanwhere = colconfig.fkwhere.Replace("{{a}}", "t");
                                }*/
                                if (true || bnofk)
                                {
                                    //row.tablealias == "" ? "m" : row.tablealias
                                    string scol = Format((row.tablealias == "" ? "" : row.tablealias + ".") + row.field, row);
                                    if (row.maxchars != null && row.maxchars != 0) scol = "left(" + scol + "," + row.maxchars + ") ";
                                    /*
                                    if (row.datatype == "IM")
                                        cols += "'<img class=\"icon\" src=\"http://localhost:5002/MyResources/' || " + row.tablealias + ".thumbnail || '\">'  as " + (row.columnalias??row.field);
                                    else
                                            */
                                    cols += scol + " as " + (row.columnalias ?? row.field);

                                }

                            }

                        }

                        _logger.LogInfo("Step 6");

                        if (objboreport.datefilter == true)
                        {
                            string strDateCondition = "";

                            if (objboreport.datefiltertype == "T")
                            {
                                strDateCondition = objboreport.datefiltercolumnname + " >= CAST(CURRENT_TIMESTAMP AS DATE) and " + objboreport.datefiltercolumnname + "<DATEADD(DD, 1, CAST(CURRENT_TIMESTAMP AS DATE))";
                            }
                            else if (objboreport.datefiltertype == "Y")
                            {
                                strDateCondition = objboreport.datefiltercolumnname + ">=DATEADD(DD, -1, CAST(CURRENT_TIMESTAMP AS DATE))  and " + objboreport.datefiltercolumnname + "<CAST(CURRENT_TIMESTAMP AS DATE)";
                            }
                            else if (objboreport.datefiltertype == "W")
                            {
                                strDateCondition = objboreport.datefiltercolumnname + " >= dateadd(day, 1-datepart(dw, getdate()), CONVERT(date,getdate()))  and " + objboreport.datefiltercolumnname + "<=  dateadd(day, 8-datepart(dw, getdate()), CONVERT(date,getdate()))";
                            }
                            else if (objboreport.datefiltertype == "LW")
                            {
                                strDateCondition = objboreport.datefiltercolumnname + " >= DATEADD(dd, -1, DATEADD(ww, DATEDIFF(ww, 0, getdate()) - 1, 0))  and " + objboreport.datefiltercolumnname + "<=  DATEADD(dd,  5, DATEADD(ww, DATEDIFF(ww, 0, getdate()) - 1, 0))";
                            }
                            else if (objboreport.datefiltertype == "L2")
                            {
                                strDateCondition = objboreport.datefiltercolumnname + " >= DATEADD(dd, -1, DATEADD(ww, DATEDIFF(ww, 0, getdate()) - 1, 0))  and " + objboreport.datefiltercolumnname + "<=  DATEADD(dd,  5, DATEADD(ww, DATEDIFF(ww, 0, getdate()) - 1, 0))";
                            }
                            else if (objboreport.datefiltertype == "M")
                            {
                                strDateCondition = "YEAR(" + objboreport.datefiltercolumnname + ") = YEAR(getdate()) AND MONTH(" + objboreport.datefiltercolumnname + ") = MONTH(getdate())";
                            }
                            else if (objboreport.datefiltertype == "LM")
                            {
                                strDateCondition = "DATEPART(m, " + objboreport.datefiltercolumnname + ") = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, " + objboreport.datefiltercolumnname + ") = DATEPART(yyyy, DATEADD(m, -1, getdate()))";
                            }
                            else if (objboreport.datefiltertype == "Y" || objboreport.datefiltertype == "FY")
                            {
                                strDateCondition = "YEAR(" + objboreport.datefiltercolumnname + ") = YEAR(getdate())";
                            }
                            else if (objboreport.datefiltertype == "LY" || objboreport.datefiltertype == "LFY")
                            {
                                strDateCondition = "YEAR(" + objboreport.datefiltercolumnname + ") = YEAR(getdate())-1";
                            }
                            else if (objboreport.datefiltertype == "L2")
                            {
                                strDateCondition = objboreport.datefiltercolumnname + ">=DATEADD(day,-14, GETDATE())";
                            }

                            if (Where != "")
                            {
                                Where = strDateCondition + " and " + Where;
                            }
                            else
                            {
                                Where = strDateCondition;
                            }
                        }

                        if (objboreportcolumn.Count() == 0) cols = "*";
                        if (objboreport.pk == null || objboreport.pk == "") objboreport.pk = objboreport.maintablealias + "." + objboreport.maintableidentityfield;
                        if (objboreport.pk.IndexOf(".") < 0) objboreport.pk = objboreport.maintablealias + "." + objboreport.pk;
                        if (objboreport.pk != null && objboreport.pk != "") cols = objboreport.pk + " as pk," + cols;

                        for (int r = 0; r < objboreportdetail.Count(); r++)
                        {
                            string separator = ",";
                            if (objboreportcolumn.ToList()[r].separator == "C")
                                separator = ",";
                            else if (objboreportcolumn.ToList()[r].separator == "BR")
                                separator = "</tr><tr>";
                            else if (objboreportdetail.ToList()[r].separator == "N")
                                separator = "\r\n";
                            //var colconfigs = from a in _context.systemcolumns.Where(c => c.tablename == tblName && c.fk == true).Distinct() select a;
                            string SQL_colconfigs = "select distinct tablename,columnname,fk,fktablename,fkidentityid,fkdescription,fkwhere,pk from systemcolumns where tablename=@tablename and fk=true";
                            var parameters_colconfigs = new { @tablename = objboreportdetail.ToList()[r].tablename };
                            var col_configs = dbConn.Query<dynamic>(SQL_colconfigs, parameters_colconfigs);
                            string[] tbl = getAllTables(objboreportdetail.ToList()[r].tablename, col_configs, objboreportdetail.ToList()[r].alias, objboreportdetail.ToList()[r].formula);
                            string formula = tbl[0];
                            string tblName = objboreportdetail.ToList()[r].tablename + " " + objboreportdetail.ToList()[r].alias;
                            tblName += tbl[1];
                            string strWhere = objboreportdetail.ToList()[r].wherecondition;
                            //if(tbl[2]!="")strWhere+=" and "+tbl[2];
                            cols += ",(select string_agg(" + formula + ", '" + separator + "') from " + tblName + " where " + strWhere + ") as " + objboreportdetail.ToList()[r].tablename;
                        }
                        _logger.LogInfo("Step {userroleid} " + userroleid);
                        if (userroleid != 6)
                        {
                            string sWhere = "";
                            dynamic param_parameters = param.parameters;
                            if (param_parameters != null && param_parameters.recordtype != "") objboreport.recordtype = param_parameters.recordtype;
                            if (objboreport.userfiltertype != null && objboreport.userfiltertype != "")
                            {
                                if (objboreport.recordtype == "M")
                                {
                                    string[] userfiltertypes = objboreport.userfiltertype.Split(',');
                                    for (int s = 0; s < userfiltertypes.Length; s++)
                                    {
                                        Tables += " cross join lateral jsonb_array_elements((" + userfiltertypes[s] + "->>'role')::jsonb) rid(raid" + s + ") ";
                                        Tables += " cross join lateral jsonb_array_elements((" + userfiltertypes[s] + "->>'user')::jsonb) uid(uaid" + s + ") ";
                                        if (sWhere != "") sWhere += " and ";
                                        sWhere += " ((uaid" + s + "::integer={{SESSIONUSERID}}) or (raid" + s + "::integer={{SESSIONROLEID}}))";
                                    }
                                }
                                //if (objboreport.recordtype == "M") sWhere += "((" + objboreport.userfiltertype + "='U' and " + objboreport.userfield + "={{SESSIONUSERID}}" + ") or (" + objboreport.userfiltertype + "='R' and " + objboreport.rolefield + "={{SESSIONROLEID}}" + "))";
                                if (objboreport.recordtype == "MS") sWhere += "(" + objboreport.userfield + "={{SESSIONUSERID}} or " + objboreport.userfield + " in ((select userid from usermasters where reportingto={{SESSIONUSERID}})))";
                                if (objboreport.recordtype == "S") sWhere += objboreport.userfield + " in (select userid from usermasters where reportingto={{SESSIONUSERID}})";
                            }
                            else if (objboreport.userfield != null && objboreport.userfield != "" && objboreport.recordtype != null && objboreport.recordtype != "" && objboreport.recordtype != "A" && usermasterresult.userroleid != companysettingsresult.adminroleid)
                            {

                                if (objboreport.recordtype == "M") sWhere += objboreport.userfield + "={{SESSIONUSERID}}";
                                if (objboreport.recordtype == "MS") sWhere += "(" + objboreport.userfield + "={{SESSIONUSERID}} or " + objboreport.userfield + " in ((select userid from usermasters where reportingto={{SESSIONUSERID}})))";
                                if (objboreport.recordtype == "S") sWhere += objboreport.userfield + " in (select userid from usermasters where reportingto={{SESSIONUSERID}})";
                            }
                            else if (objboreport.employeefield != null && objboreport.employeefield != "" && objboreport.recordtype != null && objboreport.recordtype != "" && objboreport.recordtype != "A" && usermasterresult.userroleid != companysettingsresult.adminroleid)
                            {

                                if (objboreport.recordtype == "M") sWhere += objboreport.employeefield + "={{SESSIONEMPID}}";
                                if (objboreport.recordtype == "MS") sWhere += "(" + objboreport.employeefield + "={{SESSIONEMPID}} or " + objboreport.employeefield + " in ((select employeeid from usermasters where reportingto={{SESSIONUSERID}})))";
                                if (objboreport.recordtype == "S") sWhere += objboreport.employeefield + " in (select employeeid from usermasters where reportingto={{SESSIONUSERID}})";
                            }
                            if (Where != "" && sWhere != "") Where += " and ";
                            Where += sWhere;
                        }

                        string pkcol = "";
                        if (objboreport.pk != "")
                        {
                            pkcol = objboreport.pk;
                        }
                        else
                        {
                            pkcol = objboreport.maintableidentityfield;
                        }

                        string strpk = "";
                        if (pkcol.IndexOf(".") < 0) pkcol = objboreport.maintablealias + "." + pkcol;
                        if (pkcol != null && pkcol != "") strpk = "pk_encode(" + pkcol + ") as pkcol,";


                        if (kanbantype == "SS")
                        {
                            cols += ",atm.userid as " + objboreport.kanbankey + ",atm.username as " + objboreport.kanbankey + "desc";
                            Tables += " left outer join jsonb_array_elements(" + objboreport.kanbankey + "::jsonb->'user') agt on 1=1 left  join usermasters atm on cast(agt::text as varchar)=cast(atm.userid as varchar) ";
                            kanbancols = "userid::varchar as configkey,username as configtext";
                            kanbantbl = "usermasters";
                        }

                        


                        if (isWorkFlow && !bstatusfound) cols += ","+ objboreport.maintablealias+"."+statusfield ;
                        if(!bpkfound) cols += "," + objboreport.maintablealias + "." + objboreport.maintableidentityfield + " as "+ objboreport.maintableidentityfield + " ";

                        if (objboreportcolumn.Count() != 0 && !bpkfound)
                        {
                            boreportcolumn obj =new boreportcolumn();
                            obj.field = pkfield;
                            obj.tablealias = objboreport.maintablealias;
                            obj.hide = true;
                            obj.header = pkfield;
                            objboreportcolumn=objboreportcolumn.Concat(new[] { obj});
                        }

                        SQL = "select  " + strpk + cols + " from " + Tables;

                        if (Where != "") SQL += " where " + Where;

                    }
                    if (fk != "" && fkname != "" && fkname.ToLower() != "m.pkcol")
                    {
                        if (SQL.ToLower().IndexOf(" where ") < 0)
                        {
                            SQL += " Where " + fkname + "='" + fk + "'";

                        }
                        else
                        {
                            SQL += " and " + fkname + "='" + fk + "'";
                        }
                    }
                    if (fk1 != "" && fkname1 != "" && fkname1.ToLower() != "m.pkcol")
                    {
                        if (SQL.ToLower().IndexOf(" where ") < 0)
                        {
                            SQL += " Where " + fkname1 + "='" + fk1 + "'";

                        }
                        else
                        {
                            SQL += " and " + fkname1 + "='" + fk1 + "'";
                        }
                    }


                    if (sgroupby != "")
                    {
                        SQL += " group by " + sgroupby;
                    }
                    if (objboreport.reporttype != "Q" && objboreport.sortby1 != null && objboreport.sortby1 != "")
                    {
                        SQL += " order by " + objboreport.sortby1;
                    }

                    if (objboreport.reporttype != "Q" && objboreport.sortby2 != null && objboreport.sortby2 != "")
                    {
                        SQL += "," + objboreport.sortby2;
                    }

                    if (objboreport.reporttype != "Q" && objboreport.sortby3 != null && objboreport.sortby3 != "")
                    {
                        SQL += "," + objboreport.sortby3;
                    }
                    
                    if (param.parameters != null && param.parameters.count > 0) // && //ss
                    {
                        //string p=param.parameters.ToString().Replace("\r\n","");
                        //JToken  data = JToken.Parse(p);
                        //  data = JToken.Parse(p);

                        foreach (JProperty x in param.parameters) //
                        {
                            string name = x.Name;
                            string value = "";
                            if (x.Value.HasValues) value = ((dynamic)(x.Value)).Value.ToString(); //.HasValues

                            SQL = SQL.Replace("{{" + name + "}}", value);
                        }

                        /* 
                        foreach (JProperty x in data) {
                            string name = x.Name;
                            JToken value = x.Value;
                        }                    
                        */
                    }
                    if (param.addparams != null && param.addparams.count > 0)
                    {
                        int pcnt = 0;
                        foreach (JProperty x in param.addparams) //JProperty
                        {
                            string name = x.Name;
                            string value = "";

                            value = ((dynamic)(x.Value)).ToString();
                            if (SQL.ToLower().IndexOf(" where ") < 0)
                            {
                                SQL += " Where ";
                            }
                            else if (pcnt == 0)
                                SQL += " and ";
                            else if (pcnt > 0)
                                SQL += " and ";
                            SQL = SQL + name + "=" + value;
                            pcnt++;
                        }
                    }

                    
                    


                    
                    bool bWhere = false;
                    if (isWorkFlow)
                    {
                        //SQL +=
                        string SQL1 = " left join boworkflows w on pkvalue=pk1 left join jsonb_array_elements((currentapprovers->>'user')::jsonb) uaid on 1=1 left join jsonb_array_elements((currentapprovers->>'role')::jsonb) raid on 1=1 where (uaid.value::integer={{SESSIONUSERID}} or raid.value::integer={{SESSIONROLEID}} or coalesce(m1." + statusfield + ",'')=''  or m1." + statusfield+"='A')";
                        SQL = SQL.Replace(objboreport.maintablename, "(select m1.* from (select " + objboreport.maintableidentityfield + " as pk1,* from " + objboreport.maintablename + ") m1 " + SQL1 + ") m ");

                    }
                    //SQL="( and status='A' and )"
                    if (objboreport.postquery != null)
                    {
                        if (bWhere)
                            SQL = SQL + " where ";
                        else
                            SQL = SQL + " and ";

                        SQL = SQL + objboreport.postquery;
                    }
                    //dapper dbConn.Open ();   
                    SQL = SQL.Replace("{{param.fk}}", param.fk);
                    SQL = SQL.Replace("{{param.modulepkcol}}", param.modulepkcol);
                    //dapper cmd.CommandText = SQL;



                    SQL = SQL.Replace("\n", " ");
                    _logger.LogInfo("SQL "+ SQL);

                    SQL = SQL.Replace("{{SESSIONCOMPANYID}}", cid.ToString());
                    SQL = SQL.Replace("{{SESSIONUSERID}}", sessionuserid.ToString());
                    SQL = SQL.Replace("{{SESSIONBRANCHID}}", sessionbranchid.ToString());
                    SQL = SQL.Replace("{{SESSIONROLEID}}", userroleid.ToString());
                    SQL = SQL.Replace("{{SESSIONEMPID}}", sessionempid.ToString());
                    SQL = SQL.Replace("{{SESSIONFINYEAR}}", finyearid.ToString());

                    SQL = "select * from (" + SQL + ") m2 ";

                    objboreport.SQL = SQL;


                    

                    IEnumerable<dynamic> dataresults = dbConn.Query<dynamic>(SQL, parameters);
                    if (objboreportcolumn.Count() == 0 && dataresults.Count() == 0)
                    {

                        string SQL_ColDef = "select column_name as field,column_name as header from information_schema.columns  where table_name=@tablename and is_identity='NO' and column_name not in('companyid','createdby','createddate','updatedby','updateddate') order by ordinal_position";
                        var parameters_ColDef = new { @tablename = objboreport.maintablename };
                        objboreportcolumn = dbConn.Query<dynamic>(SQL_ColDef, parameters_ColDef);
                    }
                    //using (var cmd = dbConn.CreateCommand())
                    //{
                    var first = dataresults.FirstOrDefault();
                    /*
                    var reader = cmd.ExecuteReader ();

                    //if (reader != null)
                    //{
                    //Load the reader into a datatable
                    System.Data.DataTable dataresults = new System.Data.DataTable ();

                    dataresults.Load (reader);
                    //results=dataresults;
                    */
                    dynamic results = new ExpandoObject();

                    if (!((objboreport.header1 != null && objboreport.header1 != "") || (objboreport.footer1 != null && objboreport.footer1 != "") || (objboreport.headerquery1 != null && objboreport.headerquery1 != "")))
                    {
                        results.Rows = dataresults;// (JArray) JsonConvert.DeserializeObject (JsonConvert.SerializeObject (dataresults)); //dataresults.Rows;
                    }
                    if (first != null) results.Columns = first.Columns;
                    if ((objboreport.header1 != null && objboreport.header1 != "") || (objboreport.footer1 != null && objboreport.footer1 != "") || (objboreport.headerquery1 != null && objboreport.headerquery1 != ""))
                    {
                        results.DynamicRows = new ExpandoObject();
                        ArrayList arrRows = new ArrayList();
                        //concentrate - looping thru rows
                        for (int resultrow = 0; resultrow < dataresults.Count(); resultrow++)
                        {
                            ReportSection section = new ReportSection();

                            if (objboreport.header1 != null && objboreport.header1 != "")
                            {
                                section.header1 = objboreport.header1;
                                section.header1 = section.header1.Replace("{{param.fk}}", param.fk);
                            }

                            if (objboreport.footer1 != null && objboreport.footer1 != "")
                            {
                                section.footer1 = objboreport.footer1;
                                section.footer1 = section.footer1.Replace("{{param.fk}}", param.fk);
                            }

                            string header1SQL = "";
                            if (objboreport.headerquery1 != null && objboreport.headerquery1 != "") header1SQL = objboreport.headerquery1;

                            JObject json = JObject.FromObject(first);


                            foreach (JProperty p in json.Properties()) //for (int i = 0; i < first.Columns.Count; i++)
                            {//to do
                                /* if (objboreport.headerquery1 != null && objboreport.headerquery1 != "") header1SQL = header1SQL.Replace("{{main." + first.Columns[i].ToString() + "}}", dataresults.ToList()[resultrow][i].ToString());
                                 if (objboreport.header1 != null && objboreport.header1 != "") section.header1 = section.header1.Replace("{{main." + first.Columns[i].ToString() + "}}", dataresults.ToList()[resultrow][i].ToString());
                                 if (objboreport.footer1 != null && objboreport.footer1 != "") section.footer1 = section.footer1.Replace("{{main." + first.Columns[i].ToString() + "}}", dataresults.ToList()[resultrow][i].ToString());*/
                            }
                            if (objboreport.headerquery1 != null && objboreport.headerquery1 != "")
                            {
                                header1SQL = header1SQL.Replace("{{param.fk}}", param.fk);

                                header1SQL = header1SQL.Replace("\n", " ");

                                objboreport.headerquery1 = header1SQL;

                                //dapper cmd.CommandText = header1SQL;
                                section.header1results = dbConn.Query<dynamic>(SQL, parameters);
                                /*
                                using (var headerreader = cmd.ExecuteReader ()) { //
                                    System.Data.DataTable header1results = new System.Data.DataTable ();
                                    header1results.Load (headerreader);
                                    section.header1results = (JArray) JsonConvert.DeserializeObject (JsonConvert.SerializeObject (header1results)); //header1results;
                                    //objboreport.header1results"]= JsonConvert.SerializeObject(header1results);
                                }
                                */
                            }
                            section.Row = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dataresults.ToList()[resultrow])); //dapper dataresults.Rows[resultrow]
                            arrRows.Add(section);
                        }
                        results.Rows = arrRows;
                    }


                    dynamic statusdata = null;
                    if (objboreport.headerquery != null && objboreport.headerquery != "")
                    {
                        string headerSQL = objboreport.headerquery;
                        headerSQL = headerSQL.Replace("{{param.fk}}", param.fk);
                        headerSQL = headerSQL.Replace("\n", " ");

                        objboreport.headerSQL = headerSQL;

                        //cmd.CommandText = headerSQL;
                        var headerreader = dbConn.Query<dynamic>(headerSQL, parameters);

                        foreach (var row in headerreader)
                        {
                            JObject json = JObject.FromObject(row);
                            foreach (JProperty p in json.Properties())
                                objboreport.header = objboreport.header.Replace("##" + p.Name + "##", p.Value.ToString());

                        }

                        for (int i = 0; i < headerreader.Count(); i++)
                        {
                            //todo

                            //objboreport.header = objboreport.header.Replace("##" + headerreader.GetName(i).ToString() + "##", headerreader.GetValue(i).ToString());

                        }


                    }
                    /*
                    if(objboreport.headerquery1!=null && objboreport.headerquery1!="")
                    {
                        string header1SQL=objboreport.headerquery1;
                        header1SQL = header1SQL.Replace("{{param.fk}}", param.fk);
                        header1SQL = header1SQL.Replace("\n", " ");
                        cmd.CommandText = header1SQL;
                        using (var headerreader = cmd.ExecuteReader())
                        {
                            System.Data.DataTable header1results = new System.Data.DataTable();
                            header1results.Load(headerreader);
                            objboreport.header1results=header1results;
                            //objboreport.header1results"]= JsonConvert.SerializeObject(header1results);
                        }

                    }
                    */
                    if (objboreport.footerquery != null && objboreport.footerquery != "")
                    {
                        string footerSQL = objboreport.footerquery;
                        footerSQL = footerSQL.Replace("{{param.fk}}", param.fk);
                        footerSQL = footerSQL.Replace("\n", " ");

                        objboreport.footerSQL = footerSQL;
                        //cmd.CommandText = footerSQL;

                        var footerreader = dbConn.Query<dynamic>(footerSQL, parameters);


                        foreach (var row in footerreader)
                        {
                            JObject json = JObject.FromObject(row);
                            foreach (JProperty p in json.Properties())
                                objboreport.footer = objboreport.footer.Replace("##" + p.Name + "##", p.Value.ToString());

                        }

                        foreach (var row in footerreader)
                        {

                            //objboreport.footer = objboreport.footer.Replace("##" + row.GetName(i).ToString() + "##", row.GetValue(i).ToString());
                        }



                    }
                    if (objboreport.kanbankey != null && objboreport.kanbankey != "")
                    {
                        /*statusdata=(from a in _context.boconfigvalues
                                            where a.param == objboreport.kanbankey
                                            select a).ToList();
                                            */
                        SQL = "select " + kanbancols + " from " + kanbantbl;
                        if (kanbanwhere != "") SQL = SQL + " where " + kanbanwhere;

                        SQL = SQL.Replace("{{param.key}}", param.key);
                        objboreport.kanbanSQL = SQL;

                        statusdata = dbConn.Query<dynamic>(SQL, parameters);
                        /*

                        cmd.CommandText = SQL;
                    NpgsqlDataReader reader = cmd.ExecuteReader ();
                    statusdata = new System.Data.DataTable ();
                    statusdata.Load (reader);
                        */
                    }

                    ArrayList fkresults = new ArrayList();
                    for (int f = 0; f < fkdatas.Count; f++)
                    {
                        systemcolumn colconfig = (systemcolumn)fkdatas[f];
                        /*statusdata=(from a in _context.boconfigvalues
                                            where a.param == objboreport.kanbankey
                                            select a).ToList();
                                            */
                        SQL = "select " + colconfig.fkidentityid + " as key," + colconfig.fkdescription + " as title from " + colconfig.tablename + " t ";
                        if (colconfig.fkwhere != "") SQL += " where " + colconfig.fkwhere.Replace("{{a}}", "t");


                        //cmd.CommandText = SQL;

                        var fkresultdata = dbConn.Query<dynamic>(SQL, parameters);// cmd.ExecuteReader();
                                                                                  //System.Data.DataTable fkresultdata = new System.Data.DataTable();
                                                                                  //fkresultdata.Load(reader);
                        fkresults.Add(fkresultdata);
                    }
                    SQL = "select * from bomenumasters where actionkey='" + objboreport.actionkey + "'";
                    IEnumerable<dynamic> bomenumasters = dbConn.Query<dynamic>(SQL, parameters);
                    dynamic bomenumaster = null;
                    if (bomenumasters.Count() > 0) bomenumaster = bomenumasters.FirstOrDefault();
                    IEnumerable<dynamic> bomenuactions = null;
                    if (bomenumasters.Count() > 0)
                    {
                        SQL = "select * from bomenuactions where menuid='" + bomenumaster.menuid + "'";
                        bomenuactions = dbConn.Query<dynamic>(SQL, parameters);
                    }
                    //cmd.Dispose();
                    //}
                    dbConn.Close();


                    //dapper cmd.Dispose ();
                    dbConn.Dispose();
                    return (new { boreport = objboreport, boreportcolumns = objboreportcolumn, results, statusdata, fkresults, bomenumaster, bomenuactions });
                }

                return (new { });
            }
            catch (Exception ex)
            {
                //if (dbConn != null) dbConn.Close ();
                string strError = ex.ToString();
                _logger.LogInfo("Error {s} "+ strError);
                return ex;
                //return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message + "    " + SQL + "\r\n    Report Code " + param.id + "   fkname " + param.fkname + " fk " + param.fk);
                //return Ok(new { err = strError, boreport = "" });//objboreport
                //
            }

            //

            return null;
        }
        public string[] getAllTables(string tblName, IEnumerable<dynamic> colconfigs, string alias, string formula)
        {
            string[] ret = new string[3];
            ArrayList tblconfigslist = new ArrayList();
            string Tables = "";
            string[] collist = formula.Split('~');
            string strwhere = "";
            //colconfig.columnname
            //var colconfigs = from a in _context.systemcolumns.Where(c => c.tablename == tblName && c.fk == true).Distinct() select a;

            foreach (var colconfig in colconfigs)
            {
                if (colconfig.fk == true)
                {

                    for (int c = 0; c < collist.Length; c++)
                    {
                        if (collist[c] == colconfig.columnname)// && colconfig.derived==false
                        {
                            if (colconfig.fktablename != "boconfigvalues" && colconfig.fktablename != "bomasterdatas")// && colconfig.fktablename != "usermasters" && colconfig.fktablename != "hrmsemployees"
                                collist[c] = " t" + colconfig.syscolumnid + "." + colconfig.fkdescription;
                            /*
                            else if (colconfig.fktablename == "usermasters" || colconfig.fktablename == "hrmsemployees")
                                collist[c] = " COALESCE ('<img style=\"icon\" src=\"http://localhost:5002/MyResources/' || t" + colconfig.syscolumnid + ".thumbnail || '>',t" + colconfig.syscolumnid + "." + colconfig.fkdescription + ")";
                            */
                            else
                                collist[c] = " case when t" + colconfig.syscolumnid + ".htmlcode is not null and t" + colconfig.syscolumnid + ".htmlcode!='' then  t" + colconfig.syscolumnid + ".htmlcode else t" + colconfig.syscolumnid + "." + colconfig.fkdescription + " end ";
                            //collist[c] = " COALESCE (t" + colconfig.syscolumnid + ".htmlcode,t" + colconfig.syscolumnid + "."+colconfig.fkdescription+")";

                            Tables += " left  join " + colconfig.fktablename + " t" + colconfig.syscolumnid;

                            strwhere = " on " + "cast(" + alias + "." + colconfig.columnname + " as varchar)=cast(" + "t" + colconfig.syscolumnid + "." + colconfig.fkidentityid + " as varchar)";

                            if (colconfig.fkwhere != null && colconfig.fkwhere != "")
                                strwhere += " and " + colconfig.fkwhere.Replace("{{a}}", "t" + colconfig.syscolumnid);
                            Tables += strwhere;
                            break;
                        }
                    }
                }
            }
            ret[0] = String.Join(' ', collist);
            ret[1] = Tables;
            ret[2] = "";
            return ret;
        }

        private string Format(string colname, dynamic coldetails)
        {
            string ret = colname;
            //MON,DD YYYY
            if (coldetails.datatype == "date") ret = "to_char(" + colname + ",'MON dd, YYYY') ";
            //if (coldetails.datatype == "date") ret = "to_date(to_char(" + colname + ",'DD-MON-YYYY'),'DD-MON-YYYY') ";
            if (coldetails.datatype == "time") ret = "to_char(" + colname + ", 'HH24:MI') ";
            //if (coldetails.datatype == "SS") ret = "getuseraccess(" + colname + ") ";
            return ret;
        }

    }
}

