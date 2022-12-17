using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IcustomersecurityquestionshistoryService
    {  

        dynamic Get_customersecurityquestionshistories();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_historyid(int historyid);
dynamic Get_customersecurityquestionshistory(string sid);
         dynamic Get_customersecurityquestionshistory(int id);
        dynamic Save_customersecurityquestionshistory(string token,customersecurityquestionshistory obj_customersecurityquestionshistory);
        dynamic Delete(int id);
    }  
    }  
