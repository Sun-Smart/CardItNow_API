using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace carditnow.Services
{
    public interface IbouserbranchaccessService
    {

        dynamic Get_bouserbranchaccesses();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_accessid(int accessid);
        dynamic Get_bouserbranchaccess(string sid);
        dynamic Get_bouserbranchaccess(int id);
        dynamic Save_bouserbranchaccess(string token, bouserbranchaccess obj_bouserbranchaccess);
        dynamic Delete(int id);
    }
}
