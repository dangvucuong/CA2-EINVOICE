<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ex="http://exslt.org/dates-and-times"
    xmlns:fn="http://www.w3.org/2005/02/xpath-functions"
    xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
    <xsl:output method="html" />
    <xsl:param name="imgLogo" />
    <xsl:param name="percent" select="''" />
    <xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
    <xsl:decimal-format name="usd" decimal-separator="." grouping-separator="," />
    <xsl:template match="HDon">
        <xsl:variable name="xetphi" select="count(DLHDon/NDHDon/DSHHDVu/HHDVu[DVTinh='0*1'])" />
        <xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
        <xsl:variable name="TSuat" select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat" />
        <xsl:variable name="soHHdu" select="0-(count(DLHDon/NDHDon/DSHHDVu/HHDVu/STT))" />
        <xsl:variable name="somucthue" select="count(DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat)" />
        <xsl:variable name="DVTTe" select="DLHDon/TTChung/DVTTe" />
        <xsl:variable name="HVTNMHang" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='HVTNMHang']/DLieu" />
        <xsl:variable name="DChiNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='DChi']/DLieu" />
        <xsl:variable name="DCTDTuNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='DCTDTu']/DLieu" />
        <xsl:variable name="STKNHangNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='STKNHang']/DLieu" />
        <xsl:variable name="TNHangNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='TNHang']/DLieu" />
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
              .textfont{
              font-size:13pt;
              font-family:"Times New Roman";
              color:Black;
              }
