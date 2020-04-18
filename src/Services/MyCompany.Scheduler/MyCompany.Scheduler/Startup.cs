using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyCompany.Scheduler.Models;
using MyCompany.Scheduler.Scheduler;
using MyCompany.Scheduler.Services;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MyCompany.Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddLogging();

            services.AddSingleton<IHostedService, ScheduleTask>();

            services.AddOptions();
            services.Configure<ScheduleOption>(Configuration.GetSection("ScheduleOption"));

            QuartzSetup(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void QuartzSetup(IServiceCollection services)
        {
            services.AddTransient<IJob, ScheduleJob>();

            var jobs = Configuration.GetSection("ScheduleJobs").GetChildren();
            var items = new List<ScheduleItem>();

            foreach (var job in jobs)
            {
                items.Add(new ScheduleItem
                {
                    JobName = job.GetValue<string>("JobName"),
                    Command = job.GetValue<string>("Command"),
                    Cron = job.GetValue<string>("Cron"),
                    Enabled = job.GetValue<bool>("Enabled")
                });
            }

            var schedulerFactory = new StdSchedulerFactory();
            services.AddSingleton<ISchedulerFactory>(schedulerFactory);
            services.AddSingleton<IScheduleService>(new ScheduleService(items, schedulerFactory));

            var provider = services.BuildServiceProvider();
            IJobFactory jobFactory = new CustomJobFactory(provider);
            IScheduler scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.JobFactory = jobFactory;
        }
    }
}
