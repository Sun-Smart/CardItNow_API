using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace nTireBO.Services
{
   public interface IbodashboardService
    {  

        dynamic Get_bodashboards();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_dashboardid(int dashboardid);
dynamic Get_bodashboard(string sid);
         dynamic Get_bodashboard(int id);
        dynamic Save_bodashboard(string token,bodashboard obj_bodashboard);
        dynamic Delete(int id);
    }  
    }  
