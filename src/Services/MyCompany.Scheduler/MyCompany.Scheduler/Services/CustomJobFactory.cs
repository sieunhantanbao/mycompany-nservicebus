using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.Services
{
    public class CustomJobFactory : IJobFactory
    {
        private readonly IServiceProvider _provider;

        public CustomJobFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _provider.GetService(typeof(IJob));
            return (IJob)job;
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
