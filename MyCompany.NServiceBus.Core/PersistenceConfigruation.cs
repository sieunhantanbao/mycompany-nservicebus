using NServiceBus;
using NServiceBus.FluentConfiguration.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Core
{
    public class PersistenceConfigruation : IDefaultPersistenceConfiguration<SqlPersistence>
    {
        void IDefaultPersistenceConfiguration<SqlPersistence>.ConfigurePersistence(PersistenceExtensions<SqlPersistence> persistenceConfiguration)
        {
            persistenceConfiguration.TablePrefix("");
            var subscriptions = persistenceConfiguration.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        }
    }
}
