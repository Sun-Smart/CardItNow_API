using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IgeographymasterService
    {  

        dynamic Get_geographymasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_geoid(int geoid);
dynamic Get_geographymaster(string sid);
         dynamic Get_geographymaster(int id);
        dynamic Save_geographymaster(string token,geographymaster obj_geographymaster);
        dynamic Delete(int id);
    }  
    }  
