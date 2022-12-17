using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IrecipientdiscountService
    {  

        dynamic Get_recipientdiscounts();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_discountid(int discountid);
dynamic Get_recipientdiscount(string sid);
         dynamic Get_recipientdiscount(int id);
        dynamic Save_recipientdiscount(string token,recipientdiscount obj_recipientdiscount);
        dynamic Delete(int id);
    }  
    }  
