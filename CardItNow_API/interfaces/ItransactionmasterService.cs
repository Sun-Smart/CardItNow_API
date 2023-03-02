using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ItransactionmasterService
    {  

        dynamic Get_transactionmasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_transactionid(int transactionid);
dynamic Get_transactionmaster(string sid);
         dynamic Get_transactionmaster(int id);
        dynamic Save_transactionmaster(string token,transactionmaster obj_transactionmaster);
        dynamic Delete(int id);

        dynamic Get_purpose();

        dynamic Get_payee();


        dynamic transactiondocument(string token, transactionmaster obj_transactionmaster);

        dynamic newjob();

      Task<dynamic> jobdoc(jobdoc model);

        dynamic billamountcalculation(string token, transactionmaster obj_transactionmaster);
    }  
    }  
