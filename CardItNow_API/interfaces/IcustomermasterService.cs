using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
   public interface IcustomermasterService
    {  

        dynamic Get_customermasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_customerid(int customerid);
        string SendOTP(string email);
        string ProcessDocument(processdocument model);
        public string UpdateProfileInformation(string email, string firstname, string lastname, string mobile, DateTime dateofbirth, string address, int geoid, int cityid, string postalcode, DateTime idissuedate, DateTime idexpirydate);        
        string PasswordSet(string email,string password);
        string SetPinConfig(string email, string pin);
        dynamic Get_customermaster(string sid);
         dynamic Get_customermaster(int id);
        dynamic Save_customermaster(string token,customermaster obj_customermaster);
        dynamic Delete(int id);
    }  
    }  
