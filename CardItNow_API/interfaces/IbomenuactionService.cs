using carditnow.Models;
using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace carditnow.Services
{
    public interface IbomenuactionService
    {

        dynamic Get_bomenuactions();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_actionid(int actionid);
        dynamic Get_bomenuaction(string sid);
        dynamic Get_bomenuaction(int id);
        dynamic Save_bomenuaction(string token, bomenuaction obj_bomenuaction);
        dynamic Delete(int id);
    }
}
