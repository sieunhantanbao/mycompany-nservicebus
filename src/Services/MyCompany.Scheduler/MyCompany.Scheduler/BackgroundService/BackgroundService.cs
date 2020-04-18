using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.Scheduler.BackgroundService
{
    public abstract class BackgroundService : IHostedService
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _cancelTokenSrc = new CancellationTokenSource();
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task which is executing
            _executingTask = ExecuteAsync(_cancelTokenSrc.Token);

            // If the task complete then return it
            // This will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise, it is running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop if without start
            if (_executingTask == null)
            {
                return;
            }
            try
            {
                // Send the cancellation signal to the execute method
                _cancelTokenSrc.Cancel();
            }
            finally
            {
                // Wait until the task is complete or the cancel token triggers
                await Task.WhenAll(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            do
            {
                await Proccess();
                await Task.Delay(5000, cancellationToken);
            }
            while (!cancellationToken.IsCancellationRequested);
        }

        protected abstract Task Proccess();
    }
}
