using carditnow.Models;
using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace carditnow.Services
{
    public interface IbousermenuaccessService
    {

        dynamic Get_bousermenuaccesses();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_usermenuaccessid(int usermenuaccessid);
        dynamic Get_bousermenuaccess(string sid);
        dynamic Get_bousermenuaccess(int id);
        dynamic Save_bousermenuaccess(string token, bousermenuaccess obj_bousermenuaccess);
        dynamic Delete(int id);
    }
}
