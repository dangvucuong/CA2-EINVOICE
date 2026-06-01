Imports System.Web.Services.Protocols

Public Class Authenticate
    Public Class AuthHeader
        Inherits SoapHeader
        Public Property Username As String
        Public Property Password As String
    End Class
End Class
