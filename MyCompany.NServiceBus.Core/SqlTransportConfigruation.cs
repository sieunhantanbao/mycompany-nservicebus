using NServiceBus;
using NServiceBus.FluentConfiguration.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Core
{
    public class SqlTransportConfigruation : IDefaultTransportConfiguration<SqlServerTransport>
    {
        void IDefaultTransportConfiguration<SqlServerTransport>.ConfigureTransport(TransportExtensions<SqlServerTransport> transport)
        {
            transport.UseSchemaForQueue("error", "dbo");
            transport.UseSchemaForQueue("audit", "dbo");
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        }
    }
}
