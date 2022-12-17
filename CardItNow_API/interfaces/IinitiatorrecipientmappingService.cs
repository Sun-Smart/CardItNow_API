using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IinitiatorrecipientmappingService
    {  

        dynamic Get_initiatorrecipientmappings();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_mappingid(int mappingid);
dynamic Get_initiatorrecipientmapping(string sid);
         dynamic Get_initiatorrecipientmapping(int id);
        dynamic Save_initiatorrecipientmapping(string token,initiatorrecipientmapping obj_initiatorrecipientmapping);
        dynamic Delete(int id);
    }  
    }  
