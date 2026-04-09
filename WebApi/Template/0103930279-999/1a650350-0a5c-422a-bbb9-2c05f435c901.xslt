<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:ex="http://exslt.org/dates-and-times"
                xmlns:fn="http://www.w3.org/2005/02/xpath-functions"
                xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
	<xsl:output method="html" />
	<xsl:param name="imgLogo" />
	<xsl:param name="percent" select="''" />
	<xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
	<xsl:decimal-format name="usd" decimal-separator="." grouping-separator="," />
	<!-- <xsl:variable name="buyerSign" select="//*[local-name() = 'X509SubjectName']" />
  <xsl:variable name="sub1" select=" substring-after($buyerSign,'CN=')" />
  <xsl:variable name="sub2" select=" substring-before($sub1,',')" />
  <xsl:variable name="Buyer" select="//*[local-name()='Signature']/@Id"></xsl:variable> -->
	<xsl:template match="HDon">
		<xsl:variable name="xetphi" select="count(DLHDon/NDHDon/DSHHDVu/HHDVu[DVTinh='0*1'])" />
		<xsl:variable name="currency" select="DLHDon/TTChung/DVTTe" />
		<!-- <xsl:variable name="digest1" select="(//*[local-name() = 'DigestValue'])[1]" />
      <xsl:variable name="digest2" select="(//*[local-name() = 'DigestValue'])[2]" /> -->
		<xsl:variable name="tax" select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat" />
		<xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
		<xsl:variable name="somucthue" select="count(DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat)" />
		<xsl:variable name="soHHdu" select="10-(count(DLHDon/NDHDon/DSHHDVu/HHDVu/STT))" />
		<xsl:variable name="HVTNMHang" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='HVTNMHang']/DLieu" />
		<xsl:variable name="DChiNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='DChi']/DLieu" />
		<xsl:variable name="DCTDTuNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='DCTDTu']/DLieu" />
		<xsl:variable name="STKNHangNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='STKNHang']/DLieu" />
		<xsl:variable name="TNHangNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='TNHang']/DLieu" />
		<xsl:variable name="TenNMHCNhan" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='TenNMHCNhan']/DLieu" />
		<xsl:variable name="Ten" select=" DLHDon/NDHDon/NMua/Ten" />
		<xsl:variable name="HVTNMHang1" select=" DLHDon/NDHDon/NMua/HVTNMHang" />
		<xsl:variable name="MST" select=" DLHDon/NDHDon/NMua/MST" />
		<xsl:variable name="countTen" select="count(DLHDon/NDHDon/NMua/Ten)" />
		<xsl:variable name="STKNHangNBan" select="DLHDon/NDHDon/NBan/TTKhac/TTin[TTruong='STKNHang']/DLieu" />
		<xsl:variable name="TNHangNBan" select="DLHDon/NDHDon/NBan/TTKhac/TTin[TTruong='TNHang']/DLieu" />
		<xsl:variable name="GhiChu" select="DLHDon/TTChung/TTKhac/TTin[TTruong='GhiChu']/DLieu" />
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
        font-size:11pt;  
        font-family:"Times New Roman";
        color:Black;
        }
				</style>
			</head>
			<body style="font-family:Times New Roman" class="textfont">
				<div style="viewstyle;border:none ">
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
                                    border: 10px solid transparent;
                                    border-width:20px;z-index:1;border-color:white">
						<div id="header" style="display:flex;flex-direction:paramOpacityHeaderFlexDirection;padding-right:10px;padding-top:10px;">
							<div style="width:185px;display:flex;justify-content:center" rowspan="5">
								<img style="height:100px;align-content:center;position:static;left:0;top:0;object-fit: scale-down;padding-top:5px"
                                         id="imgSample" src="paramLogo" />
							</div>
							<div style="flex:1;text-align:left;">
								<tr style="ten_cong_ty_css_display;">
									<td style="padding-left:10px;width:100%;ten_cong_ty_css" >
										<span style="color:red;text-transform: uppercase;font-size:13pt;">
											<b>
												<xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
											</b>
										</span>
									</td>
								</tr>
								<br/>
								<tr style="mst_css_display;">
									<td style="padding-left:10px;width:150px;font-size:10pt;mst_css;">                    
                                            Mã số thuế
										<i>(Tax code)</i>:
										<span style="font-weight:bold; letter-spacing:5px">
											<xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
										</span>
									</td>
								</tr>
								<br/>
								<tr style="dia_chi_css_display;">
									<td style="padding-left:10px;width:150px;font-size:10pt;dia_chi_css">
                                            Địa chỉ
										<i>(Address)</i>:
										<xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
									</td>
								</tr>
								<br/>
								<tr style="so_tai_khoan_css_display;">
									<td style="padding-left:10px;font-size:10pt;so_tai_khoan_css;">
                                            Số tài khoản
										<i>(Account No)</i>:
										<xsl:value-of select="DLHDon/NDHDon/NBan/STKNHang" /> 
                                            Tại:
										<xsl:value-of select="DLHDon/NDHDon/NBan/TNHang" />
									</td>
								</tr>
								<br/>
								<tr style="dien_thoai_css_display;">
									<td style="padding-left:10px;font-size:10pt;dien_thoai_css;">
                                            Điện thoại
										<i>(Tel)</i>:
										<xsl:value-of select="DLHDon/NDHDon/NBan/SDThoai" />
                                            &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;
										<xsl:choose>
											<xsl:when test="DLHDon/NDHDon/NBan/DCTDTu!=''"> Email:
												<xsl:value-of select="DLHDon/NDHDon/NBan/DCTDTu" />
											</xsl:when>
											<xsl:otherwise></xsl:otherwise>
										</xsl:choose>
										<xsl:choose>
											<xsl:when test="DLHDon/NDHDon/NBan/Website!=''">
                                                    &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;  &#160;&#160;&#160;
                                                    Website:
												<xsl:value-of select="DLHDon/NDHDon/NBan/Website" />
											</xsl:when>
											<xsl:otherwise></xsl:otherwise>
										</xsl:choose>
									</td>
								</tr>
							</div>
						</div>
						<hr style="background-color:black;width:100%;height:1px;margin-bottom:1px" />
						<table style="width:100%;line-height:25px;font-size:11pt;color:black">
							<tr>
								<td style="width:185px">

                                    </td>
								<td style="width:50%;text-align:center;">
									<span style="font-weight:bold; font-size:15pt;text-transform: uppercase;color:red">
										<xsl:value-of select="DLHDon/TTChung/THDon" />
									</span>
									<br/>
									<span style="font-weight:bold; font-size:15pt;font-style:italic;color:red">(SALES INVOICE)</span>
									<br/>
									<span style="font-size:10pt; text-align:center">
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
								<td style="width:25%; padding-top:5px;font-size:9.5pt;">
                                        Mẫu số
									<i>(Form)</i>:
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
									<i>(No)</i>:
									<span style="color: red;font-size:16pt">
										<xsl:value-of select="substring(
                                                    concat('00000000', DLHDon/TTChung/SHDon), 
                                                    string-length(DLHDon/TTChung/SHDon) + 1, 
                                                    8
                                                )"/>
									</span>
									<br />
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
						<table style="width:100%;color:black">
							<tr>
								<td style="text-align:center" >
									<b>
                                            MÃ CQT CẤP:
										<xsl:if test="MCCQT !=''">
											<xsl:value-of select="MCCQT"/>
										</xsl:if>
									</b>
								</td>
							</tr>
						</table>
						<table style="width:100%;line-height:25px;font-size:10pt;color:black">
							<tr style="ho_ten_nguoi_mua_css_display;">
								<td style="padding-left:10px;;width:38%">
                                        Họ tên người mua hàng
									<i>(Customer Name)</i>:
								</td>
								<td style="width:62%;ho_ten_nguoi_mua_css;">
									<span style="color:black;font-size:10pt;">
										<xsl:value-of select="DLHDon/NDHDon/NMua/HVTNMHang" />
									</span>
			          &#160;&#160;&#160; &#160;&#160;&#160; &#160;&#160;&#160; &#160;
									<xsl:if test="DLHDon/NDHDon/NMua/CCCDan!=''">CCCD
										<i>(Citizen ID No.)</i>:
										<span style="color:black;font-size:10pt;">
											<xsl:value-of select="DLHDon/NDHDon/NMua/CCCDan" />
										</span>
									</xsl:if>
								</td>
							</tr>
							<tr style="don_vi_mua_hang_css_display;">
								<td style="padding-left:10px">
                                        Tên đơn vị
									<i>(Company's)</i>:
								</td>
								<td style="don_vi_mua_hang_css;">
									<span style="color:black;font-size:10pt;">
										<xsl:value-of select="DLHDon/NDHDon/NMua/Ten" />
									</span>
								</td>
							</tr>
							<tr style="mst_nguoi_mua_css_display;">
								<td style="padding-left:10px">
                                        Mã số thuế
									<i>(Tax code)</i>:
								</td>
								<td style="mst_nguoi_mua_css;">
									<span style="color:black;font-size:10pt;">
										<xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
									</span>
								</td>
							</tr>
							<tr style="dia_chi_nguoi_mua_css_display;">
								<td style="padding-left:10px">
                                        Địa chỉ
									<i>(Address)</i>:
								</td>
								<td style="dia_chi_nguoi_mua_css;">
									<span style="color:black;font-size:10pt;">
										<xsl:value-of select="DLHDon/NDHDon/NMua/DChi" />
									</span>
								</td>
							</tr>
							<xsl:if test="DLHDon/NDHDon/NMua/MDVQHNSach!=''">
								<tr>
									<td style="padding-left:10px;">
										Mã số ĐVQHNS
										<i>
											(Budget Code)</i>:
									</td>
									<td>
										<span style="color:black;font-size:10pt;">
											<xsl:value-of select="DLHDon/NDHDon/NMua/MDVQHNSach" />
										</span>
									</td>
								</tr>
							</xsl:if>
							<tr style="so_tai_khoan_nguoi_mua_css_display;">
								<td style="padding-left:10px">
                                        Số tài khoản
									<i>(Account No)</i>:
								</td>
								<td style="so_tai_khoan_nguoi_mua_css;">
									<span style="color:black;font-size:10pt;">
										<xsl:value-of select="DLHDon/NDHDon/NMua/STKNHang" /> 
                                        Tại:
										<xsl:value-of select="DLHDon/NDHDon/NMua/TNHang" />
									</span>
								</td>
							</tr>
							<tr>
								<td style="padding-left:10px">
                                        Hình thức thanh toán
									<i>(Payment Method)</i>:
								</td>
								<td>
									<span style="color:black;font-size:10pt;">
										<xsl:value-of select="DLHDon/TTChung/HTTToan" />
									</span>
                                        &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;
                                        &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;
								</td>
							</tr>
							<tr>
								<td style="padding-left:10px">Đồng tiền thanh toán
									<i>(Payment currency)</i>:
								</td>
								<td>
									<span style="color:black;font-size:10pt;">
										<xsl:value-of select="DLHDon/TTChung/DVTTe" />
									</span>&#160;&#160;&#160;&#160;
									<xsl:if test="DLHDon/TTChung/TGia !='0'">Tỷ giá:
										<span style="color:black;font-size:10pt;">
											<xsl:value-of select="format-number(DLHDon/TTChung/TGia, '#.###','vnd')" />
										</span>
									</xsl:if>
								</td>
							</tr>
						</table>
						<xsl:choose>
							<xsl:when test="DLHDon/TTChung/TTHDLQuan!=''">
								<div style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt">Hóa đơn
									<xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=1">thay thế</xsl:if>
									<xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=2">điều chỉnh</xsl:if> cho hóa đơn số
									<xsl:value-of select="substring(
  concat('00000000', TTChung/TTHDLQuan/SHDCLQuan), 
  string-length(DLHDon/TTChung/TTHDLQuan/SHDCLQuan) + 1, 
  7
)"/>, mẫu số
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
								<td style="border: 1px solid black" >2</td>
								<td style="border: 1px solid black" width="10%">3</td>
								<td style="border: 1px solid black" width="10%">4</td>
								<td style="border: 1px solid black" width="15%">5</td>
								<td style="border:1px solid black" width="15%">6=4x5</td>
							</tr>
						</table>
						<table style="width:100%;text-align:center; font-size:10pt;border: 1px solid black;paramTableBG; color:black;" >
							<xsl:variable name="lien" select="paramlien" />
							<xsl:choose>
								<xsl:when test="$lien='0'">
									<xsl:choose>
										<xsl:when test="count(DLHDon/NDHDon/DSHHDVu/HHDVu) &lt; 11" >
											<xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
												<tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
													<td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
														<xsl:choose>
															<xsl:when test="TChat!=4">
																<xsl:value-of select="STT" />
															</xsl:when>
															<xsl:otherwise></xsl:otherwise>
														</xsl:choose>
													</td>
													<td style="text-align:left;border-right:1px solid black;padding-left:3px">
														<xsl:call-template name="split">
															<xsl:with-param name="text" select="THHDVu"/>
														</xsl:call-template>
													</td>
													<td width="10%" style="text-align:center;border-right:1px solid black">
														<xsl:choose>
															<xsl:when test="DVTinh !='0'">
																<xsl:value-of select="DVTinh" />
															</xsl:when>
														</xsl:choose >
													</td>
													<td width="10%" style="text-align:center;border-right:1px solid black">
														<xsl:choose>
															<xsl:when test="DVTinh!='0'">
																<xsl:choose>
																	<xsl:when test="SLuong &gt; 1">
																		<xsl:value-of select="format-number(SLuong,'#.###.###,##','vnd')" />
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="format-number(SLuong,'#.###.###.##0,##','vnd')" />
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:when>
														</xsl:choose >
													</td>
													<td width="15%" style="text-align:right;border-right:1px solid black">
														<xsl:choose>
															<xsl:when test="DVTinh!='0'">
																<xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
															</xsl:when>
														</xsl:choose >
													</td>
													<td width="15%" style="border-right:1px solid black!important;text-align:right">
														<xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
													</td>
												</tr>
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
												<tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
													<td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
														<xsl:choose>
															<xsl:when test="TChat!=4">
																<xsl:value-of select="STT" />
															</xsl:when>
															<xsl:otherwise></xsl:otherwise>
														</xsl:choose>
													</td>
													<td style="text-align:left;border-right:1px solid black;padding-left:3px">
														<xsl:call-template name="split">
															<xsl:with-param name="text" select="THHDVu"/>
														</xsl:call-template>
													</td>
													<td width="10%" style="text-align:center;border-right:1px solid black">
														<xsl:choose>
															<xsl:when test="DVTinh !='0'">
																<xsl:value-of select="DVTinh" />
															</xsl:when>
														</xsl:choose >
													</td>
													<td width="10%" style="text-align:center;border-right:1px solid black">
														<xsl:choose>
															<xsl:when test="DVTinh!='0'">
																<xsl:choose>
																	<xsl:when test="SLuong &gt; 1">
																		<xsl:value-of select="format-number(SLuong,'#.###.###,##','vnd')" />
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="format-number(SLuong,'#.###.###.##0,##','vnd')" />
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:when>
														</xsl:choose >
													</td>
													<td width="15%" style="text-align:right;border-right:1px solid black">
														<xsl:choose>
															<xsl:when test="DVTinh!='0'">
																<xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
															</xsl:when>
														</xsl:choose >
													</td>
													<td width="15%" style="border-right:1px solid black!important;text-align:right">
														<xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
													</td>
												</tr>
											</xsl:for-each>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
										<xsl:variable name="line" select="position()" />
										<tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
											<td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
												<xsl:choose>
													<xsl:when test="TChat!=4">
														<xsl:value-of select="STT" />
													</xsl:when>
													<xsl:otherwise></xsl:otherwise>
												</xsl:choose>
											</td>
											<td style="text-align:left;border-right:1px solid black;padding-left:3px">
												<xsl:call-template name="split">
													<xsl:with-param name="text" select="THHDVu"/>
												</xsl:call-template>
											</td>
											<td width="10%" style="text-align:center;border-right:1px solid black">
												<xsl:choose>
													<xsl:when test="DVTinh !='0'">
														<xsl:value-of select="DVTinh" />
													</xsl:when>
												</xsl:choose >
											</td>
											<td width="10%" style="text-align:center;border-right:1px solid black">
												<xsl:choose>
													<xsl:when test="DVTinh!='0'">
														<xsl:choose>
															<xsl:when test="SLuong &gt; 1">
																<xsl:value-of select="format-number(SLuong,'#.###.###,##','vnd')" />
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="format-number(SLuong,'#.###.###.##0,##','vnd')" />
															</xsl:otherwise>
														</xsl:choose>
													</xsl:when>
												</xsl:choose >
											</td>
											<td width="15%" style="text-align:right;border-right:1px solid black">
												<xsl:choose>
													<xsl:when test="DVTinh!='0'">
														<xsl:value-of select="format-number(DGia, '#.###,##','vnd')" />
													</xsl:when>
												</xsl:choose >
											</td>
											<td width="15%" style="border-right:1px solid black!important;text-align:right">
												<xsl:value-of select="format-number(ThTien, '#.###','vnd')" />
											</td>
										</tr>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:choose>
								<xsl:when test="$soHHdu &gt; 0">
									<xsl:for-each select="(//node())[$soHHdu >= position()]">
										<tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
											<td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;"></td>
											<td style="text-align:left;border-right:1px solid black;padding-left:3px"></td>
											<td width="10%" style="text-align:center;border-right:1px solid black"></td>
											<td width="10%" style="text-align:center;border-right:1px solid black"></td>
											<td width="15%" style="text-align:center;border-right:1px solid black"></td>
											<td width="15%" style="text-align:center;border-right:1px solid black"></td>
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
								<table style="width: 100%; border-bottom: 1px solid black">
									<tr style="height: 30px; border-bottom: 1px solid black;border-left:1px solid black;border-right:1px solid black;font-size:10pt">
										<td style="border-left: none!important; border-right: none;padding-left:10px;">
										Cộng tiền bán hàng hóa, dịch vụ
											<xsl:if test="$GhiChu!=''">(
												<xsl:value-of select="$GhiChu" />)
											</xsl:if>:
										</td>
										<!-- <td style="width:10%;border-left: none!important; border-right: none"></td>
                                        <td style="width:15%;border-left: none!important; border-right: none">
                     
                    </td>
                                        <td style="width:10%;border-left: none!important; border-right: none"></td>
                                        <td style="width:10%;border-left: none!important; border-right: none"></td>-->
										<td style="width:10%;border-right: none!important;border-left: none;text-align:right">
											<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###','vnd')" />
										</td>
									</tr>
								</table>
								<table style="width: 100%; text-align: left; border-bottom: 1px none black; border-bottom: 1px solid black;">
									<tr style="height: 30px; border-bottom: 1px none black;font-size:10pt">
										<td  style="width:100%; border-left: 1px solid black; border-right: 1px solid black; text-align: left;padding-left:10px;">
                      Số tiền viết bằng chữ:
											<xsl:if test="DLHDon/NDHDon/TToan/TgTTTBSo  &lt; 0">
												<i>Âm </i>
											</xsl:if>
											<xsl:variable name="AmountWord" select="DLHDon/NDHDon/TToan/TgTTTBChu"/>
											<xsl:value-of select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))"/>
										</td>
									</tr>
								</table>
								<table style="width: 100%;" class="textfont">
									<tr>
										<td style="border: none; padding-top: 5px; text-align: center;width:35%">
									Người mua hàng
											<i>(Buyer)</i>
											<br />
									(Ký, ghi rõ họ tên)
											<br/>
											<i>(Signature and full name)</i>
										</td>
										<td style="border: none; padding-top: 5px; text-align: center;width:30%">
											<div style="paramNguoiCD">
										Ngày ..... tháng ..... năm.....
												<br />
										Người chuyển đổi
												<i>(Converter)</i>
												<br />
										(Ký, ghi rõ họ tên)
												<i>
													<br/>
											(Signature and full name)
												</i>
											</div>
										</td>
										<td style="border: none; padding-top: 5px; text-align: center;width:35%">
									Người bán hàng
											<i>(Seller)</i>
											<br />
									(Ký, ghi rõ họ tên)
											<br/>
											<i>(Signature and full name)</i>
										</td>
									</tr>
									<tr>
										<td style="padding-top:3px; width:35%;text-align:center;">

								</td>
										<td style="border: none; padding-top: 1px; text-align: center;width:30%">
											<div style="width:100%;text-align:center;paramNguoiCD">
												<br />
												<br />

                                        Ngày chuyển đổi
												<i> (Conversion Date) </i>
												<br />
												<xsl:value-of select="substring( //*[local-name() = 'SigningTime'],9,2)"/>-
												<xsl:value-of select="substring( //*[local-name() = 'SigningTime'],6,2)"/>-
												<xsl:value-of select="substring( //*[local-name() = 'SigningTime'],0,5)"/>
											</div>
										</td>
										<td style="padding-top:3px; width:35%;padding-right:5px;text-align:center;">
											<div style="paramSign;background-image:url(data:image/jpeg;base64,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); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid red; text-align:left;padding-top:5px; ">
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
					</div>
					<div  style="word-spacing:3px;font-size:9.5pt;text-align:center">
						<i>
							Giải pháp hóa đơn điện tử được cung cấp bởi:
							<b> Công ty cổ phần công nghệ thẻ Nacencomm</b>. Mã số thuế:
							<b>0103930279</b>.
						</i>
					</div>
					<div style="width:100%;padding-top:0px;text-align:center;padding-bottom:1px;font-size:9.5pt">
						<span>
							<i>Tra cứu hóa đơn tại địa chỉ trang web: https://ca2einv.nacencomm.vn </i>&#160;&#160;&#160;&#160;Mã tra cứu:
							<b>
								<xsl:value-of select="DLHDon/TTChung/TTKhac/TTin/DLieu" />
							</b>
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
	<xsl:template name="split">
		<xsl:param name="text" select="."/>
		<xsl:if test="string-length($text) > 0">
			<xsl:variable name="output-text">
				<xsl:value-of select="normalize-space(substring-before(concat($text, '|'), '|'))"/>
			</xsl:variable>
			<xsl:if test="normalize-space($output-text) != ''">
				<xsl:value-of select="$output-text"/>
				<br/>
			</xsl:if>
			<xsl:call-template name="split">
				<xsl:with-param name="text" select="substring-after($text, '|')"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>