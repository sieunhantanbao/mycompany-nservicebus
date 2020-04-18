using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NServiceBus.Common;

namespace NServiceBus.Gateway.OperationHandlers
{
    [Export(typeof(IModuleInitilizer))]
    public class Bootstraper : IModuleInitilizer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddMediatR(typeof(Bootstraper).GetTypeInfo().Assembly);
        }
    }
}
