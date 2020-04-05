using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.FileBasedRouting;
using NServiceBus.FluentConfiguration.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MyCompany.NServiceBus.Core
{
    public static class Extensions
    {
        public static IEndpointInstance UseNServiceBus(this IServiceCollection services, IConfiguration config)
        {
            var busOptions = new ServiceBusOptions();
            config.GetSection("NServiceBusOption").Bind(busOptions);

            services.Configure<ServiceBusOptions>(config.GetSection("NServiceBusOption"));

            var configureAnEnpoint = new ConfigureNServiceBus()
                .WithEndpoint<EnpointConfigruation>(busOptions.EndPointName)
                .WithTransport<SqlServerTransport, SqlTransportConfigruation>
                (
                    transport =>
                    {
                        transport.ConnectionString(busOptions.ConnectionString);
                    }
                )
                .WithRouting
                (
                    routing =>
                    {
                        if (!string.IsNullOrWhiteSpace(busOptions.FileBaseRouting))
                        {
                            routing.UseFileBasedRouting(busOptions.FileBaseRouting);
                        }
                    }
                )
                .WithPersistence<SqlPersistence, PersistenceConfigruation>
                (
                    persistence =>
                    {
                        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                        persistence.ConnectionBuilder
                            (
                                connectionBuilder: () =>
                                {
                                    return new SqlConnection(busOptions.ConnectionString);
                                }
                            );
                    }
                );

            if (!string.IsNullOrEmpty(busOptions.LicensePath))
            {
                configureAnEnpoint.Configuration.LicensePath(busOptions.LicensePath);
            }

            if (busOptions.IsGateway)
            {
                configureAnEnpoint.Configuration.SendOnly();
            }

            SqlHelper.EnsureDatabaseExist(busOptions.ConnectionString);

            configureAnEnpoint.Configuration.UseContainer<ServicesBuilder>
                (
                    customizations =>
                    {
                        customizations.ExistingServices(services);
                    }
                );

            var start = configureAnEnpoint.ManageEndpoint().Start();
            services.AddSingleton<IEndpointInstance>(start.Instance);

            return start.Instance;
        }
    }
}
