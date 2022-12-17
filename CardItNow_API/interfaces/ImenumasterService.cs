using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ImenumasterService
    {
        IEnumerable<Object> GetListBy_menuurl(string menuurl);
        IEnumerable<Object> Get_usermenumaster(dynamic param);
        dynamic Get_menumasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_menuid(int menuid);
dynamic Get_menumaster(string sid);
         dynamic Get_menumaster(int id);
        dynamic Save_menumaster(string token,menumaster obj_menumaster);
        dynamic Delete(int id);
    }  
    }  
