using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Core
{
    public class ServiceBusOptions
    {
        public string ContractsPath { get; set; }
        public string EndPointName { get; set; }
        public string ConnectionString { get; set; }
        public string FileBaseRouting { get; set; }
        public string LicensePath { get; set; }
        public bool IsGateway { get; set; }
    }
}
