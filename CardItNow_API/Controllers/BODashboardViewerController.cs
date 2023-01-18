using carditnow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using nTireBO.Models;

//using SunSmartnTireProducts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SunSmartnTireProducts.Controllers
{
    [Authorize]
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class BODashboardViewerController : ControllerBase
    {
        private readonly BODashboardViewerContext _context;

        public BODashboardViewerController(BODashboardViewerContext context)
        {
            _context = context;
        }

        private string GetApiModule(string port)
        {
            string ret = "";
            if (port == "7002") ret = "http://localhost:7002/ntireboapi";
            else if (port == "7002") ret = "http://localhost:7002/ntireboapi";
            else if (port == "7003") ret = "http://localhost:7003/ntirecamsapi";
            else if (port == "7018") ret = "http://localhost:7018/ntirecontentapi";
            else if (port == "7020") ret = "http://localhost:7020/ntireprojectapi";
            else if (port == "7008") ret = "http://localhost:7008/ntiredmsapi";
            else if (port == "7019") ret = "http://localhost:7019/ntireprocurementapi";
            else if (port == "7024") ret = "http://localhost:7024/ntirevisitorapi";
            else if (port == "7014") ret = "http://localhost:7014/ntirehrmsapi";
            else if (port == "7016") ret = "http://localhost:7016/ntirelegalapi";
            else if (port == "7006") ret = "http://localhost:7006/ntirecrmapi";
            else if (port == "7021") ret = "http://localhost:7021/ntirepropertyapi";
            else if (port == "7011") ret = "http://localhost:7011/ntirehelpdeskapi";
            return ret;
        }

        [HttpGet("{id}/{dt}/{p1?}/{p2?}/{p3?}")]
        public async Task<ActionResult<boreport>> GetDashboard(int id, string dt = "", string p1 = "", string p2 = "", string p3 = "")
        {
            try
            {
                var user = HttpContext.User;
                string s = user.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString();

                int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());

                string ConfigParam = "ApprovalStatus";

                var result = from a in _context.bodashboards.Where(f => (f.dashboardid ?? 0) == id)

                             select new
                             {
                                 dashboardid = a.dashboardid,
                                 dashboardname = a.dashboardname,
                                 rows = a.rows,
                                 cols = a.cols,
                                 design = a.design,
                                 remarks = a.remarks,
                                 userid = a.userid,
                                 module = a.module,
                                 status = a.status,
                             };
                var bodashboard = result.FirstOrDefault();

                var bodashboarddetail = (from a in _context.bodashboarddetails
                                         from t in _context.boconfigvalues.Where(t => a.charttype == t.configkey && t.param == "charttype").DefaultIfEmpty()
                                         from d1 in _context.boconfigvalues.Where(d1 => a.parameter1datetype == d1.configkey && d1.param == "datefiltertype").DefaultIfEmpty()
                                         from d2 in _context.boconfigvalues.Where(d2 => a.parameter2datetype == d2.configkey && d2.param == "datefiltertype").DefaultIfEmpty()
                                         from d3 in _context.boconfigvalues.Where(d3 => a.parameter3datetype == d3.configkey && d3.param == "datefiltertype").DefaultIfEmpty()
                                         from t1 in _context.masterdatatypes.Where(t1 => a.parameter1type == t1.datatypeid).DefaultIfEmpty()
                                         from t2 in _context.masterdatatypes.Where(t2 => a.parameter2type == t2.datatypeid).DefaultIfEmpty()
                                         from t3 in _context.masterdatatypes.Where(t3 => a.parameter3type == t3.datatypeid).DefaultIfEmpty()
                                         where a.dashboardid == id

                                         select new
                                         {
                                             a.dashboarddetailid,
                                             a.dashboardid,
                                             a.dashboardname,
                                             a.title,
                                             a.row,
                                             a.col,
                                             a.charttype,
                                             a.tablename,
                                             a.recordname,
                                             a.parameter,
                                             a.reportid,
                                             a.menuid,
                                             a.name,
                                             a.value,
                                             a.parameter1variable,
                                             a.parameter1type,
                                             a.parameter1datetype,
                                             a.parameter2variable,
                                             a.parameter2type,
                                             a.parameter2datetype,
                                             a.parameter3variable,
                                             a.parameter3type,
                                             a.parameter3datetype,
                                             a.status,
                                             charttypedesc = t.configtext,
                                             parameter1datetypedesc = d1.configtext,
                                             parameter2datetypedesc = d2.configtext,
                                             parameter3datetypedesc = d3.configtext,
                                             parameter1typedesc = t1.masterdatatypename,
                                             parameter2typedesc = t2.masterdatatypename,
                                             parameter3typedesc = t3.masterdatatypename,
                                         }).ToList();

                ArrayList results = new ArrayList();
                HttpClient client = new HttpClient();
                string value = Request.Headers["Authorization"];
                client.DefaultRequestHeaders.Add("Authorization", value);

                string strURL = "";
                string strActualParameter = "";
                string strParameter = "";
                dynamic response = null;
                dynamic responseOutput = null;
                ArrayList values = new ArrayList();
                string strParameter1 = "";
                dynamic jlabel = null;
                object objValue = null;
                dynamic datacount = null;
                dynamic jObj;

                ArrayList monthlist = new ArrayList();
                /*
                monthlist.Add(new ChartValueType { label = "Jan", name = "1", value = "0" });
                monthlist.Add(new ChartValueType { label = "Feb", name = "2", value = "0" });
                monthlist.Add(new ChartValueType { label = "Mar", name = "3", value = "0" });
                monthlist.Add(new ChartValueType { label = "Apr", name = "4", value = "0" });
                monthlist.Add(new ChartValueType { label = "May", name = "5", value = "0" });
                monthlist.Add(new ChartValueType { label = "Jun", name = "6", value = "0" });
                monthlist.Add(new ChartValueType { label = "Jul", name = "7", value = "0" });
                monthlist.Add(new ChartValueType { label = "Aug", name = "8", value = "0" });
                monthlist.Add(new ChartValueType { label = "Sep", name = "9", value = "0" });
                monthlist.Add(new ChartValueType { label = "Oct", name = "10", value = "0" });
                monthlist.Add(new ChartValueType { label = "Nov", name = "11", value = "0" });
                monthlist.Add(new ChartValueType { label = "Dec", name = "12", value = "0" });
                */

                monthlist.Add(new ChartValueType { name = "Jan", label = "1", value = "0" });
                monthlist.Add(new ChartValueType { name = "Feb", label = "2", value = "0" });
                monthlist.Add(new ChartValueType { name = "Mar", label = "3", value = "0" });
                monthlist.Add(new ChartValueType { name = "Apr", label = "4", value = "0" });
                monthlist.Add(new ChartValueType { name = "May", label = "5", value = "0" });
                monthlist.Add(new ChartValueType { name = "Jun", label = "6", value = "0" });
                monthlist.Add(new ChartValueType { name = "Jul", label = "7", value = "0" });
                monthlist.Add(new ChartValueType { name = "Aug", label = "8", value = "0" });
                monthlist.Add(new ChartValueType { name = "Sep", label = "9", value = "0" });
                monthlist.Add(new ChartValueType { name = "Oct", label = "10", value = "0" });
                monthlist.Add(new ChartValueType { name = "Nov", label = "11", value = "0" });
                monthlist.Add(new ChartValueType { name = "Dec", label = "12", value = "0" });

                object[] monthlistarr = monthlist.ToArray();
                //dynamic monthlistjson = new { data=monthlistarr };

                foreach (var dtl in bodashboarddetail)
                {
                    if (dtl.charttype == "R")
                    {
                        continue;
                    }
                    else if (dtl.reportid != null && dtl.reportid != "" && dtl.charttype != "R")//dtl.charttype != "R"
                    {
                        /*dynamic param = new { id= dtl.reportid,SessionUser= "ss",parameters= "",addparams= "",  status= "all",modulename= "",modulepkcol="",key="",pkvalue= 0};*/
                        ReportParam param = new ReportParam();
                        param.id = dtl.reportid; param.SessionUser = "ss"; param.parameters = null; param.addparams = null; param.status = "all"; param.modulename = ""; param.modulepkcol = ""; param.key = ""; param.pkvalue = 0;
                        response = (client.PostAsJsonAsync(GetApiModule("5000") + "/ReportViewer", param)).Result;
                        responseOutput = response.Content.ReadAsStringAsync();
                        jObj = JsonConvert.DeserializeObject(responseOutput.Result).results;
                        /*
                        for (int c = 1; c < jObj.Rows.Count; c++)
                        {
                            var cobj = jObj.Rows[c];
                            /*var cnt = cobj[dtl.value];
                            datacount = cnt.Value;
                            if (datacount == null) datacount = 0;
                        }*/
                        datacount = jObj.Rows.Count;
                        objValue = new ChartData { dashboardname = dtl.dashboardname, row = dtl.row, col = dtl.col, data = JsonConvert.SerializeObject(jObj.Rows), backgroundColor = getColor("background", datacount, dtl.dashboardid, dtl.row, dtl.col), hoverBackgroundColor = getColor("hover", datacount, dtl.dashboardid, dtl.row, dtl.col), borderColor = getColor("border", datacount, dtl.dashboardid, dtl.row, dtl.col) };
                        values = new ArrayList();
                        values.Add(objValue);
                    }
                    else
                    {
                        strURL = GetApiModule(bodashboard.module.ToString()) + "/" + dtl.recordname;

                        strParameter = dtl.parameter;
                        strActualParameter = dtl.parameter;
                        string label = "";
                        label = dtl.name;//dtl.parameter.Replace("_bydate", "");
                        if (label.IndexOf("/") >= 0) label = label.Substring(0, label.IndexOf("/"));
                        if (dtl.parameter1variable != null && dtl.parameter1variable != "")
                        {
                            strParameter = strParameter.Replace("{" + dtl.parameter1variable + "}", p1);
                            strActualParameter = strActualParameter.Replace("{" + dtl.parameter1variable + "}", "");
                        }
                        if (dtl.parameter2variable != null && dtl.parameter2variable != "")
                        {
                            strParameter = strParameter.Replace("{" + dtl.parameter2variable + "}", p2);
                            strActualParameter = strActualParameter.Replace("{" + dtl.parameter2variable + "}", "");
                        }
                        if (dtl.parameter3variable != null && dtl.parameter3variable != "")
                        {
                            strParameter = strParameter.Replace("{" + dtl.parameter3variable + "}", p3);
                            strActualParameter = strActualParameter.Replace("{" + dtl.parameter3variable + "}", "");
                        }

                        for (int i = 1; i <= 3; i++)
                        {
                            strParameter = strParameter.Replace("//", "/");
                            strActualParameter = strActualParameter.Replace("//", "/");
                        }

                        {
                            strParameter1 = "";

                            if (dtl.parameter == "monthwise" || dtl.parameter == "monthwise_bydate")
                            {
                                /*
                                var response1 = (client.GetAsync("http://localhost:7002/ntireboapi/boconfigvalue/param/month")).Result;
                                var responseOutput1 = response1.Content.ReadAsStringAsync();
                                jlabel = JsonConvert.DeserializeObject(responseOutput1.Result);
                                */
                                jlabel = JsonConvert.DeserializeObject<List<ChartValueType>>(JsonConvert.SerializeObject(monthlist));
                                /*
                                jlabel = JsonConvert.DeserializeObject<List<ChartValueType>>(@"[{ label = '1', name = '1', value = '0' },{ label = '2', name = '2', value = '0' },{ label = '3', name = '3', value = '0' },{ label = '4', name = '4', value = '0' },{ label = '5', name = '5', value = '0' },
{ label = '6', name = '6', value = '0' },{ label = '7', name = '7', value = '0' },{ label = '8', name = '8', value = '0' },{ label = '9', name = '9', value = '0' },
{ label = '10', name = '10', value = '0' },{ label = '11', name = '11', value = '0' },{ label = '12', name = '12', value = '0' },]");
                                */
                            }
                            else
                            {
                                strParameter1 = "/" + strParameter;// + label;//"/"+strParameter; // "/getList_"
                                //label = label.Replace("/date/ALL", "");
                                response = (client.GetAsync(strURL + "/getList_" + label)).Result;
                                responseOutput = response.Content.ReadAsStringAsync();
                                if (response.StatusCode == System.Net.HttpStatusCode.OK && responseOutput.Result != null) jlabel = JsonConvert.DeserializeObject(responseOutput.Result);
                            }
                        }
                        if (strParameter != "")
                        {
                            if (!strParameter.StartsWith('/')) strParameter = "/" + strParameter;
                        }

                        int max = 0;
                        if (dtl.charttype == "A" || dtl.charttype == "ML" || dtl.charttype == "MB" || dtl.charttype == "L" || dtl.charttype == "O") max = 1;
                        //if(dtl.parameter == "monthwise") max = 1;
                        objValue = null;
                        values = new ArrayList();
                        for (int i = 0; i <= max; i++)
                        {
                            string dt1 = dt;

                            strParameter1 = "";
                            if (max > 0)
                            {
                                if (i == 0) dt1 = "FY";
                                if (i == 1) dt1 = "LY";

                                strParameter1 = (strActualParameter.Replace("/date/ALL", "")) + "/date/" + dt1;
                            }
                            else
                            {
                                //strParameter1 = strParameter + "/date/" + dt1;
                                strParameter1 = strParameter;
                            }

                            for (int j = 1; j <= 3; j++)
                            {
                                strParameter1 = strParameter1.Replace("//", "/");
                            }
                            if (strParameter1 != "")
                            {
                                if (!strParameter1.StartsWith('/')) strParameter1 = "/" + strParameter1;
                            }

                            response = (client.GetAsync(strURL + strParameter1)).Result;

                            responseOutput = response.Content.ReadAsStringAsync();
                            jObj = JsonConvert.DeserializeObject<List<dynamic>>(responseOutput.Result);

                            if (jObj != null) datacount = jObj.Count;

                            if (datacount == null) datacount = 0;

                            objValue = new ChartData { dashboardname = dtl.dashboardname, row = dtl.row, col = dtl.col, data = responseOutput.Result, backgroundColor = getColor("background", datacount, dtl.dashboardid, dtl.row, dtl.col), hoverBackgroundColor = getColor("hover", datacount, dtl.dashboardid, dtl.row, dtl.col), borderColor = getColor("border", datacount, dtl.dashboardid, dtl.row, dtl.col) };
                            values.Add(objValue);
                        }
                    }
                    if (dtl.charttype == "A" || dtl.charttype == "ML" || dtl.charttype == "MB" || dtl.charttype == "O")
                    {
                        var objResult = ConvertSeries(values, dtl, jlabel);
                        datacount = objResult.Count;
                        results.Add(new { dashboardname = dtl.dashboardname, data = objResult, backgroundColor = getColor("background", datacount, dtl.dashboardid, dtl.row, dtl.col), hoverBackgroundColor = getColor("hover", datacount, dtl.dashboardid, dtl.row, dtl.col), borderColor = getColor("border", datacount, dtl.dashboardid, dtl.row, dtl.col) });
                    }
                    else
                    {
                        var objResult = Convert(values, dtl);
                        if (dtl.parameter == "monthwise" || dtl.parameter == "monthwise")
                        {
                            var obj_Label = objResult;
                            for (int h = 0; h < monthlistarr.Length; h++)
                            {
                                for (int k = 0; k < obj_Label.labels.Length; k++)
                                {
                                    try
                                    {
                                        dynamic dy = (dynamic)obj_Label;
                                        string dyname = ((dy).labels[k]).ToString();
                                        if ((((dynamic)monthlistarr[h])).name == dyname)
                                        {
                                            (((dynamic)monthlistarr[h])).value = ((dy).data[k]);
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string strexception = ex.ToString();
                                    }
                                }
                            }

                            for (int h = 0; h < monthlistarr.Length; h++)
                            {
                                ((dynamic)monthlistarr[h]).name = GetMonthName(((dynamic)monthlistarr[h]).name);
                            }
                            datacount = monthlistarr.Length;
                            results.Add(new { dashboardname = dtl.dashboardname, row = dtl.row, col = dtl.col, data = monthlistarr, backgroundColor = getColor("background", datacount, dtl.dashboardid, dtl.row, dtl.col), hoverBackgroundColor = getColor("hover", datacount, dtl.dashboardid, dtl.row, dtl.col), borderColor = getColor("border", datacount, dtl.dashboardid, dtl.row, dtl.col) });
                        }
                        else
                        {
                            datacount = objResult.data.Length;
                            results.Add(new { dashboardname = dtl.dashboardname, row = dtl.row, col = dtl.col, data = objResult, backgroundColor = getColor("background", datacount, dtl.dashboardid, dtl.row, dtl.col), hoverBackgroundColor = getColor("hover", datacount, dtl.dashboardid, dtl.row, dtl.col), borderColor = getColor("border", datacount, dtl.dashboardid, dtl.row, dtl.col) });
                        }
                    }
                }
                //}
                return Ok(new { bodashboard, bodashboarddetail, results });
            }
            catch (Exception ex)
            {
                string s = ex.InnerException.ToString();
                //_logger.LogError($"Controller: Dashboard() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
            return Ok(null);
        }

        private string[] getColor(string key, dynamic? count, int? dashboardid, int? row, int? col)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            try
            {
                if (count != null) count = int.Parse(count.ToString());
            }
            catch (Exception ex)
            { }
            string[] ret = new string[count];
            ArrayList bgcolors = new ArrayList();
            ArrayList hovercolors = new ArrayList();
            ArrayList bdcolors = new ArrayList();
            bgcolors.Add(new string[] { "#F1F1F1", "#202020", "#7E909A", "#1C4E80", "#A5D8DD", "#EA6A47", "#0091D5", "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", });
            bgcolors.Add(new string[] { "#000000", "#AC3E31", "#484848", "#DBAE58", "#DADADA", "#20283E", "#488A99", "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", });
            bgcolors.Add(new string[] { "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#F39B24", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#DC3811", "#3265CB", "#0098C6", "#980098", "#109618", "#FF9800", "#35D4ED", "#CDAEC8", "#57C879", "#003C5E", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#00283E", "#557D93", "#AABEC9", "#313131", "#7F6150", "#FFC3A1", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#5E3C00", "#AD9C7F", "#5FB965", "#773473", "#773473", "#EF69E7", "#F7B4F3", "#101896", "#878BCA", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", "#F39B24", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361" });
            bgcolors.Add(new string[] { "#B3C100", "#CED2CC", "#23282D", "#4CB5F5", "#1F3F49", "#D32D41", "#6AB187", "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", });
            bgcolors.Add(new string[] { "#F1F1F1", "#202020", "#7E909A", "#1C4E80", "#A5D8DD", "#EA6A47", "#0091D5", "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", });
            bgcolors.Add(new string[] { "#000000", "#AC3E31", "#484848", "#DBAE58", "#DADADA", "#20283E", "#488A99", "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", });
            bgcolors.Add(new string[] { "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#F39B24", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#DC3811", "#3265CB", "#0098C6", "#980098", "#109618", "#FF9800", "#35D4ED", "#CDAEC8", "#57C879", "#003C5E", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#00283E", "#557D93", "#AABEC9", "#313131", "#7F6150", "#FFC3A1", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#5E3C00", "#AD9C7F", "#5FB965", "#773473", "#773473", "#EF69E7", "#F7B4F3", "#101896", "#878BCA", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9" });
            bgcolors.Add(new string[] { "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", "#F39B24", "#42A5F5", "#9CCC65", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361" });
            bgcolors.Add(new string[] { "#B3C100", "#CED2CC", "#23282D", "#4CB5F5", "#1F3F49", "#D32D41", "#6AB187", "#F25C5F", "#52219C", "#3A415A", "#F1C857", "#F1C857", "#1DA185", "#9BBB5C", "#F39B24", });

            /*   bgcolors.Add(new string[]{});
                bgcolors.Add(new string[]{});
                 bgcolors.Add(new string[]{});
                  bgcolors.Add(new string[]{});
                   bgcolors.Add(new string[]{});
                    bgcolors.Add(new string[]{});
                     bgcolors.Add(new string[]{});
                      bgcolors.Add(new string[]{});*/

            string[] hoverBackgroundColors = { "#1E88E5", "#D38A3B", "#7CB342", "#008744", "#0057e7", "#1E88E5", "#7CB342", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#1E88E5", "#D38A3B", "#7CB342", "#008744", "#0057e7", "#1E88E5", "#7CB342", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361" };
            string[] borderColors = { "#1E88E5", "#7CB342", "#D38A3B", "#008744", "#0057e7", "#1E88E5", "#7CB342", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#1E88E5", "#7CB342", "#D38A3B", "#008744", "#0057e7", "#1E88E5", "#7CB342", "#008744", "#0057e7", "#d62d20", "#ffa700", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361", "#6544a9", "#51cc00", "#5d4361" };
            string[] backgroundColors = null;

            int index = (((int)dashboardid * 1) + ((int)row * 2) + ((int)col * 3)) % 10;
            backgroundColors = (string[])bgcolors[index];

            for (int i = 0; i < count; i++)
            {
                int bgindex = ((int)(i % backgroundColors.Length));
                if (key == "background") ret[i] = backgroundColors[bgindex];
                if (key == "hover") ret[i] = hoverBackgroundColors[bgindex];
                if (key == "border") ret[i] = borderColors[bgindex];
            }

            return ret;
        }

        private string GetMonthName(string mon)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            string ret;
            if (mon == "1") return "Jan";
            else if (mon == "2") return "Feb";
            else if (mon == "3") return "Mar";
            else if (mon == "4") return "Apr";
            else if (mon == "5") return "May";
            else if (mon == "6") return "Jun";
            else if (mon == "7") return "Jul";
            else if (mon == "8") return "Aug";
            else if (mon == "9") return "Sep";
            else if (mon == "10") return "Oct";
            else if (mon == "11") return "Nov";
            else if (mon == "12") return "Dec";
            return "";
        }

        private dynamic Convert(ArrayList values, dynamic dtl)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            //ArrayList ret = new ArrayList();
            ArrayList labels = new ArrayList();
            ArrayList data = new ArrayList();
            for (int i = 0; i < values.Count; i++)
            {
                ChartData objValue = (ChartData)values[i];
                //dynamic charttype_Value = JObject.Parse(objValue.Data);

                JArray jArr_Value = (JArray)JsonConvert.DeserializeObject(objValue.data);

                if (jArr_Value != null)
                {
                    for (int j = 0; j < jArr_Value.Count; j++)
                    {
                        dynamic charttype_Value = JObject.Parse(jArr_Value[j].ToString());

                        //ChartValueType value = new ChartValueType { name = charttype_Value[dtl.name].Value, value = charttype_Value[dtl.value].Value.ToString() };

                        //ret.Add(value);
                        labels.Add(charttype_Value[dtl.name]?.Value);
                        if (dtl.value == null)
                            data.Add("0");
                        else
                        {
                            dynamic val = charttype_Value[dtl.value]?.Value;
                            if (val == null) val = 0;
                            data.Add(val.ToString());
                        }
                    }
                }
            }

            var ret = new { labels = labels.ToArray(), data = data.ToArray() };
            return ret;
        }

        /*
                private ArrayList Convert1(ArrayList values, dynamic dtl)
                {
                    int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                    ArrayList ret = new ArrayList();
                    for (int i = 0; i < values.Count; i++)
                    {
                        ChartData objValue = (ChartData)values[i];
                        //dynamic charttype_Value = JObject.Parse(objValue.Data);

                        JArray jArr_Value = (JArray)JsonConvert.DeserializeObject(objValue.data);

                        for (int j = 0; j < jArr_Value.Count; j++)
                        {
                            dynamic charttype_Value = JObject.Parse(jArr_Value[j].ToString());

                            ChartValueType value = new ChartValueType { name = charttype_Value[dtl.name].Value, value = charttype_Value[dtl.value].Value.ToString() };

                            ret.Add(value);
                        }
                    }
                    return ret;
                }
        */

        private ArrayList ConvertSeries(ArrayList values, dynamic dtl, dynamic jlabels)
        {
            int cid = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            ArrayList ret = new ArrayList();

            dynamic obj_Label;
            dynamic jArr_Label;

            if (dtl.parameter == "monthwise")
            {
                jArr_Label = jlabels;
                //obj_Label = (ArrayList)jlabels;
                //jArr_Label = (JArray)JsonConvert.DeserializeObject(jlabels.data);
            }
            else
            {
                jArr_Label = jlabels;
                //obj_Label = (ChartData)jlabels;
                //jArr_Label = (JArray)JsonConvert.DeserializeObject(jlabels.data);
            }

            ChartData objValue = (ChartData)values[0];
            JArray jArr_FY = (JArray)JsonConvert.DeserializeObject(objValue.data);

            ChartData objResult = null;
            JArray jArr_LY = null;

            if (values.Count > 1)
            {
                objResult = (ChartData)values[1];
                jArr_LY = (JArray)JsonConvert.DeserializeObject(objResult.data);
            }
            if (jArr_Label != null)
            {
                for (int j = 0; j < jArr_Label.Count; j++)
                {
                    dynamic charttype_Label;
                    // if (dtl.parameter == "monthwise")
                    //charttype_Label = jArr_Label[j];
                    //else
                    charttype_Label = jArr_Label[j];
                    dynamic charttype_LY = null;

                    //ChartValueType value = new ChartValueType { name = charttype_FY[dtl.name].Value, value = charttype_FY[dtl.value].Value };

                    ChartValueType value = new ChartValueType { name = charttype_Label.name, value = 0 };
                    if (jArr_FY != null)
                    {
                        for (int k = 0; k < jArr_FY.Count; k++)
                        {
                            dynamic charttype_FY = JObject.Parse(jArr_FY[k].ToString());

                            if (charttype_FY[dtl.name] != null && charttype_Label.label == charttype_FY[dtl.name].Value.ToString())
                            {
                                value = new ChartValueType { name = charttype_Label.name, value = charttype_FY[dtl.value].Value };
                                break;
                            }
                        }
                    }
                    ChartValueType value1 = new ChartValueType { name = charttype_Label.name, value = 0 };
                    if (jArr_LY != null)
                    {
                        for (int k = 0; k < jArr_LY.Count; k++)
                        {
                            charttype_LY = JObject.Parse(jArr_LY[k].ToString());

                            if (charttype_LY[dtl.name] != null && charttype_Label.label == charttype_LY[dtl.name].Value.ToString())
                            {
                                value1 = new ChartValueType { name = charttype_Label.name, value = charttype_LY[dtl.value].Value };
                                break;
                            }
                        }
                    }
                    ArrayList series = new ArrayList();
                    series.Add(new { name = DateTime.Now.Year.ToString(), value = (value == null ? "0" : value.value) });
                    series.Add(new { name = (DateTime.Now.Year - 1).ToString(), value = (value1 == null ? "0" : value1.value) });
                    ret.Add(new { name = charttype_Label.name == null ? charttype_Label.label : charttype_Label.name, label = charttype_Label.label, series });
                }
            }
            return ret;
        }
    }

    public class ChartData
    {
        public string dashboardname;
        public dynamic row;
        public dynamic col;
        public dynamic data;
        public string[] backgroundColor;
        public string[] hoverBackgroundColor;
        public string[] borderColor;
    }

    public class ChartValueType
    {
        public dynamic label;
        public dynamic name;
        public dynamic value;
    }
}