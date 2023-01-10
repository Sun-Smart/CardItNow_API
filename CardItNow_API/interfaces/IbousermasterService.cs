using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace carditnow.Services
{
    public interface IbousermasterService
    {

        dynamic Get_bousermasters();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_userid(int userid);
        IEnumerable<Object> GetListBy_emailid(string emailid);
        IEnumerable<Object> GetListBy_sourcereference(int sourcereference);
        dynamic Get_bousermaster(string sid);
        dynamic Get_bousermaster(int id);
        dynamic Save_bousermaster(string token, bousermastercontext obj_bousermaster);
        dynamic Delete(int id);
    }
}
