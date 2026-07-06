<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ex="http://exslt.org/dates-and-times" xmlns:fn="http://www.w3.org/2005/02/xpath-functions" xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
    <xsl:output method="html" />
    <xsl:param name="imgLogo" />
    <xsl:param name="percent" select="''" />
    <xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
    <xsl:template match="CTu">
        <xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
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
            <body style="font-family:Times New Roman; font-size: 13pt;line-height: 2;">
                <!--<div style="viewstyle;width:100%">-->
                <div style="viewstyle;border:none;    background-size: 100%; background-color: hsla(0,0%,100%,0.60);background-position: 50% 56%!important;">
                    <div id="background" style="paramMau">
              MẪU
            </div>
                    <div id="background" style="paramdisable">contentDisable</div>
                    <div style="border:1px solid #3c73b3;width:850px; min-height: 297mm;z-index:1;">
                        
                        <!-- <table width="100%" style="font-weight:bold;font-size:12pt;line-height: 1.5;">
                            <tr>
                                <td width="30%" style="padding-left:20px;text-transform: uppercase;" align="center">
                                    <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/Ten" />
                                </td>
                                <td width="40%" align="center" style="font-size:12pt;padding-top:43px">
                                    <br /> CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
                                    <br />
                                Độc lập - Tự do - Hạnh phúc
                                    <br />
                                 <i>SOCIALIST REPUBLIC OF VIETNAM
                                    <br />
                                    Independence - Freedom - Happiness
                                    <br /></i>    -->
                                
                                    <!-- <br />
                                </td>
                                <td width="30% " style="padding-left:20px;font-size:12pt;">
                                  
                
                               Mẫu số
                                    <i>(Form)</i>:
                                    <b>
                                        <xsl:value-of select="DLCTu/TTChung/MSCTu" />
                                    </b>
                                    <br />
                    Ký hiệu
                                    <i>(Serial No)</i>:
                                    <b>
                                        <xsl:value-of select="DLCTu/TTChung/KHCTu" />
                                    </b>
                                    <br />
                    Số
                                    <i>(No)</i>:
                                    <span style="color: red;font-size:13pt">
                                        <xsl:value-of select="DLCTu/TTChung/SCTu" />
                                    </span>
                                </td>
                            </tr>
                        </table> -->
                        <!-- <table width="100%" style="font-weight:bold;line-height: 1.5;">
                            <tr>
                                <td align="center">
                                    <span style="font-size:15pt;text-transform: uppercase;">
                                        <xsl:value-of select="DLCTu/TTChung/THDon" />
                                    </span>
                                    <br />
                                    <span style="font-size:12pt">
                                        <i>CERTIFICATE OF PERSONAL INCOME TAX
                                        WITHHOLDING</i>
                                    </span>
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:100%;">
                                    <b> I.THÔNG TIN TỔ CHỨC TRẢ THU NHẬP </b>
                                    <i>(Information of the income paying
                                        organization)</i>
                                </td>
                                <td>
                             
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:58%">
                                [01] Tên tổ chức trả thu nhập
                                    <i> (Name of the income paying organization):</i><br/>
                            
                                   <b><xsl:value-of select="DLCTu/NDCTu/TCTTNhap/Ten" /></b> 
                                </td>
                            </tr>
                        </table>
                      
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:40%">
                                [02] Mã số thuế
                                    <i>(Tax identification number):</i>
                                </td>
                                <td style="width:60%;">
                                    <span style="font-size:12pt;font-weight:bold; letter-spacing: 5px;">
                                        <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/MST" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:width:25%">
                                [03] Địa chỉ
                                    <i>(Address):</i>
                                </td>
                                <td style="width:75%;">
                                    <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/DChi" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:31%">
                                [04] Điện thoại
                                    <i>(Telephone number):</i>
                                </td>
                                <td style="wdith:69%;">
                                    <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/SDThoai" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:100%;">
                                    <b> II.THÔNG TIN NGƯỜI NỘP THUẾ</b>
                                    <i>(Information of taxpayer)</i>
                                </td>
                                <td>
                                
                                </td>
                            </tr>
                        </table>-->
                        <table style="width:100%;">
                            <tr>
                                <td style="text-align:center; padding-top:0px;padding-left:0px!important;width:180px" rowspan="5" >
                                    <img style="width:180px;height:110px;align-content:center;position:static;left:0;top:0;object-fit: scale-down;"
                                      id="imgSample" src="paramLogo">
            </img>
                                </td>
                                <td colspan="3" style="padding-left:5px">
                                    <span style="font-weight:bold; font-size:18pt" >
                                        <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/Ten" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size:13pt;width:29%;padding-left:5px;vertical-align:top">
            Địa chỉ
                                    <i>(Address)</i>
                                </td>
                                <td style="font-size:13pt;width:2%;vertical-align:top">:</td>
                                <td style="font-size:13pt;width:69%">
                                    <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/DChi" />
                                    <!-- <xsl:value-of select="DLHDon/NDHDon/NBan/DChi" /> -->
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size:13pt">
                                    <span style="font-weight:bold;padding-left:5px">
              Mã số thuế
                                        <i>(Tax code)</i>
                                    </span>
                                </td>
                                <td>:</td>
                                <td style="font-weight:bold;">
                                    <du>
                                         <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/MST" />
                                    </du>
                                </td>
                            </tr>
                            <tr style="font-size:13pt;padding-left:5px">
                                <td style="padding-left:5px">
            Điện thoại
                                    <i>(Tel)</i>
                                </td>
                                <td>:</td>
                                <td>
                                    <!--024 394 101 26-->
                                    <xsl:value-of select="DLCTu/NDCTu/TCTTNhap/SDThoai" />
            &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;
            <!-- Fax:
                                    <xsl:value-of select="DLHDon/NDHDon/NBan/Fax" /> -->
                                </td>
                            </tr>
                            <!-- <tr>
                                <td style="font-size:13pt;padding-left:5px">
            Số tài khoản
                                    <i>(Account No)</i>
                                </td>
                                <td>:</td>
                                <td>
                                 
                                    <xsl:value-of select="DLHDon/NDHDon/NBan/STKNHang" /> &#160;  &#160; &#160;&#160; &#160;&#160; &#160;&#160;&#160;&#160; &#160;&#160;
            Tại
                                    <i>(At)</i>:
                                    <xsl:value-of select="DLHDon/NDHDon/NBan/TNHang" />
                                </td>
                            </tr> -->
                        </table>
                        <hr style="background-color:black;width:100%;height:1px;margin-bottom:5px" />
                        <table style="width:100%;">
                            <tr>
                                <td style="width:10%">

          </td>
                                <td style="padding-top:5px;text-align:center; ">
                                    <span style="font-weight:bold; font-size:18pt;text-transform: uppercase;">
                                        <xsl:value-of select="DLCTu/TTChung/THDon" />
                                    </span><br/>
                                    <span style="font-size:12pt">
                                        <i>CERTIFICATE OF PERSONAL INCOME TAX
                                        WITHHOLDING</i>
                                    </span>
                                    <br/>
                                    <span style="font-weight:normal;font-size:12.5pt;display:param1_1">param1</span>
                                </td>
                                <td style="width:40%;text-align:left;padding-left:30px">
                                    <div style="padding-bottom:10px">
              Mẫu số
                                        <i>(Form No)</i>:
                                        <b>
                                            <xsl:value-of select="DLCTu/TTChung/MSCTu" />
                                        </b>
                                    </div>
                                    <div style="padding-bottom:5px">
              Ký hiệu
                                        <i>(Serial No)</i>:
                                        <b>
                                            <xsl:value-of select="DLCTu/TTChung/KHCTu" />
                                        </b>
                                    </div>
                                    <div>
              Số
                                        <i>(Invoice No)</i>:
                                        <span style="color: red;font-size:16pt">
                                            <xsl:value-of select="DLCTu/TTChung/SCTu" />
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%">

          </td>
                                <td style="text-align:center;">
                                    <!-- <span style="font-size:10.5pt">
              Ngày
                                        <i>(Date)</i>&#160;
                                        <xsl:variable name="string">
                                            <xsl:value-of select="substring(DLHDon/TTChung/NLap,9,2)"/>
                                        </xsl:variable>
                                        <xsl:value-of select="$string" />

              tháng
                                        <i>(month)</i> &#160;
                                        <xsl:variable name="string1">
                                            <xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)"/>
                                        </xsl:variable>
                                        <xsl:value-of select="$string1" />
              năm
                                        <i>(year)</i>&#160;
                                        <xsl:variable name="string2">
                                            <xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)"/>
                                        </xsl:variable>
                                        <xsl:value-of select="$string2" />
                                    </span> -->
                                </td>
                                <!-- <td style="text-align:center;width:30%">
                                    <div style="color: red; font-size: 10pt;padding-top:5px;  display: normal;text-align:center">
                                        <div style="paramChuyendoi">
                HOÁ ĐƠN CHUYỂN ĐỔI
                                            <br />
                TỪ HOÁ ĐƠN ĐIỆN TỬ
                                        </div>
                                    </div>
                                </td> -->
                            </tr>
                        </table>
                        <br/>
                        <table>
                            <tr>
                                <td style="padding-left:200px" >
                                    <b>
                                        <xsl:if test="MCCQT !=''">
                        MÃ CQT CẤP:
                                            <xsl:value-of select="MCCQT"/>
                                        </xsl:if>
                                    </b>
                                </td>
                            </tr>
                        </table>
                       
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:100%;">
                                    <b> II.THÔNG TIN NGƯỜI NỘP THUẾ</b>
                                    <i>(Information of taxpayer)</i>
                                </td>
                                <td>
                                
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:30%">
                                [05] Họ và tên
                                    <i>(Full Name):</i>
                                </td>
                                <td style="width:70%;border-bottom:1px dotted gray">
                                  <b><xsl:value-of select="DLCTu/NDCTu/NNT/Ten" /></b>  
                                </td>
                            </tr>
                        </table> 
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:40%">
                                [06] Mã số thuế
                                    <i>(Tax identification number):</i>
                                </td>
                                <td style="width:60%;border-bottom:1px dotted gray;font-weight:bold; letter-spacing: 5px;">
                                    <xsl:value-of select="DLCTu/NDCTu/NNT/MST" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:30%">
                                [07] Quốc tịch
                                    <i>(Nationality):</i>
                                </td>
                                <td style="width:70%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="DLCTu/NDCTu/NNT/QTich" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <!-- <td style="padding-left:20px;width:35%">
                                [08] Cá nhân cư trú <i>(Resident individual):</i>
                            </td> -->
                                <!-- <td style="width:65%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="DLCTu/NDCTu/NNT/CNCTru" />
                                </td> -->
                                <td style="padding-left:20px;width:40%">
                                    <xsl:choose>
                                        <xsl:when test="DLCTu/NDCTu/NNT/CNCTru=1">  [08] Cá nhân cư trú
                                            <i>(Resident individual):</i>
                                            <input type="checkbox" checked="checked" />
                                        </xsl:when>
                                        <xsl:otherwise>  [08] Cá nhân cư trú
                                            <i>(Resident individual):</i>
                                            <input type="checkbox" />
                                        </xsl:otherwise>
                                    </xsl:choose>&#160;&#160;&#160;&#160;
                                    <xsl:choose>
                                        <xsl:when test="DLCTu/NDCTu/NNT/CNCTru=0"> [09] Cá nhân không cư trú
                                            <i>(Non-resident individual):</i>
                                            <input type="checkbox" checked="checked" />
                                        </xsl:when>
                                        <xsl:otherwise>  [09] Cá nhân không cư trú
                                            <i>(Non-resident individual):</i>
                                            <input type="checkbox" />
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:100%">
                                [10] Địa chỉ hoặc số điện thoại liên hệ
                                    <i>(Contact Address or Telephone
                                    Number):</i>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:1%;">
                                
                            </td>
                                <td style="padding-left:20px;width:99%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="DLCTu/NDCTu/NNT/DChi" />
                                    <xsl:choose>
                                        <xsl:when test="DLCTu/NDCTu/NNT/SDThoai!=''">&#160;&#160;&#160;&#160;Số điện thoại:
                                            <xsl:value-of select="DLCTu/NDCTu/NNT/SDThoai" />
                                        </xsl:when>
                                        <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:100%;">
                                    Trường hợp không có mã số thuế thì ghi thông tin cá nhân theo 2 chỉ tiêu [11] &amp;	 [12]  dưới đây:
                                    <br/>
                                    <i>(If Taxpayer does not have Tax identification number, please fill in 2 following items [11] &amp;	 [12]):</i>
                                </td>
                                <td>
                                 
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:53%">
                                    [11] Số CMND/CCCD hoặc số hộ chiếu  <i>(ID/Passport Number)</i>:
                                </td>
                                <td style="width:47%;border-bottom:1px dotted gray;font-weight:bold; letter-spacing: 5px;">
                                    <xsl:value-of select="DLCTu/NDCTu/NNT/CMND" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:25%">
                                      [12] Nơi cấp <i>(Place of issue):</i>
                                </td>
                                <td style="width:40%;border-bottom:1px dotted gray; letter-spacing:-0.5px">
                                    <xsl:value-of select="DLCTu/NDCTu/NNT/NCCMND" />
                                </td>
                                <td style="width:24%;letter-spacing:-0.5px">
                                    [13] Ngày cấp  <i> (Date of issue):</i>
                                </td>
                                <td style="width:16%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="substring(DLCTu/NDCTu/NNT/NgCCMND,9,2)"/>/
                                    <xsl:value-of select="substring(DLCTu/NDCTu/NNT/NgCCMND,6,2)"/>/
                                    <xsl:value-of select="substring(DLCTu/NDCTu/NNT/NgCCMND,0,5)"/>
                                    <!-- <xsl:variable name="issuedDate" select="DLCTu/NDCTu/NNT/NgCCMND"/>
                                    <xsl:value-of select="concat(substring($issuedDate, 9, 2), '/', substring($issuedDate, 6, 2), '/', substring($issuedDate, 1, 4))"/> -->
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:100%;">
                                    <b> III.THÔNG TIN THUẾ THU NHẬP CÁ NHÂN KHẤU TRỪ </b>
                                    <i>(Information of personal income
                                        tax
                                        withholding)</i>
                                </td>
                                <td>
                                    <!-- <span style="font-weight:bold; font-size:12pt">
                                    <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
                                </span>
                                <br /> -->
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:33%">
                                [14] Khoản thu nhập
                                    <i>(Type of income):</i>
                                </td>
                                <td style="width:67%;border-bottom:1px dotted gray">
                                   <xsl:choose>
                                        <xsl:when test="DLCTu/NDCTu/TTNCKTru/KTNhap  &gt; 1"> <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCKTru/KTNhap, '#.###.###,########','vnd')" /> </xsl:when>
                                        <xsl:otherwise><xsl:value-of select="DLCTu/NDCTu/TTNCKTru/KTNhap" /></xsl:otherwise>
                                    </xsl:choose>
                                </td>
                            </tr>
                        </table>
                         <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:75%">
                                [14a] Khoản đóng bảo hiểm bắt buộc <i>(The aforesaid deductible insurance
                                    premiums)</i>:
                            </td>
                                <td style="width:25%;border-bottom:1px dotted gray">
                               <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCKTru/BHiem, '#.###.###,########','vnd')" />
                                </td>
                            </tr>
                        </table> 
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:42%">
                                [15] Thời điểm trả thu nhập
                                    <i>(Time of income payment):</i>
                                </td>
                                <td style="width:48%;border-bottom:1px dotted gray;  letter-spacing:-0.5px;">
                                    từ tháng <i>(from month)</i>  &#160; <xsl:value-of select="DLCTu/NDCTu/TTNCKTru/TThang" />&#160;
                                    đến tháng <i>(to month)</i>  &#160; <xsl:value-of select="DLCTu/NDCTu/TTNCKTru/DThang" />&#160;
                                      năm <i>(year)</i> &#160;<xsl:value-of select="DLCTu/NDCTu/TTNCKTru/Nam" />&#160;
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:70%">
                                [16] Tổng thu nhập chịu thuế phải khấu trừ
                                    <i>(Total taxable income to be
                                    withheld):</i>
                                </td>
                                <td style="width:30%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCKTru/TTNCThue, '#.###.###,########','vnd')" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:50%">
                                [17] Tổng thu nhập tính thuế
                                    <i>(Total tax calculation income
                                   ):</i>
                                </td>
                                <td style="width:50%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCKTru/TTNTThue, '#.###.###,########','vnd')" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:70%">
                                [18] Số thuế thu nhập cá nhân đã khấu trừ
                                    <i>(Amount of personal income tax
                                    withheld):</i>
                                </td>
                                <td style="width:30%;border-bottom:1px dotted gray">
                                    <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCKTru/SThue, '#.###.###,########','vnd')" />
                                </td>
                            </tr>
                        </table>
                        <table style="width:100%;">
                            <tr>
                                <td style="padding-left:20px;width:68%">
                                    [19] Số thu nhập còn được nhận <i>(Amount of income still received
                                        ):</i>
                                </td>
                                    <td style="width:32%;border-bottom:1px dotted black">
                                        <xsl:value-of select="format-number(DLCTu/NDCTu/TTNCKTru/STNCDNhan, '#.###.###,########','vnd')" />
                                     
                                    </td>
                            </tr>
                        </table>

                        <table style="line-height: 1.5;">
                            <tr>
                                <td width="60%"></td>
                                <td>Ngày
                                    <i>(date)</i>
                                    <xsl:value-of select="substring( DLCTu/TTChung/NLap,9,2)"/>
                               
                                     tháng
                                    <i>(month)</i>
                                    <xsl:value-of select="substring(DLCTu/TTChung/NLap,6,2)"/> năm
                                    <i>(year)</i>
                                    <xsl:value-of select="substring( DLCTu/TTChung/NLap,0,5)"/>
                                    <br />
                                ĐẠI DIỆN TỔ CHỨC TRẢ THU NHẬP
                                    <br />
                                    <i>(Income paying organization)</i>
                                    <br />
                                (Chữ ký điện tử, chữ ký số)
                                    <br />
                                    <!-- Ký đóng dấu (ghi rõ họ tên và chức vụ)
                                    <br />
                              <i>(Signature,seal,full name and designature)</i>  -->
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
                                            <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],0,5)"/>
                                        </span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width:100%;padding-top:15px;text-align:center;">
                    (Cần kiểm tra đối chiếu khi lập, giao, nhận hoá đơn, chứng từ)
                </div>
                    <div style="width:100%;text-align:center;padding-bottom:5px;">
                        <span style="font-size:12px;">
                        Chuỗi xác thực:
                            <b>
                                <xsl:value-of select="$digest" />
                            </b>
                        </span>
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
                            <i>Tra cứu hóa đơn, chứng từ tại địa chỉ trang web: https://hoadon78.nacencomm.vn </i>
                        </span>
                    </div>
                    <br/>
                    <br/>
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>