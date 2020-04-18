using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.NServiceBus.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ContractAttribute: Attribute
    {
        public ContractAttribute(string command)
        {
            Command = command;
        }
        public string Command { get; private set; }
    }
}
