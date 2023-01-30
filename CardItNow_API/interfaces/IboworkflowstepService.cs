using SunSmartnTireProducts.Models;
using System;
using System.Collections.Generic;

namespace nTireBiz.Services
{
    public interface IboworkflowstepService
    {

        dynamic Get_boworkflowsteps();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_workflowstepid(int workflowstepid);
        dynamic Get_boworkflowstep(string sid);
        dynamic Get_boworkflowstep(int id);
        dynamic Save_boworkflowstep(string token, boworkflowstep obj_boworkflowstep);
        dynamic Delete(int id);
    }
}
