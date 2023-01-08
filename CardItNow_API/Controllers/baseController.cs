using carditnow.Models;
using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SunSmartnTireProducts.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace carditnow.Controllers
{
    public class baseController : ControllerBase
    {
        public ILoggerManager _logger;
        public baseController( ILoggerManager logger)
        {
            _logger = logger;
        }
        protected ObjectResult HandleError(Exception ex,string Method, string CustomeMessage="")
        {
            _logger.LogError($"Controller:Method:{Method}CustomeMessage:{CustomeMessage}Error:{ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error");
        }
    }
}