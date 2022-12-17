using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ItransactiondetailService
    {  

        dynamic Get_transactiondetails();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_transactiondetailid(int transactiondetailid);
dynamic Get_transactiondetail(string sid);
         dynamic Get_transactiondetail(int id);
        dynamic Save_transactiondetail(string token,transactiondetail obj_transactiondetail);
        dynamic Delete(int id);
    }  
    }  
