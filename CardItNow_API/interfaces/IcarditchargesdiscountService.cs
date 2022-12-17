using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IcarditchargesdiscountService
    {  

        dynamic Get_carditchargesdiscounts();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_discountid(int discountid);
dynamic Get_carditchargesdiscount(string sid);
         dynamic Get_carditchargesdiscount(int id);
        dynamic Save_carditchargesdiscount(string token,carditchargesdiscount obj_carditchargesdiscount);
        dynamic Delete(int id);
    }  
    }  
