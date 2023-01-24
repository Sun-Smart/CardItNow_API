using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IcustomerpaymodeService
    {  

        dynamic Get_customerpaymodes();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_payid(int payid);
        dynamic Get_customerpaymode(string sid);
        dynamic Get_customerCard(int id); //Get Cutomer Card list code
         dynamic Get_customerpaymode(int id);
        dynamic Save_customerpaymode(string token,customerpaymode obj_customerpaymode);
        dynamic SaveCutomerCardDeatils(customerpaymode obj_customerpaymode);
        dynamic Delete(int id);
    }  
    }  
