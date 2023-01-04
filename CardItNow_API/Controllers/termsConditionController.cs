using carditnow.Services;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nTireBO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Controllers
{
    [Route("carditnowapi/[controller]")]
    [ApiController]
    public class termsConditionController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IboconfigvalueService _boconfigvalueService;
        private readonly IcustomertermsacceptanceService _customertermsacceptanceService;
        private readonly ItermsmasterService _termsmasterService;

        public termsConditionController(IHttpContextAccessor objhttpContextAccessor, ItermsmasterService obj_termsmasterService, ILoggerManager logger)
        {
            _termsmasterService = obj_termsmasterService;
            _logger = logger;

        }

        [HttpGet]
        [Route("TermsList")]
        public async Task<ActionResult<IEnumerable<Object>>> Get_NoAuthFullList()
        {
            try
            {
                var result = _termsmasterService.Get_NoAuthFullList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Controller: GetFullList() \r\n{ex}");
                return StatusCode(StatusCodes.Status417ExpectationFailed, "GetList " + ex.Message + "  " + ex.InnerException?.Message);
            }
        }

    }
}

