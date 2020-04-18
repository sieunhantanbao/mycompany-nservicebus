using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCompany.Scheduler.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Services
{
    public class ScheduleJob : IJob
    {
        private readonly ILogger<ScheduleJob> _logger;
        private readonly IOptions<ScheduleOption> _scheduleOption;
        internal static string COMMAND_KEY = "COMMAND";
        public ScheduleJob(ILogger<ScheduleJob> logger, IOptions<ScheduleOption> scheduleOption)
        {
            _logger = logger;
            _scheduleOption = scheduleOption;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var command = context.JobDetail.JobDataMap.Get(COMMAND_KEY);
            var urlGateway = _scheduleOption.Value.URLGatewayAPI;

            _logger.LogInformation("Executing to the Gateway with command: " + command);
            var httpClient = new HttpClient();
            var payLoad = $"{{ \"command\": \"{command}\", \"payload\": {{ }}}}";
            await httpClient.PostAsync(urlGateway, new StringContent(payLoad, Encoding.UTF8, "application/json"));
        }
    }
}
