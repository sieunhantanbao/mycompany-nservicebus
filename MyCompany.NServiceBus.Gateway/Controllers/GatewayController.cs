using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Gateway.Helpers;
using NServiceBus.Gateway.Models;
using NServiceBus.Gateway.Services;

namespace NServiceBus.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;
        private readonly IRegisterService _registerService;
        private readonly IEndpointInstance _endpointInstance;
        public GatewayController(IRegisterService registerService,
                                 IEndpointInstance endpointInstance,
                                 ILogger<GatewayController> logger)
        {
            _logger = logger;
            _registerService = registerService;
            _endpointInstance = endpointInstance?? throw new ArgumentNullException(nameof(_endpointInstance));
        }

        [HttpPost("bus-queue")]
        public async Task<IActionResult> BusQueue([FromBody] EventReg value)
        {

            return await PostNserviceBusCommand(value, "BusQueue");
        }

        private async Task<IActionResult> PostNserviceBusCommand(EventReg value, string operation)
        {
            if(value == null)
            {
                _logger.LogInformation("The request body is missing");
                return new BadRequestResult();
            }
            var result = Ok();
            try
            {
                _logger.LogInformation($"The {operation} start");

                // Find contract and create object instance
                var type = _registerService.FindContract(value.Command);
                var obj = Activator.CreateInstance(type, Guid.NewGuid());

                var test = JsonConvert.SerializeObject(value.Payload);

                //Map PayLoad to Object
                value.Payload.To(obj);

                // Send to bus
                await _endpointInstance.Send(obj);

                _logger.LogInformation($"The {operation} completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return result;
        }
    }
}
