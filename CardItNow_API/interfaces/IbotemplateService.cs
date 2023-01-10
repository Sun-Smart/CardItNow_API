using carditnow.Models;
using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace carditnow.Services
{
    public interface IbotemplateService
    {

        dynamic Get_botemplates();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_templateid(int templateid);
        IEnumerable<Object> GetListBy_templatecode(string templatecode);
        dynamic Get_botemplate(string sid);
        dynamic Get_botemplate(int id);
        dynamic Save_botemplate(string token, botemplate obj_botemplate);
        dynamic Delete(int id);
    }
}
