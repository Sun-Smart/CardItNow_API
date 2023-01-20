//------------------------------------------------------------------------------

using carditnow.Models;
using carditnow.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*
////////using FluentDateTime;
////////using FluentDate;
using System.Data;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

//using SunSmartnTireProducts.Models;
using nTireBO.Models;
using nTireBO.Services;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace nTireBOWebAPI.Controllers
{
    [AllowAnonymous]
    [Route("carditnowapi/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly usermasterContext _context;
        private IConfiguration _config;
        private ItokenService _service;
        // private readonly IcustomermasterService _customermasterService;

        //public TokenController(IcustomermasterService obj_customermasterService)
        //{
        //    _customermasterService = obj_customermasterService;           

        //}

        public static TokenController Current { get; set; }
        public string BaseAddress { get; set; } = @"http://localhost:65521/";
        public string RealmId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long AccessTokenExpiresIn { get; set; }
        public string SuccessView { get; set; }
        public string ErrorView { get; set; }

        public TokenController(IConfiguration config, usermasterContext context, ItokenService service)
        {
            _config = config;
            _context = context;
            _service = service;
        }

        [HttpPost]
        //[GzipCompression]
        [Route("forgot")]
        public async void forgot([FromBody] ForgotPassword pwdform)
        {
            var claims = new[] {
                new Claim ("email", pwdform.email.ToString ())
            };
            var tokenString = _service.GetToken(claims, 300);
            string data = "Please follow this link to reset your password:<p><a href='http://localhost/ntire/SSnTireApp/#/resetpassword?sptoken=" + tokenString + "'>http://localhost/ntire/SSnTireApp/#/resetpassword?sptoken=" + tokenString + "</a>";
            Helper.Email(data, pwdform.email.ToString(), "User", "SunSmart");
        }

        [HttpPost]
        //[GzipCompression]
        [Route("change")]
        public async void resetpassword([FromBody] ResetPassword resetpwd)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(resetpwd.sptoken) as JwtSecurityToken;
            try
            {
                string email = tokenS.Claims.FirstOrDefault(c => c.Type == "email").Value.ToString();
                string sql = "UPDATE usermasters SET emailpassword = crypt('" + resetpwd.password + "',gen_salt('bf')) where email='" + email + "'";
                _context.Database.ExecuteSqlRaw(sql);
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        [HttpGet]
        //[GzipCompression]
        [Route("config/{sptoken}")]
        public IActionResult config(string sptoken)
        {
            return Ok();
        }



        [HttpGet]
        public IActionResult CreateToken(LoginModel login)
        { //LoginModel login
            /*LoginModel login=new LoginModel();
            login.Username=Username;
            login.Password=Password;*/
            //IActionResult response = Unauthorized();
            /*

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer EK5OQFKOYIIKTPTFCGBMOWEEBRKC2U66");

            {
                var fileStreamContent = new StreamContent((System.IO.File.OpenRead("C:\\Users\\Lenovo\\Downloads\\speech.ogg")));
                //var fileStreamContent = new ByteArrayContent((System.IO.File.ReadAllBytes("C:\\Users\\Lenovo\\Downloads\\speech.wav")));

                fileStreamContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("audio/mpeg");//audio/wav
                // multipartFormContent.Add(fileStreamContent, name: "file", fileName: "speech.mp3");

                var response1 =await (client.PostAsync("https://api.wit.ai/speech?v=20220622", fileStreamContent));
                //var response1 = await (client.GetAsync("https://api.wit.ai/message?v=20220712&q=AccountBalance"));
                var responseresult = response1.Content.ReadAsStringAsync();
            }
            var response2 = Ok(new { token = "" });
            return;
            */
            //if (((login.email != null)||(login.email!="")) && ((login.Password != null) || (login.Password != "")))
            //{
            if (!string.IsNullOrEmpty(login.email) && (!string.IsNullOrEmpty(login.Password)))
            {

                IActionResult response; //= Ok(new { token = "" });

                if (login.host != null) login.host = login.host.Replace(".sunsmart.com", "");
                var user = _service.Authenticate(login);

                if (user != null)
                {
                    var tokenString = _service.BuildToken(user);
                    response = Ok(new { token = tokenString });
                    return response;
                }
                else
                {

                    return response = Ok(new { Message = "Invalid login credential" });
                }
            }

            else if((!string.IsNullOrEmpty(login.email))&&(string.IsNullOrEmpty(login.Password)))
            {
                return new OkObjectResult(new { Message = "Please enter your Password !" });
            }
            else if((!string.IsNullOrEmpty(login.Password))&&(string.IsNullOrEmpty(login.email)))
            {
                return new OkObjectResult(new { Message = "Please enter your Email ID !" });
            }
            else
            {
                return new OkObjectResult(new { Message = "Please enter your Email ID and Password !" });
            }

        }

        private static Random random = new Random();
        private object service;

        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        //public async Task<ActionResult<string>> SendOTP(string email)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(email))
        //        {
        //            var result = _customermasterService.SendOTP(email);
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return Content("Required email id");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.LogError($"Controller: GeGetUserEmail_validate(string email)\r\n{ex}");
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, "GeGetUserEmail_validate " + ex.Message + "  " + ex.InnerException?.Message);
        //    }

        //}

    }
}