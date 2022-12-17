using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IuserrolemasterService
    {  

        dynamic Get_userrolemasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_roleid(int roleid);
dynamic Get_userrolemaster(string sid);
         dynamic Get_userrolemaster(int id);
        dynamic Save_userrolemaster(string token,userrolemaster obj_userrolemaster);
        dynamic Delete(int id);
    }  
    }  