i{
    font-size:12pt;
}
              @page {
                size: A4;
                margin: 20px;
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
                <body style="font-family:Times New Roman" class="textfont">
                    <div style="viewstyle;border:none;">
                        <div id="background" style="paramMau">
              MẪU
            </div>
                        <div id="background" style="paramdisable">contentDisable</div>
                        <div style="border:2px solid black;width:860px;height: auto; min-height: 100%;border-image:url(data:image/jpg;base64,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) 30 round;border-width:20px;z-index:1">
                            <table style="width:100%;" class="textfont">
                                <tr >
                                    <td style="width:20%;text-align:center; padding-top:10px;padding-left:0px!important" rowspan="2" >
                                        <img style="width:180px;height:130px;align-content:center;position:static;left:0;top:0;"
                                                                  id="imgSample" src="paramLogo">
									</img>
                                    </td>
                                    <td style="width:50%;padding-top:5px;text-align:center; ">
                                        <span style="font-weight:bold; font-size:18pt;color:MidnightBlue;text-transform: uppercase;">
                                            <xsl:value-of select="DLHDon/TTChung/THDon" />
                                        </span>
                                        <br/>
                                        <span style="font-weight:bold; font-size:18pt;color:MidnightBlue">(VAT INVOICE)</span>
                                        <br/>
                                        <span style="font-weight:normal;font-size:10.5pt;display:none"></span>
                                    </td>
                                    <td style="padding-left:10px;padding-top:20px;width:30%;">

                    Mẫu số
                                        <i>(Form No)</i>&#160;&#160;:
                                        <b>
                                            <xsl:value-of select="DLHDon/TTChung/KHMSHDon" />
                                        </b>
                                        <br />
                    Ký hiệu
                                        <i>(Serial No)</i>:
                                        <b>
                                            <xsl:value-of select="DLHDon/TTChung/KHHDon" />
                                        </b>
                                        <br />
                    Số
                                        <i>(No)</i>&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;:
                                        <span style="color: red;font-size:16pt">
                                            <xsl:value-of select="substring(
  concat('00000000', DLHDon/TTChung/SHDon), 
  string-length(DLHDon/TTChung/SHDon) + 1, 
  8
)"/>
                                        </span>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;">
                                        <span style="font-size:13pt">
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
                                        </span>
                                    </td>
                                    <td style="text-align:center;padding-left:80px">
                                        <div style="color: red; font-size: 10pt;padding-top:5px;  display: normal;text-align:center">
                                            <div style="paramChuyendoi">
                        HOÁ ĐƠN CHUYỂN ĐỔI
                                                <br />
                        TỪ HOÁ ĐƠN ĐIỆN TỬ
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                            <hr style="background-color:black;width:100%;height:1px;margin-bottom:5px" />
                            <table style="width:100%;line-height:30px" class="textfont">
                                <tr>
                                    <td style="padding-left:20px;" colspan="2">
                                        <span style="font-weight:bold; font-size:15pt;color:Red">
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
                                        </span>
                                        <br/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:20px;width:25%">                    
                    Mã số thuế
                                        <i>(Tax code)</i>:
                                    </td>
                                    <td style="width:75%">
                                        <span style="font-weight:bold; font-size:13pt">
                                            <du>
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
                                            </du>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:20px;vertical-align: text-top;">
                    Địa chỉ
                                        <i>(Address)</i>:
                                    </td>
                                    <td style="vertical-align: text-top;">
                                        <xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
                                    </td>
                                </tr>
                                <tr style="display:normal">
                                    <td style="padding-left:20px;">
                                        <span style="display:normal">
                      Số tài khoản
                                            <i>(Account No)</i>:
                                        </span>
                                    </td>
                                    <td>
                                        <span style="display:normal">
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/STKNHang" /> 
                    Tại:
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/TNHang" />
                                        </span>
                                    </td>
                                </tr>
                                <tr style="display:normal" >
                                    <td style="padding-left:20px;">
                                        <span style="display:normal">
                    Điện thoại
                                            <i>(Tel)</i>:
                                        </span>
                                    </td>
                                    <td>
                                        <span style="display:normal">
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/SDThoai" />
					          &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;
                                        </span>
                                        <span style="display:none">
                      Fax:
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/Fax" />
                                        </span>
                                    </td>
                                </tr>
                                <tr style="display:none">
                                    <td style="padding-left:20px;">
                                        <span style="display:none">
                      Email:
                                  
                    </span>
                                    </td>
                                    <td>
                                        <span style="display:none">
                          &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160; &#160;&#160;&#160; &#160;&#160;&#160; &#160;&#160;&#160; &#160;&#160;&#160; &#160;&#160;&#160; &#160;&#160;&#160;
                      Website:&#160;&#160;&#160; 
                    </span>
                                    </td>
                                </tr>
                            </table>
                            <hr style="background-color:black;width:100%;height:0.5px;margin-bottom:5px;margin-top:5px" />
                            <table style="width:100%;line-height:30px" class="textfont">
                                <tr>
                                    <td style="padding-left:20px;width:40%">
                    Họ tên người mua
                                        <i>(Customer Name)</i>:
                                    </td>
                                    <td style="width:60%; color: red">
                                        <b>
                                            <xsl:value-of select="DLHDon/NDHDon/NMua/HVTNMHang" />
                                            <xsl:if test="$HVTNMHang!=''">
                                                <xsl:value-of select="$HVTNMHang" />
                                            </xsl:if>
                                        </b>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:30px" class="textfont">
                                <tr>
                                    <td style="padding-left:20px;width:25%;padding-bottom:0px">
                                    Tên đơn vị
                                        <i>(Company's)</i>:
                                    </td>
                                    <td style="color: red">
                                        <b>
                                            <xsl:value-of select="DLHDon/NDHDon/NMua/Ten" />
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:20px;">
                    Mã số thuế
                                        <i>(Tax code)</i>:
                                    </td>
                                    <td style="font-size:13pt">
                                        <du>
                                            <b>
                                                <xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
                                            </b>
                                        </du>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left:20px;vertical-align: text-top;">
                    Địa chỉ
                                        <i>(Address)</i>:
                                    </td>
                                    <td style="vertical-align: text-top;">
                                        <xsl:value-of select="DLHDon/NDHDon/NMua/DChi" />
                                        <xsl:if test="$DChiNMua!=''">
                                            <xsl:value-of select="$DChiNMua" />
                                        </xsl:if>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:30px" class="textfont">
                                <tr>
                                    <td style="padding-left:20px;">
                    Hình thức thanh toán
                                        <i>(Payment Method)</i>:
                                        <xsl:value-of select="DLHDon/TTChung/HTTToan" />
                                
                     &#160;&#160;&#160; &#160;&#160;&#160;Số tài khoản
                                        <i>(Account No)</i>:
                                        <xsl:value-of select="DLHDon/NDHDon/NMua/STKNHang" />
                                        <xsl:if test="$STKNHangNMua!=''">
                                            <xsl:value-of select="$STKNHangNMua"  />
                                        </xsl:if>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;line-height:30px" class="textfont">
                                <tr>
                                    <td style="padding-left:20px;width:38%">
                    Đồng tiền thanh toán
                                        <i>(Payment Currency)</i>:
                                    </td>
                                    <td style="width:62%;">
                                        <xsl:value-of select="DLHDon/TTChung/DVTTe" />&#160;&#160;&#160;&#160;
                                        <xsl:if test="DLHDon/TTChung/TGia !=''">Tỷ giá:
                                            <xsl:value-of select="format-number(DLHDon/TTChung/TGia, '#.###','vnd')" />
                                        </xsl:if>
                                    </td>
                                </tr>
                            </table>
                            <xsl:choose>
                                <xsl:when test="DLHDon/TTChung/TTHDLQuan!=''">
                                    <div style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt">Hóa đơn
                                        <xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=1">thay thế</xsl:if>
                                        <xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=2">điều chỉnh</xsl:if> cho hóa đơn số
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/SHDCLQuan" />, mẫu số
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/KHMSHDCLQuan" />, ký hiệu
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/KHHDCLQuan" />, ngày
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,9,2)"/> tháng
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,6,2)"/> năm
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,0,5)"/>
                                    </div>
                                </xsl:when>
                                <xsl:otherwise>
                                   
                                </xsl:otherwise>
                            </xsl:choose>
                            <table style="width:100%;text-align:center; font-size:13pt;border-top: 1px solid black;border-bottom:1px solid black;border-left:1px solid black;border-right:1px solid black;" >
                                <tr style="height:30px;">
                                    <td width="5%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black">
                                        <span style="font-size:13pt">
                                            <b>STT
                                                <br/>
                                                <i>(No.)</i>
                                            </b>
                                        </span>
                                    </td>
                                    <td style="border: 1px solid black;" width="30%">
                                        <span style="font-size:13pt;">
                                            <b>Tên hàng hóa, dịch vụ
                                                <br/>
                                                <i>(Name of goods and services)</i>
                                            </b>
                                        </span>
                                    </td>
                                    <td width="7%" style="border: 1px solid black">
                                        <span style="font-size:13pt">
                                            <b>ĐVT
                                                <br/>
                                                <i>(Unit)</i>
                                            </b>
                                        </span>
                                    </td>
                                    <td width="7%" style="border: 1px solid black">
                                        <span style="font-size:13pt">
                                            <b>Số lượng
                                                <br/>
                                                <i>(Quantity)</i>
                                            </b>
                                        </span>
                                    </td>
                                    <td width="8%" style="border: 1px solid black">
                                        <span style="font-size:13pt">
                                            <b>
                      Đơn giá
                                                <br/>
                                                <i>(Unit price
)</i>
                                                <!--<br/> trước thuế<br/> GTGT-->
                                            </b>
                                        </span>
                                    </td>
                                    <td width="10%;border-bottom:1px solid black" style="border: 1px solid black">
                                        <span style="font-size:13pt">
                                            <b>
                      Thành tiền
                                                <br/>
                                                <i>(Total amount)</i>
                                                <!--<br/> trước thuế <br/>GTGT-->
                                            </b>
                                        </span>
                                    </td>
                                    <xsl:choose>
                                        <xsl:when test="$somucthue &gt; 1">
                                            <td width="7%" style="border: 1px solid black">
                                                <span style="font-size:13pt">
                                                    <b>
                          Thuế suất GTGT(%)
                                                        <br/>
                                                        <i>(VAT rate %)</i>
                                                    </b>
                                                </span>
                                            </td>
                                        </xsl:when>
                                        <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                </tr>
                                <tr style="height:80%; font-weight:bold" >
                                    <td style="border-right: 1px solid black;border-left: 1px solid black;border: 1px solid black" width="5%">1</td>
                                    <td style="border-right: 1px solid black;border: 1px solid black">2</td>
                                    <td style="border-right: 1px solid black;border: 1px solid black" width="5%">3</td>
                                    <td style="border-right: 1px solid black;border: 1px solid black" width="7%">4</td>
                                    <td style="border-right: 1px solid black;border: 1px solid black" width="8%">5</td>
                                    <td style="border-right: 1px solid black;border: 1px solid black" width="10%">6=4x5</td>
                                    <xsl:choose>
                                        <xsl:when test="$somucthue &gt; 1">
                                            <td style="border-right: 1px solid black;border: 1px solid black" width="7%">7</td>
                                        </xsl:when>
                                        <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                </tr>
                                <!-- </table>
                        <table style="width:100%;text-align:center; font-size:13pt;border: 1px solid black;paramTableBG; color:black;word-break: break-word;" > -->
                                <xsl:variable name="lien" select="paramlien" />
                                <xsl:choose>
                                    <xsl:when test="$lien='0'">
                                        <xsl:choose>
                                            <xsl:when test="count(DLHDon/NDHDon/DSHHDVu/HHDVu) &lt; 11" >
                                                <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                    <tr style="height:30px;border-top: none!important;border-bottom:1px dotted gray;">
                                                        <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                            <xsl:choose>
                                                                <xsl:when test="contains(.,'|')">
                                                                    <xsl:value-of select=" substring-before(DVTinh,'|')" />
                                                                </xsl:when>
                                                                <xsl:otherwise>
                                                                    <xsl:choose>
                                                                        <xsl:when test="TChat!=4">
                                                                            <xsl:value-of select="STT" />
                                                                        </xsl:when>
                                                                        <xsl:otherwise></xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:otherwise>
                                                            </xsl:choose>
                                                        </td>
                                                        <td style="width:30%;text-align:left;border-right:1px solid black;padding-left:5px">
                                                            <xsl:value-of select="THHDVu" />
                                                        </td>
                                                        <td width="7%" style="text-align:center;border-right:1px solid black">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh !='0'">
                                                                    <xsl:value-of select="DVTinh" />
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="7%" style="text-align:center;border-right:1px solid black">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh!='0'">
                                                                    <xsl:choose>
                                                                        <xsl:when test="SLuong &gt; 1">
                                                                            <xsl:value-of select="format-number(SLuong,'#.###.###,##','vnd')" />
                                                                        </xsl:when>
                                                                        <xsl:otherwise>
                                                                            <xsl:value-of select="format-number(SLuong,'#.###.###.###0,##','vnd')" />
                                                                        </xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="8%" style="text-align:right;border-right:1px solid black; padding-right:5px;">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh!='0'">
                                                                    <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="10%" style="border-right:1px solid black!important;text-align:right;padding-right:5px;">
                                                            <xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
                                                        </td>
                                                        <xsl:choose>
                                                            <xsl:when test="$somucthue &gt; 1">
                                                                <td style="border-right: 1px solid black;text-align:right;padding-right:2px" width="7%">
                                                                    <xsl:choose>
                                                                        <xsl:when test="TChat!=4">
                                                                            <xsl:variable name="vat" select="TSuat" />
                                                                            <xsl:choose>
                                                                                <xsl:when test="$vat='-1'">
                                                                                    <xsl:value-of select="'\'" />
                                                                                </xsl:when>
                                                                                <xsl:otherwise>
                                                                                    <xsl:choose>
                                                                                        <xsl:when test="$vat='0'">
                                                                                            <xsl:value-of select="0" />
                                                                                        </xsl:when>
                                                                                        <xsl:otherwise>
                                                                                            <xsl:choose>
                                                                                                <xsl:when test="$vat='KHAC:3.5%'">5% x 70%</xsl:when>
                                                                                                <xsl:otherwise>
                                                                                                    <xsl:choose>
                                                                                                        <xsl:when test="$vat='KHAC:7%'">10% x 70%</xsl:when>
                                                                                                        <xsl:otherwise>
                                                                                                            <xsl:value-of   select="$vat"/>
                                                                                                        </xsl:otherwise>
                                                                                                    </xsl:choose>
                                                                                                </xsl:otherwise>
                                                                                            </xsl:choose>
                                                                                        </xsl:otherwise>
                                                                                    </xsl:choose >
                                                                                </xsl:otherwise>
                                                                            </xsl:choose >
                                                                        </xsl:when>
                                                                        <xsl:otherwise></xsl:otherwise>
                                                                    </xsl:choose>
                                                                </td>
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose>
                                                    </tr>
                                                </xsl:for-each>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                    <tr style="height:30px;border-top: none!important;border-bottom:1px dotted gray;">
                                                        <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                            <xsl:choose>
                                                                <xsl:when test="contains(.,'|')">
                                                                    <xsl:value-of select=" substring-before(DVTinh,'|')" />
                                                                </xsl:when>
                                                                <xsl:otherwise>
                                                                    <xsl:choose>
                                                                        <xsl:when test="TChat!=4">
                                                                            <xsl:value-of select="STT" />
                                                                        </xsl:when>
                                                                        <xsl:otherwise></xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:otherwise>
                                                            </xsl:choose>
                                                        </td>
                                                        <td style="width:30%;text-align:left;border-right:1px solid black;padding-left:5px">
                                                            <xsl:value-of select="THHDVu" />
                                                        </td>
                                                        <td width="7%" style="text-align:center;border-right:1px solid black">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh !='0'">
                                                                    <xsl:value-of select="DVTinh" />
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="7%" style="text-align:center;border-right:1px solid black">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh!='0'">
                                                                    <xsl:choose>
                                                                        <xsl:when test="SLuong &gt; 1">
                                                                            <xsl:value-of select="format-number(SLuong,'#.###.###,##','vnd')" />
                                                                        </xsl:when>
                                                                        <xsl:otherwise>
                                                                            <xsl:value-of select="format-number(SLuong,'#.###.###.###0,##','vnd')" />
                                                                        </xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="8%" style="text-align:right;border-right:1px solid black;padding-right:5px;">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh!='0'">
                                                                    <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="10%" style="border-right:1px solid black!important;text-align:right;padding-right:5px;">
                                                            <xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
                                                        </td>
                                                        <xsl:choose>
                                                            <xsl:when test="$somucthue &gt; 1">
                                                                <td style="border-right: 1px solid black;text-align:right;padding-right:5px" width="7%">
                                                                    <xsl:choose>
                                                                        <xsl:when test="TChat!=4">
                                                                            <xsl:variable name="vat" select="TSuat" />
                                                                            <xsl:choose>
                                                                                <xsl:when test="$vat='-1'">
                                                                                    <xsl:value-of select="'\'" />
                                                                                </xsl:when>
                                                                                <xsl:otherwise>
                                                                                    <xsl:choose>
                                                                                        <xsl:when test="$vat='0'">
                                                                                            <xsl:value-of select="0" />
                                                                                        </xsl:when>
                                                                                        <xsl:otherwise>
                                                                                            <xsl:choose>
                                                                                                <xsl:when test="$vat='KHAC:3.5%'">5% x 70%</xsl:when>
                                                                                                <xsl:otherwise>
                                                                                                    <xsl:choose>
                                                                                                        <xsl:when test="$vat='KHAC:7%'">10% x 70%</xsl:when>
                                                                                                        <xsl:otherwise>
                                                                                                            <xsl:value-of   select="$vat"/>
                                                                                                        </xsl:otherwise>
                                                                                                    </xsl:choose>
                                                                                                </xsl:otherwise>
                                                                                            </xsl:choose>
                                                                                        </xsl:otherwise>
                                                                                    </xsl:choose >
                                                                                </xsl:otherwise>
                                                                            </xsl:choose >
                                                                        </xsl:when>
                                                                        <xsl:otherwise></xsl:otherwise>
                                                                    </xsl:choose>
                                                                </td>
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose>
                                                    </tr>
                                                </xsl:for-each>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                            <xsl:variable name="line" select="position()" />
                                            <xsl:choose>
                                                <xsl:when test="floor(($line - 1) div 10) = ($lien - 1)">
                                                    <tr style="height:30px;border-top: none!important;border-bottom:1px dotted gray;">
                                                        <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                            <xsl:choose>
                                                                <xsl:when test="TChat!=4">
                                                                    <xsl:value-of select="STT" />
                                                                </xsl:when>
                                                                <xsl:otherwise></xsl:otherwise>
                                                            </xsl:choose>
                                                        </td>
                                                        <td style="width:30%;text-align:left;border-right:1px solid black;padding-left:5px">
                                                            <xsl:value-of select="THHDVu" />
                                                        </td>
                                                        <td width="7%" style="text-align:center;border-right:1px solid black">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh !='0'">
                                                                    <xsl:value-of select="DVTinh" />
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="7%" style="text-align:center;border-right:1px solid black">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh!='0'">
                                                                    <xsl:choose>
                                                                        <xsl:when test="SLuong &gt; 1">
                                                                            <xsl:value-of select="format-number(SLuong,'#.###.###,##','vnd')" />
                                                                        </xsl:when>
                                                                        <xsl:otherwise>
                                                                            <xsl:value-of select="format-number(SLuong,'#.###.###.###0,##','vnd')" />
                                                                        </xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="8%" style="text-align:right;border-right:1px solid black;padding-right:5px;">
                                                            <xsl:choose>
                                                                <xsl:when test="DVTinh!='0'">
                                                                    <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                </xsl:when>
                                                            </xsl:choose >
                                                        </td>
                                                        <td width="10%" style="border-right:1px solid black!important;text-align:right;padding-right:5px;">
                                                            <xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
                                                        </td>
                                                        <xsl:choose>
                                                            <xsl:when test="$somucthue &gt; 1">
                                                                <td style="border-right: 1px solid black;text-align:right;padding-right:5px;padding-right:5px;" width="7%">
                                                                    <xsl:choose>
                                                                        <xsl:when test="TChat!=4">
                                                                            <xsl:variable name="vat" select="TSuat" />
                                                                            <xsl:choose>
                                                                                <xsl:when test="$vat='-1'">
                                                                                    <xsl:value-of select="'\'" />
                                                                                </xsl:when>
                                                                                <xsl:otherwise>
                                                                                    <xsl:choose>
                                                                                        <xsl:when test="$vat='0'">
                                                                                            <xsl:value-of select="0" />
                                                                                        </xsl:when>
                                                                                        <xsl:otherwise>
                                                                                            <xsl:choose>
                                                                                                <xsl:when test="$vat='KHAC:3.5%'">5% x 70%</xsl:when>
                                                                                                <xsl:otherwise>
                                                                                                    <xsl:choose>
                                                                                                        <xsl:when test="$vat='KHAC:7%'">10% x 70%</xsl:when>
                                                                                                        <xsl:otherwise>
                                                                                                            <xsl:value-of   select="$vat"/>
                                                                                                        </xsl:otherwise>
                                                                                                    </xsl:choose>
                                                                                                </xsl:otherwise>
                                                                                            </xsl:choose>
                                                                                        </xsl:otherwise>
                                                                                    </xsl:choose >
                                                                                </xsl:otherwise>
                                                                            </xsl:choose >
                                                                        </xsl:when>
                                                                        <xsl:otherwise></xsl:otherwise>
                                                                    </xsl:choose>
                                                                </td>
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose>
                                                    </tr>
                                                </xsl:when>
                                            </xsl:choose >
                                        </xsl:for-each>
                                    </xsl:otherwise>
                                </xsl:choose>
                                <xsl:choose>
                                    <xsl:when test="$soHHdu &gt; 0">
                                        <xsl:for-each select="(//node())[$soHHdu >= position()]">
                                            <tr style="height:30px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;"></td>
                                                <td style="width:30%;text-align:left;border-right:1px solid black;padding-left:3px"></td>
                                                <td width="7%" style="text-align:center;border-right:1px solid black"></td>
                                                <td width="7%" style="text-align:center;border-right:1px solid black"></td>
                                                <td width="8%" style="text-align:right;border-right:1px solid black"></td>
                                                <td width="10%" style="border-right:1px solid black!important;text-align:right"></td>
                                                <xsl:choose>
                                                    <xsl:when test="$somucthue &gt; 1">
                                                        <td style="border-right: 1px solid black;text-align:right;padding-right:5px" width="7%"></td>
                                                    </xsl:when>
                                                    <xsl:otherwise></xsl:otherwise>
                                                </xsl:choose>
                                            </tr>
                                        </xsl:for-each>
                                    </xsl:when>
                                    <xsl:otherwise></xsl:otherwise>
                                </xsl:choose>
                            </table>
                            <div style="width:100%;display:paramfooter">
                                <div style="idparamTongtien">
                                    <xsl:choose>
                                        <xsl:when test="$somucthue &gt; 1">
                                            <table style="width:100%;text-align:left;border-left:1px solid black;border-top:1px solid black;font-size:11pt;border-right:1px solid black;font-size:11pt; ">
                                                <tr style="border:1px solid black">
                                                    <td style="border-right:1px solid none;text-align:left">
												Tổng tiền chưa có thuế GTGT
                                                        <i>(Total amount without VAT)</i>:
                                                    </td>
                                                    <td style="border-right:1px solid black;text-align:right">
                                                        <xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTCThue, '#.###','vnd')"/>
                                                    </td>
                                                </tr>
                                                <xsl:for-each select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat">
                                                    <xsl:sort select="TSuat" />
                                                    <tr>
                                                        <td style="border-bottom: 1px solid black; border-left: 1px solid black">
  Tổng thuế
                                                            <i>(Total tax)</i>:
                                                            <xsl:choose>
                                                                <xsl:when test="TSuat!='KHAC:7%'">
                                                                    <xsl:choose>
                                                                        <xsl:when test="TSuat!='KHAC:3.5%'">
                                                                            <xsl:value-of   select="TSuat"/>
                                                                        </xsl:when>
                                                                        <xsl:otherwise>5% x 70%</xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:when>
                                                                <xsl:otherwise>10% x 70%</xsl:otherwise>
                                                            </xsl:choose>
                                                        </td>
                                                        <td style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                                                            <xsl:value-of   select="format-number(TThue, '#.###','vnd')"/>
                                                        </td>
                                                    </tr>
                                                </xsl:for-each>
                                                <tr style="border:1px solid black">
                                                    <td style="border-right:1px solid none;text-align:left">Tổng tiền thuế giá trị gia tăng
                                                        <i>(
Total value added tax)</i>:
                                                    </td>
                                                    <td style="border-right:1px solid black;text-align:right">
                                                        <xsl:choose>
                                                            <xsl:when test="TSuat='\'">
                                                                <xsl:value-of select="'\'"/>
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:choose>
                                                                    <xsl:when test="TSuat='0'">
                                                                        <xsl:value-of select="'0'"/>
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTThue, '#.###','vnd')"/>
                                                                    </xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </td>
                                                </tr>
                                                <tr style="border:1px solid black;font-weight:bold">
                                                    <td style="border-right:1px solid nonek;text-align:left">
												Tổng cộng tiền thanh toán
                                                        <i>
													(Total payment
													)
												</i>:
                                                    </td>
                                                    <td style="border-right:1px solid black;text-align:right">
                                                        <xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###','vnd')"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <table style="width: 100%; border: 1px solid black" class="textfont">
                                                <tr style="height: 30px; border-bottom: 1px solid black">
                                                    <td style="border-left: none!important; border-right: none"></td>
                                                    <td style="width:10%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:5%;border-left: none!important; border-right: none;">

											</td>
                                                    <td style="width:5%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:35%;border-left: none!important; border-right: none;text-align:right;">
												Cộng tiền hàng
                                                        <i>(Total)</i>:
                                                    </td>
                                                    <td style="width:20%;border-right: none!important;border-left: none;text-align:right;padding-right:5px;">
                                                        <b>
                                                            <xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTCThue, '#.###','vnd')"/>
                                                        </b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%; border-left: 1px solid black;border-right: 1px solid black" class="textfont">
                                                <tr style="height: 30px; border-bottom: 1px solid black">
                                                    <td style="border-left: none!important; border-right: none;padding-left:5px;">

												Thuế suất GTGT
                                                        <i>(VAT rate)</i>:
                                                        <b>
                                                            <xsl:for-each select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat">
                                                                <xsl:choose>
                                                                    <xsl:when test="TSuat='KHAC:3.5%'">5% x 70%</xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="TSuat='KHAC:7%'">10% x 70%</xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="TSuat" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose>
                                                                <!--<xsl:choose>
                                                    <xsl:when test="STT=1">
                                                        <xsl:variable name="vat" select="TSuat" />
                                                        <xsl:choose>
                                                            <xsl:when test="$vat='-1'">
                                                                <xsl:value-of select="'\'" />
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:choose>
                                                                    <xsl:when test="$vat='0'">
                                                                        <xsl:value-of select="0" />
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$vat='KHAC:3.5%'">5% x 70%</xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$vat='KHAC:7%'">10% x 70%</xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of   select="$vat"/>
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose>
                                                                            </xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:when>
                                                    <xsl:otherwise></xsl:otherwise>
                                                </xsl:choose>-->
                                                            </xsl:for-each>
                                                        </b>
                                                    </td>
                                                    <td style="width:0%;border-left: none!important; border-right: none">

                    </td>
                                                    <td style="width:0%;border-left: none!important; border-right: none;">
                     
                    </td>
                                                    <td style="width:5%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:35%;border-left: none!important; border-right: none;text-align:right;">
                     Tiền thuế GTGT
                                                        <i>(VAT value)</i>:
                                                    </td>
                                                    <td style="width:20%;border-right: none!important;border-left: none;text-align:right;padding-right:5px;">
                                                        <b>
                                                            <xsl:choose>
                                                                <xsl:when test="TSuat='\'">
                                                                    <xsl:value-of select="'\'"/>
                                                                </xsl:when>
                                                                <xsl:otherwise>
                                                                    <xsl:choose>
                                                                        <xsl:when test="TSuat='0'">
                                                                            <xsl:value-of select="'0'"/>
                                                                        </xsl:when>
                                                                        <xsl:otherwise>
                                                                            <xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTThue, '#.###','vnd')"/>
                                                                        </xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:otherwise>
                                                            </xsl:choose>
                                                        </b>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%; border-left: 1px solid black;border-right: 1px solid black" class="textfont">
                                                <tr style="height: 30px; border-bottom: 1px solid black">
                                                    <td style="width:20%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:0%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:15%;border-left: none!important; border-right: none;">
                     
                    </td>
                                                    <td style="width:6%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:38%;border-left: none!important; border-right: none;text-align:right">
                     Tổng cộng tiền thanh toán
                                                        <i>(Total payment)</i>:
                                                    </td>
                                                    <td style="width:20%;border-right: none!important;border-left: none;text-align:right;padding-right:5px;">
                                                        <b>
                                                            <xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###','vnd')"/>
                                                        </b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                    <table style="width:100%;text-align:left;border-left:1px solid black">
                                        <tr style="height:30px;border-right:1px solid black">
                                            <td width="100%" style="border-left:none!important; border-right:none!important; text-align:left;padding-left:3px" colspan="6">
                  Số tiền viết bằng chữ
                                                <i>(In words)</i>:
                                                <xsl:variable name="AmountWord" select="DLHDon/NDHDon/TToan/TgTTTBChu"/>
                                                <b>
                                                    <xsl:value-of select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))"/>
                  ./.
                                                </b>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%;border-top:1px solid black" class="textfont">
                                        <tr>
                                            <td style="border: none; padding-top: 5px; text-align: center;width:30%">
                                                <b>Người mua hàng</b>
                                                <i>(Buyer)</i>
                                                <br />
                (Ký, ghi rõ họ tên)
                                                <br/>
                                                <i>(Signature and full name)</i>
                                            </td>
                                            <td style="border: none; padding-top: 5px; text-align: center;width:40%">
                                                <div style="paramNguoiCD">
                                                    <b> Người chuyển đổi</b>
                                                    <i>(Converter)</i>
                                                    <br />
                  (Ký, ghi rõ họ tên)
                                                    <i>
                                                        <br/>
                    (Signature and full name)
                                                    </i>
                                                </div>
                                            </td>
                                            <td style="border: none; padding-top: 5px; text-align: center;width:30%">
                                                <b> Người bán hàng</b>
                                                <i>(Seller)</i>
                                                <br />
                (Ký, ghi rõ họ tên)
                                                <br/>
                                                <i>(Signature and full name)</i>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%"></td>
                                            <td style="width: 40%"></td>
                                            <td style="text-align:right; height:80px; width: 30%;text-align:center;">
                                                <div style="paramSign;background-image:url(data:image/jpeg;base64,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); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid #249F38; text-align:left;padding-top:5px; ">
                                                    <span style="color:red;">
                                                        <b> Signature valid</b>
                                                        <br/>
                    Được ký bởi:
                                                        <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
                                                        <br/>
                    Ngày ký:
                                                        <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],9,2)"/>-
                                                        <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],6,2)"/>-
                                                        <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],0,5)"/>
                                                    </span>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="text-align: center; border: none; padding-top: 3px;padding-bottom:3px">
                (Cần kiểm tra đối chiếu khi lập, giao, nhận hoá đơn)
              </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div  style="padding-top:0px;text-align:left;padding-bottom:10px;font-size:11.5pt;align:center;px;-ms-transform: rotate(-90deg);-webkit-transform: rotate(-90deg);transform: rotate(-90deg);width:900px;left:485px;top:-700px;float:right;height:15px;position:relative;">
                                <i>
              Giải pháp hóa đơn điện tử được cung cấp bởi:
                                    <b> Công ty cổ phần công nghệ thẻ Nacencomm</b>. Mã số thuế:
                                    <b>0103930279</b>.
                                </i>
                            </div>
                        </div>
                        <div style="width:100%;padding-top:3px;text-align:center;padding-bottom:3px;">
                            <span>
              Chuỗi xác thực
                                <i>(Digest Value)</i>:
                                <b >
                                    <xsl:value-of select="$digest" />
                                </b>
                            </span>
                        </div>
                        <div style="width:100%;padding-top:3px;text-align:center;padding-bottom:3px;">
                            <span>
                                <i>Tra cứu hóa đơn tại địa chỉ trang web: https://hoadon78.nacencomm.vn </i>
                            </span>
                        </div>
                    </div>
                    <xsl:variable name="lien" select="paramlien" />
                    <xsl:choose>
                        <xsl:when test="$lien &gt; 1">
                            <div style="text-align:center;padding-top:0px">
                Tiep theo trang truoc -
                                <span style="text-align:center;padding-top:3px"> param3 </span>
                            </div>
                        </xsl:when>
                        <xsl:otherwise>
                            <div style="text-align:center;padding-top:3px"> param3 </div>
                        </xsl:otherwise>
                    </xsl:choose>
                </body>
            </page>
        </html>
    </xsl:template>
</xsl:stylesheet>