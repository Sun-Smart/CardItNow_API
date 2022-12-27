using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardItNow.interfaces
{
    public interface IverifyotpService
    {
        public string VerifyOTP(string email, string otp);
    }
}
