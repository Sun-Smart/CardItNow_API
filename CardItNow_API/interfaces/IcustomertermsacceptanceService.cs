using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
    public interface IcustomertermsacceptanceService
    {

        dynamic Get_customertermsacceptances();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_customertermid(int customertermid);
        dynamic Get_customertermsacceptance(string sid);
        dynamic Get_customertermsacceptance(int id);
        dynamic Save_customertermsacceptance(string token, customertermsacceptance obj_customertermsacceptance);
        dynamic Delete(int id);
        dynamic customeracceptancetermscondition(Customeracceptanceterms model);
    }
}
