using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Service.Pdf;
using Microsoft.Extensions.Logging;
using Polly;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Service.Pdf
{
    public class PdfService : IPdfService, IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3);
        private readonly ILogger<PdfService> _logger;

        private static IBrowser _browser;

        private static readonly SemaphoreSlim _browserLock =
            new SemaphoreSlim(1, 1);

        // recycle browser theo thời gian thay vì theo số file
        private static DateTime _lastRecycleTime =
            DateTime.UtcNow;

        private static readonly TimeSpan RecycleInterval =
            TimeSpan.FromMinutes(30);

        private const string ChromePath =
            @"C:\Deploy\Chrome\chrome-win64\chrome.exe";

        public PdfService(ILogger<PdfService> logger)
        {
            _logger = logger;

            if (!System.IO.File.Exists(ChromePath))
                throw new Exception($"chrome.exe not found at {ChromePath}");
        }

        private async Task EnsureBrowserAsync()
        {
            if (_browser != null && !_browser.IsClosed)
                return;

            await _browserLock.WaitAsync();

            try
            {
                if (_browser == null || _browser.IsClosed)
                {
                    _logger.LogInformation("Launching Chrome browser...");

                    _browser = await Puppeteer.LaunchAsync(
                        new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = ChromePath,
                            Timeout = 0,

                            Args = new[]
                            {
                                "--headless=new",

                                "--no-sandbox",
                                "--disable-setuid-sandbox",

                                "--disable-dev-shm-usage",
                                "--disable-gpu",

                                "--disable-site-isolation-trials",

                                "--renderer-process-limit=2",

                                "--disable-backgrounding-occluded-windows",
                                "--disable-renderer-backgrounding",

                                "--no-first-run",
                                "--no-default-browser-check",

                                "--disable-background-networking",
                                "--disable-sync",

                                "--metrics-recording-only",

                                "--password-store=basic",
                                "--use-mock-keychain"
                            }
                        });

                    _lastRecycleTime = DateTime.UtcNow;
                }
            }
            finally
            {
                _browserLock.Release();
            }
        }

        public async Task<byte[]> ConvertFromHtmlAsync(string html)
        {
            await _semaphore.WaitAsync();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                return await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () =>
                    {
                        IPage page = null;

                        try
                        {
                            await EnsureBrowserAsync();

                            page = await _browser.NewPageAsync();

                            await page.SetContentAsync(
                                html,
                                new NavigationOptions
                                {
                                    WaitUntil = new[]
                                    {
                                        WaitUntilNavigation.Networkidle0
                                    }
                                });

                            var pdf = await page.PdfDataAsync(
                                new PdfOptions
                                {
                                    Format = PaperFormat.A4,
                                    PrintBackground = true
                                });

                            // recycle theo thời gian
                            if (DateTime.UtcNow - _lastRecycleTime > RecycleInterval)
                            {
                                _ = Task.Run(async () =>
                                {
                                    await _browserLock.WaitAsync();

                                    try
                                    {
                                        if (_browser != null)
                                        {
                                            _logger.LogWarning(
                                                "Recycling browser by timer");

                                            await _browser.CloseAsync();

                                            _browser.Dispose();

                                            _browser = null;

                                            _lastRecycleTime =
                                                DateTime.UtcNow;
                                        }
                                    }
                                    finally
                                    {
                                        _browserLock.Release();
                                    }
                                });
                            }

                            return pdf;
                        }
                        finally
                        {
                            if (page != null)
                            {
                                await page.CloseAsync();
                                page.Dispose();
                            }
                        }
                    });
            }
            finally
            {
                _semaphore.Release();

                _logger.LogInformation(
                    $"PDF generated in {stopwatch.ElapsedMilliseconds} ms");
            }
        }

        public void Dispose()
        {
            _browserLock.Wait();

            try
            {
                if (_browser != null)
                {
                    _browser.CloseAsync().Wait();

                    _browser.Dispose();

                    _browser = null;
                }
            }
            finally
            {
                _browserLock.Release();
            }
        }
    }
}