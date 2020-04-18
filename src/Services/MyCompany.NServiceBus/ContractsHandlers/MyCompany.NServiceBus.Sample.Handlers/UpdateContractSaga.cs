using MyCompany.NServiceBus.Sample.Contracts;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.NServiceBus.Sample.Handlers
{
    public class UpdateContractSaga: SqlSaga<UpdateContractSagaData>,
        IAmStartedByMessages<StartUpdateContract>,
        IHandleMessages<ProccessUpdateContract>,
        IHandleMessages<CompleteUpdateContract>
    {
        public UpdateContractSaga()
        {

        }

        protected override string CorrelationPropertyName => nameof(UpdateContractSagaData.Id);

        public async Task Handle(StartUpdateContract message, IMessageHandlerContext context)
        {
            Console.WriteLine($"{message.ProcessId}--StartUpdateContract:::" + string.Join("=======", message.AccountNumbers));

            var processRequest = new ProccessUpdateContract(message.ProcessId)
            {
                AccountNumbers = message.AccountNumbers
            };

            await context.SendLocal(processRequest);
        }

        public async Task Handle(ProccessUpdateContract message, IMessageHandlerContext context)
        {
            Console.WriteLine($"{message.ProcessId}--ProccessUpdateContract:::" + string.Join("=====", message.AccountNumbers));

            var completeRequest = new CompleteUpdateContract(message.ProcessId)
            {
                AccountNumbers = message.AccountNumbers
            };

            await context.SendLocal(completeRequest);
        }

        public async Task Handle(CompleteUpdateContract message, IMessageHandlerContext context)
        {
            Console.WriteLine($"{message.ProcessId}--CompleteUpdateContract:::" + message.AccountNumbers);
            MarkAsComplete();
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartUpdateContract>(c => c.ProcessId);
            mapper.ConfigureMapping<ProccessUpdateContract>(c => c.ProcessId);
            mapper.ConfigureMapping<CompleteUpdateContract>(c => c.ProcessId);
        }
    }
}
