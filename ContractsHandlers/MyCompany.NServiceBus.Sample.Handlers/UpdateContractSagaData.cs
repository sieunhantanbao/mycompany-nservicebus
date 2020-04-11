using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Sample.Handlers
{
    public class UpdateContractSagaData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
        public List<string> AccountNumbers { get; set; }
    }
}
