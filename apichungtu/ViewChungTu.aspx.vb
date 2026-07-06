Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Net
Partial Class ViewChungTu
    Inherits System.Web.UI.Page
    Private Sub ViewChungTu_Load(sender As Object, e As EventArgs) Handles Me.Load
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
        If String.IsNullOrEmpty(machungtu) _
        OrElse String.IsNullOrEmpty(madonvi) Then
            Response.Write("Thiếu tham số")
            Return
        End If
        Dim html As String =
            GetHtmlFromService(machungtu, madonvi)
        ltrHtml.Text = html
    End Sub
    Private Function GetHtmlFromService(
        machungtu As String,
        madonvi As String) As String
        Try
            Dim client As New service
            Dim json As String = client.XemChungTu(machungtu, madonvi)
            Dim obj As JObject =
                JObject.Parse(json)
            Dim html As String =
                obj("data").ToString()
            Return html
        Catch ex As Exception
            Return "<h2>Lỗi lấy chứng từ</h2>" &
                   "<br/>" &
                   ex.Message
        End Try
    End Function
End Class
