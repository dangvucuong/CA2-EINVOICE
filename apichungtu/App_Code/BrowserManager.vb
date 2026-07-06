Imports PuppeteerSharp
Imports System.Threading.Tasks
Public Class BrowserManager
    Private Shared _browser As IBrowser
    Private Shared ReadOnly _lock As New Object()
    Public Shared Async Function GetBrowser() _
        As Task(Of IBrowser)
        If _browser IsNot Nothing Then
            Return _browser
        End If
        Dim fetcher As New BrowserFetcher()
        If fetcher.GetInstalledBrowsers().Count = 0 Then
            Await fetcher.DownloadAsync()
        End If
        Dim launchOptions As New LaunchOptions With {
            .Headless = True,
            .Args = {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage",
                "--disable-gpu",
                "--disable-extensions",
                "--disable-background-networking",
                "--disable-sync",
                "--metrics-recording-only",
                "--mute-audio",
                "--no-first-run",
                "--single-process"
            }
        }
        Dim browser =
            Await Puppeteer.LaunchAsync(launchOptions)
        SyncLock _lock
            If _browser Is Nothing Then
                _browser = browser
            End If
        End SyncLock
        Return _browser
    End Function
End Class