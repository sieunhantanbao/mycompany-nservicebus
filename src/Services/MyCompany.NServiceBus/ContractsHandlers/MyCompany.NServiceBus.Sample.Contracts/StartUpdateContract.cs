using MyCompany.NServiceBus.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Sample.Contracts
{
    public class StartUpdateContract : BaseCommand
    {
        public string[] AccountNumbers { get; set; }
        public StartUpdateContract(Guid id) : base(id)
        {
        }
    }
}
