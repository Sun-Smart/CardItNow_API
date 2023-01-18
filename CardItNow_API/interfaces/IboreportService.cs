using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;
using carditnow.Models;

namespace nTireBO.Services
{
    public interface IboreportService
    {

        dynamic Get_boreports();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_reportid(int reportid);
        IEnumerable<Object> GetListBy_reportcode(string reportcode);
        dynamic Get_boreport(string sid);
        dynamic Get_boreport(int id);
        dynamic Save_boreport(string token, boreport obj_boreport);
        dynamic Delete(int id);
        //object Save_boreport(string token, boreport data);
    }
}
