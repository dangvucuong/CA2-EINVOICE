<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ex="http://exslt.org/dates-and-times" xmlns:fn="http://www.w3.org/2005/02/xpath-functions" xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
    <xsl:output method="html" />
    <xsl:param name="imgLogo" />
    <xsl:param name="percent" select="''" />
    <xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
    <xsl:template match="CTu">
        <xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
        <xsl:variable name="MTCuu" select="DLCTu/TTChung/TTKhac/TTin[TTruong='MTCuu']/DLieu" />
        <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
            <head>
                <title>E-Invoice</title>
                <meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8' />
                <style type="text/css">
                #tblContent td,
                #tblContent th {
                    border: 1px dotted gray;
                    padding: 0;
                    margin: 0;
                }

                table {
                    border-collapse: collapse;
line-height: 1.7;
                }

                .bg {
                    background: url(Background.png) no-repeat center center fixed;
                    -webkit-background-size: cover;
                    -moz-background-size: cover;
                    -o-background-size: cover;
                    background-size: cover;
                    background-attachment: fixed;
                }

                .style1 {
                    background: url('Background.png');
                    background-repeat: no-repeat;
                    background-position: center;
                    -webkit-background-size: cover;
                    -moz-background-size: cover;
                    -o-background-size: cover;
                    background-size: cover;

                }

                du {
                    letter-spacing: 5px;
                }

                #watermark {
                    opacity: 0.2;
                    font-size: 52px;
                    color: 'black';
                    background: '#ccc';
                    position: absolute;
                    cursor: default;
                    user-select: none;
                    -webkit-user-select: none;
                    -khtml-user-select: none;
                    -moz-user-select: none;
                    -ms-user-select: none;
                    right: 205px;
                    bottom: 525px;
                }

                i {
                    font-size: 11pt;
                }

                @page {
                    size: A4;
                    max-width: 210mm;
                    margin: auto;
                                }

                @media print {

                    html,
                    body {
                        width: 100%;
                    height: 100%;
                    margin: 0 auto;
                    padding: 10;
                    }
                }
            </style>
            </head>
            <body style="font-family:Times New Roman; font-size: 13pt;line-height: 28px;">
                <!--<div style="viewstyle;width:100%">-->
                <div style="viewstyle;border:none;    background-size: 100%; background-color: hsla(0,0%,100%,0.60);background-position: 50% 56%!important;">
                    <div id="background" style="paramMau">
              MẪU
            </div>
                    <div id="background" style="paramdisable;width: 190px;height: 125px;top:50%;font-size: 60pt">contentDisable</div>
                    <div style="border:1px solid #3c73b3;width:850px; min-height: 297mm;z-index:1;">
                        <!-- <div style="border:2px solid black;width:850px;border-image:url(data:image/jpeg;base64,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) 30 round;border-width:20px;z-index:1"> -->
                        <!-- <table width="100%" style="font-weight:bold;line-height: 28px;">
                            <tr>
                                <td width="75% "></td>
                            </tr>
                        </table> -->
                        <table width="100%" style="font-weight:bold;font-size:12pt;line-height: 28px;">
                            <tr>
                                <td width="30%" style="padding-left:20px;text-transform: uppercase;" align="center">
                                    <br>
                                        <img style="width:220px;height:110px;align-content:center;position:static;left:0;top:0;object-fit: scale-down;"
                                      id="imgSample" src="paramLogo">
            </img>
                                    </br>
                                </td>
                                <td width="40%" align="center" style="font-size:12pt;padding-top:5px">
                                    <br /> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
                                    <br />
                                Độc lập - Tự do - Hạnh phúc
                                    <br />
                                   
                                ---------------
                                    <br />
                                </td>
                                <td width="30% " style="padding-left:20px;font-size:12pt;padding-top:20px;">
                                    <!-- <xsl:value-of select="DLHDon/TTChung/KHMSHDon" /> -->
                
                               Mẫu số
                                    <b>
                                        <xsl:value-of select="DLCTu/TTChung/MSCTu" />
                                    </b>
                                    <br />
                    Ký hiệu :
                                    <b>
                                        <xsl:value-of select="DLCTu/TTChung/KHCTu" />
                                    </b>
                                    <br />
                    Số :
                                    <span style="color: red;font-size:13pt">
                                        <xsl:value-of select="DLCTu/TTChung/SCTu" />
                                    </span>
                                    <br/>
                                    <div style="color: red; font-size: 10pt;padding-top:5px;  display: normal;text-align:left">
                                        <div style="paramChuyendoi">
                        CHỨNG TỪ CHUYỂN ĐỔI
                                            <br />
                        TỪ CHỨNG TỪ ĐIỆN TỬ
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <xsl:choose>
                            <xsl:when test="DLCTu/TTChung/TTCTLQuan!=''">
                                <div
                                        style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt">Chứng từ
                                    <xsl:if test="DLCTu/TTChung/TTCTLQuan/TCCTu=1">thay thế</xsl:if>
                                    <xsl:if test="DLCTu/TTChung/TTCTLQuan/TCCTu=2">điều chỉnh</xsl:if> cho chứng từ số
                                    <xsl:value-of select="DLCTu/TTChung/TTCTLQuan/SCTCLQuan"/>, mẫu số
                                    <xsl:value-of select="DLCTu/TTChung/TTCTLQuan/KHMSCTCLQuan" />, ký hiệu
                                    <xsl:value-of select="DLCTu/TTChung/TTCTLQuan/KHCTCLQuan" />, ngày
                                    <xsl:value-of select="substring(DLCTu/TTChung/TTCTLQuan/NLCTCLQuan,9,2)" /> tháng
                                    <xsl:value-of select="substring(DLCTu/TTChung/TTCTLQuan/NLCTCLQuan,6,2)" /> năm
                                    <xsl:value-of select="substring(DLCTu/TTChung/TTCTLQuan/NLCTCLQuan,0,5)" />
                                </div>
                            </xsl:when>
                            <xsl:otherwise> </xsl:otherwise>
                        </xsl:choose>
                        <table width="100%" style="font-weight:bold;line-height: 28px;">
                            <tr>
                                <td align="center">
                                    <span style="font-size:15pt;text-transform: uppercase;">
                                        <xsl:value-of select="DLCTu/TTChung/TCTu" />
                                    </span>
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td style="padding-left:20px;width:100%;">
                                    <b> I.THÔNG TIN TỔ CHỨC TRẢ THU NHẬP </b>
                                </td>
                                <td>
                             
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[01] Tên tổ chức trả thu nhập :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <b style="text-transform: uppercase">
                                            <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/Ten" />
                                        </b>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[02] Mã số thuế :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;letter-spacing: 5px;">
                                        <b>
                                            <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/MST" />
                                        </b>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[03] Địa chỉ :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/DChi" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[04] Điện thoại :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/SDThoai" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td style="padding-left:20px;width:100%;">
                                    <b> II.THÔNG TIN CÁ NHÂN, HỘ KINH DOANH, CÁ NHÂN KINH DOANH</b>
                                </td>
                                <td>
                                
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[05] Họ và tên :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <b>
                                            <xsl:value-of select="DLCTu/NDCTu/NNT/Ten" />
                                        </b>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[06] Mã số thuế :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/NNT/MST" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[07] Quốc tịch :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/NNT/QTich" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <!-- <td style="padding-left:20px;width:35%">
                                [08] Cá nhân cư trú <i>(Resident individual):</i>
                            </td> -->
                                <!-- <td style="width:65%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="DLCTu/NDCTu/NNT/CNCTru" />
                                </td> -->
                                <td style="padding-left:20px;width:40%">
                                    <xsl:choose>
                                        <xsl:when test="DLCTu/NDCTu/NNT/CNCTru=1">  [08] Cá nhân cư trú :
                                            <input type="checkbox" checked="checked" />
                                        </xsl:when>
                                        <xsl:otherwise>  [08] Cá nhân cư trú :
                                            <input type="checkbox" />
                                        </xsl:otherwise>
                                    </xsl:choose>&#160;&#160;&#160;&#160;
                                    <xsl:choose>
                                        <xsl:when test="DLCTu/NDCTu/NNT/CNCTru=0"> [09] Cá nhân không cư trú :
                                            <input type="checkbox" checked="checked" />
                                        </xsl:when>
                                        <xsl:otherwise>  [09] Cá nhân không cư trú :
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[10] Địa chỉ :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/NNT/DChi" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[11] Điện thoại liên hệ :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/NNT/SDThoai" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[12] Số định danh cá nhân, giấy tờ tùy thân hoặc số hộ chiếu
                                        <i>(Trường hợp chưa đăng ký thuế):</i>
                                    </span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/NNT/CCCDan" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td style="padding-left:20px;width:100%;text-align:left">
                                    <b> III.THÔNG TIN THUẾ THU NHẬP CÁ NHÂN KHẤU TRỪ </b>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[13] Khoản thu nhập :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/TTNCNKTru/KTNhap" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[14] Khoản đóng bảo hiểm bắt buộc :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCNKTru/BHiem, '#.###.###,########','vnd')" />
                                        <xsl:if test="DLCTu/NDCTu/TTNCNKTru/BHiem=0">0</xsl:if>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[15] Khoản đóng từ thiện, nhân đạo, khuyến học :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCNKTru/TThien , '#.###.###,########','vnd')" />
                                        <xsl:if test="DLCTu/NDCTu/TTNCNKTru/TThien=0">0</xsl:if>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[16] Thời điểm trả thu nhập :</span>
                                    <span style="border-bottom: 0px dotted black; flex-grow: 1; margin-left: 5px;">
                                       Từ tháng &#160;
                                        <xsl:value-of select="DLCTu/NDCTu/TTNCNKTru/TThang" />&#160;
                                    đến tháng &#160;
                                        <xsl:value-of select="DLCTu/NDCTu/TTNCNKTru/DThang" />&#160;
                                      năm &#160;
                                        <xsl:value-of select="DLCTu/NDCTu/TTNCNKTru/Nam" />&#160;
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[17] Tổng thu nhập chịu thuế phải khấu trừ :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCNKTru/TTNCThue, '#.###.###,########','vnd')" />
                                        <xsl:if test="DLCTu/NDCTu/TTNCNKTru/TTNCThue=0">0</xsl:if>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[18] Tổng thu nhập tính thuế :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCNKTru/TTNTThue, '#.###.###,########','vnd')" />
                                        <xsl:if test="DLCTu/NDCTu/TTNCNKTru/TTNTThue=0">0</xsl:if>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px">
                            <tr>
                                <td colspan="2" style="display: flex; align-items: center;padding-left:20px;">
                                    <span>[19] Số thuế thu nhập cá nhân đã khấu trừ :</span>
                                    <span style="border-bottom: 1px dotted black; flex-grow: 1; margin-left: 5px;">
                                        <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCNKTru/SThue, '#.###.###,########','vnd')" />
                                        <xsl:if test="DLCTu/NDCTu/TTNCNKTru/SThue=0">0</xsl:if>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;line-height: 28px;">
                            <tr>
                                <td width="60%">
                                    <div style="width:100%;text-align:center;paramNguoiCD">
                  Người chuyển đổi
                                        <br />
                  (Ký, ghi rõ họ tên)
                                    </div>
                                </td>
                                <td style="width:40%;text-align:center">
                                    <br/>
                                    Ngày&#160;
                                    <xsl:value-of select="substring( DLCTu/TTChung/NLap,9,2)"/>&#160;
                               
                                     tháng&#160;
                                    <xsl:value-of select="substring(DLCTu/TTChung/NLap,6,2)"/>&#160;năm
                                    
                                    &#160;
                                    <xsl:value-of select="substring( DLCTu/TTChung/NLap,0,5)"/>
                                    <br />
                                    <b>ĐẠI DIỆN TỔ CHỨC TRẢ THU NHẬP</b>
                                    <br />
                                (Chữ ký điện tử, chữ ký số)
                                    <br />
                                    <br />
                                    <div style="paramSign;background-image:url(data:image/jpeg;base64,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); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid red; text-align:left;padding-top:5px; ">
                                        <span style="color:red;">
                    Được ký bởi:
                                            <!-- <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/" /> -->
                                            <xsl:value-of select="substring-before( substring-after(//*[local-name() = 'X509SubjectName'],'CN='), ',')" />
                                            <br/>
                    Ngày ký:
                                            <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],9,2)"/>-
                                            <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],6,2)"/>-
                                            <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],1,4)"/>
                                        </span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width:100%;padding-top:5px;text-align:center;">
                    (Cần kiểm tra đối chiếu khi lập, giao, nhận hoá đơn, chứng từ)
                </div>
                    <div style="text-align: center;">
                        <i>
                        Giải pháp hóa đơn, chứng từ điện tử được cung cấp bởi:
                            <b> Công ty cổ phần công nghệ thẻ Nacencomm</b>. Mã số thuế:
                            <b>0103930279</b>.
                        </i>
                    </div>
                    <div style="width:100%;padding-top:0px;text-align:center;padding-bottom:1px;">
                        <span>
                            <i>Tra cứu hóa đơn, chứng từ tại địa chỉ trang web: https://hoadon78.nacencomm.vn
                                <xsl:if test="$MTCuu!=''">Mã tra cứu :
                                    <b>
                                        <xsl:value-of select="$MTCuu"/>
                                    </b>
                                </xsl:if>
                            </i>
                        </span>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>