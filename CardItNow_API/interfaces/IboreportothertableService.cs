using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace nTireBO.Services
{
   public interface IboreportothertableService
    {  

        dynamic Get_boreportothertables();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_othertableid(int othertableid);
dynamic Get_boreportothertable(string sid);
         dynamic Get_boreportothertable(int id);
        dynamic Save_boreportothertable(string token,boreportothertable obj_boreportothertable);
        dynamic Delete(int id);
    }  
    }  
