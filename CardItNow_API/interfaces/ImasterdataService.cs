using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ImasterdataService
    {  

        dynamic Get_masterdatas();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_masterdataid(int masterdataid);
dynamic Get_masterdata(string sid);
         dynamic Get_masterdata(int id);
        dynamic Save_masterdata(string token,masterdata obj_masterdata);
        dynamic Delete(int id);
    }  
    }  
