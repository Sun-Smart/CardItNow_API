using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IcustomersecurityquestionService
    {  

        dynamic Get_customersecurityquestions();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_securityquestionid(int securityquestionid);
dynamic Get_customersecurityquestion(string sid);
         dynamic Get_customersecurityquestion(int id);
        dynamic Save_customersecurityquestion(string token,customersecurityquestion obj_customersecurityquestion);
        dynamic Delete(int id);
    }  
    }  
