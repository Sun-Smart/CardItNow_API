using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;
using CardItNow.Models;

namespace carditnow.Services
{
    public interface IcustomerdocumentverificationService
    {
        string SendOTP(string email);

        dynamic Save_customerdocumentverification(string token, customerdocumentverification obj_customermaster);

        dynamic Get_customermasterdocument(string sid);

        dynamic Get_customermasterdocument(int id);

        dynamic Get_customermaster(int id);

        dynamic Get_customermaster1(int id);




    }
}
