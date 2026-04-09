<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ex="http://exslt.org/dates-and-times"
    xmlns:fn="http://www.w3.org/2005/02/xpath-functions"
    xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
	<xsl:output method="html" />
	<xsl:param name="imgLogo" />
	<xsl:param name="paramlien" />
	<xsl:param name="percent" select="''" />
	<xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
	<xsl:decimal-format name="usd" decimal-separator="." grouping-separator="," />
	<xsl:decimal-format name="number" decimal-separator="," grouping-separator="." />
	<xsl:template match="HDon">
	

		<xsl:variable name="xetphi" select="count(DLHDon/NDHDon/DSHHDVu/HHDVu[DVTinh='0*1'])" />
		<xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
		<xsl:variable name="TSuat" select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat" />
		<xsl:variable name="soHHdu" select="1-(count(DLHDon/NDHDon/DSHHDVu/HHDVu/STT))" />
		<xsl:variable name="DVTTe" select="DLHDon/TTChung/DVTTe" />
		<xsl:variable name="HVTNMHang" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='HVTNMHang']/DLieu" />
		<xsl:variable name="DChiNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='DChi']/DLieu" />
		<xsl:variable name="DCTDTuNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='DCTDTu']/DLieu" />
		<xsl:variable name="DiachiCH" select="DLHDon/NDHDon/NBan/TTKhac/TTin[TTruong='DiachiCH']/DLieu" />
		<xsl:variable name="TenCH" select="DLHDon/NDHDon/NBan/TTKhac/TTin[TTruong='TenCH']/DLieu" />

		<xsl:variable name="STKNHangNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='STKNHang']/DLieu" />
		<xsl:variable name="TNHangNMua" select="DLHDon/NDHDon/NMua/TTKhac/TTin[TTruong='TNHang']/DLieu" />
		<xsl:variable name="somucthue" select="count(DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat)" />
		<html lang="en"
            xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<title>E-Invoice</title>
				<meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8' />
				<style type="text/css">
					#tblContent td, #tblContent th {
					border:1px solid black;
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
					table tr, td {
					page-break-inside: avoid; }
					div>.page-break { page-break-inside:
					avoid; padding-top: 5px; } .sign-flex { page-break-inside: avoid !important;
					page-break-after: auto; padding-top: 0 !important; }

					body { width: auto; height: auto; margin: 0 auto; }
					@page {
					size: A5;
					margin: 20px;
					}
					@media print {
					html, body {
					width: 148mm;
					height: 210mm;
					zoom: 98%;
					center;
					}
					}
					@media screen and (min-width: 0px) and (max-width: 480px)   {
					#mobile {
					<!-- display: none; -->
					zoom: 30%;

					}
					}
					@media screen and (min-width: 481px) and (max-width: 900px)   {
					#mobile {
					<!-- display: none; -->
					zoom: 60%;

					}
					}
				</style>
				<!-- <script type="text/javascript" language="javascript">
                    <![CDATA[ 
                        $(document).ready(function (){

                            var codedemo= $('#qr').html();
                         
                            var urlqrcode='https://chart.googleapis.com/chart?chs=60x60&cht=qr&chl=val&choe="UTF-8"';
                        
                         urlqrcode=urlqrcode.replaceAll('amp;','');
                          urlqrcode=urlqrcode.replaceAll('val',codedemo);
                    
                           var imageParent = document.getElementById("qrcode");
                           imageParent.src = urlqrcode;
                           
                        });
        
                    ]]>
                </script> -->
			</head>
			<page id="mobile"  size="A5">
				<body style="font-family:Times New Roman; background: rgb(240,240,240);">
					<!-- <div id='qr' style="display:none">
                        <xsl:value-of select="//*[local-name() = 'DLQRCode']"/>
                    </div> -->
					<div style="viewstyle;border:none;width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;">
						<div id="background" style="paramMau">


							MẪU
						</div>
						<div id="background" style="paramdisable">contentDisable</div>
						<div style="border:none;width:820px;height: auto; min-height: auto;">
							<table style="width:100%;">
								<tr >
									<td style="width:200px;text-align:center;font-size:14pt; padding-top:1px;padding-left:0px!important;color:black">
										<img style="height:100px;;align-content:center;position:static;left:0;top:0;object-fit: scale-down;"
															  id="imgSample" src="paramLogo">
										</img>
									</td>
									<td style="width:25%;text-transform: uppercase;">
										<b>
											<xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
										</b>
									</td>
									<td style="width:30%;padding-top:5px;text-align:center; ">
										<span style="font-weight:bold; font-size:15pt;text-transform: uppercase;color:green;font-family:arial">
											<xsl:choose>
												<xsl:when test="DLHDon/TTChung/THDon!='Hóa đơn giá trị gia tăng' and DLHDon/TTChung/THDon!='HÓA ĐƠN GIÁ TRỊ GIA TĂNG' and DLHDon/TTChung/THDon!='HOÁ ĐƠN GIÁ TRỊ GIA TĂNG'">
													<xsl:value-of select="DLHDon/TTChung/THDon" />
												</xsl:when>
												<xsl:otherwise>
													HÓA ĐƠN <br/> GIÁ TRỊ GIA TĂNG
												</xsl:otherwise>
											</xsl:choose>
										</span>
										<!-- <br/>
                                        <span style="font-weight:bold; font-size:15pt">(VAT INVOICE)</span> -->
										<br/>
										<span style="font-weight:normal;font-size:10.5pt;display:param1_1">param1</span>
									</td>
									<td style="width:25%;padding-left:0px;padding-top:20px;">
										<!-- Mẫu số
                                        <i>(Form)</i>:
                                        <b>
                                            <xsl:value-of select="DLHDon/TTChung/KHMSHDon" />
                                        </b>
                                        <br /> -->
										Ký hiệu:
										<!-- <i>(Serial No)</i>: -->
										<b>
											<xsl:value-of select="DLHDon/TTChung/KHMSHDon" />
											<xsl:value-of select="DLHDon/TTChung/KHHDon" />
										</b>
										<br />
										Số:
										<!-- <i>(No)</i>: -->
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
									<td>
										<td></td>
									</td>
									<td style="text-align:center;">
										<span style="font-size:10.5pt">
											&#160;
											<i>Ngày</i>&#160;
											<!-- <i>(Date)</i>&#160; -->
											<xsl:variable name="string">
												<xsl:value-of select="substring(DLHDon/TTChung/NLap,9,2)"/>
											</xsl:variable>
											<xsl:value-of select="$string" />

											&#160;
											<i>tháng</i>&#160;
											<!-- <i>(month)</i> &#160; -->
											<xsl:variable name="string1">
												<xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)"/>
											</xsl:variable>
											<xsl:value-of select="$string1" />
											&#160;
											<i>năm</i>&#160;
											<!-- <i>(year)</i>&#160; -->
											<xsl:variable name="string2">
												<xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)"/>
											</xsl:variable>
											<xsl:value-of select="$string2" />
										</span>
										<br/>
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
							<table style="width:100%;">
								<tr>
									<td style="text-align:center" >
										Mã cơ quan thuế cấp:
										<b>
											<xsl:if test="MCCQT !=''">
												<span style="color:red">
													<xsl:value-of select="MCCQT"/>
												</span>
											</xsl:if>
										</b>
									</td>
								</tr>
							</table>
							<!-- <hr style="background-color:black;width:100%;height:1px;margin-bottom:1px" /> -->
							<!-- Thông tin bán hàng -->
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:15%" >
										Đơn vị bán hàng:
										<!-- </td>
                                    <td style="padding-left:0px;"> -->


									</td>
									<td style="border-bottom: 2px dotted black;width:85%">
										<span style="font-weight:bold; font-size:10.5pt;text-transform: uppercase;">
											<xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
										</span>
									</td>
									<!-- <td rowspan="2">
                                        <img id="qrcode" src="paramqrcode" alt="" style="width:100px;height:100px" />
                                    </td> -->
								</tr>
							</table>
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:10%">
										Mã số thuế:
										<!-- </td>
                                    <td style="width:75%"> -->

									</td>
									<td style="border-bottom: 2px dotted black;width:90%">
										<span style="font-weight:bold; font-size:10.5pt">
											<xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
										</span>
									</td>
								</tr>
							</table>
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:20%">
										Địa chỉ đơn vị bán hàng:
										<!-- <i>(Address)</i>:
                                    </td>
                                    <td> -->

									</td>
									<td style="border-bottom: 2px dotted black;width:80%">
										<xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
									</td>
								</tr>
							</table>
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:18%">
										Địa điểm bán hàng:
										<!-- <i>(Address)</i>:
                                    </td>
                                    <td> -->

									</td>
									<td style="border-bottom: 2px dotted black;width:82%">
										<xsl:value-of select="$TenCH" />
									</td>
								</tr>
							</table>
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:7%">
										Địa chỉ:
										<!-- <i>(Address)</i>:
                                    </td>
                                    <td> -->

									</td>
									<td style="border-bottom: 2px dotted black;width:93%">
										<xsl:value-of select="$DiachiCH" />

									</td>
								</tr>
							</table>
							<!-- <xsl:choose>
                                    <xsl:when test="DLHDon/NDHDon/NBan/STKNHang!=''">
                                    <table style="width:100%;line-height:25px;font-size:10.5pt">
                                    <tr>
                                        <td style="padding-left:5px;">
                        Số tài khoản:
                                           
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/STKNHang" /> 
                                            &#160;&#160; Tại:
                                            <xsl:value-of select="DLHDon/NDHDon/NBan/TNHang" />
                                        </td>
                                    </tr>
                                </table>
                                </xsl:when>
                                    <xsl:otherwise></xsl:otherwise>
                                </xsl:choose> -->
							<!-- <tr>
                                    <td style="padding-left:5px;">
                    Điện thoại
                                        <i>(Tel)</i>:
                                    </td>
                                    <td>
                                        <xsl:value-of select="DLHDon/NDHDon/NBan/SDThoai" />
					           &#160;&#160;&#160;  &#160;&#160;&#160;
                                        <xsl:choose>
                                            <xsl:when test="DLHDon/NDHDon/NBan/Fax!=''"> Fax:
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/Fax" />
                                            </xsl:when>
                                            <xsl:otherwise></xsl:otherwise>
                                        </xsl:choose>
                                        <xsl:choose>
                                            <xsl:when test="DLHDon/NDHDon/NBan/Website!=''">
                                    &#160;&#160;&#160;  &#160;&#160;&#160;  
                                      Website:
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/Website" />
                                            </xsl:when>
                                            <xsl:otherwise></xsl:otherwise>
                                        </xsl:choose>
                                        <xsl:choose>
                                            <xsl:when test="DLHDon/NDHDon/NBan/DCTDTu!=''">
                                    &#160;&#160;&#160;   
                                      Email:
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/DCTDTu" />
                                            </xsl:when>
                                            <xsl:otherwise></xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                </tr> -->
							<!-- <hr style="background-color:black;width:100%;height:0.5px;margin-bottom:1px;margin-top:1px" /> -->
							<!-- Thông tin người mua hàng -->
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:15%;">
										Họ tên người mua:
										<!-- <i>(Customer Name)</i>:
                                    </td>
                                    <td style="width:65%"> -->

									</td>
									<td style="border-bottom: 2px dotted black;width:85%">
										<xsl:value-of select="DLHDon/NDHDon/NMua/HVTNMHang" />
										<xsl:if test="$HVTNMHang!=''">
											<xsl:value-of select="$HVTNMHang" />
										</xsl:if>
									</td>
								</tr>
							</table>
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:10%">
										Tên đơn vị:
										<!-- <i>(Buyer's name
                                            )</i>:
                                    </td>
                                    <td> -->
									</td>
									<td style="border-bottom: 2px dotted black;width:50%">
										<xsl:value-of select="DLHDon/NDHDon/NMua/Ten" />
									</td>
									<td style="width:10%;">
										Mã số thuế:

									</td>
									<td style="border-bottom: 2px dotted black;width:30%">
										<xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
									</td>
								</tr>
							</table>
							<!-- <table style="width:100%;line-height:25px;font-size:10.5pt">
                                <tr>
                                    <td style="padding-left:5px;">
                    Mã số thuế
                                        <i>(Tax code)</i>:
                                    </td>
                                    <td>
                                        <xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
                                    </td>
                                </tr>
                            </table> -->
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:7%;">
										Địa chỉ:
										<!-- <i>(Address)</i>:
                                    </td>
                                    <td> -->


									</td>
									<td style="border-bottom: 2px dotted black;width:93%;">
										<xsl:value-of select="DLHDon/NDHDon/NMua/DChi" />
										<xsl:if test="$DChiNMua!=''">
											<xsl:value-of select="$DChiNMua" />
										</xsl:if>
									</td>
								</tr>
							</table>
							<xsl:choose>
								<xsl:when test="DLHDon/NDHDon/NMua/STKNHang!=''">
									<table style="width:100%;line-height:25px;font-size:10.5pt">
										<tr>
											<td style="padding-left:5px;">
												Số tài khoản:
												<!-- </td>
                                        <td> -->

											</td>
											<td style="border-bottom: 2px dotted black">
												<xsl:value-of select="DLHDon/NDHDon/NMua/STKNHang" />
												<xsl:if test="$STKNHangNMua!=''">
													<xsl:value-of select="$STKNHangNMua" />
												</xsl:if>
												&#160;&#160;&#160; Tại:
												<xsl:value-of select="DLHDon/NDHDon/NMua/TNHang" />
												<xsl:if test="$TNHangNMua!=''">
													<xsl:value-of select="$TNHangNMua" />
												</xsl:if>
											</td>
										</tr>
									</table>
								</xsl:when>
								<xsl:otherwise></xsl:otherwise>
							</xsl:choose>
							<table style="width:100%;line-height:25px;font-size:10.5pt">
								<tr>
									<td style="padding-left:5px;width:18%;">
										Hình thức thanh toán:
										<!-- <i>(Payment Method)</i>:
                                    </td>
                                    <td> -->

									</td>
									<td style="border-bottom: 2px dotted black;width:32%;">
										<xsl:value-of select="DLHDon/TTChung/HTTToan" />
										<td style="width:15%;"> Đồng tiền thanh toán:</td>
										<td style="border-bottom: 2px dotted black;width:35%;">
											<xsl:value-of select="DLHDon/TTChung/DVTTe" />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;
										</td>
									</td>
									<td>
										<xsl:if test="DLHDon/TTChung/TGia !=''">
											Tỷ giá:
											<xsl:value-of select="format-number(DLHDon/TTChung/TGia, '###.###.###','number')" />
										</xsl:if>
									</td>
								</tr>
							</table>
							<!-- <table style="width:100%;line-height:25px;font-size:10.5pt">
                                <tr>
                                    <td style="padding-left:5px;">Đồng tiền thanh toán
                                        <i>(Payment currency:
)</i>:
                                    </td>
                                    <td>
                                        <xsl:value-of select="DLHDon/TTChung/DVTTe" />&#160;&#160;&#160;&#160;
                                        <xsl:if test="DLHDon/TTChung/TGia !=''">Tỷ giá:
                                            <xsl:value-of select="format-number(DLHDon/TTChung/TGia, '###.###.###','number')" />
                                        </xsl:if>
                                    </td>
                                </tr>
                            </table> -->
							<!-- <div id="watermark">
