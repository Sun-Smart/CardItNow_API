using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace nTireBO.Services
{
   public interface IboconfigvalueService
    {  

        dynamic Get_boconfigvalues();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_configid(int configid);
        IEnumerable<Object> GetListBy_param(string param);
dynamic Get_boconfigvalue(string sid);
         dynamic Get_boconfigvalue(int id);
        dynamic Save_boconfigvalue(string token,boconfigvalue obj_boconfigvalue);
        dynamic Delete(int id);
    }  
    }  
