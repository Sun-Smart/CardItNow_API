using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace carditnow.Services
{
    public interface IcustomerdetailService
    {

        dynamic Get_customerdetails();
        IEnumerable<Object> GetFullList();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_customerdetailid(int customerdetailid);
        dynamic Get_customerdetail(string sid);
        dynamic Get_customerdetail(int id);
        dynamic Save_customerdetail(string token, customerdetail obj_customerdetail);
        dynamic Delete(int id);
        customerdetail ProcessOCR(customerdetail model);
    }
}
