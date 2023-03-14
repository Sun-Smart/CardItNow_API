using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
    public interface IcitymasterService
    {

        dynamic Get_citymasters();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_cityid(int cityid);
        IEnumerable<Object> GetListBy_geoid(int geoid);

        IEnumerable<Object> GetListBy_geoid2(int geoid, int provienceid);
        dynamic Get_citymaster(string sid);
        dynamic Get_citymaster(int id);
        dynamic Save_citymaster(string token, citymaster obj_citymaster);
        dynamic Delete(int id);
    }
}
