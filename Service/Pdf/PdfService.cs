using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Service.Pdf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Polly;
using PuppeteerSharp;

namespace Service.Pdf
{
    public class PdfService : IPdfService, IDisposable
    {
        private readonly DefaultObjectPool<IBrowser> _browserPool;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(6); // Giá trị thử nghiệm, điều chỉnh dựa trên tài nguyên server
        private readonly ILogger<PdfService> _logger;

        public PdfService(ILogger<PdfService> logger)
        {
            _logger = logger;
            var policy = new BrowserPoolPolicy();
            _browserPool = new DefaultObjectPool<IBrowser>(policy, 3); // Pool tối đa 5 browser instances
        }

        private class BrowserPoolPolicy : IPooledObjectPolicy<IBrowser>
        {
            public IBrowser Create()
            {
                var browserFetcher = new BrowserFetcher();
                browserFetcher.DownloadAsync().GetAwaiter().GetResult();
                return Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    Timeout = 0
                }).GetAwaiter().GetResult();
            }

            public bool Return(IBrowser browser)
            {
                return browser != null && !browser.IsClosed;
            }
        }

        private async Task<IPage> GetOrCreatePageAsync(IBrowser browser)
        {
            return await browser.NewPageAsync(); // Luôn tạo page mới
            // var pages = await browser.PagesAsync();
            // var page = pages.FirstOrDefault(p => !p.IsClosed) ?? await browser.NewPageAsync();
            // return page;
        }

        public async Task<byte[]> ConvertFromHtmlAsync(string html)
        {
            _logger.LogInformation("Starting PDF conversion");
            var stopwatch = Stopwatch.StartNew();

            return await Policy
                .Handle<Exception>()
                .RetryAsync(3, (exception, retryCount) =>
                {
                    _logger.LogWarning($"Retry {retryCount} for PDF conversion due to error: {exception.Message}");
                })
                .ExecuteAsync(async () =>
                {
                    await _semaphore.WaitAsync();
                    IBrowser browser = null;
                    IPage page = null;
                    try
                    {
                        _logger.LogInformation("Getting browser from pool...");
                        browser = _browserPool.Get();
                        _logger.LogInformation($"Browser acquired in {stopwatch.ElapsedMilliseconds}ms");
                        _logger.LogInformation("Creating new page...");
                        page = await GetOrCreatePageAsync(browser);
                        _logger.LogInformation($"Page created in {stopwatch.ElapsedMilliseconds}ms");
                        _logger.LogInformation("Setting content...");
                        await page.SetContentAsync(html);
                        _logger.LogInformation($"Content set in {stopwatch.ElapsedMilliseconds}ms");
                        _logger.LogInformation("Generating PDF...");
                        var pdf = await page.PdfDataAsync(new PdfOptions
                        {
                            Format = PuppeteerSharp.Media.PaperFormat.A4,
                            PrintBackground = true
                        });
                        _logger.LogInformation($"PDF generated in {stopwatch.ElapsedMilliseconds}ms");
                        return pdf;
                    }
                    finally
                    {
                        if (page != null)
                            await page.CloseAsync();
                        if (browser != null)
                            _browserPool.Return(browser);
                        _semaphore.Release();
                        _logger.LogInformation($"PDF conversion completed in {stopwatch.ElapsedMilliseconds}ms");
                    }
                });
        }

        public void Dispose()
        {
            // Không cần đóng browser ở đây vì pool sẽ quản lý, nhưng có thể thêm logic cleanup nếu cần
        }
    }
}