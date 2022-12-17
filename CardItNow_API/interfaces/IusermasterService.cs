using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IusermasterService
    {  

        dynamic Get_usermasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_userid(int userid);
dynamic Get_usermaster(string sid);
         dynamic Get_usermaster(int id);
        dynamic Save_usermaster(string token,usermaster obj_usermaster);
        dynamic Delete(int id);
    }  
    }  
