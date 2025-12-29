namespace Contracts.Service.Core
{
    public interface ITaskQueueService
    {
        void EnqueueTask(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> DequeueTaskAsync(CancellationToken cancellationToken);
    }
}