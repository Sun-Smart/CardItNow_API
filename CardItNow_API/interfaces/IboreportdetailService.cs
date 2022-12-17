using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace nTireBO.Services
{
   public interface IboreportdetailService
    {  

        dynamic Get_boreportdetails();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_reportdetailid(int reportdetailid);
dynamic Get_boreportdetail(string sid);
         dynamic Get_boreportdetail(int id);
        dynamic Save_boreportdetail(string token,boreportdetail obj_boreportdetail);
        dynamic Delete(int id);
    }  
    }  
