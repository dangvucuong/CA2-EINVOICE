Imports Newtonsoft.Json.Linq
Imports PuppeteerSharp
Imports PuppeteerSharp.Media
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Web

Partial Class DownloadZip
    Inherits System.Web.UI.Page

    Private Const MaxItems As Integer = 20

    Protected Sub Page_Load(
        sender As Object,
        e As EventArgs) Handles Me.Load

        Try
            Dim downloadType As String =
                If(Request.QueryString("type"), "").Trim().ToLowerInvariant()
            Dim madonvi As String =
                If(Request.QueryString("madonvi"), "").Trim()
            Dim machungtuRaw As String =
                If(Request.QueryString("machungtu"), "").Trim()

            If String.IsNullOrEmpty(madonvi) OrElse String.IsNullOrEmpty(machungtuRaw) Then
                WriteErrorResponse(400, "Thiếu tham số madonvi hoặc machungtu.")
                Return
            End If

            If downloadType <> "pdf" AndAlso downloadType <> "xml" Then
                WriteErrorResponse(400, "type phải là pdf hoặc xml.")
                Return
            End If

            Dim ids = machungtuRaw.Split(","c).
                Select(Function(x) x.Trim()).
                Where(Function(x) x <> "").
                Distinct().
                Take(MaxItems).
                ToList()

            If ids.Count = 0 Then
                WriteErrorResponse(400, "Danh sách machungtu không hợp lệ.")
                Return
            End If

            Dim zipEntries As New Dictionary(Of String, Byte())()

            For Each machungtu In ids
                Try
                    Dim entryName As String
                    Dim fileBytes As Byte()

                    If downloadType = "pdf" Then
                        Dim html = GetHtmlFromService(machungtu, madonvi)
                        fileBytes = HtmlToPdf(html)
                        entryName = machungtu & "_" & madonvi & ".pdf"
                    Else
                        fileBytes = GetXmlBytes(machungtu, madonvi)
                        entryName = machungtu & "_" & madonvi & ".xml"
                    End If

                    If fileBytes Is Nothing OrElse fileBytes.Length = 0 Then
                        Continue For
                    End If

                    Dim uniqueName = entryName
                    Dim suffix = 1
                    While zipEntries.ContainsKey(uniqueName)
                        suffix += 1
                        uniqueName = Path.GetFileNameWithoutExtension(entryName) & "_" & suffix & Path.GetExtension(entryName)
                    End While
                    zipEntries(uniqueName) = fileBytes
                Catch ex As Exception
                    System.Diagnostics.Trace.WriteLine("DownloadZip item " & machungtu & ": " & ex.Message)
                End Try
            Next

            If zipEntries.Count = 0 Then
                WriteErrorResponse(404, "Không tạo được file tải xuống (không có XML/PDF hợp lệ). Kiểm tra mã đơn vị (MST) và mã chứng từ.")
                Return
            End If

            Dim zipBytes = ChungTuZipBuilder.CreateZip(zipEntries)
            Dim zipName =
                If(downloadType = "pdf", "ChungTuPdf.zip", "ChungTuXml.zip")

            Response.Clear()
            Response.ContentType = "application/zip"
            Response.AddHeader(
                "content-disposition",
                "attachment;filename=" & zipName)
            Response.BinaryWrite(zipBytes)
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            WriteErrorResponse(500, "Lỗi tạo file zip: " & ex.Message)
        End Try
    End Sub

    Private Sub WriteErrorResponse(statusCode As Integer, message As String)
        Response.Clear()
        Response.StatusCode = statusCode
        Response.ContentType = "text/plain; charset=utf-8"
        Response.Write(message)
        Response.Flush()
        HttpContext.Current.ApplicationInstance.CompleteRequest()
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
                client.XemChungTu(machungtu, madonvi)
            Dim obj As JObject = JObject.Parse(json)
            Return obj("data").ToString()
        Catch ex As Exception
            Return "<h2>Lỗi lấy chứng từ</h2><br/>" & ex.Message
        End Try
    End Function

    ''' <summary>Giống DownloadXML.aspx — LayXmlChungTu(madonvi, machungtu).</summary>
    Private Function GetXmlBytes(
        machungtu As String,
        madonvi As String) As Byte()

        Dim client As New service()
        Dim b64 As String = client.LayXmlChungTu(madonvi, machungtu)
        If String.IsNullOrWhiteSpace(b64) Then
            Return Nothing
        End If
        Return DecodeXmlPayload(b64)
    End Function

    Private Function DecodeXmlPayload(payload As String) As Byte()
        Dim trimmed = payload.Trim()
        If trimmed.StartsWith("<") Then
            Return Encoding.UTF8.GetBytes(trimmed)
        End If
        Try
            Dim xmlContent = Encoding.UTF8.GetString(Convert.FromBase64String(trimmed))
            Return Encoding.UTF8.GetBytes(xmlContent)
        Catch
            Return Encoding.UTF8.GetBytes(trimmed)
        End Try
    End Function

    Private NotInheritable Class ChungTuZipBuilder
        Private Sub New()
        End Sub

        Public Shared Function CreateZip(files As Dictionary(Of String, Byte())) As Byte()
            If files Is Nothing OrElse files.Count = 0 Then
                Return New Byte() {}
            End If

            Using ms As New MemoryStream()
                Dim central As New List(Of Byte())()
                For Each kvp In files
                    Dim nameBytes = Encoding.UTF8.GetBytes(SanitizeEntryName(kvp.Key))
                    Dim data = If(kvp.Value, New Byte() {})
                    Dim crc = Crc32(data)
                    Dim localOffset = CLng(ms.Position)
                    WriteUInt32(ms, &H4034B50UI)
                    WriteUInt16(ms, 20)
                    WriteUInt16(ms, 0)
                    WriteUInt16(ms, 0)
                    WriteUInt16(ms, 0)
                    WriteUInt16(ms, 0)
                    WriteUInt32(ms, crc)
                    WriteUInt32(ms, CUInt(data.Length))
                    WriteUInt32(ms, CUInt(data.Length))
                    WriteUInt16(ms, CUShort(nameBytes.Length))
                    WriteUInt16(ms, 0)
                    ms.Write(nameBytes, 0, nameBytes.Length)
                    If data.Length > 0 Then
                        ms.Write(data, 0, data.Length)
                    End If
                    central.Add(BuildCentralHeader(nameBytes, crc, data.Length, CUInt(localOffset)))
                Next

                Dim centralStart = ms.Position
                For Each ch In central
                    ms.Write(ch, 0, ch.Length)
                Next
                Dim centralSize = ms.Position - centralStart

                WriteUInt32(ms, &H6054B50UI)
                WriteUInt16(ms, 0)
                WriteUInt16(ms, 0)
                WriteUInt16(ms, CUShort(central.Count))
                WriteUInt16(ms, CUShort(central.Count))
                WriteUInt32(ms, CUInt(centralSize))
                WriteUInt32(ms, CUInt(centralStart))
                WriteUInt16(ms, 0)

                Return ms.ToArray()
            End Using
        End Function

        Private Shared Function SanitizeEntryName(name As String) As String
            Dim n = If(name, "file").Replace("\"c, "_"c).Replace("/"c, "_"c)
            If n.Length > 180 Then
                n = n.Substring(n.Length - 180)
            End If
            Return n
        End Function

        Private Shared Function BuildCentralHeader(
            nameBytes As Byte(),
            crc As UInteger,
            size As Integer,
            localOffset As UInteger) As Byte()

            Using ms As New MemoryStream()
                WriteUInt32(ms, &H2014B50UI)
                WriteUInt16(ms, 20)
                WriteUInt16(ms, 20)
                WriteUInt16(ms, 0)
                WriteUInt16(ms, 0)
                WriteUInt16(ms, 0)
                WriteUInt16(ms, 0)
                WriteUInt32(ms, crc)
                WriteUInt32(ms, CUInt(size))
                WriteUInt32(ms, CUInt(size))
                WriteUInt16(ms, CUShort(nameBytes.Length))
                WriteUInt16(ms, 0)
                WriteUInt16(ms, 0)
                WriteUInt16(ms, 0)
                WriteUInt16(ms, 0)
                WriteUInt32(ms, 0)
                WriteUInt32(ms, localOffset)
                ms.Write(nameBytes, 0, nameBytes.Length)
                Return ms.ToArray()
            End Using
        End Function

        Private Shared Sub WriteUInt16(ms As Stream, value As UShort)
            ms.WriteByte(CByte(value And &HFF))
            ms.WriteByte(CByte((value >> 8) And &HFF))
        End Sub

        Private Shared Sub WriteUInt32(ms As Stream, value As UInteger)
            ms.WriteByte(CByte(value And &HFF))
            ms.WriteByte(CByte((value >> 8) And &HFF))
            ms.WriteByte(CByte((value >> 16) And &HFF))
            ms.WriteByte(CByte((value >> 24) And &HFF))
        End Sub

        Private Shared Function Crc32(data As Byte()) As UInteger
            Dim crc As UInteger = &HFFFFFFFFUI
            For Each b In data
                crc = crc Xor b
                For i = 0 To 7
                    Dim mask = CUInt(If((crc And 1UI) = 1UI, &HFFFFFFFFUI, 0UI))
                    crc = (crc >> 1) Xor (3988292384UI And mask)
                Next
            Next
            Return crc Xor &HFFFFFFFFUI
        End Function
    End Class

End Class
