using System.Threading.Channels;
using Contracts.Service.Core;

namespace Service.Core
{
    public class TaskQueueService : ITaskQueueService
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue;

        public TaskQueueService()
        {
            _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
        }

        public void EnqueueTask(Func<CancellationToken, Task> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _queue.Writer.TryWrite(task);
        }

        public async Task<Func<CancellationToken, Task>> DequeueTaskAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }

  
}