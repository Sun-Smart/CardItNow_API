using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IcustomermasterService
    {  

        dynamic Get_customermasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_customerid(int customerid);
        string SendOTP(string email);
dynamic Get_customermaster(string sid);
         dynamic Get_customermaster(int id);
        dynamic Save_customermaster(string token,customermaster obj_customermaster);
        dynamic Delete(int id);
    }  
    }  
