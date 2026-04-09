<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:ex="http://exslt.org/dates-and-times" xmlns:fn="http://www.w3.org/2005/02/xpath-functions"
                xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
    <xsl:output method="html" />
    <xsl:param name="imgLogo" />
    <xsl:param name="paramlien" />
    <xsl:param name="percent" select="''" />
    <xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
    <xsl:template match="HDon">
        <xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
        <xsl:variable name="tax" select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat" />
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
                </style>
            </head>
            <body style="font-family:Times New Roman">
                <!--<div style="viewstyle;width:100%">-->
                <div style="viewstyle;border:none">
                    <div id="background" style="paramMau">
                        MẪU
                        
                    </div>
                    <div id="background" style="paramdisable">contentDisable</div>
                    <div style="border:2px solid black;width:100%;height: auto; min-height: 100%;background-image:url(paramVien);
                                border-color: white;
                                background-size: 100% 100%;
                                background-clip: padding-box;
                                box-sizing: border-box;
                                padding: 20px;
                                border: 20px solid transparent;
                                border-width:20px;z-index:1;border-color:white">
                        <div id="header" style="display:flex;flex-direction:paramOpacityHeaderFlexDirection;;padding-top:10px;padding-right:10px">
                            <div id="header_left" style="width:30%;">
                                <span style="ten_cong_ty_css_display;;ten_cong_ty_css;">
                                    Đơn vị:
                                    <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />      
                                </span>
                                <br />
                                <span style="mst_css_display;;mst_css;">
                                    MST:
                                    
                                    <xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
                                    
                                </span>
                                <br /> 
                                <span style="dia_chi_css_display;;dia_chi_css;">
                                    Địa chỉ:
                                    <xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
                                </span>
                                
                            </div>
                            <div id="header_center" style="flex:1;text-align:center">
                                <span style="font-weight:bold; font-size:12.5pt;">
                                    CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
                                    <br />
                                    <u> Độc lập - Tự do - Hạnh phúc</u>
                                </span>
                            </div>
                            <div id="header_right" style="width:20%;">
                                Mẫu số &#160;:
                                <b>
                                    <xsl:value-of select="DLHDon/TTChung/KHMSHDon" />
                                </b>
                                <br />
                                Ký hiệu&#160;:
                                <b>
                                    <xsl:value-of select="DLHDon/TTChung/KHHDon" />
                                </b>
                                <br />
                                Số &#160;:
                                <span style="color: red;font-size:16pt">
                                    <xsl:value-of select="substring(
                                            concat('0000000', DLHDon/TTChung/SHDon), 
                                            string-length(DLHDon/TTChung/SHDon) + 1, 
                                            7
                                        )" />
                                </span>
                                <br />
                                <div
                                    style="color: red; font-size: 10pt;padding-top:5px;  display: normal;text-align:center">
                                    <div style="paramChuyendoi">
                                        HOÁ ĐƠN CHUYỂN ĐỔI
                                        <br />
                                        TỪ HOÁ ĐƠN ĐIỆN TỬ
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <div>
                            <table style="width:100%;color:#A52A2A">
                                <tr>
                                    <!-- <td></td> -->
                                    <td style="width:100%;text-align:center">
                                        <span style="font-weight:bold; font-size:14pt;text-transform: uppercase;">
                                            <xsl:value-of select="DLHDon/TTChung/THDon" />
                                        </span>
                                        <br />
                                        <span style="font-weight:normal;font-size:10.5pt;display:param1_1">param1</span>
                                        <br/>
                                        <span style="text-align:center;font-size:11pt">Tên loại phí,lệ phí:
                                            <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                <xsl:value-of select="THHDVu" />
                                            </xsl:for-each>
                                        </span>
                                        
                                    </td>
                                    <!-- <td></td> -->
                                </tr>
                            </table>
                            <table style="color:#A52A2A">
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
                            <xsl:choose>
                                <xsl:when test="DLHDon/TTChung/TTHDLQuan!=''">
                                    <div style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt">
                                        Hóa đơn
                                        <xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=1">thay thế</xsl:if>
                                        <xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=2">điều chỉnh</xsl:if> cho hóa đơn số
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/SHDCLQuan" />, mẫu số
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/KHMSHDCLQuan" />, ký hiệu
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/KHHDCLQuan" />, ngày
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,9,2)" /> tháng
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,6,2)" /> năm
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,0,5)" />
                                    </div>
                                </xsl:when>
                                <xsl:otherwise>
                                    
                                </xsl:otherwise>
                            </xsl:choose>
                            <table style="width:100%;don_vi_mua_hang_css_display;">
                                <tr style="height:27px;don_vi_mua_hang_css;">
                                    <td style="padding-left:20px;width:12%">
                                        Tên người mua hàng:
                                    </td>
                                    <td style="border-bottom:1px dotted black;width:88%">
                                        <xsl:value-of select="DLHDon/NDHDon/NMua/HVTNMHang" />
                                    </td>
                                </tr>
                                <xsl:if test="DLHDon/NDHDon/NMua/CCCDan!=''">
									<tr>
										<td style="padding-left:10px;">
											Số CCCD:
                                            										</td>
										<td>
											<xsl:value-of select="DLHDon/NDHDon/NMua/CCCDan" />
										</td>
									</tr>
							</xsl:if>
                            </table>
                            <table style="width:100%;mst_nguoi_mua_css_display;">
                                <tr style="height:27px;mst_nguoi_mua_css;">
                                    <td style="padding-left:20px;width:13%">
                                        Mã số thuế :
                                        
                                    </td>
                                    <td style="border-bottom:1px dotted black;width:87%">
                                        <xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
                                    </td>
                                </tr>
                                <xsl:if test="DLHDon/NDHDon/NMua/MDVQHNSach!=''">
									<tr>
										<td style="padding-left:10px;">
											Mã số ĐVQHNS
											<i>
												(Budget Code)
											</i>:
										</td>
										<td>
											<xsl:value-of select="DLHDon/NDHDon/NMua/MDVQHNSach" />
										</td>
									</tr>
								</xsl:if>
                            </table>
                            <table style="width:100%;dia_chi_nguoi_mua_css_display;">
                                <tr style="height:27px;dia_chi_nguoi_mua_css;">
                                    <td style="padding-left:20px;width:10%">
                                        Địa chỉ :
                                        
                                    </td>
                                    <td style="border-bottom:1px dotted black;width:90%">
                                        <xsl:value-of select="DLHDon/NDHDon/NMua/DChi" />
                                    </td>
                                </tr>
                            </table>
                            
                            <table style="width:100%;color:#A52A2A">
                                <tr style="height:27px">
                                    <td style="padding-left:20px;width:10%">
                                        Số tiền :
                                        
                                    </td>
                                    <td style="border-bottom:1px dotted black;width:90%;font-weight:bold">
                                        <xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###','vnd')" />
                                        <xsl:if test="DLHDon/NDHDon/TToan/TgTTTBSo=0">0</xsl:if>
                                    </td>
                                </tr>
                            </table>
                            
                            <table style="width:100%;color:#A52A2A;">
                                <tr style="height: 27px; border-bottom: 1px none black">
                                    <td
                                        style="padding-left:20px;border-left: none!important; border-right: none!important; text-align: left;width:15%">
                                        Viết bằng chữ:
                                    </td>
                                    <td style="border-bottom:1px dotted black;width:85%;font-weight:bold">
                                        <i>
                                            <xsl:variable name="AmountWord" select="DLHDon/NDHDon/TToan/TgTTTBChu" />
                                            <xsl:value-of
                                                select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))" />
                                        </i>
                                    </td>
                                </tr>
                            </table>
                            <table style="width:100%;color:#A52A2A">
                                <tr style="height:27px">
                                    <td style="padding-left:20px;width:20%">
                                        Hình thức thanh toán :
                                    </td>
                                    <td style="border-bottom:1px dotted black;width:80%">
                                        <xsl:value-of select="DLHDon/TTChung/HTTToan" />
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;color:#A52A2A;" class="textfont">
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center;width:30%">
                                        <span style="font-size:13pt">
                                            <i> Ngày&#160;
                                                <xsl:variable name="string">
                                                    <xsl:value-of select="substring(DLHDon/TTChung/NLap,9,2)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string" />
                                                
                                                tháng &#160;
                                                <xsl:variable name="string1">
                                                    <xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string1" />
                                                năm&#160;
                                                <xsl:variable name="string2">
                                                    <xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)" />
                                                </xsl:variable>
                                                <xsl:value-of select="$string2" />
                                            </i>
                                        </span>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border: none; padding-top: 10px; text-align: center;width:30%">
                                        <b>Người nộp tiền</b>
                                        <br />
                                        <i>(Ký, ghi rõ họ tên)</i>
                                    </td>
                                    <td style="border: none; padding-top: 10px; text-align: center;width:40%">
                                        <div style="paramNguoiCD">
                                            <b> Người chuyển đổi</b>
                                            <br />
                                            <i>(Ký, ghi rõ họ tên)</i>
                                        </div>
                                    </td>
                                    <td style="border: none; padding-top: 10px; text-align: center;width:30%">
                                        <b>Người thu tiền</b>
                                        <br />
                                        <i>(Ký, ghi rõ họ tên)</i>
                                    </td>
                                </tr>
                                <br />
                                <tr>
                                    <td style="width: 30%;padding-top:30px">
                                        
                                        <span style="font-size:10pt;">
                                            Giải pháp hóa đơn điện tử được cung cấp bởi:
                                            <b> Công ty cổ phần công nghệ thẻ Nacencomm</b>.
                                            <br /> Mã số thuế:
                                            <b>0103930279</b>.
                                        </span>
                                        
                                    </td>
                                    <td style="width: 40%;padding-left:20px">
                                        
                                    </td>
                                    <td style="text-align:right; padding-top: 5px; width: 30%;text-align:center;font-size:10pt">
                                        <div
                                            style="paramSign;background-image:url(data:image/jpeg;base64,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); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid red; text-align:left;padding-top:5px; ">
                                            <span style="color:red;">
                                                <b> Signature valid</b>
                                                <br />
                                                Được ký bởi:
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
                                                <br />
                                                Ngày ký:
                                                <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],9,2)" />-
                                                <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],6,2)" />-
                                                <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],0,5)" />
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td colspan="3" style="text-align: center; border: none; padding-top: 30px;">
                                        <span>
                                            Chuỗi xác thực :
                                            <b>
                                                <xsl:value-of select="$digest" />
                                            </b>
                                            <br />
                                          <i>Tra cứu hóa đơn tại địa chỉ trang web: https://ca2einvoice.nacencomm.vn </i><xsl:if test="DLHDon/TTChung/TTKhac/TTin/TTruong='MTCuu'">&#160;&#160;&#160;&#160;Mã tra cứu: <b><xsl:value-of select="DLHDon/TTChung/TTKhac/TTin/DLieu" /></b></xsl:if>
                                        </span><br/>
                                        (Cần kiểm tra đối chiếu khi lập, giao, nhận hoá đơn)
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    
                    <div style="width:100%;padding-top:15px;text-align:center;padding-bottom:5px;color:#A52A2A">
                        <br />
                    </div>
                    
                    <div style="width:100%;padding-top:0px;text-align:center;padding-bottom:10px;color:#A52A2A;">
                        
                    </div>
                </div>
                
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>