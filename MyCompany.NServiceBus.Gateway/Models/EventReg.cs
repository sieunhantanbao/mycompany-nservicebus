using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.Models
{
    public class EventReg
    {
        public EventReg()
        {
            Payload = new ExpandoObject();
        }
        public string Command { get; set; }
        public ExpandoObject Payload { get; set; }
    }
}
