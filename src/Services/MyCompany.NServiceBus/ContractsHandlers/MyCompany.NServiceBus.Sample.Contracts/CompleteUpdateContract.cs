using MyCompany.NServiceBus.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Sample.Contracts
{
    public class CompleteUpdateContract: BaseCommand
    {
        public string[] AccountNumbers { get; set; }
        public CompleteUpdateContract(Guid id) : base(id)
        {
        }
    }
}
