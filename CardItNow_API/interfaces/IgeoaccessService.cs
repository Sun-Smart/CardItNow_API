using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IgeoaccessService
    {  

        dynamic Get_geoaccesses();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_geoaccessid(int geoaccessid);
dynamic Get_geoaccess(string sid);
         dynamic Get_geoaccess(int id);
        dynamic Save_geoaccess(string token,geoaccess obj_geoaccess);
        dynamic Delete(int id);
    }  
    }  
