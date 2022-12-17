using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace nTireBO.Services
{
   public interface IboreportcolumnService
    {  

        dynamic Get_boreportcolumns();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string  key);
        IEnumerable<Object> GetListBy_reportcolumnid(int reportcolumnid);
dynamic Get_boreportcolumn(string sid);
         dynamic Get_boreportcolumn(int id);
        dynamic Save_boreportcolumn(string token,boreportcolumn obj_boreportcolumn);
        dynamic Delete(int id);
    }  
    }  
