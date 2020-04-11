using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCompany.NServiceBus.Core;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace MyCompany.NServiceBus.Sample.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?? string.Empty;
            IEndpointInstance endpoint = null;
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.json", false, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var config = hostContext.Configuration;
                    services.AddOptions();
                    services.AddEntityFrameworkSqlServer();

                    endpoint = services.UseNServiceBus(config);
                });
            await builder.RunConsoleAsync();
            await endpoint.Stop().ConfigureAwait(false);
        }
    }
}
