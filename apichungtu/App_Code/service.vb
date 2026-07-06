Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml
Imports System.Threading
Imports System.Security.Cryptography.Xml
Imports System.Collections.Generic
Imports System.Security.Cryptography
Imports System.Security
Imports Newtonsoft.Json
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.ServiceModel.Activities
Imports clDLCTu
Imports System.Xml.XPath
Imports System.Xml.Xsl
Imports System.Data.OleDb
Imports System.Web.Script.Serialization
Imports System.Runtime.Remoting.Contexts
Imports WebSupergoo.ABCpdf11
Imports Microsoft.VisualBasic.Logging
Imports WebSupergoo.Org.BouncyCastle.Crypto
Imports System.Net.Mail
Imports Newtonsoft.Json.Linq
Imports WebSupergoo.ABCpdf11.Internal


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class service
    Inherits System.Web.Services.WebService

    'Dim connectionString As String = "Data Source=10.10.20.108\MSSQLSERVER_UAT;Initial Catalog=evoice;Persist Security Info=True;User ID=sa;Password=zaq1ZAQ!;Connect Timeout=0"
    Dim connectionString As String = "data source = 172.16.100.16;initial catalog=evoice;user id=sa;password=zaq1xsw2ZAQ!XSW@;MultipleActiveResultSets=True;Max Pool Size=128;"

    Dim connString_UAT_POS As String = "data source = 10.10.20.108\MSSQLSERVER_UAT;initial catalog=ca2pos;user id=sa;password=zaq1ZAQ!;MultipleActiveResultSets=True;Max Pool Size=128;"

    Dim ConnectionStringtvan As String = "Data Source=10.10.20.15;Initial Catalog=InterTRCA2;User ID=ncm;Password=C@vn2chuaboc!;Connect Timeout=0;TrustServerCertificate=True;"
    'Dim connectionString68 As String = "Data Source=10.10.20.15;Initial Catalog=evoicedb78;User ID=ncm;Password=C@vn2chuaboc!;Connect Timeout=0;TrustServerCertificate=True;"

    Dim divtracuu As String = "padding-top:0px;text-align:left;padding-bottom:10px;font-size:11.5pt;align:center;px;-ms-transform: rotate(-90deg);-webkit-transform: rotate(-90deg);transform: rotate(-90deg);width:900px;left:485px;top:-600px;float:right;height:15px;position:relative;"


    'Đăng ký tờ khai
    <WebMethod()>
    Public Function Taotokhai(sjsonTTChungTK As String, sjsonTTCTS As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim kq As Integer = TaotokhaiCT70(sjsonTTChungTK, sjsonTTCTS, "", "")

            If kq > 0 Then
                response("status") = "success"
                response("message") = "Tạo tờ khai thành công"
                response("data") = kq
            Else
                response("status") = "error"
                response("message") = "Tạo tờ khai không thành công"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function Suatokhai(sjsonTTChungTK As String, sjsonTTCTS As String, Matokhai_CT As Integer, madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim kq As Integer = SuatokhaiCT70(Matokhai_CT, sjsonTTChungTK, sjsonTTCTS, "", "", madonvi)

            If kq > 0 Then
                response("status") = "success"
                response("message") = "Cập nhật tờ khai thành công"
                response("data") = kq
            Else
                response("status") = "error"
                response("message") = "Cập nhật tờ khai không thành công"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    Private Function TaotokhaiCT70(ByVal sjsonTTChungTK As String, ByVal sjsonTTCTS As String, ByVal sjsonTTUNhiem As String, ByVal xmltokhai As String) As Integer

        Dim kq As Integer = 0
        Dim jTTChungTK As clDLCTu.ThongtintokhaiCT = Nothing
        Dim dt_cts As DataTable = New DataTable()
        Dim dt_dsunhiem As DataTable = New DataTable()

        Dim xmltokhaict As String = ""

        Try
            If Not String.IsNullOrEmpty(sjsonTTChungTK) Then jTTChungTK = JsonConvert.DeserializeObject(Of clDLCTu.ThongtintokhaiCT)(sjsonTTChungTK)

            If Not String.IsNullOrEmpty(sjsonTTCTS) Then
                dt_cts = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTCTS)
            End If

            If Not String.IsNullOrEmpty(sjsonTTUNhiem) Then
                dt_dsunhiem = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTUNhiem)
            End If

        Catch __unusedJsonSerializationException1__ As JsonSerializationException
            Return -1
        End Try

        If jTTChungTK IsNot Nothing AndAlso dt_cts.Rows.Count > 0 Then
            Dim trangthaitk As Integer = 1

            If Not String.IsNullOrEmpty(jTTChungTK.KhoaphienTN) Then
                trangthaitk = 2
            End If


            If Not String.IsNullOrEmpty(xmltokhai) Then
                xmltokhaict = xmltokhai
            Else
                xmltokhaict = TaoxmltokhaiCT70(sjsonTTChungTK, sjsonTTCTS, sjsonTTUNhiem)
            End If

            Dim byte1 As Byte() = System.Text.Encoding.UTF8.GetBytes(xmltokhaict)
            xmltokhaict = Convert.ToBase64String(byte1)

            Using conn As SqlConnection = New SqlConnection(connectionString)
                conn.Open()

                Using comm As SqlCommand = conn.CreateCommand()
                    comm.CommandText =
                    "INSERT INTO Tokhaichungtu(" &
                        "PBan, MSo, Ten, HThuc, TNNT, MST, CQTQLy, MCQTQLy, " &
                        "NLHe, DCLHe, DCTDTu, DTLHe, DDanh, NLap, TCCNPHanh, CQTPHanh, " &
                        "CTTNCNhan, CTKTTTMDTu, BLTPLPKIn, BLTPLPIn, BLTTPLPhi, " &
                        "CDLQCCQT, CDLQTCTN, CDLQTCTNUT, Ngaytao, Ngaycapnhat, Trangthai, " &
                        "SerialNo, Taikhoan, KhoaphienTN, co_quan_thue_id, XMLTokhai" &
                        ") OUTPUT INSERTED.MatokhaiCT " &
                    "VALUES (" &
                        "@PBan, @MSo, @Ten, @HThuc, @TNNT, @MST, @CQTQLy, @MCQTQLy, " &
                        "@NLHe, @DCLHe, @DCTDTu, @DTLHe, @DDanh, @NLap, @TCCNPHanh, @CQTPHanh, " &
                        "@CTTNCNhan, @CTKTTTMDTu, @BLTPLPKIn, @BLTPLPIn, @BLTTPLPhi, " &
                        "@CDLQCCQT, @CDLQTCTN, @CDLQTCTNUT, GETDATE(), GETDATE(), @Trangthai, " &
                        "@SerialNo, @Taikhoan, @KhoaphienTN,@co_quan_thue_id,@XMLTokhai" &
                    ")"
                    comm.Parameters.AddWithValue("@PBan", jTTChungTK.PBan)
                    comm.Parameters.AddWithValue("@MSo", jTTChungTK.MSo)
                    comm.Parameters.AddWithValue("@Ten", jTTChungTK.Ten)
                    comm.Parameters.AddWithValue("@HThuc", jTTChungTK.HThuc)
                    comm.Parameters.AddWithValue("@TNNT", jTTChungTK.TNNT)
                    comm.Parameters.AddWithValue("@MST", jTTChungTK.MST)
                    comm.Parameters.AddWithValue("@CQTQLy", jTTChungTK.CQTQLy)
                    comm.Parameters.AddWithValue("@MCQTQLy", jTTChungTK.MCQTQLy)
                    comm.Parameters.AddWithValue("@NLHe", jTTChungTK.NLHe)
                    comm.Parameters.AddWithValue("@DCLHe", jTTChungTK.DCLHe)
                    comm.Parameters.AddWithValue("@DCTDTu", jTTChungTK.DCTDTu)
                    comm.Parameters.AddWithValue("@DTLHe", jTTChungTK.DTLHe)
                    comm.Parameters.AddWithValue("@DDanh", jTTChungTK.DDanh)
                    comm.Parameters.AddWithValue("@NLap", jTTChungTK.NLap)
                    comm.Parameters.AddWithValue("@TCCNPHanh", jTTChungTK.TCCNPHanh)
                    comm.Parameters.AddWithValue("@CQTPHanh", jTTChungTK.CQTPHanh)
                    comm.Parameters.AddWithValue("@CTTNCNhan", jTTChungTK.CTTNCNhan)
                    comm.Parameters.AddWithValue("@CTKTTTMDTu", jTTChungTK.CTKTTTMDTu)
                    comm.Parameters.AddWithValue("@BLTPLPKIn", jTTChungTK.BLTPLPKIn)
                    comm.Parameters.AddWithValue("@BLTPLPIn", jTTChungTK.BLTPLPIn)
                    comm.Parameters.AddWithValue("@BLTTPLPhi", jTTChungTK.BLTTPLPhi)
                    comm.Parameters.AddWithValue("@CDLQCCQT", jTTChungTK.CDLQCCQT)
                    comm.Parameters.AddWithValue("@CDLQTCTN", jTTChungTK.CDLQTCTN)
                    comm.Parameters.AddWithValue("@CDLQTCTNUT", jTTChungTK.CDLQTCTNUT)
                    comm.Parameters.AddWithValue("@Trangthai", trangthaitk)
                    comm.Parameters.AddWithValue("@SerialNo", If(CObj(jTTChungTK.SerialNo), DBNull.Value))
                    comm.Parameters.AddWithValue("@Taikhoan", If(CObj(jTTChungTK.Taikhoan), DBNull.Value))
                    comm.Parameters.AddWithValue("@KhoaphienTN", If(CObj(jTTChungTK.KhoaphienTN), DBNull.Value))
                    comm.Parameters.AddWithValue("@co_quan_thue_id", jTTChungTK.co_quan_thue_id)
                    comm.Parameters.AddWithValue("@XMLTokhai", xmltokhaict)
                    kq = Convert.ToInt32(comm.ExecuteScalar())
                End Using

                If kq = 0 Then Return 0
                Dim col As DataColumn = New DataColumn("MatokhaiCT", GetType(Integer))
                dt_cts.Columns.Add(col)
                col.SetOrdinal(0)

                For Each row As DataRow In dt_cts.Rows
                    row("MatokhaiCT") = kq
                Next

                Using cmdCreate As SqlCommand = conn.CreateCommand()
                    cmdCreate.CommandText =
                        "IF OBJECT_ID('tempdb..#TempTKCTu') IS NOT NULL DROP TABLE #TempTKCTu; " &
                        "CREATE TABLE #TempTKCTu (" &
                        "MatokhaiCT int, STT numeric(3,0), TTChuc nvarchar(400), Seri nvarchar(40), " &
                        "TNgay nvarchar(40), DNgay nvarchar(40), HThuc tinyint" &
                     ");"
                    cmdCreate.ExecuteNonQuery()
                End Using

                Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(conn)
                    bulkCopy.DestinationTableName = "#TempTKCTu"
                    bulkCopy.WriteToServer(dt_cts)
                End Using

                Using cmdInsert As SqlCommand = conn.CreateCommand()
                    cmdInsert.CommandText = "INSERT INTO Tokhaichungtu_DanhsachCTS SELECT * FROM #TempTKCTu"
                    cmdInsert.ExecuteNonQuery()
                End Using

                If dt_dsunhiem.Rows.Count > 0 Then
                    Dim col1 As DataColumn = New DataColumn("MatokhaiCT", GetType(Integer))
                    dt_dsunhiem.Columns.Add(col1)
                    col1.SetOrdinal(0)

                    For Each row As DataRow In dt_dsunhiem.Rows
                        row("MatokhaiCT") = kq
                    Next

                    Using cmdCreate As SqlCommand = conn.CreateCommand()
                        cmdCreate.CommandText =
                        "IF OBJECT_ID('tempdb..#TempUNhiem') IS NOT NULL DROP TABLE #TempUNhiem; " &
                        "CREATE TABLE #TempUNhiem (" &
                        "MatokhaiCT int, LDKUNhiem tinyint, STT numeric(3,0), TLCTu nvarchar(100), " &
                        "KHMCTu nvarchar(7), KHCTu nvarchar(6), MST nvarchar(14), TTChuc nvarchar(400), " &
                        "MDich nvarchar(255), TNgay date, DNgay date, PThuc nvarchar(50)" &
                        ");"
                        cmdCreate.ExecuteNonQuery()
                    End Using

                    Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(conn)
                        bulkCopy.DestinationTableName = "#TempUNhiem"
                        bulkCopy.WriteToServer(dt_dsunhiem)
                    End Using

                    Using cmdInsert As SqlCommand = conn.CreateCommand()
                        cmdInsert.CommandText = "INSERT INTO Tokhaichungtu_DKUNhiem SELECT * FROM #TempUNhiem"
                        cmdInsert.ExecuteNonQuery()
                    End Using
                End If
            End Using
        Else
            kq = -100
        End If

        Return kq
    End Function

    Private Function SuatokhaiCT70(ByVal MatokhaiCT As Integer, ByVal sjsonTTChungTK As String, ByVal sjsonTTCTS As String, ByVal sjsonTTUNhiem As String, ByVal xmltokhai As String, ByVal madonvi As String) As Integer
        Dim kq As Integer = 0
        Dim checksua As Integer = 0
        Dim jTTChungTK As clDLCTu.ThongtintokhaiCT = Nothing
        Dim dt_cts As DataTable = New DataTable()
        Dim dt_dsunhiem As DataTable = New DataTable()

        Try
            If Not String.IsNullOrEmpty(sjsonTTChungTK) Then jTTChungTK = JsonConvert.DeserializeObject(Of clDLCTu.ThongtintokhaiCT)(sjsonTTChungTK)

            If Not String.IsNullOrEmpty(sjsonTTCTS) Then
                dt_cts = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTCTS)
            End If

            If Not String.IsNullOrEmpty(sjsonTTUNhiem) Then
                dt_dsunhiem = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTUNhiem)
            End If

        Catch __unusedJsonSerializationException1__ As JsonSerializationException
            Return -1
        End Try

        If jTTChungTK IsNot Nothing AndAlso dt_cts.Rows.Count > 0 Then
            Dim trangthaitk As Integer = 1

            If Not String.IsNullOrEmpty(jTTChungTK.KhoaphienTN) Then
                trangthaitk = 2
            End If

            Dim dt As DataTable = Laythongtintokhaict(MatokhaiCT, madonvi)

            If dt.Rows.Count > 0 Then
                Dim trangthaitokhai As String = dt.Rows(0)("Trangthai").ToString()

                If trangthaitokhai = "1" Then
                    checksua = 1
                Else
                    Return -3
                End If
            Else
                Return -2
            End If

            If checksua = 1 Then
                Dim xmltokhaict As String = ""

                If Not String.IsNullOrEmpty(xmltokhai) Then
                    xmltokhaict = xmltokhai
                Else
                    xmltokhaict = TaoxmltokhaiCT70(sjsonTTChungTK, sjsonTTCTS, sjsonTTUNhiem)
                End If

                Dim byte1 As Byte() = System.Text.Encoding.UTF8.GetBytes(xmltokhaict)
                xmltokhaict = Convert.ToBase64String(byte1)

                Using conn As SqlConnection = New SqlConnection(connectionString)
                    conn.Open()

                    Using comm As SqlCommand = conn.CreateCommand()
                        comm.CommandText = "Update Tokhaichungtu set " &
                        "PBan=@PBan, MSo=@MSo, Ten=@Ten, HThuc=@HThuc, TNNT=@TNNT, MST=@MST, CQTQLy=@CQTQLy, MCQTQLy=@MCQTQLy, " &
                        "NLHe=@NLHe, DCLHe=@DCLHe, DCTDTu=@DCTDTu, DTLHe=@DTLHe, DDanh=@DDanh, NLap=@NLap, TCCNPHanh=@TCCNPHanh, CQTPHanh=@CQTPHanh, " &
                        "CTTNCNhan=@CTTNCNhan, CTKTTTMDTu=@CTKTTTMDTu, BLTPLPKIn=@BLTPLPKIn, BLTPLPIn=@BLTPLPIn, BLTTPLPhi=@BLTTPLPhi, " &
                        "CDLQCCQT=@CDLQCCQT, CDLQTCTN=@CDLQTCTN, CDLQTCTNUT=@CDLQTCTNUT, Ngaycapnhat=getdate(), Trangthai=@Trangthai, " &
                        "SerialNo=@SerialNo, Taikhoan=@Taikhoan, KhoaphienTN=@KhoaphienTN, co_quan_thue_id=@co_quan_thue_id ,XMLTokhai=@XMLTokhai " &
                        "where MatokhaiCT=@MatokhaiCT"
                        comm.Parameters.AddWithValue("@MatokhaiCT", MatokhaiCT)
                        comm.Parameters.AddWithValue("@PBan", jTTChungTK.PBan)
                        comm.Parameters.AddWithValue("@MSo", jTTChungTK.MSo)
                        comm.Parameters.AddWithValue("@Ten", jTTChungTK.Ten)
                        comm.Parameters.AddWithValue("@HThuc", jTTChungTK.HThuc)
                        comm.Parameters.AddWithValue("@TNNT", jTTChungTK.TNNT)
                        comm.Parameters.AddWithValue("@MST", jTTChungTK.MST)
                        comm.Parameters.AddWithValue("@CQTQLy", jTTChungTK.CQTQLy)
                        comm.Parameters.AddWithValue("@MCQTQLy", jTTChungTK.MCQTQLy)
                        comm.Parameters.AddWithValue("@NLHe", jTTChungTK.NLHe)
                        comm.Parameters.AddWithValue("@DCLHe", jTTChungTK.DCLHe)
                        comm.Parameters.AddWithValue("@DCTDTu", jTTChungTK.DCTDTu)
                        comm.Parameters.AddWithValue("@DTLHe", jTTChungTK.DTLHe)
                        comm.Parameters.AddWithValue("@DDanh", jTTChungTK.DDanh)
                        comm.Parameters.AddWithValue("@NLap", jTTChungTK.NLap)
                        comm.Parameters.AddWithValue("@TCCNPHanh", jTTChungTK.TCCNPHanh)
                        comm.Parameters.AddWithValue("@CQTPHanh", jTTChungTK.CQTPHanh)
                        comm.Parameters.AddWithValue("@CTTNCNhan", jTTChungTK.CTTNCNhan)
                        comm.Parameters.AddWithValue("@CTKTTTMDTu", jTTChungTK.CTKTTTMDTu)
                        comm.Parameters.AddWithValue("@BLTPLPKIn", jTTChungTK.BLTPLPKIn)
                        comm.Parameters.AddWithValue("@BLTPLPIn", jTTChungTK.BLTPLPIn)
                        comm.Parameters.AddWithValue("@BLTTPLPhi", jTTChungTK.BLTTPLPhi)
                        comm.Parameters.AddWithValue("@CDLQCCQT", jTTChungTK.CDLQCCQT)
                        comm.Parameters.AddWithValue("@CDLQTCTN", jTTChungTK.CDLQTCTN)
                        comm.Parameters.AddWithValue("@CDLQTCTNUT", jTTChungTK.CDLQTCTNUT)
                        comm.Parameters.AddWithValue("@Trangthai", trangthaitk)
                        comm.Parameters.AddWithValue("@SerialNo", If(CObj(jTTChungTK.SerialNo), DBNull.Value))
                        comm.Parameters.AddWithValue("@Taikhoan", If(CObj(jTTChungTK.Taikhoan), DBNull.Value))
                        comm.Parameters.AddWithValue("@KhoaphienTN", If(CObj(jTTChungTK.KhoaphienTN), DBNull.Value))
                        comm.Parameters.AddWithValue("@co_quan_thue_id", jTTChungTK.co_quan_thue_id)
                        comm.Parameters.AddWithValue("@XMLTokhai", xmltokhaict)
                        kq = comm.ExecuteNonQuery()
                    End Using

                    If kq = 0 Then Return 0

                    Using cmdXoaCTS As SqlCommand = conn.CreateCommand()
                        cmdXoaCTS.CommandText = "Delete Tokhaichungtu_DanhsachCTS where MatokhaiCT=@MatokhaiCT"
                        cmdXoaCTS.Parameters.AddWithValue("@MatokhaiCT", MatokhaiCT)
                        cmdXoaCTS.ExecuteNonQuery()
                    End Using

                    If Not dt_cts.Columns.Contains("MatokhaiCT") Then
                        Dim col As New DataColumn("MatokhaiCT", GetType(Integer))
                        dt_cts.Columns.Add(col)
                        col.SetOrdinal(0)
                    End If

                    For Each row As DataRow In dt_cts.Rows
                        row("MatokhaiCT") = MatokhaiCT
                    Next

                    Using cmdCreate As SqlCommand = conn.CreateCommand()
                        cmdCreate.CommandText =
                        "IF OBJECT_ID('tempdb..#TempTKCTu') IS NOT NULL DROP TABLE #TempTKCTu; " &
                        "CREATE TABLE #TempTKCTu (" &
                        "MatokhaiCT int, STT numeric(3,0), TTChuc nvarchar(400), Seri nvarchar(40), " &
                        "TNgay datetime, DNgay datetime, HThuc tinyint" &
                        ");"
                        cmdCreate.ExecuteNonQuery()
                    End Using

                    Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(conn)
                        bulkCopy.DestinationTableName = "#TempTKCTu"
                        bulkCopy.WriteToServer(dt_cts)
                    End Using

                    Using cmdInsert As SqlCommand = conn.CreateCommand()
                        cmdInsert.CommandText = "INSERT INTO Tokhaichungtu_DanhsachCTS SELECT * FROM #TempTKCTu"
                        cmdInsert.ExecuteNonQuery()
                    End Using

                    If dt_dsunhiem.Rows.Count > 0 Then

                        Using cmdXoaUN As SqlCommand = conn.CreateCommand()
                            cmdXoaUN.CommandText = "Delete Tokhaichungtu_DKUNhiem where MatokhaiCT=@MatokhaiCT"
                            cmdXoaUN.Parameters.AddWithValue("@MatokhaiCT", MatokhaiCT)
                            cmdXoaUN.ExecuteNonQuery()
                        End Using

                        If Not dt_dsunhiem.Columns.Contains("MatokhaiCT") Then
                            Dim col1 As New DataColumn("MatokhaiCT", GetType(Integer))
                            dt_dsunhiem.Columns.Add(col1)
                            col1.SetOrdinal(0)
                        End If

                        For Each row As DataRow In dt_dsunhiem.Rows
                            row("MatokhaiCT") = MatokhaiCT
                        Next

                        Using cmdCreate As SqlCommand = conn.CreateCommand()
                            cmdCreate.CommandText =
                            "IF OBJECT_ID('tempdb..#TempUNhiem') IS NOT NULL DROP TABLE #TempUNhiem; " &
                            "CREATE TABLE #TempUNhiem (" &
                            "MatokhaiCT int, LDKUNhiem tinyint, STT numeric(3,0), TLCTu nvarchar(100), " &
                            "KHMCTu nvarchar(7), KHCTu nvarchar(6), MST nvarchar(14), TTChuc nvarchar(400), " &
                            "MDich nvarchar(255), TNgay date, DNgay date, PThuc nvarchar(50)" &
                            ");"
                            cmdCreate.ExecuteNonQuery()
                        End Using

                        Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(conn)
                            bulkCopy.DestinationTableName = "#TempUNhiem"
                            bulkCopy.WriteToServer(dt_dsunhiem)
                        End Using

                        Using cmdInsert As SqlCommand = conn.CreateCommand()
                            cmdInsert.CommandText = "INSERT INTO Tokhaichungtu_DKUNhiem SELECT * FROM #TempUNhiem"
                            cmdInsert.ExecuteNonQuery()
                        End Using
                    End If
                End Using
            End If
        Else
            kq = -100
        End If

        Return kq
    End Function



    Private Function TaoxmltokhaiCT70(ByVal sjsonTTChungTK As String, ByVal sjsonTTCTS As String, ByVal sjsonTTUNhiem As String) As String
        Dim kq As String = ""
        Dim jTTChungTK As clDLCTu.ThongtintokhaiCT = Nothing
        Dim dt_cts As DataTable = New DataTable()
        Dim dt_dsunhiem As DataTable = New DataTable()

        Try
            If Not String.IsNullOrEmpty(sjsonTTChungTK) Then jTTChungTK = JsonConvert.DeserializeObject(Of clDLCTu.ThongtintokhaiCT)(sjsonTTChungTK)

            If Not String.IsNullOrEmpty(sjsonTTCTS) Then
                dt_cts = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTCTS)
            End If

            If Not String.IsNullOrEmpty(sjsonTTUNhiem) Then
                dt_dsunhiem = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTUNhiem)
            End If

        Catch __unusedJsonSerializationException1__ As JsonSerializationException
            Return "-1"
        End Try

        If jTTChungTK IsNot Nothing AndAlso dt_cts.Rows.Count > 0 Then
            Dim linkelement As String = ""
            Dim doc As XmlDocument = New XmlDocument()
            Dim TKhaiNode As XmlElement = doc.CreateElement("", "TKhai", linkelement)
            doc.AppendChild(TKhaiNode)
            Dim code As String = System.Guid.NewGuid().ToString().Replace("-", "")
            Dim DLTKhaiNode As XmlNode = doc.CreateElement("", "DLTKhai", linkelement)
            Dim productAttribute As XmlAttribute = doc.CreateAttribute("Id")
            productAttribute.Value = "_" & code
            DLTKhaiNode.Attributes.Append(productAttribute)
            TKhaiNode.AppendChild(DLTKhaiNode)
            Dim TTChungNode As XmlNode = doc.CreateElement("", "TTChung", linkelement)
            DLTKhaiNode.AppendChild(TTChungNode)
            Dim PBXMLNode As XmlNode = doc.CreateElement("", "PBan", linkelement)
            PBXMLNode.AppendChild(doc.CreateTextNode(jTTChungTK.PBan))
            TTChungNode.AppendChild(PBXMLNode)
            Dim MSoNode As XmlNode = doc.CreateElement("", "MSo", linkelement)
            MSoNode.AppendChild(doc.CreateTextNode(jTTChungTK.MSo))
            TTChungNode.AppendChild(MSoNode)
            Dim TenNode As XmlNode = doc.CreateElement("", "Ten", linkelement)
            TenNode.AppendChild(doc.CreateTextNode(jTTChungTK.Ten))
            TTChungNode.AppendChild(TenNode)
            Dim HThucNode As XmlNode = doc.CreateElement("", "HThuc", linkelement)
            HThucNode.AppendChild(doc.CreateTextNode(jTTChungTK.HThuc.ToString()))
            TTChungNode.AppendChild(HThucNode)
            Dim TNNTNode As XmlNode = doc.CreateElement("", "TNNT", linkelement)
            TNNTNode.AppendChild(doc.CreateTextNode(jTTChungTK.TNNT))
            TTChungNode.AppendChild(TNNTNode)
            Dim MSTNode As XmlNode = doc.CreateElement("", "MST", linkelement)
            MSTNode.AppendChild(doc.CreateTextNode(jTTChungTK.MST))
            TTChungNode.AppendChild(MSTNode)
            Dim CQTQLyNode As XmlNode = doc.CreateElement("", "CQTQLy", linkelement)
            CQTQLyNode.AppendChild(doc.CreateTextNode(jTTChungTK.CQTQLy))
            TTChungNode.AppendChild(CQTQLyNode)
            Dim MCQTQLyNode As XmlNode = doc.CreateElement("", "MCQTQLy", linkelement)
            MCQTQLyNode.AppendChild(doc.CreateTextNode(jTTChungTK.MCQTQLy))
            TTChungNode.AppendChild(MCQTQLyNode)
            Dim NLHeNode As XmlNode = doc.CreateElement("", "NLHe", linkelement)
            NLHeNode.AppendChild(doc.CreateTextNode(jTTChungTK.NLHe))
            TTChungNode.AppendChild(NLHeNode)
            Dim DCLHeNode As XmlNode = doc.CreateElement("", "DCLHe", linkelement)
            DCLHeNode.AppendChild(doc.CreateTextNode(jTTChungTK.DCLHe))
            TTChungNode.AppendChild(DCLHeNode)
            Dim DCTDTuNode As XmlNode = doc.CreateElement("", "DCTDTu", linkelement)
            DCTDTuNode.AppendChild(doc.CreateTextNode(jTTChungTK.DCTDTu))
            TTChungNode.AppendChild(DCTDTuNode)
            Dim DTLHeNode As XmlNode = doc.CreateElement("", "DTLHe", linkelement)
            DTLHeNode.AppendChild(doc.CreateTextNode(jTTChungTK.DTLHe))
            TTChungNode.AppendChild(DTLHeNode)
            Dim DDanhNode As XmlNode = doc.CreateElement("", "DDanh", linkelement)
            DDanhNode.AppendChild(doc.CreateTextNode(jTTChungTK.DDanh))
            TTChungNode.AppendChild(DDanhNode)
            Dim NLapNode As XmlNode = doc.CreateElement("", "NLap", linkelement)
            NLapNode.AppendChild(doc.CreateTextNode(Thoigianchuan(jTTChungTK.NLap)))
            TTChungNode.AppendChild(NLapNode)
            Dim NDTKhaiNode As XmlNode = doc.CreateElement("", "NDTKhai", linkelement)
            DLTKhaiNode.AppendChild(NDTKhaiNode)
            Dim DTPHanhNode As XmlNode = doc.CreateElement("", "DTPHanh", linkelement)
            NDTKhaiNode.AppendChild(DTPHanhNode)
            Dim TCCNPHanhNode As XmlNode = doc.CreateElement("", "TCCNPHanh", linkelement)
            TCCNPHanhNode.AppendChild(doc.CreateTextNode(jTTChungTK.TCCNPHanh.ToString()))
            DTPHanhNode.AppendChild(TCCNPHanhNode)
            Dim CQTPHanhNode As XmlNode = doc.CreateElement("", "CQTPHanh", linkelement)
            CQTPHanhNode.AppendChild(doc.CreateTextNode(jTTChungTK.CQTPHanh.ToString()))
            DTPHanhNode.AppendChild(CQTPHanhNode)
            Dim LHSDungNode As XmlNode = doc.CreateElement("", "LHSDung", linkelement)
            NDTKhaiNode.AppendChild(LHSDungNode)
            Dim CTTNCNhanNode As XmlNode = doc.CreateElement("", "CTTNCNhan", linkelement)
            CTTNCNhanNode.AppendChild(doc.CreateTextNode(jTTChungTK.CTTNCNhan.ToString()))
            LHSDungNode.AppendChild(CTTNCNhanNode)
            Dim CTKTTTMDTuNode As XmlNode = doc.CreateElement("", "CTKTTTMDTu", linkelement)
            CTKTTTMDTuNode.AppendChild(doc.CreateTextNode(jTTChungTK.CTKTTTMDTu.ToString()))
            LHSDungNode.AppendChild(CTKTTTMDTuNode)
            Dim BLTPLPKInNode As XmlNode = doc.CreateElement("", "BLTPLPKIn", linkelement)
            BLTPLPKInNode.AppendChild(doc.CreateTextNode(jTTChungTK.BLTPLPKIn.ToString()))
            LHSDungNode.AppendChild(BLTPLPKInNode)
            Dim BLTPLPInNode As XmlNode = doc.CreateElement("", "BLTPLPIn", linkelement)
            BLTPLPInNode.AppendChild(doc.CreateTextNode(jTTChungTK.BLTPLPIn.ToString()))
            LHSDungNode.AppendChild(BLTPLPInNode)
            Dim BLTTPLPhiNode As XmlNode = doc.CreateElement("", "BLTTPLPhi", linkelement)
            BLTTPLPhiNode.AppendChild(doc.CreateTextNode(jTTChungTK.BLTTPLPhi.ToString()))
            LHSDungNode.AppendChild(BLTTPLPhiNode)
            Dim HTGDLCTDTNode As XmlNode = doc.CreateElement("", "HTGDLCTDT", linkelement)
            NDTKhaiNode.AppendChild(HTGDLCTDTNode)
            Dim CDLQCCQTNode As XmlNode = doc.CreateElement("", "CDLQCCQT", linkelement)
            CDLQCCQTNode.AppendChild(doc.CreateTextNode(jTTChungTK.CDLQCCQT.ToString()))
            HTGDLCTDTNode.AppendChild(CDLQCCQTNode)
            Dim CDLQTCTNNode As XmlNode = doc.CreateElement("", "CDLQTCTN", linkelement)
            CDLQTCTNNode.AppendChild(doc.CreateTextNode(jTTChungTK.CDLQTCTN.ToString()))
            HTGDLCTDTNode.AppendChild(CDLQTCTNNode)
            Dim CDLQTCTNUTNode As XmlNode = doc.CreateElement("", "CDLQTCTNUT", linkelement)
            CDLQTCTNUTNode.AppendChild(doc.CreateTextNode(jTTChungTK.CDLQTCTNUT.ToString()))
            HTGDLCTDTNode.AppendChild(CDLQTCTNUTNode)
            Dim DSCTSSDungNode As XmlNode = doc.CreateElement("", "DSCTSSDung", linkelement)
            NDTKhaiNode.AppendChild(DSCTSSDungNode)

            If dt_cts.Rows.Count > 0 Then

                For i As Integer = 0 To dt_cts.Rows.Count - 1
                    Dim CTSNode As XmlNode = doc.CreateElement("", "CTS", linkelement)
                    DSCTSSDungNode.AppendChild(CTSNode)
                    Dim STTNode As XmlNode = doc.CreateElement("", "STT", linkelement)
                    STTNode.AppendChild(doc.CreateTextNode(dt_cts.Rows(i)("STT").ToString()))
                    CTSNode.AppendChild(STTNode)
                    Dim TTChucNode As XmlNode = doc.CreateElement("", "TTChuc", linkelement)
                    TTChucNode.AppendChild(doc.CreateTextNode(dt_cts.Rows(i)("TTChuc").ToString()))
                    CTSNode.AppendChild(TTChucNode)
                    Dim SeriNode As XmlNode = doc.CreateElement("", "Seri", linkelement)
                    SeriNode.AppendChild(doc.CreateTextNode(dt_cts.Rows(i)("Seri").ToString()))
                    CTSNode.AppendChild(SeriNode)
                    Dim TNgayNode As XmlNode = doc.CreateElement("", "TNgay", linkelement)
                    TNgayNode.AppendChild(doc.CreateTextNode(Thoigianchuan_datetime(dt_cts.Rows(i)("TNgay").ToString())))
                    CTSNode.AppendChild(TNgayNode)
                    Dim DNgayNode As XmlNode = doc.CreateElement("", "DNgay", linkelement)
                    DNgayNode.AppendChild(doc.CreateTextNode(Thoigianchuan_datetime(dt_cts.Rows(i)("DNgay").ToString())))
                    CTSNode.AppendChild(DNgayNode)
                    Dim HThucCKSNode As XmlNode = doc.CreateElement("", "HThuc", linkelement)
                    HThucCKSNode.AppendChild(doc.CreateTextNode(dt_cts.Rows(i)("HThuc").ToString()))
                    CTSNode.AppendChild(HThucCKSNode)
                Next
            End If

            Dim DSDKUNhiemNode As XmlNode = doc.CreateElement("", "DSDKUNhiem", linkelement)
            NDTKhaiNode.AppendChild(DSDKUNhiemNode)

            If dt_dsunhiem.Rows.Count > 0 Then

                For i As Integer = 0 To dt_dsunhiem.Rows.Count - 1
                    Dim DKUNhiemNode As XmlNode = doc.CreateElement("", "DKUNhiem", linkelement)
                    DSDKUNhiemNode.AppendChild(DKUNhiemNode)
                    Dim LDKUNhiemNode As XmlNode = doc.CreateElement("", "LDKUNhiem", linkelement)
                    LDKUNhiemNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("LDKUNhiem").ToString()))
                    DKUNhiemNode.AppendChild(LDKUNhiemNode)
                    Dim STTUNNode As XmlNode = doc.CreateElement("", "STT", linkelement)
                    STTUNNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("STT").ToString()))
                    DKUNhiemNode.AppendChild(STTUNNode)
                    Dim TLCTuNode As XmlNode = doc.CreateElement("", "TLCTu", linkelement)
                    TLCTuNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("TLCTu").ToString()))
                    DKUNhiemNode.AppendChild(TLCTuNode)
                    Dim KHMCTuNode As XmlNode = doc.CreateElement("", "KHMCTu", linkelement)
                    KHMCTuNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("KHMCTu").ToString()))
                    DKUNhiemNode.AppendChild(KHMCTuNode)
                    Dim KHCTuNode As XmlNode = doc.CreateElement("", "KHCTu", linkelement)
                    KHCTuNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("KHCTu").ToString()))
                    DKUNhiemNode.AppendChild(KHCTuNode)
                    Dim MSTUNNode As XmlNode = doc.CreateElement("", "MST", linkelement)
                    MSTUNNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("MST").ToString()))
                    DKUNhiemNode.AppendChild(MSTUNNode)
                    Dim TTChucNode As XmlNode = doc.CreateElement("", "TTChuc", linkelement)
                    TTChucNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("TTChuc").ToString()))
                    DKUNhiemNode.AppendChild(TTChucNode)
                    Dim MDichNode As XmlNode = doc.CreateElement("", "MDich", linkelement)
                    MDichNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("MDich").ToString()))
                    DKUNhiemNode.AppendChild(MDichNode)
                    Dim TNgayUNNode As XmlNode = doc.CreateElement("", "TNgay", linkelement)
                    TNgayUNNode.AppendChild(doc.CreateTextNode(Thoigianchuan(dt_dsunhiem.Rows(i)("TNgay").ToString())))
                    DKUNhiemNode.AppendChild(TNgayUNNode)
                    Dim DNgayUNNode As XmlNode = doc.CreateElement("", "DNgay", linkelement)
                    DNgayUNNode.AppendChild(doc.CreateTextNode(Thoigianchuan(dt_dsunhiem.Rows(i)("DNgay").ToString())))
                    DKUNhiemNode.AppendChild(DNgayUNNode)
                    Dim PThucNode As XmlNode = doc.CreateElement("", "PThuc", linkelement)
                    PThucNode.AppendChild(doc.CreateTextNode(dt_dsunhiem.Rows(i)("PThuc").ToString()))
                    DKUNhiemNode.AppendChild(PThucNode)
                Next
            End If

            Dim DSCKSNode As XmlNode = doc.CreateElement("", "DSCKS", linkelement)
            TKhaiNode.AppendChild(DSCKSNode)
            Dim CKSNNTNode As XmlNode = doc.CreateElement("", "NNT", linkelement)
            DSCKSNode.AppendChild(CKSNNTNode)
            Dim CCKSKhacNode As XmlNode = doc.CreateElement("", "CCKSKhac", linkelement)
            DSCKSNode.AppendChild(CCKSKhacNode)
            kq = doc.InnerXml
        Else
            kq = "-100"
        End If

        Return kq
    End Function


    Private Function Thoigianchuan_datetime(ByVal thoigian As String) As String
        Dim ngay1 As DateTime = DateTime.Parse(thoigian)
        thoigian = ngay1.ToString("yyyy-MM-ddTHH:mm:ss")
        Return thoigian
    End Function

    Private Function Thoigianchuan(ByVal thoigian As String) As String
        Dim ngay1 As DateTime = DateTime.Parse(thoigian)
        thoigian = ngay1.ToString("yyyy-MM-dd")
        Return thoigian
    End Function

    Private Function Laythongtintokhaict(ByVal MatokhaiCT As Integer, ByVal madonvi As String) As DataTable
        Dim dt As New DataTable("DLTokhaiCT")

        Try
            Using connection As New SqlConnection(connectionString)
                Dim myQuery As String = "SELECT * FROM Tokhaichungtu WHERE MatokhaiCT = @MatokhaiCT AND MST=@MST AND TrangthaicuoiTKCT = 1"

                Using myCommand As New SqlCommand(myQuery, connection)
                    myCommand.Parameters.Add("@MatokhaiCT", SqlDbType.Int).Value = MatokhaiCT
                    myCommand.Parameters.Add("@MST", SqlDbType.NVarChar, 15).Value = madonvi

                    Using adapter As New SqlDataAdapter(myCommand)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Lỗi khi lấy thông tin tờ khai: " & ex.Message, ex)
        Finally
            SqlConnection.ClearAllPools()
        End Try

        Return dt
    End Function

    Private Function LaydanhsachCTS(ByVal MatokhaiCT As Integer) As DataTable
        Dim dt As New DataTable("DLCTS")

        Try
            Using connection As New SqlConnection(connectionString)
                Dim myQuery As String = "SELECT * FROM Tokhaichungtu_DanhsachCTS WHERE MatokhaiCT = @MatokhaiCT"

                Using myCommand As New SqlCommand(myQuery, connection)
                    myCommand.Parameters.Add("@MatokhaiCT", SqlDbType.Int).Value = MatokhaiCT

                    Using adapter As New SqlDataAdapter(myCommand)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Lỗi khi lấy danh sách CTS: " & ex.Message, ex)
        Finally
            SqlConnection.ClearAllPools()
        End Try

        Return dt
    End Function


    <WebMethod()>
    Public Function Laythongtintokhai(ByVal MatokhaiCT As Integer, ByVal madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim dtTokhai As DataTable = Laythongtintokhaict(MatokhaiCT, madonvi)
            Dim dtCTS As DataTable = LaydanhsachCTS(MatokhaiCT)

            If dtTokhai IsNot Nothing AndAlso dtTokhai.Rows.Count > 0 Then
                response("status") = "success"
                response("message") = "Lấy thông tin tờ khai thành công"

                response("data") = New With {
                .tokhai = dtTokhai,
                .danhsachCTS = dtCTS
            }
            Else
                response("status") = "error"
                response("message") = "Không tìm thấy tờ khai"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function




    Private Function CreatFileXML_Thong_diep_den_co_quan_thue(ByVal MNGui As String, ByVal MNNhan As String, ByVal MLTDiep As String, ByVal MTDiep As String, ByVal MTDTChieu As String, ByVal SLuong As String, ByVal MST As String, ByVal strChuoiHoaDon As String) As String
        Dim kq As String = ""
        'Tao thong tin XML chung

        Dim linkelement As String = ""

        Dim doc As XmlDocument = New XmlDocument()
        Dim docNode As XmlNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(docNode)

        'The TDiep
        Dim TDiepNode As XmlElement = doc.CreateElement("", "TDiep", linkelement)
        doc.AppendChild(TDiepNode)
        'TT Chung
        Dim TTChungTDNode As XmlElement = doc.CreateElement("", "TTChung", linkelement)
        TDiepNode.AppendChild(TTChungTDNode)
        'PBan
        Dim PBanTTNode As XmlNode = doc.CreateElement("", "PBan", linkelement)
        PBanTTNode.AppendChild(doc.CreateTextNode("2.1.0"))
        TTChungTDNode.AppendChild(PBanTTNode)
        'MNGui
        Dim MNGuiNode As XmlNode = doc.CreateElement("", "MNGui", linkelement)
        MNGuiNode.AppendChild(doc.CreateTextNode(MNGui))
        TTChungTDNode.AppendChild(MNGuiNode)
        ' MNNhan
        Dim MNNhanNode As XmlNode = doc.CreateElement("", "MNNhan", linkelement)
        MNNhanNode.AppendChild(doc.CreateTextNode(MNNhan))
        TTChungTDNode.AppendChild(MNNhanNode)
        'MLTDiep
        Dim MLTDiepNode As XmlNode = doc.CreateElement("", "MLTDiep", linkelement)
        MLTDiepNode.AppendChild(doc.CreateTextNode(MLTDiep))
        TTChungTDNode.AppendChild(MLTDiepNode)
        'MTDiep
        Dim MTDiepNode As XmlNode = doc.CreateElement("", "MTDiep", linkelement)
        MTDiepNode.AppendChild(doc.CreateTextNode(MTDiep))
        TTChungTDNode.AppendChild(MTDiepNode)
        'MTDTChieu
        Dim MTDTChieuNode As XmlNode = doc.CreateElement("", "MTDTChieu", linkelement)
        MTDTChieuNode.AppendChild(doc.CreateTextNode(MTDTChieu))
        TTChungTDNode.AppendChild(MTDTChieuNode)
        'MST
        Dim MSTTTNode As XmlNode = doc.CreateElement("", "MST", linkelement)
        MSTTTNode.AppendChild(doc.CreateTextNode(MST))
        TTChungTDNode.AppendChild(MSTTTNode)
        'SLuong
        Dim SLuongNode As XmlNode = doc.CreateElement("", "SLuong", linkelement)
        SLuongNode.AppendChild(doc.CreateTextNode(SLuong))
        TTChungTDNode.AppendChild(SLuongNode)
        'DLieu
        Dim DLieuNode As XmlElement = doc.CreateElement("", "DLieu", linkelement)
        TDiepNode.AppendChild(DLieuNode)
        Dim lstNode As XmlNodeList = doc.GetElementsByTagName("DLieu")
        Dim convert = XmlStringToXmlNode(strChuoiHoaDon)

        For i As Integer = 0 To lstNode.Count - 1
            Dim xnode As XmlNode = lstNode(lstNode.Count - 1)
            xnode.AppendChild(xnode.OwnerDocument.ImportNode(convert, True))
        Next

        kq = doc.InnerXml
        Return kq
    End Function

    Private Function XmlStringToXmlNode(ByVal xmlInputString As String) As XmlNode
        Dim rd = Encoding.UTF8.GetString(Convert.FromBase64String(xmlInputString))

        If String.IsNullOrEmpty(xmlInputString.Trim()) Then
            Throw New ArgumentNullException("xmlInputString")
        End If

        Dim xd = New XmlDocument()

        Using sr = New StringReader(rd)
            xd.Load(sr)
        End Using

        Return xd.DocumentElement
    End Function

    Public Function IsBase64String(ByVal base64 As String) As Boolean
        base64 = base64.Trim()
        Return (base64.Length Mod 4 = 0) AndAlso Regex.IsMatch(base64, "^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)
    End Function

    <WebMethod()>
    Public Function GuiToKhaiCQT(Matokhai_CT As Integer, madonvi As String, thongdiep As String) As String
        Dim message As String = String.Empty
        Dim response As New Dictionary(Of String, Object)()

        Try
            If Not String.IsNullOrEmpty(thongdiep) Then
                Dim signText As String = thongdiep
                Dim checkb64 As Boolean = IsBase64String(signText)
                If checkb64 = True Then
                    Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                    Dim key As String = "0103930279" & guidstr.Replace("-", "")
                    Dim xmlthongdiep As String = CreatFileXML_Thong_diep_den_co_quan_thue("0103930279", "0103930279", "109", key, "", "1", madonvi, signText)
                    Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(xmlthongdiep))
                    'Dim instd = InsertThongdiep(base64thongdiep, key, "109", String.Empty, String.Empty, madonvi)

                    ''Gui thong diep 

                    Dim servicetvan As ServiceTVAN.WSInterTRCA2 = New ServiceTVAN.WSInterTRCA2()
                    Dim ttketnoi As New ServiceTVAN.AuthHeader
                    ttketnoi.Username = "ntvan"
                    ttketnoi.Password = "123456"
                    servicetvan.AuthHeaderValue = ttketnoi
                    servicetvan.Timeout = 2147483647
                    Dim macode As String = servicetvan.Guithongdiep(xmlthongdiep, 0)
                    If macode.Length > 10 Then
                        'ins bang nghiep vu to khai, thong tin chi tiet

                        Insert_Thongtintokhai(macode, base64thongdiep, madonvi, Matokhai_CT)
                        'lay ket qua phan hoi tu CQT
                        Dim phanhoi As String = String.Empty
                        While phanhoi = ""
                            Thread.Sleep(10000)
                            phanhoi = servicetvan.LayKQThongdiep(macode, "0103930279")
                        End While

                        If phanhoi = "-1" Then
                            message = "0|Xác thực không đúng"
                        ElseIf phanhoi = "-5" Then
                            message = "0|Mã số thuế trong thông điệp không khớp với tài khoản"
                        ElseIf phanhoi = "-6" Then
                            message = "0|Không có thông điệp nào thỏa mãn"
                        ElseIf phanhoi = "-7" Then
                            message = "1|Chưa có kết quả phản hồi của cơ quan thuế"
                        Else
                            'insert log giai phap nhan ket qua
                            Dim base64phanhoi = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(phanhoi))
                            'boc tach dlieu xml 102
                            UpdateToKhaiSauGui(3, madonvi, Matokhai_CT)

                            'Call Insert_TDiepPhanhoi(macode, phanhoi)
                            If phanhoi.Contains("<THop>1</THop>") Or phanhoi.Contains("<THop>3</THop>") Then
                                message = "1|CQT đã tiếp nhận tờ khai của DN, vui lòng chờ CQT xử lý"
                            Else
                                message = "1|CQT không tiếp nhận tờ khai của DN, vui lòng quay lại danh sách để xem chi tiết."
                            End If
                        End If
                        message = "1|Đã gửi tờ khai lên Cơ quan thuế, vui lòng đợi phản hồi."
                    Else
                        message = "0|Không gửi được tờ khai lên CQT"
                    End If
                Else
                    message = "0|Sai định dạng thông điệp"
                End If

            End If

            Dim parts() As String = message.Split("|"c)

            If parts.Length > 1 Then
                If parts(0) = "1" Then
                    response("status") = "success"
                Else
                    response("status") = "error"
                End If
                response("message") = parts(1)
            Else
                response("status") = "error"
                response("message") = "Lỗi không xác định"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)


    End Function


    Private Sub Insert_Thongtintokhai(khoaphien As String, xmlthongdiep As String, mst As String, matokhai As String)
        Dim conn As New SqlConnection(connectionString)
        Dim cmd As New SqlCommand("update Tokhaichungtu set XMLTokhai=@xmlthongdiep, KhoaphienTN=@khoaphien where MST=@mst and MatokhaiCT=@matokhai", conn)
        Try
            conn.Open()
            cmd.Parameters.AddWithValue("@khoaphien", khoaphien)
            cmd.Parameters.AddWithValue("@xmlthongdiep", xmlthongdiep)
            cmd.Parameters.AddWithValue("@mst", mst)
            cmd.Parameters.AddWithValue("@matokhai", matokhai)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn.State = ConnectionState.Open Then conn.Close()
            conn.Dispose()
            SqlConnection.ClearAllPools()
        End Try
    End Sub


    Private Function InsertThongdiep(xmlthongdiep As String, key As String, mltdiep As String, mtdtchieu As String, khoaphien As String, madonvi As String) As Integer
        Dim res As Integer = 0
        Try
            Dim sql As String = "insert into Logtruyennhan(Phienban,MNGui,MNNhan,MLTDiep,MTDiep,MST,SLuong,XMLThongdiep,Phanloaithongdiep,Thoigian,Trangthai,MTDTChieu,Khoaphien)values ('2.1.0','0103930279','0103930279','" & mltdiep & "','" & key & "','" & madonvi & "','1','" & xmlthongdiep & "','','" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "','1','" & mtdtchieu & "','" & khoaphien & "') "
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            res = 1
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            res = 0
        End Try

        Return res
    End Function

    Private Function UpdateToKhaiSauGui(trangthai As String, mst As String, matokhai As Integer) As Integer
        Dim res As Integer = 0
        Dim conn As New SqlConnection(connectionString)
        Dim cmd As New SqlCommand("update Tokhaichungtu set Trangthai=@trangthai where MST=@mst and MatokhaiCT=@matokhai", conn)
        Try
            conn.Open()
            cmd.Parameters.AddWithValue("@trangthai", trangthai)
            cmd.Parameters.AddWithValue("@mst", mst)
            cmd.Parameters.AddWithValue("@matokhai", matokhai)
            cmd.ExecuteNonQuery()
            res = 1
        Catch ex As Exception
            res = 0
        Finally
            If cmd IsNot Nothing Then cmd.Dispose()
            If conn.State = ConnectionState.Open Then conn.Close()
            conn.Dispose()
            SqlConnection.ClearAllPools()
        End Try
        Return res
    End Function


    <WebMethod()>
    Public Function Laydanhsachtokhaict(madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim sql As String = "SELECT MatokhaiCT, HThuc, NLap, TNNT, Trangthai, khoaphienTN FROM Tokhaichungtu WHERE MST=@MST AND TrangthaicuoiTKCT=1 ORDER BY MatokhaiCT DESC"

            Dim dt As New DataTable("DS")
            Using connection As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, connection)
                    cmd.Parameters.AddWithValue("@MST", madonvi)

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            '--- Tạo list khóa phiên
            Dim listKhoaPhien As New List(Of String)()
            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("khoaphienTN")) AndAlso Not String.IsNullOrWhiteSpace(row("khoaphienTN").ToString()) Then
                    listKhoaPhien.Add(row("khoaphienTN").ToString())
                End If
            Next

            '--- Nối các giá trị thành chuỗi, cách nhau dấu phẩy
            Dim chuoiKhoaPhien As String = String.Join(",", listKhoaPhien)

            '--- Lấy bảng kết quả truyền nhận
            Dim sqtruyennhan As DataTable = LayKQTruyennhantokhaichungtu(madonvi, chuoiKhoaPhien)

            '--- Tạo cột mới cho DataTable nếu chưa có
            If Not dt.Columns.Contains("ketquaphanhoi") Then
                dt.Columns.Add("ketquaphanhoi", GetType(String))
            End If

            '--- Tạo Dictionary để map kết quả
            Dim dictKQ As New Dictionary(Of String, String)()
            For Each row2 As DataRow In sqtruyennhan.Rows
                Dim khoaphien As String = row2("Khoaphien").ToString()
                Dim kq As String = row2("Trangthai").ToString()
                If Not dictKQ.ContainsKey(khoaphien) Then
                    dictKQ.Add(khoaphien, kq)
                End If
            Next

            '--- Gán giá trị vào dt
            For Each row1 As DataRow In dt.Rows
                Dim khoaphien As String = row1("khoaphienTN").ToString()
                If dictKQ.ContainsKey(khoaphien) Then
                    row1("ketquaphanhoi") = dictKQ(khoaphien)
                Else
                    row1("ketquaphanhoi") = ""
                End If
            Next

            '--- Convert DataTable -> List(Of Dictionary)
            Dim dataList As New List(Of Dictionary(Of String, Object))()
            For Each dr As DataRow In dt.Rows
                Dim rowDict As New Dictionary(Of String, Object)()
                For Each col As DataColumn In dt.Columns
                    rowDict(col.ColumnName) = dr(col)
                Next
                dataList.Add(rowDict)
            Next

            response("status") = "success"
            response("message") = "Lấy dữ liệu thành công"
            response("data") = dataList

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    Public Function LayKQTruyennhantokhaichungtu(ByVal MaDV As String, ByVal dskhoaphien As String) As DataTable
        Dim dt As New DataTable("DSHD")
        ' Kiểm tra danh sách khóa phiên rỗng
        If String.IsNullOrWhiteSpace(dskhoaphien) Then Return dt

        ' Tách danh sách khóa phiên thành DataTable
        Dim dtKhoaphien As New DataTable()
        dtKhoaphien.Columns.Add("Khoaphien", GetType(String))
        For Each kp As String In dskhoaphien.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)
            dtKhoaphien.Rows.Add(kp.Trim())
        Next

        Try
            Using connection As New SqlConnection(ConnectionStringtvan)
                connection.Open()

                ' Tạo bảng tạm để chứa danh sách Khoaphien
                Dim createTempTable As String = "CREATE TABLE #tmp_Khoaphien (Khoaphien NVARCHAR(250));"
                Using cmdCreate As New SqlCommand(createTempTable, connection)
                    cmdCreate.ExecuteNonQuery()
                End Using

                ' Bulk insert dữ liệu vào bảng tạm
                Using bulkCopy As New SqlBulkCopy(connection)
                    bulkCopy.DestinationTableName = "#tmp_Khoaphien"
                    bulkCopy.WriteToServer(dtKhoaphien)
                End Using

                ' Truy vấn chính
                Dim myQuery As String = "SELECT " &
                    "a.Khoaphien, " &
                    "b.KQ110, " &
                    "c.KQ111, " &
                    "a.KQ999, " &
                    "CASE " &
                    "WHEN c.KQ111 = 1 THEN N'CQT đã chấp nhận' " &
                    "WHEN b.KQ110 = 2 or b.KQ110 = 4 THEN N'CQT không chấp nhận' " &
                    "WHEN (b.KQ110 = 1 or b.KQ110 = 3) AND c.KQ111 IS NULL THEN N'CQT đã tiếp nhận. Chờ xử lý' " &
                    "WHEN a.KQ999 = 1 AND b.KQ110 IS NULL AND c.KQ111 IS NULL THEN N'CQT đã nhận tờ khai. Chờ phản hồi' " &
                    "ELSE N'Chưa nhận tờ khai' " &
                    "END AS Trangthai " &
                    "FROM ( " &
                    "SELECT " &
                    "Khoaphien, " &
                    "evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep, 'TDiep/DLieu/TBao/TTTNhan') AS KQ999 " &
                    "FROM Logtruyennhan " &
                    "WHERE " &
                    "Khoaphien IN (SELECT Khoaphien FROM #tmp_Khoaphien) " &
                    "AND MNGui = 'TCT' AND MLTDiep = '999' " &
                    ") a " &
                    "LEFT JOIN ( " &
                    "SELECT " &
                    "Khoaphien, " &
                    "evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep, 'TDiep/DLieu/TBao/DLTBao/THop') AS KQ110 " &
                    "FROM Logtruyennhan " &
                    "WHERE " &
                    "Khoaphien IN (SELECT Khoaphien FROM #tmp_Khoaphien) " &
                    "AND MNGui = 'TCT' AND MLTDiep = '110' " &
                    ") b ON a.Khoaphien = b.Khoaphien " &
                    "LEFT JOIN ( " &
                    "SELECT " &
                    "Khoaphien, " &
                    "evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep, 'TDiep/DLieu/TBao/DLTBao/TTXNCQT') AS KQ111 " &
                    "FROM Logtruyennhan " &
                    "WHERE " &
                    "Khoaphien IN (SELECT Khoaphien FROM #tmp_Khoaphien) " &
                    "AND MNGui = 'TCT' AND MLTDiep = '111' " &
                    ") c ON a.Khoaphien = c.Khoaphien " &
                    "ORDER BY a.Khoaphien;"

                Using myCommand As New SqlCommand(myQuery, connection)
                    myCommand.Parameters.Add("@MST", SqlDbType.NVarChar).Value = MaDV

                    Using adapter As New SqlDataAdapter(myCommand)
                        adapter.Fill(dt)
                    End Using
                End Using

                ' Xóa bảng tạm
                Using cmdDrop As New SqlCommand("DROP TABLE #tmp_Khoaphien;", connection)
                    cmdDrop.ExecuteNonQuery()
                End Using

                SqlConnection.ClearAllPools()
            End Using
        Catch ex As Exception
            Dim msg As String = ex.ToString()
            ' Có thể log lỗi ra file / DB nếu cần
        End Try
        XuLyNullTrongDataTable(dt)
        Return dt
    End Function

    <WebMethod()>
    Public Function LayXmlTokhai(mst As String, matokhai As Integer) As String
        Dim response As New Dictionary(Of String, Object)()
        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("SELECT XMLTokhai FROM Tokhaichungtu WHERE MST=@mst AND MatokhaiCT=@matokhai", conn)
            cmd.Parameters.AddWithValue("@mst", mst)
            cmd.Parameters.AddWithValue("@matokhai", matokhai)

            Try
                conn.Open()
                Dim obj As Object = cmd.ExecuteScalar()

                If obj IsNot Nothing AndAlso obj IsNot DBNull.Value Then
                    response("status") = "success"
                    response("message") = "Lấy tờ khai thành công"
                    response("data") = obj.ToString()
                Else
                    response("status") = "error"
                    response("message") = "Không tìm thấy tờ khai"
                End If

            Catch ex As Exception
                response("status") = "error"
                response("message") = "Lỗi hệ thống: " & ex.Message
            End Try
        End Using

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function UpdateToKhaiSauKy(xmlthongdiep As String, trangthai As String, mst As String, matokhai As Integer) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "UPDATE Tokhaichungtu SET XMLTokhai = @xmlthongdiep, Trangthai = @trangthai WHERE MST = @mst AND MatokhaiCT = @matokhai"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.Add("@xmlthongdiep", SqlDbType.NVarChar).Value = xmlthongdiep
                    cmd.Parameters.Add("@trangthai", SqlDbType.NVarChar, 20).Value = trangthai
                    cmd.Parameters.Add("@mst", SqlDbType.NVarChar, 50).Value = mst
                    cmd.Parameters.Add("@matokhai", SqlDbType.Int).Value = matokhai

                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        response("status") = "success"
                        response("message") = "Cập nhật tờ khai thành công"
                    Else
                        response("status") = "error"
                        response("message") = "Không tìm thấy tờ khai để cập nhật"
                    End If
                End Using
            End Using

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    <WebMethod()>
    Public Function ViewThongDiep(xmlthongdiep As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            response("status") = "success"
            response("message") = "Thành công"
            response("data") = System.Xml.Linq.XDocument.Parse(xmlthongdiep).ToString()

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    <WebMethod()>
    Public Function LayDanhSachTruyenNhanChungTu(machungtu As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim chuoiketnoi As String = ConnectionStringtvan
            Dim khoaphien As String = LoadKhoaPhienChungTu(machungtu)

            If String.IsNullOrEmpty(khoaphien) Then
                response("status") = "success"
                response("message") = "Không tìm thấy khoá phiên"

            Else
                Dim dt As DataTable = LoadDataLogTruyennhan(khoaphien, chuoiketnoi)
                response("status") = "success"
                response("message") = "Lấy nhật ký truyền nhận thành công"
                response("data") = dt
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function LayDanhSachTruyenNhanToKhai(matokhaict As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim chuoiketnoi As String = ConnectionStringtvan
            Dim khoaphien As String = LoadKhoaPhienToKhaiCT(matokhaict)

            If String.IsNullOrEmpty(khoaphien) Then
                response("status") = "success"
                response("message") = "Không tìm thấy khoá phiên"

            Else
                Dim dt As DataTable = LoadDataLogTruyennhan(khoaphien, chuoiketnoi)
                response("status") = "success"
                response("message") = "Lấy nhật ký truyền nhận thành công"
                response("data") = dt
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    Private Function LoadKhoaPhienToKhaiCT(matokhaict As String) As String
        Dim res As String = String.Empty
        Try
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "Select KhoaphienTN from Tokhaichungtu where MatokhaiCT = '" & matokhaict & "' and TrangthaicuoiTKCT=1"
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    res = reader(0).ToString
                End While
            Else
                res = String.Empty
            End If
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            Return res
        Catch ex As Exception
            Return res
        End Try
    End Function


    Private Function LoadKhoaPhienChungTu(machungtu As String) As String
        Dim res As String = String.Empty
        Try
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "Select KhoaphienTN from ChungtuthueTNCN where MaCT = '" & machungtu & "' and TrangthaicuoiCT=1"
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    res = reader(0).ToString
                End While
            Else
                res = String.Empty
            End If
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            Return res
        Catch ex As Exception
            Return res
        End Try
    End Function

    Private Function LoadDataLogTruyennhan(khoaphien As String, chuoiketnoi As String) As DataTable
        Dim dt As DataTable = New DataTable("DSTN")

        Try
            Dim servicetvan As ServiceTVAN.WSInterTRCA2 = New ServiceTVAN.WSInterTRCA2()
            Dim ttketnoi As New ServiceTVAN.AuthHeader
            ttketnoi.Username = "ntvan"
            ttketnoi.Password = "123456"
            servicetvan.AuthHeaderValue = ttketnoi
            servicetvan.Timeout = 2147483647

            dt = servicetvan.LayketquathongdiepTQ_khoaphien(khoaphien, "0103930279")

            If Not dt.Columns.Contains("Trangthai") Then
                dt.Columns.Add("Trangthai", GetType(String))
            End If

            Dim xmldoc As New XmlDocument

            For Each row As DataRow In dt.Rows
                Dim xml = Encoding.UTF8.GetString(Convert.FromBase64String(row("XMLThongdiep").ToString()))

                If Not IsDBNull(row("MLTDiep")) AndAlso row("MLTDiep").ToString() = "110" Then
                    Dim chuoiMTa As String = LayDanhSachMTa(xml)

                    If String.IsNullOrWhiteSpace(chuoiMTa) Then
                        row("Trangthai") = ""
                    Else
                        row("Trangthai") = "CQT từ chối: " & chuoiMTa
                    End If
                End If


                If Not IsDBNull(row("MLTDiep")) AndAlso row("MLTDiep").ToString() = "999" Then
                    If xml.Contains("<TTTNhan>1</TTTNhan>") Then
                        ''--lỗi thông điệp
                        xmldoc.LoadXml(xml)
                        Dim element As XmlElement
                        element = TryCast(xmldoc.GetElementsByTagName("MTa")(0), XmlElement)
                        row("Trangthai") = "Lỗi dữ liệu thông điệp: " & element.InnerText
                    Else
                        row("Trangthai") = "thông điệp hợp lệ"
                    End If
                End If

            Next

            Return dt
        Catch ex As Exception
            Return dt
        End Try
    End Function

    Private Function LayXMLThongdiep(ByVal idtruyennhan As String) As String
        Dim conn As SqlConnection = New SqlConnection()
        Dim comm As SqlCommand = New SqlCommand()
        conn.ConnectionString = ConnectionStringtvan
        conn.Open()
        comm.Connection = conn
        Dim result As String = String.Empty
        comm.CommandText = "Select  XMLThongdiep from Logtruyennhan where idTruyennhan = '" & idtruyennhan & "'"
        Dim reader As SqlDataReader = comm.ExecuteReader()

        If reader.HasRows Then

            While reader.Read()

                If reader(0) IsNot DBNull.Value Then
                    result = reader(0).ToString()
                    result = Encoding.UTF8.GetString(Convert.FromBase64String(result))
                Else
                    result = String.Empty
                End If
            End While
        Else
            result = String.Empty
        End If

        reader.Close()
        conn.Close()
        comm.Dispose()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return result
    End Function

    Private Function LayDanhSachMTa(xmlChuoi As String) As String
        Try
            Dim doc As New XmlDocument()
            doc.LoadXml(xmlChuoi)

            Dim nodeList As XmlNodeList = doc.SelectNodes("//DSLDKCNhan/LDo/MTa")
            Dim danhSachMTa As New List(Of String)

            For Each node As XmlNode In nodeList
                If node IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(node.InnerText) Then
                    danhSachMTa.Add(node.InnerText.Trim())
                End If
            Next

            Return String.Join("; ", danhSachMTa)
        Catch ex As Exception
            ' Xử lý lỗi nếu XML không hợp lệ
            Return ""
        End Try
    End Function


    <WebMethod()>
    Public Function LayDanhSachMau(madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim sql As String = "SELECT * FROM mauhoadon WHERE MaDV = @madonvi ORDER BY ThoigianPH DESC"

            Dim dt As New DataTable("DS")
            Using connection As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, connection)
                    cmd.Parameters.AddWithValue("@madonvi", madonvi)

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using


            response("status") = "success"
            response("data") = dt
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function GetTemplateFiles(ByVal folderName As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            If String.IsNullOrEmpty(folderName) Then
                folderName = ""
            End If

            Dim lst As New List(Of mauhienthi)

            Dim basePath As String = HttpContext.Current.Server.MapPath("~/")

            Dim srcpath As String = basePath
            If Not String.IsNullOrEmpty(folderName) Then
                srcpath = Path.Combine(srcpath, folderName.Replace("/", ""))
            End If


            Dim di As New DirectoryInfo(srcpath)
            If di.Exists Then
                For Each fri As FileInfo In di.GetFiles("*.xslt")
                    Dim item As New mauhienthi
                    item.name = fri.Name
                    item.Filepath = fri.FullName
                    lst.Add(item)
                Next
            End If

            response("status") = "success"
            response("data") = lst
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    Private Function CreatFileXMLCT_Mau70(Tinhchat As String, TenCT As String, MSCTu As String, KHCTu As String, SCTu As String, NLap As String, mstnbh As String, tennbh As String, DiachiNBH As String, DienthoaiNBH As String) As String
        'New XAttribute("Id", "_" & idct),

        Dim base64chungthu As String = String.Empty
        Dim doc As New XDocument(
           New XDeclaration("1.0", "utf-8", "yes"),
           New XElement("CTu",
               New XElement("DLCTu",
                   New XElement("TTChung",
                       New XElement("PBan", "2.1.0"),
                       New XElement("TCTu", TenCT),
                       New XElement("MSCTu", MSCTu),
                       New XElement("KHCTu", KHCTu),
                       New XElement("SCTu", SCTu),
                       New XElement("NLap", NLap),
                       New XElement("TTKhac",
                           New XElement("TTin",
                               New XElement("TTruong", "MTCuu"),
                               New XElement("KDLieu", "string")
                            )
                       )
                   ),
                   New XElement("NDCTu",
                       New XElement("TCTTNhap",
                           New XElement("Ten", mstnbh),
                           New XElement("MST", tennbh),
                           New XElement("DChi", DiachiNBH),
                           New XElement("SDThoai", DienthoaiNBH)
                       ),
                       New XElement("NNT",
                           New XElement("Ten", ""),
                           New XElement("MST", ""),
                           New XElement("DChi", ""),
                           New XElement("QTich", ""),
                           New XElement("CNCTru", 0),
                           New XElement("CCCDan", ""),
                            New XElement("SDThoai", ""),
                           New XElement("DCTDTu", "")
                       ),
                       New XElement("TTNCNKTru",
                           New XElement("KTNhap", ""),
                           New XElement("TThang", ""),
                           New XElement("DThang", ""),
                           New XElement("Nam", ""),
                           New XElement("BHiem", ""),
                           New XElement("TThien", ""),
                           New XElement("TTNCThue", ""),
                           New XElement("TTNTThue", ""),
                           New XElement("SThue", "")
                       )
                   )
               ),
               New XElement("DSCKS",
                   New XElement("TCTTNhap")
)
)
               )

        Dim byte1 As Byte() = Text.Encoding.UTF8.GetBytes(doc.ToString)
        base64chungthu = Convert.ToBase64String(byte1)
        Return base64chungthu
    End Function

    <WebMethod()>
    Public Function XemTruocMau(
            loaichungtu As String,
            id_chitiet As String,
            mauhienthi As String,
            madonvi As String,
            tenchungtu As String,
            ctbg As String,
            ctlogo As String
        ) As String

        Dim response As New Dictionary(Of String, Object)()

        Try
            If Not String.IsNullOrEmpty(id_chitiet) Then
                Dim res As String
                Dim madv As String = madonvi
                Dim dv As DonViInfo = GetThongTinDonVi(madv)

                ' Thông tin đơn vị
                Dim DiachiNBH As String = dv.DiaChi
                Dim DienthoaiNBH As String = dv.DienThoai
                Dim tendv As String = dv.TenDV
                Dim FaxNBH As String = dv.Fax
                Dim NganhangNBH As String = dv.NganHang
                Dim SotaikhoanNBH As String = dv.STK
                Dim Email As String = dv.Email
                Dim mstnbh As String = dv.MaDV
                Dim tennbh As String = dv.TenDV
                Dim xmlValue As String = String.Empty

                ' Xử lý theo loại chứng từ
                xmlValue = CreatFileXMLCT_Mau70(0, "Chứng từ khấu trừ thuế thu nhập cá nhân", "03/TNCN", "CT/25E", "0000000", Now.ToString("yyyy-MM-dd"), mstnbh, tennbh, DiachiNBH, DienthoaiNBH)

                Dim xml As Byte() = Convert.FromBase64String(xmlValue)

                ' Render với XSLT
                Dim xsltPath As String = mauhienthi
                res = GetHtmlMauChungTu(xsltPath, xml, madonvi, 0, loaichungtu, ctbg, ctlogo)
                res = res.Replace("NaN", "")

                ' Trả về JSON
                response("status") = "success"
                response("data") = res
            Else
                response("status") = "error"
                response("message") = "Thiếu id_chitiet"
            End If
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return Newtonsoft.Json.JsonConvert.SerializeObject(response)
    End Function

    Public Function GetHtmlMauChungTu(xsltPath As String, xml As Byte(), madv As String, type As Integer, mauso As String, ctbg As String, ctlogo As String) As String
        Dim stream As New MemoryStream(xml)
        Dim document As New XPathDocument(stream)
        Dim writer As New StringWriter()
        Dim argList As New XsltArgumentList()

        ' logo và background ở dạng base64
        Dim logob64 As String = String.Empty
        Dim bgb64 As String = String.Empty

        ' Logo
        If Not String.IsNullOrEmpty(ctlogo) Then
            logob64 = GetBase64DataUri(ctlogo)
        Else
            logob64 = GetBase64DataUri("UploadIMG/logo/blank.png")
        End If

        ' Background
        If Not String.IsNullOrEmpty(ctbg) Then
            bgb64 = GetBase64DataUri(ctbg)
        Else
            bgb64 = GetBase64DataUri("UploadIMG/bg/blank.png")
        End If

        Dim oldfile As String = xsltPath
        Dim destfile As String = HttpContext.Current.Server.MapPath("~/xml/" & madv & "_" & "view.xslt")

        If type = 0 Then
            Dim stylemau As String = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;" &
                                 "background:transparent;display:block;top:45%;left:40%;color:red;" &
                                 "font-size:70pt;text-align:center;padding-top:10px;"

            Dim filename As String = Left(Path.GetFileNameWithoutExtension(xsltPath), 1)
            Dim bgstyle As String

            If filename = "B" Then
                bgstyle = "width:900px;margin:auto; padding-top:20px;z-index:1;" &
                      "background-image: url('" & bgb64 & "'); background-size:80%; background-position: center;" &
                      "background-repeat:no-repeat"
            Else
                bgstyle = "width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;" &
                      "background-image: url('" & bgb64 & "'); background-size:80%; background-position: center;" &
                      "background-repeat:no-repeat"
            End If

            Dim paramsubtitle As String = "none"
            Dim paramsubtitlecontent As String = String.Empty
            Dim paramSubtitleDiv As String = "none"
            Dim paramSubtitleContentDiv As String = "&#160;"

            Dim styledisabled As String = "position:absolute;z-index:0;width:300px;height:140px;" &
                                      "border:5px solid red;background:transparent;display:none;top:45%;" &
                                      "left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;"
            Dim noidungdisabled As String = "&#160;"


            ' Thay thế trong XSLT
            Dim fileReader As String = My.Computer.FileSystem.ReadAllText(oldfile).
            Replace("viewstyle", bgstyle).
            Replace("paramLogo", logob64).
            Replace("paramChuyendoi", "display:normal").
            Replace("paramSign", "display:normal").
            Replace("paramMau", stylemau).
            Replace("paramNguoiCD", "width:100%;text-align:center;display:normal").
            Replace("paramdisable", styledisabled).
            Replace("contentDisable", noidungdisabled).
            Replace("param1_1", paramsubtitle).
            Replace("param1", paramsubtitlecontent).
            Replace("param2_2", paramSubtitleDiv).
            Replace("param2", paramSubtitleContentDiv).
            Replace("paramlien", "0").
            Replace("paramdisplay", "display:none")

            My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
        End If

        ' Biến đổi XSLT
        Dim transform As New XslCompiledTransform()
        transform.Load(destfile, New XsltSettings(True, True), New XmlUrlResolver())
        transform.Transform(document, argList, writer)


        Return writer.ToString()
    End Function


    Private Function GetBase64DataUri(filePath As String) As String
        Dim absPath As String = HttpContext.Current.Server.MapPath("~/" & filePath)
        If Not File.Exists(absPath) Then
            Return ""
        End If

        Dim ext As String = Path.GetExtension(absPath).ToLower()
        Dim mimeType As String = "application/octet-stream"

        Select Case ext
            Case ".png"
                mimeType = "image/png"
            Case ".jpg", ".jpeg"
                mimeType = "image/jpeg"
            Case ".gif"
                mimeType = "image/gif"
            Case ".bmp"
                mimeType = "image/bmp"
            Case ".svg"
                mimeType = "image/svg+xml"
        End Select

        Dim fileBytes As Byte() = File.ReadAllBytes(absPath)
        Return "data:" & mimeType & ";base64," & Convert.ToBase64String(fileBytes)
    End Function

    Private Function GetLogoPath(madv As String, mauso As String) As String
        Dim res As String = String.Empty
        Dim conn As New SqlConnection(connectionString)
        conn.Open()
        Dim sql_ph As String = "select logo_path, watermark_path, xslt_path, is_show_wattermark_inner_table from mau_hoa_don where loai_hoa_don_ct_template_id='" + mauso + "' and donvi_ma_dv='" + madv + "'"
        Dim comm_ph As New SqlCommand(sql_ph, conn)
        Dim reader As SqlDataReader = comm_ph.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                res = reader("logo_path").ToString
                Exit While
            End While
        Else
            res = String.Empty
        End If
        reader.Close()
        conn.Close()
        conn.Dispose()
        comm_ph.Dispose()
        Return res
    End Function


    Private Function GetLogoPathChungTu(madv As String, mauso As String) As String
        Dim res As String = String.Empty
        Dim conn As New SqlConnection(connectionString)
        conn.Open()
        Dim sql_ph As String = "select logo from Mauhoadon where Mauso='" + mauso + "' and MaDV='" + madv + "' order by idMauHD desc"
        Dim comm_ph As New SqlCommand(sql_ph, conn)
        Dim reader As SqlDataReader = comm_ph.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                res = reader("logo").ToString
                Exit While
            End While
        Else
            res = String.Empty
        End If
        reader.Close()
        conn.Close()
        conn.Dispose()
        comm_ph.Dispose()
        Return res
    End Function






    Public Shared Function GetRandom() As String
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 8
            Dim idx As Integer = r.Next(0, 35)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString
    End Function


    <WebMethod()>
    Public Function TaoMauChungTu(
        NgayQD As String,
        khmauso As String,
        tenhd As String,
        madonvi As String,
        ctbg As String,
        ctlogo As String,
        cmbTemplateText As String,
        cmbTemplateValue As String,
        loaichungtu As String,
        soquyetdinh As String
    ) As String

        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim ThoigianPH = Now.Date


            ' ==== Copy Logo & Background ====
            Dim returnlogo, returnbg As String
            returnlogo = String.Empty
            returnbg = String.Empty


            'returnlogo = resizeImage(filePath, 0, hdMaDV("value"))
            If File.Exists(Server.MapPath(ctlogo)) Then
                Dim fname As String = Path.GetFileName(Server.MapPath(ctlogo))
                File.Copy(Server.MapPath(ctlogo), Server.MapPath("~/UploadIMG/logo/" & madonvi & "_" & fname), True)
                returnlogo = "logo/" & madonvi & "_" & fname
            Else
                returnlogo = "logo/" & "blank.png"
            End If


            If File.Exists(Server.MapPath(ctbg)) Then
                Dim fname As String = Path.GetFileName(Server.MapPath(ctbg))
                File.Copy(Server.MapPath(ctbg), Server.MapPath("~/UploadIMG/bg/" & madonvi & "_" & fname), True)
                returnbg = "bg/" & madonvi & "_" & fname
            Else
                returnbg = "logo/" & "blank.png"
            End If

            ' ==== Xử lý file XSLT ====
            Dim templatexslt As String = cmbTemplateValue
            Dim checkborder As String = Left(Path.GetFileNameWithoutExtension(templatexslt), 1)
            Dim newfile As String

            If checkborder = "B" Then
                newfile = Server.MapPath(String.Format("~/xml/xslt/B_{0}_{1}_{2}", madonvi, GetRandom(), cmbTemplateText))
            Else
                newfile = Server.MapPath(String.Format("~/xml/xslt/{0}_{1}_{2}", madonvi, GetRandom(), cmbTemplateText))
            End If

            If File.Exists(templatexslt) Then
                File.Copy(templatexslt, newfile, True)
            End If

            ' ==== Insert DB ====
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim sqlIns As String =
                    "INSERT INTO mauhoadon " &
                    "(MaDV, MaloaiHD, Mauso, ThoigianPH, SoQD, NgayQD, Logo, Nen, Filexslt, BGType, Innhieutrang, Sohhtrentrang, idLoaiHD_CT, TenHD) " &
                    "VALUES " &
                    "(@Madv, @maloaihd, @Mauso, @thoigianph, @soqd, @ngayqd, @logo, @Nen, @Filexslt, @BGType, @Innhieutrang, @Sohhtrentrang, @idLoaiHD_CT, @TenHD)"


                Using comm As New SqlCommand(sqlIns, conn)
                    comm.Parameters.AddWithValue("@Madv", madonvi)
                    comm.Parameters.AddWithValue("@maloaihd", loaichungtu)
                    comm.Parameters.AddWithValue("@Mauso", khmauso)
                    comm.Parameters.AddWithValue("@thoigianph", ThoigianPH.ToString("yyyy-MM-dd"))
                    comm.Parameters.AddWithValue("@soqd", soquyetdinh)
                    comm.Parameters.AddWithValue("@ngayqd", NgayQD)
                    comm.Parameters.AddWithValue("@logo", "UploadIMG/" & returnlogo)
                    comm.Parameters.AddWithValue("@Nen", "UploadIMG/" & returnbg)
                    comm.Parameters.AddWithValue("@Filexslt", newfile)
                    comm.Parameters.AddWithValue("@BGType", 0)
                    comm.Parameters.AddWithValue("@Innhieutrang", 1)
                    comm.Parameters.AddWithValue("@Sohhtrentrang", 10)
                    comm.Parameters.AddWithValue("@idLoaiHD_CT", 14) ' hardcode
                    comm.Parameters.AddWithValue("@TenHD", "Chứng từ khấu trừ thuế thu nhập cá nhân")
                    comm.ExecuteNonQuery()
                End Using
            End Using

            response("status") = "success"
            response("message") = "Tạo mẫu chứng từ thành công"
            response("data") = New With {
            .logo = returnlogo,
            .bg = returnbg,
            .xslt = newfile
        }

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    <WebMethod()>
    Public Function SuaMauChungTu(
        madonvi As String,
        ctbg As String,
        ctlogo As String,
        soquyetdinh As String,
        NgayQD As String,
        idmau As String
    ) As String

        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim ThoigianPH = Now.Date

            ' ==== Copy Logo & Background ====
            Dim returnlogo, returnbg As String
            returnlogo = String.Empty
            returnbg = String.Empty


            If File.Exists(Server.MapPath(ctlogo)) Then
                Dim fname As String = Path.GetFileName(Server.MapPath(ctlogo))
                File.Copy(Server.MapPath(ctlogo), Server.MapPath("~/UploadIMG/logo/" & madonvi & "_" & fname), True)
                returnlogo = "logo/" & madonvi & "_" & fname
            Else
                returnlogo = "logo/" & "blank.png"
            End If


            If File.Exists(Server.MapPath(ctbg)) Then
                Dim fname As String = Path.GetFileName(Server.MapPath(ctbg))
                File.Copy(Server.MapPath(ctbg), Server.MapPath("~/UploadIMG/bg/" & madonvi & "_" & fname), True)
                returnbg = "bg/" & madonvi & "_" & fname
            Else
                returnbg = "logo/" & "blank.png"
            End If

            ' ==== Build dynamic SQL ====
            Dim sql As String = "UPDATE mauhoadon SET SoQD=@SoQD, NgayQD=@NgayQD"
            If Not String.IsNullOrEmpty(ctlogo) Then sql &= ", Logo=@Logo"
            If Not String.IsNullOrEmpty(ctbg) Then sql &= ", Nen=@Nen"
            sql &= " WHERE idMauHD=@idMauHD"

            ' ==== Update DB ====
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using comm As New SqlCommand(sql, conn)
                    comm.Parameters.AddWithValue("@SoQD", soquyetdinh)
                    comm.Parameters.AddWithValue("@NgayQD", NgayQD)
                    comm.Parameters.AddWithValue("@idMauHD", idmau)

                    If Not String.IsNullOrEmpty(ctlogo) Then
                        comm.Parameters.AddWithValue("@Logo", "UploadIMG/" & returnlogo)
                    End If
                    If Not String.IsNullOrEmpty(ctbg) Then
                        comm.Parameters.AddWithValue("@Nen", "UploadIMG/" & returnbg)
                    End If

                    comm.ExecuteNonQuery()
                End Using
            End Using


            response("status") = "success"
            response("message") = "Cập nhật mẫu chứng từ thành công"
            response("data") = New With {
            .logo = returnlogo,
            .bg = returnbg
        }

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function




    <WebMethod()>
    Public Function UploadLogoMau(madonvi As String, base64File As String, fileName As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim tempFolder As String = "~/temp/" & madonvi & "_logo/"
            Dim savePath As String = HttpContext.Current.Server.MapPath(tempFolder)
            If Not Directory.Exists(savePath) Then
                Directory.CreateDirectory(savePath)
            End If

            ' Giải mã base64
            Dim fileBytes As Byte() = Convert.FromBase64String(base64File)

            ' Tạo tên file an toàn
            Dim safeFileName As String = Path.GetFileName(fileName)
            Dim fullPath As String = Path.Combine(savePath, safeFileName)

            File.WriteAllBytes(fullPath, fileBytes)

            response("status") = "success"
            response("message") = "Upload logo thành công"
            response("data") = "temp/" & madonvi & "_logo/" & safeFileName
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi upload logo: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function UploadBgMau(madonvi As String, base64File As String, fileName As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim tempFolder As String = "~/temp/" & madonvi & "_bg/"
            Dim savePath As String = HttpContext.Current.Server.MapPath(tempFolder)
            If Not Directory.Exists(savePath) Then
                Directory.CreateDirectory(savePath)
            End If

            ' Giải mã base64
            Dim fileBytes As Byte() = Convert.FromBase64String(base64File)

            ' Tạo tên file an toàn
            Dim safeFileName As String = Path.GetFileName(fileName)
            Dim fullPath As String = Path.Combine(savePath, safeFileName)

            File.WriteAllBytes(fullPath, fileBytes)

            response("status") = "success"
            response("message") = "Upload background thành công"
            response("data") = "temp/" & madonvi & "_bg/" & safeFileName
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi upload background: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    Private Function GetThongTinDonVi(madv As String) As DonViInfo
        Dim dv As New DonViInfo()

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim sql As String = "SELECT ma_dv, ten_dv, dia_chi, dien_thoai, fax, ngan_hang, stk, email  FROM donvi WHERE ma_dv = @MaDV"
            Using comm As New SqlCommand(sql, conn)
                comm.Parameters.AddWithValue("@MaDV", madv)
                Using reader As SqlDataReader = comm.ExecuteReader()
                    If reader.Read() Then
                        dv.MaDV = If(reader("ma_dv") Is DBNull.Value, "", reader("ma_dv").ToString())
                        dv.TenDV = If(reader("ten_dv") Is DBNull.Value, "", reader("ten_dv").ToString())
                        dv.DiaChi = If(reader("dia_chi") Is DBNull.Value, "", reader("dia_chi").ToString())
                        dv.DienThoai = If(reader("dien_thoai") Is DBNull.Value, "", reader("dien_thoai").ToString())
                        dv.Fax = If(reader("fax") Is DBNull.Value, "", reader("fax").ToString())
                        dv.NganHang = If(reader("ngan_hang") Is DBNull.Value, "", reader("ngan_hang").ToString())
                        dv.STK = If(reader("stk") Is DBNull.Value, "", reader("stk").ToString())
                        dv.Email = If(reader("email") Is DBNull.Value, "", reader("email").ToString())
                    End If
                End Using
            End Using
        End Using

        Return dv
    End Function

    'Đăng ký phát hành chứng từ
    <WebMethod()>
    Public Function DangKyPhatHanhChungTu(madonvi As String, kyhieuct As String, mausoct As String, sobatdau As String, soketthuc As String, ngaysudung As String, loaict As String, userid As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Dim checkmauso As String = CheckMauHoaDon(madonvi)
        Dim namht As String = Right(Now.Year, 2)
        mausoct = "03/TNCN"
        kyhieuct = "CT/" & namht & "E"

        If checkmauso = False Then
            response("status") = "error"
            response("message") = "Chưa có mẫu chứng từ"
            Return JsonConvert.SerializeObject(response)
        End If

        Dim check As Boolean = False
        Dim soctkt As Integer = CheckKyhieu(madonvi, mausoct, kyhieuct)

        If soctkt > 0 Then
            'txtSohdBD.Text = sohdkt + 1
            'txtSohdBD.ReadOnly = True
            check = True

        Else
            check = True
            ' txtSohdBD.Text = 1
            'sohoadonbd = txtSohdBD.Text
            'txtSohdBD.ReadOnly = True
            'thoa man dk/ chua co ky hieu trong bang phat hanh
        End If

        If check = True Then
            'Dim ngayht As String = Now.Date
            'Kiem tra ngay su dung khong duoc phep nho hon ngay phat hanh hoa don (Trong bang mauhoadon)
            Dim NgayQDinh As String = GetThoigianph(madonvi, mausoct)
            If NgayQDinh <> "0" Then
                If DateDiff("d", NgayQDinh, ngaysudung) < 0 Then
                    response("status") = "error"
                    response("message") = "Ngày sử dụng phải lớn hơn hoặc bằng ngày phát hành mẫu hoá đơn"
                Else
                    Dim thongtinphathanh As String = GetthongtinPH(madonvi, mausoct, kyhieuct, ngaysudung, sobatdau, soketthuc, loaict, userid)

                    Dim parts() As String = thongtinphathanh.Split("|"c)

                    If parts.Length > 1 Then
                        If parts(0) = "1" Then
                            response("status") = "success"
                        Else
                            response("status") = "error"
                        End If
                        response("message") = parts(1)
                    Else
                        response("status") = "error"
                        response("message") = "Kết quả không hợp lệ: " & thongtinphathanh
                    End If
                End If
            Else
                response("status") = "error"
                response("message") = "Ngày phát hành không hợp lệ."
            End If
        Else
            response("status") = "error"
            response("message") = "Lỗi hệ thống"
        End If


        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function Capnhatphathanhchungtu(soluongmoi As String, madonvi As String, idphathanh As String, kyhieu As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                Dim sql As String = "Update hoa_don_dang_ky_phat_hanh set so_luong = @soluongmoi, so_ket_thuc=@soluongmoi where id=@idphathanh and ky_hieu=@kyhieu and donvi_ma_dv=@madonvi"
                Using comm As New SqlCommand(sql, conn)
                    comm.Parameters.AddWithValue("@soluongmoi", soluongmoi)
                    comm.Parameters.AddWithValue("@madonvi", madonvi)
                    comm.Parameters.AddWithValue("@idphathanh", idphathanh)
                    comm.Parameters.AddWithValue("@kyhieu", kyhieu)

                    Dim rowsAffected As Integer = comm.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        response("status") = "success"
                        response("message") = "Cập nhật thành công"
                    Else
                        response("status") = "error"
                        response("message") = "Cập nhật không thành công"
                    End If
                End Using
            End Using

            SqlConnection.ClearAllPools()
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function Xoaphathanhchungtu(idphathanh As String, madonvi As String, mauso As String, kyhieu As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim checkph As Boolean = CheckIdHDPH(idphathanh)
            If checkph = True Then
                Dim checkhd As Boolean = checkChungtudaviet(madonvi, mauso, kyhieu)
                If checkhd = False Then
                    'del thong bao phat hanh
                    Dim res As Integer = DelHoadonph(idphathanh, madonvi)
                    If res = 1 Then
                        response("status") = "success"
                        response("message") = "Xóa thông tin phát hành chứng từ thành công"
                    Else
                        response("status") = "error"
                        response("message") = "Xóa thông tin phát hành chứng từ không thành công"
                    End If

                Else
                    response("status") = "error"
                    response("message") = "Lô phát hành đã xuất chứng từ. Không thể xóa"
                End If
            Else
                response("status") = "error"
                response("message") = "Không tìm thấy thông tin phát hành chứng từ"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    Private Function DelHoadonph(idhdph As String, madonvi As String) As Integer
        Dim res As Integer = 0
        Dim sql As String = "update hoa_don_dang_ky_phat_hanh set is_deleted=1 where donvi_ma_dv='" & madonvi & "' and  id=" & idhdph & ""
        Dim conn As SqlConnection = New SqlConnection(connectionString)
        conn.Open()
        Dim comm As New SqlCommand(sql, conn)
        comm.ExecuteNonQuery()
        res = 1
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return res
    End Function

    Private Function checkChungtudaviet(madonvi As String, mauso As String, kyhieu As String) As Boolean
        Dim sql As String = "select COUNT(MaCT) from ChungtuthueTNCN where MasothueTC='" & madonvi & "' and  MSChungtu='" & mauso & "' and KHChungtu='" & kyhieu & "' and TrangthaicuoiCT=1"
        Dim conn As SqlConnection = New SqlConnection(connectionString)
        conn.Open()
        Dim comm As New SqlCommand(sql, conn)
        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                If reader(0) IsNot DBNull.Value Then
                    If reader(0) > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End While
        Else
            Return False
        End If
        reader.Close()
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
    End Function


    Private Function CheckIdHDPH(idph As String) As Boolean
        Dim check As Boolean = False
        Dim conn As New SqlConnection(connectionString)
        conn.Open()
        Dim comm As New SqlCommand
        comm.Connection = conn
        comm.CommandText = "Select * from hoa_don_dang_ky_phat_hanh where id=@idph"
        comm.Parameters.AddWithValue("@idph", idph)

        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            check = True
        Else
            check = False
        End If

        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return check
    End Function


    Private Function CheckKyhieu(donvi_ma_dv As String, mau_so As String, ky_hieu As String) As Integer
        Dim res = 0
        Dim conn As New SqlConnection(connectionString)
        conn.Open()
        Dim comm As New SqlCommand
        comm.Connection = conn
        comm.CommandText = "Select so_ket_thuc from hoa_don_dang_ky_phat_hanh where donvi_ma_dv=@donvi_ma_dv and mau_so=@mau_so and ky_hieu=@ky_hieu"
        comm.Parameters.AddWithValue("@donvi_ma_dv", donvi_ma_dv)
        comm.Parameters.AddWithValue("@mau_so", mau_so)
        comm.Parameters.AddWithValue("@ky_hieu", ky_hieu)
        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                res = Convert.ToInt32(reader(0))
            End While
            reader.Close()
        Else
            res = 0
        End If
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return res
    End Function


    Private Function GetthongtinPH(macn As String, mauso As String, kyhieu As String, ngaysd As String, sohoadonbd As String, sohoadonkt As String, loaihd As String, userid As String) As String
        Dim result As String = String.Empty
        Dim idchitiet, tenhd As String
        'idchitiet = getIDChitiet(mauso, loaihd)
        idchitiet = 14
        tenhd = loaihd
        Try
            Dim sql_ph As String = "SELECT TOP 1 ngay_su_dung AS ngayphgannhat, so_ket_thuc, so_bat_dau " &
                       "FROM hoa_don_dang_ky_phat_hanh INNER JOIN donvi ON hoa_don_dang_ky_phat_hanh.donvi_ma_dv = donvi.ma_dv " &
                       "WHERE donvi.ma_dv = '" & macn & "' " &
                       "AND hoa_don_dang_ky_phat_hanh.mau_so = '" & mauso & "' " &
                       "AND hoa_don_dang_ky_phat_hanh.ky_hieu = '" & kyhieu & "' " &
                       "AND hoa_don_dang_ky_phat_hanh.hoa_don_dang_ky_phat_hanh_trang_thai_id = 1 " &
                       "ORDER BY hoa_don_dang_ky_phat_hanh.ngay_su_dung DESC"

            Dim conn As New SqlConnection(connectionString)
            conn.Open()

            Dim comm_ph As New SqlCommand(sql_ph, conn)
            Dim reader As SqlDataReader = comm_ph.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    If Convert.ToDateTime(ngaysd) <= Convert.ToDateTime(reader("ngayphgannhat")) Then
                        result = "0|Ngày sử dụng phải lớn hơn ngày sử dụng của lần phát hành gần nhất"
                    Else
                        Dim sohdkt As String = reader("so_ket_thuc").ToString
                        'If CLng(sohoadonbd) <= CLng(sohdkt) Then
                        If CLng(sohoadonbd) <= CLng(sohdkt) Then
                            result = "0|Số chứng từ bắt đầu phải lớn hơn số chứng từ kết thúc của lần phát hành gần nhất"
                        Else
                            'result = kiemtrahuyloph(mauso, kyhieu, macn, sohoadonbd, reader("ngayphgannhat").ToString, sohoadonkt, ngaysd)
                            'If result = "1" Then
                            '    'lo cu da su dung het hoac huy, duoc phep phat hanh lo moi
                            '    If CLng(sohoadonkt) <= CLng(sohoadonbd) Then
                            '        result = "0|Số chứng từ kết thúc phải lớn hơn số chứng từ bắt đầu"
                            '    Else
                            '        result = LaysohdPH(macn, mauso, kyhieu, sohoadonkt, sohoadonbd, ngaysd, idchitiet, tenhd)
                            '    End If

                            'End If
                            If CLng(sohoadonkt) <= CLng(sohoadonbd) Then
                                result = "0|Số chứng từ kết thúc phải lớn hơn số chứng từ bắt đầu"
                            Else
                                result = LaysohdPH(macn, mauso, kyhieu, sohoadonkt, sohoadonbd, ngaysd, idchitiet, tenhd, userid)
                            End If
                        End If
                    End If
                End While
            Else
                ' result = "Chưa có dữ liệu trong bảng hoadonph"
                If CLng(sohoadonkt) <= CLng(sohoadonbd) Then
                    result = "0|Số chứng từ kết thúc phải lớn hơn số chứng từ bắt đầu"
                Else
                    result = LaysohdPH(macn, mauso, kyhieu, sohoadonkt, sohoadonbd, ngaysd, idchitiet, tenhd, userid)
                End If
            End If
            reader.Close()
            conn.Close()
            conn.Dispose()
            comm_ph.Dispose()
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            result = "0|Error GetthongtinPH:  " & ex.Message
        End Try

        Return result
    End Function

    Private Function getIDChitiet(Maloaihd As String, Tenhd As String) As String
        Dim kq As String = String.Empty
        Dim conn As New SqlConnection(connectionString) 'Tao doi tuong ket noi
        conn.Open()
        Dim sql_DV As String = "select idLoaiHD_CT from vloaihd where MaloaiHD='" & Maloaihd & "' and TenHD1=N'" & Tenhd & "'"
        Dim comm_DV As New SqlCommand(sql_DV, conn)
        Dim reader As SqlDataReader = comm_DV.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                If reader(0) IsNot DBNull.Value Then
                    kq = reader(0).ToString
                End If
            End While
        End If
        reader.Close()
        conn.Close()
        conn.Dispose()
        comm_DV.Dispose()
        SqlConnection.ClearAllPools()
        Return kq
    End Function

    Private Function LaysohdPH(macn As String, mauso As String, kyhieu As String, sohdketthuc As String, sohdbatdau As String, ngaysd As String, idchitiet As String, tenhd As String, userid As String) As String
        'lay so hoa don da phat hanh cua cac chi nhanh
        Dim res As String = String.Empty
        Dim sql As String = String.Empty
        'query
        Dim sql_ins As String = "Insert into hoa_don_dang_ky_phat_hanh(mau_so, so_luong, so_bat_dau, so_ket_thuc, ngay_su_dung, donvi_ma_dv,  so_qd, ngay_qd, ky_hieu,loai_hoa_don_ct_id, ten_hoa_don,hoa_don_dang_ky_phat_hanh_trang_thai_id, is_deleted, created_time, created_user_id, last_modified_times, last_modified_user_id) values('" & mauso & "' ,'" & (CLng(sohdketthuc) - CLng(sohdbatdau) + 1) & "', '" & CLng(sohdbatdau) & "', '" & CLng(sohdketthuc) & "', '" & ngaysd & "', '" & macn & "', 'QD', '" & ngaysd & "', '" & kyhieu & "','" & idchitiet & "',N'" & tenhd & "','1' , 0, GETDATE(), '" & userid & "', GETDATE(), '" & userid & "')"

        Try
            sql = "select * from hoa_don_dang_ky_phat_hanh,donvi where hoa_don_dang_ky_phat_hanh.donvi_ma_dv=donvi.ma_dv and donvi.ma_dv='" & macn & "' and hoa_don_dang_ky_phat_hanh.mau_so ='" & mauso & "' AND hoa_don_dang_ky_phat_hanh.ky_hieu ='" & kyhieu & "' and hoa_don_dang_ky_phat_hanh.hoa_don_dang_ky_phat_hanh_trang_thai_id=1 order by so_bat_dau"
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand(sql, conn)
            Dim da As New SqlDataAdapter(comm)
            Dim tbl As New DataTable()
            da.Fill(tbl)
            If tbl.Rows.Count > 0 Then
                'lay cac khoang so da dang ky
                Dim i, j, n, k As Integer
                Dim tuso(10) As Long, denso(10) As Long
                Dim sobd(10), sokt(10) As Long
                Dim binsert As Boolean = False
                For i = 0 To tbl.Rows.Count - 1
                    tuso(i) = tbl.Rows(i)(3).ToString
                    denso(i) = tbl.Rows(i)(4).ToString
                    n = i
                Next
                '=================lay khoang chua dang ky
                'kiem tra khoang nhap vao voi khoang dau tien
                If CLng(sohdketthuc) < tuso(0) Then 'nhap luon

                    Dim comm1 As New SqlCommand
                    comm1.Connection = conn
                    comm1.CommandText = sql_ins
                    Dim iIns As Integer = comm1.ExecuteNonQuery()
                    If iIns > 0 Then
                        'thong bao phat hanh thanh cong
                        binsert = True
                    End If
                End If
                For j = 1 To n
                    If CLng(tuso(j)) - CLng(denso(j - 1)) > 0 Then
                        sobd(j) = denso(j - 1) + 1
                        sokt(j) = tuso(j) - 1
                        k = j
                    End If
                Next
                'kiem tra voi cac khoang nay
                For i = 1 To k
                    If CLng(sohdbatdau) >= sobd(i) And CLng(sohdketthuc) <= sokt(i) Then

                        Dim comm1 As New SqlCommand
                        comm1.Connection = conn
                        comm1.CommandText = sql_ins
                        Dim iIns As Integer = comm1.ExecuteNonQuery()
                        If iIns > 0 Then
                            binsert = True
                        End If
                    End If
                Next
                'kiem tra khoang nhap vao voi khoang cuoi cung
                If CLng(sohdbatdau) > denso(n) Then 'nhap luon

                    Dim comm1 As New SqlCommand
                    comm1.Connection = conn
                    comm1.CommandText = sql_ins
                    Dim iIns As Integer = comm1.ExecuteNonQuery()

                    If iIns > 0 Then
                        binsert = True
                    End If
                End If
                '======== Kết thúc việc lấy cac khoang chua dang ky
                If binsert = False Then
                    res = "0|Số hoá đơn này đã được đăng ký!"
                Else
                    res = "1|Phát hành lô chứng từ thành công"
                End If

            Else

                Dim comm1 As New SqlCommand
                comm1.Connection = conn
                comm1.CommandText = sql_ins
                Dim ilt = comm1.ExecuteNonQuery()


                res = "1|Phát hành lô chứng từ thành công"
            End If
            conn.Close()
            conn.Dispose()
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            res = "0|Error LaysohdPH:  " & ex.Message
        End Try
        Return res
    End Function


    Private Function kiemtrahuyloph(mauso As String, kyhieu As String, macn As String, sohoadonbd As String, ngayphgannhat As String, sohoadonkt As String, ngaysd As String) As String
        Dim res As String = String.Empty
        Dim ngayhdgannhat As String = String.Empty
        Try
            'Kiem tra lo hoa don phat hanh da duoc su dung hay chua. Neu chua su dung thi khong cho phat hanh tiep
            Dim sql_GetHD As String = "select top 1 SoHoaDon,NgayHD from hoadon68 where left(masohd,17)='" & mauso & kyhieu & "'  and Machinhanh='" & macn & "' and SoHoaDon >='" & Right("0000000" & sohoadonbd, 7) & "' AND NgayHD >='" & ngayphgannhat & "'  order by SoHoaDon DESC"
            Dim conn As New SqlConnection(connectionString)
            conn.Open()

            Dim comm_GetHD As New SqlCommand(sql_GetHD, conn)
            Dim reader As SqlDataReader = comm_GetHD.ExecuteReader
            If reader.HasRows Then
                'lo hoa don truoc do da su dung
                'check co thong bao huy lo hay khong
                Dim sql_checkcohuy As String = "select denso from tblHuylohd where denso<='" & sohoadonkt & "' and denso >='" & sohoadonbd & "' and mauso='" & mauso & "' and kyhieu='" & kyhieu & "' and macndv='" & macn & "'"
                Dim comm_checkcohuy As New SqlCommand(sql_checkcohuy, conn)
                Dim reader1 As SqlDataReader = comm_checkcohuy.ExecuteReader
                If reader1.HasRows Then
                    While reader.Read
                        ngayhdgannhat = reader("NgayHD").ToString
                    End While
                    If ngaysd <= ngayhdgannhat Then
                        res = "Ngày sử dụng phải lớn hơn ngày lập hoá đơn gần nhất (ngày:  " & ngayhdgannhat & ")"
                    Else
                        'da huy lo cu cho phep tao lo moi
                        res = "1"
                    End If
                    reader.Close()
                Else
                    'khong co huy
                    res = "Lô hoá đơn trước chưa sử dụng, bạn không được phát hành thêm!"
                End If
            Else
                'cho phep tao lo moi
                res = "1"
            End If

            conn.Close()
            conn.Dispose()
            comm_GetHD.Dispose()
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            res = "Error kiemtrahuyloph:  " & ex.Message
        End Try

        Return res
    End Function


    Private Function GetThoigianph(madv As String, mauso As String) As String
        Dim res As String = String.Empty
        Try
            Dim sql_temp As String = "select NgayQD from mauhoadon where MaDV='" & madv & "' and Mauso ='" & mauso & "'"
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand(sql_temp, conn)
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    res = reader(0).ToString
                End While

            Else
                res = 0
            End If
            reader.Close()
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            res = 0
        End Try
        Return res
    End Function

    Private Function CheckMauHoaDon(madv As String) As Boolean
        Dim result As Boolean = False
        Try
            Dim sql_temp As String = "SELECT TOP 1 idMauHD FROM Mauhoadon WHERE MaDV=@MaDV AND idLoaiHD_CT=14"
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using comm As New SqlCommand(sql_temp, conn)
                    comm.Parameters.AddWithValue("@MaDV", madv)

                    Dim reader As SqlDataReader = comm.ExecuteReader()
                    If reader.HasRows Then
                        result = True
                    Else
                        result = False
                    End If
                    reader.Close()
                End Using
            End Using
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            result = False
        End Try
        Return result
    End Function

    <WebMethod()>
    Public Function LayDSHoaDonDKPH(ByVal madonvi As String, ByVal mau_so As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim dt As New DataTable()

            Using conn As New SqlConnection(connectionString)
                Dim sql As String = "SELECT * FROM hoa_don_dang_ky_phat_hanh WHERE mau_so = @mau_so AND donvi_ma_dv = @madonvi AND is_deleted = 0 ORDER BY id DESC"

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@mau_so", mau_so)
                    cmd.Parameters.AddWithValue("@madonvi", madonvi)

                    Dim adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(dt)
                End Using
            End Using

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                response("status") = "success"
                response("message") = "Lấy danh sách hóa đơn đăng ký phát hành thành công"
                response("data") = dt
            Else
                response("status") = "error"
                response("message") = "Không tìm thấy bản ghi"
                response("data") = Nothing
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function Laythongtinchungtu(machungtu As Integer, madonvi As String) As String
        Dim dt As New DataTable("DLTokhaiCT")
        Dim response As New Dictionary(Of String, Object)()
        Try
            Using connection As New SqlConnection(connectionString)
                Dim myQuery As String = "SELECT * FROM ChungtuthueTNCN WHERE MaCT = @MaCT AND MasothueTC=@MasothueTC AND TrangthaicuoiCT = 1"

                Using myCommand As New SqlCommand(myQuery, connection)
                    myCommand.Parameters.Add("@MaCT", SqlDbType.Int).Value = machungtu
                    myCommand.Parameters.Add("@MasothueTC", SqlDbType.NVarChar, 15).Value = madonvi

                    Using adapter As New SqlDataAdapter(myCommand)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                response("status") = "success"
                response("message") = "Lấy thông tin thành công"

                response("data") = dt
            Else
                response("status") = "error"
                response("message") = "Không tìm thấy chứng từ"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    <WebMethod()>
    Public Function UpdateChungTuSauKy(xmlthongdiep As String, trangthai As String, mst As String, machungtu As Integer) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "UPDATE ChungtuthueTNCN SET XMLChungtu = @xmlthongdiep, TinhtrangCT = @trangthai WHERE MasothueTC = @mst AND MaCT = @machungtu"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.Add("@xmlthongdiep", SqlDbType.NVarChar).Value = xmlthongdiep
                    cmd.Parameters.Add("@trangthai", SqlDbType.NVarChar, 20).Value = trangthai
                    cmd.Parameters.Add("@mst", SqlDbType.NVarChar, 50).Value = mst
                    cmd.Parameters.Add("@machungtu", SqlDbType.Int).Value = machungtu

                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        response("status") = "success"
                        response("message") = "Cập nhật chứng từ thành công"
                    Else
                        response("status") = "error"
                        response("message") = "Không tìm tháy chứng từ"
                    End If
                End Using
            End Using

            Dim res As Integer = Trusoluonghoadon(mst)

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    Public Function Trusoluonghoadon(mst As String) As Integer
        Dim response As New Dictionary(Of String, Object)()

        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "update donvi set total_cks_con_lai= total_cks_con_lai -1, last_modified_times=GETDATE() where ma_dv=@madv"


                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.Add("@madv", SqlDbType.NVarChar).Value = mst
                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                End Using
            End Using
            Return 1
        Catch ex As Exception
            Return -1
        End Try
    End Function



    Private Function CheckMaCTGoc(mausogoc As String, kyhieugoc As String, soctgoc As String, madonvi As String) As CTGocResult
        Dim result As New CTGocResult()

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using comm As New SqlCommand()
                comm.Connection = conn
                'comm.CommandText =
                '    "WITH dsall AS (" &
                '    "   SELECT MaCT,MSChungtu,KHChungtu,Sochungtu, PhanbietCT, TinhtrangCT, TinhchatCT, TenTC, DiachiTC, SoCMND, MasothueTC,EmailNNT,CanhanCT, QuoctichNNT, DienthoaiNNT, Tenchungtu, NgaylapCT, ThunhapCN, Baohiem, TongTNChiuthue, TongTNTinhthue, ThueTNCN, TThien, ThangTN, Denthang, NamTN, MasothueNNT, DiachiNNT, TenNNT " &
                '    "   FROM ChungtuthueTNCN " &
                '    "   WHERE MasothueTC=@MasothueTC " &
                '    "     AND ((MSChungtu=@MSChungtu AND KHChungtu=@KHChungtu AND Sochungtu=@Sochungtu)" &
                '    "       OR (KHMSCTLienquan=@MSChungtu AND KHCTLienquan=@KHChungtu AND SoCTLienquan=@Sochungtu)) " &
                '    "     AND TrangthaicuoiCT = 1 AND TinhtrangCT <> 6" &
                '    "), cre_ctgoc AS (" &
                '    "   SELECT MaCT,PhanbietCT, TinhtrangCT, TinhchatCT, TenTC, DiachiTC, SoCMND, MasothueTC,EmailNNT,CanhanCT, QuoctichNNT, DienthoaiNNT, Tenchungtu, NgaylapCT, ThunhapCN, Baohiem, TongTNChiuthue, TongTNTinhthue, ThueTNCN, TThien, ThangTN, Denthang, NamTN, MasothueNNT, DiachiNNT, TenNNT " &
                '    "   FROM dsall " &
                '    "   WHERE MSChungtu=@MSChungtu AND KHChungtu=@KHChungtu AND Sochungtu=@Sochungtu" &
                '    "), cre_slct AS (" &
                '    "   SELECT COUNT(MaCT) AS SLCTu FROM dsall" &
                '    ") " &
                '    "SELECT * FROM cre_slct a CROSS JOIN (SELECT * FROM cre_ctgoc) b"

                comm.CommandText = "pro_chungtu_checkthaythedieuchinh"
                comm.CommandType = CommandType.StoredProcedure

                comm.Parameters.AddWithValue("@MasothueTC", madonvi)
                comm.Parameters.AddWithValue("@MSChungtu", mausogoc)
                comm.Parameters.AddWithValue("@KHChungtu", kyhieugoc)
                comm.Parameters.AddWithValue("@Sochungtu", soctgoc)

                Using reader As SqlDataReader = comm.ExecuteReader()
                    If reader.Read() Then
                        result.MaCT = reader("MaCT").ToString()
                        result.PhanbietCT = reader("PhanbietCT").ToString()
                        result.TinhtrangCT = reader("TinhtrangCT").ToString()
                        result.TinhchatCT = reader("TinhchatCT").ToString()
                        result.TenTC = reader("TenTC").ToString()
                        result.DiachiTC = reader("DiachiTC").ToString()
                        result.MasothueTC = reader("MasothueTC").ToString()
                        result.Tenchungtu = reader("Tenchungtu").ToString()
                        result.NgaylapCT = reader("NgaylapCT").ToString()
                        result.SoCMND = reader("SoCMND").ToString()
                        result.CanhanCT = reader("CanhanCT").ToString()
                        result.DienthoaiNNT = reader("DienthoaiNNT").ToString()
                        result.QuoctichNNT = reader("QuoctichNNT").ToString()
                        result.EmailNNT = reader("EmailNNT").ToString()
                        result.MasothueNNT = reader("MasothueNNT").ToString()
                        result.DiachiNNT = reader("DiachiNNT").ToString()
                        result.TenNNT = reader("TenNNT").ToString()


                        result.ThunhapCN = reader("ThunhapCN").ToString()
                        result.Baohiem = reader("Baohiem").ToString()
                        result.TongTNChiuthue = reader("TongTNChiuthue").ToString()
                        result.TongTNTinhthue = reader("TongTNTinhthue").ToString()
                        result.ThueTNCN = reader("ThueTNCN").ToString()
                        result.TThien = reader("TThien").ToString()

                        result.ThangTN = reader("ThangTN").ToString()
                        result.Denthang = reader("Denthang").ToString()
                        result.NamTN = reader("NamTN").ToString()

                        result.NamTN = reader("NamTN").ToString()
                        result.SLCTu = reader("SLCTu").ToString()
                    End If
                End Using
            End Using
        End Using

        SqlConnection.ClearAllPools()
        Return result
    End Function



    Private Function CheckThayTheCT(mausoctgoc As String, kyhieugoc As String, sochungtugoc As String, loaictapdung As Integer, madonvi As String) As String
        Dim Dieukienlap As Boolean = False
        Dim trangthaihdDC As Boolean = False
        Dim result As String = String.Empty

        Dim checkMCQT As Boolean = False
        Dim checkChungTuGoc As CTGocResult = CheckMaCTGoc(mausoctgoc, kyhieugoc, sochungtugoc, madonvi)
        If Not String.IsNullOrEmpty(checkChungTuGoc.MaCT) Then
            Dim phanloai_CTGoc As Integer = checkChungTuGoc.PhanbietCT

            If checkChungTuGoc.SLCTu < 2 Then
                'chung tu goc la hoa don moi hoac chung tu thay the
                If phanloai_CTGoc <= 1 Or (phanloai_CTGoc = 2 And checkChungTuGoc.TinhchatCT = 0) Then
                    If checkChungTuGoc.TinhtrangCT = 3 Or checkChungTuGoc.TinhtrangCT = 33 Then
                        'ct goc da co ma cua cqt
                        Dieukienlap = True
                    Else
                        'ct chua duoc cap ma
                        result = "-5|Chứng từ gốc chưa có mã của CQT không thể lập thay thế"
                    End If
                    If Dieukienlap = True Then
                        result = "2|" + checkChungTuGoc.MaCT
                    End If
                ElseIf phanloai_CTGoc = 2 And checkChungTuGoc.TinhchatCT = 2 Then
                    result = "-2|Chứng từ gốc là chứng từ điều chỉnh. Không thể lập thay thế"
                End If
            Else
                result = "-1|Chứng từ gốc đã bị thay thế không thể lập tiếp CT thay thế"
            End If
        Else
            result = "-4|Chứng từ gốc không tồn tại."
        End If

        Return result
    End Function


    Private Function CheckDieuChinhCT(mausoctgoc As String, kyhieugoc As String, sochungtugoc As String, loaictapdung As Integer, madonvi As String) As String
        Dim Dieukienlap As Boolean = False
        Dim result As String = String.Empty

        Dim checkChungTuGoc As CTGocResult = CheckMaCTGoc(mausoctgoc, kyhieugoc, sochungtugoc, madonvi)
        If Not String.IsNullOrEmpty(checkChungTuGoc.MaCT) Then
            Dim phanloai_CTGoc As Integer = checkChungTuGoc.PhanbietCT

            If checkChungTuGoc.SLCTu >= 2 Then
                result = "-1|Chứng từ gốc đã bị thay thế hoặc điều chỉnh, không thể lập tiếp chứng từ điều chỉnh"
            ElseIf phanloai_CTGoc = 0 Then
                If checkChungTuGoc.TinhtrangCT = 3 Or checkChungTuGoc.TinhtrangCT = 33 Then
                    Dieukienlap = True
                Else
                    result = "-5|Chứng từ gốc chưa có mã của CQT không thể lập điều chỉnh"
                End If
                If Dieukienlap = True Then
                    result = "2|" + checkChungTuGoc.MaCT
                End If
            ElseIf phanloai_CTGoc = 2 Then
                result = "-2|Không thể lập điều chỉnh vì chứng từ điều chỉnh không phải là chứng từ gốc"
            ElseIf phanloai_CTGoc = 1 Then
                result = "-10|Chứng từ gốc là chứng từ thay thế, không thể lập điều chỉnh"
            End If
        Else
            result = "-4|Chứng từ gốc không tồn tại."
        End If
        Return result

    End Function


    <WebMethod()>
    Public Function TaoChungTuThayTheDieuChinh(
        madonvi As String,
        mau_so As String,
        kyhieu As String,
        tenchungtu As String,
        ngaylap As String,
        mstnguoint As String,
        tennnt As String,
        diachi As String,
        dienthoai As String,
        email As String,
        cccd As String,
        tuthang As String,
        denthang As String,
        nam As String,
        quoctich As String,
        khoanthunhap As String,
        canhancutru As String,
        tongthunhapchiuthue As String,
        tongthunhaptinhthue As String,
        thuetncn As String,
        baohiem As String,
        tthien As String,
        TinhchatCT As String,
        LoaiCTLienquan As String,
        KHMSCTLienquan As String,
        KHCTLienquan As String,
        SoCTLienquan As String,
        NgaylapCTLienquan As String
    ) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim result As String = String.Empty

            If TinhchatCT = 1 Then
                result = CheckThayTheCT(KHMSCTLienquan, KHCTLienquan, SoCTLienquan, TinhchatCT, madonvi)
            Else
                result = CheckDieuChinhCT(KHMSCTLienquan, KHCTLienquan, SoCTLienquan, TinhchatCT, madonvi)
            End If
            Dim parts() As String = result.Split("|"c)

            If parts.Length > 1 Then
                If parts(0) = "2" Then
                    Dim ttdvi As thongtindv = GetTTDonvi(madonvi)
                    Dim resct As Integer
                    If mau_so = "03/TNCN" Then
                        tenchungtu = "CHỨNG TỪ KHẤU TRỪ THUẾ THU NHẬP CÁ NHÂN"
                        resct = TaoCT_khongso70(tenchungtu, mau_so, kyhieu, ngaylap, TinhchatCT, LoaiCTLienquan, KHMSCTLienquan, KHCTLienquan, SoCTLienquan, NgaylapCTLienquan, String.Empty, ttdvi.tendv, madonvi, ttdvi.diachi, ttdvi.dienthoai, tennnt, mstnguoint, diachi, quoctich, canhancutru, cccd, dienthoai, email, khoanthunhap, tuthang, nam, tongthunhapchiuthue, tongthunhaptinhthue, thuetncn, denthang, baohiem, tthien)
                    End If

                    If resct > 0 Then
                        response("status") = "success"
                        response("message") = "Tạo thành công"
                        response("data") = resct

                    Else
                        response("status") = "error"
                        response("message") = "Tạo chứng từ không thành công: " & resct
                        response("data") = Nothing
                    End If

                Else
                    response("status") = "error"
                    response("message") = parts(1)
                End If
            Else
                response("status") = "error"
                response("message") = "Có lỗi"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    <WebMethod()>
    Public Function CheckChungTuThayTheDieuChinh(
        madonvi As String,
        mau_so As String,
        kyhieu As String,
        sochungtu As String,
        TinhchatCT As String
    ) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim result As String = String.Empty

            If TinhchatCT = 1 Then
                result = CheckThayTheCT(mau_so, kyhieu, sochungtu, TinhchatCT, madonvi)
            Else
                result = CheckDieuChinhCT(mau_so, kyhieu, sochungtu, TinhchatCT, madonvi)
            End If
            Dim parts() As String = result.Split("|"c)

            If parts.Length > 1 Then
                If parts(0) = "2" Then
                    response("status") = "success"
                    response("message") = "Hợp lệ"
                    response("data") = parts(1)
                Else
                    response("status") = "error"
                    response("message") = parts(1)
                End If
            Else
                response("status") = "error"
                response("message") = "Có lỗi"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    'Tạo chứng từ
    <WebMethod()>
    Public Function TaoChungTu(
        madonvi As String,
        mau_so As String,
        kyhieu As String,
        tenchungtu As String,
        ngaylap As String,
        mstnguoint As String,
        tennnt As String,
        diachi As String,
        dienthoai As String,
        email As String,
        cccd As String,
        tuthang As String,
        denthang As String,
        nam As String,
        quoctich As String,
        khoanthunhap As String,
        canhancutru As String,
        tongthunhapchiuthue As String,
        tongthunhaptinhthue As String,
        thuetncn As String,
        baohiem As String,
        tthien As String
    ) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim ttdvi As thongtindv = GetTTDonvi(madonvi)
            Dim resct As Integer

            If mau_so = "03/TNCN" Then
                tenchungtu = "CHỨNG TỪ KHẤU TRỪ THUẾ THU NHẬP CÁ NHÂN"
                resct = TaoCT_khongso70(tenchungtu, mau_so, kyhieu, ngaylap, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, ttdvi.tendv, madonvi, ttdvi.diachi, ttdvi.dienthoai, tennnt, mstnguoint, diachi, quoctich, canhancutru, cccd, dienthoai, email, khoanthunhap, tuthang, nam, tongthunhapchiuthue, tongthunhaptinhthue, thuetncn, denthang, baohiem, tthien)
            End If

            If resct > 0 Then
                response("status") = "success"
                response("message") = "Tạo chứng từ thành công"
                response("data") = resct
            Else
                response("status") = "error"

                Select Case resct
                    Case -1
                        response("message") = "Không thể sinh (gen) XML."
                    Case -2
                        response("message") = "Không cập nhật được XML chứng từ."
                    Case -3
                        response("message") = "Không thể insert chứng từ."
                    Case -4
                        response("message") = "Ngày lập chứng từ không hợp lệ."
                    Case Else
                        response("message") = "Tạo chứng từ không thành công. Mã lỗi: " & resct
                End Select

                response("data") = Nothing
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function




    'Sửa chứng từ
    <WebMethod()>
    Public Function SuaChungTu(
          machungtu As String,
        madonvi As String,
        mau_so As String,
        kyhieu As String,
        ngaylap As String,
        mstnguoint As String,
        tennnt As String,
        diachi As String,
        dienthoai As String,
        email As String,
        cccd As String,
        tuthang As String,
        denthang As String,
        nam As String,
        quoctich As String,
        khoanthunhap As String,
        canhancutru As String,
        tongthunhapchiuthue As String,
        tongthunhaptinhthue As String,
        thuetncn As String,
        baohiem As String,
        tthien As String
    ) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim ttdvi As thongtindv = GetTTDonvi(madonvi)
            Dim resct As Integer

            If mau_so = "03/TNCN" Then
                resct = SuaCT_khongso70(machungtu, mau_so, kyhieu, ngaylap, ttdvi.tendv, madonvi, ttdvi.diachi, ttdvi.dienthoai, tennnt, mstnguoint, diachi, quoctich, canhancutru, cccd, dienthoai, email, khoanthunhap, tuthang, nam, tongthunhapchiuthue, tongthunhaptinhthue, thuetncn, denthang, baohiem, tthien)
            End If

            If resct > 0 Then
                response("status") = "success"
                response("message") = "Cập nhật chứng từ thành công"
                response("data") = resct
            Else
                response("status") = "error"

                Select Case resct
                    Case -1
                        response("message") = "Không thể sinh (gen) XML."
                    Case -2
                        response("message") = "Không cập nhật được XML chứng từ."
                    Case -3
                        response("message") = "Không thể insert chứng từ."
                    Case -4
                        response("message") = "Ngày lập chứng từ không hợp lệ."
                    Case Else
                        response("message") = "Cập nhật chứng từ không thành công. Mã lỗi: " & resct
                End Select

                response("data") = Nothing
            End If


        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function






    Private Function TaoCT_khongso70(Tenchungtu As String, MSChungtu As String, KHChungtu As String, NgaylapCT As String, TinhchatCT As String, LoaiCTLienquan As String, KHMSCTLienquan As String, KHCTLienquan As String, SoCTLienquan As String, NgaylapCTLienquan As String, GhichuCTLQ As String, TenTC As String, MasothueTC As String, DiachiTC As String, DienthoaiTC As String, TenNNT As String, MasothueNNT As String, DiachiNNT As String, QuoctichNNT As String, CanhanCT As String, SoCMND As String, DienthoaiNNT As String, EmailNNT As String, KhoanTN As String, ThangTN As Integer, NamTN As Integer, TongTNChiuthue As Double, TongTNTinhthue As Double, ThueTNCN As Double, Denthang As Integer, Baohiem As Double, TThien As Double) As Integer
        Dim kq = 0
        Dim Sochungtu = "0"
        'Check ngayhd
        'If KHChungtu = "undefined" Then
        '    KHChungtu = LayKHmoinhat_CTu(MasothueTC)
        'End If
        Dim namht As String = Right(Now.Year, 2)
        KHChungtu = "CT/" & namht & "E"
        Tenchungtu = "CHỨNG TỪ KHẤU TRỪ THUẾ THU NHẬP CÁ NHÂN"
        Dim checkngay As Integer = CheckngayCT(MasothueTC, MSChungtu, KHChungtu, NgaylapCT)
        If checkngay = 1 Then 'Ngay  hop le
            Dim matracuu As String = GetRandom()
            Dim idct As Integer = Insert_ChungtuthueTNCN(Tenchungtu, MSChungtu, KHChungtu, Sochungtu, NgaylapCT, TinhchatCT, LoaiCTLienquan, KHMSCTLienquan, KHCTLienquan, SoCTLienquan, NgaylapCTLienquan, GhichuCTLQ, TenTC, MasothueTC, DiachiTC, DienthoaiTC, TenNNT, MasothueNNT, DiachiNNT, QuoctichNNT, CanhanCT, SoCMND, DienthoaiNNT, EmailNNT, KhoanTN, ThangTN, NamTN, TongTNChiuthue, TongTNTinhthue, ThueTNCN, Denthang, Baohiem, TThien)

            If idct > 0 Then
                ' genXML
                Dim xmlchungtu As String = GenerateChungTuXml(idct)

                If String.IsNullOrEmpty(xmlchungtu) Then
                    'lỗi gen xml
                    kq = -1
                Else

                    If UpdateXMLCTu(idct, xmlchungtu) > 0 Then
                        kq = idct
                    Else
                        kq = -2  'không update đc xml chứng từ
                    End If
                End If
            Else
                kq = -3 'Không ins được chứng từ
            End If
        Else
            kq = -4 'ngày lập chứng từ không hợp lệ
        End If
        Return kq
    End Function


    Private Function SuaCT_khongso70(MaCT As String, MSChungtu As String, KHChungtu As String, NgaylapCT As String, TenTC As String, MasothueTC As String, DiachiTC As String, DienthoaiTC As String, TenNNT As String, MasothueNNT As String, DiachiNNT As String, QuoctichNNT As String, CanhanCT As String, SoCMND As String, DienthoaiNNT As String, EmailNNT As String, KhoanTN As String, ThangTN As Integer, NamTN As Integer, TongTNChiuthue As Double, TongTNTinhthue As Double, ThueTNCN As Double, Denthang As Integer, Baohiem As Double, TThien As Double) As Integer
        Dim kq = 0
        Dim Sochungtu = "0"
        Dim namht As String = Right(Now.Year, 2)
        KHChungtu = "CT/" & namht & "E"


        'Check ngayhd
        Dim checkngay As Integer = CheckngayCT(MasothueTC, MSChungtu, KHChungtu, NgaylapCT)
        If checkngay = 1 Then 'Ngay  hop le
            Dim idct As Integer = Update_ChungtuthueTNCN(MaCT, MSChungtu, KHChungtu, Sochungtu, NgaylapCT, TenTC, MasothueTC, DiachiTC, DienthoaiTC, TenNNT, MasothueNNT, DiachiNNT, QuoctichNNT, CanhanCT, SoCMND, DienthoaiNNT, EmailNNT, KhoanTN, ThangTN, NamTN, TongTNChiuthue, TongTNTinhthue, ThueTNCN, Denthang, Baohiem, TThien)

            If idct > 0 Then
                ' genXML
                Dim xmlchungtu As String = GenerateChungTuXml(idct)

                If String.IsNullOrEmpty(xmlchungtu) Then
                    'lỗi gen xml
                    kq = -1
                Else

                    If UpdateXMLCTu(idct, xmlchungtu) > 0 Then
                        kq = idct
                    Else
                        kq = -2  'không update đc xml chứng từ
                    End If
                End If
            Else
                kq = -3 'Không ins được chứng từ
            End If
        Else
            kq = -4 'ngày lập chứng từ không hợp lệ
        End If
        Return kq
    End Function



    Private Function CheckngayCT(MasothueTC As String, MSCTu As String, KHCTu As String, ngaylapct As String) As Integer
        Dim kq = 0
        Dim ngaysd_phathanh As String = Layngaysd_CTu(MasothueTC, MSCTu, KHCTu)
        If Not Equals(ngaysd_phathanh, "") Then
            Dim ngaysdct_ph = Date.Parse(ngaysd_phathanh)
            Dim ngayctu = Date.Parse(ngaylapct)
            If ngayctu >= ngaysdct_ph Then

                Dim ngayhdmax As String = LayngayCTMax(MasothueTC, MSCTu, KHCTu)
                If Not Equals(ngayhdmax, "") Then
                    If ngayctu >= Date.Parse(ngayhdmax) Then
                        kq = 1
                    Else
                        kq = -1 ' ngayct phai >= ngayct max
                    End If
                Else
                    kq = 1
                End If
            Else
                kq = -2 ' Ngayct phai >= ngaysd ct
            End If
        Else
            Return -3 'Khong co thong tin phat hanh 
        End If

        Return kq
    End Function

    Private Function LayngayCTMax(MasothueTC As String, MSCTu As String, KHCTu As String) As String
        Dim kq As String = String.Empty
        Try
            Dim connection As SqlConnection = New SqlConnection(connectionString)
            Dim dt As DataTable = New DataTable()
            Dim sql As String = String.Empty
            If KHCTu = "undefined" Then
                sql = "select top 1 NgaylapCT,Sochungtu from ChungtuthueTNCN  where MasothueTC=@msttc and MSChungtu=@msct and NgaylapCT >='2026-01-01' and Trangthai>0 order by NgaylapCT desc"
            Else
                sql = "select top 1 NgaylapCT,Sochungtu from ChungtuthueTNCN  where MasothueTC=@msttc and MSChungtu=@msct and KHChungtu=@kyhieuct and Trangthai>0 order by Sochungtu desc "
            End If
            Dim cmd As SqlCommand = New SqlCommand(sql, connection)
            cmd.Parameters.AddWithValue("@msttc", MasothueTC)
            cmd.Parameters.AddWithValue("@msct", MSCTu)
            cmd.Parameters.AddWithValue("@kyhieuct", KHCTu)
            Dim adapter As SqlDataAdapter = New SqlDataAdapter()
            adapter.SelectCommand = cmd
            adapter.Fill(dt)
            If dt.Rows.Count > 0 Then
                kq = Thoigianchuan(dt.Rows(0)("NgaylapCT").ToString())
            End If
            cmd.Dispose()
            connection.Close()
            connection.Dispose()
            SqlConnection.ClearAllPools()

        Catch ex As Exception

        End Try
        Return kq
    End Function
    Private Function LayKHmoinhat_CTu(masothuetc As String) As String
        Dim kq As String = String.Empty
        Try
            Using connection As SqlConnection = New SqlConnection(connectionString)
                Dim dt As DataTable = New DataTable()
                Dim sql As String = String.Empty
                sql = "select top 1 ky_hieu from hoa_don_dang_ky_phat_hanh  where donvi_ma_dv=@madv and mau_so=@mauso and ngay_su_dung >='2026-01-01' and is_deleted=0 order by id desc"

                Dim cmd As SqlCommand = New SqlCommand(sql, connection)
                cmd.Parameters.AddWithValue("@madv", masothuetc)

                Dim reader As SqlDataReader = cmd.ExecuteReader
                If reader.HasRows Then
                    While reader.Read
                        kq = reader("ky_hieu").ToString
                    End While
                End If
            End Using
        Catch ex As Exception
        End Try
        Return kq
    End Function

    Private Function Layngaysd_CTu(masothuetc As String, khmsctu As String, khctu As String) As String
        Dim kq As String = String.Empty
        Try
            Using connection As SqlConnection = New SqlConnection(connectionString)
                Dim dt As DataTable = New DataTable()
                Dim sql As String = String.Empty
                If khctu = "undefined" Then
                    sql = "select top 1 ngay_su_dung from hoa_don_dang_ky_phat_hanh  where donvi_ma_dv=@madv and mau_so=@mauso and ngay_su_dung >='2026-01-01' and is_deleted=0 order by id desc"
                Else
                    sql = "select top 1 ngay_su_dung from hoa_don_dang_ky_phat_hanh  where donvi_ma_dv=@madv and mau_so=@mauso and ky_hieu=@kyhieu and is_deleted=0 order by id desc"
                End If

                Dim cmd As SqlCommand = New SqlCommand(sql, connection)
                cmd.Parameters.AddWithValue("@madv", masothuetc)
                cmd.Parameters.AddWithValue("@mauso", khmsctu)
                cmd.Parameters.AddWithValue("@kyhieu", khctu)
                Dim adapter As SqlDataAdapter = New SqlDataAdapter()
                adapter.SelectCommand = cmd
                adapter.Fill(dt)
                If dt.Rows.Count > 0 Then
                    kq = Thoigianchuan(dt.Rows(0)("ngay_su_dung").ToString())
                End If
                cmd.Dispose()

            End Using

        Catch ex As Exception

        End Try
        Return kq
    End Function


    <WebMethod()>
    Public Function LaysoCT_update(MasothueTC As String, KHCTu As String, mactu As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim kq_update_soct As String = String.Empty
            Dim somaxctu = 0
            Dim namht As String = Right(Now.Year, 2)
            KHCTu = "CT/" & namht & "E"
            Dim sql = "select max(convert(int,sochungtu)) as somaxctu from ChungtuthueTNCN where MasothueTC=@msttchuc and KHChungtu=@kyhieuctu and TrangthaicuoiCT=1"
            Dim conn As SqlConnection = New SqlConnection(connectionString)
            conn.Open()
            Dim cmd As SqlCommand = New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@msttchuc", MasothueTC)
            cmd.Parameters.AddWithValue("@kyhieuctu", KHCTu)

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            If reader.HasRows Then
                While reader.Read()
                    somaxctu = Convert.ToInt32(reader(("somaxctu")).ToString())
                End While
            End If

            reader.Close()

            Dim tongmua As Integer = 0
            tongmua = LayTongSodangky(MasothueTC)
            Dim tongdasd As Integer = 0
            tongdasd = LayTongSoCT_Dasd(MasothueTC)
            Dim soctu_update = 0
            If tongmua - tongdasd > 0 Then
                soctu_update = somaxctu + 1
                Dim kq_up As Integer = UpdateSoCTu(mactu, soctu_update)
                If kq_up > 0 Then
                    kq_update_soct = "1|" & GenerateChungTuXml(mactu)
                End If
            Else
                kq_update_soct = "0|Đã hết số lượng đăng ký"
            End If

            conn.Close()
            conn.Dispose()
            SqlConnection.ClearAllPools()


            Dim parts() As String = kq_update_soct.Split("|"c)

            If parts.Length > 1 Then
                If parts(0) = "1" Then
                    response("status") = "success"
                    response("data") = parts(1)
                    response("message") = "Lấy số chứng từ thành công"
                Else
                    response("status") = "error"
                    response("message") = parts(1)
                End If
            Else
                response("status") = "error"
                response("message") = "Có lỗi"
            End If
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function LaySoCTMulti_update(MasothueTC As String, KHCTu As String, DanhSachMaCTu As List(Of String)) As String
        Dim results As New List(Of Dictionary(Of String, Object))()
        Dim response As New Dictionary(Of String, Object)()

        Try
            For Each mactu As String In DanhSachMaCTu
                Dim result As New Dictionary(Of String, Object)()
                Dim somaxctu As Integer = 0
                Dim namht As String = Right(Now.Year, 2)
                KHCTu = "CT/" & namht & "E"

                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Dim sql As String = "select max(convert(int,sochungtu)) as somaxctu from ChungtuthueTNCN where MasothueTC=@msttchuc and KHChungtu=@kyhieuctu and TrangthaicuoiCT=1"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@msttchuc", MasothueTC)
                        cmd.Parameters.AddWithValue("@kyhieuctu", KHCTu)
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            If reader.HasRows Then
                                While reader.Read()
                                    If Not IsDBNull(reader("somaxctu")) Then
                                        somaxctu = Convert.ToInt32(reader("somaxctu"))
                                    End If
                                End While
                            End If
                        End Using
                    End Using

                    Dim tongmua As Integer = LayTongSodangky(MasothueTC)
                    Dim tongdasd As Integer = LayTongSoCT_Dasd(MasothueTC)
                    Dim soctu_update As Integer = 0
                    Dim kq_update_soct As String = ""

                    If tongmua - tongdasd > 0 Then
                        soctu_update = somaxctu + 1
                        Dim kq_up As Integer = UpdateSoCTu(mactu, soctu_update)
                        If kq_up > 0 Then
                            kq_update_soct = "1|" & GenerateChungTuXml(mactu)
                            result("status") = "success"
                            result("mactu") = mactu
                            result("data") = GenerateChungTuXml(mactu)
                        Else
                            result("status") = "error"
                            result("mactu") = mactu
                            result("message") = "Cập nhật số chứng từ thất bại"
                        End If
                    Else
                        result("status") = "error"
                        result("mactu") = mactu
                        result("message") = "Đã hết số lượng đăng ký"
                    End If
                End Using

                results.Add(result)
            Next

            response("status") = "success"
            response("results") = results
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    'Private Function UpdateSoCTu(mactu As String, soctu_update As Integer) As Integer
    '    Dim kq_update As Integer = 0

    '    Dim Sql As String = "Update ChungtuthueTNCN set Sochungtu=@soctu where  MaCT=@mactu"

    '    Using conn As SqlConnection = New SqlConnection(connectionString)
    '        conn.Open()
    '        Dim comm As SqlCommand = New SqlCommand(Sql, conn)
    '        comm.Parameters.AddWithValue("@mactu", mactu)
    '        comm.Parameters.AddWithValue("@soctu", soctu_update)
    '        kq_update = comm.ExecuteNonQuery()
    '        comm.Dispose()
    '    End Using

    '    Return kq_update
    'End Function

    Private Function UpdateSoCTu(mactu As String, soctu_update As Integer) As Integer
        Dim kq_update As Integer = 0
        Dim sqlCheck As String = "SELECT ISNULL(Sochungtu, 0) AS Sochungtu FROM ChungtuthueTNCN WHERE MaCT = @mactu"

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            '--- Kiểm tra giá trị hiện tại của Sochungtu ---
            Dim currentSoCT As Integer = 0
            Using cmdCheck As New SqlCommand(sqlCheck, conn)
                cmdCheck.Parameters.AddWithValue("@mactu", mactu)
                Dim result = cmdCheck.ExecuteScalar()
                If result IsNot Nothing Then
                    currentSoCT = Convert.ToInt32(result)
                End If
            End Using

            '--- Nếu Sochungtu = 0 thì update ---
            If currentSoCT = 0 Then
                Dim sqlUpdate As String = "UPDATE ChungtuthueTNCN SET Sochungtu = @soctu WHERE MaCT = @mactu"
                Using cmdUpdate As New SqlCommand(sqlUpdate, conn)
                    cmdUpdate.Parameters.AddWithValue("@soctu", soctu_update)
                    cmdUpdate.Parameters.AddWithValue("@mactu", mactu)
                    kq_update = cmdUpdate.ExecuteNonQuery()
                End Using
            Else
                ' Nếu đã có số chứng từ thì xem như đã thành công, trả về 1
                kq_update = 1
            End If
        End Using

        Return kq_update
    End Function


    Private Function LayTongSodangky(Madonvi As String) As Integer
        Dim tongmua = 0
        Dim sql = "select sum(tong_so_luong) as Tongdky from donvi_mua_chukyso where donvi_mst=@mst and is_deleted=0"
        Dim conn As SqlConnection = New SqlConnection(connectionString)
        conn.Open()
        Dim cmd As SqlCommand = New SqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("@mst", Madonvi)
        Dim reader As SqlDataReader = cmd.ExecuteReader()
        If reader.HasRows Then
            While reader.Read()
                If Not String.IsNullOrEmpty(reader("Tongdky")) Then
                    tongmua = Convert.ToInt32(reader("Tongdky").ToString())
                End If
            End While
        End If

        reader.Close()
        conn.Close()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return tongmua

    End Function

    Private Function LayTongSoCT_Dasd(Madonvi As String) As Integer
        Dim tongmua = 0
        Dim sql = "select count(MaCT) as Tongsd from ChungtuthueTNCN where MasothueTC=@mst and Sochungtu >0    and TrangthaicuoiCT=1"
        Dim conn As SqlConnection = New SqlConnection(connectionString)
        conn.Open()
        Dim cmd As SqlCommand = New SqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("@mst", Madonvi)
        Dim reader As SqlDataReader = cmd.ExecuteReader()
        If reader.HasRows Then
            While reader.Read()

                If Not String.IsNullOrEmpty(reader("Tongsd")) Then
                    tongmua = Convert.ToInt32(reader("Tongsd").ToString())
                End If
            End While
        End If

        reader.Close()
        conn.Close()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return tongmua

    End Function

    Private Function Insert_ChungtuthueTNCN(Tenchungtu As String, MSChungtu As String, KHChungtu As String, Sochungtu As String, NgaylapCT As String, TinhchatCT As String, LoaiCTLienquan As String, KHMSCTLienquan As String, KHCTLienquan As String, SoCTLienquan As String, NgaylapCTLienquan As String, GhichuCTLQ As String, TenTC As String, MasothueTC As String, DiachiTC As String, DienthoaiTC As String, TenNNT As String, MasothueNNT As String, DiachiNNT As String, QuoctichNNT As String, CanhanCT As String, SoCMND As String, DienthoaiNNT As String, EmailNNT As String, KhoanTN As String, ThangTN As Integer, NamTN As String, TongTNChiuthue As Double, TongTNTinhthue As Double, ThueTNCN As Double, Denthang As Integer, Baohiem As Double, TThien As Double) As Integer
        Dim kq = 0
        Dim Phienban = "2.1.0"
        Dim conn As SqlConnection = New SqlConnection()
        Dim comm As SqlCommand = New SqlCommand()
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn
        Dim matracuu As String = GetRandom()
        Dim ngaycapnhat = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
        comm.CommandText = "Insert into ChungtuthueTNCN(Phienban,Tenchungtu,MSChungtu,KHChungtu,Sochungtu,NgaylapCT,TinhchatCT,LoaiCTLienquan,KHMSCTLienquan,KHCTLienquan,SoCTLienquan,NgaylapCTLienquan,GhichuCTLQ,TenTC,MasothueTC,DiachiTC,DienthoaiTC,TenNNT,MasothueNNT,DiachiNNT,QuoctichNNT,CanhanCT,SoCMND,DienthoaiNNT,EmailNNT,ThunhapCN,ThangTN,NamTN,TongTNChiuthue,TongTNTinhthue,ThueTNCN,Matracuu,Trangthai,TinhtrangCT,Thoigiancapnhat,Denthang,Baohiem, TThien,KhoanTN, PhanbietCT) values (@Phienban, @Tenchungtu, @MSChungtu, @KHChungtu, @Sochungtu, @NgaylapCT, @TinhchatCT,@LoaiCTLienquan,@KHMSCTLienquan,@KHCTLienquan,@SoCTLienquan,@NgaylapCTLienquan,@GhichuCTLQ,@TenTC,@MasothueTC,@DiachiTC,@DienthoaiTC,@TenNNT,@MasothueNNT,@DiachiNNT,@QuoctichNNT,@CanhanCT,@SoCMND,@DienthoaiNNT,@EmailNNT,@ThunhapCN,@ThangTN,@NamTN,@TongTNChiuthue,@TongTNTinhthue,@ThueTNCN,@Matracuu,@Trangthai,@TinhtrangCT,@Thoigiancapnhat,@Denthang,@Baohiem, @TThien,@KhoanTN, @PhanbietCT)"
        comm.Parameters.AddWithValue("@Phienban", Phienban)
        comm.Parameters.AddWithValue("@Tenchungtu", Tenchungtu)
        comm.Parameters.AddWithValue("@MSChungtu", MSChungtu)
        comm.Parameters.AddWithValue("@KHChungtu", KHChungtu)
        comm.Parameters.AddWithValue("@Sochungtu", Sochungtu)
        comm.Parameters.AddWithValue("@NgaylapCT", NgaylapCT)
        comm.Parameters.AddWithValue("@TinhchatCT", TinhchatCT)
        comm.Parameters.AddWithValue("@LoaiCTLienquan", LoaiCTLienquan)
        comm.Parameters.AddWithValue("@KHMSCTLienquan", KHMSCTLienquan)
        comm.Parameters.AddWithValue("@KHCTLienquan", KHCTLienquan)
        comm.Parameters.AddWithValue("@SoCTLienquan", SoCTLienquan)
        comm.Parameters.AddWithValue("@NgaylapCTLienquan", NgaylapCTLienquan)
        comm.Parameters.AddWithValue("@GhichuCTLQ", GhichuCTLQ)
        comm.Parameters.AddWithValue("@TenTC", TenTC)
        comm.Parameters.AddWithValue("@MasothueTC", MasothueTC)
        comm.Parameters.AddWithValue("@DiachiTC", DiachiTC)
        comm.Parameters.AddWithValue("@DienthoaiTC", DienthoaiTC)
        comm.Parameters.AddWithValue("@TenNNT", TenNNT)
        comm.Parameters.AddWithValue("@MasothueNNT", MasothueNNT)
        comm.Parameters.AddWithValue("@DiachiNNT", DiachiNNT)
        comm.Parameters.AddWithValue("@QuoctichNNT", QuoctichNNT)
        comm.Parameters.AddWithValue("@CanhanCT", CanhanCT)
        comm.Parameters.AddWithValue("@SoCMND", SoCMND)
        comm.Parameters.AddWithValue("@DienthoaiNNT", DienthoaiNNT)
        comm.Parameters.AddWithValue("@EmailNNT", EmailNNT)
        comm.Parameters.AddWithValue("@ThunhapCN", KhoanTN)
        comm.Parameters.AddWithValue("@ThangTN", ThangTN)
        comm.Parameters.AddWithValue("@NamTN", NamTN)
        comm.Parameters.AddWithValue("@TongTNChiuthue", TongTNChiuthue)
        comm.Parameters.AddWithValue("@TongTNTinhthue", TongTNTinhthue)
        comm.Parameters.AddWithValue("@ThueTNCN", ThueTNCN)
        comm.Parameters.AddWithValue("@Matracuu", matracuu)
        comm.Parameters.AddWithValue("@Trangthai", 1)
        comm.Parameters.AddWithValue("@TinhtrangCT", 0)
        comm.Parameters.AddWithValue("@Thoigiancapnhat", ngaycapnhat)
        'bo sung them theo ban cap nhat
        comm.Parameters.AddWithValue("@Denthang", Denthang)
        comm.Parameters.AddWithValue("@Baohiem", Baohiem)
        comm.Parameters.AddWithValue("@TThien", TThien)
        comm.Parameters.AddWithValue("@KhoanTN", KhoanTN)
        comm.Parameters.AddWithValue("@PhanbietCT", TinhchatCT)

        comm.ExecuteNonQuery()

        'Lay idhd tuong ung
        Dim sqlSel = "select MaCT from ChungtuthueTNCN where MSChungtu='" & MSChungtu & "' and MasothueTC='" & MasothueTC & "' and Trangthai=1 and Thoigiancapnhat='" & ngaycapnhat & "' and Matracuu='" & matracuu & "'"
        comm.CommandText = sqlSel
        Dim reader As SqlDataReader = comm.ExecuteReader()
        If reader.HasRows Then
            While reader.Read()
                kq = Convert.ToInt32(reader(CInt(0)).ToString())
            End While
        End If
        reader.Close()
        conn.Close()
        comm.Dispose()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return kq
    End Function


    Private Function Update_ChungtuthueTNCN(
    MaCT As String, MSChungtu As String, KHChungtu As String, Sochungtu As String, NgaylapCT As String,
    TenTC As String, MasothueTC As String, DiachiTC As String, DienthoaiTC As String,
    TenNNT As String, MasothueNNT As String, DiachiNNT As String, QuoctichNNT As String,
    CanhanCT As String, SoCMND As String, DienthoaiNNT As String, EmailNNT As String,
    KhoanTN As String, ThangTN As Integer, NamTN As String,
    TongTNChiuthue As Double, TongTNTinhthue As Double, ThueTNCN As Double,
    Denthang As Integer, Baohiem As Double, TThien As Double) As Integer

        Dim kq As Integer = 0
        Dim ngaycapnhat As String = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using comm As New SqlCommand()
                comm.Connection = conn

                ' UPDATE câu lệnh
                comm.CommandText = "UPDATE ChungtuthueTNCN SET " &
                               "KHChungtu=@KHChungtu, Sochungtu=@Sochungtu, NgaylapCT=@NgaylapCT, TenTC=@TenTC, " &
                               "MasothueTC=@MasothueTC, DiachiTC=@DiachiTC, DienthoaiTC=@DienthoaiTC, TenNNT=@TenNNT, " &
                               "MasothueNNT=@MasothueNNT, DiachiNNT=@DiachiNNT, QuoctichNNT=@QuoctichNNT, CanhanCT=@CanhanCT, " &
                               "SoCMND=@SoCMND, DienthoaiNNT=@DienthoaiNNT, EmailNNT=@EmailNNT, ThunhapCN=@ThunhapCN, " &
                               "ThangTN=@ThangTN, NamTN=@NamTN, TongTNChiuthue=@TongTNChiuthue, TongTNTinhthue=@TongTNTinhthue, " &
                               "ThueTNCN=@ThueTNCN, Thoigiancapnhat=@Thoigiancapnhat, Denthang=@Denthang, Baohiem=@Baohiem, " &
                               "TThien=@TThien, KhoanTN=@KhoanTN " &
                               "WHERE MaCT=@MaCT AND MasothueTC=@MasothueTC"

                comm.Parameters.AddWithValue("@MaCT", MaCT)
                comm.Parameters.AddWithValue("@MSChungtu", MSChungtu)
                comm.Parameters.AddWithValue("@KHChungtu", KHChungtu)
                comm.Parameters.AddWithValue("@Sochungtu", Sochungtu)
                comm.Parameters.AddWithValue("@NgaylapCT", NgaylapCT)
                comm.Parameters.AddWithValue("@TenTC", TenTC)
                comm.Parameters.AddWithValue("@MasothueTC", MasothueTC)
                comm.Parameters.AddWithValue("@DiachiTC", DiachiTC)
                comm.Parameters.AddWithValue("@DienthoaiTC", DienthoaiTC)
                comm.Parameters.AddWithValue("@TenNNT", TenNNT)
                comm.Parameters.AddWithValue("@MasothueNNT", MasothueNNT)
                comm.Parameters.AddWithValue("@DiachiNNT", DiachiNNT)
                comm.Parameters.AddWithValue("@QuoctichNNT", QuoctichNNT)
                comm.Parameters.AddWithValue("@CanhanCT", CanhanCT)
                comm.Parameters.AddWithValue("@SoCMND", SoCMND)
                comm.Parameters.AddWithValue("@DienthoaiNNT", DienthoaiNNT)
                comm.Parameters.AddWithValue("@EmailNNT", EmailNNT)
                comm.Parameters.AddWithValue("@ThunhapCN", KhoanTN)
                comm.Parameters.AddWithValue("@ThangTN", ThangTN)
                comm.Parameters.AddWithValue("@NamTN", NamTN)
                comm.Parameters.AddWithValue("@TongTNChiuthue", TongTNChiuthue)
                comm.Parameters.AddWithValue("@TongTNTinhthue", TongTNTinhthue)
                comm.Parameters.AddWithValue("@ThueTNCN", ThueTNCN)
                comm.Parameters.AddWithValue("@Thoigiancapnhat", ngaycapnhat)
                'bo sung them theo ban cap nhat
                comm.Parameters.AddWithValue("@Denthang", Denthang)
                comm.Parameters.AddWithValue("@Baohiem", Baohiem)
                comm.Parameters.AddWithValue("@TThien", TThien)
                comm.Parameters.AddWithValue("@KhoanTN", KhoanTN)


                ' Thực hiện UPDATE và kiểm tra số hàng bị ảnh hưởng
                Dim rowsAffected As Integer = comm.ExecuteNonQuery()

                If rowsAffected > 0 Then
                    ' Nếu update thành công, trả về MaCT (ép về Integer)
                    Integer.TryParse(MaCT, kq) ' nếu MaCT không phải số, kq sẽ = 0
                Else
                    kq = 0
                End If


            End Using
        End Using

        Return kq
    End Function




    Public Function GenerateChungTuXml(idct As String) As String
        Dim TinhchatCT As String = String.Empty

        Dim phienban As String = String.Empty
        Dim tenctu As String = String.Empty
        Dim msoctu As String = String.Empty
        Dim kyhieuct As String = String.Empty
        Dim sochungtu As String = String.Empty
        Dim ngaylap As String = String.Empty
        Dim tentochuc As String = String.Empty
        Dim msttochuc As String = String.Empty
        Dim diachitc As String = String.Empty
        Dim sdttochuc As String = String.Empty
        Dim tennnt As String = String.Empty
        Dim mstnnt As String = String.Empty
        Dim dchinnt As String = String.Empty
        Dim qtich As String = String.Empty
        Dim canhancutru As Integer = 0
        Dim cccdan_nnt As String = String.Empty
        Dim emailnnt As String = String.Empty
        Dim sdtnnt As String = String.Empty
        Dim khoanthunhap As String = String.Empty
        Dim tuthang As String = String.Empty
        Dim denthang As String = String.Empty
        Dim nam As String = String.Empty
        Dim baohiem As String = String.Empty
        Dim tuthien As String = String.Empty
        Dim tongtnchiuthue As String = String.Empty
        Dim tongtntinhthue As String = String.Empty
        Dim sothue As String = String.Empty
        Dim matracuu As String = String.Empty
        Dim loaictlquan As String = String.Empty
        Dim khmsctlquan As String = String.Empty
        Dim kyhieuctlquan As String = String.Empty
        Dim soctulquan As String = String.Empty
        Dim ngaylapctlquan As String = String.Empty
        Dim ghichu As String = String.Empty
        Dim connection As SqlConnection = New SqlConnection(connectionString)

        Dim dt As DataTable = New DataTable()
        Try
            'Lay thong tin chung tu

            Dim sql As String = "select MaCT, Phienban, Tenchungtu,MSChungtu, KHChungtu, Sochungtu,NgaylapCT,TinhchatCT,LoaiCTLienquan, KHMSCTLienquan, KHCTLienquan,SoCTLienquan,NgaylapCTLienquan, TenTC, MasothueTC, DiachiTC, DienthoaiTC, TenNNT, MasothueNNT,DiachiNNT,QuoctichNNT,CanhanCT, SoCMND, DienthoaiNNT, EmailNNT, ThangTN, NamTN,TongTNChiuthue, TongTNTinhthue, ThueTNCN, Matracuu,GhichuCTLQ, KhoanTN, Denthang, Baohiem,TThien from ChungtuthueTNCN where MaCT=@Mactu and TrangthaicuoiCT=1"
            connection.Open()
            Dim cmd As SqlCommand = New SqlCommand(sql, connection)
            cmd.Parameters.AddWithValue("Mactu", idct)
            Dim reader As SqlDataReader = cmd.ExecuteReader

            If reader.HasRows Then
                While reader.Read
                    TinhchatCT = Convert.ToInt16(reader("TinhchatCT"))
                    phienban = reader("Phienban")
                    tenctu = reader("Tenchungtu")
                    msoctu = reader("MSChungtu")
                    kyhieuct = reader("KHChungtu")
                    sochungtu = reader("Sochungtu")
                    ngaylap = Thoigianchuan(reader("NgaylapCT"))
                    loaictlquan = reader("LoaiCTLienquan")
                    khmsctlquan = reader("KHMSCTLienquan")
                    kyhieuctlquan = reader("KHCTLienquan")
                    kyhieuctlquan = reader("KHCTLienquan")
                    soctulquan = reader("SoCTLienquan")
                    If ngaylapctlquan IsNot DBNull.Value Or Not String.IsNullOrEmpty(reader("NgaylapCTLienquan")) Then
                        ngaylapctlquan = Thoigianchuan(reader("NgaylapCTLienquan"))
                    End If
                    tentochuc = reader("TenTC")
                    msttochuc = reader("MasothueTC")
                    diachitc = reader("DiachiTC")
                    sdttochuc = reader("DienthoaiTC")
                    tennnt = reader("TenNNT")
                    mstnnt = reader("MasothueNNT")
                    dchinnt = reader("DiachiNNT")
                    qtich = reader("QuoctichNNT")
                    canhancutru = reader("CanhanCT")
                    cccdan_nnt = reader("SoCMND")

                    sdtnnt = reader("DienthoaiNNT")
                    emailnnt = reader("EmailNNT")

                    tuthang = reader("ThangTN")
                    nam = reader("NamTN")
                    tongtnchiuthue = Convert.ToDouble(reader("TongTNChiuthue")).ToString
                    tongtntinhthue = Convert.ToDouble(reader("TongTNTinhthue")).ToString
                    sothue = Convert.ToDouble(reader("ThueTNCN")).ToString
                    matracuu = reader("Matracuu")
                    ghichu = reader("GhichuCTLQ")
                    khoanthunhap = reader("KhoanTN")
                    denthang = reader("Denthang")
                    baohiem = Convert.ToDouble(reader("Baohiem")).ToString
                    tuthien = Convert.ToDouble(reader("TThien")).ToString

                End While

            End If

            cmd.Dispose()
            connection.Close()
            connection.Dispose()
            SqlConnection.ClearAllPools()
        Catch ex As SqlException
            Return String.Empty
        End Try
        Dim base64chungthu As String = String.Empty
        If TinhchatCT > 0 Then
            'xml dieu chinh/thaythe
            Dim doc As New XDocument(
           New XDeclaration("1.0", "utf-8", "yes"),
           New XElement("CTu",
               New XElement("DLCTu",
                   New XAttribute("Id", "_" & idct),
                   New XElement("TTChung",
                       New XElement("PBan", phienban),
                       New XElement("TCTu", tenctu),
                       New XElement("MSCTu", msoctu),
                       New XElement("KHCTu", kyhieuct),
                       New XElement("SCTu", sochungtu),
                       New XElement("NLap", ngaylap),
                       New XElement("TTCTLQuan",
                            New XElement("TCCTu", TinhchatCT),
                            New XElement("LHCTLQuan", loaictlquan),
                            New XElement("KHMSCTCLQuan", khmsctlquan),
                            New XElement("KHCTCLQuan", kyhieuctlquan),
                            New XElement("SCTCLQuan", soctulquan),
                            New XElement("NLCTCLQuan", ngaylapctlquan),
                            New XElement("GChu", ghichu)
                        ),
                       New XElement("TTKhac",
                           New XElement("TTin",
                           New XElement("TTruong", "MTCuu"),
                           New XElement("KDLieu", "string"),
                           New XElement("DLieu", matracuu)
                           )
                       )
                   ),
                   New XElement("NDCTu",
                       New XElement("TCTTNhap",
                           New XElement("Ten", tentochuc),
                           New XElement("MST", msttochuc),
                           New XElement("DChi", diachitc),
                           New XElement("SDThoai", sdttochuc)
                       ),
                       New XElement("NNT",
                           New XElement("Ten", tennnt),
                           New XElement("MST", mstnnt),
                           New XElement("DChi", dchinnt),
                           New XElement("QTich", qtich),
                           New XElement("CNCTru", canhancutru),
                           New XElement("CCCDan", cccdan_nnt),
                            New XElement("SDThoai", sdtnnt),
                           New XElement("DCTDTu", emailnnt)
                       ),
                       New XElement("TTNCNKTru",
                           New XElement("KTNhap", khoanthunhap),
                           New XElement("TThang", tuthang),
                           New XElement("DThang", denthang),
                           New XElement("Nam", nam),
                           New XElement("BHiem", baohiem),
                           New XElement("TThien", tuthien),
                           New XElement("TTNCThue", tongtnchiuthue),
                           New XElement("TTNTThue", tongtntinhthue),
                           New XElement("SThue", sothue)
                       )
                   )
               ),
               New XElement("DSCKS",
                   New XElement("TCTTNhap")
               )
           )
       )

            Dim byte1 As Byte() = Text.Encoding.UTF8.GetBytes(doc.ToString)
            base64chungthu = Convert.ToBase64String(byte1)

        Else
            'xml moi
            Dim doc As New XDocument(
           New XDeclaration("1.0", "utf-8", "yes"),
           New XElement("CTu",
               New XElement("DLCTu",
                   New XAttribute("Id", "_" & idct),
                   New XElement("TTChung",
                       New XElement("PBan", phienban),
                       New XElement("TCTu", tenctu),
                       New XElement("MSCTu", msoctu),
                       New XElement("KHCTu", kyhieuct),
                       New XElement("SCTu", sochungtu),
                       New XElement("NLap", ngaylap),
                       New XElement("TTKhac",
                           New XElement("TTin",
                               New XElement("TTruong", "MTCuu"),
                               New XElement("KDLieu", "string"),
                               New XElement("DLieu", matracuu)
                           )
                       )
                   ),
                   New XElement("NDCTu",
                       New XElement("TCTTNhap",
                           New XElement("Ten", tentochuc),
                           New XElement("MST", msttochuc),
                           New XElement("DChi", diachitc),
                           New XElement("SDThoai", sdttochuc)
                       ),
                       New XElement("NNT",
                           New XElement("Ten", tennnt),
                           New XElement("MST", mstnnt),
                           New XElement("DChi", dchinnt),
                           New XElement("QTich", qtich),
                           New XElement("CNCTru", canhancutru),
                           New XElement("CCCDan", cccdan_nnt),
                            New XElement("SDThoai", sdtnnt),
                           New XElement("DCTDTu", emailnnt)
                       ),
                       New XElement("TTNCNKTru",
                           New XElement("KTNhap", khoanthunhap),
                           New XElement("TThang", tuthang),
                           New XElement("DThang", denthang),
                           New XElement("Nam", nam),
                           New XElement("BHiem", baohiem),
                           New XElement("TThien", tuthien),
                           New XElement("TTNCThue", tongtnchiuthue),
                           New XElement("TTNTThue", tongtntinhthue),
                           New XElement("SThue", sothue)
                       )
                   )
               ),
               New XElement("DSCKS",
                   New XElement("TCTTNhap")
               )
           )
       )

            Dim byte1 As Byte() = Text.Encoding.UTF8.GetBytes(doc.ToString)
            base64chungthu = Convert.ToBase64String(byte1)

        End If

        Return base64chungthu
    End Function



    Private Function UpdateXMLCTu(MaCT As Integer, XMLChungtu As String) As Integer
        Dim kq = 0
        If Not Equals(XMLChungtu, "") Then
            Dim Sql As String = "Update ChungtuthueTNCN set XMLChungtu=@XMLChungtu,TinhtrangCT=1 where  MaCT='" & MaCT.ToString() & "'"
            Dim conn As SqlConnection = New SqlConnection(connectionString)
            conn.Open()
            Dim comm As SqlCommand = New SqlCommand(Sql, conn)
            comm.Parameters.AddWithValue("@XMLChungtu", XMLChungtu)
            kq = comm.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            SqlConnection.ClearAllPools()
        End If
        Return kq
    End Function



    Public Function GetTTDonvi(madv As String) As thongtindv
        Dim res As New thongtindv
        Dim sContent As String = String.Empty
        Dim conn As New SqlConnection(connectionString)
        conn.Open()
        Dim sql_Check As String = String.Format("Select * from donvi where  ma_dv = '{0}'", madv)
        Dim comm_Check As New SqlCommand(sql_Check, conn)
        Dim reader As SqlDataReader = comm_Check.ExecuteReader
        If reader.HasRows Then
            Dim item As New thongtindv
            While reader.Read
                item.madv = madv.Trim
                If reader("ten_dv") IsNot DBNull.Value Then
                    item.tendv = reader("ten_dv").ToString
                Else
                    item.tendv = String.Empty
                End If

                If reader("dia_chi") IsNot DBNull.Value Then
                    item.diachi = reader("dia_chi").ToString
                Else
                    item.diachi = String.Empty
                End If
                If reader("dien_thoai") IsNot DBNull.Value Then
                    item.dienthoai = reader("dien_thoai").ToString
                Else
                    item.dienthoai = String.Empty
                End If
                If reader("email") IsNot DBNull.Value Then
                    item.email = reader("email").ToString
                Else
                    item.email = String.Empty
                End If
            End While
            reader.Close()
            res = item
        Else
            res = New thongtindv

        End If
        conn.Close()
        conn.Dispose()
        comm_Check.Dispose()
        Return res
    End Function



    <WebMethod()>
    Public Function Capnhatchungtudaky(idhd As String, signedxml As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                Dim sql As String = "UPDATE ChungtuthueTNCN SET XMLChungtu = @signedxml, TinhtrangCT = 2 WHERE MaCT = @idhd"
                Using comm As New SqlCommand(sql, conn)
                    comm.Parameters.AddWithValue("@signedxml", signedxml)
                    comm.Parameters.AddWithValue("@idhd", idhd)

                    Dim rowsAffected As Integer = comm.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        response("status") = "success"
                        response("message") = "Cập nhật chứng từ thành công"
                    Else
                        response("status") = "error"
                        response("message") = "Không tìm thấy chứng từ cần cập nhật"
                    End If
                End Using
            End Using

            SqlConnection.ClearAllPools()
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    '<WebMethod()>
    'Public Function LaysoCT(mact As String) As String
    '    Dim response As New Dictionary(Of String, Object)()

    '    Try
    '        Dim client As New WSHoadonCA2.WSHoadonCA2()
    '        Dim resp As WSHoadonCA2.KQCapnhatsoCT = client.Capnhatsochungtu_CTKhongso(mact)

    '        If resp IsNot Nothing Then
    '            response("status") = "success"
    '            response("message") = "Cập nhật chứng từ thành công"
    '            response("data") = resp
    '        Else
    '            response("status") = "error"
    '            response("message") = "Không nhận được dữ liệu từ service"
    '        End If

    '    Catch ex As Exception
    '        response("status") = "error"
    '        response("message") = "Lỗi hệ thống: " & ex.Message
    '    End Try

    '    Return JsonConvert.SerializeObject(response)
    'End Function


    Private Function LoadDataEdit(mact As String, MasothueTC As String) As thongtinCT
        Dim item As New thongtinCT
        Try
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "Select * from ChungtuthueTNCN where MaCT = '" & mact & "' And MasothueTC= '" & MasothueTC & "' "
            comm.CommandTimeout = 0
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    item.MaCT = mact
                    item.TenCT = reader("Tenchungtu")
                    item.MSChungtu = reader("MSChungtu")
                    item.KHChungtu = reader("KHChungtu")
                    item.Sochungtu = reader("Sochungtu")
                    item.NgaylapCT = reader("NgaylapCT")
                    item.TinhchatCT = reader("TinhchatCT")
                    item.TenTC = reader("TenTC")
                    item.MasothueTC = reader("MasothueTC")
                    item.DiachiTC = reader("DiachiTC")
                    item.DienthoaiTC = reader("DienthoaiTC")
                    item.MasothueNNT = reader("MasothueNNT")
                    item.TenNNT = reader("TenNNT")
                    item.DiachiNNT = reader("DiachiNNT")
                    item.QuoctichNNT = reader("QuoctichNNT")
                    item.CanhanCT = reader("CanhanCT")
                    item.SoCMND = reader("SoCMND")

                    item.LoaiCTLienquan = reader("LoaiCTLienquan")
                    item.KHMSCTLienquan = reader("KHMSCTLienquan")
                    item.KHCTLienquan = reader("KHCTLienquan")
                    item.SoCTLienquan = reader("SoCTLienquan")
                    item.NgaylapCTLienquan = reader("NgaylapCTLienquan")


                    If reader("NgaycapCMND") IsNot DBNull.Value Then
                        item.NgaycapCMND = reader("NgaycapCMND")
                    Else
                        item.NgaycapCMND = String.Empty
                    End If

                    If reader("NoicapCMND") IsNot DBNull.Value Then
                        item.NoicapCMND = reader("NoicapCMND")
                    Else
                        item.NoicapCMND = String.Empty
                    End If

                    item.DienthoaiNNT = reader("DienthoaiNNT")
                    item.EmailNNT = reader("EmailNNT")

                    If reader("ThunhapCN") IsNot DBNull.Value Then
                        item.ThunhapCN = reader("ThunhapCN")
                    Else
                        item.ThunhapCN = String.Empty
                    End If

                    item.ThangTN = reader("ThangTN")
                    item.NamTN = reader("NamTN")

                    If reader("TongTNChiuthue") IsNot DBNull.Value Then
                        item.TongTNChiuthue = reader("TongTNChiuthue")
                    Else
                        item.TongTNChiuthue = 0
                    End If

                    If reader("TongTNTinhthue") IsNot DBNull.Value Then
                        item.TongTNTinhthue = reader("TongTNTinhthue")
                    Else
                        item.TongTNTinhthue = 0
                    End If

                    If reader("ThueTNCN") IsNot DBNull.Value Then
                        item.ThueTNCN = reader("ThueTNCN")
                    Else
                        item.ThueTNCN = 0
                    End If

                    If reader("Denthang") IsNot DBNull.Value Then
                        item.Denthang = reader("Denthang")
                    Else
                        item.Denthang = String.Empty
                    End If
                    If reader("Baohiem") IsNot DBNull.Value Then
                        item.Baohiem = reader("Baohiem")
                    Else
                        item.Baohiem = 0
                    End If

                    If reader("SothunhapDN") IsNot DBNull.Value Then
                        item.SoTNDN = reader("SothunhapDN")
                    Else
                        item.SoTNDN = 0
                    End If
                    If reader("TThien") IsNot DBNull.Value Then
                        item.TThien = reader("TThien")
                    Else
                        item.TThien = 0
                    End If



                End While

            End If
            reader.Close()
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            SqlConnection.ClearAllPools()

        Catch ex As Exception
            Dim err As String = ex.Message
        End Try

        Return item
    End Function




    <WebMethod()>
    Public Function Inchuyendoi(mact As String, madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            If String.IsNullOrEmpty(mact) Then
                response("status") = "error"
                response("message") = "Mã chứng từ không hợp lệ"
                Return JsonConvert.SerializeObject(response)
            End If

            Dim trangthai As String = LoadData(mact, "TinhtrangCT")

            If Not String.IsNullOrEmpty(trangthai) AndAlso Convert.ToInt32(trangthai) = 33 Then
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Using comm As New SqlCommand("update ChungtuthueTNCN set TinhtrangCT=3 where MaCT =@mact and MasothueTC=@madv", conn)
                        comm.CommandTimeout = 0
                        comm.Parameters.AddWithValue("@mact", mact)
                        comm.Parameters.AddWithValue("@madv", madonvi)
                        comm.ExecuteNonQuery()
                    End Using
                End Using
                SqlConnection.ClearAllPools()
                response("status") = "success"
                response("message") = "In chuyển đổi thành công"
                response("data") = mact
            Else
                response("status") = "error"
                response("message") = "In chuyển đổi không thành công"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    Private Function LoadData(idhd As String, fieldname As String) As String
        Dim res As String = String.Empty
        Try
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "Select " & fieldname & " from ChungtuthueTNCN where MaCT = '" & idhd & "'"
            comm.CommandTimeout = 0
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    res = reader(0).ToString
                End While
            Else
                res = String.Empty
            End If
            reader.Close()
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            SqlConnection.ClearAllPools()
            Return res
        Catch ex As Exception
            Return res
        End Try
    End Function

    Private Function CheckTrangthaiGuithue_chungtu(mact As String) As Integer
        Dim res As Integer = 0
        Try
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "Select KQ213 from ChungtuthueTNCN where MaCT =@mact"
            comm.Parameters.AddWithValue("@mact", mact)
            comm.CommandTimeout = 0
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    res = Convert.ToInt16(reader(0).ToString)
                End While
            Else
                res = 0
            End If
            reader.Close()
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            SqlConnection.ClearAllPools()
            Return res
        Catch ex As Exception
            Return res
        End Try
    End Function
    Private Function UpdateKhoaphienCTSauGui(TinhtrangCT As String, MasothueTC As String, MaCT As Integer, KhoaphienTN As String) As Integer
        Dim res As Integer = 0
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using cmd As New SqlCommand("UPDATE ChungtuthueTNCN SET TinhtrangCT=@TinhtrangCT, KhoaphienTN=@KhoaphienTN WHERE MasothueTC=@MasothueTC AND MaCT=@MaCT", conn)
                cmd.Parameters.Add("@TinhtrangCT", SqlDbType.NVarChar, 50).Value = TinhtrangCT
                cmd.Parameters.Add("@MasothueTC", SqlDbType.NVarChar, 20).Value = MasothueTC
                cmd.Parameters.Add("@MaCT", SqlDbType.Int).Value = MaCT
                cmd.Parameters.Add("@KhoaphienTN", SqlDbType.NVarChar, 250).Value = KhoaphienTN
                Try
                    res = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    res = 0
                End Try
            End Using

            Using cmdd As New SqlCommand("insert into KQTNChungtu(KhoaphienTN,MaCT,MSChungtu,KHChungtu,Sochungtu,NgaylapCT,Ngaygui) select a.KhoaphienTN,a.MaCT,a.MSChungtu,a.KHChungtu,a.Sochungtu,a.NgaylapCT,getdate() from ChungtuthueTNCN a left join KQTNChungtu b on a.KhoaphienTN=b.KhoaphienTN where a.KhoaphienTN=@KhoaphienTN and b.KhoaphienTN is null", conn)
                cmdd.Parameters.Add("@KhoaphienTN", SqlDbType.NVarChar, 250).Value = KhoaphienTN
                Try
                    'conn.Open()
                    res = cmdd.ExecuteNonQuery()
                Catch ex As Exception
                    res = 0
                End Try
            End Using

        End Using
        Return res
    End Function


    Private Function InsertThongdiep211(xmlthongdiep As String, key As String, mltdiep As String, mtdtchieu As String, khoaphien As String, madonvi As String) As Integer
        Dim res As Integer = 0
        Try
            Dim sql As String = "insert into Logtruyennhan(Phienban,MNGui,MNNhan,MLTDiep,MTDiep,MST,SLuong,XMLThongdiep,Phanloaithongdiep,Thoigian,Trangthai,MTDTChieu,Khoaphien)values ('2.1.0','0103930279','0103930279','" & mltdiep & "','" & key & "','" & madonvi & "','1','" & xmlthongdiep & "','','" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "','1','" & mtdtchieu & "','" & khoaphien & "') "
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            res = 1
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
            SqlConnection.ClearAllPools()
        Catch ex As Exception
            res = 0
        End Try

        Return res
    End Function


    Public Function Taothongdiep211(MSTTCGP As String, MST As String, MaCT As String) As String
        Dim kq = ""


        'Hợp lệ
        Dim dsmtt As DataTable = LayDSChungtu70(MST, MaCT)
        Dim dem_dsmtt As Integer = dsmtt.Rows.Count
        If dem_dsmtt = 0 Then
            Return ""
        End If
        Dim MNGui = MSTTCGP
        Dim MNNhan = "0103930279"
        Dim MLTDiep = "211"
        Dim MTDiep = ""
        Dim code As String = Guid.NewGuid().ToString().Replace("-", "")
        code = code.ToUpper()
        MTDiep = MNGui & code
        Dim MTDTChieu = ""
        Dim SLuong As String = dem_dsmtt.ToString()
        Dim keyid = MTDiep
        ' Tao thong tin XML chung

        Dim linkelement = ""

        Dim doc = New XmlDocument()
        Dim docNode As XmlNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(docNode)

        ' The TDiep
        Dim TDiepNode As XmlElement = doc.CreateElement("", "TDiep", linkelement)
        doc.AppendChild(TDiepNode)
        ' TT Chung
        Dim TTChungTDNode As XmlElement = doc.CreateElement("", "TTChung", linkelement)
        TDiepNode.AppendChild(TTChungTDNode)
        ' PBan
        Dim PBanTTNode As XmlNode = doc.CreateElement("", "PBan", linkelement)
        PBanTTNode.AppendChild(doc.CreateTextNode("2.1.0"))
        TTChungTDNode.AppendChild(PBanTTNode)
        ' MNGui
        Dim MNGuiNode As XmlNode = doc.CreateElement("", "MNGui", linkelement)
        MNGuiNode.AppendChild(doc.CreateTextNode(MNGui))
        TTChungTDNode.AppendChild(MNGuiNode)
        ' MNNhan
        Dim MNNhanNode As XmlNode = doc.CreateElement("", "MNNhan", linkelement)
        MNNhanNode.AppendChild(doc.CreateTextNode(MNNhan))
        TTChungTDNode.AppendChild(MNNhanNode)
        ' MLTDiep
        Dim MLTDiepNode As XmlNode = doc.CreateElement("", "MLTDiep", linkelement)
        MLTDiepNode.AppendChild(doc.CreateTextNode(MLTDiep))
        TTChungTDNode.AppendChild(MLTDiepNode)
        ' MTDiep
        Dim MTDiepNode As XmlNode = doc.CreateElement("", "MTDiep", linkelement)
        MTDiepNode.AppendChild(doc.CreateTextNode(MTDiep))
        TTChungTDNode.AppendChild(MTDiepNode)
        ' MTDTChieu
        Dim MTDTChieuNode As XmlNode = doc.CreateElement("", "MTDTChieu", linkelement)
        MTDTChieuNode.AppendChild(doc.CreateTextNode(MTDTChieu))
        TTChungTDNode.AppendChild(MTDTChieuNode)
        ' MST
        Dim MSTTTNode As XmlNode = doc.CreateElement("", "MST", linkelement)
        MSTTTNode.AppendChild(doc.CreateTextNode(MST))
        TTChungTDNode.AppendChild(MSTTTNode)
        ' SLuong
        Dim SLuongNode As XmlNode = doc.CreateElement("", "SLuong", linkelement)
        SLuongNode.AppendChild(doc.CreateTextNode(SLuong))
        TTChungTDNode.AppendChild(SLuongNode)
        ' DLieu

        Dim DLieuNode As XmlElement = doc.CreateElement("", "DLieu", linkelement)
        'Dim productAttribute As XmlAttribute = doc.CreateAttribute("Id")
        'productAttribute.Value = "_" & keyid
        'DLieuNode.Attributes.Append(productAttribute)
        TDiepNode.AppendChild(DLieuNode)

        Dim lstNode As XmlNodeList = doc.GetElementsByTagName("DLieu")

        Dim dlHDon = String.Empty
        For i = 0 To dem_dsmtt - 1
            Dim base64xml As String = dsmtt.Rows(i)("XMLChungtu").ToString()
            ' add node HDon
            Dim convert = XmlStringToXmlNode(base64xml)
            Dim xnode As XmlNode = lstNode(0)
            xnode.AppendChild(xnode.OwnerDocument.ImportNode(convert, True))
        Next

        ''' CKSNNT
        'XmlElement CKSNNT = doc.CreateElement("", "CKSNNT", linkelement);
        'TDiepNode.AppendChild(CKSNNT);

        kq = doc.InnerXml
        Return kq ' Tài khoản tích hợp hóa đơn không hợp lệ
    End Function


    <WebMethod()>
    Public Function LayThongTinchungTuGoc(madonvi As String, mausogoc As String, kyhieugoc As String, soctgoc As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim result As CTGocResult = CheckMaCTGoc(mausogoc, kyhieugoc, soctgoc, madonvi)
            response("status") = "success"
            response("data") = result
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function




    Private Function LayDSChungtu70(ByVal MST As String, ByVal MaCT As String) As DataTable
        Dim dt As DataTable = New DataTable("DSMTT")
        Dim conn = New SqlConnection()
        Dim comm = New SqlCommand()
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn

        comm.CommandText = "select ngaylapct,Sochungtu,XMLChungtu from ChungtuthueTNCN where MasothueTC=@MST and MaCT=@MaCT and MSChungtu='03/TNCN' and khoaphienTN is null"
        comm.Parameters.Add("@MaCT", System.Data.SqlDbType.NVarChar).Value = MaCT
        comm.Parameters.Add("@MST", System.Data.SqlDbType.NVarChar).Value = MST

        comm.CommandTimeout = 0
        Dim adapter As SqlDataAdapter = New SqlDataAdapter()
        adapter.SelectCommand = comm
        adapter.Fill(dt)
        adapter.Dispose()
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return dt
    End Function

    <WebMethod()>
    Public Function LayKQTruyennhanchungtudientu(ByVal MaDV As String, ByVal dskhoaphien As String) As DataTable
        Dim dt As DataTable = New DataTable("Danhsachketquachungtu")

        If String.IsNullOrWhiteSpace(dskhoaphien) Then Return dt
        Dim dtKhoaphien As DataTable = New DataTable()
        dtKhoaphien.Columns.Add("Khoaphien", GetType(String))

        For Each kp In dskhoaphien.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
            dtKhoaphien.Rows.Add(kp.Trim())
        Next

        Try

            Using connection As SqlConnection = New SqlConnection(ConnectionStringtvan)
                connection.Open()
                Dim createTempTable As String = "CREATE TABLE #tmp_Khoaphien (Khoaphien NVARCHAR(250));"

                Using cmdCreate As SqlCommand = New SqlCommand(createTempTable, connection)
                    cmdCreate.ExecuteNonQuery()
                End Using

                Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(connection)
                    bulkCopy.DestinationTableName = "#tmp_Khoaphien"
                    bulkCopy.WriteToServer(dtKhoaphien)
                End Using

                Dim myQuery As String =
                "WITH ALLTDIEP AS (" & vbCrLf &
                "    SELECT" & vbCrLf &
                "        a.Khoaphien," & vbCrLf &
                "        MTDiep," & vbCrLf &
                "        MLTDiep," & vbCrLf &
                "        MNGui," & vbCrLf &
                "        MTDTchieu," & vbCrLf &
                "        XMLThongdiep" & vbCrLf &
                "    FROM Logtruyennhan a inner join #tmp_Khoaphien b on a.khoaphien=b.khoaphien" & vbCrLf &
                "    WHERE a.MST=@MST" & vbCrLf &
                ")," & vbCrLf &
                "CTE_TDGuiGoc AS (" & vbCrLf &
                "    SELECT Khoaphien, MTDiep, mltdiep, MNgui" & vbCrLf &
                "    FROM ALLTDIEP" & vbCrLf &
                "    WHERE MNGui = 'V0103930279' AND mltdiep <> '999'" & vbCrLf &
                ")," & vbCrLf &
                "CTE_TD999 AS (" & vbCrLf &
                "    SELECT MTDTchieu," & vbCrLf &
                "           CASE " & vbCrLf &
                "               WHEN CHARINDEX('TTTNhan', evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep,'')) > 0 " & vbCrLf &
                "               THEN evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep, 'TDiep/DLieu/TBao/TTTNhan')" & vbCrLf &
                "               ELSE 0" & vbCrLf &
                "           END AS KQ999" & vbCrLf &
                "    FROM ALLTDIEP" & vbCrLf &
                "    WHERE MNGui = 'TCT' AND mltdiep = '999'" & vbCrLf &
                ")," & vbCrLf &
                "CTE_TD213 AS (" & vbCrLf &
                "    SELECT MTDTchieu, KQ213," & vbCrLf &
                "           CTu.value('(MSCTu)[1]','nvarchar(10)') AS MSCTu," & vbCrLf &
                "           CTu.value('(KHCTu)[1]','nvarchar(255)') AS KHCTu," & vbCrLf &
                "           CTu.value('(SCTu)[1]','nvarchar(255)') AS SCTu," & vbCrLf &
                "           CAST(CTu.query('(DSLDo)[1]') AS NVARCHAR(MAX)) AS DSLDo" & vbCrLf &
                "    FROM (" & vbCrLf &
                "         SELECT MTDTchieu," & vbCrLf &
                "                evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep, 'TDiep/DLieu/TBao/DLTBao/LTBao') AS KQ213," & vbCrLf &
                "                CAST(NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'DSCTu','LCTu') AS XML) AS XmlData" & vbCrLf &
                "         FROM ALLTDIEP" & vbCrLf &
                "         WHERE MNGui = 'TCT' AND mltdiep='213'" & vbCrLf &
                "    ) AS T OUTER APPLY XmlData.nodes('/CTu') AS L(CTu)" & vbCrLf &
                ")" & vbCrLf &
                "SELECT DISTINCT b.Khoaphien AS KhoaphienTN, c.KQ999, d.KQ213, MSCTu, KHCTu, SCTu, DSLDo" & vbCrLf &
                "FROM ALLTDIEP a" & vbCrLf &
                "INNER JOIN CTE_TDGuiGoc b ON a.MNgui = b.MNgui" & vbCrLf &
                "LEFT JOIN CTE_TD999 c ON b.MTDiep = c.MTDTchieu" & vbCrLf &
                "LEFT JOIN CTE_TD213 d ON b.MTDiep = d.MTDTchieu"


                Using myCommand As SqlCommand = New SqlCommand(myQuery, connection)
                    myCommand.Parameters.Add("@MST", SqlDbType.NVarChar).Value = MaDV

                    Using adapter As SqlDataAdapter = New SqlDataAdapter(myCommand)
                        adapter.Fill(dt)
                        Dim capnhatkq As Integer = CapnhatKQTruyennhanchungtudientu(dt)
                    End Using
                End Using

                Using cmdDrop As SqlCommand = New SqlCommand("DROP TABLE #tmp_Khoaphien;", connection)
                    cmdDrop.ExecuteNonQuery()
                End Using

                SqlConnection.ClearAllPools()
            End Using

        Catch ex As Exception
            Dim msg As String = ex.ToString()
        End Try

        XuLyNullTrongDataTable(dt)
        Return dt
    End Function

    Private Sub XuLyNullTrongDataTable(ByVal dt As DataTable)
        For Each row As DataRow In dt.Rows
            For Each col As DataColumn In dt.Columns
                If IsDBNull(row(col)) Then
                    Dim dataType As Type = col.DataType

                    If dataType Is GetType(String) Then
                        row(col) = ""
                    ElseIf dataType Is GetType(Integer) OrElse dataType Is GetType(Long) OrElse dataType Is GetType(Short) Then
                        row(col) = 0
                    ElseIf dataType Is GetType(Decimal) OrElse dataType Is GetType(Single) OrElse dataType Is GetType(Double) Then
                        row(col) = 0.0
                    ElseIf dataType Is GetType(DateTime) Then
                        row(col) = New DateTime(1900, 1, 1)
                    ElseIf dataType Is GetType(Boolean) Then
                        row(col) = False
                    Else
                        row(col) = "Không có DL" ' Nếu không biết thì để null (hoặc gán tùy theo yêu cầu)
                    End If
                End If
            Next
        Next
        dt.AcceptChanges()
    End Sub



    Private Function CapnhatKQTruyennhanchungtudientu(ByVal KetquaTN As DataTable) As Integer
        Dim kqcn As Integer = 0
        If KetquaTN Is Nothing OrElse KetquaTN.Rows.Count = 0 Then Return -1

        Try

            Using connection As SqlConnection = New SqlConnection(connectionString)
                connection.Open()
                Dim createTempTable As String = "CREATE TABLE #tmp_KQTN ( " &
                    "KhoaphienTN NVARCHAR(250), " &
                    "KQ999 NVARCHAR(10), " &
                    "KQ213 NVARCHAR(10), " &
                    "MSCTu NVARCHAR(10), " &
                    "KHCTu NVARCHAR(10), " &
                    "SCTu NVARCHAR(10)," &
                    "DSLDo NVARCHAR(MAX));"

                Using cmdCreate As SqlCommand = New SqlCommand(createTempTable, connection)
                    cmdCreate.ExecuteNonQuery()
                End Using

                Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(connection)
                    bulkCopy.DestinationTableName = "#tmp_KQTN"
                    bulkCopy.WriteToServer(KetquaTN)
                End Using

                Dim update1 As String = "UPDATE a Set a.KQ999 = b.KQ999, a.KQ213 = b.KQ213, a.Ngaycapnhatcuoi = GETDATE() FROM KQTNChungtu a inner JOIN #tmp_KQTN b On a.KhoaphienTN = b.KhoaphienTN ;"

                Using cmdUpdate1 As SqlCommand = New SqlCommand(update1, connection)
                    kqcn = cmdUpdate1.ExecuteNonQuery()
                End Using

                If kqcn > 0 Then
                    ' Cập nhật DSLDo nếu KQ213 là 10 hoặc 12
                    Dim update2 As String =
                    "UPDATE a " & vbCrLf &
                    "SET " & vbCrLf &
                    "    a.DSLDo = b.DSLDo, " & vbCrLf &
                    "    a.Ngaycapnhatcuoi = GETDATE() " & vbCrLf &
                    "FROM KQTNChungtu a " & vbCrLf &
                    "INNER JOIN #tmp_KQTN b " & vbCrLf &
                    "    ON a.KhoaphienTN = b.KhoaphienTN " & vbCrLf &
                    "    AND a.MSChungtu = b.MSCTu " & vbCrLf &
                    "    AND a.KHChungtu = b.KHCTu " & vbCrLf &
                    "    AND Convert(int, a.Sochungtu) = b.SCTu " & vbCrLf &
                    "WHERE a.KQ213 IN ('10', '12');"


                    Using cmdUpdate2 As SqlCommand = New SqlCommand(update2, connection)
                        Dim check As Integer = cmdUpdate2.ExecuteNonQuery()
                    End Using
                End If

                Using cmdDrop As SqlCommand = New SqlCommand("DROP TABLE #tmp_KQTN;", connection)
                    cmdDrop.ExecuteNonQuery()
                End Using

                SqlConnection.ClearAllPools()
            End Using

        Catch ex As Exception
            Dim msg As String = ex.ToString()
            kqcn = -2
        End Try

        Return kqcn
    End Function


    Public Function CapnhatKhoaphienchungtuchuacapnhatKQ(ByVal MaDV As String, ByVal KHCTu As String) As String
        Dim kqBuilder As New StringBuilder()

        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim myQuery As String = "SELECT DISTINCT a.KhoaphienTN " &
                        "FROM KQTNChungtu a " &
                        "INNER JOIN ChungtuthueTNCN b ON a.KhoaphienTN = b.KhoaphienTN " &
                        "WHERE ((a.KQ999 = '0' AND a.KQ213 IS NULL) OR a.KQ999 IS NULL) " &
                        "AND a.MSChungtu = '03/TNCN' " &
                        "AND a.KHChungtu = @khctu " &
                        "AND Masothuetc = @madv"

                Using cmd As New SqlCommand(myQuery, connection)
                    cmd.Parameters.Add("@madv", SqlDbType.VarChar).Value = MaDV
                    cmd.Parameters.Add("@khctu", SqlDbType.VarChar).Value = KHCTu

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                kqBuilder.Append(reader.GetValue(0).ToString()).Append(",")
                            End If
                        End While
                    End Using
                End Using
            End Using

            If kqBuilder.Length > 0 Then
                kqBuilder.Length -= 1 ' Xóa dấu phẩy cuối
            End If
        Catch ex As Exception
            ' Xử lý ngoại lệ nếu cần
        End Try

        If Not String.IsNullOrEmpty(kqBuilder.ToString()) Then
            LayKQTruyennhanchungtudientu(MaDV, kqBuilder.ToString())
        End If

        Return kqBuilder.ToString()
    End Function


    <WebMethod()>
    Public Function LayDanhSachChungTuNhap(loaiTimKiem As Integer,
                                       mauso As String,
                                       kyhieu As String,
                                       tungay As String,
                                       denngay As String,
                                       sochungtu As String,
                                       matracuu As String,
                                       madonvi As String,
                                       pageIndex As Integer,
                                       pageSize As Integer) As String

        ' loaiTimKiem: 0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu

        Dim response As New Dictionary(Of String, Object)()
        Dim sql As String = ""
        Dim sqlCount As String = ""
        Dim dt As New DataTable("DS")
        Dim totalRecords As Integer = 0

        Try
            '--- Xây dựng SQL
            If mauso = "03/TNCN" Then
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                        "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 "

                        ' chỉ thêm điều kiện ngày khi có giá trị
                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= "AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"

                    Case 1
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a  WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                    Case 2
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                End Select
            Else
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                        "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 "

                        ' chỉ thêm điều kiện ngày khi có giá trị
                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= "AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"
                    Case 1
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT=1"
                    Case 2
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT=1"
                End Select
            End If

            ' Câu count (loại bỏ ORDER BY, phân trang)
            sqlCount = "SELECT COUNT(*) FROM (" & sql & ") AS T"
            ' Thêm order by + phân trang nếu chưa có
            sql &= " ORDER BY a.MaCT DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"

            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Đếm tổng bản ghi
                Using countCmd As New SqlCommand(sqlCount, connection)
                    countCmd.Parameters.AddWithValue("@MaDV", madonvi)
                    countCmd.Parameters.AddWithValue("@MS", mauso)
                    countCmd.Parameters.AddWithValue("@KH", kyhieu)
                    If sql.Contains("@TuNgay") Then countCmd.Parameters.AddWithValue("@TuNgay", tungay)
                    If sql.Contains("@DenNgay") Then countCmd.Parameters.AddWithValue("@DenNgay", denngay)
                    If sql.Contains("@SoCT") Then countCmd.Parameters.AddWithValue("@SoCT", sochungtu)
                    If sql.Contains("@matracuu") Then countCmd.Parameters.AddWithValue("@matracuu", matracuu)
                    totalRecords = Convert.ToInt32(countCmd.ExecuteScalar())
                End Using

                ' Lấy dữ liệu trang
                Using cmd As New SqlCommand(sql, connection)
                    cmd.Parameters.AddWithValue("@MaDV", madonvi)
                    cmd.Parameters.AddWithValue("@MS", mauso)
                    cmd.Parameters.AddWithValue("@KH", kyhieu)
                    If sql.Contains("@TuNgay") Then cmd.Parameters.AddWithValue("@TuNgay", tungay)
                    If sql.Contains("@DenNgay") Then cmd.Parameters.AddWithValue("@DenNgay", denngay)
                    If sql.Contains("@SoCT") Then cmd.Parameters.AddWithValue("@SoCT", sochungtu)
                    If sql.Contains("@matracuu") Then cmd.Parameters.AddWithValue("@matracuu", matracuu)
                    Dim offset As Integer = (pageIndex - 1) * pageSize
                    cmd.Parameters.AddWithValue("@Offset", offset)
                    cmd.Parameters.AddWithValue("@PageSize", pageSize)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))

            response("status") = "success"
            response("data") = dt
            response("pagination") = New With {
            .pageIndex = pageIndex,
            .pageSize = pageSize,
            .totalRecords = totalRecords,
            .totalPages = totalPages
        }

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function LayDanhSachChungTuDaKy(loaiTimKiem As Integer,
                                       mauso As String,
                                       kyhieu As String,
                                       tungay As String,
                                       denngay As String,
                                       sochungtu As String,
                                       matracuu As String,
                                       madonvi As String,
                                       pageIndex As Integer,
                                       pageSize As Integer) As String

        Dim response As New Dictionary(Of String, Object)()
        Dim sql As String = ""
        Dim sqlCount As String = ""
        Dim dt As New DataTable("DS")
        Dim totalRecords As Integer = 0

        Try
            '--- Xây dựng SQL
            If mauso = "03/TNCN" Then
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                            "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT IN (2,6) "

                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= " AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"

                    Case 1
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT IN (2,6) AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                    Case 2
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT IN (2,6) AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                End Select
            Else
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                       "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT>1"

                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= " AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"

                    Case 1
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT>1"
                    Case 2
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT>1"
                End Select
            End If

            ' Câu count
            sqlCount = "SELECT COUNT(*) FROM (" & sql & ") AS T"
            ' Thêm order by + phân trang
            sql &= " ORDER BY a.MaCT DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"

            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Đếm tổng bản ghi
                Using countCmd As New SqlCommand(sqlCount, connection)
                    countCmd.Parameters.AddWithValue("@MaDV", madonvi)
                    countCmd.Parameters.AddWithValue("@MS", mauso)
                    countCmd.Parameters.AddWithValue("@KH", kyhieu)
                    If sql.Contains("@TuNgay") Then countCmd.Parameters.AddWithValue("@TuNgay", tungay)
                    If sql.Contains("@DenNgay") Then countCmd.Parameters.AddWithValue("@DenNgay", denngay)
                    If sql.Contains("@SoCT") Then countCmd.Parameters.AddWithValue("@SoCT", sochungtu)
                    If sql.Contains("@matracuu") Then countCmd.Parameters.AddWithValue("@matracuu", matracuu)
                    totalRecords = Convert.ToInt32(countCmd.ExecuteScalar())
                End Using

                ' Lấy dữ liệu trang
                Using cmd As New SqlCommand(sql, connection)
                    cmd.Parameters.AddWithValue("@MaDV", madonvi)
                    cmd.Parameters.AddWithValue("@MS", mauso)
                    cmd.Parameters.AddWithValue("@KH", kyhieu)
                    If sql.Contains("@TuNgay") Then cmd.Parameters.AddWithValue("@TuNgay", tungay)
                    If sql.Contains("@DenNgay") Then cmd.Parameters.AddWithValue("@DenNgay", denngay)
                    If sql.Contains("@SoCT") Then cmd.Parameters.AddWithValue("@SoCT", sochungtu)
                    If sql.Contains("@matracuu") Then cmd.Parameters.AddWithValue("@matracuu", matracuu)
                    Dim offset As Integer = Math.Max(0, (pageIndex - 1) * pageSize)
                    cmd.Parameters.AddWithValue("@Offset", offset)
                    cmd.Parameters.AddWithValue("@PageSize", pageSize)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))

            response("status") = "success"
            response("data") = dt
            response("pagination") = New With {
            .pageIndex = pageIndex,
            .pageSize = pageSize,
            .totalRecords = totalRecords,
            .totalPages = totalPages
        }

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    <WebMethod()>
    Public Function LayDanhSachChungTuDaGuiCQT(loaiTimKiem As Integer,
                                    mauso As String,
                                    kyhieu As String,
                                    tungay As String,
                                    denngay As String,
                                    sochungtu As String,
                                    matracuu As String,
                                    madonvi As String,
                                    pageIndex As Integer,
                                    pageSize As Integer) As String

        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim sql As String = ""
            Dim sqlCount As String = ""
            Dim chuoiketnoi As String = connectionString

            ' Tính offset cho phân trang
            Dim offset As Integer = (pageIndex - 1) * pageSize

            ' Xác định câu SQL
            If mauso = "03/TNCN" Then
                Select Case loaiTimKiem
                    Case 0
                        sql = GetSqlQuery(1, madonvi, tungay, denngay, mauso, kyhieu, "", "", False, True)
                        sqlCount = GetSqlQuery(1, madonvi, tungay, denngay, mauso, kyhieu, "", "", True, False)
                        CapnhatKhoaphienchungtuchuacapnhatKQ(madonvi, kyhieu)
                    Case 1
                        sql = GetSqlQuery(2, madonvi, "", "", mauso, kyhieu, sochungtu, "", False, True)
                        sqlCount = GetSqlQuery(2, madonvi, "", "", mauso, kyhieu, sochungtu, "", True, False)
                        CapnhatKhoaphienchungtuchuacapnhatKQ(madonvi, kyhieu)
                    Case 2
                        sql = GetSqlQuery(3, madonvi, "", "", mauso, kyhieu, "", matracuu, False, True)
                        sqlCount = GetSqlQuery(3, madonvi, "", "", mauso, kyhieu, "", matracuu, True, False)
                        CapnhatKhoaphienchungtuchuacapnhatKQ(madonvi, kyhieu)
                    Case Else
                        sql = String.Empty
                End Select
            End If

            Dim totalRecords As Integer = 0
            Dim totalPages As Integer = 0
            Dim dt As New DataTable("DSHD")

            Using connection As New SqlConnection(chuoiketnoi)
                connection.Open()

                ' Đếm tổng số bản ghi
                Using cmdCount As New SqlCommand(sqlCount, connection)
                    totalRecords = Convert.ToInt32(cmdCount.ExecuteScalar())
                End Using

                totalPages = CInt(Math.Ceiling(totalRecords / pageSize))

                ' Lấy dữ liệu phân trang
                Using cmd As New SqlCommand(sql, connection)
                    cmd.Parameters.AddWithValue("@Offset", offset)
                    cmd.Parameters.AddWithValue("@PageSize", pageSize)

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            response("status") = "success"
            response("data") = dt
            response("pagination") = New With {
            .pageIndex = pageIndex,
            .pageSize = pageSize,
            .totalRecords = totalRecords,
            .totalPages = totalPages
        }

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    Private Function GetSqlQuery(loaiSelect As Integer,
                          maSoThue As String,
                          tuNgay As String,
                          denNgay As String,
                          msChungTu As String,
                          khChungTu As String,
                          soChungtu As String,
                          maTracu As String,
                          isCount As Boolean,
                          enablePaging As Boolean) As String

        ' Xây dựng điều kiện WHERE
        Dim whereFilter As String = ""
        Select Case loaiSelect
            Case 1
                'whereFilter = String.Format(
                '"a.MasothueTC = '{0}' AND (CONVERT(date, a.NgaylapCT) BETWEEN '{1}' AND '{2}') AND a.MSChungtu = '{3}' AND a.KHChungtu = '{4}'",
                'maSoThue, tuNgay, denNgay, msChungTu, khChungTu)

                whereFilter = "a.MasothueTC = '" & maSoThue & "'"

                ' Nếu có khoảng ngày thì mới thêm vào
                If Not String.IsNullOrEmpty(tuNgay) AndAlso Not String.IsNullOrEmpty(denNgay) Then
                    whereFilter &= " AND (CONVERT(date, a.NgaylapCT) BETWEEN '" & tuNgay & "' AND '" & denNgay & "')"
                End If

                ' Nếu có KHChungtu thì mới thêm vào
                If Not String.IsNullOrEmpty(khChungTu) Then
                    whereFilter &= " AND a.KHChungtu = '" & khChungTu & "'"
                End If

                ' Luôn có MSChungtu
                whereFilter &= " AND a.MSChungtu = '" & msChungTu & "'"
            Case 2
                whereFilter = String.Format(
                "a.MasothueTC = '{0}' AND a.Sochungtu='{1}' AND a.MSChungtu = '{2}' AND a.KHChungtu = '{3}'",
                maSoThue, soChungtu, msChungTu, khChungTu)
            Case 3
                whereFilter = String.Format(
                "a.MasothueTC = '{0}' AND a.Matracuu = '{1}' AND a.MSChungtu = '{2}' AND a.KHChungtu = '{3}'",
                maSoThue, maTracu, msChungTu, khChungTu)
            Case Else
                Throw New ArgumentException("Loại select không hợp lệ. Chỉ được 1, 2 hoặc 3.")
        End Select

        If isCount Then
            ' Query đếm bản ghi
            Return "SELECT COUNT(*) " &
               "FROM ChungtuthueTNCN a " &
               "LEFT JOIN KQTNChungtu b ON a.KhoaphienTN = b.khoaphienTN AND a.MaCT = b.mact " &
               "WHERE " & whereFilter & " AND TinhtrangCT IN ('3','33')"
        Else
            ' Query chính có phân trang
            Dim sql As String =
            "WITH dsall AS (" & vbCrLf &
            "    SELECT a.*," & vbCrLf &
            "           CASE " & vbCrLf &
            "                WHEN (b.KQ213 = 2 OR (b.KQ213 IN (10, 12) AND b.DSLDo IS NULL)) THEN N'CQT đã chấp nhận'" & vbCrLf &
            "                WHEN (b.KQ213 IN (10, 12) AND b.DSLDo IS NOT NULL) THEN N'CQT không chấp nhận'" & vbCrLf &
            "                WHEN b.KQ999 = 0 AND b.KQ213 IS NULL THEN N'CQT đã tiếp nhận. Chờ phản hồi'" & vbCrLf &
            "                WHEN b.KQ999 > 0 AND b.KQ213 IS NULL THEN N'Sai định dạng thông điệp'" & vbCrLf &
            "                WHEN a.KhoaphienTN IS NULL  THEN N'Chưa gửi thuế'" & vbCrLf &
            "                ELSE N'CQT chưa tiếp nhận'" & vbCrLf &
            "           END AS TrangthaiguiCQT," & vbCrLf &
            "           b.DSLDo" & vbCrLf &
            "    FROM ChungtuthueTNCN a" & vbCrLf &
            "    LEFT JOIN KQTNChungtu b" & vbCrLf &
            "           ON a.KhoaphienTN = b.khoaphienTN" & vbCrLf &
            "          AND a.MaCT = b.mact" & vbCrLf &
            "    WHERE " & whereFilter & vbCrLf &
            "      AND TinhtrangCT IN ('3', '33')" & vbCrLf &
            ")," & vbCrLf &
            "cre_ctlienquan AS (" & vbCrLf &
            "    SELECT MSChungtu, KHChungtu, Sochungtu, KHMSCTLienquan, KHCTLienquan, SoCTLienquan, PhanbietCT" & vbCrLf &
            "    FROM ChungtuthueTNCN" & vbCrLf &
            "    WHERE MasothueTC = '" & maSoThue & "'" & vbCrLf &
            "      AND TrangthaicuoiCT = 1" & vbCrLf &
            "      AND PhanbietCT > 0" & vbCrLf &
            "      AND TinhchatCT > 0" & vbCrLf &
            ")," & vbCrLf &
            "cre_ctlienquan1 AS (" & vbCrLf &
            "    SELECT a.MaCT," & vbCrLf &
            "           a.PhanbietCT," & vbCrLf &
            "           CASE " & vbCrLf &
            "                WHEN a.PhanbietCT = 1 THEN N'Bị thay thế bởi CT: '+  b.KHChungtu +', '+ b.Sochungtu" & vbCrLf &
            "                ELSE N'Bị điều chỉnh bởi CT: '+ b.KHChungtu +', '+ b.Sochungtu" & vbCrLf &
            "           END AS GhichuCT" & vbCrLf &
            "    FROM dsall a" & vbCrLf &
            "    INNER JOIN cre_ctlienquan b" & vbCrLf &
            "            ON a.MSChungtu = b.KHMSCTLienquan" & vbCrLf &
            "           AND a.KHChungtu = b.KHCTLienquan" & vbCrLf &
            "           AND a.Sochungtu = b.SoCTLienquan" & vbCrLf &
            ")," & vbCrLf &
            "cre_ctlienquan2 AS (" & vbCrLf &
            "    SELECT MaCT," & vbCrLf &
            "           PhanbietCT," & vbCrLf &
            "           CASE " & vbCrLf &
            "                WHEN PhanbietCT = 1 AND TinhchatCT > 0 THEN N'Thay thế cho CT: '+ KHCTLienquan +', '+ SoCTLienquan" & vbCrLf &
            "                WHEN PhanbietCT = 2 AND TinhchatCT > 0 THEN N'Điều chỉnh cho CT: '+ KHCTLienquan +', '+ SoCTLienquan" & vbCrLf &
            "                ELSE ''" & vbCrLf &
            "           END AS GhichuCT" & vbCrLf &
            "    FROM dsall" & vbCrLf &
            ")," & vbCrLf &
            "cre_trangthaict AS (" & vbCrLf &
            "    SELECT * FROM cre_ctlienquan1" & vbCrLf &
            "    UNION" & vbCrLf &
            "    SELECT * FROM cre_ctlienquan2" & vbCrLf &
            ")," & vbCrLf &
            "cre_trangthaicttongquat AS (" & vbCrLf &
            "    SELECT MaCT," & vbCrLf &
            "           GhiChuGop = STUFF(" & vbCrLf &
            "                (SELECT ';' + GhichuCT" & vbCrLf &
            "                 FROM cre_trangthaict t2" & vbCrLf &
            "                 WHERE t2.MaCT = t1.MaCT" & vbCrLf &
            "                   AND GhichuCT IS NOT NULL" & vbCrLf &
            "                   AND LTRIM(RTRIM(GhichuCT)) <> ''" & vbCrLf &
            "                 ORDER BY PhanbietCT" & vbCrLf &
            "                 FOR XML PATH(''), TYPE" & vbCrLf &
            "                ).value('.', 'NVARCHAR(MAX)')," & vbCrLf &
            "                1, 1, ''" & vbCrLf &
            "           )" & vbCrLf &
            "    FROM cre_trangthaict t1" & vbCrLf &
            "    GROUP BY MaCT" & vbCrLf &
            ")" & vbCrLf &
            "SELECT a.*," & vbCrLf &
            "       CASE WHEN b.GhiChuGop IS NOT NULL THEN b.GhiChuGop ELSE N'Chứng từ mới' END AS GhichuCT" & vbCrLf &
            "FROM dsall a" & vbCrLf &
            "LEFT JOIN cre_trangthaicttongquat b" & vbCrLf &
            "       ON a.MaCT = b.MaCT" & vbCrLf &
            "ORDER BY a.MaCT DESC "

            ' Thêm phân trang nếu enablePaging = True
            If enablePaging Then
                sql &= vbCrLf & "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
            End If

            Return sql
        End If
    End Function



    'Xuất excel
    <WebMethod()>
    Public Function XuatDanhSachChungTuNhap(loaiTimKiem As Integer,
                                       mauso As String,
                                       kyhieu As String,
                                       tungay As String,
                                       denngay As String,
                                       sochungtu As String,
                                       matracuu As String,
                                       madonvi As String
                                       ) As String

        ' loaiTimKiem: 0 theo ngày, 1 theo số chứng từ, 2 theo mã tra cứu

        Dim response As New Dictionary(Of String, Object)()
        Dim sql As String = ""
        Dim dt As New DataTable("DSnhap_excel")

        Try
            '--- Xây dựng SQL
            If mauso = "03/TNCN" Then
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                        "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 "

                        ' chỉ thêm điều kiện ngày khi có giá trị
                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= "AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"

                    Case 1
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a  WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                    Case 2
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                End Select
            Else
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                        "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT=1 "

                        ' chỉ thêm điều kiện ngày khi có giá trị
                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= "AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"
                    Case 1
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT=1"
                    Case 2
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.TinhchatCT,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT=1"
                End Select
            End If


            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Lấy dữ liệu trang
                Using cmd As New SqlCommand(sql, connection)
                    cmd.Parameters.AddWithValue("@MaDV", madonvi)
                    cmd.Parameters.AddWithValue("@MS", mauso)
                    cmd.Parameters.AddWithValue("@KH", kyhieu)
                    If sql.Contains("@TuNgay") Then cmd.Parameters.AddWithValue("@TuNgay", tungay)
                    If sql.Contains("@DenNgay") Then cmd.Parameters.AddWithValue("@DenNgay", denngay)
                    If sql.Contains("@SoCT") Then cmd.Parameters.AddWithValue("@SoCT", sochungtu)
                    If sql.Contains("@matracuu") Then cmd.Parameters.AddWithValue("@matracuu", matracuu)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using


            response("status") = "success"
            response("data") = dt
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function XuatDanhSachChungTuDaKy(loaiTimKiem As Integer,
                                       mauso As String,
                                       kyhieu As String,
                                       tungay As String,
                                       denngay As String,
                                       sochungtu As String,
                                       matracuu As String,
                                       madonvi As String) As String

        Dim response As New Dictionary(Of String, Object)()
        Dim sql As String = ""
        Dim dt As New DataTable("DSdaky_excel")


        Try
            '--- Xây dựng SQL
            If mauso = "03/TNCN" Then
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                            "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT IN (2,6) "

                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= " AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"

                    Case 1
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT IN (2,6) AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                    Case 2
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT IN (2,6) AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH"
                End Select
            Else
                Select Case loaiTimKiem
                    Case 0
                        sql = "SELECT a.MaCT,a.KHChungtu,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan " &
                       "FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.TinhtrangCT>1"

                        If Not String.IsNullOrEmpty(tungay) AndAlso Not String.IsNullOrEmpty(denngay) Then
                            sql &= " AND (CONVERT(date,a.NgaylapCT) BETWEEN @TuNgay AND @DenNgay) "
                        End If

                        If Not String.IsNullOrEmpty(kyhieu) Then
                            sql &= "AND a.KHChungtu=@KH "
                        End If

                        sql &= "AND a.MSChungtu=@MS"

                    Case 1
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Sochungtu=@SoCT AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT>1"
                    Case 2
                        sql = "SELECT a.MaCT,a.NgaylapCT,a.Sochungtu,a.MasothueNNT,a.TenNNT,a.SoCMND,a.ThueTNCN,a.Matracuu,a.TinhtrangCT,a.PhanbietCT,a.KHMSCTLienquan,a.KHCTLienquan,a.SoCTLienquan,'' AS TrangthaiguiCQT FROM ChungtuthueTNCN a WHERE a.MasothueTC=@MaDV AND a.Matracuu=@matracuu AND a.MSChungtu=@MS AND a.KHChungtu=@KH AND a.TinhtrangCT>1"
                End Select
            End If


            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Lấy dữ liệu trang
                Using cmd As New SqlCommand(sql, connection)
                    cmd.Parameters.AddWithValue("@MaDV", madonvi)
                    cmd.Parameters.AddWithValue("@MS", mauso)
                    cmd.Parameters.AddWithValue("@KH", kyhieu)
                    If sql.Contains("@TuNgay") Then cmd.Parameters.AddWithValue("@TuNgay", tungay)
                    If sql.Contains("@DenNgay") Then cmd.Parameters.AddWithValue("@DenNgay", denngay)
                    If sql.Contains("@SoCT") Then cmd.Parameters.AddWithValue("@SoCT", sochungtu)
                    If sql.Contains("@matracuu") Then cmd.Parameters.AddWithValue("@matracuu", matracuu)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using



            response("status") = "success"
            response("data") = dt

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function XuatDanhSachChungTuDaGuiCQT(loaiTimKiem As Integer,
                                    mauso As String,
                                    kyhieu As String,
                                    tungay As String,
                                    denngay As String,
                                    sochungtu As String,
                                    matracuu As String,
                                    madonvi As String) As String

        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim sql As String = ""
            Dim chuoiketnoi As String = connectionString


            ' Xác định câu SQL
            If mauso = "03/TNCN" Then
                Select Case loaiTimKiem
                    Case 0
                        sql = GetSqlQuery(1, madonvi, tungay, denngay, mauso, kyhieu, "", "", False, False)
                        CapnhatKhoaphienchungtuchuacapnhatKQ(madonvi, kyhieu)
                    Case 1
                        sql = GetSqlQuery(2, madonvi, "", "", mauso, kyhieu, sochungtu, "", False, False)
                        CapnhatKhoaphienchungtuchuacapnhatKQ(madonvi, kyhieu)
                    Case 2
                        sql = GetSqlQuery(3, madonvi, "", "", mauso, kyhieu, "", matracuu, False, False)
                        CapnhatKhoaphienchungtuchuacapnhatKQ(madonvi, kyhieu)
                    Case Else
                        sql = String.Empty
                End Select
            End If

            Dim totalRecords As Integer = 0
            Dim totalPages As Integer = 0
            Dim dt As New DataTable("DSHD")

            Using connection As New SqlConnection(chuoiketnoi)
                connection.Open()



                ' Lấy dữ liệu phân trang
                Using cmd As New SqlCommand(sql, connection)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            response("status") = "success"
            response("data") = dt
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    Private Function LayXmlChungTuDaKy(madonvi As String, machungtu As Integer) As String
        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("SELECT XMLChungtu FROM ChungtuthueTNCN WHERE MasothueTC = @madonvi AND MaCT = @machungtu and TinhtrangCT=2 and TrangthaicuoiCT=1", conn)
            cmd.Parameters.AddWithValue("@madonvi", madonvi)
            cmd.Parameters.AddWithValue("@machungtu", machungtu)

            Try
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()

                ' ✅ Kiểm tra kết quả hợp lệ
                If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                    Return result.ToString()
                Else
                    Return String.Empty
                End If

            Catch ex As Exception
                ' ✅ Ghi log nếu cần
                ' LogError("LayXmlChungTuDaKy", ex)
                Return String.Empty
            End Try
        End Using
    End Function
    <WebMethod>
    Public Function LayXmlChungTu(madonvi As String, machungtu As Integer) As String
        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("SELECT XMLChungtu FROM ChungtuthueTNCN WHERE MasothueTC = @madonvi AND MaCT = @machungtu", conn)
            cmd.Parameters.AddWithValue("@madonvi", madonvi)
            cmd.Parameters.AddWithValue("@machungtu", machungtu)

            Try
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()

                ' ✅ Kiểm tra kết quả hợp lệ
                If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                    Return result.ToString()
                Else
                    Return String.Empty
                End If

            Catch ex As Exception
                ' ✅ Ghi log nếu cần
                ' LogError("LayXmlChungTuDaKy", ex)
                Return String.Empty
            End Try
        End Using
    End Function


    <WebMethod()>
    Public Function GuiChungTuCQT(machungtu As String, madonvi As String) As String
        Dim message As String = String.Empty
        Dim response As New Dictionary(Of String, Object)()

        Dim thongdiepdaky As String = LayXmlChungTuDaKy(madonvi, machungtu)

        Try
            If Not String.IsNullOrEmpty(thongdiepdaky) Then
                Dim signText As String = thongdiepdaky
                Dim checkb64 As Boolean = IsBase64String(signText)
                If checkb64 = True Then

                    Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                    Dim key As String = "0103930279" & guidstr.Replace("-", "")
                    Dim xmlthongdiep As String = Taothongdiep211("0103930279", madonvi, machungtu)


                    Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(xmlthongdiep))

                    ''Gui thong diep 
                    Dim servicetvan As ServiceTVAN.WSInterTRCA2 = New ServiceTVAN.WSInterTRCA2()
                    Dim ttketnoi As New ServiceTVAN.AuthHeader
                    ttketnoi.Username = "ntvan"
                    ttketnoi.Password = "123456"
                    servicetvan.AuthHeaderValue = ttketnoi
                    servicetvan.Timeout = 2147483647
                    Dim macode As String = servicetvan.Guithongdiep(xmlthongdiep, 0)

                    If macode.Length > 10 Then
                        Dim kqphathanh = CheckTrangthaiGuithue_chungtu(machungtu)
                        If kqphathanh <> 2 Then
                            UpdateKhoaphienCTSauGui(33, madonvi, machungtu, macode)
                        End If

                        ''lay ket qua phan hoi tu CQT
                        Dim phanhoi As String = String.Empty
                        While phanhoi = ""
                            Thread.Sleep(10000)
                            phanhoi = servicetvan.LayKQThongdiep(macode, "0103930279")
                        End While

                        LayKQTruyennhanchungtudientu(madonvi, macode)

                        If phanhoi = "-1" Then
                            message = "0|Xác thực không đúng"
                        ElseIf phanhoi = "-5" Then
                            message = "0|Mã số thuế trong thông điệp không khớp với tài khoản"
                        ElseIf phanhoi = "-6" Then
                            message = "0|Không có thông điệp nào thỏa mãn"
                        ElseIf phanhoi = "-7" Then
                            message = "1|Chưa có kết quả phản hồi của cơ quan thuế"
                        Else
                            Dim base64phanhoi = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(phanhoi))
                            If phanhoi.Contains("<LTBao>2</LTBao>") Then
                                message = "1|CQT đã chấp nhận chứng từ"
                                Try
                                    GuiMailCT(machungtu, madonvi)
                                Catch ex As Exception

                                End Try


                            Else
                                message = "1|CQT không chấp nhận, vui lòng quay lại danh sách để xem chi tiết."
                            End If
                        End If

                    Else
                        message = "0|Không gửi được chứng từ lên CQT"
                    End If
                Else
                    message = "0|Sai định dạng thông điệp"
                End If

            End If

            Dim parts() As String = message.Split("|"c)

            If parts.Length > 1 Then
                If parts(0) = "1" Then
                    response("status") = "success"
                Else
                    response("status") = "error"
                End If
                response("message") = parts(1)
            Else
                response("status") = "error"
                response("message") = "Lỗi không xác định"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function XoaNhapChungTu(mact As String, madv As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            If String.IsNullOrEmpty(mact) Then
                response("status") = "error"
                response("message") = "MaCT không hợp lệ."
                Return JsonConvert.SerializeObject(response)
            End If

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using cmd As New SqlCommand("UPDATE ChungtuthueTNCN SET Trangthai=0, TinhtrangCT=0, TrangthaicuoiCT=0 WHERE MasothueTC=@MasothueTC AND MaCT=@MaCT", conn)
                    cmd.Parameters.Add("@MasothueTC", SqlDbType.NVarChar, 20).Value = madv
                    cmd.Parameters.Add("@MaCT", SqlDbType.Int).Value = Convert.ToInt32(mact)

                    Dim rows As Integer = cmd.ExecuteNonQuery()

                    If rows > 0 Then
                        ' Ghi log
                        response("status") = "success"
                        response("message") = "Xóa nháp chứng từ thành công."
                    Else
                        response("status") = "error"
                        response("message") = "Không tìm thấy chứng từ cần xóa."
                    End If
                End Using
            End Using

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function HuyChungTu(machungtu As String, kyhieuchungtu As String, madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            If String.IsNullOrEmpty(machungtu) Then
                response("status") = "error"
                response("message") = "MaCT không hợp lệ."
                Return JsonConvert.SerializeObject(response)
            End If

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using cmd As New SqlCommand("UPDATE ChungtuthueTNCN SET TinhtrangCT=6 WHERE MaCT=@MaCT And KHChungtu=@KHChungtu and MasothueTC=@madonvi", conn)
                    cmd.Parameters.Add("@MaCT", SqlDbType.Int).Value = Convert.ToInt32(machungtu)
                    cmd.Parameters.Add("@KHChungtu", SqlDbType.NVarChar, 10).Value = kyhieuchungtu
                    cmd.Parameters.Add("@madonvi", SqlDbType.NVarChar, 20).Value = madonvi


                    Dim rows As Integer = cmd.ExecuteNonQuery()
                    If rows > 0 Then
                        ' Ghi log
                        response("status") = "success"
                        response("message") = "Hủy chứng từ thành công."
                    Else
                        response("status") = "error"
                        response("message") = "Không tìm thấy chứng từ cần hủy."
                    End If
                End Using
            End Using

            SqlConnection.ClearAllPools()

        Catch ex As Exception
            response("status") = "error"
            If ex.InnerException IsNot Nothing Then
                response("message") = "Lỗi: " & ex.InnerException.Message
            Else
                response("message") = "Lỗi: " & ex.Message
            End If
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    <WebMethod()>
    Public Function LayDanhSachKyHieu(mauso As String, madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Dim dt As New DataTable()

        Try
            Dim sql As String = "SELECT ky_hieu FROM hoa_don_dang_ky_phat_hanh WHERE mau_so=@mauso AND donvi_ma_dv=@madonvi ORDER BY id DESC"

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.Add("@mauso", SqlDbType.NVarChar, 20).Value = mauso
                    cmd.Parameters.Add("@madonvi", SqlDbType.NVarChar, 50).Value = madonvi

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            response("status") = "success"
            response("data") = dt

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    <WebMethod()>
    Public Function LayMauSoChungTu(maDonVi As String, mauso As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim listMauSo As New List(Of String)()
            Dim sql As String = "SELECT DISTINCT mau_so FROM hoa_don_dang_ky_phat_hanh WHERE mau_so=@MauSo AND donvi_ma_dv=@MaDV AND loai_hoa_don_ct_id=14 AND is_deleted=0 ORDER BY mau_so"

            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.Add("@MauSo", SqlDbType.NVarChar, 10).Value = mauso
                    cmd.Parameters.Add("@MaDV", SqlDbType.NVarChar, 20).Value = maDonVi
                    conn.Open()
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            If Not rdr.IsDBNull(0) Then
                                listMauSo.Add(rdr.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using

            response("status") = "success"
            response("data") = listMauSo
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function LayLoaiChungTu(maDonVi As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim listLoaiHD As New List(Of Dictionary(Of String, Object))()
            Dim sql As String = "SELECT DISTINCT mau_so AS MSChungtu, ten_hoa_don AS Tenchungtu FROM hoa_don_dang_ky_phat_hanh WHERE donvi_ma_dv=@MaDV AND loai_hoa_don_ct_id=14 AND is_deleted=0"

            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.Add("@MaDV", SqlDbType.NVarChar, 50).Value = maDonVi
                    conn.Open()
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            Dim item As New Dictionary(Of String, Object)()
                            item("MSChungtu") = rdr("MSChungtu").ToString()
                            item("Tenchungtu") = rdr("Tenchungtu").ToString()
                            listLoaiHD.Add(item)
                        End While
                    End Using
                End Using
            End Using

            response("status") = "success"
            response("data") = listLoaiHD
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    <WebMethod()>
    Public Function DayLoChungTu(fileName As String, fileBase64 As String, madonvi As String, mauso As String, kyhieu As String, ngaylap As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Dim count As Integer = 0

        Try
            ' 📂 1. Lưu file xuống thư mục tạm
            Dim uploadDir As String = HttpContext.Current.Server.MapPath("~/temp/")
            If Not Directory.Exists(uploadDir) Then
                Directory.CreateDirectory(uploadDir)
            End If

            Dim filePath As String = Path.Combine(uploadDir, Guid.NewGuid().ToString() & "_" & fileName)
            Dim fileBytes As Byte() = Convert.FromBase64String(fileBase64)
            File.WriteAllBytes(filePath, fileBytes)

            ' 📖 2. Đọc Excel bằng OleDb
            Dim excelConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filePath & ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'"
            Dim dt1 As New DataTable()
            Using connection As New OleDbConnection(excelConnectionString)
                connection.Open()
                Using cmd As New OleDbCommand("SELECT * FROM [Sheet1$]", connection)
                    Using adapter As New OleDbDataAdapter(cmd)
                        adapter.Fill(dt1)
                    End Using
                End Using
            End Using

            dt1 = RemoveEmptyRows(dt1) ' 👉 bạn đã có hàm này

            ' 📌 3. Validate dữ liệu
            Dim validatedDataList As New List(Of ValidatedRowData)()

            For j As Integer = 0 To dt1.Rows.Count - 1
                Dim rowData As New ValidatedRowData()

                ' --- Đọc tất cả cột từ Excel ---
                If dt1.Rows(j)("Ten_NNT") IsNot DBNull.Value Then rowData.tennnt = dt1.Rows(j)("Ten_NNT").ToString().Trim()
                If dt1.Rows(j)("MST_NNT") IsNot DBNull.Value Then rowData.mstnguoinnt = dt1.Rows(j)("MST_NNT").ToString().Trim()
                If dt1.Rows(j)("Diachi_NNT") IsNot DBNull.Value Then rowData.diachinnt = dt1.Rows(j)("Diachi_NNT").ToString().Trim()
                If dt1.Rows(j)("Dienthoai_NNT") IsNot DBNull.Value Then rowData.dienthoainnt = dt1.Rows(j)("Dienthoai_NNT").ToString().Trim()
                If dt1.Rows(j)("Email_NNT") IsNot DBNull.Value Then rowData.emailnnt = dt1.Rows(j)("Email_NNT").ToString().Trim()
                If dt1.Rows(j)("CMND_CCCD") IsNot DBNull.Value Then rowData.cmndnnt = dt1.Rows(j)("CMND_CCCD").ToString().Trim()
                If dt1.Rows(j)("CNcutru") IsNot DBNull.Value Then rowData.canhancutru = dt1.Rows(j)("CNcutru")
                If dt1.Rows(j)("TNTuthang") IsNot DBNull.Value Then rowData.thunhaptuthang = dt1.Rows(j)("TNTuthang").ToString().Trim()
                If dt1.Rows(j)("TNDenthang") IsNot DBNull.Value Then rowData.thunhapdenthang = dt1.Rows(j)("TNDenthang")
                If dt1.Rows(j)("NamTN") IsNot DBNull.Value Then rowData.nam = dt1.Rows(j)("NamTN").ToString().Trim()
                If dt1.Rows(j)("Quoctich") IsNot DBNull.Value Then rowData.quoctich = dt1.Rows(j)("Quoctich").ToString().Trim()
                If dt1.Rows(j)("Khoanthunhap") IsNot DBNull.Value Then rowData.khoanthunhap = dt1.Rows(j)("Khoanthunhap").ToString().Trim()
                If dt1.Rows(j)("TongthunhapchiuVAT") IsNot DBNull.Value Then rowData.tongthunhapchiuthue = dt1.Rows(j)("TongthunhapchiuVAT").ToString().Trim()
                If dt1.Rows(j)("TongthunhaptinhVAT") IsNot DBNull.Value Then rowData.tongthunhaptinhthue = dt1.Rows(j)("TongthunhaptinhVAT").ToString().Trim()
                If dt1.Rows(j)("SoThue") IsNot DBNull.Value Then rowData.thuetncn = dt1.Rows(j)("SoThue").ToString().Trim()
                If dt1.Rows(j)("Baohiem") IsNot DBNull.Value Then rowData.baohiem = dt1.Rows(j)("Baohiem")

                ' Riêng mẫu số 03/TNCN có thêm trường TThien
                If mauso = "03/TNCN" Then
                    If dt1.Rows(j)("TThien") IsNot DBNull.Value Then rowData.tthien = dt1.Rows(j)("TThien")
                Else
                    If dt1.Rows(j)("SoTNduocnhan") IsNot DBNull.Value Then rowData.sothunhapdn = dt1.Rows(j)("SoTNduocnhan")
                    If dt1.Columns.Contains("Ngaycap") AndAlso dt1.Rows(j)("Ngaycap") IsNot DBNull.Value Then rowData.ngaycap = dt1.Rows(j)("Ngaycap").ToString().Trim()
                    If dt1.Columns.Contains("Noicap") AndAlso dt1.Rows(j)("Noicap") IsNot DBNull.Value Then rowData.noicap = dt1.Rows(j)("Noicap").ToString().Trim()
                End If

                ' Validate dòng
                Dim errorMessage As String = ValidateRowData(rowData, j + 1, mauso)
                If Not String.IsNullOrEmpty(errorMessage) Then
                    response("status") = "error"
                    response("message") = errorMessage
                    Return New JavaScriptSerializer().Serialize(response)
                End If

                validatedDataList.Add(rowData)
            Next

            ' 📌 4. Gọi service tạo chứng từ
            Dim madv As String = madonvi
            Dim ttdvi As thongtindv = GetTTDonvi(madv)
            Dim tenct As String = "CHỨNG TỪ KHẤU TRỪ THUẾ THU NHẬP CÁ NHÂN"

            For Each rowData In validatedDataList
                Dim resct As Integer

                If mauso = "03/TNCN" Then
                    resct = TaoCT_khongso70(tenct, mauso, kyhieu, ngaylap, 0, "", "", "", "", "", "", ttdvi.tendv, ttdvi.madv, ttdvi.diachi, ttdvi.dienthoai,
                                                 rowData.tennnt, rowData.mstnguoinnt, rowData.diachinnt, rowData.quoctich, rowData.canhancutru, rowData.cmndnnt,
                                                 rowData.dienthoainnt, rowData.emailnnt, rowData.khoanthunhap, rowData.thunhaptuthang, rowData.nam,
                                                 rowData.tongthunhapchiuthue, rowData.tongthunhaptinhthue, rowData.thuetncn, rowData.thunhapdenthang,
                                                 rowData.baohiem, rowData.tthien)


                End If
                count += 1
            Next

            ' 📌 5. Trả về JSON
            response("status") = "success"
            response("count") = count
            Return New JavaScriptSerializer().Serialize(response)

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
            Return New JavaScriptSerializer().Serialize(response)
        End Try
    End Function



    ' Hàm validate từng dòng dữ liệu
    Private Function ValidateRowData(rowData As ValidatedRowData, rowIndex As Integer, mauso As String) As String
        ' Kiểm tra tên người nộp thuế
        If String.IsNullOrEmpty(rowData.tennnt) Then
            Return "ERROR: Tên người nộp thuế không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        ' Kiểm tra địa chỉ
        If String.IsNullOrEmpty(rowData.diachinnt) Then
            Return "ERROR: Địa chỉ người nộp thuế không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        ' Kiểm tra số điện thoại
        If String.IsNullOrEmpty(rowData.dienthoainnt) Then
            Return "ERROR: Số điện thoại người nộp thuế không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        If Not IsNumeric(rowData.dienthoainnt) OrElse rowData.dienthoainnt.Length < 10 OrElse rowData.dienthoainnt.Length > 11 Then
            Return "ERROR: Số điện thoại người nộp thuế phải là số và có từ 10 đến 11 chữ số tại dòng " & (rowIndex + 1).ToString()
        End If

        ' Kiểm tra MST hoặc CCCD
        If String.IsNullOrEmpty(rowData.mstnguoinnt) AndAlso String.IsNullOrEmpty(rowData.cmndnnt) Then
            Return "ERROR: Phải có MST hoặc CMND/CCCD tại dòng " & (rowIndex + 1).ToString()
        End If

        ' Kiểm tra định dạng CCCD nếu có
        If Not String.IsNullOrEmpty(rowData.cmndnnt) Then
            Dim value = rowData.cmndnnt.Trim()
            If value.Length < 9 OrElse value.Length > 12 Then
                Return "ERROR: CMND/CCCD - Số hộ chiếu phải có từ 9 đến 12 ký tự tại dòng " &
               (rowIndex + 1).ToString()
            End If
        End If

        ' Các kiểm tra khác...
        If String.IsNullOrEmpty(rowData.canhancutru.ToString()) Then
            Return "ERROR: Cá nhân cư trú không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        If rowData.canhancutru.ToString() <> "0" AndAlso rowData.canhancutru.ToString() <> "1" Then
            Return "ERROR: Giá trị cá nhân cư trú phải là 0 hoặc 1 tại dòng " & (rowIndex + 1).ToString()
        End If

        If String.IsNullOrEmpty(rowData.thunhaptuthang) Then
            Return "ERROR: Thu nhập từ tháng không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        If String.IsNullOrEmpty(rowData.thunhapdenthang.ToString()) Then
            Return "ERROR: Thu nhập đến tháng không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        If String.IsNullOrEmpty(rowData.nam) Then
            Return "ERROR: Năm thu nhập không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        If String.IsNullOrEmpty(rowData.khoanthunhap) Then
            Return "ERROR: Khoản thu nhập không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        Dim tongthunhapchiuthueErrMsg As String = ValidateDecimal216(rowData.tongthunhapchiuthue, "Tổng thu nhập chịu thuế", rowIndex)
        If tongthunhapchiuthueErrMsg <> "" Then Return tongthunhapchiuthueErrMsg

        Dim tongthunhaptinhthueErrMsg As String = ValidateDecimal216(rowData.tongthunhaptinhthue, "Tổng thu nhập tính thuế", rowIndex)
        If tongthunhaptinhthueErrMsg <> "" Then Return tongthunhaptinhthueErrMsg

        Dim thuetncnErrMsg As String = ValidateDecimal216(rowData.thuetncn, "Số thuế", rowIndex)
        If thuetncnErrMsg <> "" Then Return thuetncnErrMsg

        Dim baohiemErrMsg As String = ValidateDecimal216(rowData.baohiem, "Bảo hiểm", rowIndex)
        If baohiemErrMsg <> "" Then Return baohiemErrMsg

        ' Kiểm tra các trường đặc biệt theo mẫu số
        If mauso = "03/TNCN" Then
            Dim tthienErrMsg As String = ValidateDecimal216(rowData.tthien, "Bảo hiểm", rowIndex)
            If tthienErrMsg <> "" Then Return tthienErrMsg
        End If

        Return String.Empty ' Không có lỗi
    End Function

    Private Function ValidateDecimal216(value As Object, fieldName As String, rowIndex As Integer) As String
        If String.IsNullOrEmpty(value.ToString()) Then
            Return "ERROR: " & fieldName & " không được để trống tại dòng " & (rowIndex + 1).ToString()
        End If

        Dim val As Decimal
        If Not Decimal.TryParse(value.ToString(), val) Then
            Return "ERROR: " & fieldName & " không đúng định dạng số tại dòng " & (rowIndex + 1).ToString()
        End If

        Dim parts() As String = value.ToString().Split("."c)
        Dim totalDigits As Integer = parts(0).Length + If(parts.Length > 1, parts(1).Length, 0)
        Dim decimalDigits As Integer = If(parts.Length > 1, parts(1).Length, 0)

        If totalDigits > 21 OrElse decimalDigits > 6 Then
            Return "ERROR: " & fieldName & " phải có định dạng tối đa 21 chữ số, trong đó không quá 6 chữ số thập phân tại dòng " & (rowIndex + 1).ToString()
        End If

        Return "" ' Không có lỗi
    End Function

    Private Sub CleanupResources(oleda As OleDbDataAdapter, connection As OleDbConnection, command As OleDbCommand)
        Try
            If oleda IsNot Nothing Then oleda.Dispose()
            If connection IsNot Nothing Then
                If connection.State = ConnectionState.Open Then connection.Close()
                connection.Dispose()
            End If
            If command IsNot Nothing Then command.Dispose()
        Catch ex As Exception
            ' Log error if needed
        End Try
    End Sub

    ' Hàm loại bỏ các dòng trống
    Private Function RemoveEmptyRows(dt As DataTable) As DataTable
        Dim newDt As DataTable = dt.Clone() ' Tạo bảng mới với cùng cấu trúc

        For Each row As DataRow In dt.Rows
            ' Kiểm tra xem dòng có dữ liệu thực sự không
            If Not IsEmptyRow(row) Then
                newDt.ImportRow(row)
            End If
        Next

        Return newDt
    End Function

    ' Hàm kiểm tra dòng trống (kiểm tra tất cả các cột)
    Private Function IsEmptyRow(row As DataRow) As Boolean
        For Each item As Object In row.ItemArray
            If item IsNot DBNull.Value AndAlso Not String.IsNullOrWhiteSpace(item.ToString()) Then
                Return False ' Có ít nhất một cột có dữ liệu
            End If
        Next
        Return True ' Tất cả các cột đều trống
    End Function



    <WebMethod()>
    Public Function XemChungTu(machungtu As String, madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim trangthaihd As String = String.Empty
            Dim mschungtu As String = String.Empty
            Dim khchungtu As String = String.Empty
            Dim sochungtu As String = String.Empty
            Dim base64xml As String = String.Empty
            Dim Ghichu As String = String.Empty
            '====Load thong tin hoa don====
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "Select MasothueTC, TinhtrangCT, MSChungtu,KHChungtu,Sochungtu, XMLChungtu from ChungtuthueTNCN where MaCT = '" & machungtu & "'"
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    trangthaihd = reader("TinhtrangCT")
                    mschungtu = reader("MSChungtu")
                    khchungtu = reader("KHChungtu")
                    sochungtu = reader("Sochungtu")
                    base64xml = reader("XMLChungtu")
                End While
            End If
            reader.Close()
            conn.Close()
            comm.Dispose()
            conn.Dispose()
            SqlConnection.ClearAllPools()

            Dim content = loadcontent(madonvi, base64xml, trangthaihd, mschungtu, machungtu, khchungtu, sochungtu)

            response("status") = "success"
            response("data") = content
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)

    End Function

    Private Function loadcontent(mst As String, base64xml As String, trangthai As String, mschungtu As String, mact As String, khchungtu As String, sochungtu As String) As String
        Dim res As String = String.Empty

        If Not String.IsNullOrEmpty(base64xml) Then
            Dim xmlfile As String = Encoding.UTF8.GetString(Convert.FromBase64String(base64xml))
            Dim result = Encoding.UTF8.GetString(Convert.FromBase64String(base64xml))


            Dim xml As Byte() = Convert.FromBase64String(base64xml)
            Dim xsltPath As String = String.Empty
            Dim style As String = "width:900px;margin:auto; border:2px solid black; padding-top:20px;"
            Dim tenhd As String = "Chứng từ khấu trừ thuế thu nhập cá nhân"
            Dim phithuequan As String = String.Empty

            Dim oldfile As String = String.Empty
            oldfile = GetXSLT(mschungtu, mst, tenhd)

            Dim destfile As String = Server.MapPath("~/temp/" & Path.GetRandomFileName & ".xslt")
            Dim logopath As String = GetLogoPathChungTu(mst, mschungtu)
            Dim templogofilename As String = Now.ToString("ddMMyyyyHHmmss") & "_templogo.jpg"
            Dim b64logo As String = String.Empty
            If Not String.IsNullOrEmpty(logopath) Then
                Dim fn As String = Server.MapPath(logopath)
                If File.Exists(fn) Then
                    File.Copy(fn, Server.MapPath("~/temp/" & templogofilename), True)
                    b64logo = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("~/temp/" & templogofilename)))
                    b64logo = "data:image/jpg;base64," & b64logo

                Else
                    b64logo = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("xml/blank.png")))
                    b64logo = "data:image/jpg;base64," & b64logo

                End If
            Else
                b64logo = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("xml/blank.png")))
                b64logo = "data:image/jpg;base64," & b64logo
            End If

            Dim bgInfo As BGInfo = GetBGInfo(mst, mschungtu)
            Dim bgtype As Integer = bgInfo.BGType
            Dim bgpath As String = bgInfo.Nen
            Dim checkxsltborder As String = Left(Path.GetFileNameWithoutExtension(oldfile), 1)

            Dim bgstyle As String
            Dim table_style As String

            If checkxsltborder = "B" Then
                If bgtype = 0 Then
                    bgstyle = "width:850px;margin:auto; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & bgpath & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = String.Empty
                Else
                    bgstyle = "width:850px;margin:auto; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & "" & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = "background-image: url('" & bgpath & "'); background-size:cover; background-position: center;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                End If
            Else
                If bgtype = 0 Then
                    bgstyle = "width:850px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & bgpath & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = String.Empty
                Else
                    bgstyle = "width:850px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & "" & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = "background-image: url('" & bgpath & "'); background-size:cover; background-position: center;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                End If
            End If

            Dim chukykh As Boolean = False

            Dim sohdbs As String = String.Empty

            Dim paramsubtitle, paramsubtitlecontent, paramSubtitleDiv, paramSubtitleContentDiv As String
            paramsubtitle = "none"
            paramSubtitleDiv = "normal"
            paramsubtitlecontent = String.Empty
            paramSubtitleContentDiv = "&#160;"

            Dim thongtinchungtu As ChungTuInfo = CheckCTGocBiThayTheDC(mst, mact, mschungtu, khchungtu, sochungtu)

            Dim styledisabled, noidungdisabled As String
            noidungdisabled = String.Empty
            styledisabled = "position:absolute;z-index:0;width:300px;height:100px;border:3px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;"
            noidungdisabled = "&#160;"

            If thongtinchungtu.SLCTu > 1 Then
                Select Case thongtinchungtu.PhanBietCT
                    Case 1
                        noidungdisabled = "CHỨNG TỪ BỊ THAY THẾ"
                        styledisabled = "position:absolute;z-index:0;width:auto !important;height:60px !important;border:5px solid red;background:transparent;display:block;top:60%;left:45%;color:red;font-size:20pt !important;text-align:center;padding-top:10px !important;font-weight:bold"
                    Case 2
                        noidungdisabled = "CHỨNG TỪ BỊ ĐIỀU CHỈNH"
                        styledisabled = "position:absolute;z-index:0;width:auto !important;height:60px !important;border:5px solid red;background:transparent;display:block;top:60%;left:45%;color:red;font-size:20pt !important;text-align:center;padding-top:10px !important;font-weight:bold"
                End Select
            End If

            Dim fileReader As String = String.Empty
            Dim checkgiamthue As Integer = 0
            Dim readerfile As String = String.Empty
            Dim doc As XmlDocument = New XmlDocument
            doc.LoadXml(result)
            Dim xdocumentfile = ConvertToXDocument(doc)
            checkgiamthue = GetElement(xdocumentfile, "TTKhac")

            If trangthai = "0" Then
                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", "display:none").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramlien", "0").Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramdisplay", "display:none")
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
            ElseIf trangthai = "1" Then
                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", "display:none").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramlien", "0").Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramdisplay", "display:none")
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
            ElseIf trangthai = "2" Then
                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", "display:normal").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramlien", "0").Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramdisplay", "display:none")
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
            ElseIf trangthai = "3" Then
                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:normal").Replace("paramSign", "display:normal").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:normal").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramlien", "0").Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramdisplay", "display:none")
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
            ElseIf trangthai = "5" Or trangthai = "6" Then
                styledisabled = "position:absolute;z-index:0;width:auto;height:70px;border:5px solid red;background:transparent;display:block;top:45%;left:50%;color:red;font-size:25pt;text-align:center;padding-top:10px;font-weight:bold"

                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", "display:normal").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "normal").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", "HỦY").Replace("paramlien", "0").Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramdisplay", "display:none")
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
            Else
                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", "display:normal").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramlien", "0").Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramdisplay", "display:none")
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
            End If
            xsltPath = destfile
            Dim content = GetHtmlXemChungTu(xsltPath, xml)
            content = content.Replace("&amp;", "&")

            res = content
            'File.Delete(destfile)
        End If
        Return res
    End Function

    Private Function GetElement(ByVal doc As XDocument, ByVal elementName As String) As Integer
        For Each node As XNode In doc.DescendantNodes()
            If TypeOf node Is XElement Then
                Dim element As XElement = CType(node, XElement)
                If element.Name.LocalName.Equals(elementName) Then
                    Return 1
                End If
            End If
        Next
        Return 0
    End Function

    Public Function ConvertToXDocument(ByVal input As XmlDocument) As XDocument
        Using reader = New XmlNodeReader(input)
            reader.MoveToContent()
            Return XDocument.Load(reader)
        End Using
    End Function


    Public Function GetHtmlXemChungTu(xsltPath As String, xml As Byte()) As String

        Dim stream As New MemoryStream(xml)
        Dim document As New XPathDocument(stream)
        Dim writer As New StringWriter()
        Dim argList As XsltArgumentList = New XsltArgumentList()


        Dim transform As New XslCompiledTransform()
        transform.Load(xsltPath, New XsltSettings(True, True), New XmlUrlResolver())
        transform.Transform(document, argList, writer)
        Return writer.ToString()
    End Function



    Public Class BGInfo
        Public Property BGType As Integer
        Public Property Nen As String
    End Class


    Private Function GetBGInfo(madv As String, mauso As String) As BGInfo
        Dim result As New BGInfo With {
        .BGType = 0,
        .Nen = String.Empty
    }

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using comm As New SqlCommand("SELECT BGType, Nen FROM mauhoadon WHERE MaDV = @MaDV AND Mauso = @Mauso", conn)

                comm.Parameters.AddWithValue("@MaDV", madv)
                comm.Parameters.AddWithValue("@Mauso", mauso)

                Using reader As SqlDataReader = comm.ExecuteReader()
                    If reader.Read() Then
                        If Not IsDBNull(reader("BGType")) Then
                            result.BGType = Convert.ToInt32(reader("BGType"))
                        End If
                        If Not IsDBNull(reader("Nen")) Then
                            result.Nen = reader("Nen").ToString()
                        End If
                    End If
                End Using
            End Using
        End Using

        SqlConnection.ClearAllPools()
        Return result
    End Function

    Private Function GetXSLT(mauso As String, madv As String, tenhd As String) As String
        Dim res As String = String.Empty
        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                comm.Connection = conn
                conn.Open()

                If madv = "5700500039" Or madv = "0105581922" Or madv = "0201742890" Or madv = "0104832394" Then
                    comm.CommandText = "SELECT Filexslt FROM mauhoadon WHERE Mauso = @Mauso AND MaDV = @MaDV"
                ElseIf mauso = "03/TNCN" Then
                    comm.CommandText = "SELECT TOP 1 Filexslt FROM mauhoadon WHERE Mauso = @Mauso AND MaDV = @MaDV AND idLoaiHD_CT=14 ORDER BY idMauHD DESC"
                Else
                    comm.CommandText = "SELECT Filexslt FROM mauhoadon WHERE Mauso = 'CTT56' AND MaDV = @MaDV AND TenHD = N'Chứng từ khấu trừ thuế thu nhập cá nhân'"
                End If

                comm.Parameters.AddWithValue("@Mauso", mauso)
                comm.Parameters.AddWithValue("@MaDV", madv)

                Using reader As SqlDataReader = comm.ExecuteReader()
                    If reader.Read() Then
                        res = reader(0).ToString()
                    End If
                End Using
            End Using
        End Using

        SqlConnection.ClearAllPools()
        Return res
    End Function


    Public Class ChungTuInfo
        Public Property PhanBietCT As Integer
        Public Property TinhChatCT As Integer
        Public Property SLCTu As Integer

    End Class

    Private Function CheckCTGocBiThayTheDC(madv As String, MaCT As String, mschungtu As String, khchungtu As String, sochungtu As String) As ChungTuInfo
        Dim result As New ChungTuInfo()

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using comm As New SqlCommand()
                comm.Connection = conn

                comm.CommandText =
                  "WITH dsall AS (" &
                  "   SELECT MaCT,MSChungtu,KHChungtu,Sochungtu, PhanbietCT, TinhchatCT" &
                  "   FROM ChungtuthueTNCN " &
                  "   WHERE MasothueTC=@MasothueTC " &
                  "     AND ((MSChungtu=@MSChungtu AND KHChungtu=@KHChungtu AND Sochungtu=@Sochungtu)" &
                  "       OR (KHMSCTLienquan=@MSChungtu AND KHCTLienquan=@KHChungtu AND SoCTLienquan=@Sochungtu)) " &
                  "     AND TrangthaicuoiCT = 1" &
                  "), cre_ctgoc AS (" &
                  "   SELECT MaCT,PhanbietCT, TinhchatCT" &
                  "   FROM dsall " &
                  "   WHERE MSChungtu=@MSChungtu AND KHChungtu=@KHChungtu AND Sochungtu=@Sochungtu" &
                  "), cre_slct AS (" &
                  "   SELECT COUNT(MaCT) AS SLCTu FROM dsall" &
                  ") " &
                  "SELECT * FROM cre_slct a CROSS JOIN (SELECT * FROM cre_ctgoc) b"

                comm.Parameters.AddWithValue("@MasothueTC", madv)
                comm.Parameters.AddWithValue("@MSChungtu", mschungtu)
                comm.Parameters.AddWithValue("@KHChungtu", khchungtu)
                comm.Parameters.AddWithValue("@Sochungtu", sochungtu)

                Using reader As SqlDataReader = comm.ExecuteReader()
                    If reader.Read() Then
                        result.PhanBietCT = Convert.ToInt32(reader("PhanbietCT"))
                        result.TinhChatCT = Convert.ToInt32(reader("TinhchatCT"))
                        result.SLCTu = Convert.ToInt32(reader("SLCTu"))

                    End If
                End Using
            End Using
        End Using

        SqlConnection.ClearAllPools()
        Return result
    End Function


    <WebMethod()>
    Public Function XemThongDiep(MatokhaiCT As String, madonvi As String, type As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim content As String = String.Empty

            Dim chuoiketnoi As String = String.Empty

            Dim Khoaphien As String = String.Empty


            If (type = 6) Then
                Khoaphien = getKhoaPhienTD110(MatokhaiCT)
            End If

            chuoiketnoi = ConnectionStringtvan

            If type = 5 Then

                Dim td109 = getKetqua109(MatokhaiCT)
                If String.IsNullOrEmpty(td109) Then
                    td109 = getKetqua109(MatokhaiCT)
                End If

                Dim xml As Byte() = System.Text.Encoding.UTF8.GetBytes(td109)
                Dim xsltPath As String = Server.MapPath("~/xml/MauTD109/tokhai70.xslt")
                content = GetHtmlXemThongDiep(xsltPath, xml).Replace("NAN", "/")
            End If

            If type = 6 Then
                Dim tdct = getKetqua(Khoaphien, 111, chuoiketnoi)
                If String.IsNullOrEmpty(tdct) Then
                    tdct = getKetqua(Khoaphien, 110, chuoiketnoi)
                    If String.IsNullOrEmpty(tdct) Then
                        response("status") = "error"
                        response("message") = "Không có dữ liệu"
                        response("data") = ""
                        Return JsonConvert.SerializeObject(response)
                    End If

                    Dim xml As Byte() = System.Text.Encoding.UTF8.GetBytes(tdct)
                    Dim xsltPath As String = Server.MapPath("~/xml/MauTD110/ThongbaoChapnhanCT_CQT.xslt")
                    content = GetHtmlXemThongDiep(xsltPath, xml).Replace("NAN", "/")
                Else
                    Dim xml As Byte() = System.Text.Encoding.UTF8.GetBytes(tdct)
                    Dim xsltPath As String = Server.MapPath("~/xml/MauTD111/ThongbaoChapnhanCT_CQT.xslt")
                    content = GetHtmlXemThongDiep(xsltPath, xml).Replace("NAN", "/")
                End If

            End If



            response("status") = "success"
            response("data") = content
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)

    End Function


    Private Function getKetqua109(ByVal MatokhaiCT As String) As String
        Dim conn As SqlConnection = New SqlConnection()
        Dim comm As SqlCommand = New SqlCommand()
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn
        Dim result As String = String.Empty
        comm.CommandText = "Select XMLTokhai from Tokhaichungtu where MatokhaiCT = '" & MatokhaiCT & "' and TrangthaicuoiTKCT=1"
        Dim reader As SqlDataReader = comm.ExecuteReader()
        If reader.HasRows Then
            While reader.Read
                result = reader("XMLTokhai").ToString
                result = Encoding.UTF8.GetString(Convert.FromBase64String(result))
            End While
        Else
            result = String.Empty
        End If
        conn.Close()
        comm.Dispose()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return result
    End Function

    Private Function getKhoaPhienTD110(ByVal MatokhaiCT As String) As String
        Dim conn As SqlConnection = New SqlConnection()
        Dim comm As SqlCommand = New SqlCommand()
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn
        Dim result As String = String.Empty
        comm.CommandText = "Select KhoaphienTN from Tokhaichungtu where MatokhaiCT = '" & MatokhaiCT & "' and TrangthaicuoiTKCT=1"
        Dim reader As SqlDataReader = comm.ExecuteReader()
        If reader.HasRows Then
            While reader.Read
                result = reader("KhoaphienTN").ToString
            End While
        Else
            result = String.Empty
        End If
        conn.Close()
        comm.Dispose()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return result
    End Function


    Private Function getKetqua(ByVal khoaphien As String, ByVal mltdiep As String, chuoiketnoi As String) As String
        Dim conn As SqlConnection = New SqlConnection()
        Dim comm As SqlCommand = New SqlCommand()
        conn.ConnectionString = chuoiketnoi
        conn.Open()
        comm.Connection = conn
        Dim result As String = String.Empty
        comm.CommandText = "Select  XMLThongdiep from Logtruyennhan where Khoaphien = '" & khoaphien & "' and MLTDiep='" & mltdiep & "'"
        Dim reader As SqlDataReader = comm.ExecuteReader()
        If reader.HasRows Then
            While reader.Read()
                If reader(0) IsNot DBNull.Value Then
                    result = reader(0).ToString()
                    result = Encoding.UTF8.GetString(Convert.FromBase64String(result))
                Else
                    result = String.Empty
                End If
            End While
        Else
            result = String.Empty
        End If
        conn.Close()
        comm.Dispose()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return result
    End Function

    Public Function GetHtmlXemThongDiep(xsltPath As String, xml As Byte()) As String

        Dim stream As New MemoryStream(xml)
        Dim document As New XPathDocument(stream)
        Dim writer As New StringWriter()
        Dim argList As XsltArgumentList = New XsltArgumentList()


        Dim transform As New XslCompiledTransform()
        transform.Load(xsltPath, New XsltSettings(True, True), New XmlUrlResolver())
        transform.Transform(document, argList, writer)
        Return writer.ToString()
    End Function

    Private Function getKhoaphien(ByVal idtruyennhan As String, chuoiketnoi As String) As String
        Dim conn As SqlConnection = New SqlConnection()
        Dim comm As SqlCommand = New SqlCommand()
        conn.ConnectionString = chuoiketnoi
        conn.Open()
        comm.Connection = conn
        Dim result As String = String.Empty
        comm.CommandText = "Select  Khoaphien from Logtruyennhan where idTruyennhan = '" & idtruyennhan & "'"
        Dim reader As SqlDataReader = comm.ExecuteReader()

        If reader.HasRows Then

            While reader.Read()
                If reader(0) IsNot DBNull.Value Then
                    result = reader(0).ToString()
                Else
                    result = String.Empty
                End If
            End While
        Else
            result = String.Empty
        End If

        reader.Close()
        conn.Close()
        comm.Dispose()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return result
    End Function

    Private Function getKhoaPhienTD301(ByVal MaTBSSCT As String, MST As String) As String

        Dim conn As SqlConnection = New SqlConnection()
        Dim comm As SqlCommand = New SqlCommand()
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn
        Dim result As String = String.Empty
        comm.CommandText = "Select KhoaphienTN from TBSSChungtu where MaTBSSCT = '" & MaTBSSCT & "' and MST = '" & MST & "'  and Trangthaicuoi=1"
        Dim reader As SqlDataReader = comm.ExecuteReader()
        If reader.HasRows Then
            While reader.Read
                result = reader("KhoaphienTN").ToString
            End While
        Else
            result = String.Empty
        End If
        conn.Close()
        comm.Dispose()
        conn.Dispose()
        SqlConnection.ClearAllPools()
        Return result
    End Function

    <WebMethod()>
    Public Function XemKetQuaTBSS(matbssct As String, madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim content As String = String.Empty

            Dim chuoiketnoi As String = String.Empty

            Dim Khoaphien As String = String.Empty


            Khoaphien = getKhoaPhienTD301(matbssct, madonvi)

            chuoiketnoi = ConnectionStringtvan

            Dim xmlTD = getKetqua(Khoaphien, 301, chuoiketnoi)
            If String.IsNullOrEmpty(xmlTD) Then
                xmlTD = getKetqua(Khoaphien, 213, chuoiketnoi)
                If String.IsNullOrEmpty(xmlTD) Then


                    xmlTD = getKetqua(Khoaphien, -1, chuoiketnoi)

                    If String.IsNullOrEmpty(xmlTD) Then
                        response("status") = "error"
                        response("data") = "Không có dữ liệu"
                        Return JsonConvert.SerializeObject(response)
                    End If


                End If

            End If


            Dim xml As Byte() = System.Text.Encoding.UTF8.GetBytes(xmlTD)

            Dim xsltPath As String = Server.MapPath("~/xml/MauTD301/Tdiep_04tbss_ctu.xslt")
            'content = GetHtml(xsltPath, xml).Replace("NAN", "/")
            Dim fullContent As String = GetHtmlXemChungTu(xsltPath, xml).Replace("NAN", "/")


            Dim startIdx As Integer = fullContent.IndexOf("<html", StringComparison.OrdinalIgnoreCase)
            Dim endIdx As Integer = fullContent.LastIndexOf("</html>", StringComparison.OrdinalIgnoreCase)


            Dim cleanHtml As String = String.Empty
            If startIdx >= 0 AndAlso endIdx > startIdx Then

                cleanHtml = fullContent.Substring(startIdx, (endIdx - startIdx) + 7)
            End If


            content = cleanHtml



            response("status") = "success"
            response("data") = content
        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)

    End Function




    <WebMethod()>
    Public Function TaiPDFChungTu(machungtu As String, madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim htmlContent As String = madonvi

            Dim fdname As String = Path.GetRandomFileName()
            Dim tempPath As String = Server.MapPath("~/temp/")
            Dim htmlPath As String = Path.Combine(tempPath, fdname & ".html")
            Dim pdfPath As String = Path.Combine(tempPath, fdname & ".pdf")

            System.IO.File.WriteAllText(htmlPath, htmlContent, System.Text.Encoding.UTF8)

            Dim theDoc As New Doc()
            theDoc.Rect.Inset(0, 0)
            theDoc.HtmlOptions.Engine = EngineType.Chrome
            theDoc.Page = theDoc.AddPage()
            theDoc.HtmlOptions.RepaintDelay = 2000
            theDoc.HtmlOptions.RepaintTimeout = 5000

            Dim theID As Integer = theDoc.AddImageUrl("file:///" & htmlPath)
            While theDoc.Chainable(theID)
                theDoc.Page = theDoc.AddPage()
                theID = theDoc.AddImageToChain(theID)
            End While

            For i As Integer = 1 To theDoc.PageCount
                theDoc.PageNumber = i
                theDoc.Flatten()
            Next

            theDoc.Save(pdfPath)
            theDoc.Clear()
            theDoc.Dispose()

            ' Bước 3: Convert PDF sang Base64
            Dim fileBytes As Byte() = System.IO.File.ReadAllBytes(pdfPath)
            Dim base64Content As String = Convert.ToBase64String(fileBytes)

            ' Bước 4: Trả JSON
            response("status") = "success"
            response("data") = base64Content

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function LoadSoChungTu(madonvi As String, mauso As String, kyhieu As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim dt As New DataTable()
            Dim Sql As String = String.Format("Select a.Sochungtu,a.NgaylapCT  from ChungtuthueTNCN a left join KQTNChungtu b on  a.KhoaphienTN=b.khoaphienTN and a.MaCT=b.mact where  a.MasothueTC='" & madonvi & "' and (a.MSChungtu='{0}') and (a.KHChungtu='{1}') and TinhtrangCT in ('3', '33') and (b.KQ213 = 2 OR (b.KQ213 IN (10, 12) AND b.DSLDo IS NULL))  order by a.Sochungtu", mauso, kyhieu)

            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(Sql, conn)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
            ' Bước 4: Trả JSON
            response("status") = "success"
            response("data") = dt

        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function




    ''Gửi Thông báo sai sót
    <WebMethod()>
    Public Function TaoThongBaoSaiSot(sjsonTTChungTBSS As String, sjsonTTCT As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Try
            Dim matbss As Integer
            matbss = TaotbssCT70(sjsonTTChungTBSS, sjsonTTCT)


            If matbss > 0 Then
                response("status") = "success"
                response("message") = "Tạo thành công"
                response("data") = matbss
            Else
                response("status") = "error"
                response("message") = "Tạo thông báo sai sót không thành công"
            End If


        Catch ex As Exception
            response("status") = "error"
            response("message") = ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function UpdateChungTuTBSSSauKy(xmlthongdiep As String, trangthai As String, madonvi As String, matbsschungtu As Integer) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "UPDATE TBSSChungtu SET XMLTBSS = @xmlthongdiep, Trangthai = @trangthai WHERE MST = @madonvi AND MaTBSSCT = @MaTBSSCT"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.Add("@xmlthongdiep", SqlDbType.NVarChar).Value = xmlthongdiep
                    cmd.Parameters.Add("@trangthai", SqlDbType.NVarChar, 20).Value = trangthai
                    cmd.Parameters.Add("@madonvi", SqlDbType.NVarChar, 50).Value = madonvi
                    cmd.Parameters.Add("@MaTBSSCT", SqlDbType.Int).Value = matbsschungtu

                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        response("status") = "success"
                        response("message") = "Cập nhật chứng từ thành công"
                    Else
                        response("status") = "error"
                        response("message") = "Không tìm thấy chứng từ để cập nhật"
                    End If
                End Using
            End Using

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    Private Function TaotbssCT70(ByVal sjsonTTChungTBSS As String, ByVal sjsonTTCT As String) As Integer
        Dim kq As Integer = 0

        Dim jTTChungTK As ThongtinTBSSChungtu = Nothing
        Dim dt_ctss As New DataTable()

        Try
            If Not String.IsNullOrEmpty(sjsonTTChungTBSS) Then
                jTTChungTK = JsonConvert.DeserializeObject(Of ThongtinTBSSChungtu)(sjsonTTChungTBSS)
            End If

            If Not String.IsNullOrEmpty(sjsonTTCT) Then
                dt_ctss = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTCT)
            End If

        Catch ex As JsonSerializationException
            Return -1 ' JSON sai định dạng
        End Try

        If jTTChungTK IsNot Nothing AndAlso dt_ctss.Rows.Count > 0 Then
            Dim xmltbss As String = TaoxmlTBSSChungtu(sjsonTTChungTBSS, sjsonTTCT)

            If xmltbss.Length < 10 Then
                Return Convert.ToInt32(xmltbss)
            End If

            Dim byte1 As Byte() = System.Text.Encoding.UTF8.GetBytes(xmltbss)
            xmltbss = Convert.ToBase64String(byte1)

            Using conn As New SqlConnection(connectionString)
                conn.Open()

                Using comm As SqlCommand = conn.CreateCommand()
                    comm.CommandText = "INSERT INTO TBSSChungtu (" &
                   "PBan, MSo, Ten, Loai, So, NTBCCQT, MCQT, TCQT, TNNT, MST, DDanh, NTBao, " &
                   "Ngaytao, Ngaycapnhat, Trangthai, SerialNo, Taikhoan, XMLTBSS) " &
                   "OUTPUT INSERTED.MaTBSSCT " &
                   "VALUES (" &
                   "@PBan, @MSo, @Ten, @Loai, @So, @NTBCCQT, @MCQT, @TCQT, @TNNT, @MST, @DDanh, @NTBao, " &
                   "GETDATE(), GETDATE(), @Trangthai, @SerialNo, @Taikhoan, @XMLTBSS)"

                    comm.Parameters.AddWithValue("@PBan", jTTChungTK.PBan)
                    comm.Parameters.AddWithValue("@MSo", jTTChungTK.MSo)
                    comm.Parameters.AddWithValue("@Ten", jTTChungTK.Ten)
                    comm.Parameters.AddWithValue("@Loai", jTTChungTK.Loai)
                    comm.Parameters.AddWithValue("@So", If(jTTChungTK.So IsNot Nothing, jTTChungTK.So, DBNull.Value))
                    comm.Parameters.AddWithValue("@NTBCCQT", If(jTTChungTK.NTBCCQT IsNot Nothing, jTTChungTK.NTBCCQT, DBNull.Value))
                    comm.Parameters.AddWithValue("@MCQT", jTTChungTK.MCQT)
                    comm.Parameters.AddWithValue("@TCQT", jTTChungTK.TCQT)
                    comm.Parameters.AddWithValue("@TNNT", jTTChungTK.TNNT)
                    comm.Parameters.AddWithValue("@MST", jTTChungTK.MST)
                    comm.Parameters.AddWithValue("@DDanh", jTTChungTK.DDanh)
                    comm.Parameters.AddWithValue("@NTBao", jTTChungTK.NTBao)
                    comm.Parameters.AddWithValue("@Trangthai", 1)
                    comm.Parameters.AddWithValue("@SerialNo", If(jTTChungTK.SerialNo IsNot Nothing, jTTChungTK.SerialNo, DBNull.Value))
                    comm.Parameters.AddWithValue("@Taikhoan", If(jTTChungTK.Taikhoan IsNot Nothing, jTTChungTK.Taikhoan, DBNull.Value))
                    comm.Parameters.AddWithValue("@XMLTBSS", xmltbss)

                    kq = Convert.ToInt32(comm.ExecuteScalar())
                End Using

                If kq = 0 Then Return 0

                ' Gắn MaTBSSCT vào từng dòng
                Dim col As New DataColumn("MaTBSSCT", GetType(Integer))
                dt_ctss.Columns.Add(col)
                col.SetOrdinal(0)

                For Each row As DataRow In dt_ctss.Rows
                    row("MaTBSSCT") = kq
                Next

                ' Tạo bảng tạm
                Using cmdCreate As SqlCommand = conn.CreateCommand()
                    cmdCreate.CommandText =
                        "IF OBJECT_ID('tempdb..#TempTBSSChungtuchitiet') IS NOT NULL DROP TABLE #TempTBSSChungtuchitiet; " &
                        "CREATE TABLE #TempTBSSChungtuchitiet (" &
                        "MaTBSSCT int, " &
                        "STT numeric(4,0), " &
                        "KHMSCTu nvarchar(10), " &
                        "KHCTu nvarchar(9), " &
                        "SCTu nvarchar(7), " &
                        "NLap nvarchar(40), " &
                        "LCTDT tinyint, " &
                        "LDo nvarchar(255) " &
                        ");"
                    cmdCreate.ExecuteNonQuery()
                End Using

                ' Đổ dữ liệu vào bảng tạm
                Using bulkCopy As New SqlBulkCopy(conn)
                    bulkCopy.DestinationTableName = "#TempTBSSChungtuchitiet"
                    bulkCopy.WriteToServer(dt_ctss)
                End Using

                ' Insert từ bảng tạm vào chính thức
                Using cmdInsert As SqlCommand = conn.CreateCommand()
                    cmdInsert.CommandText = "INSERT INTO TBSSChungtuchitiet SELECT * FROM #TempTBSSChungtuchitiet"
                    cmdInsert.ExecuteNonQuery()
                End Using

            End Using
        Else
            kq = -100 ' Dữ liệu không hợp lệ
        End If

        Return kq
    End Function

    Private Function TaoxmlTBSSChungtu(sjsonTTChungTBSS As String, sjsonTTCT As String) As String
        Dim kq As String = ""

        Dim jTTChungTBSS As ThongtinTBSSChungtu = Nothing
        Dim dt_ctss As New DataTable()

        Try
            If Not String.IsNullOrEmpty(sjsonTTChungTBSS) Then
                jTTChungTBSS = JsonConvert.DeserializeObject(Of ThongtinTBSSChungtu)(sjsonTTChungTBSS)
            End If

            If Not String.IsNullOrEmpty(sjsonTTCT) Then
                dt_ctss = JsonConvert.DeserializeObject(Of DataTable)(sjsonTTCT)
            End If
        Catch ex As JsonSerializationException
            Return "-1" ' JSON sai định dạng
        End Try

        If jTTChungTBSS IsNot Nothing AndAlso dt_ctss.Rows.Count > 0 Then
            Dim linkelement As String = ""
            Dim doc As New XmlDocument()

            Dim sId As String
            sId = System.Guid.NewGuid.ToString()

            ' TBao
            Dim TBaoNode As XmlElement = doc.CreateElement("", "TBao", linkelement)
            doc.AppendChild(TBaoNode)

            ' DLTBao
            Dim DLTBaoNode As XmlNode = doc.CreateElement("", "DLTBao", linkelement)
            Dim productAttribute As XmlAttribute = doc.CreateAttribute("Id")
            productAttribute.Value = "_" & sId
            DLTBaoNode.Attributes.Append(productAttribute)
            TBaoNode.AppendChild(DLTBaoNode)

            ' PBan
            Dim PBXMLNode As XmlNode = doc.CreateElement("", "PBan", linkelement)
            PBXMLNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.PBan))
            DLTBaoNode.AppendChild(PBXMLNode)

            ' MSo
            Dim MSoNode As XmlNode = doc.CreateElement("", "MSo", linkelement)
            MSoNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.MSo))
            DLTBaoNode.AppendChild(MSoNode)

            ' Ten
            Dim TenNode As XmlNode = doc.CreateElement("", "Ten", linkelement)
            TenNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.Ten))
            DLTBaoNode.AppendChild(TenNode)

            ' Loai
            Dim LoaiNode As XmlNode = doc.CreateElement("", "Loai", linkelement)
            LoaiNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.Loai.ToString()))
            DLTBaoNode.AppendChild(LoaiNode)

            If jTTChungTBSS.Loai = 2 Then
                Try
                    Dim SoNode As XmlNode = doc.CreateElement("", "So", linkelement)
                    SoNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.So.ToString()))
                    DLTBaoNode.AppendChild(SoNode)

                    Dim NTBCCQTNode As XmlNode = doc.CreateElement("", "NTBCCQT", linkelement)
                    NTBCCQTNode.AppendChild(doc.CreateTextNode(Thoigianchuan(jTTChungTBSS.NTBCCQT)))
                    DLTBaoNode.AppendChild(NTBCCQTNode)
                Catch ex As Exception
                    Return "-200"
                End Try
            End If

            ' MCQT
            Dim MCQTNode As XmlNode = doc.CreateElement("", "MCQT", linkelement)
            MCQTNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.MCQT))
            DLTBaoNode.AppendChild(MCQTNode)

            ' TCQT
            Dim TCQTNode As XmlNode = doc.CreateElement("", "TCQT", linkelement)
            TCQTNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.TCQT))
            DLTBaoNode.AppendChild(TCQTNode)

            ' TNNT
            Dim TNNTNode As XmlNode = doc.CreateElement("", "TNNT", linkelement)
            TNNTNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.TNNT))
            DLTBaoNode.AppendChild(TNNTNode)

            ' MST
            Dim MSTNode As XmlNode = doc.CreateElement("", "MST", linkelement)
            MSTNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.MST))
            DLTBaoNode.AppendChild(MSTNode)

            ' DDanh
            Dim DDanhNode As XmlNode = doc.CreateElement("", "DDanh", linkelement)
            DDanhNode.AppendChild(doc.CreateTextNode(jTTChungTBSS.DDanh))
            DLTBaoNode.AppendChild(DDanhNode)

            ' NTBao
            Dim NTBaoNode As XmlNode = doc.CreateElement("", "NTBao", linkelement)
            NTBaoNode.AppendChild(doc.CreateTextNode(Thoigianchuan(jTTChungTBSS.NTBao)))
            DLTBaoNode.AppendChild(NTBaoNode)

            ' DSCTu
            Dim DSCTuNode As XmlNode = doc.CreateElement("", "DSCTu", linkelement)
            DLTBaoNode.AppendChild(DSCTuNode)

            If dt_ctss.Rows.Count > 0 Then
                For Each row As DataRow In dt_ctss.Rows
                    Dim CTuNode As XmlNode = doc.CreateElement("", "CTu", linkelement)
                    DSCTuNode.AppendChild(CTuNode)

                    Dim STTNode As XmlNode = doc.CreateElement("", "STT", linkelement)
                    STTNode.AppendChild(doc.CreateTextNode(row("STT").ToString()))
                    CTuNode.AppendChild(STTNode)

                    Dim KHMSCTuNode As XmlNode = doc.CreateElement("", "KHMSCTu", linkelement)
                    KHMSCTuNode.AppendChild(doc.CreateTextNode(row("KHMSCTu").ToString()))
                    CTuNode.AppendChild(KHMSCTuNode)

                    Dim KHCTuNode As XmlNode = doc.CreateElement("", "KHCTu", linkelement)
                    KHCTuNode.AppendChild(doc.CreateTextNode(row("KHCTu").ToString()))
                    CTuNode.AppendChild(KHCTuNode)

                    Dim SCTuNode As XmlNode = doc.CreateElement("", "SCTu", linkelement)
                    SCTuNode.AppendChild(doc.CreateTextNode(row("SCTu").ToString()))
                    CTuNode.AppendChild(SCTuNode)

                    Dim NLapNode As XmlNode = doc.CreateElement("", "NLap", linkelement)
                    NLapNode.AppendChild(doc.CreateTextNode(Thoigianchuan(row("NLap").ToString())))
                    CTuNode.AppendChild(NLapNode)

                    Dim LCTDTNode As XmlNode = doc.CreateElement("", "LCTDT", linkelement)
                    LCTDTNode.AppendChild(doc.CreateTextNode(row("LCTDT").ToString()))
                    CTuNode.AppendChild(LCTDTNode)

                    Dim LDoNode As XmlNode = doc.CreateElement("", "LDo", linkelement)
                    LDoNode.AppendChild(doc.CreateTextNode(row("LDo").ToString()))
                    CTuNode.AppendChild(LDoNode)
                Next
            End If

            ' DSCKS
            Dim DSCKSNode As XmlNode = doc.CreateElement("", "DSCKS", linkelement)
            TBaoNode.AppendChild(DSCKSNode)

            Dim CKSNNTNode As XmlNode = doc.CreateElement("", "NNT", linkelement)
            DSCKSNode.AppendChild(CKSNNTNode)

            Dim CCKSKhacNode As XmlNode = doc.CreateElement("", "CCKSKhac", linkelement)
            DSCKSNode.AppendChild(CCKSKhacNode)

            kq = doc.InnerXml
        Else
            kq = "-100" ' DL đầu vào không hợp lệ
        End If

        Return kq
    End Function


    <WebMethod()>
    Public Function GuiTBSSLenCQT(signedtext As String, madonvi As String, matbsschungtu As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Dim kqguithue As String = String.Empty

        Try

            If Not String.IsNullOrEmpty(matbsschungtu) Then

                Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                Dim key As String = "0103930279" & guidstr.Replace("-", "")

                Dim xmlthongdiep As String = CreatFileXML_Thong_diep_den_co_quan_thue("0103930279", madonvi, "304", key, "", "1", madonvi, signedtext)

                Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(xmlthongdiep))

                ''Gui thong diep 
                Dim servicetvan As ServiceTVAN.WSInterTRCA2 = New ServiceTVAN.WSInterTRCA2()
                Dim ttketnoi As New ServiceTVAN.AuthHeader
                ttketnoi.Username = "ntvan"
                ttketnoi.Password = "123456"
                servicetvan.AuthHeaderValue = ttketnoi
                servicetvan.Timeout = 2147483647
                Dim macode As String = servicetvan.Guithongdiep(xmlthongdiep, 0)
                'Dim macode As String = "0103930279-998_0_0_100_40676b3bfdc141ba9e2ea6a346085836"
                If macode.Length > 10 Then

                    UpdateKhoaphienTBSSSauGui(3, madonvi, matbsschungtu, macode)

                    ''lay ket qua phan hoi tu CQT
                    Dim phanhoi As String = String.Empty
                    While phanhoi = ""
                        Thread.Sleep(10000)
                        phanhoi = servicetvan.LayKQThongdiep(macode, "0103930279")
                    End While

                    LayKQTruyennhanchungtudientu(madonvi, macode)

                    If phanhoi = "-1" Then
                        kqguithue = "0|Xác thực không đúng"
                    ElseIf phanhoi = "-5" Then
                        kqguithue = "0|Mã số thuế trong thông điệp không khớp với tài khoản"
                    ElseIf phanhoi = "-6" Then
                        kqguithue = "0|Không có thông điệp nào thỏa mãn"
                    ElseIf phanhoi = "-7" Then
                        kqguithue = "1|Chưa có kết quả phản hồi của cơ quan thuế"
                    Else
                        If phanhoi.Contains("<THop>1</THop>") Or phanhoi.Contains("<THop>3</THop>") Then
                            kqguithue = "1|CQT đã tiếp nhận, vui lòng chờ CQT xử lý"
                        Else
                            kqguithue = "1|CQT không tiếp nhận, vui lòng quay lại danh sách để xem chi tiết."
                        End If
                    End If
                    'kqguithue = "1|Đã gửi chứng từ lên Cơ quan thuế, vui lòng đợi phản hồi."
                Else
                    kqguithue = "0|Không gửi được chứng từ lên CQT"
                End If

            Else
                kqguithue = "0|Chưa có thông tin chứng từ"
            End If



            Dim parts() As String = kqguithue.Split("|"c)

            If parts.Length > 1 Then
                If parts(0) = "1" Then
                    response("status") = "success"
                Else
                    response("status") = "error"
                End If
                response("message") = parts(1)
            Else
                response("status") = "error"
                response("message") = "Lỗi không xác định"
            End If
        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function

    Protected Sub UpdateKhoaphienTBSSSauGui(Trangthai As String, MST As String, MaTBSSCT As Integer, KhoaphienTN As String)
        Dim res1 As Integer = 0
        Dim res2 As Integer = 0

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using cmd As New SqlCommand("UPDATE TBSSChungtu SET Trangthai=@Trangthai, KhoaphienTN=@KhoaphienTN WHERE MST=@MST AND MaTBSSCT=@MaTBSSCT", conn)
                cmd.Parameters.Add("@Trangthai", SqlDbType.NVarChar, 50).Value = Trangthai
                cmd.Parameters.Add("@MST", SqlDbType.NVarChar, 20).Value = MST
                cmd.Parameters.Add("@MaTBSSCT", SqlDbType.Int).Value = MaTBSSCT
                cmd.Parameters.Add("@KhoaphienTN", SqlDbType.NVarChar, 250).Value = KhoaphienTN
                Try
                    res1 = cmd.ExecuteNonQuery()
                Catch ex As Exception
                    res1 = 0
                End Try
            End Using

            Using cmdd As New SqlCommand("insert into KQTNTBSSChungtu(KhoaphienTN,MaCT,MSChungtu,KHChungtu,Sochungtu,NgaylapCT,Ngaygui) select a.KhoaphienTN,a.MaTBSSCT,c.KHMSCTu,c.KHCTu,c.SCTu,c.NLap,getdate() from TBSSChungtu a inner join TBSSChungtuchitiet c on a.MaTBSSCT=c.MaTBSSCT left join KQTNTBSSChungtu b on a.KhoaphienTN=b.KhoaphienTN where a.KhoaphienTN=@KhoaphienTN and b.KhoaphienTN is null", conn)
                cmdd.Parameters.Add("@KhoaphienTN", SqlDbType.NVarChar, 250).Value = KhoaphienTN
                Try
                    'conn.Open()
                    res2 = cmdd.ExecuteNonQuery()
                Catch ex As Exception
                    res2 = 0
                End Try
            End Using

        End Using

    End Sub



    <WebMethod()>
    Public Function LayXmlTBSSChungTu(madonvi As String, matbss_ct As Integer) As String
        Dim response As New Dictionary(Of String, Object)()
        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("SELECT XMLTBSS FROM TBSSChungtu WHERE MST=@mst AND MaTBSSCT=@matbss_ct", conn)
            cmd.Parameters.AddWithValue("@mst", madonvi)
            cmd.Parameters.AddWithValue("@matbss_ct", matbss_ct)

            Try
                conn.Open()
                Dim obj As Object = cmd.ExecuteScalar()

                If obj IsNot Nothing AndAlso obj IsNot DBNull.Value Then
                    response("status") = "success"
                    response("message") = "Lấy thông báo sai sót thành công thành công"
                    response("data") = obj.ToString()
                Else
                    response("status") = "error"
                    response("message") = "Không tìm thấy thông tin"
                End If

            Catch ex As Exception
                response("status") = "error"
                response("message") = "Lỗi hệ thống: " & ex.Message
            End Try
        End Using

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod()>
    Public Function Laythongtintbsschungtu(matbss_ct As Integer, madonvi As String) As String
        Dim dtChungtu As New DataTable("TBSSChungtu")
        Dim dtChitiet As New DataTable("TBSSChungtuchitiet")
        Dim response As New Dictionary(Of String, Object)()

        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' ---- Lấy thông tin TBSSChungtu ----
                Dim queryChungtu As String = "SELECT * FROM TBSSChungtu WHERE MST=@madonvi AND MaTBSSCT=@matbss_ct"
                Using cmd As New SqlCommand(queryChungtu, connection)
                    cmd.Parameters.Add("@matbss_ct", SqlDbType.Int).Value = matbss_ct
                    cmd.Parameters.Add("@madonvi", SqlDbType.NVarChar, 15).Value = madonvi

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dtChungtu)
                    End Using
                End Using

                ' ---- Lấy danh sách chi tiết ----
                Dim queryChitiet As String = "SELECT * FROM TBSSChungtuchitiet WHERE MaTBSSCT=@matbss_ct"
                Using cmd As New SqlCommand(queryChitiet, connection)
                    cmd.Parameters.Add("@matbss_ct", SqlDbType.Int).Value = matbss_ct

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dtChitiet)
                    End Using
                End Using
            End Using

            If dtChungtu.Rows.Count > 0 Then
                response("status") = "success"
                response("message") = "Lấy thông tin thành công"

                ' ---- Gộp cả 2 bảng vào 1 object ----
                Dim data As New Dictionary(Of String, Object)()
                data("TBSSChungtu") = dtChungtu
                data("TBSSChungtuchitiet") = dtChitiet

                response("data") = data
            Else
                response("status") = "error"
                response("message") = "Không tìm thấy thông báo sai sót"
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function



    Public Function CapnhatKhoaphienchungtutbsschuacapnhatKQ(ByVal MaDV As String, ByVal KHCTu As String) As String
        Dim kqBuilder As New StringBuilder()

        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim myQuery As String = "SELECT DISTINCT a.KhoaphienTN " &
                        "FROM KQTNTBSSChungtu a " &
                        "INNER JOIN TBSSChungtu b ON a.KhoaphienTN = b.KhoaphienTN " &
                        "WHERE ((a.KQ999 = '0' AND a.KQ301 IS NULL) OR a.KQ999 IS NULL) " &
                        "AND a.MSChungtu = '03/TNCN' " &
                        "AND a.KHChungtu = @khctu " &
                        "AND MST = @madv"

                Using cmd As New SqlCommand(myQuery, connection)
                    cmd.Parameters.Add("@madv", SqlDbType.VarChar).Value = MaDV
                    cmd.Parameters.Add("@khctu", SqlDbType.VarChar).Value = KHCTu

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                kqBuilder.Append(reader.GetValue(0).ToString()).Append(",")
                            End If
                        End While
                    End Using
                End Using
            End Using

            If kqBuilder.Length > 0 Then
                kqBuilder.Length -= 1 ' Xóa dấu phẩy cuối
            End If
        Catch ex As Exception
            ' Xử lý ngoại lệ nếu cần
        End Try

        If Not String.IsNullOrEmpty(kqBuilder.ToString()) Then
            LayKQTruyennhanchungtutbss(MaDV, kqBuilder.ToString())
        End If

        Return kqBuilder.ToString()
    End Function



    <WebMethod()>
    Public Function Laydanhsachtbsschungtu(madonvi As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Dim dtTBSS As DataTable = New DataTable("DSTBSSCT")

        Try
            CapnhatKhoaphienchungtutbsschuacapnhatKQ(madonvi, "CT/25E")

            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim sql As String =
                "SELECT a.MaTBSSCT," & vbCrLf &
                "       a.Trangthai," & vbCrLf &
                "       b.KHCTu," & vbCrLf &
                "       b.SCTu," & vbCrLf &
                "       b.NLap," & vbCrLf &
                "       b.LDo," & vbCrLf &
                "       c.KQ301," & vbCrLf &
                "       c.KQKhac," & vbCrLf &
                "       CASE " & vbCrLf &
                "            WHEN c.KQ301 = 1 AND c.DSLDo IS NULL AND c.KQKhac IS NULL" & vbCrLf &
                "                 THEN N'CQT đã duyệt thông báo sai sót'" & vbCrLf &
                "            WHEN c.KQ301 = 1 AND c.DSLDo IS NOT NULL AND c.KQKhac IS NULL" & vbCrLf &
                "                 THEN c.DSLDo" & vbCrLf &
                "            WHEN c.DSLDo IS NOT NULL AND c.DSLDo <> '' AND c.KQKhac = 213" & vbCrLf &
                "                 THEN c.DSLDo" & vbCrLf &
                "            ELSE c.MTa" & vbCrLf &
                "       END AS kqxly," & vbCrLf &
                "       CASE " & vbCrLf &
                "            WHEN c.KQ301 = 1 THEN N'Hợp lệ'" & vbCrLf &
                "            WHEN c.KQKhac IN (213, -1)  THEN N'Không hợp lệ'" & vbCrLf &
                "            ELSE N''" & vbCrLf &
                "       END AS Ketquadoichieu" & vbCrLf &
                "FROM TBSSChungtu a" & vbCrLf &
                "INNER JOIN TBSSChungtuchitiet b" & vbCrLf &
                "       ON a.MaTBSSCT = b.MaTBSSCT" & vbCrLf &
                "LEFT JOIN KQTNTBSSChungtu c" & vbCrLf &
                "       ON a.KhoaphienTN = c.KhoaphienTN" & vbCrLf &
                "      AND b.SCTu = c.Sochungtu" & vbCrLf &
                "WHERE a.MST = N'" & madonvi & "'" & vbCrLf &
                "  AND a.Trangthaicuoi = 1;"

                Using cmd As New SqlCommand(sql, connection)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dtTBSS)
                    End Using
                End Using
            End Using

            response("status") = "success"
            response("message") = "Lấy dữ liệu thành công"
            response("data") = dtTBSS

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
            response("data") = Nothing
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    Public Function LayKQTruyennhanchungtutbss(ByVal MaDV As String, ByVal dskhoaphien As String) As DataTable
        Dim dt As DataTable = New DataTable("DSHD")

        If String.IsNullOrWhiteSpace(dskhoaphien) Then Return dt
        Dim dtKhoaphien As DataTable = New DataTable()
        dtKhoaphien.Columns.Add("Khoaphien", GetType(String))

        For Each kp In dskhoaphien.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
            dtKhoaphien.Rows.Add(kp.Trim())
        Next

        Try

            Using connection As SqlConnection = New SqlConnection(ConnectionStringtvan)
                connection.Open()
                Dim createTempTable As String = "CREATE TABLE #tmp_Khoaphien (Khoaphien NVARCHAR(250));"

                Using cmdCreate As SqlCommand = New SqlCommand(createTempTable, connection)
                    cmdCreate.ExecuteNonQuery()
                End Using

                Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(connection)
                    bulkCopy.DestinationTableName = "#tmp_Khoaphien"
                    bulkCopy.WriteToServer(dtKhoaphien)
                End Using

                'Dim myQuery As String =
                '       "WITH ALLTDIEP AS (" & vbCrLf &
                '       "    SELECT a.Khoaphien, MTDiep, MLTDiep, MNGui, MTDTchieu, XMLThongdiep" & vbCrLf &
                '       "    FROM Logtruyennhan a INNER JOIN #tmp_Khoaphien b ON a.khoaphien=b.khoaphien" & vbCrLf &
                '       "    WHERE a.MST=@MST" & vbCrLf &
                '       ")," & vbCrLf &
                '       "CTE_TDGuiGoc AS (" & vbCrLf &
                '       "    SELECT Khoaphien, MTDiep, mltdiep, MNgui" & vbCrLf &
                '       "    FROM ALLTDIEP" & vbCrLf &
                '       "    WHERE MNGui = 'V0103930279' AND mltdiep <> '999'" & vbCrLf &
                '       ")," & vbCrLf &
                '       "CTE_TD999 AS (" & vbCrLf &
                '       "    SELECT MTDTchieu," & vbCrLf &
                '       "           CASE WHEN CHARINDEX('TTTNhan', evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep,'')) > 0" & vbCrLf &
                '       "                THEN 1" & vbCrLf &
                '       "                ELSE 0 END AS KQ999" & vbCrLf &
                '       "    FROM ALLTDIEP" & vbCrLf &
                '       "    WHERE MNGui = 'TCT' AND mltdiep = '999'" & vbCrLf &
                '       ")," & vbCrLf &
                '       "CTE_TD301 AS (" & vbCrLf &
                '       "    SELECT MTDTchieu, KQ301," & vbCrLf &
                '       "           CTu.value('(KHMSCTu)[1]','nvarchar(10)') AS KHMSCTu," & vbCrLf &
                '       "           CTu.value('(KHCTu)[1]','nvarchar(255)') AS KHCTu," & vbCrLf &
                '       "           CTu.value('(SCTu)[1]','nvarchar(255)') AS SCTu," & vbCrLf &
                '       "           CAST(CTu.query('(DSLDKTNhan)[1]') AS NVARCHAR(MAX)) AS DSLDo" & vbCrLf &
                '       "    FROM (" & vbCrLf &
                '       "         SELECT MTDTchieu, 1 AS KQ301," & vbCrLf &
                '       "                CAST(NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'DSCTu','DLTBao') AS XML) AS XmlData" & vbCrLf &
                '       "         FROM ALLTDIEP" & vbCrLf &
                '       "         WHERE MNGui = 'TCT' AND mltdiep='301'" & vbCrLf &
                '       "    ) AS T OUTER APPLY XmlData.nodes('/CTu') AS L(CTu)" & vbCrLf &
                '       ")," & vbCrLf &
                '        "CTE_TDKHAC AS (" & vbCrLf &
                '        "    SELECT MTDTchieu, MLTdiep AS KQKHAC," & vbCrLf &
                '        "           NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'MTa','DLieu') AS MTa," & vbCrLf &
                '        "           NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'MTLoi','LDo') AS MTLoi" & vbCrLf &
                '        "    FROM ALLTDIEP" & vbCrLf &
                '        "    WHERE MNGui = 'TCT' AND mltdiep NOT IN ('301','999')" & vbCrLf &
                '        ")" & vbCrLf &
                '        "SELECT DISTINCT b.Khoaphien AS KhoaphienTN, c.KQ999, d.KQ301, d.KHMSCTu, d.KHCTu, d.SCTu, d.DSLDo, e.KQKHAC," & vbCrLf &
                '        "CASE WHEN e.MTLoi IS NULL THEN e.MTa ELSE e.MTLoi END AS MTa" & vbCrLf &
                '        "FROM ALLTDIEP a" & vbCrLf &
                '        "INNER JOIN CTE_TDGuiGoc b ON a.MNgui = b.MNgui" & vbCrLf &
                '        "LEFT JOIN CTE_TD999 c ON b.MTDiep = c.MTDTchieu" & vbCrLf &
                '        "LEFT JOIN CTE_TD301 d ON b.MTDiep = d.MTDTchieu" & vbCrLf &
                '        "LEFT JOIN CTE_TDKHAC e ON b.MTDiep = e.MTDTchieu"

                Dim myQuery As String =
    "WITH ALLTDIEP AS (" & vbCrLf &
    "    SELECT a.Khoaphien, MTDiep, MLTDiep, MNGui, MTDTchieu, XMLThongdiep" & vbCrLf &
    "    FROM Logtruyennhan a INNER JOIN #tmp_Khoaphien b ON a.khoaphien=b.khoaphien" & vbCrLf &
    "    WHERE a.MST=@MST" & vbCrLf &
    ")," & vbCrLf &
    "CTE_TDGuiGoc AS (" & vbCrLf &
    "    SELECT Khoaphien, MTDiep, mltdiep, MNgui" & vbCrLf &
    "    FROM ALLTDIEP" & vbCrLf &
    "    WHERE MNGui = 'V0103930279' AND mltdiep <> '999'" & vbCrLf &
    ")," & vbCrLf &
    "CTE_TD999 AS (" & vbCrLf &
    "    SELECT MTDTchieu," & vbCrLf &
    "           CASE WHEN CHARINDEX('TTTNhan', evoicedb78.dbo.ufn_CLR_DecodeBase64(XMLThongdiep,'')) > 0 THEN 1 ELSE 0 END AS KQ999" & vbCrLf &
    "    FROM ALLTDIEP" & vbCrLf &
    "    WHERE MNGui = 'TCT' AND mltdiep = '999'" & vbCrLf &
    ")," & vbCrLf &
    "CTE_TD301 AS (" & vbCrLf &
    "    SELECT MTDTchieu, KQ301," & vbCrLf &
    "           CTu.value('(KHMSCTu)[1]','nvarchar(10)') AS KHMSCTu," & vbCrLf &
    "           CTu.value('(KHCTu)[1]','nvarchar(255)') AS KHCTu," & vbCrLf &
    "           CTu.value('(SCTu)[1]','nvarchar(255)') AS SCTu," & vbCrLf &
    "           CAST(CTu.query('(DSLDKTNhan)[1]') AS NVARCHAR(MAX)) AS DSLDo" & vbCrLf &
    "    FROM (" & vbCrLf &
    "         SELECT MTDTchieu, 1 AS KQ301," & vbCrLf &
    "                CAST(NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'DSCTu','DLTBao') AS XML) AS XmlData" & vbCrLf &
    "         FROM ALLTDIEP" & vbCrLf &
    "         WHERE MNGui = 'TCT' AND mltdiep='301'" & vbCrLf &
    "    ) AS T OUTER APPLY XmlData.nodes('/CTu') AS L(CTu)" & vbCrLf &
    ")," & vbCrLf &
    "CTE_TDKHAC AS (" & vbCrLf &
    "    SELECT MTDTchieu, MLTdiep AS KQKHAC," & vbCrLf &
    "           -- Trường hợp DLieu: lấy MTa / MLoi (trim và convert empty->NULL)" & vbCrLf &
    "           NULLIF(LTRIM(RTRIM(NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'MTa','DLieu'))), '')  AS MTa_DL," & vbCrLf &
    "           NULLIF(LTRIM(RTRIM(NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'MLoi','DLieu'))), '') AS MLoi_DL," & vbCrLf &
    "           -- Trường hợp LDo: lấy MTa / MTLoi" & vbCrLf &
    "           NULLIF(LTRIM(RTRIM(NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'MTa','LDo'))), '')    AS MTa_LDo," & vbCrLf &
    "           NULLIF(LTRIM(RTRIM(NTVAN.dbo.ufn_vDecodebase64xml(XMLThongdiep, 'MTLoi','LDo'))), '')  AS MLoi_LDo" & vbCrLf &
    "    FROM ALLTDIEP" & vbCrLf &
    "    WHERE MNGui = 'TCT' AND mltdiep NOT IN ('301','999')" & vbCrLf &
    ")" & vbCrLf &
    "SELECT DISTINCT b.Khoaphien AS KhoaphienTN, c.KQ999, d.KQ301, d.KHMSCTu, d.KHCTu, d.SCTu, d.DSLDo, e.KQKHAC," & vbCrLf &
    "       -- ưu tiên MTa trước, nếu không có mới lấy MLoi" & vbCrLf &
    "       COALESCE(e.MTa_DL, e.MTa_LDo, e.MLoi_DL, e.MLoi_LDo, N'') AS MTa" & vbCrLf &
    "FROM ALLTDIEP a" & vbCrLf &
    "INNER JOIN CTE_TDGuiGoc b ON a.MNgui = b.MNgui" & vbCrLf &
    "LEFT JOIN CTE_TD999 c ON b.MTDiep = c.MTDTchieu" & vbCrLf &
    "LEFT JOIN CTE_TD301 d ON b.MTDiep = d.MTDTchieu" & vbCrLf &
    "LEFT JOIN CTE_TDKHAC e ON b.MTDiep = e.MTDTchieu"


                Using myCommand As SqlCommand = New SqlCommand(myQuery, connection)
                    myCommand.Parameters.Add("@MST", SqlDbType.NVarChar).Value = MaDV

                    Using adapter As SqlDataAdapter = New SqlDataAdapter(myCommand)
                        adapter.Fill(dt)
                        Dim capnhatkq As Integer = CapnhatKQTruyennhanchungtutbss(dt)
                    End Using
                End Using

                Using cmdDrop As SqlCommand = New SqlCommand("DROP TABLE #tmp_Khoaphien;", connection)
                    cmdDrop.ExecuteNonQuery()
                End Using

                SqlConnection.ClearAllPools()
            End Using

        Catch ex As Exception
            Dim msg As String = ex.ToString()
        End Try

        XuLyNullTrongDataTable(dt)
        Return dt
    End Function


    Private Function CapnhatKQTruyennhanchungtutbss(ByVal KetquaTN As DataTable) As Integer
        Dim kqcn As Integer = 0
        If KetquaTN Is Nothing OrElse KetquaTN.Rows.Count = 0 Then Return -1

        Try
            Using connection As SqlConnection = New SqlConnection(connectionString)
                connection.Open()

                Dim createTempTable As String = "CREATE TABLE #tmp_KQTN ( " &
                   "KhoaphienTN NVARCHAR(250), " &
                   "KQ999 NVARCHAR(50), " &
                   "KQ301 NVARCHAR(50), " &
                   "KHMSCTu NVARCHAR(10), " &
                   "KHCTu NVARCHAR(255), " &
                   "SCTu NVARCHAR(255), " &
                   "DSLDo NVARCHAR(MAX), " &
                   "KQKHAC NVARCHAR(50), " &
                   "MTa NVARCHAR(MAX));"

                Using cmdCreate As SqlCommand = New SqlCommand(createTempTable, connection)
                    cmdCreate.ExecuteNonQuery()
                End Using

                Using bulkCopy As SqlBulkCopy = New SqlBulkCopy(connection)
                    bulkCopy.DestinationTableName = "#tmp_KQTN"
                    bulkCopy.WriteToServer(KetquaTN)
                End Using

                Dim update1 As String =
                   "UPDATE a " & vbCrLf &
                   "SET a.KQ999 = b.KQ999, " & vbCrLf &
                   "    a.KQ301 = b.KQ301, " & vbCrLf &
                   "    a.KQKHAC = b.KQKHAC, " & vbCrLf &
                   "    a.MTa = b.MTa, " & vbCrLf &
                   "    a.Ngaycapnhatcuoi = GETDATE() " & vbCrLf &
                   "FROM KQTNTBSSChungtu a " & vbCrLf &
                   "INNER JOIN #tmp_KQTN b ON a.KhoaphienTN =b.KhoaphienTN;"

                Using cmdUpdate1 As SqlCommand = New SqlCommand(update1, connection)
                    kqcn = cmdUpdate1.ExecuteNonQuery()
                End Using

                If kqcn > 0 Then
                    ' Cập nhật DSLDo nếu KQ213 là 10 hoặc 12
                    Dim update2 As String =
                       "UPDATE a " & vbCrLf &
                       "SET a.DSLDo = b.DSLDo, " & vbCrLf &
                       "    a.Ngaycapnhatcuoi = GETDATE() " & vbCrLf &
                       "FROM KQTNTBSSChungtu a " & vbCrLf &
                       "INNER JOIN #tmp_KQTN b " & vbCrLf &
                       "    ON a.KhoaphienTN = b.KhoaphienTN " & vbCrLf &
                       "    AND a.MSChungtu = b.KHMSCTu " & vbCrLf &
                       "    AND a.KHChungtu = b.KHCTu " & vbCrLf &
                       "    AND Convert(int, a.Sochungtu) = b.SCTu " & vbCrLf &
                       "WHERE b.KHMSCTu IS NOT NULL;"

                    Using cmdUpdate2 As SqlCommand = New SqlCommand(update2, connection)
                        Dim check As Integer = cmdUpdate2.ExecuteNonQuery()
                    End Using
                End If

                Using cmdDrop As SqlCommand = New SqlCommand("DROP TABLE #tmp_KQTN;", connection)
                    cmdDrop.ExecuteNonQuery()
                End Using

                SqlConnection.ClearAllPools()
            End Using

        Catch ex As Exception
            Dim msg As String = ex.ToString()
            kqcn = -2
        End Try

        Return kqcn
    End Function


    <WebMethod()>
    Public Function LayDanhSachTruyenNhanTBSS(matbss_ct As String) As String
        Dim response As New Dictionary(Of String, Object)()

        Try
            Dim chuoiketnoi As String = ConnectionStringtvan
            Dim khoaphien As String = LoadKhoaPhienTBSSCT(matbss_ct)

            If String.IsNullOrEmpty(khoaphien) Then
                response("status") = "success"
                response("message") = "Không tìm thấy khoá phiên"

            Else
                Dim dt As DataTable = LoadDataLogTruyennhan(khoaphien, chuoiketnoi)
                response("status") = "success"
                response("message") = "Lấy nhật ký truyền nhận thành công"
                response("data") = dt
            End If

        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    Private Function LoadKhoaPhienTBSSCT(matbss_ct As String) As String
        Dim res As String = String.Empty
        Try
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "Select KhoaphienTN from TBSSChungtu where MaTBSSCT = '" & matbss_ct & "' and Trangthaicuoi=1"
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    res = reader(0).ToString
                End While
            Else
                res = String.Empty
            End If
            conn.Close()
            conn.Dispose()
            comm.Dispose()
            Return res
        Catch ex As Exception
            Return res
        End Try
    End Function

    <WebMethod()>
    Public Function XemChiTietTBSS(matbss_ct As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Dim html As New Text.StringBuilder()

        Try

            Dim dt As New DataTable("dschitietchungtu")

            Dim sql As String = "SELECT * FROM TBSSChungtu a INNER JOIN TBSSChungtuchitiet b ON a.MaTBSSCT = b.MaTBSSCT WHERE a.MaTBSSCT = @MaTBSSCT"



            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@MaTBSSCT", matbss_ct)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            If Not dt.Columns.Contains("LCTDT_Text") Then
                dt.Columns.Add("LCTDT_Text", GetType(String))
            End If

            For Each row As DataRow In dt.Rows
                Select Case row("LCTDT").ToString()
                    Case "1" : row("LCTDT_Text") = "Chứng từ điện tử khấu trừ thuế TNCN theo Nghị định 70"
                    Case "2" : row("LCTDT_Text") = "Chứng từ điện tử khấu trừ thuế TMĐT, nền tảng số"
                    Case "3" : row("LCTDT_Text") = "Biên lai thu thuế, phí, lệ phí không in sẵn mệnh giá theo NĐ 70"
                    Case "4" : row("LCTDT_Text") = "Biên lai thu thuế, phí, lệ phí in sẵn mệnh giá theo NĐ 70"
                    Case "CTT50" : row("LCTDT_Text") = "Biên lai thu thuế của cơ quan thuế với cá nhân"
                    Case "6" : row("LCTDT_Text") = "Biên lai thu thuế, phí, lệ phí đặt in, tự in, điện tử theo TT 303"
                    Case "7" : row("LCTDT_Text") = "Chứng từ khấu trừ thuế TNCN theo NĐ 123/2020/NĐ-CP"
                End Select
            Next

            Dim tenNNT As String = "", maSoThue As String = "", tenCQT As String = "",
                maCQT As String = "", ngayLap As String = "", thangLap As String = "", namLap As String = "", nguoiKy As String = ""

            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                tenCQT = dr("TCQT")
                maCQT = dr("MCQT")
                tenNNT = dt.Rows(0)("TNNT").ToString.Replace("amp;", "")
                maSoThue = dt.Rows(0)("MST")
                nguoiKy = dt.Rows(0)("TNNT").ToString.Replace("amp;", "")
                'lbngayky.Text = dt.Rows(0)("SigningTime")
                Dim ngaytbao As String = dr("NTBao")
                ngayLap = ngaytbao.Split("/")(0)
                thangLap = ngaytbao.Split("/")(1)
                namLap = ngaytbao.Split("/")(2)
            End If


            html.AppendLine("<!DOCTYPE html>")
            html.AppendLine("<html lang='vi'>")
            html.AppendLine("<head>")
            html.AppendLine("  <meta charset='UTF-8'>")
            html.AppendLine("  <title>Thông báo chứng từ điện tử có sai sót</title>")
            html.AppendLine("</head>")
            html.AppendLine("<body style='font-family: ""Times New Roman"", serif; font-size:12pt;'>")

            ' --- Tiêu đề quốc gia ---
            html.AppendLine("<div style='margin:auto; text-align:center; font-weight:bold;'>")
            html.AppendLine("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM<br />Độc lập - Tự do - Hạnh phúc</div>")
            html.AppendLine("<hr />")

            ' --- Tiêu đề thông báo ---
            html.AppendLine("<div style='text-align:center; font-weight:bold; font-size:15pt; padding-top:10px; padding-bottom:10px;'>THÔNG BÁO CHỨNG TỪ ĐIỆN TỬ CÓ SAI SÓT</div>")

            ' --- Thông tin cơ quan thuế và người nộp thuế ---
            html.AppendLine("<div id='divcqtduyet'>")
            html.AppendLine("<table style='width:100%; border-collapse:collapse; margin-top:10px;'>")
            html.AppendLine("<tr>")
            html.AppendLine("<td style='width:20%; padding:5px;'>Kính gửi cơ quan thuế:</td>")
            html.AppendLine("<td style='width:80%; padding:5px;'>" & tenCQT & " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Mã cơ quan thuế: " & maCQT & "</td>")
            html.AppendLine("</tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td style='width:20%; padding:5px;'>Tên người nộp thuế</td>")
            html.AppendLine("<td style='width:80%; padding:5px;'>" & tenNNT & "</td>")
            html.AppendLine("</tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td style='width:20%; padding:5px;'>Mã số thuế người nộp thuế</td>")
            html.AppendLine("<td style='width:80%; padding:5px;'>" & maSoThue & "</td>")
            html.AppendLine("</tr>")
            html.AppendLine("</table>")
            html.AppendLine("</div>")


            ' --- Thông báo nội dung ---
            html.AppendLine("<div style='padding:10px; font-size:11pt;'><b>Người nộp thuế thông báo về việc chứng đơn điện tử có sai sót như sau:</b></div>")

            ' --- Bảng chi tiết ---
            html.AppendLine("<table style='width:100%; border-collapse:collapse; margin-top:10px; font-size:11pt;' border='1'>")
            html.AppendLine("<tr style='background-color:whitesmoke; font-weight:bold;'>")
            html.AppendLine("<td style='width:5%; padding:5px;'>STT</td>")
            html.AppendLine("<td style='width:10%; padding:5px;'>Ký hiệu mẫu số CT</td>")
            html.AppendLine("<td style='width:10%; padding:5px;'>Số chứng từ</td>")
            html.AppendLine("<td style='width:10%; padding:5px;'>Ngày lập</td>")
            html.AppendLine("<td style='width:20%; padding:5px;'>Loại chứng từ áp dụng</td>")
            html.AppendLine("<td style='width:30%; padding:5px;'>Lý do</td>")
            html.AppendLine("</tr>")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Dim ngayLapText As String = ""
                    If Not IsDBNull(row("NLap")) Then
                        ngayLapText = Convert.ToDateTime(row("NLap")).ToString("dd/MM/yyyy")
                    End If

                    html.AppendLine("<tr>")
                    html.AppendLine("<td style='padding:5px;'>" & row("STT") & "</td>")
                    html.AppendLine("<td style='padding:5px;'>" & row("KHMSCTu") & "</td>")
                    html.AppendLine("<td style='padding:5px;'>" & row("SCTu") & "</td>")
                    html.AppendLine("<td style='padding:5px;'>" & ngayLapText & "</td>")
                    html.AppendLine("<td style='padding:5px;'>" & row("LCTDT_Text") & "</td>")
                    html.AppendLine("<td style='padding:5px;'>" & row("LDo") & "</td>")
                    html.AppendLine("</tr>")
                Next
            Else
                html.AppendLine("<tr><td colspan='6' style='text-align:center; padding:5px;'>Không có dữ liệu</td></tr>")
            End If

            html.AppendLine("</table>")

            ' --- Chữ ký và ngày lập ---
            html.AppendLine("<div style='padding:20px;'>")
            html.AppendLine("<table style='width:100%; border-collapse:collapse;'>")
            html.AppendLine("<tr>")
            html.AppendLine("<td style='width:50%'></td>")
            html.AppendLine("<td style='width:50%; text-align:left;'>")
            html.AppendLine("Thông báo lập ngày <span>" & ngayLap & "</span> tháng <span>" & thangLap & "</span> năm <span>" & namLap & "</span><br />")
            html.AppendLine("<div style='padding-left:100px; font-weight:bold;'>NGƯỜI NỘP THUẾ</div>")
            html.AppendLine("<div style='width:70%; border:1px solid red; padding:10px; margin-top:10px; margin-bottom:50px; text-align:left;'>")
            html.AppendLine("<span style='color:red; font-family:Georgia, ""Times New Roman"", Times, serif; font-weight:bold;'>Signature valid<br />Được ký bởi: " & nguoiKy & "</span>")
            html.AppendLine("</div></td></tr>")
            html.AppendLine("</table></div>")

            html.AppendLine("</body>")
            html.AppendLine("</html>")


            'html = html.Replace("<div id='divcqtduyet'>", "<div id='divcqtduyet'> style=""")
            html = html.Replace("<div id='divcqtduyet'>",
        "<div id='divcqtduyet' style=""background-image:url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAasAAACxCAIAAAD1SQVzAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAA3ZSURBVHhe7Z1LbuPIEkV7IW/4VqOR1+KB1lJALcWopdg76IGhgVGAgGqSQUnJYEQyk6JTquA5uECjy0GKzM9hivr98wcAYK/88+///k8IIXsLBiSE7DcYkBCy32BAQsh+gwEJIfsNBiSE7DcYkBCy32BAQsh+gwEJIfsNBiSE7DcYkBCy32BAQsh+gwEJIfsNBiSE7DcYkBCy32BAQsh+U21A2QAA4JlR4vIixRgQAEKhxOVFijEgAIRCicuLFGNAAAiFEpcXKcaAABAKJS4vUowBASAUSlxepBgDAkAolLi8SDEGBIBQKHF5kWIMCAChUOLyIsUYEABCocTlRYoxIACEQonLixRjQAAIhRKXFynGgAAQCiUuL1KMAQEgFEpcXqQYAwJAKJS4vEgxBgSAUChxeZFiDAgAoVDi8iLFGBAAQqHE5UWKMSAAhEKJy4sUY0AACIUSlxcpxoAAEAolLi9SjAEBIBRKXF6kGAMCQCiUuLxIMQYEgFAocXmRYgwIAKFQ4vIixRgQAEKhxOVFijEgAIRCicuLFGNAAAiFEpcXKcaAABAKJS4vUowBASAUSlxepBgDAkAolLi8SDEGBIBQKHF5kWIMCAChUOLyIsUYEABCocTlRYoxIACEQonLixRjQAAIhRKXFynGgAAQCiUuL1KMAQEgFEpcXqQYAwJAKJS4vEgxBgSAUChxeZFiDAgAoVDi8iLFGBAAQqHE5UWKMSAAhEKJy4sUY0AACIUSlxcpxoCwnt+vkxFyehv/HeCBpGMyEynGgLCW95+fepAcf49/A3gY0zHpRoox4JWPr9eX8WQPx6+3j/GfhX62v3y9j/8HfXMdJsND8vlj2m4AzVFj0osUY0Dh18mYzy+nH7/O/V8/hqd7GDDh7ThrLgnLQHgwszFpR4oxYI+6n2Xn8HOwIXSkC8CX09vH+cdl+cwyEB7NdSjmI8UYsOPXaXay8zCxb1wXgIfj78u6+Px2vNwWZBkIj2QcnEuRYgx4uaP/Ojzhfe+e8L7MbvCzAEy5XDDmbfI+/omrxWO4XYR2PWhvLZCNFGNAWdGoe3wfv38cPy9P9D5FjjAgT3h9x8kTZJaBrenW4DJc/z28XIfuPiU4nvtSpBgDAvz19Jelw/WFu5Hz28/+7Q3JnYqdoMTlRYoxIACEQonLixRjQJP+fhafcAD4G1Hi8iLFz2bAopdlb+lvebycXn9+vX1seL9jfG/HxvdQ6k6tO6/Pw/Gre15jPoUxPo/h5/VXt0XRO37GyF1R+z3P9am8J1h1al2GhurGwG/1JnabmpOaDoD0HT+z9C1W3MJOg/gnPtx1/a7uyJ5XfYbB9lj0ITmR4mddA76bb1FeSDcTtrjlcR1q3/IW6POKU+tU6BxJ8h4UHecFnP7FblV5Sf/OvrEqZVVfJFn5qsj5rVKFQz47FRb0Wmbn+Re+uu5LTTQrzjRvJ7Lll9Rm+5+dztbdgQH1H7zIBu2wL4mX/nv/6MZK/4qtLlgYwcukn3b4pu60Tu366mo/B6xZ5L38ao/g3ALWXk1k38JiLmCNK8T5ffIy+pCVBuwxP3lyO7V+DHRttXIMmDsvOtpLAzrDw1kMll9QL63tDr8tuyMdP4Nw02FgPtB0D8MbyG5/xYDbUdD6PdZ87haD419rUXtbvZ88xqkpAZles29NmjM5NxBtA2bve5ZOuSvJ8d/RhuZVcC736brJLVMU7txCHOe3mNlc+WtMwnhgmXbbsjsuf+qeAYz/klA4B/tBODYmBtyO4ta3p/SqiTebFaWjtg7j1GYPVHxS1Qa0FylZA5oHs7CouUytVR0hVEjKGi15nd1hwOHUCo2TpqgplvTasWV3yL8747x8Dl72jwG3o6L17xnNKYYa6ndSQIkB7VlkjNRnNeBlqzYGdIoz7fCdBnRarOCCKke14JEtu2M4F6+PauagFGPA7ahq/RWzeo75iLU7KaHIgKVT+okM+Os0NUh2dhVQKam68/peAzr9stQawykUukxlZXd0/+h7uW4O9vvHgNtR1/r3DGhhGB+zPXTZvlPvMOD8jJ7HgN0Bq8PrT6GdAe2m8OrvGDBFBrQbLbsMHA5peZ24aXd05+J3feUc7NofA25HZes7A654BTdsfjhaD7o41msJasDuSPThdRs2NKA9ZpxTq975jTIDOl3jjmFp5JLmatYdtXPwKZgdsB0pjmRAe2KXXpSGwdpNFXMlWHBZrqLMgIVqKyxL+CYD9rstM0gp9ZKqGAMNDGi3m3M8lxFYQLPuwIBJZIOGVLe+Ka+ai+pQaU6MjXu9yIDm6Rgz5EkMKO32aAPaY8DcpIUBveE033YYEqWt16w7MGAS2aAh9a1vuqBkpA7D5eqgekHUUmJAc5Rbp/8UBpQfErh/yk1ZIanyMdDGgGXLUhF38Rhr1h0YMIls0JD61revt8sDaxguyQAyZ9GWc3vZgKULwI5HG/Ccfk4LA85ZHJZSkO2yKc26AwMmkQ2+F3v45pP0x0oDDt08GX+L19g7yRrQ/lSc86HdjjWNZqTegFae1IDW2TUzoLcMvDyWvgAv06w7MGAS2eB7eYQBzScg5hKs5iqdxxxYfvLf+IABU8rHQEMDej3eH9VwGPO7wFkwYI7ZAduRYp4FD4NpPlDsXW3V90sGHL716/PV/3asFNOAu30WXD4GVuz8Qr0BnWvq4Tg0Xe24atYdGDCJbNCQJgYc9GEWmJqovFZ7GKe2fs+PNuDApeDhBrRXxJat2howc9mr7/pm3YEBk8gGDalv/fLRPyIDxdmnubdtZng8A16EggF97GXgGqE06w4MmEQ2aMhWBvQ3MefAUrKmKCSiAWW3jzag82TT2qS5AbcTSrPuwIBJZIOGVLe+Pfp9F5giWE5WLmXENGDf/o82oN2nZmvcYcDhUZ7dgFt0BwZMIhs0pLb1zWGRmdjD/hd0Zh7DiqGvMHYbwIC9U+wp153vqmlTLamaMXCvAVecUe2Q9mjWHVsdcFNmB2xHiiMZsK5eFozZad9jymK9rUaMQ41gwO68zCnXHWGZWTS1kqqqX29AaY0VIthKKM26AwMmkQ0aUtn6lgj8WT2MoZJxYM6TewdBVAPa9A+3sNZ2qJSUeRvEPy9rgBWpYdiwVCIpWwmlWXdsdcBNmR2wHSkOY0BrTGTG6GCNMumYo2158ZjFOLWwBqxo6hl1BrQGTK4dzJMqmN5yVGuc/gQGrOsODJhENmhIResbF//c96DJACq9ZtqKWbMEuLIbA14U1sCAxkkt9ZG1Zlw+KXmgVZfARxuwujswYBLZoCGlrX+eKyD/NZBjfXlHmkdyzzLQ2uGaNcVAtQHN+ZM/HbMFFqZc+lvGK9uq2IDWJbDkElXfEXJIK69/pUN6iWbdsdUBN2V2wHak+IkNaI7+rvNul6/+52JPBz30l34r9tqpBdfMEVsZ64eCeWprDWje/MpOUft0cusCpy+6R5l9YHn4DV/9O87lTT3FlHu3t9uXRHyc337ePgR2KTgV/Db5iPUQ7pdQjO2wab+vaJxm3bHVAbdldsB2pPhZDZh+trE4xg/sT+lmy6RHD8duLi1+8NZYY15jjLkFJh/bnOazfN5eSC/sKs6VYPr71pM4Xz9j/hpvXdYoQ3dWUV76s67tEesSon47vPeItFv+WyoyZIZ01T6bdUf2gL8mP6z+XOijdSLFz2ZAc9WdSf8NAif9Q/c25s2vMeaKybvSzlJ2V6Xi1JZ3WHxsfYbVZe70deQi761861P1nLHq1Pp0Y+B4+lF9KZpQdsVdenphYK/QM3GfCjTrjqo5+IzrwdlB2pHiZ74PCNCUYaF37K6p05HfG7b/kp6xCp6dafe5kWIMCAChUOLyIsUYECAI5+srHv19uvEfd0hqrUykGAMCRGB2w/HZX7H9Pqbt4EaKMSBAAKyXL9a+v+pvR7eDEynGgAB/P+YL6Cvftv3Xo9vBiRRjQIAAsAa8odvBiRRjQIAIcB/wyrQd3EgxBgQIAq8FC6m1MpFiDAgAoVDi8iLFGBAAQqHE5UWKMSAAhEKJy4sUY0AACIUSlxcpxoAAEAolLi9SjAEBIBRKXF6kGAMCQCiUuLxIMQYEgFAocXmRYgwIAKFQ4vIixRgQAEKhxOVFijEgAIRCicuLFGNAAAiFEpcXKcaAABAKJS4vUowBASAUSlxepBgDAkAolLi8SDEGBIBQKHF5kWIMCAChUOLyIsUYEABCocTlRYoxIACEQonLixRjQAAIhRKXFynGgAAQCiUuL1KMAQEgFEpcXqQYAwJAKJS4vEgxBgSAUChxeZFiDAgAoVDi8iLFGBAAQqHE5UWKMSAAhEKJy4sUY0AACIUSlxcpxoAAEAolLi9SjAEBIBRKXF6kGAMCQCiUuLxIMQYEgFAocXmRYgwIAKFQ4vIixRgQAEKhxOVFijEgAIRCicuLFGNAAAiFEpcXKcaAABAKJS4vUowBASAUSlxepBgDAkAolLi8SDEGBIBQKHF5kWIMCAChUOLyIsUYEABCocTlRYoxIACEQonLixRjQAAIhRKXFynGgAAQCiUuL1KMAQEgFEpcXqQYAwJAKJS4vEgxBgSAUChxeZFiDAgAoVDi8iLFFQYkhJAwwYCEkP0GAxJC9hsMSAjZbzAgIWS/wYCEkP0GAxJC9hsMSAjZbzAgIWS/wYCEkP0GAxJC9hsMSAjZbzAgIWS/wYCEkP0GAxJC9hsMSAjZb0YDyn8AAHYIBgSAvfLnz3/DxqUrpPNJeAAAAABJRU5ErkJggg=='); z-index:0; background-size:20%; background-repeat:no-repeat; background-position:bottom;"">")

            response("status") = "success"
            response("data") = html.ToString()


        Catch ex As Exception
            response("status") = "error"
            response("message") = "Lỗi hệ thống: " & ex.Message
        End Try

        Return JsonConvert.SerializeObject(response)
    End Function


    <WebMethod>
    Public Function GetMSTFromCertBase64(certBase64 As String) As String
        Dim certBytes As Byte() = Convert.FromBase64String(certBase64)
        Dim cert As New X509Certificate2(certBytes)

        ' Decode Subject dùng UTF8
        Dim decoded As String = cert.SubjectName.Decode(X500DistinguishedNameFlags.UseUTF8Encoding)

        Dim mst As String = String.Empty
        Dim cccd As String = String.Empty

        ' Chia thành từng phần
        For Each part As String In decoded.Split(","c)
            part = part.Trim()

            ' Kiểm tra MST theo OID, bỏ prefix "OID." nếu có
            If part.StartsWith("OID.0.9.2342.19200300.100.1.1=", StringComparison.OrdinalIgnoreCase) Then
                Dim val As String = part.Substring("OID.0.9.2342.19200300.100.1.1=".Length).Trim()

                ' Nếu có MST thì lưu lại
                If val.StartsWith("MST:", StringComparison.OrdinalIgnoreCase) Then
                    mst = val.Substring(4).Trim()

                    ' Nếu có CCCD thì lưu lại
                ElseIf val.StartsWith("CCCD:", StringComparison.OrdinalIgnoreCase) Then
                    cccd = val.Substring(5).Trim()
                End If


            End If
        Next

        ' Ưu tiên MST, nếu không có thì lấy CCCD
        If Not String.IsNullOrEmpty(mst) Then
            Return mst
        ElseIf Not String.IsNullOrEmpty(cccd) Then
            Return cccd
        End If

        Return String.Empty
    End Function



    ' hàm view mau v1
    <WebMethod>
    Public Function loadcontenthd(mst As String, base64xml As String, trangthai As String, mauso As String, idhd As String, madv As String) As String
        Dim res As String = String.Empty
        Dim content As String = String.Empty
        If Not String.IsNullOrEmpty(base64xml) Then
            ' Dim xmlfile As String = GetFileXML(idhd)
            Dim result = Encoding.UTF8.GetString(Convert.FromBase64String(base64xml))
            Dim qrcode = result.Substring(result.IndexOf("<DLQRCode>") + 10, result.IndexOf("</DLQRCode>") - result.IndexOf("<DLQRCode>") - 10)
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim b64qrcode As String = String.Format("https://api.qrserver.com/v1/create-qr-code/?size=100x100&data=" + qrcode)
            Dim imageData As Byte() = Nothing

            Using wc = New System.Net.WebClient()
                imageData = wc.DownloadData(b64qrcode)
            End Using
            Dim byteqrcode = Convert.ToBase64String(imageData, 0, imageData.Length)
            b64qrcode = "data:image/jpg;base64," & byteqrcode

            Dim xml As Byte() = Convert.FromBase64String(base64xml)
            Dim xsltPath As String = String.Empty
            Dim style As String = "width:900px;margin:auto; border:2px solid black; padding-top:20px;"

            '====Load thong tin hoa don====
            Dim tenhd As String = String.Empty 'LoadData(idhd, "TenHD", chuoiketnoi)
            Dim phithuequan As Integer = 0 ' LoadData(idhd, "HDDCKPTQuan", chuoiketnoi)
            Dim phanbiethd As Integer = 0
            Dim loaihd As Integer = 0

            Dim loaidc As Integer = 0   ' GetLoaiDC(idhd, chuoiketnoi)
            Dim mskhgoc As String = String.Empty
            Dim khhdongoc As String = String.Empty
            Dim sohoadongoc As String = String.Empty
            Dim Ghichugiamthue As String = String.Empty
            Dim ngayhdgoc As DateTime
            Dim is_ky_so_succes As Integer = 0
            'mskhgoc = LoadData(idhd, "KHMSHDon", chuoiketnoi)
            'khhdongoc = LoadData(idhd, "KHHDon", chuoiketnoi)
            'sohoadongoc = LoadData(idhd, "Sohoadon", chuoiketnoi)
            'phanbiethd = LoadData(idhd, "PhanbietHD", chuoiketnoi)
            'NguonKT = LoadData(idhd, "NguonKT", chuoiketnoi)
            Dim checkgiamthue As Integer = 0
            Dim trangthaicuoihd As Integer = 0
            Dim conn As New SqlConnection(connectionString)
            conn.Open()
            Dim comm As New SqlCommand()
            comm.Connection = conn
            comm.CommandText = "select ten_hoa_don, hoa_don_dang_ky_phat_hanh_mau_so, hoa_don_dang_ky_phat_hanh_ky_hieu, ma_so_hoa_don, hoa_don_trang_thai_id, hoa_don_hinh_thuc_id,is_deleted, hoa_don_hinh_thuc_code, ngay_hoa_don_goc, giam_thue_ghi_chu, is_ky_so_succes,hoa_don_dang_ky_phat_hanh_mau_so_goc, hoa_don_dang_ky_phat_hanh_ky_hieu_goc, ma_so_hoa_don_goc from hoa_don where donvi_ma_dv='" + madv + "' and id='" + idhd + "'"
            Dim reader As SqlDataReader = comm.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    tenhd = reader("ten_hoa_don")
                    If reader("ma_so_hoa_don_goc") IsNot DBNull.Value Then
                        mskhgoc = reader("hoa_don_dang_ky_phat_hanh_mau_so_goc")
                        khhdongoc = reader("hoa_don_dang_ky_phat_hanh_ky_hieu_goc")
                        sohoadongoc = reader("ma_so_hoa_don_goc")
                        If reader("ngay_hoa_don_goc") IsNot DBNull.Value Then
                            ngayhdgoc = Convert.ToDateTime(reader("ngay_hoa_don_goc"))
                        End If
                    End If

                    trangthai = reader("hoa_don_trang_thai_id")
                    phanbiethd = Convert.ToInt32(reader("hoa_don_hinh_thuc_id"))
                    If reader("is_ky_so_succes") IsNot DBNull.Value Then
                        is_ky_so_succes = Convert.ToInt32(reader("is_ky_so_succes"))
                    End If
                    trangthaicuoihd = Convert.ToInt32(reader("is_deleted"))

                    If reader("giam_thue_ghi_chu") IsNot DBNull.Value Then
                        If Not String.IsNullOrEmpty(reader("giam_thue_ghi_chu")) Then
                            checkgiamthue = 1
                            Ghichugiamthue = reader("giam_thue_ghi_chu").ToString
                        End If
                    End If
                    phithuequan = 0


                End While

            End If
            reader.Close()
            conn.Close()
            comm.Dispose()
            conn.Dispose()
            SqlConnection.ClearAllPools()
            '====================

            If phithuequan = 1 Then
                tenhd = "Hóa đơn bán hàng phi thuế quan"
            End If
            Dim oldfile As String = String.Empty
            oldfile = Server.MapPath("~/") & GetXSLT_HD(mauso, mst)

            Dim destfile As String = Server.MapPath("~/temp/" & Path.GetRandomFileName & ".xslt")
            Dim logopath As String = GetLogoPath(mst, mauso)

            Dim templogofilename As String = Guid.NewGuid().ToString & "_templogo.jpg"
            Dim b64logo As String = String.Empty
            If Not String.IsNullOrEmpty(logopath) Then

                Dim fn As String = Server.MapPath(logopath.Replace("https://ca2einv.nacencomm.vn/", ""))
                If File.Exists(fn) Then
                    File.Copy(fn, Server.MapPath("~/temp/" & templogofilename), True)
                    b64logo = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("~/temp/" & templogofilename)))
                    b64logo = "data:image/jpg;base64," & b64logo

                Else
                    b64logo = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("Upload/blank.png")))
                    b64logo = "data:image/jpg;base64," & b64logo

                End If
            Else
                b64logo = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("Upload/blank.png")))
                b64logo = "data:image/jpg;base64," & b64logo
            End If

            Dim bgtype As Integer = GetBGType(mauso, mst)
            Dim bgpath As String = GetBGImage(mst, mauso)
            Dim checkxsltborder As String = Left(Path.GetFileNameWithoutExtension(oldfile), 1)

            Dim bgstyle As String
            Dim table_style As String

            If checkxsltborder = "B" Then
                If bgtype = 0 Then
                    bgstyle = "width:900px;margin:auto; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & bgpath & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = String.Empty
                Else
                    bgstyle = "width:900px;margin:auto; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & "" & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = "background-image: url('" & bgpath & "'); background-size:cover; background-position: center;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                End If
            Else
                If bgtype = 0 Then
                    bgstyle = "width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & bgpath & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = String.Empty
                Else
                    bgstyle = "width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;"
                    bgstyle = bgstyle & "background-image: url('" & "" & "'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                    table_style = "background-image: url('" & bgpath & "'); background-size:cover; background-position: center;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat"
                End If
            End If


            Dim paramsubtitle, paramsubtitlecontent, paramSubtitleDiv, paramSubtitleContentDiv As String
            paramsubtitle = "none"
            paramSubtitleDiv = "normal"
            paramsubtitlecontent = String.Empty
            paramSubtitleContentDiv = "&#160;"
            Dim styledisabled, noidungdisabled As String
            noidungdisabled = String.Empty
            styledisabled = "position:absolute;z-index:0;width:300px;height:100px;border:3px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;"

            Select Case phanbiethd
                Case 2
                    'hóa đơn thay thế
                    paramsubtitle = "normal"
                    paramsubtitlecontent = "(Hóa đơn thay thế)"
                    paramSubtitleDiv = "normal"
                    paramSubtitleContentDiv = "Hóa đơn thay thế cho hóa đơn số <b>" & sohoadongoc & "</b>, mẫu số <b>" & mskhgoc & "</b>, ký hiệu <b>" & khhdongoc & "</b>, ngày <b>" & ngayhdgoc.Day & "</b> tháng <b>" & ngayhdgoc.Month & "</b> năm <b>" & ngayhdgoc.Year & "</b>"
                    styledisabled = "position:absolute;z-index:0;width:auto;height:100px;border:5px solid red;background:transparent;display:none;top:65%;left:25%;color:red;font-size:50pt;text-align:center;padding-top:10px;"
                    noidungdisabled = "&#160;"
                Case 3
                    'Hóa đơn điều chỉnh
                    paramsubtitle = "normal"
                    paramsubtitlecontent = "(Hóa đơn điều chỉnh)"
                    paramSubtitleDiv = "normal"
                    paramSubtitleContentDiv = "Hóa đơn điều chỉnh cho hóa đơn số <b>" & sohoadongoc & "</b>, mẫu số <b>" & mskhgoc & "</b>, ký hiệu <b>" & khhdongoc & "</b>, ngày <b>" & ngayhdgoc.Day & "</b> tháng <b>" & ngayhdgoc.Month & "</b> năm <b>" & ngayhdgoc.Year & "</b>"
                    styledisabled = "position:absolute;z-index:0;width:auto;height:100px;border:5px solid red;background:transparent;display:none;top:65%;left:25%;color:red;font-size:50pt;text-align:center;padding-top:10px;"
                    noidungdisabled = "&#160;"
                Case 4
                    paramsubtitle = "none"
                    paramSubtitleDiv = "normal"
                    paramsubtitlecontent = String.Empty
                    paramSubtitleContentDiv = "&#160;"
                    'Hóa đơn bị điều chỉnh
                    styledisabled = "position:absolute;z-index:0;width:auto;height:70px;border:4px solid red;background:transparent;display:block;top:45%;left:50%;color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;"
                    noidungdisabled = "HÓA ĐƠN BỊ ĐIỀU CHỈNH"
                Case 6
                    paramsubtitle = "none"
                    paramSubtitleDiv = "normal"
                    paramsubtitlecontent = String.Empty
                    paramSubtitleContentDiv = "&#160;"
                    'Hóa đơn bị thay thế
                    styledisabled = "position:absolute;z-index:0;width:auto;height:70px;border:4px solid red;background:transparent;display:block;top:45%;left:50%;color:red;font-size:25pt;font-weight:bold;text-align:center;padding-top:10px;"
                    noidungdisabled = "HÓA ĐƠN BỊ THAY THẾ"
                Case Else
                    paramsubtitle = "none"
                    paramSubtitleDiv = "normal"
                    paramsubtitlecontent = String.Empty
                    paramSubtitleContentDiv = "&#160;"
                    styledisabled = "position:absolute;z-index:0;width:auto;height:100px;border:5px solid red;background:transparent;display:none;top:65%;left:25%;color:red;font-size:50pt;text-align:center;padding-top:10px;"
                    noidungdisabled = "&#160;"
            End Select

            Dim fileReader As String = String.Empty
            Dim readerfile As String = String.Empty

            Dim chuoitien As String = String.Empty
            Dim tiengiam As String = String.Empty
            If Not String.IsNullOrEmpty(Ghichugiamthue) Then
                chuoitien = Ghichugiamthue
            End If
            If trangthai = "1" Then
                'hóa đơn nháp
                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", "display:none").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramqrcode", b64qrcode).Replace("{paramtiengiam}", chuoitien)
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)
            ElseIf trangthai = "2" Then
                'hóa đơn đã phát hành thành công
                fileReader = File.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", "display:normal").Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramqrcode", b64qrcode).Replace("{paramtiengiam}", chuoitien)
                File.WriteAllText(destfile, fileReader)
            ElseIf trangthai = "3" Then
                'Hóa đơn đã hủy
                Dim paramsign As String = String.Empty
                If is_ky_so_succes = 1 Then
                    paramsign = "display:normal"
                Else
                    paramsign = "display:none"
                End If
                styledisabled = "position:absolute;z-index:0;width:300px;height:70px;border:5px solid red;background:transparent;display:block;top:45%;left:50%;color:red;font-size:25pt; font-weight:bold;text-align:center;padding-top:10px;font-weight:bold"
                noidungdisabled = "ĐÃ HỦY"
                fileReader = My.Computer.FileSystem.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:normal").Replace("paramSign", paramsign).Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:normal").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramqrcode", b64qrcode).Replace("{paramtiengiam}", chuoitien)
                My.Computer.FileSystem.WriteAllText(destfile, fileReader, False)

            Else
                'Các trạng thái khác
                Dim paramsign As String = String.Empty
                If is_ky_so_succes = 1 Then
                    paramsign = "display:normal"
                Else
                    paramsign = "display:none"
                End If
                fileReader = File.ReadAllText(oldfile).Replace("viewstyle", bgstyle).Replace("paramLogo", b64logo).Replace("paramChuyendoi", "display:none").Replace("paramSign", paramsign).Replace("paramMau", "display:none").Replace("paramNguoiCD", "width:100%;text-align:center;display:none").Replace("paramTableBG", table_style).Replace("paramBuyerSign", "none").Replace("param1_1", paramsubtitle).Replace("param1", paramsubtitlecontent).Replace("param2_2", paramSubtitleDiv).Replace("param2", paramSubtitleContentDiv).Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled).Replace("paramTracuu", divtracuu).Replace("paramSotrangdisplay", "none").Replace("param3", "Trang ").Replace("paramqrcode", b64qrcode).Replace("{paramtiengiam}", chuoitien)
                File.WriteAllText(destfile, fileReader)

            End If
            'Dim doc As XDocument = XDocument.Load(destfile)

            '' Tìm tất cả các thẻ <xsl:param> có name bắt đầu bằng số
            'Dim invalidParams = From e In doc.Descendants()
            '                    Where e.Name.LocalName = "param" AndAlso
            '                  e.Attribute("name") IsNot Nothing AndAlso
            '                  e.Attribute("name").Value.Length > 0 AndAlso
            '                  Char.IsDigit(e.Attribute("name").Value(0))
            '                    Select e

            '' Xóa các thẻ không hợp lệ
            'For Each p In invalidParams.ToList()
            '    p.Remove()
            'Next

            ' Lưu lại file
            'Doc.Save(destfile)

            xsltPath = destfile
            content = GetHtml(xsltPath, xml, mst, 1, idhd, "0")
            content = content.Replace("&amp;", "&")

            res = content
            'File.Delete(destfile)
        End If
        Return res
    End Function

    Private Function CheckHDGocBiThayTheDC(madv As String, mskhhd As String, khhdon As String, sohoadon As String, chuoiketnoi As String) As Integer
        Dim phanbietHD As Integer = 0
        Dim conn As New SqlConnection(chuoiketnoi)
        conn.Open()
        Dim comm As New SqlCommand()
        comm.Connection = conn
        comm.CommandText = "Select PhanbietHD from hoadon68 where MaChiNhanh= '" & madv & "' and KHMSHDBTThe='" & mskhhd & "' and KHHDBTThe='" & khhdon & "' and SoHDBTThe='" & sohoadon & "' and TrangthaicuoiHD=1 and TinhtrangHD <> 6"
        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                phanbietHD = Convert.ToInt32(reader(0).ToString)
            End While

        Else
            phanbietHD = 0
        End If
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        Return phanbietHD
    End Function
    Public Function GetHtml(xsltPath As String, xml As Byte(), madv As String, type As String, idhd As String, paramLienValue As String) As String
        Try
            ' 1) Load XSLT
            Dim transform As New XslCompiledTransform()
            transform.Load(xsltPath, New XsltSettings(True, True), New XmlUrlResolver())

            ' 2) Load XML đầu vào
            Using stream As New MemoryStream(xml)
                Dim doc As New XPathDocument(stream)
                Dim writer As New StringWriter()

                ' 3) Truyền param vào XSLT
                Dim args As New XsltArgumentList()
                args.AddParam("madv", "", madv)
                args.AddParam("type", "", type)
                args.AddParam("idhd", "", idhd)
                args.AddParam("paramlien", "", paramLienValue)   ' <===== THÊM VÀO ĐÂY

                ' 4) Transform
                transform.Transform(doc, args, writer)
                Return writer.ToString()
            End Using

        Catch ex As Exception
            Return ex.Message
            Return ex.Message
        End Try
    End Function

    Private Function GetSoHDBosung(idhd As String) As String
        Dim res As String = String.Empty
        Dim conn As New SqlConnection(connectionString)
        conn.Open()
        Dim comm As New SqlCommand
        comm.Connection = conn
        comm.CommandText = "select hoa_don_dang_ky_phat_hanh_mau_so_goc, hoa_don_dang_ky_phat_hanh_ky_hieu_goc, ma_so_hoa_don_goc from hoa_don where id='" + idhd + "'"
        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                If Not reader(0) Is DBNull.Value Then
                    res = reader("hoa_don_dang_ky_phat_hanh_mau_so_goc").ToString & "|" & reader("hoa_don_dang_ky_phat_hanh_ky_hieu_goc").ToString & "|" & reader("ma_so_hoa_don_goc").ToString
                Else
                    res = String.Empty
                End If
            End While
            reader.Close()
        Else
            res = String.Empty
        End If
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return res
    End Function

    Private Function GetXSLT_HD(mauso As String, madv As String) As String
        Dim res As String = String.Empty
        Dim conn As New SqlConnection
        Dim comm As New SqlCommand
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn
        comm.CommandText = "select xslt_path from mau_hoa_don where loai_hoa_don_ct_template_id='" + mauso + "' and donvi_ma_dv='" + madv + "'"
        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                res = reader(0)
            End While
            reader.Close()

        Else
            res = String.Empty
        End If
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return res
    End Function

    Private Function GetBGType(mauso As String, madv As String) As Integer
        Dim res As Integer = 0
        Dim conn As New SqlConnection
        Dim comm As New SqlCommand
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn
        comm.CommandText = "select  is_show_wattermark_inner_table from mau_hoa_don where loai_hoa_don_ct_template_id='" + mauso + "' and donvi_ma_dv='" + madv + "'"
        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                If reader(0) IsNot DBNull.Value Then
                    res = Convert.ToInt32(reader(0))
                Else
                    res = 0
                End If
            End While
            reader.Close()
        Else
            res = 0
        End If
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return res
    End Function

    Private Function GetBGImage(madv As String, mauso As String) As String
        Dim res As String = String.Empty
        Dim conn As New SqlConnection
        Dim comm As New SqlCommand
        conn.ConnectionString = connectionString
        conn.Open()
        comm.Connection = conn
        comm.CommandText = "select  watermark_path from mau_hoa_don where loai_hoa_don_ct_template_id='" + mauso + "' and donvi_ma_dv='" + madv + "'"
        Dim reader As SqlDataReader = comm.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                res = reader(0).ToString
            End While
            reader.Close()
        Else
            res = String.Empty
        End If
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return res
    End Function

 <WebMethod()>
 Public Function thongke_soluongton_hd() As DataTable
     Dim dt As New DataTable("dsthongke")

     Using connection As New SqlConnection(connectionString)

         Using cmd As New SqlCommand("bao_cao_tong_quan_so_luong_hop_nhat", connection)
             cmd.CommandType = CommandType.StoredProcedure
             cmd.CommandTimeout = 0
             Using adapter As New SqlDataAdapter(cmd)
                 connection.Open()
                 adapter.Fill(dt)
             End Using
         End Using
     End Using

     Return dt
 End Function

 

    <WebMethod>
    Public Function GetKPI_CA2POS(tungay As String, denngay As String) As DataTable

        Dim dt As New DataTable("DSKPI")
        dt.Columns.Add("DThoai", GetType(String))
        dt.Columns.Add("HoTen", GetType(String))
        dt.Columns.Add("Email", GetType(String))
        dt.Columns.Add("SLDH", GetType(Integer))
        dt.Clear()

        ''xoa du lieu cu
        Delete_tempKPI()
        '' day du lieu moi
        Insert_tempKPI(tungay, denngay)

        'lay danh sach kpi theo tai khoan
        Dim conn As New SqlConnection
        Dim comm As New SqlCommand
        conn.ConnectionString = connString_UAT_POS
        conn.Open()
        comm.Connection = conn
        comm.CommandText = "select DThoai, Hoten, Email,case when sluong is null then 0 else sluong end as SLDH from Sheet1$ left join temp_kpi on Sheet1$.DThoai =  temp_kpi.donvi_ma_dv"
        Dim adapter As SqlDataAdapter = New SqlDataAdapter
        adapter.SelectCommand = comm
        adapter.Fill(dt)
        dt.Rows.Add("0903275027", "Đặng Vũ Hồng Quang", "quangdvh@cavn.vn", "250")
        dt.Rows.Add("0913395099", "Phùng Huy Tâm", "tam@cavn.vn", "250")
        dt.Rows.Add("0977287590", "Đặng Vũ Cường", "dangvucuong@gmail.com", "250")
        dt.Rows.Add("0902432283", "Đỗ Thị Thu Hằng", "hangdtt@cavn.vn", "300")
        dt.Rows.Add("0364505518", "Phạm Đình Nhật", "dinhnhat081000@gmail.com", "300")
        '  dt.Rows.Add("0368994240", "Bùi Thị Oanh", "oanh@cavn.vn", "80")

        dt.DefaultView.Sort = "Hoten asc"
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return dt
    End Function
    Private Sub Delete_tempKPI()

        Dim conn As New SqlConnection(connString_UAT_POS)
        Dim cmd As New SqlCommand("drop table temp_kpi", conn)
        Try
            conn.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conn.Close()
            conn.Dispose()
            SqlConnection.ClearAllPools()
        End Try
    End Sub

    Private Sub Insert_tempKPI(tungay As String, denngay As String)
        Dim sql As String = "Select donvi_ma_dv, count(id) As sluong INTO temp_kpi FROM don_hang WHERE donvi_ma_dv In (Select DThoai from Sheet1$) And (ngay_tao between @tungay and @denngay) group by donvi_ma_dv"
        Dim conn As New SqlConnection
        Dim comm As New SqlCommand
        conn.ConnectionString = connString_UAT_POS
        conn.Open()
        comm.Connection = conn
        comm.CommandText = sql
        comm.Parameters.AddWithValue("@tungay", tungay)
        comm.Parameters.AddWithValue("@denngay", denngay)
        comm.ExecuteNonQuery()
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
    End Sub

    <WebMethod()>
    Public Function DSChitiet_KPI(dthoai As String, tungay As String, denngay As String) As DataTable
        Dim dt As New DataTable("DSChitiet")
        dt.Columns.Add("Ngaytao", GetType(String))
        dt.Columns.Add("SLDH", GetType(Integer))
        dt.Clear()
        Dim sql As String = "select  convert (date,ngay_tao) as Ngaytao, count(id) as SLDH From don_hang where donvi_ma_dv=@mst and ngay_tao between @tungay and @denngay group by convert (date,ngay_tao)"

        Dim conn As New SqlConnection
        Dim comm As New SqlCommand
        conn.ConnectionString = connString_UAT_POS
        conn.Open()
        comm.Connection = conn
        comm.CommandText = sql
        comm.Parameters.AddWithValue("@mst", dthoai)
        comm.Parameters.AddWithValue("@tungay", tungay)
        comm.Parameters.AddWithValue("@denngay", denngay)

        Dim adapter As SqlDataAdapter = New SqlDataAdapter
        adapter.SelectCommand = comm
        adapter.Fill(dt)
        conn.Close()
        conn.Dispose()
        comm.Dispose()
        SqlConnection.ClearAllPools()
        Return dt
    End Function


    ' update 10-04-2026
    <WebMethod>
    Public Function GuiMailCT(machungtu As String, madonvi As String) As Integer
        Dim res As Integer = 0
        Try
            Dim json As String = Laythongtinchungtu(machungtu, madonvi)
            If Not String.IsNullOrEmpty(json) Then
                Dim result = JsonConvert.DeserializeObject(Of Root)(json)

                If result IsNot Nothing AndAlso result.data.Count > 0 Then
                    Dim ct As thongtinCT = result.data(0)
                    Dim dvbanhang, mauso, kyhieu, sochungtu, mstdn, tennmh, ngaylap, emailRecv, machinhanh As String
                    dvbanhang = ct.TenTC
                    mauso = ct.MSChungtu
                    kyhieu = ct.KHChungtu
                    sochungtu = ct.Sochungtu
                    mstdn = ct.MasothueTC
                    tennmh = ct.TenNNT
                    ngaylap = Convert.ToDateTime(ct.NgaylapCT).ToString("dd/MM/yyyy")
                    emailRecv = ct.EmailNNT
                    machinhanh = ct.MasothueTC
                    Dim res_mail = SendMail(machungtu, dvbanhang, mauso, kyhieu, sochungtu, mstdn, tennmh, ngaylap, emailRecv, machinhanh)
                    If res_mail = "1" Then
                        res = 1
                    Else
                        res = -3
                    End If
                End If

            Else
                res = -1
            End If

        Catch ex As Exception
            res = -2

        End Try
        Return res
    End Function
    Private Function SendMail(idchungtu As String, dvbanhang As String, mauso As String, kyhieu As String, sochungtu As String, mstdn As String, tennmh As String, ngaylap As String, emailRecv As String, machinhanh As String) As String
        Dim emailsent As String = String.Empty
        Dim passmail As String = String.Empty
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Try
            Dim baseurl As String = "https://apichungtuv2.nacencomm.vn"
            'Dim sochungtu As String = LoadData(idchungtu, "Sochungtu")
            Dim matracuuct As String = LoadData(idchungtu, "Matracuu")

            Dim sysEmail As String = "mailhoadon@nacencomm.vn"
            emailsent = sysEmail
            passmail = "zaq1ZAQ!"
            Using client As New SmtpClient("smtp.vccloud.vn", 587)

                client.EnableSsl = True
                client.UseDefaultCredentials = False
                client.Credentials = New NetworkCredential(emailsent, passmail)

                Using message As MailMessage = New MailMessage
                    message.From = New MailAddress(emailsent, dvbanhang)
                    Dim arrMail As String() = emailRecv.Split(";")
                    Dim demmail As Integer = arrMail.Count
                    If demmail > 0 Then
                        If demmail < 4 Then
                            Dim i As Integer
                            For i = 0 To demmail - 1
                                message.To.Add(New MailAddress(arrMail(i)))
                            Next
                        Else
                            Dim i As Integer
                            For i = 0 To 3
                                message.To.Add(New MailAddress(arrMail(i)))
                            Next
                        End If
                    Else
                        message.To.Add(New MailAddress(emailRecv))
                    End If

                    'If Not String.IsNullOrEmpty(CCMail) Then
                    '    message.Bcc.Add(New MailAddress(CCMail))
                    'End If

                    Dim raw As String = idchungtu & "|" & mstdn

                    Dim token As String = CryptoHelper.Encrypt(raw)


                    Dim link As String = baseurl & "/ViewChungTu.aspx?q=" & token
                    Dim strhref As String = link

                    message.Subject = "Chứng từ xuất cho: " & mstdn & " - " & tennmh & "_Ngày: " & ngaylap
                    message.Body = "Kính gửi Ông/Bà" & tennmh & "! <br /><br />" & dvbanhang & "</b> đã xuất chứng từ khấu trừ thuế thu nhập cá nhân cho ông/bà.                    Vui lòng nhấp vào liên kết sau để xem thông tin chứng từ: <br /> <a href=""" & strhref & """ target=""_blank"">" & strhref & "</a>" & "<br /> hoặc tra cứu chứng từ theo mã: <b> " & matracuuct & " </b> <br /> Thông tin chi tiết của chứng từ: <br /><br />Mẫu số: " & mauso & "<br /> <br />Ký hiệu: " & kyhieu & "<br /><br />Số chứng từ: " & sochungtu & "<br /><br />Đây là mail tự động, Quý khách vui lòng không trả lời lại mail này.<br /><br /><b><i>Trân trọng cám ơn sự hợp tác của Quý khách hàng!</i></b><br/><br/><i>(Giải pháp Hoá đơn điện tử CA2-eInvoice được cung cấp bởi Công ty cổ phần công nghệ thẻ Nacencomm - 0103930279)</i><br/>"
                    message.IsBodyHtml = True
                    client.Send(message)
                    '   Call VLogmail(hdMaDV("value").ToString, emailsent, emailRecv, sochungtu, baseurl & "/viewct.aspx?id=" & idchungtu)
                End Using
            End Using
            Return "1"

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod>
    Public Function GetHTMLTBSS(matbss As String, madv As String) As String
        Dim response As New Dictionary(Of String, Object)()
        Dim res As String = String.Empty
        Try
            Dim b64 As String = LayXmlTBSSChungTu(madv, matbss)
            If Not String.IsNullOrEmpty(b64) Then
                Dim json As JObject = JObject.Parse(b64)
                Dim base64Xml As String = json("data").ToString()
                Dim xsltFile As String = Server.MapPath("~/Template/mautbss.xslt") '"D:\PROJECT\HOADONHOPNHAT\PRO\mautbss.xslt"
                Dim htmlFile As String = "D:\tbss304.html"
                res = TaoHTML_TBSS_TuBase64(base64Xml, xsltFile)
                response("status") = "success"
                response("data") = res
            Else
                response("status") = "error"
                response("data") = String.Empty
                res = String.Empty
            End If
        Catch ex As Exception
            response("status") = "error"
            response("data") = ex.Message
        End Try
        Return JsonConvert.SerializeObject(response)

    End Function

    Public Function TaoHTML_TBSS_TuBase64(
        base64Xml As String,
        xsltPath As String) As String
        Dim xmlBytes As Byte() =
                Convert.FromBase64String(base64Xml)

        Dim xmlString As String =
                Encoding.UTF8.GetString(xmlBytes)

        ' load xml
        Dim xmlDoc As New XmlDocument()
        xmlDoc.LoadXml(xmlString)

        ' load xslt
        Dim xslt As New XslCompiledTransform()
        xslt.Load(xsltPath)

        ' transform -> string
        Using sw As New StringWriter()

            Using writer As XmlWriter =
                    XmlWriter.Create(sw, xslt.OutputSettings)

                xslt.Transform(
                        xmlDoc,
                        Nothing,
                        writer)
            End Using
            Return sw.ToString()
        End Using

    End Function

End Class