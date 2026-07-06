<%@ WebHandler Language="VB" Class="ViewChungTu" %>

Imports System
Imports System.Web
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Class ViewChungTu
    Implements IHttpHandler


    Public Sub ProcessRequest(context As HttpContext) _
        Implements IHttpHandler.ProcessRequest


        Dim machungtu As String =
            context.Request("machungtu")

        Dim madonvi As String =
            context.Request("madonvi")


        If String.IsNullOrEmpty(machungtu) _
        OrElse String.IsNullOrEmpty(madonvi) Then

            context.Response.Write(
                "Thiếu tham số machungtu hoặc madonvi")

            Return

        End If


        Dim html As String =
            GetHtmlFromService(machungtu, madonvi)


        context.Response.Clear()

        context.Response.ContentType =
            "text/html; charset=utf-8"

        context.Response.Write(html)

        context.Response.End()


    End Sub



    Private Function GetHtmlFromService(
        machungtu As String,
        madonvi As String) As String


        Dim url As String =
            "https://apichungtuv2.nacencomm.vn/service.asmx/Xemchungtu" &
            "?machungtu=" & machungtu &
            "&madonvi=" & madonvi


        Dim request As HttpWebRequest =
            CType(WebRequest.Create(url),
            HttpWebRequest)


        request.Method = "POST"


        Dim response As HttpWebResponse =
            CType(request.GetResponse(),
            HttpWebResponse)


        Dim reader As New StreamReader(
            response.GetResponseStream())


        Dim result As String =
            reader.ReadToEnd()


        ' nếu webmethod trả dạng:
        ' <string>JSON</string>

        Dim jsonText As String = result


        If result.Contains("<string") Then

            Dim doc As New Xml.XmlDocument()

            doc.LoadXml(result)

            jsonText = doc.InnerText

        End If


        Dim obj As JObject =
            JObject.Parse(jsonText)


        Dim html As String =
            obj("data").ToString()


        Return html


    End Function



    Public ReadOnly Property IsReusable() _
        As Boolean Implements IHttpHandler.IsReusable

        Get
            Return False
        End Get

    End Property


End Class