﻿using Dapper;
using LoggerService;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using Npgsql;
using nTireBO.Services;
//using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nTireBO.Models;
//using ConvertApiDotNet;

using System.Runtime.InteropServices;

//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;
//using LuceneServices;
//using NPOI.SS.UserModel;
/*
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
*/
/*
using NPOI.XSSF.UserModel;
using NPOI.XWPF.Extractor;
using NPOI.XWPF.UserModel;
*/
using SunSmartnTireProducts.Helpers;
using System.Security.Cryptography;
using carditnow.Models;

namespace SunSmartnTireProducts.Helpers
{
    
    public class UserInfo
    {
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
    public class UserAccess
    {
        public int[] user { get; set; }
        public int[] role { get; set; }
    }
    public class WorkFlowAction
    {
        public string menuid { get; set; }
        public int actionid { get; set; }
        public string[] ids { get; set; }
        public int formid { get; set; }
        public string actionname { get; set; }
        public dynamic SessionUser { get; set; }
        public string fkname { get; set; }
        public dynamic fk { get; set; }
        public string fkname1 { get; set; }
        public dynamic fk1 { get; set; }
        public string modulename { get; set; }



        public dynamic dialogdata { get; set; }

        //public UserCredential SessionUser { get; set; }

    }
    public class FileUploadResult
    {
        public long Length { get; set; }

        public string Name { get; set; }
    }
    public class serialparam
    {
       // dynamic objserialkeyparameter;
    }
    public class SuppliersForItem
    {
        public int companyid;
        public int? supplierid;
        public string suppliercode;
        public string suppliername;
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class FormProperty
    {
        public bool edit;
    }

    public class Attachment
    {

        public string Key { get; set; }

        public dynamic desc { get; set; }
        public string name { get; set; }
        public long size { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public string filekey { get; set; }
        public string username { get; set; }
        public string uploadeddate { get; set; }
    }
    public static class Helper
    {
        //Server=127.0.0.1;Port=5432;User Id=postgres;Password=Smart123$;Database=SmartOffice
        //31697
        static string pwd = "ThePasswordToDecryptAndEncryptTheFile".ToString();
        public static string Connectionstring = "Server=172.107.203.228;Port=5432;User Id=postgres;Password=Smart@123$;Database=CardItNow;";
        //public static ILuceneService luceneservice=new LuceneService();



        //  Call this function to remove the key from memory after use for security
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        /// <summary>
        /// Creates a random salt that will be used to encrypt your file. This method is required on FileEncrypt.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }

        
        public async static void AddWorkFlow(string token, int? cid, string MenuName, int? PK)
        {
            HttpClient client = new HttpClient();

            /* 
                        var menuresponse = (client.GetAsync("http://localhost:7002/ntireboapi/bomenu/name/" + MenuName)).Result;
                        var menujsonString = menuresponse.Content.ReadAsStringAsync();
                        var menuid = (JsonConvert.DeserializeObject<dynamic>(menujsonString.Result)).menu.menuid.Value;
            */

            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = (await client.PostAsync("http://localhost:7002/ntireboapi/boworkflow/addworkflow/" + cid + "/" + MenuName + "/" + PK, null));

        }
        public async static Task<ActionResult<usermaster>> GetUser(string token, int user)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = (client.GetAsync("http://localhost:7002/ntireboapi/usermaster/user/" + user)).Result;

            string jsonString = await response.Content.ReadAsStringAsync();

            //JsonConvert.DeserializeAnonymousType(jsonString.Result, objserialkey);

