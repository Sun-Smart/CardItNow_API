using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;

namespace nTireBO.Services
{
   public interface IReportViewerService
    {

        dynamic RunReport(ReportParam param);
    }  
    }  
