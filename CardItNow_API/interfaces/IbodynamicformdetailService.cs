using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace nTireBiz.Services
{
    public interface IbodynamicformdetailService
    {

        dynamic Get_bodynamicformdetails();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_formdetailid(int formdetailid);
        dynamic Get_bodynamicformdetail(string sid);
        dynamic Get_bodynamicformdetail(int id);
        dynamic Save_bodynamicformdetail(string token, bodynamicformdetail obj_bodynamicformdetail);
        dynamic Delete(int id);
    }
}
