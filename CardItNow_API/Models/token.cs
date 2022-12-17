using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;

namespace nTireBO.Models
{

    public class UserCredential
    {
        [Key]
        [Column(TypeName = "int")]
        public int? companyid { get; set; }

        [Column(TypeName = "string")]
        public string pkcol { get; set; }

        [Column(TypeName = "int")]
        public int? userid { get; set; }

        [Column(TypeName = "int")]
        public int? employeeid { get; set; }

        [Column(TypeName = "string")]
        public string code { get; set; }

        [Column(TypeName = "string")]
        public string currency { get; set; }

        [Column(TypeName = "string")]
        public string username { get; set; }

        [Column(TypeName = "int")]
        public int? branchid { get; set; }

        [Column(TypeName = "string")]
        public string branchiddesc { get; set; }

        [Column(TypeName = "string")]
        public string language { get; set; }

        [Column(TypeName = "string")]
        public string email { get; set; }
        [Column(TypeName = "string")]
        public string usersource { get; set; }

        [Column(TypeName = "string")]
        public string countrycode { get; set; }

        [Column(TypeName = "string")]
        public string defaultpage { get; set; }

        [Column(TypeName = "string")]
        public string theme { get; set; }

        [Column(TypeName = "string")]
        public string layoutpage { get; set; }

        [Column(TypeName = "int")]
        public int? userroleid { get; set; }
        [Column(TypeName = "int")]
        public int? role { get; set; }

        [Column(TypeName = "int")]
        public int? finyearid { get; set; }

        [Column(TypeName = "string")]
        public string finyeardesc { get; set; }
    }

    public class LoginModel
    {
        public string email { get; set; }
        public string Password { get; set; }
        public string host { get; set; }
    }
    public class ForgotPassword
    {
        public string email { get; set; }
    }

    public class ResetPassword
    {
        public string password { get; set; }
        public string sptoken { get; set; }
    }
}

