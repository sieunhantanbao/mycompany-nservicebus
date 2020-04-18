using MyCompany.NServiceBus.Common;
using System;
using System.Collections.Generic;

namespace MyCompany.NServiceBus.Sample.Contracts
{
    [Contract("UpdateContractCommand")]
    public class UpdateContractCommand : BaseCommand
    {
        public string[] AccountNumbers { get; set; }

        public UpdateContractCommand(Guid processId) : base(processId)
        {
        }
    }
}
