using NServiceBus;
using NServiceBus.FluentConfiguration.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Core
{
    public class EnpointConfigruation : IDefaultEndpointConfiguration
    {
        void IDefaultEndpointConfiguration.ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("aduit");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.EnableOutbox();
        }
    }
}
