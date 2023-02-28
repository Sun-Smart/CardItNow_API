using carditnow.Models;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nTireBO.Services;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace carditnow.Controllers
{

    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class customermasterController : baseController
    {
        private ILoggerManager _logger;
        private int cid = 0;
        private int uid = 0;
        private string uname = "";
        private string uidemail = "";
        private readonly IcustomermasterService _customermasterService;
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly IavatarmasterService _avatarmasterService;
        private readonly IcustomerdetailService _customerdetailService;
        private readonly IcustomertermsacceptanceService _customertermsacceptanceService;
        private readonly IcustomerpaymodeService _customerpaymodeService;
        private readonly IcustomersecurityquestionService _customersecurityquestionService;
        private readonly IcustomersecurityquestionshistoryService _customersecurityquestionshistoryService;

        public customermasterController(IHttpContextAccessor objhttpContextAccessor, IcustomermasterService obj_customermasterService, IboconfigvalueService obj_boconfigvalueService, IavatarmasterService obj_avatarmasterService, IcustomerdetailService obj_customerdetailService, IcustomertermsacceptanceService obj_customertermsacceptanceService, IcustomerpaymodeService obj_customerpaymodeService, IcustomersecurityquestionService obj_customersecurityquestionService, IcustomersecurityquestionshistoryService obj_customersecurityquestionshistoryService, ILoggerManager logger) : base(logger)
        {
            _customermasterService = obj_customermasterService;
            _logger = logger;
            if (objhttpContextAccessor.HttpContext.User.Claims.Any())
            {
                cid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
                uid = int.Parse(objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
                uname = "";
                uidemail = "";
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
                if (objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid") != null) uidemail = objhttpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "emailid").Value.ToString();
                _customermasterService = obj_customermasterService;
                _boconfigvalueService = obj_boconfigvalueService;
                _avatarmasterService = obj_avatarmasterService;
                _customerdetailService = obj_customerdetailService;
                _customertermsacceptanceService = obj_customertermsacceptanceService;
                _customerpaymodeService = obj_customerpaymodeService;
                _customersecurityquestionService = obj_customersecurityquestionService;
                _customersecurityquestionshistoryService = obj_customersecurityquestionshistoryService;
            }
        }

        [Authorize]
        // GET: api/customermaster
        [HttpGet]
        public async Task<ActionResult<dynamic>> Get_customermasters()
        {
            try
            {
                var result = _customermasterService.Get_customermasters();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customermasters()\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customermasters " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // PUT: api/customermaster/5
        [Authorize]
        [HttpGet]
        [Route("fulllist")]
        public async Task<ActionResult<IEnumerable<Object>>> GetFullList()
        {
            try
            {
                var result = _customermasterService.GetFullList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        //Dump of the table.If param field exists, filter by param
        [Route("param/{key}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetList(string key)
        {
            try
            {
                var result = _customermasterService.GetList(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetList(string  key) \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("customerid/{customerid}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetListBy_customerid(int customerid)
        {
            try
            {
                var result = _customermasterService.GetListBy_customerid(customerid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetListBy_customerid(int customerid)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetListBy_customerid " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpPost("SendOTP")]
        public async Task<ActionResult<IEnumerable<Object>>> SendOTP(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var result = _customermasterService.SendOTP(email);
                    return Ok(result);
                }
                else
                {
                    return Content("fail");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Password Config(string email,string password)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Password Config " + ex.Message + "  " + ex.InnerException?.Message);
            }

        }

        [HttpPost("Password Config")]
        public async Task<ActionResult<IEnumerable<Object>>> PasswordSet(string email, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var result = _customermasterService.PasswordSet(email, password);
                    return Ok(result);
                }
                else
                {
                    return Content("fail");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Password Config(string email,string password)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Password Config " + ex.Message + "  " + ex.InnerException?.Message);
            }

        }

        [HttpPost("TPINConfig")]
        public async Task<ActionResult<IEnumerable<Object>>> SetPinConfig(string email, string pin)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var result = _customermasterService.SetPinConfig(email, pin);
                    return Ok(result);
                }
                else
                {
                    return Content("fail");
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex, "SetPinConfig");
            }

        }

        [HttpPost("ProcessDocument")]
        public async Task<ActionResult<IEnumerable<Object>>> ProcessDocument(processdocument model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.email))
                {
                    var result = _customermasterService.ProcessDocument(model);
                    return Ok(result);
                }
                else
                {
                    //throw new Exception("Test error message");
                    return StatusCode(StatusCodes.Status400BadRequest, "Email is Required");                    
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Process Document");
            }

        }

        [HttpPost("UpdateProfileInformation")]
        public dynamic UpdateProfileInformation(ProfileInformationUpdate model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.email))
                {
                    var result = _customermasterService.UpdateProfileInformation(model);
                    return Ok(result);
                }
                else
                {
                    //throw new Exception("Test error message");
                    return StatusCode(StatusCodes.Status400BadRequest, "Email is Required");
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Process Document");
            }

        }


        [HttpGet("e/{sid}")]
        public async Task<ActionResult<customermaster>> Get_customermaster(string sid)
        {
            try
            {
                var result = _customermasterService.Get_customermaster(sid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Get_customermaster(string sid)\r\n {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customermaster(string sid) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // GET: api/customermaster/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<customermaster>> Get_customermaster(int id)
        {
            try
            {
                var result = _customermasterService.Get_customermaster(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Get_customermaster(int id)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Get_customermaster(int id) " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getdefaultdata")]
        public async Task<ActionResult<Object>> GetDefaultData()
        {
            try
            {
                var list_mode = getList_mode().Result.Result;
                var list_type = getList_type().Result.Result;
                var list_defaultavatar = getList_defaultavatar().Result.Result;
                var result = (new { list_mode, list_type, list_defaultavatar });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:GetDefaultData() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetDefaultData() " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // POST: api/customermaster
        //[HttpPost]
        //public async Task<ActionResult<customermaster>> Post_customermaster()
        //{
        //    string token = Request.Headers["Authorization"].ToString();
        //    try
        //    {
        //        customermasterView obj_customermaster = JsonConvert.DeserializeObject<customermasterView>(Request.Form["formData"]);
        //        var result = _customermasterService.Save_customermaster(token, obj_customermaster.data);
        //        HttpClient client = new HttpClient();
        //        client.DefaultRequestHeaders.Add("Authorization", token);
        //        if (obj_customermaster.customerdetails != null && obj_customermaster.customerdetails.Count > 0)
        //        {
        //            foreach (var obj in obj_customermaster.customerdetails)
        //            {
        //                if (obj.customerdetailid == null)
        //                {
        //                    obj.customerid = result.customermaster.customerid;
        //                    _customerdetailService.Save_customerdetail(token, obj);
        //                }
        //            }
        //        }
        //        if (obj_customermaster.Deleted_customerdetail_IDs != null && obj_customermaster.Deleted_customerdetail_IDs != "")
        //        {
        //            string[] ids = obj_customermaster.Deleted_customerdetail_IDs.Split(',');
        //            foreach (var id in ids)
        //            {
        //                if (id != "")
        //                {
        //                    _customerdetailService.Delete(int.Parse(id));
        //                }
        //            }
        //        }
        //        if (obj_customermaster.customertermsacceptances != null && obj_customermaster.customertermsacceptances.Count > 0)
        //        {
        //            foreach (var obj in obj_customermaster.customertermsacceptances)
        //            {
        //                if (obj.customertermid == null)
        //                {
        //                    obj.customerid = result.customermaster.customerid;
        //                    _customertermsacceptanceService.Save_customertermsacceptance(token, obj);
        //                }
        //            }
        //        }
        //        if (obj_customermaster.Deleted_customertermsacceptance_IDs != null && obj_customermaster.Deleted_customertermsacceptance_IDs != "")
        //        {
        //            string[] ids = obj_customermaster.Deleted_customertermsacceptance_IDs.Split(',');
        //            foreach (var id in ids)
        //            {
        //                if (id != "")
        //                {
        //                    _customertermsacceptanceService.Delete(int.Parse(id));
        //                }
        //            }
        //        }
        //        if (obj_customermaster.customerpaymodes != null && obj_customermaster.customerpaymodes.Count > 0)
        //        {
        //            foreach (var obj in obj_customermaster.customerpaymodes)
        //            {
        //                if (obj.payid == null)
        //                {
        //                    obj.customerid = result.customermaster.customerid;
        //                    _customerpaymodeService.Save_customerpaymode(token, obj);
        //                }
        //            }
        //        }
        //        if (obj_customermaster.Deleted_customerpaymode_IDs != null && obj_customermaster.Deleted_customerpaymode_IDs != "")
        //        {
        //            string[] ids = obj_customermaster.Deleted_customerpaymode_IDs.Split(',');
        //            foreach (var id in ids)
        //            {
        //                if (id != "")
        //                {
        //                    _customerpaymodeService.Delete(int.Parse(id));
        //                }
        //            }
        //        }
        //        if (obj_customermaster.customersecurityquestions != null && obj_customermaster.customersecurityquestions.Count > 0)
        //        {
        //            foreach (var obj in obj_customermaster.customersecurityquestions)
        //            {
        //                if (obj.securityquestionid == null)
        //                {
        //                    obj.customerid = result.customermaster.customerid;
        //                    _customersecurityquestionService.Save_customersecurityquestion(token, obj);
        //                }
        //            }
        //        }
        //        if (obj_customermaster.Deleted_customersecurityquestion_IDs != null && obj_customermaster.Deleted_customersecurityquestion_IDs != "")
        //        {
        //            string[] ids = obj_customermaster.Deleted_customersecurityquestion_IDs.Split(',');
        //            foreach (var id in ids)
        //            {
        //                if (id != "")
        //                {
        //                    _customersecurityquestionService.Delete(int.Parse(id));
        //                }
        //            }
        //        }
        //        if (obj_customermaster.customersecurityquestionshistories != null && obj_customermaster.customersecurityquestionshistories.Count > 0)
        //        {
        //            foreach (var obj in obj_customermaster.customersecurityquestionshistories)
        //            {
        //                if (obj.historyid == null)
        //                {
        //                    obj.customerid = result.customermaster.customerid;
        //                    _customersecurityquestionshistoryService.Save_customersecurityquestionshistory(token, obj);
        //                }
        //            }
        //        }
        //        if (obj_customermaster.Deleted_customersecurityquestionshistory_IDs != null && obj_customermaster.Deleted_customersecurityquestionshistory_IDs != "")
        //        {
        //            string[] ids = obj_customermaster.Deleted_customersecurityquestionshistory_IDs.Split(',');
        //            foreach (var id in ids)
        //            {
        //                if (id != "")
        //                {
        //                    _customersecurityquestionshistoryService.Delete(int.Parse(id));
        //                }
        //            }
        //        }
        //        if (Request.Form.Files != null)
        //        {
        //            foreach (var file in Request.Form.Files)
        //            {
        //                Helper.Upload(file);
        //            }
        //        }

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Controller:Save api {ex}");
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
        //    }
        //}

//new
        [HttpPost]
        public async Task<ActionResult<customermaster>> Post_customermaster(customermasterView obj_customermaster)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
               // customermasterView obj_customermaster = JsonConvert.DeserializeObject<customermasterView>(Request.Form["formData"]);
                var result = _customermasterService.Save_customermaster(token, obj_customermaster.data);
                //HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Authorization", token);
                if (obj_customermaster.customerdetails != null && obj_customermaster.customerdetails.Count > 0)
                {
                    foreach (var obj in obj_customermaster.customerdetails)
                    {
                        if (obj.customerdetailid == null)
                        {
                            obj.customerid = result.customermaster.customerid;
                            _customerdetailService.Save_customerdetail(token, obj);
                        }
                    }
                }
                if (obj_customermaster.Deleted_customerdetail_IDs != null && obj_customermaster.Deleted_customerdetail_IDs != "")
                {
                    string[] ids = obj_customermaster.Deleted_customerdetail_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _customerdetailService.Delete(int.Parse(id));
                        }
                    }
                }
                if (obj_customermaster.customertermsacceptances != null && obj_customermaster.customertermsacceptances.Count > 0)
                {
                    foreach (var obj in obj_customermaster.customertermsacceptances)
                    {
                        if (obj.customertermid == null)
                        {
                            obj.customerid = result.customermaster.customerid;
                            _customertermsacceptanceService.Save_customertermsacceptance(token, obj);
                        }
                    }
                }
                if (obj_customermaster.Deleted_customertermsacceptance_IDs != null && obj_customermaster.Deleted_customertermsacceptance_IDs != "")
                {
                    string[] ids = obj_customermaster.Deleted_customertermsacceptance_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _customertermsacceptanceService.Delete(int.Parse(id));
                        }
                    }
                }
                if (obj_customermaster.customerpaymodes != null && obj_customermaster.customerpaymodes.Count > 0)
                {
                    foreach (var obj in obj_customermaster.customerpaymodes)
                    {
                        if (obj.payid == null)
                        {
                            obj.customerid = result.customermaster.customerid;
                            _customerpaymodeService.Save_customerpaymode(token, obj);
                        }
                    }
                }
                if (obj_customermaster.Deleted_customerpaymode_IDs != null && obj_customermaster.Deleted_customerpaymode_IDs != "")
                {
                    string[] ids = obj_customermaster.Deleted_customerpaymode_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _customerpaymodeService.Delete(int.Parse(id));
                        }
                    }
                }
                if (obj_customermaster.customersecurityquestions != null && obj_customermaster.customersecurityquestions.Count > 0)
                {
                    foreach (var obj in obj_customermaster.customersecurityquestions)
                    {
                        if (obj.securityquestionid == null)
                        {
                            obj.customerid = result.customermaster.customerid;
                            _customersecurityquestionService.Save_customersecurityquestion(token, obj);
                        }
                    }
                }
                if (obj_customermaster.Deleted_customersecurityquestion_IDs != null && obj_customermaster.Deleted_customersecurityquestion_IDs != "")
                {
                    string[] ids = obj_customermaster.Deleted_customersecurityquestion_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _customersecurityquestionService.Delete(int.Parse(id));
                        }
                    }
                }
                if (obj_customermaster.customersecurityquestionshistories != null && obj_customermaster.customersecurityquestionshistories.Count > 0)
                {
                    foreach (var obj in obj_customermaster.customersecurityquestionshistories)
                    {
                        if (obj.historyid == null)
                        {
                            obj.customerid = result.customermaster.customerid;
                            _customersecurityquestionshistoryService.Save_customersecurityquestionshistory(token, obj);
                        }
                    }
                }
                if (obj_customermaster.Deleted_customersecurityquestionshistory_IDs != null && obj_customermaster.Deleted_customersecurityquestionshistory_IDs != "")
                {
                    string[] ids = obj_customermaster.Deleted_customersecurityquestionshistory_IDs.Split(',');
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            _customersecurityquestionshistoryService.Delete(int.Parse(id));
                        }
                    }
                }
                //if (Request.Form.Files != null)
                //{
                //    foreach (var file in Request.Form.Files)
                //    {
                //        Helper.Upload(file);
                //    }
                //}

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Save api {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_mode")]
        public async Task<ActionResult<dynamic>> getList_mode()
        {
            try
            {
                var result = _boconfigvalueService.GetList("mode");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_mode() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_type")]
        public async Task<ActionResult<dynamic>> getList_type()
        {
            try
            {
                var result = _boconfigvalueService.GetList("customermastertype");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_type() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        [HttpGet]
        [Route("getList_defaultavatar")]
        public async Task<ActionResult<dynamic>> getList_defaultavatar()
        {
            try
            {
                string strCondition = "";
                var result = _avatarmasterService.GetList(strCondition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: getList_defaultavatar() {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex.Message + "  " + ex.InnerException?.Message);
            }
        }

        // DELETE: api/customermaster/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<customermaster>> Delete(int id)
        {
            try
            {
                var result = _customermasterService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Delete");
            }
        }

        [HttpPost]
        [Route("Customerauthendication")]
        public dynamic Customerauth(customerauth model)
        {
            var result = _customermasterService.Customerauth(model);
            return result;

        }
        [Authorize]
        [HttpPost]
        [Route("AccountSuspend")]
        public dynamic customeraccountsuspend(suspendAccount model)
        {
            if( (model.customerid!=null) && (Convert.ToString(model.customerid)!=""))
            {
                var result = _customermasterService.account_Suspend(model);
                return result;
            }
            else
            {
                var result1 = new
                {
                    status = "fail",
                    data = "null",/* Application-specific data would go here. */
                    message = "The customer id is null or empty." /* Or optional success message */
                };
                return JsonConvert.SerializeObject(result1);
            }
            return null;
        }

        //account_Suspend_Reactive

        [Authorize]
        [HttpPost]
        [Route("AccountReactive")]
        public dynamic account_Suspend_Reactive(suspendAccount model)
        {
            if ((model.customerid.ToString() != null) && (Convert.ToString(model.customerid) != ""))
            {
                var result = _customermasterService.account_Suspend_Reactive(model);
                return result;
            }
            else
            {
                var result1 = new
                {
                    status = "fail",
                    data = "null",/* Application-specific data would go here. */
                    message = "The customer id is null or empty." /* Or optional success message */
                };
                return JsonConvert.SerializeObject(result1);
            }
            return null;
        }


        //shy 16.2.2023

        [HttpPost]
        [Route("GetDocumenttype")]
        public dynamic GetDocumenttype(customermaster model)
        {
            // ProcessOCRResposeView response = new ProcessOCRResposeView();
            string geoig = model.geoid;
            try
            {
                var result = _customermasterService.GetListdocument_bygeoid(geoig);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller:Save api {ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Save " + ex.Message + "  " + ex.InnerException?.Message);
            }

        }


        //end

        //shy new senotp POST

        [HttpPost("SendOTP1")]
        public async Task<ActionResult<IEnumerable<Object>>> SendOTP(customermaster model)
        {
            try
            {
                string email = model.email;
                string geoid = model.geoid;


                if (!string.IsNullOrEmpty(email))
                {
                    var result = _customermasterService.SendOTP1(email,geoid);
                    return Ok(result);
                }
                else
                {
                    return Content("fail");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Password Config(string email,string password)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Password Config " + ex.Message + "  " + ex.InnerException?.Message);
            }

        }



        //feb23
        //shy new senotp POST

        [HttpPost("ReSendOTP")]
        public async Task<ActionResult<IEnumerable<Object>>> ReSendOTP(customermaster model)
        {
            try
            {
                string email = model.email;
                string geoid = model.geoid;


                if (!string.IsNullOrEmpty(email))
                {
                    var result = _customermasterService.ReSendOTP(email);
                    return Ok(result);
                }
                else
                {
                    return Content("fail");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: Password Config(string email,string password)\r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "Password Config " + ex.Message + "  " + ex.InnerException?.Message);
            }

        }






        //feb 20

        [HttpGet]
        [Route("GetsecurityQuestions")]
        public async Task<ActionResult<IEnumerable<Object>>> GetsecurityQuestions()
        {
            try
            {
                var result = _customermasterService.GetsecurityQuestions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }




    }
}