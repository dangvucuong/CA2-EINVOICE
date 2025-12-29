using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Service.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.HostedService
{
    public class BackgroundTaskService : BackgroundService
    {
        private readonly ITaskQueueService _taskQueue;
        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundTaskService(ITaskQueueService taskQueue, IServiceScopeFactory scopeFactory)
        {
            _taskQueue = taskQueue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await _taskQueue.DequeueTaskAsync(stoppingToken);
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        await task(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi xử lý tác vụ: {ex.Message}");
                }
            }
        }
    }
}