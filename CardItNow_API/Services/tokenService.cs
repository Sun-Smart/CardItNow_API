
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using nTireBO.Models;
using carditnow.Models;
using SunSmartnTireProducts.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace nTireBO.Services
{
    public class tokenService: ItokenService
    {

        private ILoggerManager _logger;
        private IHttpContextAccessor httpContextAccessor;


        int cid = 0;
        int uid = 0;
        string uname = "";
        string uidemail = "";




        public tokenService(ILoggerManager logger, IHttpContextAccessor objhttpContextAccessor)
        {

            _logger = logger;
            this.httpContextAccessor = objhttpContextAccessor;
            if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid")!=null)cid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "companyid").Value.ToString());
            if(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid")!=null) uid = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString());
            uname = "";
            uidemail = "";
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username") != null) uname = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString();
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email") != null) uidemail = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email").Value.ToString();
        }
        public string GetToken(Claim[] claims, int expireminutes)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryVerySecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("http://108.60.219.44:63939/",
                "http://108.60.219.44:63939/",
                claims,
                expires: DateTime.Now.AddMinutes(expireminutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string BuildToken(UserCredential user)
        {
            
            var claims = new[] {
                new Claim ("companyid", user.companyid == null? "": user.companyid.ToString ()),
                new Claim ("pkcol", user.pkcol == null? "": user.pkcol),
                new Claim ("code", user.code == null? "": user.code),
                new Claim ("username", user.username == null? "": user.username.ToString ()),
                new Claim ("userid", user.userid == null? "": user.userid.ToString ()),
                new Claim ("usertype", user.userroleid == null? "": user.userroleid.ToString ()),
                new Claim ("employeeid", user.employeeid == null? "": user.employeeid.ToString ()),
                new Claim ("userroleid", user.userroleid == null? "": user.userroleid.ToString ()),
                new Claim ("role", user.role == null? "": user.role.ToString ()),
                new Claim ("branchid", user.branchid == null? "": user.branchid.ToString ()),
                new Claim ("branchiddesc", user.branchiddesc == null? "": user.branchiddesc.ToString ()),
                new Claim ("finyearid", user.finyearid == null? "": user.finyearid.ToString ()),
                new Claim ("finyeardesc", user.finyeardesc == null? "": user.finyeardesc.ToString ()),
                new Claim ("currency", user.currency == null? "": user.currency.ToString ()),
                new Claim ("email", user.email == null? "": user.email.ToString ()),
                new Claim ("usersource", user.usersource == null? "": user.usersource.ToString ()),
                new Claim ("email", user.email == null? "": user.email.ToString ()),
                new Claim ("language", user.language == null? "": user.language.ToString ()),
                new Claim ("defaultpage", user.defaultpage == null? "": user.defaultpage.ToString ()),
                new Claim ("countrycode", user.countrycode == null? "": user.countrycode.ToString ()),
                new Claim ("layoutpage", user.layoutpage == null? "": user.layoutpage.ToString ()),
                new Claim ("theme", user.theme == null? "": user.theme.ToString ()),
                new Claim ("logindate", DateTime.Now.ToString ()),
                new Claim ("status", user.status == null? "": user.status.ToString ()),
                new Claim ("firstname", user.firstname == null? "": user.firstname.ToString ()),
                new Claim ("lastname", user.lastname == null? "": user.lastname.ToString ()),
                new Claim ("nickname", user.nickname == null? "": user.nickname.ToString ()),
                new Claim ("geoid", user.geoid == null? "": user.geoid.ToString ()),
                new Claim ("customertype", user.customertype == null? "": user.customertype.ToString ()),
                new Claim ("profileimage", user.profileimage == null? "": user.profileimage.ToString ()),
                new Claim ("mobile", user.mobile == null? "": user.mobile.ToString ())




        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryVerySecretKey")); //_config["Jwt:Key"]
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("http://108.60.219.44:63939/",
                "http://108.60.219.44:63939/",
                claims,
                expires: DateTime.Now.AddMinutes(3000),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserCredential Authenticate(LoginModel login)
        {
            usermaster user = null;
            int cid = 0;
            string key = login.email;
            try
            {
                CustomercrCredential c = new CustomercrCredential();
                using (var connection = new NpgsqlConnection(Helper.Connectionstring))
                {
                    // CustomercrCredential c = new CustomercrCredential();
                    UserCredential u = new UserCredential();
                   // CustomercrCredential c = new CustomercrCredential();
                   // customermaster cm = new carditnow.Models.customermaster();
                    if (key == "sunsmart")
                    {
                        u.companyid = new Random().Next();
                        u.code = "superuser";
                        u.userid = -97567;
                        u.employeeid = new Random().Next();
                    }
                    else
                    {
                        bool buserfound = false;

                        /*
                        using (var command = _context.Database.GetDbConnection ().CreateCommand ()) {
                            command.CommandText = "select usercode from usermasters where usercode = '" + key + "' and emailpassword=crypt('" + login.Password + "',emailpassword )";
                            _context.Database.OpenConnection ();
                            using (var reader = command.ExecuteReader ()) {

                                if (reader.Read ()) {
                                    buserfound = true;
                                }
                            }
                        }
                        */
                        var parameters = new { @cid = cid, @key = key, @password = login.Password, tenant = login.host };
                        string SQL = "select pk_encode(userid) as pkcol,* from usermasters where email = @key  and emailpassword=crypt(@password,emailpassword)";
                        
                        var result = connection.Query<dynamic>(SQL, parameters);
                        if (result.Count()>0)
                        {

                            if (result.FirstOrDefault().userid > 0)
                                {
                                buserfound = true;
                                cid = 1;// result.FirstOrDefault().companyid;
                                }
                            var objusermaster = result.FirstOrDefault();

                            u.pkcol = objusermaster.pkcol;

                            u.companyid = 1;
                            u.userid = objusermaster.userid;
                            u.username = objusermaster.username;
                            u.userroleid = 6;
                            u.language = "en";
                            u.email = objusermaster.email;
                            u.status = objusermaster.status;
                            u.email = objusermaster.email;
                            u.status = objusermaster.status;
                            u.firstname = objusermaster.firstname;
                            u.lastname = objusermaster.lastname;
                            u.nickname = objusermaster.nickname;
                            //u.geoid = objusermaster.basegoid;
                            //u.customertype = objusermaster.type;
                            //u.profileimage = objusermaster.customerphoto;
                            //u.mobile = objusermaster.mobile;

                        }
                        else
                        {
                            var parameters_customermaster = new { @cid = cid, @key = key, @password = login.Password, tenant = login.host };
                            //string SQL_customer = "select pk_encode(customerid) as pkcol,* from customermasters where email = @key  and password=@password";

                            string SQL_customer = "  select pk_encode(m.customerid) as pkcol,m.*,d.geoid from customermasters m left join customerdetails d on d.customerid = m.customerid where m.email = @key  and m.password = @password";


                            var result_customer = connection.Query<dynamic>(SQL_customer, parameters_customermaster);
                            if (result_customer.FirstOrDefault().customerid > 0)
                            {
                                buserfound = true;
                                cid = 1;// result.FirstOrDefault().companyid;
                            }
                            var objusermaster = result_customer.FirstOrDefault();

                            u.pkcol = objusermaster.pkcol;
                            u.companyid = 1;
                            u.userid = objusermaster.customerid;
                            u.username = objusermaster.firstname;
                            u.userroleid = 6;
                            u.language = "en";
                            u.email = objusermaster.email;
                            u.status = objusermaster.status;
                            u.firstname = objusermaster.firstname;
                            u.lastname = objusermaster.lastname;
                            u.nickname = objusermaster.nickname;
                            u.geoid = objusermaster.geoid;
                            u.customertype = objusermaster.type;
                            u.profileimage = objusermaster.customerphoto;
                            u.mobile = objusermaster.mobile;


                            //c.customerid = objusermaster.customerid;
                            //c.uid = objusermaster.uid;
                            //c.type = objusermaster.type;
                            //c.mode = objusermaster.mode;
                            //c.firstname = objusermaster.firstname;
                            //c.lastname = objusermaster.lastname;
                            //c.email = objusermaster.email;

                            //return c;
                        }
                        if (!buserfound) throw new Exception("User not found");
                    }
                    return u;
                }
            }
            catch (Exception ex)
            {
                //return BadRequest(ex);
                string s = ex.ToString();
                return null;
            }
        }
    }
}

