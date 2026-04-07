using Contracts.Service.Pdf;
using PuppeteerSharp;

namespace Service.Pdf
{
    public class PdfServiceBackUp : IPdfService
    {
        
        private static readonly BrowserFetcher _browserFetcher = new BrowserFetcher();
        private static IBrowser _browser;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(10);
        public PdfServiceBackUp()
        {
            // Khởi tạo trình duyệt khi tạo đối tượng PdfService
            InitializeBrowserAsync().GetAwaiter().GetResult();
        }
        private async Task InitializeBrowserAsync()
        {
            await _browserFetcher.DownloadAsync();
            _browser ??= await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Timeout = 0
            });
        }
        private async Task<IPage> CreatePageAsync()
        {
            if (_browser == null)
            {
                await InitializeBrowserAsync();
            }
            return await _browser.NewPageAsync();
        }
        public async Task<byte[]> ConvertFromHtmlAsync(string html)
        {
            await _semaphore.WaitAsync();
            try
            {
                var page = await CreatePageAsync();
                try
                {
                    await page.SetContentAsync(html);
                    return await page.PdfDataAsync(new PdfOptions
                    {
                        Format = PuppeteerSharp.Media.PaperFormat.A4,
                        PrintBackground = true
                    });
                }
                finally
                {
                    await page.CloseAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }

        }
     
        ~PdfServiceBackUp()
        {
            // Đảm bảo trình duyệt được đóng khi PdfService bị hủy
            _browser?.CloseAsync().GetAwaiter().GetResult();
        }
        public void Dispose()
        {
            _browser?.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
