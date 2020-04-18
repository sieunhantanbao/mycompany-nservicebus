using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyCompany.NServiceBus.Core;
using NServiceBus.Gateway.Services;

namespace NServiceBus.Gateway
{
    public class Startup
    {
        private IEndpointInstance _endpointInstance = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var obsoluteContractPath = Configuration.GetValue<string>("NServiceBusOption:ContractsPath");
            if (string.IsNullOrEmpty(obsoluteContractPath))
                throw new ArgumentNullException("NServiceBusOption:ContractsPath must have value!");

            services.AddControllers();

            //Register Services
            RegisterServices(services);
            // RegisterMediatorR(services);

            _endpointInstance = services.UseNServiceBus(Configuration);

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NServiceBus API", Version = "v1" });
            });
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

            // Use Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NServiceBus Gateway V1 Docs");

            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            var obsoluteContractPath = Configuration.GetValue<string>("NServiceBusOption:ContractsPath");
            if (string.IsNullOrEmpty(obsoluteContractPath))
                throw new ArgumentNullException("NServiceBusOption:ContractsPath must have value!");
            services.AddSingleton(typeof(IRegisterService), new RegisterService(obsoluteContractPath));
        }
        //private void RegisterMediatorR(IServiceCollection services)
        //{
        //    services.AddScoped<ServiceFactory>(p => p.GetService);

        //    var configuration = new ContainerConfiguration().WithAssembly(typeof(Bootstraper).GetTypeInfo().Assembly);
        //    using (var container = configuration.CreateContainer())
        //    {
        //        OperationInitialier = container.GetExport<IModuleInitilizer>();
        //        OperationInitialier.Initialize(services);
        //    }

        //    var provider = services.BuildServiceProvider();
        //    var mediator = provider.GetRequiredService(typeof(IMediator));
        //}
    }
}
