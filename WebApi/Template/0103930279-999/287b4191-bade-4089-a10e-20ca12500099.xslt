<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ex="http://exslt.org/dates-and-times" xmlns:fn="http://www.w3.org/2005/02/xpath-functions" xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
    <xsl:output method="html" />
    <xsl:param name="imgLogo" />
    <xsl:param name="imgStamp" />
    <xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
    <xsl:decimal-format name="usd" decimal-separator="." grouping-separator="," />
    <xsl:param name="percent" select="''" />
    <xsl:template match="HDon">
        <xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
        <!--  <xsl:variable name="tax" select="inv:invoiceData/inv:invoiceTaxBreakdowns/inv:invoiceTaxBreakdown/inv:vatPercentage" /> -->
        <xsl:variable name="xetphi" select="count(DLHDon/NDHDon/DSHHDVu/HHDVu[DVTinh='0*1'])" />
        <xsl:variable name="soHHdu" select="10-(count(DLHDon/NDHDon/DSHHDVu/HHDVu/STT))" />
        <xsl:variable name="currency" select="DLHDon/TTChung/DVTTe" />
        <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
            <head>
                <title>E-invoice</title>
                <meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8' />
                <style type="text/css">
          #tblContent td, #tblContent th {
          border:1px solid black;
          padding:0;
          margin:0;
          //font-size:11pt;
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
          // font-size:11pt;
          }

          .textfont{
          font-size:9pt;
          }
          du {
          letter-spacing: 5px;
          }
        </style>
            </head>
            <body>
                <div style="viewstyle;border:none;">
                    <div id="background" style="paramMau">
						MẪU
					</div>
                    <div id="background" style="paramdisable">
						contentDisable
					</div>
                
                    <div style="width:870px;border:2px solid black;">
                       
                        <table style="width:100%;line-height:25px">
                            <tr>
                                <td style="padding-left:20px;vertical-align: text-top;">
                            Đơn vị xuất hàng    :
                                &#160;&#160;&#160;&#160;
                                    <span style="font-weight:bold; font-size:12pt">
                                        <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
                                    </span>
                                </td>
                                <td rowspan="3">
                                    <img id="qrcode" src="paramqrcode" alt="" style="width:100px;height:100px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left:20px;">                    
                            Mã số thuế:
                       &#160;&#160;&#160;&#160;
                                    <span style="font-weight:bold; font-size:12pt">
                                        <du>
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
                                        </du>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left:20px;vertical-align: text-top;">
       Địa chỉ kho xuất hàng:&#160;&#160;&#160;&#160; <xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left:20px;vertical-align: text-top;">
          Mẫu số
                      :
                                    <b>
                                        <xsl:value-of select="DLHDon/TTChung/KHMSHDon" />
                                    </b>
                                   &#160;&#160;&#160;
                                    Ký hiệu
                        :
                                    <b>
                                        <xsl:value-of select="DLHDon/TTChung/KHHDon" />
                                    </b>
                                  &#160;&#160;&#160;
                                    Số
                       :
                                    <span style="color: red;font-size:16pt">
                                        <xsl:value-of select="substring(
