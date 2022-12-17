using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ImasterdatatypeService
    {  

        dynamic Get_masterdatatypes();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_datatypeid(int datatypeid);
dynamic Get_masterdatatype(string sid);
         dynamic Get_masterdatatype(int id);
        dynamic Save_masterdatatype(string token,masterdatatype obj_masterdatatype);
        dynamic Delete(int id);
    }  
    }  
