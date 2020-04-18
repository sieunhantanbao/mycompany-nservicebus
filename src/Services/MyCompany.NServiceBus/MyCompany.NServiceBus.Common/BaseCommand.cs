using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Common
{
    public class BaseCommand : ICommand
    {
        public BaseCommand(Guid id)
        {
            ProcessId = id;
        }
        public Guid ProcessId { get; set; }
    }
}
