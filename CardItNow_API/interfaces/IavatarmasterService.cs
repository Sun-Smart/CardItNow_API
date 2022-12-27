using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IavatarmasterService
    {  

        dynamic Get_avatarmasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_avatarid(int avatarid);
        dynamic Get_avatarmaster(string sid);
         dynamic Get_avatarmaster(int id);
        dynamic Save_avatarmaster(string token,avatarmaster obj_avatarmaster);
        dynamic Delete(int id);
    }  
    }  
