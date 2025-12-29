namespace Contracts.Service.Pdf
{
    public interface IPdfService
    {
        Task<byte[]> ConvertFromHtmlAsync(string html);
        // Task<bool> ConvertFromHtmlAsync(string html, string outputPath);
    }
}