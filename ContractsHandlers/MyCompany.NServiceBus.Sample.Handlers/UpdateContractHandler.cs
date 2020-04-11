using MyCompany.NServiceBus.Sample.Contracts;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.NServiceBus.Sample.Handlers
{
    public class UpdateContractHandler : IHandleMessages<UpdateContractCommand>
    {
        public Task Handle(UpdateContractCommand message, IMessageHandlerContext context)
        {
            var proccessId = Guid.NewGuid();
            var startRequest = new StartUpdateContract(proccessId)
            {
                AccountNumbers = message.AccountNumbers
            };

            return context.SendLocal(startRequest);
        }
    }
}
