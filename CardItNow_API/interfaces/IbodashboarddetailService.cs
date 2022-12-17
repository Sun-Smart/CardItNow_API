using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace nTireBO.Services
{
   public interface IbodashboarddetailService
    {  

        dynamic Get_bodashboarddetails();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_dashboarddetailid(int dashboarddetailid);
dynamic Get_bodashboarddetail(string sid);
         dynamic Get_bodashboarddetail(int id);
        dynamic Save_bodashboarddetail(string token,bodashboarddetail obj_bodashboarddetail);
        dynamic Delete(int id);
    }  
    }  
