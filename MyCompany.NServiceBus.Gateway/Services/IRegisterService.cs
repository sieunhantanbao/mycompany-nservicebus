using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.Services
{
    public interface IRegisterService
    {
        Type FindContract(string command);
    }
}
