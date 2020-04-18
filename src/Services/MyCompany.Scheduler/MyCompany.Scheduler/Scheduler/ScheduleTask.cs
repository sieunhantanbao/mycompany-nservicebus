using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyCompany.Scheduler.Models;
using MyCompany.Scheduler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Scheduler
{
    public class ScheduleTask : ScheduledProcessor
    {
        private readonly IScheduleService _scheduleService;
        private readonly IOptions<ScheduleOption> _scheduleOption;
        public ScheduleTask(IServiceScopeFactory serviceScopeFactory,IScheduleService scheduleService, IOptions<ScheduleOption> scheduleOption)
            : base(serviceScopeFactory, scheduleOption)
        {
            _scheduleOption = scheduleOption;
            _scheduleService = scheduleService;
        }

        protected override string Schedule => _scheduleOption.Value.ScheduleCron;

        protected override async Task ProccessInScope(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Processing starts here...");
            await _scheduleService.ScheduleScan(_scheduleOption.Value.Frequency);
        }
    }
}
