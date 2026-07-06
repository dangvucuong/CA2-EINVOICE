Imports System.Security.Cryptography
Imports System.Text

Public Class CryptoHelper

    Private Shared key As String =
        "nacencomm@2026"

    ' salt phải >= 8 bytes
    Private Shared salt As Byte() =
        Encoding.UTF8.GetBytes("salt1234")


    Public Shared Function Encrypt(text As String) As String

        Dim aes As Aes =
            Aes.Create()

        Dim pdb As New Rfc2898DeriveBytes(
            key,
            salt,
            1000)

        aes.Key =
            pdb.GetBytes(32)

        aes.IV =
            pdb.GetBytes(16)


        Using ms As New IO.MemoryStream()

            Using cs As New CryptoStream(
                ms,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write)

                Dim bytes =
                    Encoding.UTF8.GetBytes(text)

                cs.Write(bytes, 0, bytes.Length)

            End Using

            Dim base64 =
                Convert.ToBase64String(
                    ms.ToArray())

            ' chuyển sang dạng url safe
            base64 = base64.Replace("+", "-")
            base64 = base64.Replace("/", "_")
            base64 = base64.Replace("=", "")

            Return base64

        End Using

    End Function

    Public Shared Function Decrypt(cipher As String) As String
        If Not String.IsNullOrEmpty(cipher) Then
            cipher = cipher.Replace("-", "+")
            cipher = cipher.Replace("_", "/")
            Select Case cipher.Length Mod 4
                Case 2
                    cipher &= "=="
                Case 3
                    cipher &= "="
            End Select
            Dim aes As Aes =
            Aes.Create()

            Dim pdb As New Rfc2898DeriveBytes(
            key,
            salt,
            1000)

            aes.Key =
            pdb.GetBytes(32)

            aes.IV =
            pdb.GetBytes(16)

            Dim buffer =
            Convert.FromBase64String(cipher)

            Using ms As New IO.MemoryStream()
                Using cs As New CryptoStream(
                ms,
                aes.CreateDecryptor(),
                CryptoStreamMode.Write)
                    cs.Write(buffer, 0, buffer.Length)
                End Using
                Return Encoding.UTF8.GetString(
                ms.ToArray())
            End Using
        End If
        ' convert lại base64 url safe

    End Function


End Class