<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ds="http://www.w3.org/2000/09/xmldsig#"
    xmlns:ex="http://exslt.org/dates-and-times"
    extension-element-prefixes="ex">
    <xsl:output method="html" encoding="UTF-8" indent="yes"/>
    <xsl:template match="TDiep">
        <xsl:variable name="STT" select="count(//DLTBao/DSCTu/CTu/STT)" />
        <html lang="en"
            xmlns="http://www.w3.org/1999/xhtml">
            <head>
                <title>E-Invoice</title>
                <meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8' />
                <style type="text/css">
              #tblContent td, #tblContent th {
              border:1px dotted gray;
              padding:0;
              margin:0;
              }
              table {
              border-collapse: collapse;
              }

              .bg
              {
              background: url(Background.png) no-repeat center center fixed;
              -webkit-background-size: cover;
              -moz-background-size: cover;
              -o-background-size: cover;
              background-size:cover ;background-attachment: fixed;
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
              i{
                  font-size:12px;
              }
          @page {
                size: A4;
                margin: 0;
              }
              @media print {
                html, body {
                  width: 210mm;
                  height: 297mm;
                      zoom: 98%;
                  center;
                }
              }
            </style>
            </head>
            <page size="A4">
                <body style="font-family:Times New Roman">
                    <div style="viewstyle;border:2px solid #C08653;width:860px;margin:auto;background-image: url('https://hoadon78.nacencomm.vn/UploadIMG/bg/1234567857_4ofbtnq1.jpg');background-size:80%; background-position: center; background-repeat:no-repeat;">
                        <div style="width:860px;">
                            <table style="width:100%;">
                                <tr >
                                    <td style="width:65%">
                                       
                                    </td>
                                    <td style="padding-top:5px;text-align:left; ">
                                        <b>Mẫu số: </b>
                                        <span style="font-weight:bold; font-size:15pt;text-transform: uppercase;">
                                            <xsl:value-of select="DLieu/TBao/DLTBao/MSo" />
                                        </span>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20% font-size:15pt;text-transform: uppercase;text-align:center" >
                                        <xsl:value-of select="DLTBao/TCQTCTren" />
                                        <br/>
                                        <span style="font-weight:bold">
                                            <u>
                                                <xsl:value-of select="DLieu/TBao/DLTBao/TCQT" />
                                            </u>
                                        </span>
                                        <br/>
                                        Số:
                                        <xsl:choose>
                                            <xsl:when test="TTChung/MLTDiep='213'">
                                                <xsl:value-of select="DLieu/TBao/DLTBao/So" />
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:value-of select="DLieu/TBao/STBao/So" />
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                    <td style="width:20% font-size:15pt;text-transform: uppercase;text-align:center" >
                                      
                                    </td>
                                    <td style="font-weight:bold; font-size:14pt;text-transform: uppercase;text-align:center" >
                                        CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
                                        <br/>
                                        <span style="font-size:14pt">
                                            <u>Độc lập - Tự do - Hạnh phúc</u>
                                        </span>
                                        <br/>
                                        <xsl:choose>
                                            <xsl:when test="TTChung/MLTDiep='213'">
                                                <i>
                                                    <xsl:value-of select="DLieu/TBao/DLTBao/DDanh" />, ngày &#160;
                                                    <xsl:variable name="string">
                                                        <xsl:value-of select="substring(DLieu/TBao/DLTBao/NTBao,9,2)" />
                                                    </xsl:variable>
                                                    <xsl:value-of select="$string" /> tháng
                                            &#160;
                                                    <xsl:variable name="string1">
                                                        <xsl:value-of select="substring(DLieu/TBao/DLTBao/NTBao,6,2)" />
                                                    </xsl:variable>
                                                    <xsl:value-of select="$string1" /> năm
                                            &#160;
                                                    <xsl:variable name="string2">
                                                        <xsl:value-of select="substring(DLieu/TBao/DLTBao/NTBao,1,4)" />
                                                    </xsl:variable>
                                                    <xsl:value-of select="$string2" />
                                                </i>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <i>
                                                    <xsl:value-of select="DLieu/TBao/DLTBao/DDanh" />, ngày &#160;
                                                    <xsl:variable name="string">
                                                        <xsl:value-of select="substring(DLieu/TBao/STBao/NTBao,9,2)" />
                                                    </xsl:variable>
                                                    <xsl:value-of select="$string" /> tháng
                                            &#160;
                                                    <xsl:variable name="string1">
                                                        <xsl:value-of select="substring(DLieu/TBao/STBao/NTBao,6,2)" />
                                                    </xsl:variable>
                                                    <xsl:value-of select="$string1" /> năm
                                            &#160;
                                                    <xsl:variable name="string2">
                                                        <xsl:value-of select="substring(DLieu/TBao/STBao/NTBao,1,4)" />
                                                    </xsl:variable>
                                                    <xsl:value-of select="$string2" />
                                                </i>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <table style="width:100%;">
                                <tr>
                                    <td style="font-weight:bold; font-size:16pt;text-transform: uppercase;text-align:center" >
                                        <xsl:choose>
                                            <xsl:when test="TTChung/MLTDiep='213'">
                                                THÔNG BÁO
                                                <br/>
                                                <span style="font-size:12pt">
                                                    <xsl:value-of select="substring-after(DLieu/TBao/DLTBao/Ten,'báo')" />
                                                </span>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                THÔNG BÁO
                                                <br/>
                                                <span style="font-size:12pt">
                                                    <xsl:value-of select="DLieu/TBao/DLTBao/Ten" />
                                                </span>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <table style="width:100%;line-height:25px">
                                <tr>
                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                        <span>
                                            <b>Kính gửi: </b>
                                            <xsl:value-of select="DLieu/TBao/DLTBao/TNNT" />(
                                            <xsl:value-of select="DLieu/TBao/DLTBao/MST" />)
                                        </span>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:25px">
                                <tr>
                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                        <xsl:choose>
                                            <xsl:when test="TTChung/MLTDiep='213'">
                                                Sau khi xem xét Thông báo chứng từ điện tử đã lập sai ngày
                                                <xsl:variable name="string">
                                                    <xsl:value-of select="substring(DLieu/TBao/DLTBao/NTBao,9,2)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string" />/
                                                <xsl:variable name="string1">
                                                    <xsl:value-of select="substring(DLieu/TBao/DLTBao/NTBao,6,2)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string1" />/
                                                <xsl:variable name="string2">
                                                    <xsl:value-of select="substring(DLieu/TBao/DLTBao/NTBao,1,4)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string2" /> của:
                                            </xsl:when>
                                            <xsl:otherwise>Sau khi xem xét Thông báo chứng từ điện tử đã lập sai ngày
                                                <xsl:variable name="string">
                                                    <xsl:value-of select="substring(DLieu/TBao/STBao/NTBao,9,2)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string" />/
                                                <xsl:variable name="string1">
                                                    <xsl:value-of select="substring(DLieu/TBao/STBao/NTBao,6,2)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string1" />/
                                                <xsl:variable name="string2">
                                                    <xsl:value-of select="substring(DLieu/TBao/STBao/NTBao,1,4)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string2" /> của:
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:25px">
                                <tr>
                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                      Người nộp thuế:
                                        <xsl:value-of select="DLieu/TBao/DLTBao/TNNT" />
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:25px">
                                <tr>
                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                      Mã số thuế:
                                        <xsl:value-of select="DLieu/TBao/DLTBao/MST" />
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:25px">
                                <tr>
                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                      Mã giao dịch điện tử:
                                        <xsl:value-of select="DLieu/TBao/DLTBao/MGDDTu" />
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:25px">
                                <tr>
                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                        <xsl:choose>
                                            <xsl:when test="TTChung/MLTDiep='213'">
                                                Cơ quan thuế thông báo Dữ liệu Thông báo sai sót không hợp lệ
                                            </xsl:when>
                                            <xsl:otherwise></xsl:otherwise>
                                        </xsl:choose>
                                        <xsl:choose>
                                            <xsl:when test="TTChung/MLTDiep='301'">
                                                <xsl:choose>
                                                    <xsl:when test="DLieu/TBao/DLTBao/DSCTu/CTu/DSLDKTNhan/LDo/MLoi!=''">
Cơ quan thuế thông báo không tiếp nhận
                                                        <xsl:value-of select="$STT" />/
                                                        <xsl:value-of select="$STT" /> chứng từ điện tử đã lập sai.
                                                    </xsl:when>
                                                    <xsl:otherwise>Cơ quan thuế thông báo chấp nhận thông báo sai sót</xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise></xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                </tr>
                            </table>
                            <xsl:choose>
                                <xsl:when test="TTChung/MLTDiep='213'">
                                    <table style="width:100%;line-height:25px">
                                        <tr>
                                            <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                                <xsl:for-each select="DLieu/TBao/DLTBao/KHLKhac/DSLDo/LDo">
                                                    <xsl:value-of select="MTLoi" />
                                                </xsl:for-each>
                                            </td>
                                        </tr>
                                    </table>
                                </xsl:when>
                                <xsl:otherwise></xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                                <xsl:when test="TTChung/MLTDiep='301'">
                                    <xsl:choose>
                                        <xsl:when test="DLieu/TBao/DLTBao/DSCTu/CTu/DSLDKTNhan/LDo/MLoi!=''">
                                            <table style="width:100%;line-height:25px">
                                                <tr>
                                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                  Đề nghị quý công ty kiểm tra, đối chiếu chứng từ điện tử đã lập sai do thông tin chưa chính xác. Cụ thể thông tin chứng từ đã lập sai cơ quan thuế không tiếp nhận như sau:
                                       </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%;text-align:center; font-size:12pt;border-top: 1px solid black;border-bottom:1px solid black;border-left:1px solid black;border-right:1px solid black;" >
                                                <tr style="height:28px;">
                                                    <td width="5%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black;font-weight:bold">
                                        STT
                                    </td>
                                                    <td width="10%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black;font-weight:bold">
                                        Ký hiệu mẫu chứng từ

                                    </td>
                                                    <td width="10%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black;font-weight:bold">
                                         Ký hiệu chứng từ

                                    </td>
                                                    <td width="10%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black;font-weight:bold">
                                        Số chứng từ điện tử

                                    </td>
                                                    <td width="15%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black;font-weight:bold">
                                        Ngày lập chứng từ

                                    </td>
                                                    <td width="15%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black;font-weight:bold">
                                       Thông báo/ Giải trình

                                    </td>
                                                    <td width="35%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black;font-weight:bold">
                                        Lý do
                                    </td>
                                                </tr>
                                                <xsl:for-each select="DLieu/TBao/DLTBao/DSCTu/CTu">
                                                    <tr style="height:28px;">
                                                        <td style="border: 1px solid black">
                                                            <xsl:value-of select="STT" />
                                                        </td>
                                                        <td style="border: 1px solid black">
                                                            <xsl:value-of select="KHMSCTu" />
                                                        </td>
                                                        <td style="border: 1px solid black">
                                                            <xsl:value-of select="KHCTu" />
                                                        </td>
                                                        <td style="border: 1px solid black">
                                                            <xsl:value-of select="SCTu" />
                                                        </td>
                                                        <td style="border: 1px solid black">
                                                            <xsl:variable name="date" select="normalize-space(NLap)"/>
                                                            <xsl:value-of select="concat(substring($date, 9, 2), '/', substring($date, 6, 2), '/', substring($date, 1, 4))"/>
                                                        </td>
                                                        <td style="border: 1px solid black">
                                                            <xsl:if test="LADCTDT=1">Thông báo</xsl:if>
                                                            <xsl:if test="LADCTDT=2">Giải trình</xsl:if>
                                                        </td>
                                                        <td style="border: 1px solid black">
                                                            <xsl:value-of select="DSLDKTNhan/LDo/MTa" />
                                                        </td>
                                                    </tr>
                                                </xsl:for-each>
                                            </table>
                                        </xsl:when>
                                        <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise></xsl:otherwise>
                            </xsl:choose>
                            <table style="width:100%;line-height:25px">
                                <tr>
                                    <td style="display: flex; align-items: left;width:100%;padding-left:50px">
                                     Cơ quan thuế thông báo để người nộp thuế biết, thực hiện./.
                                       </td>
                                </tr>
                            </table>
                            <br/>
                            <table style="width:100%;" >
                                <tr>
                                    <td style="width:60%;">
                                   
                                </td>
                                    <td style="width:40%;font-size:13pt;text-align:center">
                                   CƠ QUAN THUẾ
                                        <br/>
                                        <i>(Chữ ký số)</i>
                                        <br/>
                                        <div style="paramSign;background-image:url(data:image/jpeg;base64,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); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid red; text-align:left;padding-top:5px; ">
                                            <span style="color:green;">
                    Được ký bởi:
                                                <xsl:value-of select="substring-before( substring-after(//*[local-name() = 'X509SubjectName'],'CN='), ',')" />
                                                <br/>
                    Ngày ký:
                                                <xsl:variable name="date" select="normalize-space((//*[local-name()='SigningTime'])[1])"/>
                                                <xsl:variable name="dateOnly" select="substring-before($date, 'T')"/>
                                                <xsl:value-of select="concat(substring($dateOnly, 9, 2), '-', substring($dateOnly, 6, 2), '-', substring($dateOnly, 1, 4))"/>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </body>
            </page>
        </html>
    </xsl:template>
</xsl:stylesheet>