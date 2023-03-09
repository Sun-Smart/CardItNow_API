using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IinitiatorrecipientprivateService
    {  

        dynamic Get_initiatorrecipientprivates();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_privateid(int privateid);
dynamic Get_initiatorrecipientprivate(string sid);
         dynamic Get_initiatorrecipientprivate(int id);
        dynamic Save_initiatorrecipientprivate(string token,initiatorrecipientprivate obj_initiatorrecipientprivate);

        dynamic Save_initiatorrecipientprivate1(string token, initiatorrecipientprivate obj_initiatorrecipientprivate);
        dynamic Delete(int id);
    }  
    }  
