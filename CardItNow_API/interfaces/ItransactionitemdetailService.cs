using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface ItransactionitemdetailService
    {  

        dynamic Get_transactionitemdetails();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_transactionitemdetailid(int transactionitemdetailid);
dynamic Get_transactionitemdetail(string sid);
         dynamic Get_transactionitemdetail(int id);
        dynamic Save_transactionitemdetail(string token,transactionitemdetail obj_transactionitemdetail);
        dynamic Delete(int id);
    }  
    }  
