using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace nTireBO.Services
{
   public interface ItokenService
    {
        string GetToken(Claim[] claims, int expireminutes);
        string BuildToken(UserCredential user);
        UserCredential Authenticate(LoginModel login);


    }  
    }  
