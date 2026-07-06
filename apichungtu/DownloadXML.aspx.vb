Imports System.Text

Partial Class DownloadXml
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
        Dim b64 As String =
            GetXmlFromService(
                machungtu,
                madonvi)
        Dim xmlContent =
    Encoding.UTF8.GetString(
        Convert.FromBase64String(b64))
        Dim bytes =
            Encoding.UTF8.GetBytes(xmlContent)
        Response.Clear()
        Response.ContentType =
            "application/xml"
        Response.AddHeader(
            "content-disposition",
            "attachment; filename=" + machungtu + "_" + madonvi + ".xml")
        Response.BinaryWrite(bytes)
        Response.End()
    End Sub

    Private Function GetXmlFromService(
        machungtu As String,
        madonvi As String) As String
        Dim client As New service()
        Dim xml As String = client.LayXmlChungTu(madonvi, machungtu)
        Return xml
    End Function


End Class