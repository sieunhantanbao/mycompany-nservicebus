using MyCompany.NServiceBus.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Sample.Contracts
{
    public class ProccessUpdateContract : BaseCommand
    {
        public ProccessUpdateContract(Guid id) : base(id)
        { 
        }
        public string [] AccountNumbers { get; set; }
    }
}
