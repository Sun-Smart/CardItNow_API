using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace nTireBiz.Services
{
    public interface IbodynamicformService
    {

        dynamic Get_bodynamicforms();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_formid(int formid);
        IEnumerable<Object> GetListBy_tableiddesc(string tableiddesc);
        dynamic Get_bodynamicform(string sid);
        dynamic Get_bodynamicform(int id);
        dynamic Save_bodynamicform(string token, bodynamicform obj_bodynamicform);
        dynamic Delete(int id);
    }
}
