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

        dynamic Save_customersecuritymultiquestions(string token, dynamic data);
        dynamic Delete(int id);

        IEnumerable<Object> GetsecurityQuestions();

        dynamic Get_customersecurityquestiondetail(int customerid);

        dynamic securityquestioncheck(string token, customersecurityquestion obj_customersecurityquestion);

         dynamic Save_customerallsecuritymultiquestions(dynamic data);

        dynamic securityquestionscheck(dynamic data);
    }  
    }  
