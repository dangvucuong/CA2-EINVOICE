Imports Newtonsoft.Json.Linq
Imports PuppeteerSharp
Imports PuppeteerSharp.Media
Imports System.IO
Imports System.Threading.Tasks
Partial Class DownloadPdf
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(
        sender As Object,
        e As EventArgs) Handles Me.Load
        Dim token As String =
            Request.QueryString("q")
        Dim raw As String =
            CryptoHelper.Decrypt(token)
        Dim arr =
            raw.Split("|")
        Dim machungtu =
            arr(0)
        Dim madonvi =
            arr(1)
        Dim html As String =
            GetHtmlFromService(
                machungtu,
                madonvi)
        Dim pdfBytes =
            HtmlToPdf(html)
        Response.Clear()
        Response.ContentType =
            "application/pdf"
        Response.AddHeader(
            "content-disposition",
            "attachment;filename=" & machungtu & "_" & madonvi & ".pdf")
        Response.BinaryWrite(pdfBytes)
        Response.End()
    End Sub
    Private Function HtmlToPdf(html As String) As Byte()

        Dim t As Task(Of Byte()) =
        Task.Run(
        Async Function() As Task(Of Byte())

            Dim exePath As String =
                "C:\chromium\chrome-win\chrome.exe"

            Dim options As New LaunchOptions()

            options.ExecutablePath = exePath
            options.Headless = True

            options.Args = New String() {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage",
                "--disable-gpu"
            }

            Dim browser =
                Await Puppeteer.LaunchAsync(options)

            Dim page =
                Await browser.NewPageAsync()

            Await page.SetContentAsync(html)


            ' css ép vừa 1 trang A4
            Await page.AddStyleTagAsync(
    New AddTagOptions With {
        .Content =
            "@page { size:A4; margin:0; } " &
            "html,body{ margin:0; padding:0; background:white;} " &
            "body{ display:flex; justify-content:center; } " &
            "body > * { width:210mm; } " &
            "*{ page-break-inside:avoid !important; }"
    })


            Dim pdf =
Await page.PdfDataAsync(
    New PdfOptions With {
        .Format = PaperFormat.A4,
        .PrintBackground = True,
        .PreferCSSPageSize = True,
        .Scale = 0.97
    })

            Await page.CloseAsync()
            Await browser.CloseAsync()
            Return pdf
        End Function)
        Return t.Result

    End Function
    Private Function GetHtmlFromService(
        machungtu As String,
        madonvi As String) As String
        Try
            Dim client As New service()
            Dim json As String =
                client.XemChungTu(
                    machungtu,
                    madonvi)
            Dim obj As JObject =
                JObject.Parse(json)
            Return obj("data").ToString()
        Catch ex As Exception
            Return "<h2>Lỗi lấy chứng từ</h2>" &
                   "<br/>" &
                   ex.Message
        End Try
    End Function
End Class