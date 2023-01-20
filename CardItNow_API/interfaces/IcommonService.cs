using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardItNow.Services
{
    public interface IcommonService
    {
        dynamic Getdocumenttype();
        dynamic GetBankList(); 
        dynamic GetPurposeList();

    }
}