concat('0000000', DLHDon/TTChung/SHDon), 
string-length(DLHDon/TTChung/SHDon) + 1, 
7
)" />
                                    </span>
                                </td>
                            </tr>
                           
                        </table>
                        <hr style="background-color:black;width:100%;height:1px;margin-bottom:1px" />
                        <table style="width:100%;">
                            <tr>
                              
                                <td style="width:100%;padding-top:5px;text-align:center; ">
                                    <span style="font-weight:bold; font-size:15pt;text-transform: uppercase;">
                                        <!-- <xsl:value-of select="DLHDon/TTChung/THDon" /> -->
                                        <xsl:value-of select="DLHDon/TTChung/THDon" />
                                    </span>
                                    <br />
                                    <span style="font-weight:normal;font-size:10.5pt;display:param1_1">param1</span>
                                </td>
                                <!-- <td style="width:30%;padding-left:40px;padding-top:20px;">
                                 
                                </td> -->
                            </tr>
                            <tr>
                                <td style="text-align:center;">
                                    <span style="font-size:10.5pt">
                                        Ngày
                            &#160;
                                        <xsl:variable name="string">
                                            <xsl:value-of select="substring(DLHDon/TTChung/NLap,9,2)" />
                                        </xsl:variable>
                                        <xsl:value-of select="$string" />
                                        tháng
                            &#160;
                                        <xsl:variable name="string1">
                                            <xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)" />
                                        </xsl:variable>
                                        <xsl:value-of select="$string1" />
                                        năm
                            &#160;
                                        <xsl:variable name="string2">
                                            <xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)" />
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
                                <td style="padding-left:200px">
                                    <b>
                                        <xsl:if test="MCCQT !=''">
                                            MÃ CQT CẤP:
                                            <xsl:value-of select="MCCQT" />
                                        </xsl:if>
                                    </b>
                                </td>
                            </tr>
                        </table>
                        <hr style="background-color: black; width: 100%; height: 2px;border:none" />
                        <table style="width: 100%;line-height:25px">
                            <tr style=";padding-left:10px;">
                                <td style="padding-left:20px">
                                    Căn cứ lệnh điều động số:
                                  
                                </td>
                                <td> <xsl:value-of select="DLHDon/NDHDon/NBan/LDDNBo" /></td>
                            </tr>
                            <tr style=";padding-left:10px;">
                                <td style="padding-left:20px">
                                    Hợp đồng số :
                                  
                                </td>
                                <td>   <xsl:value-of select="DLHDon/NDHDon/NBan/HDSo" /></td>
                            </tr>
                            <tr style="padding-left:10px;">
                                <td style="padding-left:20px">
                                    Họ tên người vận chuyển:                               
                                                                    </td>
                                <td>   <xsl:value-of select="DLHDon/NDHDon/NBan/TNVChuyen" /> </td>
                            </tr>
                            <tr style=";padding-left:10px;">
                                <td style="padding-left:20px">
                                    Phương tiện vận chuyển:                                   
                                </td>
                                <td>  <xsl:value-of select="DLHDon/NDHDon/NBan/PTVChuyen" /></td>
                            </tr>
                            
                            <tr style="height:25px">
                                <td style="padding-left:20px">
                                    Tên người xuất hàng:
                                   
                                </td>
                               <td> <xsl:value-of select="DLHDon/NDHDon/NBan/HVTNXHang" /> </td>
                            </tr>
                            <tr style="height:25px">
                                <td style="padding-left:20px">
                                    Tên người nhận hàng:
                                   
                                </td>
                                <td>  <xsl:value-of select="DLHDon/NDHDon/NMua/HVTNNHang" /></td>
                             
                            </tr>
                            <tr>
                                <td style="padding-left:20px">
                                    Mã số thuế người nhận:        
                                </td>
                                 <td>  <xsl:value-of select="DLHDon/NDHDon/NMua/MST" /></td>
                            </tr>
                         
                            <tr>
                                <td style="padding-left:20px;vertical-align: text-top;">
                                    Nhập tại kho nhận:
                                   
                                </td>
                                 <td>  <xsl:value-of select="DLHDon/NDHDon/NMua/DChi" /></td>
                            </tr>
                            <tr>
                                <td style="padding-left:20px;vertical-align: text-top;">
                                   Đơn vị tiền tệ:
                                   
                                </td>
                                 <td> <xsl:value-of select="DLHDon/TTChung/DVTTe" /> </td>
                            </tr>
                            
                        </table>
                        <xsl:choose>
                            <xsl:when test="DLHDon/TTChung/TTHDLQuan!=''">
                                <div style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt">
									Hóa đơn
                                    <xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=1">thay thế</xsl:if>
                                    <xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=2">điều chỉnh</xsl:if> cho hóa đơn số 
                                    <xsl:value-of select="substring(
  concat('0000000', TTChung/TTHDLQuan/SHDCLQuan), 
  string-length(DLHDon/TTChung/TTHDLQuan/SHDCLQuan) + 1, 
  7
)"/>
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
                        <table style="width:100%;text-align:center; border: 1px solid black;font-size:12pt;height:30px;border-left: none;border-right: none;word-break: break-word">
                            <tr>
                                <th style="border: 1px solid black;text-align:center;" width="5%" rowspan="2">STT</th>
                                <th style="border: 1px solid black;text-align:center;" rowspan="2">
                                   Tên vật tư, hàng hóa
                                </th>
                                <!-- <th style="border: 1px solid black;text-align:center;" width="20%" rowspan="2">Mã số</th> -->
                                <th style="border: 1px solid black;text-align:center;" width="10%" rowspan="2">
                                    Đơn vị tính
                                  
                                
                                </th>
                                <th style="border: 1px solid black;text-align:center;" width="10%" colspan="2">Số lượng thực xuất</th>
                                <th style="border: 1px solid black;text-align:center;" width="15%" rowspan="2">Đơn giá</th>
                                <th style="border: 1px solid black;text-align:center;" width="15%" rowspan="2">Thành tiền</th>
                            </tr>
                            
                        </table>
                        <table style="width:100%;text-align:center; font-size:12pt;border: 1px solid black;border-left: none;border-right: none;word-break: break-word;">
                            <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                <tr style="height:30px;border-top: none!important;border-bottom:1px dotted black;">
                                    <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                        <xsl:choose>
                                            <xsl:when test="$xetphi &gt; 0">
                                                <xsl:choose>
                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                        <xsl:variable name="bientru" select="substring-after(DVTinh,'*')" />
                                                        <xsl:value-of select="STT - $bientru" />
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:value-of select="''" />
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:value-of select="STT " />
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                    <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                        <xsl:choose>
                                            <xsl:when test="contains(.,'|')">
                                                <xsl:value-of select="substring-before(THHDVu,'|')" />
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:value-of select="THHDVu" />
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                    <!-- <td  width="20%" style="text-align:center;border-right:1px solid black">
                                                <xsl:value-of select="MHHDVu" />
                                                       
                                                    </td> -->
                                    <td width="10%" style="text-align:center;border-right:1px solid black">
                                        <xsl:choose>
                                            <xsl:when test="$xetphi &gt; 0">
                                                <xsl:choose>
                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                        <xsl:value-of select="substring-before(DVTinh,'*')" />
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:value-of select="''" />
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:choose>
                                                    <xsl:when test="DVTinh !='0'">
                                                        <xsl:value-of select="DVTinh" />
                                                    </xsl:when>
                                                    <xsl:otherwise></xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                    <td width="10%" style="text-align:center;border-right:1px solid black">
                                        <xsl:choose>
                                            <xsl:when test="$xetphi &gt; 0">
                                                <xsl:choose>
                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                        <xsl:value-of select="format-number(SLuong,'#.###.###.###,##','vnd')" />
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:value-of select="''" />
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:choose>
                                                    <xsl:when test="DVTinh !='0'">
                                                        <xsl:choose>
                                                            <xsl:when test="SLuong &gt; 1">
                                                                <xsl:value-of select="format-number(SLuong,'#.###.###.###,##','vnd')" />
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:value-of select="format-number(SLuong,'#.###.###.###0,##','vnd')" />
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </xsl:when>
                                                    <xsl:otherwise></xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                    <!-- <td width="5%" style="text-align:center;border-right:1px solid black">
                                                        <xsl:choose>
                                                            <xsl:when test="$xetphi &gt; 0">
                                                                <xsl:choose>
                                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                        <xsl:value-of select="format-number(SLuong,'#.###.###.###,##','vnd')" />
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:value-of select="''" />
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:choose>
                                                                    <xsl:when test="DVTinh !='0'">
                                                                        <xsl:choose>
                                                                            <xsl:when test="SLuong &gt; 1">
                                                                                <xsl:value-of select="format-number(SLuong,'#.###.###.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(SLuong,'#.###.###.###0,##','vnd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:when>
                                                                    <xsl:otherwise></xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </td> -->
                                    <td width="15%" style="text-align:right;border-right:1px solid black">
                                        <xsl:choose>
                                            <xsl:when test="$xetphi &gt; 0">
                                                <xsl:choose>
                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                        <xsl:choose>
                                                            <xsl:when test="$currency='VND'">
                                                                <xsl:value-of select="format-number(DGia, '#.###','vnd')" />
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:value-of select="''" />
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:choose>
                                                    <xsl:when test="DVTinh !='0'">
                                                        <xsl:choose>
                                                            <xsl:when test="$currency='VND'">
                                                                <xsl:value-of select="format-number(DGia, '#.###','vnd')" />
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </xsl:when>
                                                    <xsl:otherwise></xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                    <td width="15%" style="border-right:1px solid black!important;text-align:right">
                                        <xsl:choose>
                                            <xsl:when test="$xetphi &gt; 0">
                                                <xsl:choose>
                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                        <xsl:choose>
                                                            <xsl:when test="$currency='VND'">
                                                                <xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:value-of select="''" />
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:choose>
                                                    <xsl:when test="$currency='VND'">
                                                        <xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                </tr>
                            </xsl:for-each>
                            <xsl:choose>
                                <xsl:when test="$soHHdu &gt; 0">
                                    <xsl:for-each select="(//node())[$soHHdu >= position()]">
                                        <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                            <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;"></td>
                                            <td style="text-align:left;border-right:1px solid black;padding-left:3px"></td>
                                            <td width="10%" style="text-align:center;border-right:1px solid black"></td>
                                            <td width="10%" style="text-align:center;border-right:1px solid black"></td>
                                            <td width="15%" style="text-align:right;border-right:1px solid black"></td>
                                            <td width="15%" style="border-right:1px solid black!important;text-align:right"></td>
                                        </tr>
                                    </xsl:for-each>
                                </xsl:when>
                                <xsl:otherwise></xsl:otherwise>
                            </xsl:choose>
                        </table>
                        <table style="width: 100%; border-bottom: 1px solid black;border-left: none;border-right: none">
                            <tr style="height: 30px; border-bottom: 1px solid black;">
                                <td style="border-left: none!important; border-right: none;padding-left:10px;">
                                    <i>	Tổng cộng:</i>
                                </td>
                                <td style="width:10%;border-left: none!important; border-right: none"></td>
                                <td style="width:15%;border-left: none!important; border-right: none"></td>
                                <td style="width:10%;border-left: none!important; border-right: none"></td>
                                <td style="width:10%;border-left: none!important; border-right: none"></td>
                                <td style="width:10%;border-right: none!important;border-left: none;text-align:right">
                                    <i>
                                        <xsl:value-of select="format-number(DLHDon/TTKhac/TTin[TTruong='TgTTTBSo']/DLieu, '#.###','vnd')" />
                                    </i>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%; text-align: left; border-bottom: 1px none black; border-bottom: 1px solid black;border-left: none;border-right: none">
                            <tr style="height: 30px; border-bottom: 1px none black">
                                <td style="width:100%;   text-align: left;padding-left:10px;">
                                    Số tiền viết bằng chữ:
                                    <xsl:variable name="AmountWord" select="DLHDon/TTKhac/TTin[TTruong='TgTTTBChu']/DLieu" />
                                    <xsl:value-of select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))" />
                                </td>
                            </tr>
                        </table> 
                        <table style="width: 100%; border-top: none;" class="textfont">
                            <tr>
                                <td style="border: none; padding-top: 2px; text-align: center;width:20%">
                                    Người nhận hàng
                                    <br />
                                    (Ký, ghi rõ họ tên)
                                </td>
                                <td style="border: none; padding-top: 2px; text-align: center;width:20%">
                                    <div style="">
                    Thủ kho nhập
                                        <br />
                    (Ký, ghi rõ họ tên)
                                    </div>
                                </td>
                                <td style="border: none; padding-top: 2px; text-align: center;width:20%">
                                    <div style="paramNguoiCD">
                                        Ngày... tháng... năm....
                                        <br />
                                        Người chuyển đổi
                                        <br />
                                        (Ký, ghi rõ họ tên)
                                        <!--<i>
                      <br/>(Signature and full name)
                    </i>-->
                                    </div>
                                </td>
                                <td style="border: none; padding-top: 2px; text-align: center;width:20%">
                                    <div style="">
                   Thủ kho xuất
                                        <br />
                    (Ký, ghi rõ họ tên)
                                    </div>
                                </td>
                                <td style="border: none; padding-top: 2px; text-align: center;width:20%">
                                    Đơn vị xuất hàng
                                    <i></i>
                                    <br />
                                    (Ký, ghi rõ họ tên)
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%"></td>
                                <td style="border: none; padding-top: 2px; text-align: center;"></td>
                                <td ></td>
                                <td ></td>
                                <td style="text-align:center;width: 20%;text-align:center;height:80px;">
                                    <div style="paramSign;background-image:url(data:image/jpeg;base64,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); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid red; text-align:left;padding-top:5px; ">
                                        <span style="color:red;font-size:12pt">
                                            <!--<b> Signature valid</b><br/>-->
                                            Được ký bởi:
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
                                            <br />
                                            Ngày ký:
                                            <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],9,2)" />
                                            -
                                            <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],6,2)" />
                                            -
                                            <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],0,5)" />
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            
                        </table>
                        <div style="vertical-align:bottom;font-size:12pt;text-align:center;">
                            <i> (Cần kiểm tra đối chiếu khi lập, giao, nhận hoá đơn)</i>
                        </div>
                    </div>
                    <div style="width:100%;text-align:center;font-size:12pt">
                        <span>
                            Chuỗi xác thực:
                            <b>
                                <xsl:value-of select="$digest" />
                            </b>
                        </span>
                    </div>
                  
                    <div style="width:100%;text-align:center;font-size:12pt">
                        <span>
                            <i>Tra cứu hóa đơn tại địa chỉ trang web: https://ca2einvoice.nacencomm.vn/ </i>
                        </span>
                    </div>
                    <div style="word-spacing:2px;font-size:12pt;text-align:center">
                        <i>
                            Giải pháp hóa đơn điện tử được cung cấp bởi:
                            <b> Công ty cổ phần công nghệ thẻ Nacencomm</b>
                            . Mã số thuế:
                            <b>0103930279</b>
                            .
                        </i>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>