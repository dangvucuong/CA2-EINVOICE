<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ds="http://www.w3.org/2000/09/xmldsig#"
    xmlns:ex="http://exslt.org/dates-and-times"
    extension-element-prefixes="ex">
    <xsl:output method="html" encoding="UTF-8" indent="yes"/>
    <xsl:template match="BBan">
        <xsl:variable name="STT" select="count(/TBao/DLTBao/DSCTu/CTu/STT)" />
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
                    <div style="viewstyle;border:1px solid black;">
                        <div style="width:860px;">
                            <table style="width:100%;border:0px solid black;line-height:30px">
                                <tr >
                                    <td style="width:100%;text-align:center;15pt;font-weight:bold;">
                                       CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
                                        <br/>
                                        <span style="font-size:14pt">
                                            Độc lập - Tự do - Hạnh phúc
                                        </span>
                                        <br/>
                                        <span style="font-size:14pt">
                                            ---oOo---
                                        </span>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="width:100% font-size:15pt;text-transform: uppercase;text-align:center;font-weight:bold;" >
                                        BIÊN BẢN
                                        <xsl:if test="NDBBan/TTChung/TCHDon=1">THAY THẾ</xsl:if>
                                        <xsl:if test="NDBBan/TTChung/TCHDon=2">ĐIỀU CHỈNH</xsl:if>
                                        HÓA ĐƠN ĐIỆN TỬ
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="font-style:italic; font-size:12pt;text-align:left; padding-left:50px" >
                                    - Căn cứ Nghị định số 70/2025/NĐ-CP ngày 20/03/2025 của Chính phủ sửa đổi, bổ sung một số điều của Nghị định số 123/2020/NĐ-CP ngày 19/10/2020 của Chính phủ quy định về hóa đơn, chứng từ;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-style:italic; font-size:12pt;text-align:left; padding-left:50px" >
                                        <br/>
                                    - Căn cứ Thông tư số 32/2025/TT-BTC ngày 31/05/2025 của Bộ Tài chính hướng dẫn thực hiện một số điều của Luật Quản lý thuế ngày 13/6/2019, Nghị định số 123/2020/NĐ-CP ngày 19/10/2020 của Chính phủ quy định về hóa đơn, chứng từ, Nghị định số 70/2025/NĐ-CP ngày 20/03/2025 của Chính phủ sửa đổi, bổ sung một số điều của Nghị định số 123/2020/NĐ-CP. 
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td style="font-size:12pt;text-align:left; padding-left:50px" >
                                        <br/>
                                    Hôm nay, ngày&#160;
                                        <xsl:variable name="date" select="normalize-space(NDBBan/TTChung/NBBan)"/>
                                        <xsl:value-of select="concat(substring($date, 9, 2), '/', substring($date, 6, 2), '/', substring($date, 1, 4))"/>
                                        &#160;chúng tôi gồm có:
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:20px;text-transform: uppercase" >
                                       <b>BÊN BÁN:
                                        <xsl:value-of select="NDBBan/TTChung/NBan" /></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:50px" >
                                       Mã số thuế:
                                        <xsl:value-of select="NDBBan/TTChung/MSTNBan" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:50px" >
                                       Địa chỉ:
                                        <xsl:value-of select="NDBBan/TTChung/DCNBan" />
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:20px;text-transform: uppercase" >
                                      <b> BÊN MUA:
                                        <xsl:value-of select="NDBBan/TTChung/NMua" /></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:50px" >
                                       Mã số thuế:
                                        <xsl:value-of select="NDBBan/TTChung/MSTNMua" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:50px" >
                                       Số CC/CCCD/số định danh/Mã ngân sách:
                                        <xsl:value-of select="NDBBan/TTChung/DDNMua" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:50px" >
                                       Địa chỉ:
                                        <xsl:value-of select="NDBBan/TTChung/DCNMua" />
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="font-size:12pt;text-align:left; padding-left:50px" >
                                       Chúng tôi thống nhất cùng tiến hành lập biên bản về việc
                                        <xsl:if test="NDBBan/TTChung/TCHDon=1">THAY THẾ</xsl:if>
                                        <xsl:if test="NDBBan/TTChung/TCHDon=2">ĐIỀU CHỈNH</xsl:if> hóa đơn điện tử sau:
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="width:50%;font-size:12pt;text-align:left; padding-left:50px" >
                                       1.	Thông tin hóa đơn bị
                                        <xsl:if test="NDBBan/TTChung/TCHDon=1">THAY THẾ</xsl:if>
                                        <xsl:if test="NDBBan/TTChung/TCHDon=2">ĐIỀU CHỈNH</xsl:if>:
                                    </td>
                                    <td style="font-size:12pt;text-align:left;" >
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:50%;font-size:12pt;text-align:left; padding-left:50px" >
                                       -	Mẫu số:
                                        <xsl:value-of select="NDBBan/TTChung/KHMSHDon" />
                                    </td>
                                    <td style="font-size:12pt;text-align:left;" >
                                       Ký hiệu:
                                        <xsl:value-of select="NDBBan/TTChung/KHHDon" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:50%;font-size:12pt;text-align:left; padding-left:50px" >
                                       -	Số hóa đơn:
                                        <xsl:value-of select="NDBBan/TTChung/SHDon" />
                                    </td>
                                    <td style="font-size:12pt;text-align:left;" >
                                       Ngày:
                                        <xsl:variable name="date" select="normalize-space(NDBBan/TTChung/NLHDGoc)"/>
                                        <xsl:value-of select="concat(substring($date, 9, 2), '/', substring($date, 6, 2), '/', substring($date, 1, 4))"/>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="width:100%;font-size:12pt;text-align:left; padding-left:50px" >
                                       2.	Lý do
                                        <xsl:if test="NDBBan/TTChung/TCHDon=1">THAY THẾ</xsl:if>
                                        <xsl:if test="NDBBan/TTChung/TCHDon=2">ĐIỀU CHỈNH</xsl:if>:
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;font-size:12pt;text-align:left; padding-left:50px" >
                                        <xsl:value-of select="NDBBan/TTChung/DSLDTDoi/LDo" />
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:30px">
                                <tr>
                                    <td style="width:100%;font-size:12pt;text-align:left; padding-left:50px" >
                                       Biên bản được hai bên nhất trí và lập thành hai bản (02 bản), mỗi bên giữ một bản (01 bản) và có giá trị pháp lý như nhau.
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;font-size:12pt;text-align:left; padding-left:50px" >
                                        Chúng tôi cam kết và hoàn toàn chịu trách nhiệm về việc
                                        <xsl:if test="NDBBan/TTChung/TCHDon=1">THAY THẾ</xsl:if>
                                        <xsl:if test="NDBBan/TTChung/TCHDon=2">ĐIỀU CHỈNH</xsl:if> hóa đơn hóa đơn điện tử này.
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;" >
                                <tr>
                                    <td style="width:50%;font-size:13pt;text-align:center">
                                   <b>ĐẠI DIỆN BÊN MUA</b>
                                </td>
                                    <td style="width:50%;font-size:13pt;text-align:center">
                                   <b>ĐẠI DIỆN BÊN BÁN</b>
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