Hoa don chua phat hanh
</div> -->
							<xsl:choose>
								<xsl:when test="DLHDon/TTChung/TTHDLQuan!=''">
									<div style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt">
										Hóa đơn
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
							<div style="padding-top:10px;"></div>
							<!-- Thông tin bảng hàng hóa -->
							<div style="background:url('paramWaterMarkTable;');background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;">
								<table style="width:100%;text-align:center; font-size:12pt;border-top: 1px solid black;border-bottom:1px solid black;border-left:1px solid black;border-right:none;" >
									<tr style="height:25px;">
										<td width="5%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black">
											<span style="font-size: 12pt">
												<b>
													STT
													<br/>
													<i>(No.)</i>
												</b>
											</span>
										</td>
										<td style="border: 1px solid black;" width="30%">
											<span style="font-size: 12pt;">
												<b>
													Tên hàng hóa, dịch vụ
													<br/>
													<i>(Name of goods and services)</i>
												</b>
											</span>
										</td>
										<td width="5%" style="border: 1px solid black">
											<span style="font-size: 12pt">
												<b>
													ĐVT
													<br/>
													<i>(Unit)</i>
												</b>
											</span>
										</td>
										<td width="7%" style="border: 1px solid black">
											<span style="font-size: 12pt">
												<b>
													Số lượng
													<br/>
													<i>(Quantity)</i>
												</b>
											</span>
										</td>
										<td width="10%" style="border: 1px solid black">
											<span style="font-size: 12pt">
												<b>
													Đơn giá
													<br/>
													<i>
														(Unit price
														)
													</i>
													<!--<br/> trước thuế<br/> GTGT-->
												</b>
											</span>
										</td>
										<td width="13%" style="border:1px solid black">
											<span style="font-size: 12pt">
												<b>
													Thành tiền
													<br/>
													<i>(Total amount)</i>
													<!--<br/> trước thuế <br/>GTGT-->
												</b>
											</span>
										</td>
										<td width="7%" style="border: 1px solid black">
											<span style="font-size: 12pt">
												<b>
													Thuế suất GTGT(%)
													<br/>
													<i>(VAT rate %)</i>
												</b>
											</span>
										</td>
									</tr>
									<tr style="height:25px;">
										<td width="5%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black">
											1
										</td>
										<td style="border: 1px solid black;" width="30%">
											2
										</td>
										<td width="5%" style="border: 1px solid black">
											3
										</td>
										<td width="7%" style="border: 1px solid black">
											4
										</td>
										<td width="10%" style="border: 1px solid black">
											5
										</td>
										<td width="13%" style="border:1px solid black">
											6=4x5
										</td>
										<td width="7%" style="border: 1px solid black">
											7
										</td>
									</tr>
									<xsl:variable name="lien" select="$paramlien" />
									<xsl:choose>
										<xsl:when test="$lien='0'">
											<xsl:choose>
												<xsl:when test="count(DLHDon/NDHDon/DSHHDVu/HHDVu) &lt; 11" >
													<xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
														<tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
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
															<td style="width:30%;text-align:left;border-right:1px solid black;padding-left:3px">
																<xsl:value-of select="THHDVu" />
															</td>
															<td width="5%" style="text-align:center;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh !='0'">
																		<xsl:value-of select="DVTinh" />
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="7%" style="text-align:center;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh!='0'">
																		<xsl:choose>
																			<xsl:when test="SLuong!=''">
																				<xsl:choose>
																					<xsl:when test="SLuong &gt; 1">
																						<xsl:value-of select="format-number(SLuong,'#.###.###,#######','vnd')" />
																					</xsl:when>
																					<xsl:otherwise>
																						<xsl:value-of select="format-number(SLuong,'#.###.###.##0,#######','vnd')" />
																					</xsl:otherwise>
																				</xsl:choose>
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="10%" style="text-align:right;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh!='0'">
																		<xsl:choose>
																			<xsl:when test="DGia!=''">
																				<xsl:value-of select="format-number(DGia, '#.###,#######','vnd')" />
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="13%" style="border-right:1px solid black!important;text-align:right">
																<xsl:choose>
																	<xsl:when test="ThTien!=''">
																		<xsl:choose>
																			<xsl:when test="$DVTTe!='VND'">
																				<xsl:value-of select="format-number(ThTien, '#.###,#######','vnd')" />
																			</xsl:when>
																			<xsl:otherwise>
																				<xsl:value-of select="format-number(ThTien, '#.###,#######','vnd')" />
																			</xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																	<xsl:otherwise></xsl:otherwise>
																</xsl:choose>
															</td>
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
																				</xsl:choose>
																			</xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																	<xsl:otherwise></xsl:otherwise>
																</xsl:choose>
															</td>
														</tr>
													</xsl:for-each>
												</xsl:when>
												<xsl:otherwise>
													<xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
														<tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
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
															<td style="width:30%;text-align:left;border-right:1px solid black;padding-left:3px">
																<xsl:value-of select="THHDVu" />
															</td>
															<td width="5%" style="text-align:center;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh !='0'">
																		<xsl:value-of select="DVTinh" />
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="7%" style="text-align:center;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh!='0'">
																		<xsl:choose>
																			<xsl:when test="SLuong!=''">
																				<xsl:choose>
																					<xsl:when test="SLuong &gt; 1">
																						<xsl:value-of select="format-number(SLuong,'#.###.###,#######','vnd')" />
																					</xsl:when>
																					<xsl:otherwise>
																						<xsl:value-of select="format-number(SLuong,'#.###.###.##0,#######','vnd')" />
																					</xsl:otherwise>
																				</xsl:choose>
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="10%" style="text-align:right;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh!='0'">
																		<xsl:choose>
																			<xsl:when test="DGia!=''">
																				<xsl:value-of select="format-number(DGia, '#.###,#######','vnd')" />
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="13%" style="border-right:1px solid black!important;text-align:right">
																<xsl:choose>
																	<xsl:when test="ThTien!=''">
																		<xsl:choose>
																			<xsl:when test="$DVTTe!='VND'">
																				<xsl:value-of select="format-number(ThTien, '#.###,#######','vnd')" />
																			</xsl:when>
																			<xsl:otherwise>
																				<xsl:value-of select="format-number(ThTien, '#.###,#######','vnd')" />
																			</xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																	<xsl:otherwise></xsl:otherwise>
																</xsl:choose>
															</td>
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
																				</xsl:choose>
																			</xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																	<xsl:otherwise></xsl:otherwise>
																</xsl:choose>
															</td>
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

														<tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
															<td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;">
																<xsl:choose>
																	<xsl:when test="TChat!=4">
																		<xsl:value-of select="STT" />
																	</xsl:when>
																	<xsl:otherwise></xsl:otherwise>
																</xsl:choose>
															</td>
															<td style="width:30%;text-align:left;border-right:1px solid black;padding-left:3px">
																<xsl:value-of select="THHDVu" />
															</td>
															<td width="5%" style="text-align:center;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh !='0'">
																		<xsl:value-of select="DVTinh" />
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="7%" style="text-align:center;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh!='0'">
																		<xsl:choose>
																			<xsl:when test="SLuong!=''">
																				<xsl:choose>
																					<xsl:when test="SLuong &gt; 1">
																						<xsl:value-of select="format-number(SLuong,'#.###.###,#######','vnd')" />
																					</xsl:when>
																					<xsl:otherwise>
																						<xsl:value-of select="format-number(SLuong,'#.###.###.##0,#######','vnd')" />
																					</xsl:otherwise>
																				</xsl:choose>
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="10%" style="text-align:right;border-right:1px solid black">
																<xsl:choose>
																	<xsl:when test="DVTinh!='0'">
																		<xsl:choose>
																			<xsl:when test="DGia!=''">
																				<xsl:value-of select="format-number(DGia, '#.###,#######','vnd')" />
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																</xsl:choose>
															</td>
															<td width="13%" style="border-right:1px solid black!important;text-align:right">
																<xsl:choose>
																	<xsl:when test="ThTien!=''">
																		<xsl:choose>
																			<xsl:when test="$DVTTe!='VND'">
																				<xsl:value-of select="format-number(ThTien, '#.###,#######','vnd')" />
																			</xsl:when>
																			<xsl:otherwise>
																				<xsl:value-of select="format-number(ThTien, '#.###,#######','vnd')" />
																			</xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																	<xsl:otherwise></xsl:otherwise>
																</xsl:choose>
															</td>
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
																				</xsl:choose>
																			</xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																	<xsl:otherwise></xsl:otherwise>
																</xsl:choose>
															</td>
														</tr>
													</xsl:when>
												</xsl:choose>
											</xsl:for-each>
										</xsl:otherwise>
									</xsl:choose>
									
									<xsl:choose>
										<xsl:when test="$soHHdu &gt; 0">
											<!-- <xsl:for-each select="(//node())[$soHHdu >= position()]">
                                                 <tr style="height:25px;border-top: none!important;border-bottom:1px dotted gray;">
                                                 <td width="5%" style="border-left:none!important; border-right:1px solid black;text-align:center;"></td>
                                                 <td style="width:30%;text-align:left;border-right:1px solid black;padding-left:3px"></td>
                                                 <td width="5%" style="text-align:center;border-right:1px solid black"></td>
                                                 <td width="7%" style="text-align:center;border-right:1px solid black"></td>
                                                 <td width="10%" style="text-align:right;border-right:1px solid black"></td>
                                                 <td width="13%" style="border-right:1px solid black!important;text-align:right"></td>
                                                 <td style="border-right: 1px solid black;text-align:right;padding-right:5px" width="7%"></td>
                                                 </tr>
                                                 </xsl:for-each> -->
										</xsl:when>
										<xsl:otherwise></xsl:otherwise>
									</xsl:choose>
								</table>
							</div>

							<div style="width:100%;display:paramfooter">
								<div style="idparamTongtien">
									<xsl:choose>
										<xsl:when test="$somucthue &gt; 1">
											<table style="width:100%;text-align:left;border-left:1px solid black;border-top:1px solid black;font-size:10.5pt;border-right:1px solid black; ">
												<tr style="border:1px solid black">
													<td style="border-right:1px solid none;text-align:left">
														Tổng tiền chưa có thuế GTGT:
														<!-- <i>(Total amount without VAT)</i>: -->
													</td>
													<td style="border-right:1px solid black;text-align:right">
														<xsl:choose>
															<xsl:when test="DLHDon/NDHDon/TToan/TgTCThue!=''">
																<xsl:choose>
																	<xsl:when test="$DVTTe!='VND'">
																		<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTCThue, '#.###,#######','vnd')" />
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTCThue, '#.###,#######','vnd')" />
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:when>
															<xsl:otherwise></xsl:otherwise>
														</xsl:choose>
													</td>
												</tr>
												<xsl:for-each select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat">
													<xsl:sort select="TSuat" />
													<tr>
														<td style="border-bottom: 1px solid black; border-left: 1px solid black">
															Tổng thuế:
															<!-- <i>(Total tax)</i>: -->
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
															<xsl:value-of   select="format-number(TThue, '#.###,#######','vnd')"/>
														</td>
													</tr>
												</xsl:for-each>
												<tr style="border:1px solid black">
													<td style="border-right:1px solid none;text-align:left">
														Tổng tiền thuế giá trị gia tăng:
														<!-- <i>(
Total value added tax)</i>: -->
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
																		<xsl:choose>
																			<xsl:when test="DLHDon/NDHDon/TToan/TgTThue!=''">
																				<xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTThue, '#.###,#######','vnd')"/>
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:otherwise>
														</xsl:choose>
													</td>
												</tr>
												<tr style="border:1px solid black;font-weight:bold">
													<td style="border-right:1px solid nonek;text-align:left">
														Tổng cộng tiền thanh toán:
														<!-- <i>
													(Total payment
													)
												</i>: -->
													</td>
													<td style="border-right:1px solid black;text-align:right">
														<xsl:choose>
															<xsl:when test="DLHDon/NDHDon/TToan/TgTTTBSo!=''">
																<xsl:choose>
																	<xsl:when test="$DVTTe!='VND'">
																		<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###,#######','vnd')" />
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###,#######','vnd')" />
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:when>
															<xsl:otherwise></xsl:otherwise>
														</xsl:choose>
													</td>
												</tr>
											</table>
										</xsl:when>
										<xsl:otherwise>
											<table style="width: 100%; border: none;;font-size:10.5pt" class="textfont">
												<tr style="height: 30px;">
													<!-- <td style="border-left: none!important; border-right: none"></td>
                                                    <td style="width:10%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:5%;border-left: none!important; border-right: none;">

											</td>
                                                    <td style="width:5%;border-left: none!important; border-right: none"></td> -->
													<td style="width:50%;border-left: none!important; border-right: none;padding-left:10px;"></td>
													<td style="width:20.5%;border-left: none!important; border-right: none;text-align:right;">
														<b>Cộng tiền hàng:&#160;</b>
														<!-- <i>
												(Total)</i>: -->
													</td>
													<td style="width:20%;border-right: none!important;text-align:right;border-bottom: 2px dotted black">
														<xsl:choose>
															<xsl:when test="DLHDon/NDHDon/TToan/TgTCThue!=''">
																<xsl:choose>
																	<xsl:when test="$DVTTe!='VND'">
																		<b>
																			<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTCThue, '#.###,#######','vnd')" />
																		</b>
																	</xsl:when>
																	<xsl:otherwise>
																		<b>
																			<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTCThue, '#.###,#######','vnd')" />
																		</b>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:when>
															<xsl:otherwise></xsl:otherwise>
														</xsl:choose>
													</td>
												</tr>
											</table>
											<table style="width: 100%; border: none;;font-size:10.5pt" class="textfont">
												<tr style="height: 30px; border: none;">
													<td style="width:50%;border-left: none!important; border-right: none;padding-left:10px;text-align:right">
														<b>Thuế suất GTGT:&#160;</b>
														<!-- <i>(VAT rate)</i>: -->
													</td>
													<td style="border-bottom: 2px dotted black">
														<xsl:for-each select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat">
															<xsl:choose>
																<xsl:when test="TSuat='KHAC:3.5%'">5% x 70%</xsl:when>
																<xsl:otherwise>
																	<xsl:choose>
																		<xsl:when test="TSuat='KHAC:7%'">10% x 70%</xsl:when>
																		<xsl:otherwise>
																			<b>
																				<xsl:value-of select="TSuat" />
																			</b>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
													</td>
													<!-- <td style="width:0%;border-left: none!important; border-right: none">

                    </td>
                                                    <td style="width:0%;border-left: none!important; border-right: none;">
                     
                    </td>
                                                    <td style="width:10%;border-left: none!important; border-right: none"></td> -->
													<td style="width:20.5%;border:none;text-align:right">
														<b>Tiền thuế GTGT:&#160;</b>
														<!-- <i>(VAT value)</i>: -->
													</td>
													<td style="width:22%;border-right: none!important;border-left: none;text-align:right;border-bottom: 2px dotted black">
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
																		<xsl:choose>
																			<xsl:when test="DLHDon/NDHDon/TToan/TgTThue!=''">
																				<b>
																					<xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTThue, '#.###,#######','vnd')"/>
																				</b>
																			</xsl:when>
																			<xsl:otherwise></xsl:otherwise>
																		</xsl:choose>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:otherwise>
														</xsl:choose>
													</td>
												</tr>
											</table>
											<table style="width: 100%; border:none;font-size:10.5pt" class="textfont">
												<tr style="height: 30px;">
													<!-- <td style="width:20%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:0%;border-left: none!important; border-right: none"></td>
                                                    <td style="width:15%;border-left: none!important; border-right: none;">
                     
                    </td>
                                                    <td style="width:10%;border-left: none!important; border-right: none"></td> -->
													<td style="width:50%;border-left: none!important; border-right: none;padding-left:10px;"></td>
													<td style="width:20.5%;border-left: none!important; text-align:right">
														<b>Tổng cộng tiền thanh toán:&#160;</b>
														<!-- <i>(Total payment)</i>: -->
													</td>
													<td style="width:20%;border-right: none!important;border-left: none;text-align:right;border-bottom: 2px dotted black">
														<xsl:choose>
															<xsl:when test="DLHDon/NDHDon/TToan/TgTTTBSo!=''">
																<xsl:choose>
																	<xsl:when test="$DVTTe!='VND'">
																		<b>
																			<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###,#######','vnd')" />
																		</b>
																	</xsl:when>
																	<xsl:otherwise>
																		<b>
																			<xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###,#######','vnd')" />
																		</b>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:when>
															<xsl:otherwise></xsl:otherwise>
														</xsl:choose>
													</td>
												</tr>
											</table>
										</xsl:otherwise>
									</xsl:choose>
									<table style="width:100%;text-align:left;font-size:10.5pt;border:none">
										<tr style="height:30px;border:none">
											<td width="18%" style="border-left:none!important; border-right:none!important; text-align:left;padding-left:3px" colspan="6">
												<b>Số tiền viết bằng chữ:</b>
											</td>
											<td width="82%" style="border-bottom: 2px dotted black">
												<span style="font-style:italic;font-size:10.5pt">
													<xsl:variable name="AmountWord" select="DLHDon/NDHDon/TToan/TgTTTBChu"/>
													<xsl:value-of select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))"/>
													./.
												</span>
											</td>
										</tr>
									</table>
									<!-- <table style="width:100%;text-align:left;border-left:1px solid black;border-top:1px solid black;font-size:10.5pt;border-right:1px solid black;font-size:10.5pt; ">
                                        <xsl:for-each select="DLHDon/NDHDon/TToan">
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px solid black;">
                                                <td style="border-bottom: 1px solid black; border-left: 1px solid black">
                                                  Tổng tiền chưa có thuế GTGT
                                                    <i class="SizeChu">(Total amount without VAT)</i>:
                                                </td>
                                                <td style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                                                    <xsl:value-of   select="format-number(TgTCThue, '###.###.###','number')"/>
                                                </td>
                                            </tr>
                                        </xsl:for-each>
                                        <xsl:for-each select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat">
                                            <xsl:sort select="TSuat" />
                                            <tr>
                                                <td  style="border-bottom: 1px solid black; border-left: 1px solid black">
  Tổng thuế
                                                    <i class="SizeChu">(Total tax)</i>:
                                                    <xsl:choose>
                                                        <xsl:when test="TSuat!='KHAC:7%'">
                                                            <xsl:choose>
                                                                <xsl:when test="TSuat!='KHAC:3.5%'">
                                                                    <xsl:choose>
                                                                        <xsl:when test="TSuat!='KHAC:2%'">
                                                                            <xsl:value-of   select="TSuat"/>
                                                                        </xsl:when>
                                                                        <xsl:otherwise>10% x 20%</xsl:otherwise>
                                                                    </xsl:choose>
                                                                </xsl:when>
                                                                <xsl:otherwise>5% x 70%</xsl:otherwise>
                                                            </xsl:choose>
                                                        </xsl:when>
                                                        <xsl:otherwise>10% x 70%</xsl:otherwise>
                                                    </xsl:choose>
                                                </td>
                                                <td  style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                                                    <xsl:value-of   select="format-number(TThue, '###.###.###','number')"/>
                                                </td>
                                            </tr>
                                        </xsl:for-each>
                                        <xsl:for-each select="DLHDon/NDHDon/TToan/DSLPhi/LPhi">
                                            <xsl:sort select="TPhi" />
                                            <xsl:if test="TPhi!=''">
                                                <tr>
                                                    <td   style="border-bottom: 1px solid black; border-left: 1px solid black">
                                                        <xsl:value-of   select=" TLPhi"/>
                                                    </td>
                                                    <td  style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                                                        <xsl:value-of   select="format-number(TPhi, '###.###.###','number')"/>
                                                    </td>
                                                </tr>
                                            </xsl:if>
                                        </xsl:for-each>
                                        <xsl:for-each select="DLHDon/NDHDon/TToan">
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px solid black;">
                                                <td style="border-bottom: 1px solid black; border-left: 1px solid black">
                                                   Tổng tiền thuế giá trị gia tăng
                                                    <i class="SizeChu">(
Total value added tax)</i>:
                                                </td>
                                                <td style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                                                    <xsl:value-of   select="format-number(TgTThue, '###.###.###','number')"/>
                                                </td>
                                            </tr>
                                            <xsl:if test="TTCKTMai!=''">
                                                <tr style="height:25px;border-top: none!important;border-bottom:1px solid black;">
                                                    <td style="border-bottom: 1px solid black; border-left: 1px solid black">
                                                    Tổng tiền chiết khấu thương mại
                                                        <i class="SizeChu">(
Total trade discount amount)</i>
                                                    </td>
                                                    <td style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                                                        <xsl:value-of   select="format-number(TTCKTMai, '###.###.###','number')"/>
                                                    </td>
                                                </tr>
                                            </xsl:if>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px solid black;">
                                                <td style="border-bottom: 1px solid black; border-left: 1px solid black">
                                                    <b>Tổng cộng tiền thanh toán
                                                        <i class="SizeChu">(Total payment )</i> :
                                                    </b>
                                                </td>
                                                <td style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                                                    <b>
                                                        <xsl:value-of   select="format-number(TgTTTBSo, '###.###.###','number')"/>
                                                    </b>
                                                </td>
                                            </tr>
                                            <tr style="height:25px;border-top: none!important;border-bottom:1px solid black;">
                                                <td style="border-bottom: 1px solid black; border-left: 1px solid black">
                                                  Số tiền viết bằng chữ
                                                    <i class="SizeChu">(In words)</i> :
                                                    <xsl:value-of select="TgTTTBChu" /> ./.
                                                </td>
                                            </tr>
                                        </xsl:for-each>
                                    </table> -->
									<table style="width: 100%;" class="textfont">
										<tr>
											<td style="border: none; padding-top: 1px; text-align: center;width:30%">
												<b>Người mua hàng</b>
												<!-- <i>(Buyer)</i> -->
												<br />
												(Ký, ghi rõ họ tên)
												<!-- <br/>
                                        <i>(Signature and full name)</i> -->
											</td>
											<td style="border: none; padding-top: 1px; text-align: center;width:40%">
												<div style="width:100%;text-align:center;paramNguoiCD">
													<b>Người chuyển đổi</b>
													<!-- <i>(Converter)</i> -->
													<br />
													(Ký, ghi rõ họ tên)
													<!-- <i>
                                                <br/>
                    (Signature and full name)
                                            </i> -->
												</div>
											</td>
											<td style="border: none; padding-top: 1px; text-align: center;width:30%">
												<b>Người bán hàng</b>
												<!-- <i>(Seller)</i> -->
												<br />
												(Ký, ghi rõ họ tên)
												<!-- <br/>
                                        <i>(Signature and full name)</i> -->
											</td>
										</tr>
										<tr>
											<td style="width: 30%"></td>
											<td style="width: 40%"></td>
											<td style="text-align:right;  height:80px; width: 30%;text-align:center;">
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
											<td colspan="3" style="text-align: center; border: none; padding-top: 0px;padding-bottom:0px">
												(Cần kiểm tra đối chiếu khi lập, giao, nhận hoá đơn)
											</td>
										</tr>
									</table>
									<!-- <div  style="padding-top:0px;text-align:left;padding-bottom:1px;font-size:11.5pt;align:center;px;-ms-transform: rotate(-90deg);-webkit-transform: rotate(-90deg);transform: rotate(-90deg);width:900px;left:485px;top:-700px;float:right;height:15px;position:relative;">
                            <i>
              Giải pháp hóa đơn điện tử được cung cấp bởi:
                                <b> Công ty cổ phần công nghệ thẻ Nacencomm</b>. Mã số thuế:
                                <b>0103930279</b>.
                            </i>
                        </div> -->
									<div style="width:100%;padding-top:0px;text-align:center;padding-bottom:0px;">
										<span style="font-size:12px;">
											Chuỗi xác thực
											<i>(Digest Value)</i>:
											<b >
												<xsl:value-of select="$digest" />
											</b>
										</span>
									</div>
									<div style="text-align: center;">
										<i>
											Giải pháp hóa đơn điện tử được cung cấp bởi:
											<b> Công ty cổ phần công nghệ thẻ Nacencomm</b>. Mã số thuế:
											<b>0103930279</b>.
										</i>
									</div>
									<div style="width:100%;padding-top:0px;text-align:center;padding-bottom:1px;">
										<span>
											<i>Tra cứu hóa đơn tại địa chỉ trang web: https://hoadon78.nacencomm.vn </i>
										</span>
									</div>
								</div>
							</div>
						</div>
						<!-- <xsl:variable name="lien" select="paramlien" />
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
                        </xsl:choose> -->
					</div>
				</body>
			</page>
		</html>
	</xsl:template>
</xsl:stylesheet>