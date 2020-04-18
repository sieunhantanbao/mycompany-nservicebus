using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Common
{
    public interface IModuleInitilizer
    {
        void Initialize(IServiceCollection collection);
    }
}
