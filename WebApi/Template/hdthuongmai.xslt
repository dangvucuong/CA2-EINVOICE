<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:ex="http://exslt.org/dates-and-times"
                xmlns:fn="http://www.w3.org/2005/02/xpath-functions"
                xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
	<xsl:output method="html" />
	<xsl:param name="imgLogo" />
	<xsl:param name="percent" select="''" />
    <xsl:param name="paramlien" select="'1'" />
	<xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
	<xsl:decimal-format name="usd" decimal-separator="." grouping-separator="," />

	<xsl:template match="HDon">
	        <xsl:variable name="xetphi" select="count(DLHDon/NDHDon/DSHHDVu/HHDVu[DVTinh='0*1'])" />

		<xsl:variable name="currency" select="DLHDon/TTChung/DVTTe" />
		<!-- <xsl:variable name="digest1" select="(//*[local-name() = 'DigestValue'])[1]" />
      <xsl:variable name="digest2" select="(//*[local-name() = 'DigestValue'])[2]" /> -->
		
		<xsl:variable name="tax" select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat" />
		<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
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
        font-size:12pt;  
        font-family:"Times New Roman";
        color:Black;
        }
				</style>
			</head>
			<body style="font-family:Times New Roman" class="textfont">

				<div style="width:900px;margin:auto; padding-top:20px;z-index:1;width:900px;margin:auto; padding-top:20px;z-index:1;background-image: url(''); background-size:80%; background-position: 50% 50%;background-color: hsla(0,0%,100%,0.8);background-blend-mode: overlay;background-repeat:no-repeat; width:900px; margin:auto;  ">
				  <div id="background" style="paramMau">
              MẪU
            </div>
            <div id="background" style="paramdisable">contentDisable</div><div style="border:2px solid black;width:870px;border-image:url(data:image/jpg;base64,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) 30 round;border-width:20px;z-index:1;margin:auto">

						<table style="width:100%;" class="textfont">
							<tr style="ten_cong_ty_css_display;">
								<td style="text-align:center; padding-top:10px;padding-left:0px!important;width:200px" rowspan="6" >
									<img style="width:200px;height:100px;align-content:center;position:static;left:0;top:0;object-fit: scale-down;"
															  id="imgSample" src="paramLogo">
									</img>
								</td>
								<td >
									<span style="font-weight:bold; font-size:15pt;color:Black">
										<xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
									</span>
								</td>
							</tr>
							<tr style="mst_css_display;">
								<td >

									<span style="font-weight:bold;">
										Mã số thuế <i>(Tax code)</i>:
										<du>
											<xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
										</du>
									</span>
								</td>
							</tr>
							<tr style="dia_chi_css_display;">
								<td style="dia_chi_css;">
									Địa chỉ <i>(Address)</i>:
									<xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
								</td>
							</tr>

							<tr style="dien_thoai_css_display;" >
								<td >
                   <span style="display:normal">
									Điện thoại <i>(Tel)</i>:   &#160;&#160;&#160; <xsl:value-of select="DLHDon/NDHDon/NBan/SDThoai" />
									&#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160; </span>
                  <span style="display:none">
                    Fax:  <xsl:value-of select="DLHDon/NDHDon/NBan/Fax" />
                  </span>
								</td>
							</tr>
							<tr style="so_tai_khoan_css_display;">
								<td style="so_tai_khoan_css;">
                  <span style="display:normal">
                    Số tài khoản <i>(Account No)</i>:
                    &#160;&#160;
                    <xsl:value-of select="DLHDon/NDHDon/NBan/STKNHang" /> &#160;&#160; &#160;&#160; &#160;&#160;
                  </span>
                  <span style="display:normal">
                    Tại: <xsl:value-of select="DLHDon/NDHDon/NBan/TNHang" />
                  </span>
                      </td>
							</tr>
              <tr style="display:none">
                <td>
                   <span style="display:none">
                  Email: support@cavn.vn 
                </span>

                <span style="display:none">
                  Website:<xsl:value-of select="DLHDon/NDHDon/NBan/Website" />
                </span>
                </td>
                  
              </tr>    

						</table>

						<hr style="background-color:black;width:100%;height:1px;margin-bottom:5px" />
						<table style="width:100%;" class="textfont">
							<tr>
								<td style="width:30%">

								</td>
								<td style="padding-top:5px;text-align:center;width:40% ">
									<span style="font-weight:bold; font-size:18pt; color:#FF0000">HÓA ĐƠN THƯƠNG MẠI</span>
									<br/>
									<span style="font-weight:bold; font-size:18pt;color:#FF0000">(COMMERCIAL INVOICE)</span>
									<br/>
									 <span style="font-weight:normal;font-size:10.5pt;display:param1_1">param1</span>
								</td>
								<td style="width:30%;text-align:left;padding-left:30px">
									<div style="padding-bottom:10px">
										Mẫu số <i>(Form No)</i>:<b>
											<xsl:value-of select="DLHDon/TTChung/KHMSHDon" />
										</b>
									</div>
									<div style="padding-bottom:5px">
										Ký hiệu <i>(Serial No)</i>:<b>
											<xsl:value-of select="DLHDon/TTChung/KHHDon" />
										</b>
									</div>
									<div>
										Số <i>(No)</i>:<span style="color: red;font-size:16pt">
											<xsl:value-of select="DLHDon/TTChung/SHDon" />
										</span>
									</div>


								</td>
							</tr>
							<tr>
								<td style="width:30%">

								</td>
								<td style="text-align:center;width:40%">
									<span style="font-size:10pt">
										Ngày
										<xsl:variable name="string">
											<xsl:value-of select="substring(DLHDon/TTChung/NLap,9,2)"/>
										</xsl:variable>
										<xsl:value-of select="$string" />

										tháng
										<xsl:variable name="string1">
											<xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)"/>
										</xsl:variable>

										<xsl:value-of select="$string1" />
										năm
										<xsl:variable name="string2">
											<xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)"/>
										</xsl:variable>
										<xsl:value-of select="$string2" />
									</span>
								</td>
								<td style="text-align:center;width:30%">
									<div style="color: red; font-size: 10pt;padding-top:5px;  display: normal;text-align:center">
									 <div style="paramChuyendoi">
											HOÁ ĐƠN CHUYỂN ĐỔI  <br />
											TỪ HOÁ ĐƠN ĐIỆN TỬ
										</div>
									</div>
								</td>
							</tr>
						</table>
						<table style="width: 100%;" class="textfont">
							<tr style="ho_ten_nguoi_mua_css_display;height:20px" >
								<td style="width:34%;padding-left:10px;padding-top:12px">
									Họ tên người mua hàng <i>(Customer Name)</i>:
								</td>

								<td style="border-bottom: 1px dotted black; width:66%">
									<xsl:value-of select="DLHDon/NDHDon/NMua/Ten" />
								</td>
							</tr>
							<tr style="height:20px">
								<td style="width:25%;padding-left:10px;padding-top:12px">
									Tên đơn vị <i>(Company's)</i>:
								</td>
								<td style="border-bottom: 1px dotted black; width:75%">
									<xsl:value-of select="DLHDon/NDHDon/NMua/Ten" />
								</td>
							</tr>
							<tr style="height:20px">
								<td style="width:25%;padding-left:10px;padding-top:12px">
									Mã số thuế <i>(Tax code)</i>:
								</td>
								<td style="border-bottom: 1px dotted black; width:75%">
									<xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
								</td>
							</tr>
							<tr style="height:30px">
								<td style="width:25%;padding-left:10px;padding-top:12px">
									Địa chỉ <i>(Address)</i>:
								</td>
								<td style="border-bottom: 1px dotted black; width:75%">
									<xsl:value-of select="DLHDon/NDHDon/NMua/DChi" />
								</td>
							</tr>

						</table>
						<table style="width:100%" class="textfont">
							<tr>
								<td  style="width:32%;padding-left:10px;padding-top:15px">
									<span style="top:5px;position:relative">
										Hình thức thanh toán <i>(Payment Method)</i>:
									</span>
								</td>
								<td  style="border-bottom: 1px dotted black; width:10%">
									<xsl:value-of select="DLHDon/TTChung/HTTToan" />
								</td>
								<td  style="width:22%;padding-left:10px;padding-top:15px">
									<span style="top:5px;position:relative">
										Số tài khoản <i>(Account No)</i>:
									</span>
								</td>
								<td  style="border-bottom: 1px dotted black;width:30%;text-align:left">
									<xsl:value-of select="DLHDon/NDHDon/NMua/STKNHang" />
								</td>
							</tr>
						</table>

						<div style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt;display:param2_2">param2</div>
                       <div style="text-align:right">  Loại tiền <i>(Currency Code)</i> : <xsl:value-of select="DLHDon/TTChung/DVTTe" /> </div>
                      
						<table style="width:100%;text-align:center; font-size:11pt;border-top: 1px solid black;border-bottom:1px solid black;border-left:none;border-right:none;margin-top:15px" class="textfont">
							<tr style="height:25px;">
								<td width="5%" style="padding-top:10px;padding-bottom:10px;border-left: 1px solid black">
									<span >
										<b>STT</b>
									</span>

								</td>
								<td style="border: 1px solid black">
									<span>
										<b>Tên hàng hóa, dịch vụ </b>
									</span>
									<br/>
									<i> (Description of goods, service) </i>
								</td>
								<td width="10%" style="border: 1px solid black">
									<span >
										<b>ĐVT </b>
									</span>
									<br/>
									<i> (Unit) </i>
								</td>
								<td width="10%" style="border: 1px solid black">
									<span >
										<b>Số lượng</b>
									</span>
									<br/>
									<i> (Quantity) </i>

								</td>
								<td width="15%" style="border: 1px solid black">
									<span >
										<b>Đơn giá </b>
									</span>
									<br/>
									<i> (Unit price) </i>
								</td>
								<td style="width:15%;border:1px solid black">
									<span>
										<b>Thành tiền</b>
									</span>
									<br/>
									<i> (Unit price) </i>
								</td>
							</tr>
							<tr style="height:80%; font-weight:bold" >
								<td style="border: 1px solid black" width="5%">1</td>
								<td style="border: 1px solid black">2</td>
								<td style="border: 1px solid black" width="10%">3</td>
								<td style="border: 1px solid black" width="10%">4</td>
								<td style="border: 1px solid black" width="15%">5</td>
								<td style="border:1px solid black" width="15%">6=4x5</td>
							</tr>
						</table>
					
						  <table style="width:100%;text-align:center; font-size:11pt;border: 1px solid black;paramTableBG; color:black;" >
                            <xsl:variable name="lien" select="$paramlien" />
                            <xsl:choose>
                                <xsl:when test="$lien='0'">
                                    <xsl:choose>
                                        <xsl:when test="count(DLHDon/NDHDon/DSHHDVu/HHDVu) &lt; 11" >
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='1'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose>
                                                                    </xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='1'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='1'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='1'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='1'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###,##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='1'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='2'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='2'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='2'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='2'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='2'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='2'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='3'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='3'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='3'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='3'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='3'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='3'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='4'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='4'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='4'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='4'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='4'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='4'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='5'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='5'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='5'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='5'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='5'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='5'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='6'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='6'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='6'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='6'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='6'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='6'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='7'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='7'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='7'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='7'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='7'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='7'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='8'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='8'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='8'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='8'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='8'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='8'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='9'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='9'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='9'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='9'">
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
                                                                            <xsl:when test="DVTinh !=0">
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
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='9'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='9'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='10'">
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
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                   <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='10'">
                                                                <xsl:value-of select="THHDVu" />
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='10'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:value-of select="substring-before(DVTinh,'*')" />
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
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="10%" style="text-align:center;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='10'">
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
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:value-of select="format-number(SLuong,'#.###.###.###0,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="text-align:right;border-right:1px solid black">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='10'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="DVTinh !=0">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise></xsl:otherwise>
                                                                        </xsl:choose>
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                                <td width="15%" style="border-right:none!important;text-align:right">
                                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                        <xsl:choose>
                                                            <xsl:when test="STT ='10'">
                                                                <xsl:choose>
                                                                    <xsl:when test="$xetphi &gt; 0">
                                                                        <xsl:choose>
                                                                            <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                                <xsl:choose>
                                                                                    <xsl:when test="$currency='VND'">
                                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                                    </xsl:when>
                                                                                    <xsl:otherwise>
                                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                                    </xsl:otherwise>
                                                                                </xsl:choose >
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="''" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise></xsl:otherwise>
                                                        </xsl:choose >
                                                    </xsl:for-each>
                                                </td>
                                            </tr>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                                <tr style="height:25px;border-top: none!important;border-bottom:1px dotted black;">
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
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                           <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </td>
                                                    <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                        <xsl:value-of select="THHDVu" />
                                                    </td>
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
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:choose>
                                                                    <xsl:when test="DVTinh !='0'">
                                                                         <xsl:choose>
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                    </xsl:when>
                                                                    <xsl:otherwise></xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose >
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
                                                    </td>
                                                    <td width="15%" style="text-align:right;border-right:1px solid black">
                                                        <xsl:choose>
                                                            <xsl:when test="$xetphi &gt; 0">
                                                                <xsl:choose>
                                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
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
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise></xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </td>
                                                    <td width="15%" style="border-right:1px solid black!important;text-align:right">
                                                        <xsl:choose>
                                                            <xsl:when test="$xetphi &gt; 0">
                                                                <xsl:choose>
                                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:value-of select="''" />
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:choose>
                                                                    <xsl:when test="$currency='VND'">
                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </td>
                                                </tr>
                                            </xsl:for-each>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
                                        <xsl:variable name="line" select="STT" />
                                        <xsl:choose>
                                            <xsl:when test="floor(($line - 1) div 10) = ($lien - 1)">
                                                <tr style="height:25px;border-top: none!important;border-bottom:1px dotted black;">
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
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                           <xsl:choose>
                                                                       <xsl:when test="contains(.,'|')">  <xsl:value-of select=" substring-before(DVTinh,'|')" /></xsl:when>
                                                                        <xsl:otherwise>  	<xsl:choose>
																		<xsl:when test="DVTinh !=0">	<xsl:value-of select="STT " /></xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose></xsl:otherwise>
                                                                    </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </td>
                                                    <td style="text-align:left;border-right:1px solid black;padding-left:3px">
                                                        <xsl:value-of select="THHDVu" />
                                                    </td>
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
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:choose>
                                                                    <xsl:when test="DVTinh !='0'">
                                                                         <xsl:choose>
                                                                              <xsl:when test="contains(.,'|')"><xsl:value-of select="substring-after(DVTinh,'|')" /></xsl:when>
                                                                                <xsl:otherwise> <xsl:value-of select="DVTinh" /></xsl:otherwise>
                                                                            </xsl:choose>
                                                                    </xsl:when>
                                                                    <xsl:otherwise></xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose >
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
                                                    </td>
                                                    <td width="15%" style="text-align:right;border-right:1px solid black">
                                                        <xsl:choose>
                                                            <xsl:when test="$xetphi &gt; 0">
                                                                <xsl:choose>
                                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
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
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(DGia, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise></xsl:otherwise>
                                                                </xsl:choose>
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </td>
                                                    <td width="15%" style="border-right:1px solid black!important;text-align:right">
                                                        <xsl:choose>
                                                            <xsl:when test="$xetphi &gt; 0">
                                                                <xsl:choose>
                                                                    <xsl:when test="substring-before(DVTinh,'*') !='0'">
                                                                        <xsl:choose>
                                                                            <xsl:when test="$currency='VND'">
                                                                                <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                            </xsl:when>
                                                                            <xsl:otherwise>
                                                                                <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                            </xsl:otherwise>
                                                                        </xsl:choose >
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:value-of select="''" />
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                                <xsl:choose>
                                                                    <xsl:when test="$currency='VND'">
                                                                        <xsl:value-of select="format-number(ThTien, '#.###,##','vnd')" />
                                                                    </xsl:when>
                                                                    <xsl:otherwise>
                                                                        <xsl:value-of select="format-number(ThTien, '#,###.##','usd')" />
                                                                    </xsl:otherwise>
                                                                </xsl:choose >
                                                            </xsl:otherwise>
                                                        </xsl:choose >
                                                    </td>
                                                </tr>
                                            </xsl:when>
                                        </xsl:choose>
                                    </xsl:for-each>
                                </xsl:otherwise>
                            </xsl:choose>
                        </table>
                        <div style="width:100%;display:paramfooter">
                            <div style="idparamTongtien">
                <!-- <table width="100%">
               <tr style="height:30px">
                 <td width="85%" style="text-align:left;border-left:1px solid black;border-right:1px solid black;border-bottom:1px solid black;padding-left:10px;font-size:12pt;  
        font-family:Times New Roman;">Lệ phí: </td>
                <td style="text-align:center;border-left:1px solid black;border-bottom:1px solid black;border-right:1px solid black"></td>
               </tr>

                </table> -->
                <table style="width:100%;text-align:center; font-size:11pt;border-bottom: 1px solid black;paramTableBG; color:black;">
                   <xsl:for-each select="DLHDon/NDHDon/TToan/DSLPhi/LPhi">
                       <tr style="height:30px;border-top: none!important;border-bottom:1px dotted black;border-left:1px solid black;border-right:1px solid black">
                        <td>
                          Lệ phí: <xsl:value-of select="TLPhi" />
                        </td>
                        <td width="15%" style="border-left: 1px solid black;text-align:right">
                           <xsl:value-of select="format-number(TPhi, '#.###,##','vnd')" />
                        </td>
                       </tr>
                    </xsl:for-each>
                </table>
          

                <table style="width: 100%; border-bottom: 1px solid black" class="textfont">
                  <tr style="height: 30px; border-bottom: 1px solid black;border-left:1px solid black;border-right:1px solid black">
                    <td style="border-left: none!important; border-right: none;padding-left:10px;">
                      Tổng cộng tiền thanh toán <i>(Total to pay)</i>:
                    </td>
                    <td style="width:10%;border-left: none!important; border-right: none"></td>
                    <td style="width:15%;border-left: none!important; border-right: none">
                     
                    </td>
                    <td style="width:10%;border-left: none!important; border-right: none"></td>
                    <td style="width:10%;border-left: none!important; border-right: none"></td>
                    <td style="width:10%;border-right: none!important;border-left: none;text-align:right">
                     <xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###','vnd')" />
                    </td>

                  </tr>                   
                </table>

                <table style="width: 100%; text-align: left; border-bottom: 1px none black" class="textfont">
                  <tr style="height: 30px; border-bottom: 1px none black">
                    <td  style="width:100%; border-left: none!important; border-right: none!important; text-align: left;padding-left:10px;">
                      Số tiền viết bằng chữ  <i>(In words)</i>:  <xsl:variable name="AmountWord" select="DLHDon/NDHDon/TToan/TgTTTBChu"/>
                    <xsl:value-of select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))"/>
                    </td>
                  </tr>
                </table>
						</div></div>




						<table style="width: 100%;" class="textfont">
							<tr>

								<td style="border: none; padding-top: 5px; text-align: center;width:30%">
									Người mua hàng <i>(Buyer)</i>           <br />
									(Ký, ghi rõ họ tên)<br/><i>(Signature and full name)</i>

								</td>
								<td style="border: none; padding-top: 5px; text-align: center;width:40%">
									    <div style="paramNguoiCD">
										Ngày ..... tháng ..... năm.....<br />
										Người chuyển đổi<i>(Converter)</i>
										<br />
										(Ký, ghi rõ họ tên)<i>
											<br/>
											(Signature and full name)

										</i>
									</div>
								</td>
								<td style="border: none; padding-top: 5px; text-align: center;width:30%">
									Người bán hàng <i>(Seller)</i> <br />
									(Ký, ghi rõ họ tên)<br/>
									<i>(Signature and full name)</i>

								</td>

							</tr>
							<tr>
								<td style="padding-top:3px; width:30%;text-align:center;">

								</td>
								<td style="width: 30%">
								</td>
								<td style="padding-top:3px; width:40%;padding-right:5px;text-align:center;">

								  <div style="paramSign;background-image:url(data:image/jpeg;base64,iVBORw0KGgoAAAANSUhEUgAAAgAAAAB9CAYAAADUW9vMAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAfOSURBVHja7N15yGdVHcfx9zhWLpiFCS1kJBVutEKRhEW0l+2aYaUtkpW272V7tlnRnlimFRmalGUhZrZRLlkUhBUKlRUtZpRONZrN9Me5gZk2zzYzz51eLxh45pl7zu+Z72XmfO655567ZuPGjQEA/1+2UwIAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAABWge2VALaONW9cowirx22rA6onV7eujq3OqNr4+o2qgwAAsC1lsGr/6sDqYdXdrvNnp1Qfqo6vLlUqBACA+du5emT1lCkA7HYDx+xYvbT6Y/UOJUMAAJiv21dPmAb+u1drb+S4ddW51fur85UNAQBgnu5RPak6uLrj/zju6urM6oTq7MrNfwQAgBm6b3VE9dDGIr//5azqg9U5UxAAAQBgZv+n3ad6dvXYapdNHH9eY6r/C9V65UMAAJiXtdX9q2dVj6l22sTxFzdW+J9UXal8CAAA83P/6sjGyv5NXfFfXn18Gvx/oXQIAADzc8/qBdXjFjDw/6M6vXpr9WOlAwEAmJ99G/f4D6tuvoDjL2o8y396VvaDAADMzu2rZ1RPr+6wgON/W32k8Vjf75QPBABgXnZqbN7zkuouC2xzZvXa6kfKBwIAMC9rqgdVr6weuMA2v2y8yOek6holBAEAmJd9qhdXhzT27t+UDY0X+Ly1+onygQAAzMvO1XOqo6s9FtjmF9Ux1amu+kEAAObnoY3p/gcs8PiN1WnVG1z1gwAAzM+e1QsbK/x3XmCb3zTu9X/MVT8IAMC8rGms7n91tdci2n29ekX1PSUEAQCYl70bj+kdUm23wDbrq/dVb6v+ooQgAADzsUN1aPWa6o6LaHdJ9arGbn6AAADMyH7V66qDFtnuzOrlWegHAgAwKzdtLPA7prrtItpdUx1Xvbkx/Q8IAMBM7N14TO/gRbb7dfWy6rNKCAIAMB9rq6c1Fvrtuci232285vciZQQBAJiPPRrT9odOQWAxPtW43+/tfSAAADPyqMZjevstst36xsY+x1b/VEYQAIB5uMV05f78Fr6b379d3rjff7IyggAAzMddGxv0PGAJbS9tvPznHGWELWs7JQCW4aDqjCUO/udVjzf4gxkAYD52aUz5v6jFT/lXfbE6qvqVUoIAAMzDHo0p/8cusf2Jjbf/XaWUsPW4BQDzsXYV/Ju9X2PKfymD/8bqnY17/gZ/MAMA3Ii9Ggvs7tzYQneXakN1ZWPq/GfVDxq75m0JT6veUd16CW3/Vr1lar/BqQUBAPhPOzWepT+4uldjuv3Grvqvrn5efaX6eHXxZvqZ1lavnn7tsIT2f22sF/iw0wsCAPDfA/+Bjefo79PCdtC72TRLsFf15Or91QemAXel7Fq9qzpiie2vmv5OJznFsLpYAwBb3/2qUxsvvtm/xW+fW3Wbxg58p1Z3WaGf6w7Tz7TUwf+Kqa3BH8wAANexe/Xc6nnT1yvhEdPAfVj1/WX0c/fGbYV7LrH95dPgf4bTDGYAgP90dONVubuvcL/7Vp+s9lli+wOq05Yx+F9RPdvgDwIAcMM+Ml39/3gz9L3P1P9ui2z3qOoz1Z2WMfg/vfq80wsCAHDDfttYGf/I6u3VX1a4/wMab9db6JqCQ6eZg9stY/B/ZvUlpxYEAGDTLqte1XgK4Jsr3PcR08C+Kc+rTqhuucTPuaqxnsG0PwgAwCJ9u3r0NBuwUo/yrale33hU8Ma8snpvteMSP2NddWTjCQRAAACW4MppNuDwxiY/K2HP6iX991M/21Wvrd5Y3WSJff+1emlj3QAgAADL9LnGq3YvXKH+Dq8ecr3vvaF6U3XTZfT7uup4pwsEAGDlfH8KAV9bgb62n67210xfv7M6Zvr9UmyoXlO9x2mCebIREKxulzU29flE9eBl9nXvxpv4btWYtl+Odzde7AMIAMBm8pvqKdUp1QOX0c/a6rjG/f41y+jnhGk24Z9ODcyXWwAwD39obLBz/jL72XGZwf/0afbgGqcEBABgy7hsCgEXb6XPP7fxrP+VTgUIAMCW9dPGpj2/38Kfe+EUPv7gFIAAAGwd32hMw/9jC33epY2X+1ym9CAAAFvXpxuP8m1ul0+D/w+VHAQAYHV4e3XWZux/XXVU494/IAAAq8S6xmY8v9sMfW9obElsf38QAIBV6AfV+6YBeyUdV31IeUEAAFavj1bfWcH+TqzeXG1UWhAAgNXrz9Wx1dXL6OOq6gvVUxv3/dcpK2zbbAUM24azq89Xhyyy3SXVmY3X+V6kjCAAAPOyofpg9fBq1wUcf0H12Wnwv1T5QAAA5uuC6kuNFwfdkGur71bHNx4f/JOSgQAAzN+11cnVE6sdrvP99dVXq481bhWsVypAAIBty/nVt6qHVH+frvRPrr48BQQAAQC2Qeuq06bB//jGLn5XKwtwff8CAAD//wMAapgjSwqpKyoAAAAASUVORK5CYII=); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid red; text-align:left;padding-top:5px; ">
               	<span style="color:red;">
											<b> Signature valid</b><br/>
											Được ký bởi:
											<xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />

											<br/>
											Ngày ký:  <xsl:value-of select="substring(DLHDon/TTChung/NLap,9,2)"/>-
											<xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)"/>-
											<xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)"/>
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
					<div style="width:100%;padding-top:3px;text-align:center;padding-bottom:3px;">
						<span>
							Chuỗi xác thực: <b >
								
							</b>
						</span>
					</div>
					<div  style="word-spacing:6px">
						<i>
							Giải pháp hóa đơn điện tử được cung cấp bởi: <b> Công ty cổ phần công nghệ thẻ Nacencomm</b>. Mã số thuế: <b>0103930279</b>.
						</i>
					</div>
					<div style="width:100%;padding-top:3px;text-align:center;padding-bottom:0px;">
						<span>
								<i>Tra cứu hóa đơn tại địa chỉ trang web: https://ca2einvoice.nacencomm.vn </i>
								<xsl:if test="DLHDon/TTChung/TTKhac/TTin/TTruong='MTCuu'">
									&#160;&#160;&#160;&#160;Mã tra cứu:
									<b>
										<xsl:value-of select="DLHDon/TTChung/TTKhac/TTin/DLieu" />
									</b>
								</xsl:if>
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
                      <!--   <div style="text-align:center;padding-top:3px"> param3 </div> -->
                    </xsl:otherwise>
                </xsl:choose>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
