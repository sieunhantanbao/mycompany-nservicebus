using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.BackgroundService
{
    public abstract class ScopedProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ScopedProcessor(IServiceScopeFactory serviceScopeFactory): base()
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task Proccess()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProccessInScope(scope.ServiceProvider);
            }
        }
        protected abstract Task ProccessInScope(IServiceProvider serviceProvider);
    }
}