            // dynamic objtable = (JsonConvert.DeserializeObject<dynamic>(jsonString));     
            List<usermaster> tbl = (List<usermaster>)JsonConvert.DeserializeObject(jsonString, typeof(List<usermaster>));
            if (tbl.Count > 0)
            {
                usermaster objuser = tbl[0];
                return objuser;
            }
            return null;
        }
        public async static Task<ActionResult<IEnumerable<Object>>> SendEmail(string templateid, string token, int fromuser, int touser, string fromemailuser, string toemailuser, object item, ILoggerManager _logger = null)
        {
            return null;
        }
        public static bool Notify(string token, string outputsql, string actionrequestorfield, string actionassigneduserfield, string actionrequestoremailfield, string actionassigneduseremailfield, string notificationtext, Object obj, ILoggerManager _logger = null)
        {
            string strObjectType = obj.GetType().Name;
            IDictionary<string, object> data = null;
            if (strObjectType == "DapperRow") data = (IDictionary<string, object>)obj;

            int fromuser = 0;
            int touser = 0;

            string fromemailuser = "";
            string toemailuser = "";
            Object val = null;

            if (strObjectType != "DapperRow")
            {
                val = null;
                if (actionrequestorfield != null && actionrequestorfield != "") val = obj.GetType().GetProperty(actionrequestorfield).GetValue(obj, null);
                if (val != null && val.ToString() != "") fromuser = int.Parse(val.ToString());

                val = null;
                if (actionassigneduserfield != null && actionassigneduserfield != "") val = obj.GetType().GetProperty(actionassigneduserfield).GetValue(obj, null);
                if (val != null && val.ToString() != "") touser = int.Parse(val.ToString());

                val = null;
                if (actionrequestoremailfield != null && actionrequestoremailfield != "") val = obj.GetType().GetProperty(actionrequestoremailfield).GetValue(obj, null);
                if (val != null && val.ToString() != "") fromemailuser = val.ToString();

                val = null;
                if (actionassigneduseremailfield != null && actionassigneduseremailfield != "") val = obj.GetType().GetProperty(actionassigneduseremailfield).GetValue(obj, null);
                if (val != null && val.ToString() != "") toemailuser = val.ToString();

            }
            else
            {
                //                if (actionassigneduseremailfield != null && actionassigneduseremailfield != "") val = ((dynamic)obj)[actionassigneduseremailfield];

                val = null;
                if (actionrequestorfield != null && actionrequestorfield != "") val = data[actionassigneduseremailfield];
                if (val != null && val.ToString() != "") fromuser = int.Parse(val.ToString());

                val = null;
                if (actionassigneduserfield != null && actionassigneduserfield != "") val = data[actionassigneduserfield];
                if (val != null && val.ToString() != "") touser = int.Parse(val.ToString());

                val = null;
                if (actionrequestoremailfield != null && actionrequestoremailfield != "") val = data[actionrequestoremailfield];
                if (val != null && val.ToString() != "") fromemailuser = val.ToString();

                val = null;
                if (actionassigneduseremailfield != null && actionassigneduseremailfield != "") val = data[actionassigneduseremailfield];
                if (val != null && val.ToString() != "") toemailuser = val.ToString();
            }




            if (fromuser == 0) fromuser = 4;

            /*
            if (outputsql != null && outputsql.ToString () != "" && ((actionassigneduserfield != null && actionassigneduserfield != "") || (actionassigneduseremailfield != "" && actionassigneduseremailfield != "")) && notificationtext != null && notificationtext != "") {
                //((actionrequestorfield != null && actionrequestorfield != "") || (actionrequestoremailfield!=null && actionrequestoremailfield!= ""))
                NpgsqlConnection dbConn = new NpgsqlConnection (Connectionstring);

                dbConn.Open ();
                NpgsqlCommand cmd = new NpgsqlCommand (outputsql, dbConn);

                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader ();

                if (reader.Read ()) {
                    DataTable schemaTable = reader.GetSchemaTable ();

                    foreach (DataRow row in schemaTable.Rows) {
                        string colname = row["ColumnName"].ToString ();
                        if (row["ColumnName"].ToString () == actionrequestorfield && reader[colname] != null && reader[colname].ToString () != "") fromuser = int.Parse (reader[colname].ToString ());
                        if (row["ColumnName"].ToString () == actionassigneduserfield && reader[colname] != null && reader[colname].ToString () != "") touser = int.Parse (reader[colname].ToString ());
                        if (row["ColumnName"].ToString () == actionrequestoremailfield && reader[colname] != null && reader[colname].ToString () != "") fromemailuser = (reader[colname].ToString ());
                        if (row["ColumnName"].ToString () == actionassigneduseremailfield && reader[colname] != null && reader[colname].ToString () != "") toemailuser = (reader[colname].ToString ());

                        if (fromuser == 0) fromuser = 4;
                        if (fromemailuser == "") fromemailuser = "sunsmart";
                        notificationtext = notificationtext.Replace ("##" + colname.Replace (" ", "") + "##", reader[colname].ToString (), System.StringComparison.InvariantCultureIgnoreCase);

                    }

                }
                dbConn.Close ();
            }
            */
            if (notificationtext != "") Helper.SendEmail(notificationtext, token, fromuser, touser, fromemailuser, toemailuser, obj, _logger);
            return true;
        }
        public static bool Reminder(object item, DateTime? dt, TimeSpan? time, string templateid)

        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool Email(string data, string toemail, string toname, string fromname)
        {
            //return true;
            try
            {

                var message = new MimeMessage();

                message.To.Add(new MailboxAddress(toname, toemail));
                /*
                                message.From.Add(new MailboxAddress("E-mail From Name", "srimathi@sunsmartglobal.com"));

                                message.Subject = "Subject";
                                //We will say we are sending HTML. But there are options for plaintext etc. 
                                message.Body = new TextPart(TextFormat.Html)
                                {
                                Text = data
                                };

                                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                                using (var emailClient = new SmtpClient())
                                {
                                //The last parameter here is to use SSL (Which you should!)
                                emailClient.Connect("mail.sunsmartglobal.com", 587, false);

                                //Remove any OAuth functionality as we won't be using it. 
                                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                                emailClient.Authenticate("srimathi@sunsmartglobal.com", "sender@123$");
                                */
                message.From.Add(new MailboxAddress("SunSmart", "sunsmart.india@gmail.com"));

                message.Subject = "Notify from " + fromname;
                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = data
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp.gmail.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("sunsmart.india@gmail.com", "zvgwzuxonozppqsr");

                    emailClient.Send(message);

                    emailClient.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static bool UniqueValidation(object obj, string tablename, string pk)
        {

            return true;
        }
        //Task<IActionResult>
        public static bool Upload(dynamic files) //List<IFormFile> files
        {
            try
            {
                //var file = Request.Form.Files[0];

                var result = new List<FileUploadResult>();
                foreach (var file in files)
                {
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);

                    //var path = Path.Combine("c:\\inetpub\\wwwroot", file.FileName);

                    //var path = Path.Combine("E:\\SSnTireApp\\src\\assets\\images", file.FileName);
                    var folderName = System.IO.Path.Combine("Resources", "images1");
                    var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    path = System.IO.Path.Combine(path, file.FileName);
                    //var path = "E:\\SSnTireApp\\src\\assets\\images";
                    //
                    var stream = new FileStream(path, FileMode.Create);
                    file.CopyToAsync(stream);
                    result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
                }
                return true;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                //return Ok();
                throw ex;
                // return BadRequest();
            }
        }
        public static int Count(string sql, dynamic paramaters)
        {
            return 0;
        }
        public static int GetId(string key)
        {
            int ret = 0;
            try
            {
                using (NpgsqlConnection dbConn = new NpgsqlConnection(Connectionstring))
                {
                    

                    dbConn.Open();
                    string SQL = "select pk_decode('" + key + "')";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(SQL, dbConn))
                    {
                        
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = SQL;
                        NpgsqlDataReader reader = cmd.ExecuteReader();
                        System.Data.DataTable data = new System.Data.DataTable();
                        data.Load(reader);
                        ret = (data as dynamic).Rows[0][0];

                        //string outputval = "";
                        //if(outputval!=null)ret=int.Parse( outputval);
                        dbConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            return ret;
        }
        static public string pk_encode(int? toEncode)

        {

            string strEncode = toEncode.ToString() + ";" + DateTime.Now.ToString();
            byte[] toEncodeAsBytes

            = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncode);

            string returnValue

                = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }
        static public int? pk_decode(string encodedData)

        {
            if (encodedData != null && encodedData != "")
            {
                byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

                string strDecodedString = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

                int returnValue = int.Parse(strDecodedString.Split(';')[0]);

                return returnValue;
            }
            return 0;
        }

        private static string GetToken(Claim[] claims, int expireminutes)
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

        public async static Task<ActionResult<IEnumerable<Object>>> AfterExecute(string token, int querytype, object obj, string tablename, int? companyid, int? pk, string status = "", dynamic dialogdata = null, ILoggerManager _logger = null)
        {
            
            return null;
        }


       async public static void Upload(IFormFile file)
        {
            var folderName = System.IO.Path.Combine("Resources", "images1");
            var path = System.IO.Path.Combine("C:\\Users\\Lenovo\\source\\repos\\nTireSolution\\nTireBO", folderName);//Directory.GetCurrentDirectory()

            //
            string filepath = System.IO.Path.Combine(path, ((dynamic)file).Name);//file.FileName

            //path="";
            var stream = new FileStream(filepath, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
            int folderid = 0;
            

            
        }




        static string GetFileText(string name)
        {
            string fileContents = String.Empty;

            // If the file has been deleted since we took
            // the snapshot, ignore it and return the empty string.  
            if (System.IO.File.Exists(name))
            {
                fileContents = System.IO.File.ReadAllText(name);
            }
            return fileContents;
        }
 

        public static bool AddData(object item, string tablename, string module, bool bsave)
        {
            return true;
        }
        public static bool EditData(object item, string tablename, string module, bool bsave)
        {
            return true;
        }
        public static bool DeleteData(object item, string tablename, string module, bool bsave)
        {
            return true;
        }
        public static string GetSerialKeyQuery(string token, object obj, string tablename, string pk)
        {

            string sql = "";
            string pkvalue = "";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = (client.GetAsync("http://localhost:7002/ntireboapi/dynamic" + "/tablename/" + tablename)).Result;

            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (jsonString == null || jsonString == "" || jsonString == "[]")
            {
                
                return "";
                //throw new Exception("SerialKey Parameter is not configured");
            }
            //JsonConvert.DeserializeAnonymousType(jsonString.Result, objserialkey);
            //Object s=(JsonConvert.DeserializeObject<dynamic>(jsonString.Result));
            //var objserialkey = (JsonConvert.DeserializeObject<dynamic>(jsonString.Result)).objserialkeyparameter;

            List<dynamic> objserialkey1 = (List<dynamic>)JsonConvert.DeserializeObject(jsonString, typeof(List<dynamic>));

            dynamic objserialkey = objserialkey1[0];
            // var objserialkey1 = response.Result.Content.ReadAsAsync<object>();
            sql += "DO $$\r\n";
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (objserialkey.serialkeylogic.IndexOf(propertyInfo.Name) >= 0)
                {
                    sql += "DECLARE v_" + propertyInfo.Name + " character varying;\r\n";
                }
            }

            sql += "DECLARE v_DD character varying;\r\n";
            sql += "DECLARE v_MM character varying;\r\n";
            sql += "DECLARE v_MMM character varying;\r\n";
            sql += "DECLARE v_YY character varying;\r\n";
            sql += "DECLARE v_YYYY character varying;\r\n";
            sql += "DECLARE v_hh character varying;\r\n";
            sql += "DECLARE v_min character varying;\r\n";

            sql += "BEGIN\r\n";
            string value;
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.Name.Equals(pk)) pkvalue = propertyInfo.GetValue(obj).ToString();
                if (objserialkey.serialkeylogic.IndexOf(propertyInfo.Name) >= 0)
                {
                    if (propertyInfo.GetValue(obj) != null)
                    {
                        value = propertyInfo.GetValue(obj).ToString();
                        sql += "v_" + propertyInfo.Name + ":='" + value + "';\r\n";
                        if (propertyInfo.Name.Equals(pk))
                        {
                            pkvalue = value;
                        }
                    }
                    else
                    {
                        sql += "v_" + propertyInfo.Name + ":='';\r\n";
                    }
                }
            }
            sql += "v_DD:='" + DateTime.Now.ToString("dd") + "';\r\n";
            sql += "v_MM:='" + DateTime.Now.ToString("MM") + "';\r\n";
            sql += "v_MMM:='" + DateTime.Now.ToString("MMM") + "';\r\n";
            sql += "v_YY:='" + DateTime.Now.ToString("yy") + "';\r\n";
            sql += "v_YYYY:='" + DateTime.Now.ToString("yyyy") + "';\r\n";
            sql += "v_hh:='" + DateTime.Now.ToString("hh") + "';\r\n";
            sql += "v_min:='" + DateTime.Now.ToString("mm") + "';\r\n";
            sql += "update " + tablename + " set " + objserialkey.columnname + "=" + objserialkey.serialkeylogic + " where " + pk + "=" + pkvalue + ";\r\n";
            sql += "END $$;";
            //return new ObjectResult(sql);
            return sql;
        }
    }
    public class TableInParam
    {
        public int companyid;
        public int pk;
        public string tablename;
        public dynamic dialogdata { get; set; }
    }
    public class TableOutParam
    {
        public string outputparam;
        public string outputaction;
        public string gotopage;
        public string gotoid;
        public string outputsql;
    }
    
        public class WaterMark
    {
        public string filename;
    }
        public class OCRFile
    {
        public int folderid;
        public string filepath;
        public string filename;
        public string filekey;
        public string language;
    }
}