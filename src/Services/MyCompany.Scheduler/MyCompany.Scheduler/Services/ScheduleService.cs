using MyCompany.Scheduler.Models;
using NCrontab;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IList<ScheduleItem> _scheduleItems;
        private readonly ISchedulerFactory _schedulerFactory;
        public ScheduleService(IList<ScheduleItem> scheduleItems, ISchedulerFactory schedulerFactory)
        {
            _scheduleItems = scheduleItems;
            _schedulerFactory = schedulerFactory;
        }
        public async Task ScheduleScan(int frenquency)
        {
            var now = DateTime.Now;
            var nextRun = now + TimeSpan.FromMinutes(frenquency);
            var scheduler = _schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            if (scheduler.IsStarted)
            {
                await scheduler.Clear();
            }

            foreach (var scheduleItem in _scheduleItems)
            {
                if (!scheduleItem.Enabled)
                {
                    continue;
                }
                var schedule = CrontabSchedule.Parse(scheduleItem.Cron);
                var timeToRun = schedule.GetNextOccurrence(now);
                if(timeToRun >= now && timeToRun <= nextRun)
                {
                    try
                    {
                        // Get the job
                        DateTimeOffset runTime = DateBuilder.EvenMinuteDate(timeToRun);
                        IJobDetail job = JobBuilder.Create<ScheduleJob>()
                                        .WithIdentity(scheduleItem.JobName, "group")
                                        .Build();
                        job.JobDataMap.Put(ScheduleJob.COMMAND_KEY, scheduleItem.Command);

                        // Set trigger for the job
                        var trigger = TriggerBuilder
                                    .Create()
                                    .WithIdentity(scheduleItem.JobName + "_trigger", "group")
                                    .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute))
                                    .Build();
                        // Schedule the job with trigger
                        await scheduler.ScheduleJob(job, trigger);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            await scheduler.Start();
        }
    }
}
