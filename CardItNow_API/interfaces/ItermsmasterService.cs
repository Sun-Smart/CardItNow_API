using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ItermsmasterService
    {  

        dynamic Get_termsmasters();
        IEnumerable<Object> GetFullList();

        IEnumerable<Object> Get_NoAuthFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_termid(int termid);
dynamic Get_termsmaster(string sid);
         dynamic Get_termsmaster(int id);
        dynamic Save_termsmaster(string token,termsmaster obj_termsmaster);
        dynamic Delete(int id);
    }  
    }  
