Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

''' <summary>
''' Summary description for clDLCTu
''' </summary>
Public Class clDLCTu
    Public Class ThongtintokhaiCT
        Public Property PBan As String
        Public Property MSo As String
        Public Property Ten As String
        Public Property HThuc As Integer
        Public Property TNNT As String
        Public Property MST As String
        Public Property CQTQLy As String
        Public Property MCQTQLy As String
        Public Property NLHe As String
        Public Property DCLHe As String
        Public Property DCTDTu As String
        Public Property DTLHe As String
        Public Property DDanh As String
        Public Property NLap As String
        Public Property TCCNPHanh As Integer
        Public Property CQTPHanh As Integer
        Public Property CTTNCNhan As Integer
        Public Property CTKTTTMDTu As Integer
        Public Property BLTPLPKIn As Integer
        Public Property BLTPLPIn As Integer
        Public Property BLTTPLPhi As Integer
        Public Property CDLQCCQT As Integer
        Public Property CDLQTCTN As Integer
        Public Property CDLQTCTNUT As Integer
        Public Property SerialNo As String
        Public Property Taikhoan As String
        Public Property KhoaphienTN As String
        Public Property co_quan_thue_id As String

    End Class

    Public Class ThongtinCT_CTS
        Public Property STT As Integer
        Public Property TTChuc As String
        Public Property Seri As String
        Public Property TNgay As String
        Public Property DNgay As String
        Public Property HThuc As Integer
    End Class

    Public Class ThongtinCT_DKUNhiem
        Public Property LDKUNhiem As Integer
        Public Property STT As Integer
        Public Property TLCTu As String
        Public Property KHMCTu As String
        Public Property KHCTu As String
        Public Property MST As String
        Public Property TTChuc As String
        Public Property MDich As String
        Public Property TNgay As String
        Public Property DNgay As String
        Public Property PThuc As String
    End Class

    Public Class thongtindv
        Public Property madv As String
        Public Property tendv As String
        Public Property diachi As String
        Public Property dienthoai As String
        Public Property email As String

    End Class


    Public Class KQCapnhatsoCT
        Public Property Trangthaicapnhat As String
        Public Property XMLChungtucoso As String
        Public Property Mota As String

    End Class



    Public Class thongtinCT

        Public Property MaCT As String
        Public Property TenCT As String
        Public Property MSChungtu As String
        Public Property KHChungtu As String
        Public Property Sochungtu As String
        Public Property NgaylapCT As String
        Public Property TinhchatCT As String

        Public Property LoaiCTLienquan As String
        Public Property KHMSCTLienquan As String
        Public Property KHCTLienquan As String
        Public Property SoCTLienquan As String
        Public Property NgaylapCTLienquan As String

        Public Property TenTC As String
        Public Property MasothueTC As String
        Public Property DiachiTC As String

        Public Property DienthoaiTC As String
        Public Property TenNNT As String
        Public Property MasothueNNT As String
        Public Property DiachiNNT As String
        Public Property QuoctichNNT As String
        Public Property CanhanCT As String
        Public Property SoCMND As String
        Public Property NgaycapCMND As String

        Public Property NoicapCMND As String
        Public Property DienthoaiNNT As String
        Public Property EmailNNT As String
        Public Property ThunhapCN As String
        Public Property ThangTN As String

        Public Property Denthang As String
        Public Property NamTN As String
        Public Property TongTNChiuthue As Double
        Public Property TongTNTinhthue As Double
        Public Property ThueTNCN As Double

        Public Property Baohiem As Double

        Public Property SoTNDN As Double
        Public Property TThien As Double


    End Class


    Public Class mauhienthi
        Public name As String
        Public Filepath As String
    End Class

    Public Class DonViInfo
        Public Property MaDV As String
        Public Property TenDV As String
        Public Property DiaChi As String
        Public Property DienThoai As String
        Public Property Fax As String
        Public Property NganHang As String
        Public Property STK As String
        Public Property Email As String
    End Class


    Public Class ValidatedRowData
        Public tennnt As String = String.Empty
        Public mstnguoinnt As String = String.Empty
        Public diachinnt As String = String.Empty
        Public dienthoainnt As String = String.Empty
        Public emailnnt As String = String.Empty
        Public cmndnnt As String = String.Empty
        Public ngaycap As String = String.Empty
        Public noicap As String = String.Empty
        Public thunhaptuthang As String = String.Empty
        Public thunhapdenthang As String = String.Empty
        Public nam As String = String.Empty
        Public quoctich As String = String.Empty
        Public khoanthunhap As String = String.Empty
        Public canhancutru As String = String.Empty
        Public tongthunhapchiuthue As String = String.Empty
        Public tongthunhaptinhthue As String = String.Empty
        Public thuetncn As String = String.Empty
        Public baohiem As String = String.Empty
        Public sothunhapdn As String = String.Empty
        Public tthien As String = String.Empty
    End Class


    Public Class ThongtinTBSSChungtu
        Public Property PBan As String
        Public Property MSo As String
        Public Property Ten As String
        Public Property Loai As Integer
        Public Property So As String
        Public Property NTBCCQT As String
        Public Property MCQT As String
        Public Property TCQT As String
        Public Property TNNT As String
        Public Property MST As String
        Public Property DDanh As String
        Public Property NTBao As String
        Public Property SerialNo As String
        Public Property Taikhoan As String
    End Class

    Public Class TTChungtulapsschitiet
        Public Property STT As Integer
        Public Property KHMSCTu As String
        Public Property KHCTu As String
        Public Property SCTu As String
        Public Property NLap As String
        Public Property LCTDT As Integer
        Public Property LDo As String
    End Class

    Public Class CTGocResult
        Public Property MaCT As String
        Public Property PhanbietCT As String
        Public Property TinhtrangCT As String
        Public Property TinhchatCT As String
        Public Property TenTC As String
        Public Property SoCMND As String
        Public Property DiachiTC As String
        Public Property MasothueTC As String
        Public Property Tenchungtu As String
        Public Property NgaylapCT As String
        Public Property ThunhapCN As String
        Public Property Baohiem As String
        Public Property TongTNChiuthue As String
        Public Property TongTNTinhthue As String
        Public Property ThueTNCN As String
        Public Property TThien As String
        Public Property ThangTN As String
        Public Property Denthang As String
        Public Property NamTN As String
        Public Property CanhanCT As String

        Public Property MasothueNNT As String
        Public Property DiachiNNT As String
        Public Property TenNNT As String
        Public Property QuoctichNNT As String
        Public Property EmailNNT As String
        Public Property DienthoaiNNT As String

        Public Property SLCTu As String



    End Class

    Public Class Root
        Public Property status As String
        Public Property message As String
        Public Property data As List(Of thongtinCT)
    End Class
End Class

