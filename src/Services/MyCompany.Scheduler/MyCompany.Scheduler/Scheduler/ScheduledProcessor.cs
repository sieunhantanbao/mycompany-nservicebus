using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyCompany.Scheduler.BackgroundService;
using MyCompany.Scheduler.Models;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Scheduler
{
    public abstract class ScheduledProcessor : ScopedProcessor
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        protected abstract string Schedule { get; }
        public ScheduledProcessor(IServiceScopeFactory serviceScopeFactory,
            IOptions<ScheduleOption> scheduleOption
            ): base(serviceScopeFactory)
        {
            _schedule = CrontabSchedule.Parse(scheduleOption.Value.ScheduleCron);
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            do
            {
                var now = DateTime.Now;
                if(now > _nextRun)
                {
                    await Proccess();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, cancellationToken); // Delay 5s
            }
            while (!cancellationToken.IsCancellationRequested);
        }

    }
}
