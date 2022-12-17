using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IcustomerrecipientlinkService
    {  

        dynamic Get_customerrecipientlinks();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_linkid(int linkid);
dynamic Get_customerrecipientlink(string sid);
         dynamic Get_customerrecipientlink(int id);
        dynamic Save_customerrecipientlink(string token,customerrecipientlink obj_customerrecipientlink);
        dynamic Delete(int id);
    }  
    }  
