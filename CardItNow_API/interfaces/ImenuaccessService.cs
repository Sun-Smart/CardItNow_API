using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ImenuaccessService
    {  

        dynamic Get_menuaccesses();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_menuaccessid(int menuaccessid);
dynamic Get_menuaccess(string sid);
         dynamic Get_menuaccess(int id);
        dynamic Save_menuaccess(string token,menuaccess obj_menuaccess);
        dynamic Delete(int id);
    }  
    }  
