Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Dynamic
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Reflection.Emit
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography.Xml
Imports System.Threading
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl
Imports Org.BouncyCastle.Asn1.Pkcs
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Ocsp
Imports RestSharp
Imports SignLib
Imports SignLib.Certificates
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization
Imports System.Web.Services.Description
Imports System
Imports System.Xml.Linq
Imports Org.BouncyCastle.Utilities.Encoders
Imports System.Security.Claims



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class wsHSM
    Inherits System.Web.Services.WebService
    Dim chuoiketnoi As String = "Data Source=192.168.2.195;Initial Catalog=SignHSM;Persist Security Info=True;User ID=sa;Password=zaq1ZAQ!"
    Dim mstgp As String = "0103930279"
    Dim madvauthen As String = "0103930279"
    Dim passauthen As String = "0103930279@!"
    Public Authentication As Authenticate.AuthHeader

    Private Function CheckAuthenWS(masothue As String) As Integer
        Dim kq = 0
        If Authentication Is Nothing Then
            kq = -1 ' Không có thông tin kết nối
        Else
            Dim username = Authentication.Username
            Dim pass = Authentication.Password
            Dim taikhoan As ttTaikhoan = New ttTaikhoan()
            taikhoan = GetInfoAccAuthen(masothue)
            If taikhoan.isActive = 1 Then
                If taikhoan.Username = username AndAlso taikhoan.passwword = pass Then
                    kq = 1
                Else
                    kq = -2 ' thông tin đăng nhập không đúng
                End If
            Else
                kq = -3 ' Tài khoản không hoạt động
            End If
        End If
        Return kq
    End Function

    Private Function GetInfoAccAuthen(mst As String) As ttTaikhoan
        Dim kq As ttTaikhoan = New ttTaikhoan()

        Dim connection As SqlConnection = New SqlConnection(chuoiketnoi)
        Dim dt As DataTable = New DataTable()

        connection.Open()
        Dim sql = String.Empty
        sql = "select * from Taikhoan where MST=@MST"
        Dim comm As SqlCommand = New SqlCommand(sql, connection)
        comm.Parameters.AddWithValue("@MST", mst)
        Dim adapter As SqlDataAdapter = New SqlDataAdapter()
        adapter.SelectCommand = comm
        adapter.Fill(dt)
        If dt.Rows.Count > 0 Then
            kq.MST = dt.Rows(0)("idtk").ToString()
            kq.Username = dt.Rows(0)("Username").ToString()
            kq.passwword = dt.Rows(0)("Password").ToString()
            kq.isActive = Convert.ToInt32(dt.Rows(CInt(0))(CStr("isActive")).ToString())
        End If
        connection.Close()
        connection.Dispose()
        SqlConnection.ClearAllPools()
        Return kq
    End Function

    Private Function GetDecimal(ByVal i As Double) As Double
        Dim m As Integer
        m = Len(Trim(Str(i))) - InStr(Str(i), ".") + 1
        GetDecimal = Right(Str(i), Len(Str(i)) - InStr(Str(i), "."))
        GetDecimal = GetDecimal / (10 ^ m)
    End Function

    <SoapHeader("Authentication")>
    <WebMethod()>
    Public Function CA2KySo_HD(base64xml As String, masothue As String, serialnumber As String)

        Dim resapi As New ResponeKyso
        Dim res As String = String.Empty
        Dim thongbao As String = String.Empty
        Dim bsxmlchuaky As String = String.Empty
        Dim jsSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim checkauthen As Integer = CheckAuthenWS(masothue)
        If checkauthen = 1 Then
            If IsBase64String(base64xml) Then
                Dim xmlchuaky As String = String.Empty
                xmlchuaky = Encoding.UTF8.GetString(Convert.FromBase64String(base64xml))
                Dim xmldoc As New XmlDocument
                xmldoc.LoadXml(xmlchuaky)
                Dim cert As X509Certificate2 = New X509Certificate2
                '  cert = LoadCert(serialnumber)
                Dim kqky As String = SignXml(xmldoc, cert, masothue, serialnumber)
                If Not String.IsNullOrEmpty(kqky) Then
                    Dim checkbase64 As Boolean = IsBase64String(kqky)
                    If checkbase64 = True Then
                        thongbao = "Ký số thành công"
                        resapi.Macode = 1
                        resapi.Message = thongbao
                        resapi.SignedData = kqky
                        res = jsSerializer.Serialize(resapi)
                    Else
                        thongbao = "Ký số không thành công. " & kqky
                        resapi.Macode = -2
                        resapi.Message = thongbao
                        res = jsSerializer.Serialize(resapi)
                    End If
                Else
                    thongbao = "Không lấy được kết quả ký số"
                    resapi.Macode = -3
                    resapi.Message = thongbao
                    res = jsSerializer.Serialize(resapi)

                End If
            Else
                thongbao = "Sai định dạng dữ liệu"
                resapi.Macode = -1
                resapi.Message = "Sai định dạng dữ liệu"
                res = jsSerializer.Serialize(resapi)
            End If

            Call insLogCallService(masothue, serialnumber, "CA2KySo_HD", base64xml, res)
        Else
            res = "Lỗi xác thực"
        End If

        Return res
    End Function

    <WebMethod()>
    Public Function CA2KyGuiYCCM_HD(base64xml As String, masothue As String, serialnumber As String)
        Dim res As String = String.Empty
        Dim mst As String = String.Empty
        Dim bsxmlchuaky As String = String.Empty
        Dim checkauthen As Integer = CheckAuthenWS(masothue)
        If checkauthen = 1 Then

            Dim resapi As New ResponeAPICapma
            If IsBase64String(base64xml) Then
                Dim servicetvan As ServTVAN.WSInterTRCA2 = New ServTVAN.WSInterTRCA2()
                Dim ttketnoi As New ServTVAN.AuthHeader
                'Dim servicetvan As servicetvan78uat.WSInterTRCA2 = New servicetvan78uat.WSInterTRCA2()
                'Dim ttketnoi As New servicetvan78uat.AuthHeader
                ttketnoi.Username = madvauthen
                ttketnoi.Password = passauthen
                servicetvan.AuthHeaderValue = ttketnoi
                Dim macode, tdiepphanhoi, phanhoi, khoaphien, mltdiep, strThongdiep As String
                Dim macqt As String = String.Empty

                Dim xml As String = Encoding.UTF8.GetString(Convert.FromBase64String(base64xml))
                Dim xmldoc1 As XmlDocument = New XmlDocument
                xmldoc1.LoadXml(xml)

                Dim ele_mst As XmlElement
                ele_mst = TryCast(xmldoc1.GetElementsByTagName("MST")(0), XmlElement)
                mst = ele_mst.InnerText

                Dim ele_khmshdon As XmlElement
                ele_khmshdon = TryCast(xmldoc1.GetElementsByTagName("KHMSHDon")(0), XmlElement)
                Dim khmshdon As String = ele_khmshdon.InnerText

                Dim ele_khhdon As XmlElement
                ele_khhdon = TryCast(xmldoc1.GetElementsByTagName("KHHDon")(0), XmlElement)
                Dim khhdon As String = ele_khhdon.InnerText

                Dim ele_sohoadon As XmlElement
                ele_sohoadon = TryCast(xmldoc1.GetElementsByTagName("SHDon")(0), XmlElement)
                Dim sohoadon As String = ele_sohoadon.InnerText

                Dim khoaphien_prefix As String = mst & "_" & khmshdon & khhdon & "_" & sohoadon

                'check log truyennhan
                Dim kqLogTN As DataTable = servicetvan.LayketquathongdiepTQ_Khoaphien_prefix(khoaphien_prefix, mstgp)
                If kqLogTN.Rows.Count > 0 Then
                    For i = 0 To kqLogTN.Rows.Count - 1
                        mltdiep = kqLogTN.Rows(i)("MLTDiep")

                        If mltdiep = "202" Then
                            macode = 1
                            khoaphien = kqLogTN.Rows(i)("Khoaphien")
                            tdiepphanhoi = kqLogTN.Rows(i)("XMLThongdiep")
                            strThongdiep = Encoding.UTF8.GetString(Convert.FromBase64String(tdiepphanhoi))
                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(strThongdiep)

                            Dim root As XmlElement = doc.DocumentElement
                            Dim nodelist_DLieu As XmlNodeList = root.GetElementsByTagName("DLieu")
                            Dim dlhd As String = String.Empty
                            For Each node As XmlNode In nodelist_DLieu
                                Dim panode As XmlNode = node.ParentNode
                                If panode.Name = "TDiep" Then
                                    dlhd = node.InnerXml
                                End If
                            Next
                            Dim element As XmlElement
                            element = TryCast(doc.GetElementsByTagName("MCCQT")(0), XmlElement)
                            macqt = element.InnerText
                            phanhoi = "Hóa đơn đã được cấp mã"
                            Exit For
                        ElseIf kqLogTN.Rows(i)("MLTDiep") = "204" Then
                            khoaphien = kqLogTN.Rows(i)("Khoaphien")
                            tdiepphanhoi = kqLogTN.Rows(i)("XMLThongdiep")
                            strThongdiep = Encoding.UTF8.GetString(Convert.FromBase64String(tdiepphanhoi))
                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(strThongdiep)
                            If strThongdiep.Contains("<LTBao>2</LTBao>") Then
                                macode = 1
                                phanhoi = "Đã gửi lên Cơ quan thuế, hóa đơn hợp lệ."
                            Else
                                macode = 2
                                Dim element As XmlElement
                                element = TryCast(doc.GetElementsByTagName("MTLoi")(0), XmlElement)
                                phanhoi = "Hóa đơn không hợp lệ: " & element.InnerText
                            End If
                        ElseIf kqLogTN.Rows(i)("MLTDiep") = "-1" Then
                            macode = -1
                            phanhoi = "Lỗi thông điệp, phản hồi kỹ thuật -1"
                        Else
                            macode = 3
                            phanhoi = "Phản hồi thông điệp hợp lệ"
                        End If

                    Next

                    resapi.Macode = macode
                    resapi.Message = phanhoi
                    resapi.Masothue = mst
                    resapi.KHMSHDon = khmshdon
                    resapi.KHHDon = khhdon
                    resapi.Sohoadon = sohoadon

                    resapi.MCCQT = macqt
                    resapi.TransactionID = khoaphien
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                    Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                    Return jsonString
                    Exit Function

                End If

                Dim cert As X509Certificate2 = New X509Certificate2
                ' cert = LoadCert(serialnumber)
                Dim kqky As String = SignXml(xmldoc1, cert, mst, serialnumber)
                If Not String.IsNullOrEmpty(kqky) Then
                    Dim checkbase64 As Boolean = IsBase64String(kqky)
                    If checkbase64 = True Then

                        'GUI YEU CAU CAP MA
                        Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                        Dim key As String = mstgp & guidstr.Replace("-", "")

                        Dim thongdiep As String = CreatFileXML_Thong_diep_den_co_quan_thue(mstgp, "0103930279", "200", key, "", "1", mst, kqky)
                        Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(thongdiep))

                        ''Gui thong diep yeu cau cap ma hoa don
                        macode = servicetvan.Guithongdiep_MTDiep(base64thongdiep, 1)
                        If macode.Length > 2 Then

                            Dim khoaphientn As String = macode.Split("|")(0)
                            Dim mtdieptvan As String = macode.Split("|")(1)
                            Dim ngaytdiep As String = Now.ToString("yyyyMMdd")

                            Dim kqcapma As String = String.Empty
                            Dim dlhd As String = String.Empty

                            'While kqcapma = ""
                            '    Thread.Sleep(7000)
                            '    kqcapma = servicetvan.LayKQThongdiep(macode, mstgp_ontik)
                            'End While
                            Dim counttime As Integer = 0
                            Dim wsKafkaconsumer As wsKafka.ResponseKafka = New wsKafka.ResponseKafka
                            While kqcapma = ""
                                kqcapma = wsKafkaconsumer.GetResp_Kafka(mtdieptvan, ngaytdiep)
                                Thread.Sleep(1000)
                                If counttime >= 10 Then
                                    Exit While
                                End If
                            End While

                            If kqcapma = "-1" Or kqcapma = "-5" Or kqcapma = "-6" Or kqcapma = "-7" Then
                                'loi
                                ' res = "Không lấy được phản hồi của CQT: " & kqcapma
                                ' 
                                resapi.Macode = -2
                                resapi.Message = "Không lấy được phản hồi của CQT: " & kqcapma
                                resapi.Masothue = mst
                                resapi.KHMSHDon = khmshdon
                                resapi.KHHDon = khhdon
                                resapi.Sohoadon = sohoadon
                                resapi.Motaloi = kqcapma
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                                res = jsonString
                            Else
                                Dim base64phanhoi = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(kqcapma))
                                Dim doc As XmlDocument = New XmlDocument()
                                doc.LoadXml(kqcapma)

                                Dim root As XmlElement = doc.DocumentElement
                                Dim nodelist_DLieu As XmlNodeList = root.GetElementsByTagName("DLieu")
                                Dim element As XmlElement
                                element = TryCast(doc.GetElementsByTagName("MLTDiep")(0), XmlElement)

                                Dim ele_shd As XmlElement
                                ele_shd = TryCast(doc.GetElementsByTagName("SHDon")(0), XmlElement)


                                mltdiep = element.InnerText
                                If mltdiep = "202" Then
                                    For Each node As XmlNode In nodelist_DLieu
                                        Dim panode As XmlNode = node.ParentNode
                                        If panode.Name = "TDiep" Then
                                            dlhd = node.InnerXml
                                        End If
                                    Next
                                    element = TryCast(doc.GetElementsByTagName("MCCQT")(0), XmlElement)
                                    macqt = element.InnerText

                                    Dim base64dlhd As String
                                    Dim encoding As New System.Text.ASCIIEncoding()
                                    Dim byte1 As Byte() = Text.Encoding.UTF8.GetBytes(dlhd)
                                    base64dlhd = Convert.ToBase64String(byte1)
                                    resapi.Macode = 1
                                    resapi.Message = "Ký gửi yêu cầu cấp mã HĐ thành công. HĐ đã được cấp mã"
                                    resapi.Masothue = mst
                                    resapi.KHMSHDon = khmshdon
                                    resapi.KHHDon = khhdon
                                    resapi.Sohoadon = ele_shd.InnerText
                                    resapi.MCCQT = macqt
                                    resapi.XMLComaCQT = base64dlhd
                                    resapi.TransactionID = macode
                                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                    Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                                    res = jsonString

                                Else
                                    If mltdiep = "-1" Then

                                        Dim element2 As XmlElement
                                        element2 = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)

                                        resapi.Macode = -1
                                        resapi.Message = "Thông điệp không hợp lệ"
                                        resapi.Masothue = mst
                                        resapi.KHMSHDon = khmshdon
                                        resapi.KHHDon = khhdon
                                        resapi.Sohoadon = sohoadon
                                        resapi.Motaloi = "Phản hồi kỹ thuật -1: " & element2.InnerText
                                        resapi.TransactionID = macode
                                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                        Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                                        res = jsonString
                                    ElseIf mltdiep = "204" Then

                                        Dim element2 As XmlElement
                                        element2 = TryCast(doc.GetElementsByTagName("MTLoi")(0), XmlElement)

                                        resapi.Macode = 2
                                        resapi.Message = "Đã ký gửi yêu cầu cấp mã hóa đơn lên CQT. HĐ không đủ điều kiện cấp mã"
                                        resapi.Masothue = mst
                                        resapi.KHMSHDon = khmshdon
                                        resapi.KHHDon = khhdon
                                        resapi.Sohoadon = sohoadon
                                        resapi.Motaloi = element2.InnerText
                                        resapi.TransactionID = macode
                                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                        Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                                        res = jsonString
                                    Else
                                        ''999
                                        Dim element2 As XmlElement
                                        element2 = TryCast(doc.GetElementsByTagName("TTTNhan")(0), XmlElement)
                                        If element2.InnerText = 0 Then
                                            resapi.Macode = 3
                                            resapi.Message = "Phản hồi kỹ thuật hợp lệ. Chưa có kết quả kiểm tra hóa đơn"
                                            resapi.Motaloi = ""
                                        ElseIf element2.InnerText = 1 Then
                                            Dim ele_loi As XmlElement
                                            ele_loi = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)

                                            resapi.Macode = -1
                                            resapi.Message = "Thông điệp không hợp lệ"
                                            resapi.Motaloi = ele_loi.InnerText
                                        End If

                                        resapi.Masothue = mst
                                        resapi.KHMSHDon = khmshdon
                                        resapi.KHHDon = khhdon
                                        resapi.Sohoadon = sohoadon
                                        resapi.TransactionID = macode
                                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                        Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                                        res = jsonString
                                    End If
                                End If
                            End If

                        Else
                            resapi.Macode = -3
                            resapi.Masothue = mst
                            resapi.KHMSHDon = khmshdon
                            resapi.KHHDon = khhdon
                            resapi.Sohoadon = sohoadon
                            resapi.Message = "Không gửi được thông điệp lên CQT"
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                            res = jsonString
                        End If
                    Else
                        resapi.Macode = -4
                        resapi.Message = "Ký hóa đơn không thành công: " & kqky
                        resapi.Masothue = mst
                        resapi.KHMSHDon = khmshdon
                        resapi.KHHDon = khhdon
                        resapi.Sohoadon = sohoadon
                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                        Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                        res = jsonString
                    End If
                Else
                    resapi.Macode = -5
                    resapi.Message = "Không lấy được kết quả ký hóa đơn"
                    resapi.Masothue = mst
                    resapi.KHMSHDon = khmshdon
                    resapi.KHHDon = khhdon
                    resapi.Sohoadon = sohoadon
                    ' resapi.Message = kqky
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                    Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                    res = jsonString
                End If

            Else
                resapi.Macode = -6
                resapi.Message = "Sai định dạng dữ liệu chuỗi đầu vào"
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                Call insLogCallService(mst, serialnumber, "CA2KyGuiYCCM_HD", base64xml, jsonString)
                Return jsonString
                Exit Function
            End If

        Else
            res = "Tài khoản xác thực không đúng"
        End If
        Return res
    End Function

    <WebMethod()>
    Public Function CA2KyGuiTCT_HDKM(base64XML As String, masothue As String, serialnumber As String)

        Dim res As String = String.Empty
        Dim bsxmlchuaky As String = String.Empty
        Dim resapi As New ResponeAPICapma
        Dim idhd As String = String.Empty
        Dim checkauthen As Integer = CheckAuthenWS(masothue)
        If checkauthen = 1 Then
            If IsBase64String(base64XML) = False Then
                resapi.Macode = -6
                resapi.Message = "Sai định dạng dữ liệu đầu vào"
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                Return jsonString
                Exit Function
            End If

            Dim xml As String = Encoding.UTF8.GetString(Convert.FromBase64String(base64XML))
            Dim xmldoc1 As XmlDocument = New XmlDocument
            xmldoc1.LoadXml(xml)

            Dim ele_mst As XmlElement
            ele_mst = TryCast(xmldoc1.GetElementsByTagName("MST")(0), XmlElement)
            Dim mst As String = ele_mst.InnerText

            Dim ele_khmshdon As XmlElement
            ele_khmshdon = TryCast(xmldoc1.GetElementsByTagName("KHMSHDon")(0), XmlElement)
            Dim khmshdon As String = ele_khmshdon.InnerText

            Dim ele_khhdon As XmlElement
            ele_khhdon = TryCast(xmldoc1.GetElementsByTagName("KHHDon")(0), XmlElement)
            Dim khhdon As String = ele_khhdon.InnerText

            Dim ele_sohoadon As XmlElement
            ele_sohoadon = TryCast(xmldoc1.GetElementsByTagName("SHDon")(0), XmlElement)
            Dim sohoadon As String = ele_sohoadon.InnerText

            Dim khoaphien_prefix As String = mst & "_" & khmshdon & khhdon & "_" & sohoadon & "_000_"


            Dim cert As X509Certificate2 = New X509Certificate2
            ' cert = LoadCert(serialnumber)
            Dim kqky As String = SignXml(xmldoc1, cert, mst, serialnumber)
            If Not String.IsNullOrEmpty(kqky) Then
                Dim checkbase64 As Boolean = IsBase64String(kqky)
                If checkbase64 = True Then
                    'GUI YEU CAU KHONG MA
                    Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                    Dim key As String = mstgp & guidstr.Replace("-", "")

                    Dim thongdiep As String = CreatFileXML_Thong_diep_den_co_quan_thue(mstgp, mstgp, "203", key, "", "1", mst, kqky)

                    Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(thongdiep))

                    ''Gui thong diep yeu cau cap ma hoa don

                    Dim servicetvan As ServTVAN.WSInterTRCA2 = New ServTVAN.WSInterTRCA2()
                    Dim ttketnoi As New ServTVAN.AuthHeader
                    'Dim servicetvan As servicetvan78uat.WSInterTRCA2 = New servicetvan78uat.WSInterTRCA2()
                    'Dim ttketnoi As New servicetvan78uat.AuthHeader

                    ttketnoi.Username = madvauthen
                    ttketnoi.Password = passauthen
                    servicetvan.AuthHeaderValue = ttketnoi

                    Dim macode As String = servicetvan.Guithongdiep_MTDiep(base64thongdiep, 1)
                    If macode.Length > 2 Then
                        Dim khoaphientn As String = macode.Split("|")(0)
                        Dim mtdieptvan As String = macode.Split("|")(1)
                        Dim ngaytdiep As String = Now.ToString("yyyyMMdd")

                        Dim idtruyennhan As String = "0"
                        Dim kqcapma As String = String.Empty
                        Dim dlhd As String = String.Empty
                        Dim macqt As String = String.Empty
                        'While kqcapma = ""
                        '    Thread.Sleep(7000)
                        '    kqcapma = servicetvan.LayKQThongdiep(macode, "0110400071")
                        'End While
                        Dim count As Integer = 0
                        Dim wsKafkaconsumer As wsKafka.ResponseKafka = New wsKafka.ResponseKafka
                        While kqcapma = ""
                            kqcapma = wsKafkaconsumer.GetResp_Kafka(mtdieptvan, ngaytdiep)
                            Thread.Sleep(1000)
                            If count >= 10 Then
                                Exit While
                            End If
                        End While

                        Dim base64phanhoi = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(kqcapma))
                        If kqcapma = "-1" Or kqcapma = "-5" Or kqcapma = "-6" Or kqcapma = "-7" Then
                            'loi
                            ' res = "Không lấy được phản hồi của CQT: " & kqcapma
                            resapi.Macode = -2
                            resapi.Message = "Không lấy được phản hồi của CQT: " & kqcapma
                            resapi.Masothue = mst
                            resapi.KHMSHDon = khmshdon
                            resapi.KHHDon = khhdon
                            resapi.Sohoadon = sohoadon
                            resapi.Motaloi = kqcapma
                            resapi.TransactionID = macode
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                            Return jsonString
                        Else
                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(kqcapma)


                            Dim root As XmlElement = doc.DocumentElement
                            Dim nodelist_DLieu As XmlNodeList = root.GetElementsByTagName("DLieu")
                            Dim element As XmlElement
                            element = TryCast(doc.GetElementsByTagName("MLTDiep")(0), XmlElement)
                            Dim mltdiep As String = element.InnerText
                            If mltdiep = "204" Then

                                element = TryCast(doc.GetElementsByTagName("LTBao")(0), XmlElement)
                                Dim loaitb = element.InnerText

                                If loaitb = 2 Then

                                    resapi.Macode = 1
                                    resapi.Message = "Ký gửi HĐ không mã lên CQT thành công. Hóa đơn hợp lệ"
                                    resapi.Masothue = mst
                                    resapi.KHMSHDon = khmshdon
                                    resapi.KHHDon = khhdon
                                    resapi.Sohoadon = sohoadon
                                    resapi.MCCQT = ""
                                    resapi.TransactionID = macode
                                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                    Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                                    Return jsonString
                                Else

                                    Dim element2 As XmlElement
                                    element2 = TryCast(doc.GetElementsByTagName("MTLoi")(0), XmlElement)
                                    resapi.Macode = 2
                                    resapi.Message = "Đã ký gửi HĐ không mã lên CQT. Hóa đơn không hợp lệ"
                                    resapi.Masothue = mst
                                    resapi.KHMSHDon = khmshdon
                                    resapi.KHHDon = khhdon
                                    resapi.Sohoadon = sohoadon

                                    resapi.MCCQT = element2.InnerText
                                    resapi.TransactionID = macode
                                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                    Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                                    Return jsonString
                                End If
                            Else
                                If mltdiep = "-1" Then

                                    Dim element2 As XmlElement
                                    element2 = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)
                                    resapi.Macode = -1
                                    resapi.Message = "Thông điệp không hợp lệ.  Phản hồi kỹ thuật -1"
                                    resapi.Masothue = mst
                                    resapi.KHMSHDon = khmshdon
                                    resapi.KHHDon = khhdon
                                    resapi.Sohoadon = sohoadon
                                    resapi.Motaloi = element2.InnerText
                                    resapi.TransactionID = macode
                                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                    Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                                    Return jsonString
                                Else
                                    resapi.Macode = 3
                                    resapi.Message = "Phản hồi kỹ thuật hợp lệ. Chưa có kết quả kiểm tra hóa đơn"
                                    resapi.Masothue = mst
                                    resapi.KHMSHDon = khmshdon
                                    resapi.KHHDon = khhdon
                                    resapi.Sohoadon = sohoadon
                                    resapi.Motaloi = ""
                                    resapi.TransactionID = macode
                                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                    Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                                    Return jsonString

                                End If
                            End If
                        End If
                    Else
                        resapi.Macode = -3
                        resapi.Message = "Không gửi được thông điệp lên CQT"
                        resapi.Masothue = mst
                        resapi.KHMSHDon = khmshdon
                        resapi.KHHDon = khhdon
                        resapi.Sohoadon = sohoadon

                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                        Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                        Return jsonString

                    End If
                Else
                    resapi.Macode = -4
                    resapi.Message = "Ký hóa đơn không thành công: " & kqky
                    resapi.Masothue = mst
                    resapi.KHMSHDon = khmshdon
                    resapi.KHHDon = khhdon
                    resapi.Sohoadon = sohoadon
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                    Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                    Return jsonString
                End If
            Else
                resapi.Macode = -5
                resapi.Message = "Không lấy được kết quả ký hóa đơn"
                resapi.Masothue = mst
                resapi.KHMSHDon = khmshdon
                resapi.KHHDon = khhdon
                resapi.Sohoadon = sohoadon
                ' resapi.Message = kqky
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                Call insLogCallService(mst, serialnumber, "CA2KyGuiTCT_HDKM", base64XML, jsonString)
                Return jsonString
            End If

        Else
            res = "Lỗi xác thực"
        End If
        Return res
    End Function

    <WebMethod()>
    Public Function CA2GuiYCCM(xmlSigned As String, masothue As String)
        Dim res As String = String.Empty
        Dim bsxmlchuaky As String = String.Empty
        Dim resapi As New ResponeAPICapma
        Dim checkauthen As Integer = CheckAuthenWS(masothue)
        If checkauthen = 1 Then
            Dim servicetvan As ServTVAN.WSInterTRCA2 = New ServTVAN.WSInterTRCA2()
            Dim ttketnoi As New ServTVAN.AuthHeader
            'Dim servicetvan As servicetvan78uat.WSInterTRCA2 = New servicetvan78uat.WSInterTRCA2()
            'Dim ttketnoi As New servicetvan78uat.AuthHeader
            ttketnoi.Username = madvauthen
            ttketnoi.Password = passauthen
            servicetvan.AuthHeaderValue = ttketnoi
            Dim macode, tdiepphanhoi, phanhoi, khoaphien, mltdiep, strThongdiep As String
            Dim macqt As String = String.Empty

            If IsBase64String(xmlSigned) = False Then
                resapi.Macode = -4
                resapi.Message = "Sai định dạng dữ liệu chuỗi đầu vào"
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                Return jsonString
                Exit Function
            End If

            Dim xml As String = Encoding.UTF8.GetString(Convert.FromBase64String(xmlSigned))
            Dim xmldoc1 As XmlDocument = New XmlDocument
            xmldoc1.LoadXml(xml)
            Dim ele_mst As XmlElement
            ele_mst = TryCast(xmldoc1.GetElementsByTagName("MST")(0), XmlElement)
            Dim mst As String = ele_mst.InnerText

            Dim ele_khmshdon As XmlElement
            ele_khmshdon = TryCast(xmldoc1.GetElementsByTagName("KHMSHDon")(0), XmlElement)
            Dim khmshdon As String = ele_khmshdon.InnerText

            Dim ele_khhdon As XmlElement
            ele_khhdon = TryCast(xmldoc1.GetElementsByTagName("KHHDon")(0), XmlElement)
            Dim khhdon As String = ele_khhdon.InnerText

            Dim ele_sohoadon As XmlElement
            ele_sohoadon = TryCast(xmldoc1.GetElementsByTagName("SHDon")(0), XmlElement)
            Dim sohoadon As String = ele_sohoadon.InnerText

            If Not xmldoc1.InnerXml.Contains("Signature") Then
                resapi.Macode = 0
                resapi.Message = "Hóa đơn chưa có chữ ký số"
                resapi.Masothue = mst
                resapi.KHMSHDon = khmshdon
                resapi.KHHDon = khhdon
                resapi.Sohoadon = sohoadon
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                Return jsonString
                Exit Function
            End If
            Dim khoaphien_prefix As String = mst & "_" & khmshdon & khhdon & "_" & sohoadon & "_000_"

            Dim kqLogTN As DataTable = servicetvan.LayketquathongdiepTQ_Khoaphien_prefix(khoaphien_prefix, mstgp)
            If kqLogTN.Rows.Count > 0 Then
                For i = 0 To kqLogTN.Rows.Count - 1
                    mltdiep = kqLogTN.Rows(i)("MLTDiep")

                    If mltdiep = "202" Then
                        macode = 1
                        khoaphien = kqLogTN.Rows(i)("Khoaphien")
                        tdiepphanhoi = kqLogTN.Rows(i)("XMLThongdiep")
                        strThongdiep = Encoding.UTF8.GetString(Convert.FromBase64String(tdiepphanhoi))
                        Dim doc As XmlDocument = New XmlDocument()
                        doc.LoadXml(strThongdiep)

                        Dim root As XmlElement = doc.DocumentElement
                        Dim nodelist_DLieu As XmlNodeList = root.GetElementsByTagName("DLieu")
                        Dim dlhd As String = String.Empty
                        For Each node As XmlNode In nodelist_DLieu
                            Dim panode As XmlNode = node.ParentNode
                            If panode.Name = "TDiep" Then
                                dlhd = node.InnerXml
                            End If
                        Next
                        Dim element As XmlElement
                        element = TryCast(doc.GetElementsByTagName("MCCQT")(0), XmlElement)
                        macqt = element.InnerText
                        phanhoi = "Hóa đơn đã được cấp mã"
                        Exit For
                    ElseIf kqLogTN.Rows(i)("MLTDiep") = "204" Then
                        khoaphien = kqLogTN.Rows(i)("Khoaphien")
                        tdiepphanhoi = kqLogTN.Rows(i)("XMLThongdiep")
                        strThongdiep = Encoding.UTF8.GetString(Convert.FromBase64String(tdiepphanhoi))
                        Dim doc As XmlDocument = New XmlDocument()
                        doc.LoadXml(strThongdiep)
                        If strThongdiep.Contains("<LTBao>2</LTBao>") Then
                            macode = 1
                            phanhoi = "Đã gửi lên Cơ quan thuế, hóa đơn hợp lệ."
                        Else
                            macode = 2
                            Dim element As XmlElement
                            element = TryCast(doc.GetElementsByTagName("MTLoi")(0), XmlElement)
                            phanhoi = "Đã gửi cơ quan thuế. Hóa đơn không đủ điều kiện cấp mã: " & element.InnerText
                        End If
                    ElseIf kqLogTN.Rows(i)("MLTDiep") = "-1" Then
                        macode = -1
                        phanhoi = "Lỗi thông điệp, phản hồi kỹ thuật -1"
                    Else
                        macode = 3
                        phanhoi = "Gửi cơ quan thuế thành công, chưa có kết quả kiểm tra hóa đơn"
                    End If

                Next

                resapi.Macode = macode
                resapi.Message = phanhoi
                resapi.Masothue = mst
                resapi.KHMSHDon = khmshdon
                resapi.KHHDon = khhdon
                resapi.Sohoadon = sohoadon
                resapi.MCCQT = macqt
                resapi.TransactionID = khoaphien
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                Return jsonString
                Exit Function
            Else
                ''hdon da ky nhung chua gui cap ma
                'GUI YEU CAU CAP MA
                Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                Dim key As String = mstgp & guidstr.Replace("-", "")

                Dim thongdiep As String = CreatFileXML_Thong_diep_den_co_quan_thue(mstgp, "0103930279", "200", key, "", "1", mst, xmlSigned)

                Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(thongdiep))

                ''Gui thong diep yeu cau cap ma hoa don
                macode = servicetvan.Guithongdiep_MTDiep(base64thongdiep, 1)
                If macode.Length > 2 Then
                    Dim khoaphientn As String = macode.Split("|")(0)
                    Dim mtdieptvan As String = macode.Split("|")(1)
                    Dim ngaytdiep As String = Now.ToString("yyyyMMdd")

                    Dim kqcapma As String = String.Empty
                    Dim dlhd As String = String.Empty

                    'While kqcapma = ""
                    '    Thread.Sleep(5000)
                    '    kqcapma = servicetvan.LayKQThongdiep(macode, "0110400071")
                    '    End While
                    Dim counttime As Integer = 0
                    Dim wsKafkaconsumer As wsKafka.ResponseKafka = New wsKafka.ResponseKafka
                    While kqcapma = ""
                        Thread.Sleep(1000)
                        kqcapma = wsKafkaconsumer.GetResp_Kafka(mtdieptvan, ngaytdiep)
                        If counttime >= 10 Then
                            Exit While
                        End If
                    End While

                    Dim base64phanhoi = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(kqcapma))
                    If kqcapma = "-1" Or kqcapma = "-5" Or kqcapma = "-6" Or kqcapma = "-7" Then

                        resapi.Macode = -2
                        resapi.Message = "Không lấy được phản hồi của CQT: " & kqcapma
                        resapi.Masothue = mst
                        resapi.KHMSHDon = khmshdon
                        resapi.KHHDon = khhdon
                        resapi.Sohoadon = sohoadon
                        resapi.Motaloi = kqcapma
                        resapi.TransactionID = macode
                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                        Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                        res = jsonString
                    Else
                        Dim doc As XmlDocument = New XmlDocument()
                        doc.LoadXml(kqcapma)

                        Dim root As XmlElement = doc.DocumentElement
                        Dim nodelist_DLieu As XmlNodeList = root.GetElementsByTagName("DLieu")
                        Dim element As XmlElement
                        element = TryCast(doc.GetElementsByTagName("MLTDiep")(0), XmlElement)

                        Dim ele_shd As XmlElement
                        ele_shd = TryCast(doc.GetElementsByTagName("SHDon")(0), XmlElement)


                        mltdiep = element.InnerText
                        If mltdiep = "202" Then
                            For Each node As XmlNode In nodelist_DLieu
                                Dim panode As XmlNode = node.ParentNode

                                If panode.Name = "TDiep" Then
                                    dlhd = node.InnerXml
                                End If
                            Next
                            element = TryCast(doc.GetElementsByTagName("MCCQT")(0), XmlElement)
                            macqt = element.InnerText

                            resapi.Macode = 1
                            resapi.Message = "Ký gửi yêu cầu cấp mã HĐ thành công. HĐ đã được cấp mã"
                            resapi.Masothue = mst
                            resapi.KHMSHDon = khmshdon
                            resapi.KHHDon = khhdon
                            resapi.Sohoadon = ele_shd.InnerText
                            resapi.MCCQT = macqt
                            resapi.TransactionID = macode
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                            res = jsonString

                        Else
                            If mltdiep = "-1" Then

                                Dim element2 As XmlElement
                                element2 = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)

                                resapi.Macode = -1
                                resapi.Message = "Thông điệp không hợp lệ"
                                resapi.Masothue = mst
                                resapi.KHMSHDon = khmshdon
                                resapi.KHHDon = khhdon
                                resapi.Sohoadon = sohoadon
                                resapi.Motaloi = "Phản hồi kỹ thuật -1: " & element2.InnerText
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                                res = jsonString

                            ElseIf mltdiep = "204" Then

                                Dim element2 As XmlElement
                                element2 = TryCast(doc.GetElementsByTagName("MTLoi")(0), XmlElement)

                                resapi.Macode = 2
                                resapi.Message = "Đã ký gửi yêu cầu cấp mã hóa đơn lên CQT. HĐ không đủ điều kiện cấp mã"
                                resapi.Masothue = mst
                                resapi.KHMSHDon = khmshdon
                                resapi.KHHDon = khhdon
                                resapi.Sohoadon = sohoadon
                                resapi.Motaloi = element2.InnerText
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                                res = jsonString
                            Else
                                ''999
                                Dim element2 As XmlElement
                                element2 = TryCast(doc.GetElementsByTagName("TTTNhan")(0), XmlElement)
                                If element2.InnerText = 0 Then
                                    resapi.Macode = 3
                                    resapi.Message = "Phản hồi kỹ thuật hợp lệ. Chưa có kết quả kiểm tra hóa đơn"
                                    resapi.Motaloi = ""
                                ElseIf element2.InnerText = 1 Then
                                    Dim ele_loi As XmlElement
                                    ele_loi = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)

                                    resapi.Macode = -1
                                    resapi.Message = "Thông điệp không hợp lệ"
                                    resapi.Motaloi = ele_loi.InnerText
                                End If

                                resapi.Masothue = mst
                                resapi.KHMSHDon = khmshdon
                                resapi.KHHDon = khhdon
                                resapi.Sohoadon = sohoadon
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                                res = jsonString
                            End If
                        End If
                    End If

                Else
                    resapi.Macode = -3
                    resapi.Masothue = mst
                    resapi.KHMSHDon = khmshdon
                    resapi.KHHDon = khhdon
                    resapi.Sohoadon = sohoadon
                    resapi.Message = "Không gửi được thông điệp lên CQT"
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                    Call insLogCallService(masothue, "", "CA2GuiYCCM", xmlSigned, jsonString)
                    res = jsonString
                End If
            End If
        Else
            res = "Lỗi xác thực"
        End If
        Return res
    End Function
    Private Sub insLogCallService(masothue As String, serial As String, tenham As String, request As String, respone As String)

        Dim conn As New SqlConnection(chuoiketnoi)
        conn.Open()
        Dim sql_log As String = "insert into LogSignHSM(MST, SerialNumber,InputTime,Request, SignedData, Method) values(@MST, @SerialNumber,@InputTime,@Request, @SignedData, @Method)"
        Dim comm_log As New SqlCommand(sql_log, conn)
        comm_log.Parameters.AddWithValue("@MST", masothue)
        comm_log.Parameters.AddWithValue("@SerialNumber", serial)
        comm_log.Parameters.AddWithValue("@Request", request)
        comm_log.Parameters.AddWithValue("@InputTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        comm_log.Parameters.AddWithValue("@SignedData", respone)
        comm_log.Parameters.AddWithValue("@Method", tenham)
        comm_log.ExecuteNonQuery()
        conn.Close()
        conn.Dispose()
        comm_log.Dispose()
        SqlConnection.ClearAllPools()
    End Sub

    <WebMethod()>
    Public Function CA2GuiBangke_MTTien(lstData As String, masothue As String, serialnumber As String) As String
        Dim kq As String = String.Empty
        Dim resapi As New ResponeListMTTien
        Dim checkauthen As Integer = CheckAuthenWS(masothue)
        If checkauthen = 1 Then
            Try

                Dim jsonhoadon As lsthoadonMTT = JsonConvert.DeserializeObject(Of lsthoadonMTT)(lstData)
                Dim lstxml As List(Of HoadonMTTien) = jsonhoadon.Lst

                Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                Dim key As String = mstgp & guidstr.Replace("-", "")
                Dim thongdiep As String = CreatFileXML_Thong_diep_206_CQT(mstgp, "0103930279", "206", key, "", lstxml.Count, masothue.Replace(" ", ""), lstxml)
                Dim xmldoc As New XmlDocument
                xmldoc.LoadXml(thongdiep)
                Dim cert As X509Certificate2 = New X509Certificate2
                '  cert = LoadCert(serialnumber)
                Dim kqky As String = SignXml(xmldoc, cert, masothue, serialnumber)
                If Not String.IsNullOrEmpty(kqky) Then
                    Dim checkbase64 As Boolean = IsBase64String(kqky)
                    If checkbase64 = True Then
                        Dim servicetvan As ServTVAN.WSInterTRCA2 = New ServTVAN.WSInterTRCA2()
                        Dim ttketnoi As New ServTVAN.AuthHeader
                        'Dim servicetvan As servicetvan78uat.WSInterTRCA2 = New servicetvan78uat.WSInterTRCA2()
                        'Dim ttketnoi As New servicetvan78uat.AuthHeader
                        ttketnoi.Username = madvauthen
                        ttketnoi.Password = passauthen
                        servicetvan.AuthHeaderValue = ttketnoi
                        Dim macode As String = servicetvan.Guithongdiep_MTDiep(kqky, 1)
                        If macode.Length > 2 Then

                            Dim kqcapma As String = String.Empty
                            Dim dlhd As String = String.Empty
                            Dim macqt As String = String.Empty
                            Dim khoaphientn As String = macode.Split("|")(0)
                            Dim mtdieptvan As String = macode.Split("|")(1)
                            Dim ngaytdiep As String = Now.ToString("yyyyMMdd")
                            'While kqcapma = ""
                            '    Thread.Sleep(7000)
                            '    kqcapma = servicetvan.LayKQThongdiep(macode, "0110400071")

                            'End While
                            Dim counttime As Integer = 0
                            Dim wsKafkaconsumer As wsKafka.ResponseKafka = New wsKafka.ResponseKafka
                            While kqcapma = ""

                                kqcapma = wsKafkaconsumer.GetResp_Kafka(mtdieptvan, ngaytdiep)
                                Thread.Sleep(1000)
                                If counttime >= 10 Then
                                    Exit While
                                End If
                            End While
                            If kqcapma = "-1" Or kqcapma = "-5" Or kqcapma = "-6" Or kqcapma = "-7" Then

                                resapi.Macode = -2
                                resapi.Message = "Không lấy được phản hồi của CQT: " & kqcapma
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                                Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)

                                Return jsonString
                                Exit Function
                            End If
                            Dim doc As XmlDocument = New XmlDocument()
                            doc.LoadXml(kqcapma)
                            Dim root As XmlElement = doc.DocumentElement
                            Dim nodelist_DLieu As XmlNodeList = root.GetElementsByTagName("DLieu")
                            Dim element As XmlElement
                            element = TryCast(doc.GetElementsByTagName("MLTDiep")(0), XmlElement)
                            Dim mltdiep As String = element.InnerText
                            element = TryCast(doc.GetElementsByTagName("MTDiep")(0), XmlElement)
                            Dim MTDiep As String = element.InnerText

                            Dim base64phanhoi = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(kqcapma))

                            If kqcapma.Contains("<MLTDiep>204</MLTDiep>") Then
                                If kqcapma.Contains("<LTBao>2</LTBao>") Then

                                    resapi.Macode = 1
                                    resapi.Message = "Gửi cơ quan thuế thành công. CQT đã nhận hóa đơn khởi tạo từ máy tính tiền hợp lệ"
                                    resapi.TransactionID = macode

                                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                    kq = jsonString
                                    Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                                    'kq = 1 ' "Gửi cơ quan thuế thành công. Hóa đơn hợp lệ."
                                Else

                                    'element = TryCast(doc.GetElementsByTagName("DSLDo")(0), XmlElement)
                                    'Dim motaloi As String = element.InnerXml
                                    Dim motaloi As String = String.Empty
                                    Dim rootErr As XmlElement = doc.DocumentElement
                                    Dim nodes As XmlNodeList = rootErr.SelectNodes("/TDiep/DLieu/TBao/DLTBao/LHDMTTien/DSLDo")
                                    Dim lsterr As String = String.Empty
                                    Dim jsonStringerr As String = String.Empty
                                    If nodes.Count > 0 Then
                                        For j As Integer = 0 To nodes.Count - 1
                                            Dim node As XmlNode = nodes(j)
                                            Dim childnodelist As XmlNodeList = node.SelectNodes("//LDo")
                                            If childnodelist.Count > 0 Then
                                                For k As Integer = 0 To childnodelist.Count - 1
                                                    Dim child As XmlNode = childnodelist(k)
                                                    Dim stt As String = String.Empty
                                                    Dim MTLoi As String = String.Empty
                                                    MTLoi = child.SelectSingleNode("MTLoi").InnerText
                                                    If lsterr.Length = 0 Then
                                                        lsterr = MTLoi
                                                    Else
                                                        lsterr = lsterr & "," & MTLoi
                                                    End If
                                                Next
                                            End If
                                        Next
                                    End If
                                    resapi.Macode = 2
                                    resapi.Message = "Đã gửi hóa đơn lên cơ quan thuế"
                                    resapi.Hoadonloi = lsterr
                                    resapi.TransactionID = macode
                                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                                    Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                                    kq = jsonString
                                End If
                            ElseIf kqcapma.Contains("<MLTDiep>999</MLTDiep>") And kqcapma.Contains("<TTTNhan>0</TTTNhan>") Then

                                resapi.Macode = 0
                                resapi.Message = "Đã gửi hóa đơn lên cơ quan thuế. Chưa có kết quả kiểm tra hóa đơn"
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                                Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                                kq = jsonString

                            ElseIf kqcapma.Contains("<MLTDiep>-1</MLTDiep>") Then
                                Dim ele_loi As XmlElement
                                ele_loi = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)

                                resapi.Macode = -1
                                resapi.Message = "Thông điệp không hợp lệ. Phản hồi kỹ thuật -1: " & ele_loi.InnerText
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                                kq = jsonString

                            ElseIf kqcapma.Contains("<MLTDiep>999</MLTDiep>") And kqcapma.Contains("<TTTNhan>1</TTTNhan>") Then
                                Dim ele_loi As XmlElement
                                ele_loi = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)
                                resapi.Macode = -1
                                resapi.Message = "Thông điệp không hợp lệ: " & ele_loi.InnerText
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                                Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                                kq = jsonString

                            Else
                                resapi.Macode = -6
                                resapi.Message = "Chưa có phản hồi của Cơ quan thuế"
                                resapi.TransactionID = macode
                                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                                kq = jsonString
                                Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)

                            End If
                        Else
                            resapi.Macode = -3
                            resapi.Message = "Không gửi được thông điệp lên Cơ quan thuế"
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            kq = jsonString
                            Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                        End If
                    Else
                        resapi.Macode = -4
                        resapi.Message = "Ký thông điệp không thành công"
                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                        kq = jsonString

                        Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                    End If
                Else
                    resapi.Macode = -5
                    resapi.Message = "Không lấy được kết quả ký thông điệp"
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()

                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                    kq = jsonString
                    Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)
                End If
                Return kq
            Catch ex As Exception
                resapi.Macode = -6

                resapi.Message = ex.Message
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()

                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                kq = jsonString
                Call insLogCallService(masothue, serialnumber, "CA2GuiBangke_MTTien", "", jsonString)

            End Try
        Else
            kq = "Lỗi xác thực"
        End If
        Return kq
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
        PBanTTNode.AppendChild(doc.CreateTextNode("2.0.0"))
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
    Private Function LoadCert(ByVal serial As String) As X509Certificate2
        Dim kq As X509Certificate2 = Nothing
        Dim oStore As X509Store = New X509Store(StoreLocation.CurrentUser)
        oStore.Open(OpenFlags.[ReadOnly])
        Dim oCert As X509Certificate2Collection = oStore.Certificates.Find(X509FindType.FindBySerialNumber, serial, False)
        If oCert.Count > 0 Then
            kq = oCert(0)
        End If
        oStore.Close()
        Return kq
    End Function
    Public Function SignXml(ByVal xmlDoc As XmlDocument, ByVal cert As X509Certificate2, mst As String, serialnumber As String) As String

        Dim id As String = String.Empty
        Dim base64 As String = String.Empty
        Dim tagky As String = String.Empty

        Dim pathcert As String = String.Empty
        Try
            If mst = "0103930279-999" Or mst = "0103930279-998" Then
                'pathcert = "D:\Nacencomm-999.pfx"
                pathcert = HttpContext.Current.Server.MapPath("~/Cert/Nacencomm-999.pfx")
                cert.Import(pathcert, "1", X509KeyStorageFlags.Exportable)

            Else
                pathcert = HttpContext.Current.Server.MapPath("~/Cert/") + mst + ".p12"
                ' pathcert = Server.MapPath("~/Cert/" & mst & ".p12")
                cert.Import(pathcert, "12345678", X509KeyStorageFlags.Exportable)
            End If

            Dim serrial_Cert As String = cert.GetSerialNumberString
            If serialnumber.ToUpper.CompareTo(serrial_Cert.ToUpper) = 0 Then
                Dim rsaCSP As RSA = cert.GetRSAPrivateKey()

                If xmlDoc Is Nothing Then Throw New ArgumentException("xmlDoc")
                Dim signedXml As SignedXml = New SignedXml(xmlDoc)

                If cert.HasPrivateKey Then
                    If xmlDoc.InnerXml.Contains("</TTChung><DLieu Id=") Then
                        Dim xmlnodemtt As XmlNodeList = xmlDoc.GetElementsByTagName("DLieu")
                        id = xmlnodemtt(0).Attributes("Id").Value
                        tagky = "CKSNNT"
                    End If

                    Dim xmlnode As XmlNodeList = xmlDoc.GetElementsByTagName("DLTKhai")

                    For i As Integer = 0 To xmlnode.Count - 1
                        id = xmlnode(i).Attributes("Id").Value
                        tagky = "NNT"
                    Next


                    Dim xmlnode1 As XmlNodeList = xmlDoc.GetElementsByTagName("DLHDon")
                    If String.IsNullOrEmpty(tagky) Then
                        For i As Integer = 0 To xmlnode1.Count - 1
                            id = xmlnode1(i).Attributes("Id").Value
                            tagky = "NBan"
                        Next
                    End If

                    Dim xmlnode2 As XmlNodeList = xmlDoc.GetElementsByTagName("DLTBao")

                    For i As Integer = 0 To xmlnode2.Count - 1
                        id = xmlnode2(i).Attributes("Id").Value
                        tagky = "NNT"
                    Next

                    Dim xmlnode3 As XmlNodeList = xmlDoc.GetElementsByTagName("DLBTHop")

                    For i As Integer = 0 To xmlnode3.Count - 1
                        id = xmlnode3(i).Attributes("Id").Value
                        tagky = "NNT"
                    Next

                    Dim cspParams As CspParameters = New CspParameters()
                    cspParams.KeyContainerName = "XML_DSIG_RSA_KEY"
                    'Dim rsakey As RSACryptoServiceProvider = New RSACryptoServiceProvider()


                    If cert.SignatureAlgorithm.FriendlyName = "sha1RSA" Then
                        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url
                        signedXml.SigningKey = cert.PrivateKey

                    Else
                        ' signedXml.SigningKey = cert.GetRSAPrivateKey()
                        signedXml.SigningKey = rsaCSP    ' cert.GetRSAPrivateKey()
                        signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"
                    End If

                    signedXml.Signature.Id = tagky & "-" & id.Replace("_", "")
                    Dim doc As XmlDocument = New XmlDocument()
                    Dim node As XmlElement = doc.CreateElement("SignatureProperties", xmlDoc.NamespaceURI)
                    Dim node1 As XmlElement = doc.CreateElement("SignatureProperty")
                    node1.SetAttribute("Target", "#" & tagky & "-" & id.Replace("_", ""))
                    Dim node2 As XmlNode = doc.CreateNode(XmlNodeType.Element, "", "SigningTime", "")
                    node2.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    node1.AppendChild(node2)
                    node.AppendChild(node1)
                    doc.AppendChild(node)
                    Dim dataObject As DataObject = New DataObject()
                    dataObject.Id = "Obj-" & tagky & "-" & id.Replace("_", "")
                    dataObject.Data = doc.ChildNodes
                    signedXml.AddObject(dataObject)
                    Dim reference As Reference = New Reference()
                    reference.Uri = "#" & id
                    reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256"

                    signedXml.AddReference(reference)
                    Dim reference1 As Reference = New Reference()
                    reference1.Uri = "#Obj-" & tagky & "-" & id.Replace("_", "")
                    reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256"
                    signedXml.AddReference(reference1)
                    Dim keyInfo As KeyInfo = New KeyInfo()
                    Dim keyInfoData As KeyInfoX509Data = New KeyInfoX509Data(cert)
                    keyInfo.AddClause(keyInfoData)
                    keyInfoData.AddSubjectName(cert.Subject)

                    signedXml.KeyInfo = keyInfo
                    Dim signInfo As SignedInfo = New SignedInfo()
                    Dim env As XmlDsigEnvelopedSignatureTransform = New XmlDsigEnvelopedSignatureTransform()
                    reference.AddTransform(env)
                    signedXml.ComputeSignature()
                    Dim xmlDigitalSignature As XmlElement = signedXml.GetXml()
                    Dim abc As XmlNode = xmlDoc.ImportNode(xmlDigitalSignature, True)
                    Dim lstNode As XmlNodeList = xmlDoc.GetElementsByTagName(tagky)

                    For i As Integer = 0 To lstNode.Count - 1
                        Dim xnode As XmlNode = lstNode(lstNode.Count - 1)
                        xnode.AppendChild(xmlDigitalSignature)
                    Next

                    Dim kq As String = xmlDoc.InnerXml
                    Dim data As Byte() = System.Text.Encoding.UTF8.GetBytes(xmlDoc.InnerXml)
                    base64 = Convert.ToBase64String(data)
                End If

            Else
                base64 = "Serial không hợp lệ"
            End If

        Catch ex As Exception
            base64 = ex.Message
        End Try

        Return base64
    End Function
    Public Function IsBase64String(ByVal base64 As String) As Boolean
        base64 = base64.Trim()
        Return (base64.Length Mod 4 = 0) AndAlso Regex.IsMatch(base64, "^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)
    End Function
    Private Function CreatFileXML_Thong_diep_206_CQT(ByVal MNGui As String, ByVal MNNhan As String, ByVal MLTDiep As String, ByVal MTDiep As String,
            ByVal MTDTChieu As String, soluong As String, MSTNBH As String, lstData As List(Of HoadonMTTien)) As String
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
        PBanTTNode.AppendChild(doc.CreateTextNode("2.0.1"))
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

        'Dim jsonhoadon As lsthoadonMTT = JsonConvert.DeserializeObject(Of lsthoadonMTT)(lstData)
        'Dim lstxml As List(Of HoadonMTTien) = jsonhoadon.Lst

        'MST
        Dim MSTTTNode As XmlNode = doc.CreateElement("", "MST", linkelement)
        MSTTTNode.AppendChild(doc.CreateTextNode(MSTNBH))
        TTChungTDNode.AppendChild(MSTTTNode)
        'SLuong
        Dim SLuongNode As XmlNode = doc.CreateElement("", "SLuong", linkelement)
        SLuongNode.AppendChild(doc.CreateTextNode(soluong))
        TTChungTDNode.AppendChild(SLuongNode)
        'DLieu

        Dim DLieuNode As XmlElement = doc.CreateElement("", "DLieu", linkelement)
        Dim productAttribute As XmlAttribute = doc.CreateAttribute("Id")
        productAttribute.Value = "_" & MTDiep
        DLieuNode.Attributes.Append(productAttribute)
        TDiepNode.AppendChild(DLieuNode)

        Dim lstNode As XmlNodeList = doc.GetElementsByTagName("DLieu")

        For i = 0 To lstData.Count - 1
            Dim base64xml As String = lstData(i).Base64XML.ToString
            'add node HDon
            Dim convert = XmlStringToXmlNode(base64xml)
            Dim xnode As XmlNode = lstNode(0)
            xnode.AppendChild(xnode.OwnerDocument.ImportNode(convert, True))

        Next

        '===================end lay thong tin hd==============
        'DLieu
        Dim CKSNNT As XmlElement = doc.CreateElement("", "CKSNNT", linkelement)
        TDiepNode.AppendChild(CKSNNT)

        kq = doc.InnerXml
        Return kq
    End Function
    <WebMethod()>
    Public Function CA2GuiTBSS_HD(MaCQTQL As String, TenCQTQL As String, Diadanh As String, Masothue As String, TenNNT As String,
                                      KHMSHdon As String, KHHDon As String, Sohoadon As String, NgayHD As String, MaCQT As String,
                                  LoaiHDAP As String, TChatTBao As String, Lydo As String, Serialnumber As String)
        Dim resapi As New ResponeAPICapma
        Dim checkauthen As Integer = CheckAuthenWS(Masothue)
        If checkauthen = 1 Then
            If Not String.IsNullOrEmpty(Sohoadon) Then
                Dim xmltbss As String = CreatFileXML_TBSS(MaCQTQL, TenCQTQL, TenNNT, Masothue, Diadanh, DateTime.Now.ToString("yyyy-MM-dd"), MaCQT, KHMSHdon, KHHDon, Sohoadon, NgayHD, LoaiHDAP, TChatTBao, Lydo)
                Dim xmldoc As XmlDocument = New XmlDocument
                xmldoc.LoadXml(xmltbss)
                Dim cert As X509Certificate2 = New X509Certificate2
                ' cert = LoadCert(Serialnumber)
                Dim kqky As String = SignXml(xmldoc, cert, Masothue, Serialnumber)
                If String.IsNullOrEmpty(kqky) Then
                    resapi.Macode = -4
                    resapi.Message = "Ký thông báo sai sót không thành công"
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                    Return jsonString
                    Exit Function
                End If
                Dim checkkhieu As String = KHHDon.Substring(3, 1)
                Dim thongdiep As String = String.Empty
                Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
                Dim key As String = "0103930279" & guidstr.Replace("-", "")

                If checkkhieu = "T" Then
                    thongdiep = CreatFileXML_TDiepguiTCT("0103930279", "0103930279", "300", key, "", 1, Masothue, kqky)
                ElseIf checkkhieu = "M" Then
                    thongdiep = CreatFileXML_TDiepguiTCT("0103930279", "0103930279", "303", key, "", 1, Masothue, kqky)
                End If

                Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(thongdiep))
                Dim servicetvan As ServTVAN.WSInterTRCA2 = New ServTVAN.WSInterTRCA2()
                Dim ttketnoi As New ServTVAN.AuthHeader
                'Dim servicetvan As servicetvan78uat.WSInterTRCA2 = New servicetvan78uat.WSInterTRCA2()
                'Dim ttketnoi As New servicetvan78uat.AuthHeader
                ttketnoi.Username = madvauthen
                ttketnoi.Password = passauthen
                servicetvan.AuthHeaderValue = ttketnoi

                Dim macode As String = servicetvan.Guithongdiep_MTDiep(base64thongdiep, 1)
                If macode.Length > 2 Then
                    Dim kquaCQT As String = String.Empty

                    Dim khoaphientn As String = macode.Split("|")(0)
                    Dim mtdieptvan As String = macode.Split("|")(1)
                    Dim ngaytdiep As String = Now.ToString("yyyyMMdd")

                    'While kquaCQT = ""
                    '    Thread.Sleep(5000)
                    '    kquaCQT = servicetvan.LayKQThongdiep(macode, "0110400071")
                    'End While
                    Dim counttime As Integer = 0
                    Dim kqcapma As String = String.Empty
                    Dim wsKafkaconsumer As wsKafka.ResponseKafka = New wsKafka.ResponseKafka
                    While kquaCQT = ""

                        kquaCQT = wsKafkaconsumer.GetResp_Kafka(mtdieptvan, ngaytdiep)
                        Thread.Sleep(1000)
                        If counttime >= 10 Then
                            Exit While
                        End If
                    End While

                    If kquaCQT = "-1" Or kquaCQT = "-5" Or kquaCQT = "-6" Or kquaCQT = "-7" Then
                        'loi
                        ' res = "Không lấy được phản hồi của CQT: " & kqcapma
                        resapi.Macode = -2
                        resapi.Message = "Không lấy được phản hồi của CQT: " & kquaCQT
                        resapi.Masothue = Masothue
                        resapi.KHMSHDon = KHMSHdon
                        resapi.KHHDon = KHHDon
                        resapi.Sohoadon = Sohoadon
                        resapi.Motaloi = kquaCQT
                        resapi.TransactionID = macode
                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                        Call insLogCallService(Masothue, Serialnumber, "CA2GuiTBSS_HD", KHMSHdon & KHHDon & Sohoadon & "-" & MaCQT & "-" & TChatTBao, jsonString)
                        Return jsonString
                    Else

                        Dim doc As XmlDocument = New XmlDocument()
                        doc.LoadXml(kquaCQT)
                        Dim root As XmlElement = doc.DocumentElement
                        Dim nodelist_DLieu As XmlNodeList = root.GetElementsByTagName("DLieu")
                        Dim element As XmlElement
                        element = TryCast(doc.GetElementsByTagName("MLTDiep")(0), XmlElement)
                        Dim mltdiep As String = element.InnerText

                        element = TryCast(doc.GetElementsByTagName("TTTNCCQT")(0), XmlElement)
                        Dim ttxn As String = element.InnerText

                        If mltdiep = "301" And ttxn = "1" Then
                            'kq = 1
                            resapi.Macode = 1
                            resapi.Message = "CQT đã duyệt thông báo sai sót"
                            resapi.Masothue = Masothue
                            resapi.KHMSHDon = KHMSHdon
                            resapi.KHHDon = KHHDon
                            resapi.Sohoadon = Sohoadon
                            resapi.TransactionID = macode
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(Masothue, Serialnumber, "CA2GuiTBSS_HD", KHMSHdon & KHHDon & Sohoadon & "-" & MaCQT & "-" & TChatTBao, jsonString)
                            Return jsonString
                            Exit Function

                        ElseIf mltdiep = "301" And ttxn = "2" Then
                            'kq = 0
                            element = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)
                            resapi.Macode = 2
                            resapi.Message = "Cơ quan thuế đã từ chối duyệt thông báo sai sót"
                            resapi.Masothue = Masothue
                            resapi.KHMSHDon = KHMSHdon
                            resapi.KHHDon = KHHDon
                            resapi.Sohoadon = Sohoadon
                            resapi.Motaloi = element.InnerText
                            resapi.TransactionID = macode

                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(Masothue, Serialnumber, "CA2GuiTBSS_HD", KHMSHdon & KHHDon & Sohoadon & "-" & MaCQT & "-" & TChatTBao, jsonString)
                            Return jsonString
                            Exit Function

                        ElseIf mltdiep = "-1" Then

                            Dim element2 As XmlElement
                            element2 = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)
                            resapi.Macode = -1
                            resapi.Message = "Thông điệp không hợp lệ.  Phản hồi kỹ thuật -1"
                            resapi.Masothue = Masothue
                            resapi.KHMSHDon = KHMSHdon
                            resapi.KHHDon = KHHDon
                            resapi.Sohoadon = Sohoadon
                            resapi.Motaloi = element2.InnerText
                            resapi.TransactionID = macode
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(Masothue, Serialnumber, "CA2GuiTBSS_HD", KHMSHdon & KHHDon & Sohoadon & "-" & MaCQT & "-" & TChatTBao, jsonString)
                            Return jsonString
                            Exit Function
                        Else

                            resapi.Macode = 3
                            resapi.Message = "Đã gửi TBSS lên CQT. Chưa có phản hồi kết quả"
                            resapi.Masothue = Masothue
                            resapi.KHMSHDon = KHMSHdon
                            resapi.KHHDon = KHHDon
                            resapi.Sohoadon = Sohoadon
                            resapi.TransactionID = macode
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(Masothue, Serialnumber, "CA2GuiTBSS_HD", KHMSHdon & KHHDon & Sohoadon & "-" & MaCQT & "-" & TChatTBao, jsonString)
                            Return jsonString
                            Exit Function
                        End If
                    End If
                Else

                    resapi.Macode = -3
                    resapi.Message = "Không gửi được thông điệp lên cơ quan thuế"
                    resapi.Masothue = Masothue
                    resapi.KHMSHDon = KHMSHdon
                    resapi.KHHDon = KHHDon
                    resapi.Sohoadon = Sohoadon
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                    Call insLogCallService(Masothue, Serialnumber, "CA2GuiTBSS_HD", KHMSHdon & KHHDon & Sohoadon & "-" & MaCQT & "-" & TChatTBao, jsonString)
                    Return jsonString
                    Exit Function
                End If
            Else
                resapi.Macode = 0
                resapi.Message = "Không có số hóa đơn"
                resapi.Masothue = Masothue
                resapi.KHMSHDon = KHMSHdon
                resapi.KHHDon = KHHDon
                resapi.Sohoadon = Sohoadon
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                Call insLogCallService(Masothue, Serialnumber, "CA2GuiTBSS_HD", KHMSHdon & KHHDon & Sohoadon & "-" & MaCQT & "-" & TChatTBao, jsonString)
                Return jsonString
                Exit Function
            End If
        Else
            Return "Lỗi xác thực" '
        End If

    End Function
    Private Function CreatFileXML_TBSS(ByVal MCQT As String, ByVal TCQT As String, ByVal TNNT As String, ByVal MST As String, ByVal DDanh As String, ByVal NTBao As String, ByVal MCQTCap As String, KHMSHDon As String, khhdon As String, Sohd As String, NgayHD As String, ByVal LADHDDT As String, ByVal TCTBao As String, ByVal LDo As String) As String
        Dim kq As String = ""
        'Tao thong tin XML chung

        Dim linkelement As String = ""
        Dim doc As XmlDocument = New XmlDocument()
        Dim docNode As XmlNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "")
        doc.AppendChild(docNode)

        'The TBao
        Dim TBaoNode As XmlElement = doc.CreateElement("", "TBao", linkelement)
        doc.AppendChild(TBaoNode)
        Dim sId As String
        sId = System.Guid.NewGuid.ToString()

        'The DLTBao
        Dim DLTBaoNode As XmlNode = doc.CreateElement("", "DLTBao", linkelement)
        Dim productAttribute As XmlAttribute = doc.CreateAttribute("Id")
        productAttribute.Value = "_" & sId
        DLTBaoNode.Attributes.Append(productAttribute)
        TBaoNode.AppendChild(DLTBaoNode)

        'PBan
        Dim PBanNode As XmlNode = doc.CreateElement("", "PBan", linkelement)
        PBanNode.AppendChild(doc.CreateTextNode("2.0.1"))
        DLTBaoNode.AppendChild(PBanNode)
        'MSo
        Dim MSoNode As XmlNode = doc.CreateElement("", "MSo", linkelement)
        MSoNode.AppendChild(doc.CreateTextNode("04/SS-HĐĐT"))
        DLTBaoNode.AppendChild(MSoNode)
        'Ten
        Dim TenNode As XmlNode = doc.CreateElement("", "Ten", linkelement)
        TenNode.AppendChild(doc.CreateTextNode("Thông báo hóa đơn điện tử có sai sót"))
        DLTBaoNode.AppendChild(TenNode)
        'Loai
        Dim LoaiNode As XmlNode = doc.CreateElement("", "Loai", linkelement)
        LoaiNode.AppendChild(doc.CreateTextNode("1"))
        DLTBaoNode.AppendChild(LoaiNode)

        'MCQT

        Dim MCQTNode As XmlNode = doc.CreateElement("", "MCQT", linkelement)
        MCQTNode.AppendChild(doc.CreateTextNode(MCQT))
        DLTBaoNode.AppendChild(MCQTNode)


        'TCQT
        Dim TCQTNode As XmlNode = doc.CreateElement("", "TCQT", linkelement)
        TCQTNode.AppendChild(doc.CreateTextNode(TCQT))
        DLTBaoNode.AppendChild(TCQTNode)
        'TNNT
        Dim TNNTNode As XmlNode = doc.CreateElement("", "TNNT", linkelement)
        TNNTNode.AppendChild(doc.CreateTextNode(TNNT))
        DLTBaoNode.AppendChild(TNNTNode)
        'MST
        Dim MSTNode As XmlNode = doc.CreateElement("", "MST", linkelement)
        MSTNode.AppendChild(doc.CreateTextNode(MST))
        DLTBaoNode.AppendChild(MSTNode)


        'DDanh
        Dim DDanhNode As XmlNode = doc.CreateElement("", "DDanh", linkelement)
        DDanhNode.AppendChild(doc.CreateTextNode(DDanh))
        DLTBaoNode.AppendChild(DDanhNode)
        'NTBao
        Dim NTBaoNode As XmlNode = doc.CreateElement("", "NTBao", linkelement)
        NTBaoNode.AppendChild(doc.CreateTextNode(NTBao))
        DLTBaoNode.AppendChild(NTBaoNode)
        'DSHDon
        Dim DSHDonNode As XmlNode = doc.CreateElement("", "DSHDon", linkelement)
        DLTBaoNode.AppendChild(DSHDonNode)

        'HDon
        Dim HDonNode As XmlNode = doc.CreateElement("", "HDon", linkelement)
        DSHDonNode.AppendChild(HDonNode)
        'STT
        Dim STTNode As XmlNode = doc.CreateElement("", "STT", linkelement)
        STTNode.AppendChild(doc.CreateTextNode("1"))
        HDonNode.AppendChild(STTNode)

        'KHMSHDon
        Dim KHMSHDonNode As XmlNode = doc.CreateElement("", "KHMSHDon", linkelement)
        KHMSHDonNode.AppendChild(doc.CreateTextNode(KHMSHDon))
        HDonNode.AppendChild(KHMSHDonNode)
        'KHHDon
        Dim KHHDonNode As XmlNode = doc.CreateElement("", "KHHDon", linkelement)
        KHHDonNode.AppendChild(doc.CreateTextNode(khhdon))
        HDonNode.AppendChild(KHHDonNode)
        'SHDon
        Dim SHDonNode As XmlNode = doc.CreateElement("", "SHDon", linkelement)
        SHDonNode.AppendChild(doc.CreateTextNode(Sohd))
        HDonNode.AppendChild(SHDonNode)
        'Ngay
        Dim NgayNode As XmlNode = doc.CreateElement("", "Ngay", linkelement)
        NgayNode.AppendChild(doc.CreateTextNode(NgayHD))
        HDonNode.AppendChild(NgayNode)
        'MCQTCap

        Dim MCQTCapNode As XmlNode = doc.CreateElement("", "MCCQT", linkelement)
        MCQTCapNode.AppendChild(doc.CreateTextNode(MCQTCap))
        HDonNode.AppendChild(MCQTCapNode)
        'LADHDDT
        Dim LADHDDTNode As XmlNode = doc.CreateElement("", "LADHDDT", linkelement)
        LADHDDTNode.AppendChild(doc.CreateTextNode(LADHDDT))
        HDonNode.AppendChild(LADHDDTNode)
        'TCTBao
        Dim TCTBaoNode As XmlNode = doc.CreateElement("", "TCTBao", linkelement)
        TCTBaoNode.AppendChild(doc.CreateTextNode(TCTBao))
        HDonNode.AppendChild(TCTBaoNode)

        'LDo
        If Not String.IsNullOrEmpty(LDo) Then
            Dim LDoNode As XmlNode = doc.CreateElement("", "LDo", linkelement)
            LDoNode.AppendChild(doc.CreateTextNode(LDo))
            HDonNode.AppendChild(LDoNode)
        End If

        ' DS CKS
        Dim DSCKSNode As XmlNode = doc.CreateElement("", "DSCKS", linkelement)
        TBaoNode.AppendChild(DSCKSNode)
        ' NNT CKS

        Dim NNTCKSNode As XmlNode = doc.CreateElement("", "NNT", linkelement)
        DSCKSNode.AppendChild(NNTCKSNode)
        'NMua CKS
        Dim CCKSKhacCKSNode As XmlNode = doc.CreateElement("", "CCKSKhac", linkelement)
        DSCKSNode.AppendChild(CCKSKhacCKSNode)
        kq = doc.InnerXml
        Return kq
    End Function
    Private Function CreatFileXML_TDiepguiTCT(ByVal MNGui As String, ByVal MNNhan As String, ByVal MLTDiep As String, ByVal MTDiep As String, ByVal MTDTChieu As String, ByVal SLuong As String, ByVal MST As String, ByVal strChuoiHoaDon As String) As String
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
        PBanTTNode.AppendChild(doc.CreateTextNode("2.0.0"))
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

    <WebMethod>
    Public Function CA2KyGuitokhaiDK_TCT(base64xmltokhai As String, serialno As String, masothue As String)
        Dim resapi As New ResponeTokhai
        Dim checkauthen As Integer = CheckAuthenWS(masothue)
        If checkauthen = 1 Then

            If IsBase64String(base64xmltokhai) = False Then
                resapi.Macode = -4
                resapi.Message = "Sai định dạng dữ liệu chuỗi đầu vào"
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)

                Return jsonString
                Exit Function
            End If

            Dim xmltokhai As String = Encoding.UTF8.GetString(Convert.FromBase64String(base64xmltokhai))

            Dim xmldoc As XmlDocument = New XmlDocument
            xmldoc.LoadXml(xmltokhai)
            Dim cert As X509Certificate2 = New X509Certificate2
            ' cert = LoadCert(serialno)
            Dim kqky As String = SignXml(xmldoc, cert, masothue, serialno)
            If String.IsNullOrEmpty(kqky) Then
                resapi.Macode = -3
                resapi.Message = "Ký thông tờ khai không thành công"
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                Call insLogCallService(masothue, serialno, "CA2KyGuitokhaiDK_TCT", base64xmltokhai, jsonString)
                Return jsonString
                Exit Function
            End If

            Dim guidstr As String = System.Guid.NewGuid.ToString().ToUpper
            Dim key As String = "0103930279" & guidstr.Replace("-", "")

            Dim thongdiep As String = String.Empty
            thongdiep = CreatFileXML_TDiepguiTCT("0103930279", "0103930279", "100", key, "", 1, masothue, kqky)

            Dim base64thongdiep = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(thongdiep))
            Dim servicetvan As ServTVAN.WSInterTRCA2 = New ServTVAN.WSInterTRCA2()
            Dim ttketnoi As New ServTVAN.AuthHeader

            'Dim servicetvan As servicetvan78uat.WSInterTRCA2 = New servicetvan78uat.WSInterTRCA2()
            'Dim ttketnoi As New servicetvan78uat.AuthHeader

            ttketnoi.Username = madvauthen
            ttketnoi.Password = passauthen
            servicetvan.AuthHeaderValue = ttketnoi

            Dim macode As String = servicetvan.Guithongdiep_MTDiep(base64thongdiep, 1)
            If macode.Length > 2 Then
                Dim kquaCQT As String = String.Empty

                Dim khoaphientn As String = macode.Split("|")(0)
                Dim mtdieptvan As String = macode.Split("|")(1)
                Dim ngaytdiep As String = Now.ToString("yyyyMMdd")

                Dim kqphanhoi As String = String.Empty

                'While kqphanhoi = ""
                '    Thread.Sleep(2000)
                '    kqphanhoi = servicetvan.LayKQThongdiep(khoaphientn, mstgp)
                'End While
                Dim counttime As Integer = 0

                Dim wsKafkaconsumer As wsKafka.ResponseKafka = New wsKafka.ResponseKafka
                While kqphanhoi = ""
                    kqphanhoi = wsKafkaconsumer.GetResp_Kafka(mtdieptvan, ngaytdiep)
                    Thread.Sleep(1000)
                    If counttime >= 10 Then
                        Exit While
                    End If
                End While

                If kqphanhoi.Length > 2 Then
                    Dim doc As XmlDocument = New XmlDocument()
                    doc.LoadXml(kqphanhoi)
                    Dim element As XmlElement
                    element = TryCast(doc.GetElementsByTagName("MLTDiep")(0), XmlElement)
                    Dim mltdiep As String = element.InnerText
                    Dim element_THop As XmlElement
                    element_THop = TryCast(doc.GetElementsByTagName("THop")(0), XmlElement)
                    Dim THop As String = element_THop.InnerText
                    If mltdiep = "102" Then
                        If THop = "1" Or THop = "3" Then

                            resapi.Macode = 102
                            resapi.Message = "CQT đã tiếp nhận tờ khai của NNT, đang chờ xử lý"
                            resapi.TransactionID = khoaphientn
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(masothue, serialno, "CA2KyGuitokhaiDK_TCT", base64xmltokhai, jsonString)
                            Return jsonString
                        Else
                            Dim element_mtaloi As XmlElement
                            element_mtaloi = TryCast(doc.GetElementsByTagName("MTa")(0), XmlElement)
                            resapi.Macode = -102
                            resapi.Message = "CQT không tiếp nhận tờ khai của NNT"
                            resapi.Motaloi = element_mtaloi.InnerText
                            resapi.TransactionID = khoaphientn
                            Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                            Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                            Call insLogCallService(masothue, serialno, "CA2KyGuitokhaiDK_TCT", base64xmltokhai, jsonString)
                            Return jsonString
                        End If
                    Else
                        resapi.Macode = 0
                        resapi.Message = "Chưa có kết quả xử lý tờ khai từ CQT"
                        resapi.TransactionID = khoaphientn
                        Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                        Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                        Call insLogCallService(masothue, serialno, "CA2KyGuitokhaiDK_TCT", base64xmltokhai, jsonString)
                        Return jsonString
                    End If
                Else
                    resapi.Macode = -1
                    resapi.Message = "Chưa có kết quả phản hồi từ CQT"
                    resapi.TransactionID = khoaphientn
                    Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                    Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                    Call insLogCallService(masothue, serialno, "CA2KyGuitokhaiDK_TCT", base64xmltokhai, jsonString)
                    Return jsonString
                End If
            Else
                resapi.Macode = -2
                resapi.Message = "Không gửi được tờ khai lên cơ quan thuế." & macode
                Dim javaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim jsonString As String = javaScriptSerializer.Serialize(resapi)
                Call insLogCallService(masothue, serialno, "CA2KyGuitokhaiDK_TCT", base64xmltokhai, jsonString)
                Return jsonString
            End If

        Else
            Return "Lỗi xác thực"
        End If
    End Function

    <WebMethod>
    Public Function CheckCertRevoked(base64Cert As String) As String
        base64Cert = base64Cert.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "")
        Dim Cert As X509Certificate2 = New X509Certificate2
        Cert.Import(Convert.FromBase64String(base64Cert.ToString()))
        Dim checkrevoked As String = String.Empty
        Dim ch As X509Chain = New X509Chain()
        ch.ChainPolicy.RevocationMode = X509RevocationMode.Online
        ch.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot
        ch.ChainPolicy.UrlRetrievalTimeout = New TimeSpan(1000)
        ch.ChainPolicy.VerificationTime = DateTime.Now
        ch.Build(Cert)
        If ch.ChainStatus.Length = 0 Then
            checkrevoked = "Certificate is valid"
        Else
            For Each status As X509ChainStatus In ch.ChainStatus
                If status.Status = X509ChainStatusFlags.Revoked Then
                    checkrevoked = "Certificate has revoked"
                ElseIf status.Status = X509ChainStatusFlags.OfflineRevocation Then
                    checkrevoked = "The certificate revocation list (CRL) being offline"
                ElseIf status.Status = X509ChainStatusFlags.UntrustedRoot Then
                    checkrevoked = "Root Certificate untrusted"
                ElseIf status.Status = X509ChainStatusFlags.RevocationStatusUnknown Then
                    checkrevoked = "The certificate revocation list (CRL) being offline or unavailable"
                End If
            Next
        End If


        ''CHECK WITH CHILKAT

        ''Dim http As New Chilkat.Http
        ''Dim status As Integer = 0
        ''Dim thongbao As String = String.Empty
        ''Dim cert As New Chilkat.Cert
        ''Dim success As Boolean = cert.LoadFromBase64(base64Cert)

        ''Dim ocspUrl As String = cert.OcspUrl
        '' The status can have 4 values:
        '' -1: Unable to check because of an error.
        ''  0: Good
        ''  1: Revoked
        ''  2: Unknown
        ''status = http.OcspCheck(ocspUrl, 443)
        ''If (status < 0) Then
        ''    thongbao = "Unable to check because of an error"
        ''ElseIf status = 0 Then
        ''    thongbao = "Good"
        ''ElseIf status = 1 Then
        ''    thongbao = "Revoked"
        ''ElseIf status = 2 Then
        ''    thongbao = "Unknown"
        ''End If

        ''Dim checkrevoked As Integer = cert.CheckRevoked()

        Return checkrevoked

    End Function

    <WebMethod>
    Public Function CheckCertExpired(base64Cert As String) As String
        Try
            base64Cert = base64Cert.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Replace("\r\n", "")
            Dim Cert As X509Certificate2 = New X509Certificate2()
            Cert.Import(Convert.FromBase64String(base64Cert))

            'Dim ch As X509Chain = New X509Chain()
            'ch.ChainPolicy.RevocationMode = X509RevocationMode.Online
            'ch.Build(Cert)
            'Dim rawdata As Byte() = Cert.RawData

            Dim expStatus As Boolean = False
            expStatus = Cert.Verify
            Return expStatus
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function



    Public Class ListSHD
        Private Shared NextID As Integer = 1
        Public Property ProductID As Integer
        Public Property KHMS As String
        Public Property KHHD As String
        Public Property SHD As String
        Public Property NgayHD As String
        Public Property MaCQT As String
        Public Sub New()
            ProductID = NextID
            NextID += 1
        End Sub
    End Class

    Public Class ResponeKyso
        Public Property Macode As Integer
        Public Property Message As String

        Public Property SignedData As String

    End Class

    Public Class ResponeAPI
        Public Property Macode As Integer
        Public Property Message As String

    End Class

    Public Class ResponeAPICapma
        Public Property Macode As Integer
        Public Property Message As String

        Public Property Masothue As String
        Public Property KHMSHDon As String
        Public Property KHHDon As String
        Public Property Sohoadon As String

        Public Property MCCQT As String
        Public Property Motaloi As String
        Public Property TransactionID As String
        Public Property XMLComaCQT As String
    End Class
    Public Class ResponeListMTTien
        Public Property Macode As Integer
        Public Property Message As String
        Public Property Hoadonloi As String

        Public Property TransactionID As String

    End Class

    Public Class ResponeTokhai
        Public Property Macode As Integer
        Public Property Message As String
        Public Property Motaloi As String
        Public Property TransactionID As String

    End Class
    Public Class HoadonMTTien
        Public Property Base64XML As String
    End Class

    Public Class lsthoadonMTT
        Public Property Lst As List(Of HoadonMTTien)
    End Class
    Public Class ttTaikhoan
        Public Property Username As String
        Public Property passwword As String
        Public Property MST As String
        Public Property isActive As Integer

    End Class
End Class



