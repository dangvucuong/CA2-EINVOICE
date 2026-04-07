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
		<xsl:variable name="NgayDocThangNay" select="DLHDon/TTChung/TTKhac/TTin[TTruong='NgayDocThangNay']/DLieu" />
		<xsl:variable name="NgayDocThangTruoc" select="DLHDon/TTChung/TTKhac/TTin[TTruong='NgayDocThangTruoc']/DLieu" />
		<xsl:variable name="SoCuong" select="DLHDon/TTChung/TTKhac/TTin[TTruong='SoCuong']/DLieu" />
		<xsl:variable name="MaKH" select="DLHDon/TTChung/TTKhac/TTin[TTruong='MaKH']/DLieu" />
		<xsl:variable name="ChiSoDHThangNay" select="DLHDon/TTChung/TTKhac/TTin[TTruong='ChiSoDHThangNay']/DLieu" />
		<xsl:variable name="ChiSoDHThangTruoc" select="DLHDon/TTChung/TTKhac/TTin[TTruong='ChiSoDHThangTruoc']/DLieu" />
		<xsl:variable name="ChiSoDHThangNayCu" select="DLHDon/TTChung/TTKhac/TTin[TTruong='ChiSoDHThangNayCu']/DLieu" />
		<xsl:variable name="ChiSoDHThangTruocCu" select="DLHDon/TTChung/TTKhac/TTin[TTruong='ChiSoDHThangTruocCu']/DLieu" />
		<xsl:variable name="TongSoNgay" select="DLHDon/TTChung/TTKhac/TTin[TTruong='TongSoNgay']/DLieu" />
		<xsl:variable name="Tieuthu" select="DLHDon/TTChung/TTKhac/TTin[TTruong='Tieuthu']/DLieu" />
		<xsl:variable name="MaNguoiMua" select="DLHDon/TTChung/TTKhac/TTin[TTruong='MaNguoiMua']/DLieu" />
		<html lang="en"
            xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<title>E-Invoice</title>
				<meta HTTP-EQUIV='Content-Type' CONTENT='text/html; charset=utf-8' />
				<style type="text/css">
              #tblContent td, #tblContent th {
              border:1px solid #214c70;
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
                color: '#214c70';
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
                margin: 5px;
				
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
				<body style="font-family:Arial; background: rgb(240,240,240);">
					<!-- <div id='qr' style="display:none">
                        <xsl:value-of select="//*[local-name() = 'DLQRCode']"/>
                    </div> -->
					<div style="viewstyle;border:none;background-color: hsla(0,0%,100%,0.8);background-size: 49%;padding-left:25px">
						<div id="background" style="paramMau">
                        

              MẪU
            </div>
						<div id="background" style="paramdisable;top:320px;">contentDisable</div>
						<div style="border:none;width:210mm;height: auto; min-height: auto;">
							<table style="width:100%;">
								<tr >
									<!-- <td style="width:15%;text-align:center;font-size:14pt; padding-top:1px;padding-left:0px!important;color:#214c70">
										<img style="height:37px;align-content:center;position:static;left:0;top:0;object-fit: scale-down;"
															  id="imgSample" src="paramLogo">
									</img>
									</td> -->
									<td style="width:40%;text-transform: uppercase;text-align:center;color:#214c70">
										<b>
											<xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
										</b>
									</td>
									<td style="width:40%;padding-top:5px;text-align:center; ">
										<span style="font-weight:bold; font-size:15pt;text-transform: uppercase;color:#214c70;font-family:arial">
											<xsl:value-of select="DLHDon/TTChung/THDon" />
										</span>
										<br/>
										<span style="text-align:center;font-size:9.5pt;color:#214c70">
											<br/>
											Tháng:
											<xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)"/>/
											<xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)"/>
										</span>
										<br/>
										<span style="font-weight:normal;font-size:9.5pt;display:param1_1">param1</span>
									</td>
									<td style="width:20%;padding-left:0px;padding-top:0px;color:#214c70">
										Mẫu số:
										<!-- <i>(Form)</i>: -->
										<b>
											<xsl:value-of select="DLHDon/TTChung/KHMSHDon" />
										</b>
										<br />
                    Ký hiệu:
										<!-- <i>(Serial No)</i>: -->
										<b>
											<!-- <xsl:value-of select="DLHDon/TTChung/KHMSHDon" /> -->
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
									<td style="width:40%;text-align:center;color:#214c70">
										<xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
										<br/>
										Mã số thuế:
										<span style="font-weight:bold; font-size:12pt">
											<du>
												<xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
											</du>
										</span>
										<br/>
										ĐT:
										<xsl:value-of select="DLHDon/NDHDon/NBan/SDThoai" />
									</td>
									<td style="width:40%;text-align:center;padding-left:0px">
										<div style="color: red; font-size: 10pt;padding-top:5px;  display: normal;text-align:center">
											<div style="paramChuyendoi">
							HOÁ ĐƠN CHUYỂN ĐỔI
												<br />
							TỪ HOÁ ĐƠN ĐIỆN TỬ
											</div>
										</div>
									</td>
									<td style="width:20%;text-align:left;color:#214c70;font-size:9.5pt;">
										Công ty sẽ tạm dừng cung cấp nước nếu khách hàng không trả tiền đúng thời gian
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
							<table style="width:100%;text-align:center; font-size:9.5pt;border-top: 1px solid #214c70;border-bottom:0px solid #214c70;border-left:1px solid #214c70;border-right:1px solid #214c70;color:#214c70" >
								<tr>
									<td style ="text-align:left;padding-left:10px;border-bottom:1px solid #214c70;height:28px;font-weight: bold;border-right:1px solid #214c70;"  colspan="6"> MÃ CQT CẤP:
										<xsl:if test="MCCQT !=''">
											<xsl:value-of select="MCCQT"/>
										</xsl:if>
									</td>
								</tr>
								<tr style="height:28px;">
									<td width="28%" style="padding-top:1px;padding-bottom:1px;color:#214c70">
										<b>Họ và tên địa chỉ khách hàng</b>
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										Ngày đọc tháng này
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										Ngày đọc tháng trước
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										Số ngày sử dụng
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										Ngày làm hóa đơn
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										Số cuống
									</td>
								</tr>
								<tr style="height:28px;">
									<td width="28%" style ="text-align:left;padding-left:10px;border-bottom:1px solid #214c70;border: 1px solid #214c70;">
										Mã khách hàng:
										<xsl:value-of select="$MaNguoiMua"/>
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										<xsl:value-of select="$NgayDocThangNay"/>
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										<xsl:value-of select="$NgayDocThangTruoc"/>
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										<xsl:value-of select="$TongSoNgay"/>
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										<xsl:value-of select="substring(DLHDon/TTChung/NLap,9,2)"/>/
										<xsl:value-of select="substring(DLHDon/TTChung/NLap,6,2)"/>/
										<xsl:value-of select="substring(DLHDon/TTChung/NLap,0,5)"/>
									</td>
									<td width="14%" style="border: 1px solid #214c70;">
										<xsl:value-of select="$SoCuong"/>
									</td>
								</tr>
								<!-- </table>
							<table style="line-height: 1.5;width:100%;text-align:center; font-size:9.5pt;border-top: 0px solid #214c70;border-bottom:0px solid #214c70;border-left:1px solid #214c70;border-right:1px solid #214c70;color:#214c70" > -->
								<tr style="height:28px;">
									<td width="28.5%" rowspan="8" style ="text-align:left;padding-left:10px;border-bottom:0px solid #214c70;border-top: 1px solid #214c70;border-left:1px solid #214c70;border-right:1px solid #214c70;">
									<span style="line-height: 2">Seri đồng hồ:
										<br/>
										Tên khách hàng:
										<xsl:value-of select="DLHDon/NDHDon/NMua/Ten" />
										<xsl:value-of select="DLHDon/NDHDon/NMua/HVTNMHang" />
										<br/>
										Địa chỉ:
										<xsl:value-of select="DLHDon/NDHDon/NMua/DChi" />
										<br/>
									Mã số thuế:
										<xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
										<br/>
									Phương thức thanh toán:
										<xsl:value-of select="DLHDon/TTChung/HTTToan" /></span>	
										<br/>
										<b>Đơn vị bán hàng </b>
										<br/>
										Được ký bởi:
										<br/>
										
										<div style="paramSign;background-image:url(data:image/jpeg;base64,/9j/4QbTRXhpZgAATU0AKgAAAAgADAEAAAMAAAABAaUAAAEBAAMAAAABAKYAAAECAAMAAAADAAAAngEGAAMAAAABAAIAAAESAAMAAAABAAEAAAEVAAMAAAABAAMAAAEaAAUAAAABAAAApAEbAAUAAAABAAAArAEoAAMAAAABAAMAAAExAAIAAAAfAAAAtAEyAAIAAAAUAAAA04dpAAQAAAABAAAA6AAAASAACAAIAAgACIuAAAAnEAAIi4AAACcQQWRvYmUgUGhvdG9zaG9wIDIyLjMgKFdpbmRvd3MpADIwMjQ6MDE6MjYgMTc6MDI6MzcAAAAEkAAABwAAAAQwMjMxoAEAAwAAAAH//wAAoAIABAAAAAEAAAGloAMABAAAAAEAAACmAAAAAAAAAAYBAwADAAAAAQAGAAABGgAFAAAAAQAAAW4BGwAFAAAAAQAAAXYBKAADAAAAAQACAAACAQAEAAAAAQAAAX4CAgAEAAAAAQAABU0AAAAAAAAASAAAAAEAAABIAAAAAf/Y/+0ADEFkb2JlX0NNAAL/7gAOQWRvYmUAZIAAAAAB/9sAhAAMCAgICQgMCQkMEQsKCxEVDwwMDxUYExMVExMYEQwMDAwMDBEMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMAQ0LCw0ODRAODhAUDg4OFBQODg4OFBEMDAwMDBERDAwMDAwMEQwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAA/AKADASIAAhEBAxEB/90ABAAK/8QBPwAAAQUBAQEBAQEAAAAAAAAAAwABAgQFBgcICQoLAQABBQEBAQEBAQAAAAAAAAABAAIDBAUGBwgJCgsQAAEEAQMCBAIFBwYIBQMMMwEAAhEDBCESMQVBUWETInGBMgYUkaGxQiMkFVLBYjM0coLRQwclklPw4fFjczUWorKDJkSTVGRFwqN0NhfSVeJl8rOEw9N14/NGJ5SkhbSVxNTk9KW1xdXl9VZmdoaWprbG1ub2N0dXZ3eHl6e3x9fn9xEAAgIBAgQEAwQFBgcHBgU1AQACEQMhMRIEQVFhcSITBTKBkRShsUIjwVLR8DMkYuFygpJDUxVjczTxJQYWorKDByY1wtJEk1SjF2RFVTZ0ZeLys4TD03Xj80aUpIW0lcTU5PSltcXV5fVWZnaGlqa2xtbm9ic3R1dnd4eXp7fH/9oADAMBAAIRAxEAPwD0VJJJQsakkkklKSSSSUpJJJJSkkkklKSSSSUpJJJJSkPIyK8eve/UnRjBy4/+R/lJZGRXj1736k6MYOXFY1ttl1hssMuP3Afut/kqPLl4dB835MWXLw6D5vyb+P1RrjtyAGTw9s7fg793+ur3nyDqCufVnDycit7aqh6jXHSo/iWn/BqPHnO0tfHqx48x2lr49XXSTplZbL//0PRUkklCxqSSSSUpJJJJSkkkvM6AaklJSklmZmebJqoMV/nP4Lv6v7rP+rQsXNsxvb9Or9zw/wCL/dURzxEq6fvMRzxEq6fvOwko1W13M9Sp25vB8QfBw/NUlKDeoZAb1CkPIyK8eve/UnRrRy4pZGRXj1736zo1o5cfBY111l9hssMuOgA4A/dao8uXh0Hzfkx5cvDoPmVddZdYbLDLjwBwB+61QSTta57gxgLnO0DR3VTUnuS1dSe5K7GPseGVjc92gAWxiYrMZmnusd9N/wD3xv8AITYmI3GYfzrXfTd2/qN/ko6tYsXDqfm/6LZxYuHU/N/0VJJJKVmf/9H0VJJJQsakkkklKSSSSUpVeo05FtQFR3MGr6xy7wP8rb+4rSSEo8QIPVbKPECD1efSWvl4LL5eyGXfvdnf1/8AyayrK31vLLGlrxyCqc8Zgddu7UnjMDrt3Xqtspf6lR2u7+BH7rh+ctJnU6DUXvBbY3/Bjuf5Dv3VlJJQySjsqGSUdkl11l9hssMk6AdgP3WoaSdrXPcGMBc52jWjum6k9yVupPclTWue4MYNznGGtHdbGJiNxmEmHWuHvd2A/cYliYbcZsmHXOHvd4fyGI6tYsXDrL5v+i2cWLh1Pzf9FSSSSlZlJJJJKf/S9FSSSULGpJJJJSkkkklKSSSSUpDvoqyGbLBx9Fw5b/VREkiARR1QQCKLjZOJbjH3e5h+jYOD5O/dcgLffs2O9SNke7dxH8qVlZOLS078axjmH/Blwkf1S4+5qrZMJGsdR26tfJhI1jqO3VrNa5zg1oLnOMNaOSVr4eI3GbLoda4e53YD9xihgU0VcWNsvcPcWkGB+6z/AL85W1JixcPql83/AEV+LFw+qXzf9FSSSSlZlJJJJKUkkkkp/9n/7Q7sUGhvdG9zaG9wIDMuMAA4QklNBAQAAAAAAAccAgAAAgAAADhCSU0EJQAAAAAAEOjxXPMvwRihontnrcVk1bo4QklNBDoAAAAAAOUAAAAQAAAAAQAAAAAAC3ByaW50T3V0cHV0AAAABQAAAABQc3RTYm9vbAEAAAAASW50ZWVudW0AAAAASW50ZQAAAABDbHJtAAAAD3ByaW50U2l4dGVlbkJpdGJvb2wAAAAAC3ByaW50ZXJOYW1lVEVYVAAAAAEAAAAAAA9wcmludFByb29mU2V0dXBPYmpjAAAADABQAHIAbwBvAGYAIABTAGUAdAB1AHAAAAAAAApwcm9vZlNldHVwAAAAAQAAAABCbHRuZW51bQAAAAxidWlsdGluUHJvb2YAAAAJcHJvb2ZDTVlLADhCSU0EOwAAAAACLQAAABAAAAABAAAAAAAScHJpbnRPdXRwdXRPcHRpb25zAAAAFwAAAABDcHRuYm9vbAAAAAAAQ2xicmJvb2wAAAAAAFJnc01ib29sAAAAAABDcm5DYm9vbAAAAAAAQ250Q2Jvb2wAAAAAAExibHNib29sAAAAAABOZ3R2Ym9vbAAAAAAARW1sRGJvb2wAAAAAAEludHJib29sAAAAAABCY2tnT2JqYwAAAAEAAAAAAABSR0JDAAAAAwAAAABSZCAgZG91YkBv4AAAAAAAAAAAAEdybiBkb3ViQG/gAAAAAAAAAAAAQmwgIGRvdWJAb+AAAAAAAAAAAABCcmRUVW50RiNSbHQAAAAAAAAAAAAAAABCbGQgVW50RiNSbHQAAAAAAAAAAAAAAABSc2x0VW50RiNSbHRAxACjwAAAAAAAAAp2ZWN0b3JEYXRhYm9vbAEAAAAAUGdQc2VudW0AAAAAUGdQcwAAAABQZ1BDAAAAAExlZnRVbnRGI1JsdAAAAAAAAAAAAAAAAFRvcCBVbnRGI1JsdAAAAAAAAAAAAAAAAFNjbCBVbnRGI1ByY0BZAAAAAAAAAAAAEGNyb3BXaGVuUHJpbnRpbmdib29sAAAAAA5jcm9wUmVjdEJvdHRvbWxvbmcAAAAAAAAADGNyb3BSZWN0TGVmdGxvbmcAAAAAAAAADWNyb3BSZWN0UmlnaHRsb25nAAAAAAAAAAtjcm9wUmVjdFRvcGxvbmcAAAAAADhCSU0D7QAAAAAAEACOPXAAAgABAI49cAACAAE4QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAB44QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAE4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAA4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAE4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADkwAAAAYAAAAAAAAAAAAAAKYAAAGlAAAALwB6ADUAMQAwADgANAA1ADUAMAAyADEANAAwADMAXwA2ADUANgAxAGMAYgBmADMAOAAwADMANgBmAGIANAA2AGQAYwA2AGMAZQA1AGEANgBjADgAOAA1ADgAZgAwADYAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAaUAAACmAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAEAAAAAAABudWxsAAAAAgAAAAZib3VuZHNPYmpjAAAAAQAAAAAAAFJjdDEAAAAEAAAAAFRvcCBsb25nAAAAAAAAAABMZWZ0bG9uZwAAAAAAAAAAQnRvbWxvbmcAAACmAAAAAFJnaHRsb25nAAABpQAAAAZzbGljZXNWbExzAAAAAU9iamMAAAABAAAAAAAFc2xpY2UAAAASAAAAB3NsaWNlSURsb25nAAAAAAAAAAdncm91cElEbG9uZwAAAAAAAAAGb3JpZ2luZW51bQAAAAxFU2xpY2VPcmlnaW4AAAANYXV0b0dlbmVyYXRlZAAAAABUeXBlZW51bQAAAApFU2xpY2VUeXBlAAAAAEltZyAAAAAGYm91bmRzT2JqYwAAAAEAAAAAAABSY3QxAAAABAAAAABUb3AgbG9uZwAA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TRVxY2y9w9xaQYH7rP8AvzlbUmLFw+qXzf8ARX4sXD6pfN/0VJJJKVmUkkkkpSSSSSn/2QA4QklNBCEAAAAAAFcAAAABAQAAAA8AQQBkAG8AYgBlACAAUABoAG8AdABvAHMAaABvAHAAAAAUAEEAZABvAGIAZQAgAFAAaABvAHQAbwBzAGgAbwBwACAAMgAwADIAMQAAAAEAOEJJTQQGAAAAAAAHAAgBAQABAQD/4Q29aHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA2LjAtYzAwNiA3OS4xNjQ3NTMsIDIwMjEvMDIvMTUtMTE6NTI6MTMgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtbG5zOnBob3Rvc2hvcD0iaHR0cDovL25zLmFkb2JlLmNvbS9waG90b3Nob3AvMS4wLyIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6ZTQ0MDkwMGItNzEzYS1hNDQwLTg4MTctMGM5YTk3YjcwNGRlIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOmI4ZDNjMzU1LWI5YjMtNzI0Zi1iOTI5LTc4OTNmNTc1NmU1NyIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSI0MjFFNTNGMTgzRkEzNEJEQjg4OUY3M0NEMUNERDFDQyIgZGM6Zm9ybWF0PSJpbWFnZS9qcGVnIiBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIiBwaG90b3Nob3A6SUNDUHJvZmlsZT0iIiB4bXA6Q3JlYXRlRGF0ZT0iMjAyNC0wMS0yNlQxNjo1OToyOCswNzowMCIgeG1wOk1vZGlmeURhdGU9IjIwMjQtMDEtMjZUMTc6MDI6MzcrMDc6MDAiIHhtcDpNZXRhZGF0YURhdGU9IjIwMjQtMDEtMjZUMTc6MDI6MzcrMDc6MDAiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJzYXZlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDo3OTkzNDVlNS1mYjE2LWQxNGMtYjg1MS1hOTc0ZmE2ZTVkODUiIHN0RXZ0OndoZW49IjIwMjQtMDEtMjZUMTc6MDI6MzcrMDc6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCAyMi4zIChXaW5kb3dzKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6YjhkM2MzNTUtYjliMy03MjRmLWI5MjktNzg5M2Y1NzU2ZTU3IiBzdEV2dDp3aGVuPSIyMDI0LTAxLTI2VDE3OjAyOjM3KzA3OjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgMjIuMyAoV2luZG93cykiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDw/eHBhY2tldCBlbmQ9InciPz7/7gAhQWRvYmUAZEAAAAABAwAQAwIDBgAAAAAAAAAAAAAAAP/bAIQAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQICAgICAgICAgICAwMDAwMDAwMDAwEBAQEBAQEBAQEBAgIBAgIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMD/8IAEQgApgGlAwERAAIRAQMRAf/EANkAAQACAgMBAQEAAAAAAAAAAAAHCAQJAgMFBgEKAQEAAgMBAQEAAAAAAAAAAAAABwgDBAYCAQUQAAEEAgICAgICAgMAAAAAAAQCAwUGAQcgUDBAABAxCBETEjIzFBURAAMAAQIFAgMGAwUJAAAAAAECAwQRBQAhEhMGMSIgUBQwQEEyIxUQUWFxoSQ0VLFSYkODRGQlFhIAAgECBAIFCAYJAwQDAAAAAQIDEQQhMUEFAFFQYXESIiAwgbEyQlITEECRocFi8NFyssIjMxQG4dJD8YKS4lMkFf/aAAwDAQECEQMRAAAA/s/jfhwAAAAAAAAAAAAAAAAAAABw8+efr0AAAAAAAAAAAAAAAAAAAAAAAABCnAcRQCvFeLPytK1wZqmLKzZgAAAAAAAAAAAAAAAAAAAAAABCXBcNr7rnXfBwa4kDoejv5Yywkh9H0QAAAAAAAAAAAAAAAAAAAAAAhjguL16VxrjhYsAAzM+e28xTBbSY5dyMmUAAAAAAAAAAAAAAAAAAAACGuB4zXlXKuOFi1wABm58+yi0VnJK6fpgAAAAAAAAAAAAAAAAAAABDXD8dr0rdW/Awa4AAzM2bYPY+xc2d33AAAAAAAAAAAAAAAAAAAAAiTi+R13Vtrf5mrqAADLzZthNkLGTj3vddPjwAAAAAAAAAAAAAB4X5+h6m5tZOTKAAAAIh4rj9d9bK3+dr6wAAzc+bYJY6w83d33gAAAAAAAAAAAAAAjLk+Y14Vurf9h+3+1fuw9gvsf2/2wAABEnF8jrwrZXDytbTAAHd7933sRYSwUiyEAAAAAAAAAAAAAAIT4Lh9f8AXSvHma+qPb3t69NgJ9nuQu+AAET8VyeuuttbfM1tQAAd3v3fKwk/WHkmSAAAAAAAAAAAAAABW2MI1o1AUC4+PEAOX37aiWJZubOc1ZGTKBFPH8nrsrRW3ydbUAAHd793xsFP1iJLkgAAAAAAAAAAAAAAQhwPD68q3Vw4fPIAAEsdh19+rEWC+t/Y/Yivj+V101prX5WrqAADt9+722Bn2ycoSR04/oAAAAAAAAAAAAAHIxseOmsIwvVOIYj4vgAAH0H6H6Vspal2osQQ74+ppAADs9er32Cn+xkmSQAAAAAAAAAAAAAAAAIDjyP6LQBAfi6OkAAAAAAO316vZYKe7HSZJZ9AAAAAAAAAAAAAAAAA+J/C/DoBXOvMa8xzAAAAAA5+vt7J/n2yEnyWAAAAAAAAAAAAAAAAAAMLDgpFA0G1qi6LwAAAB2evV6J/ny0Mqyhg4cQAAAAAAAAAAAAAAAAAAH4+VyjKOKPQJA3n6+uAABy+/btztOVopZlYAAAAAAAAAAAAAAAAAAAACP8AmueoBXavMf8AO86AB+vt0Jymu10vS6AAAAAAAAAAAAAAAAAAAAAB5mpqUagKCK8xvHP4+Dk+3OnGbLXy/LgAAAAAAAAAAAAAAAAAAAAAAArBFEWUrg6EMXBhuZN812xmCXQAAAAAAAAAAAAAAAAAAAAAAABBEe8FDPEcTd2eZz7DrAAAAAAAAAP/2gAIAQIAAQUA6rKv6XOzud3arqHjSyCqlsbLKRyGCmuwud1brbBRL5hH1A2aVrz9ctEZZReuu1yZgBnnnSHeA5JAj1Y2W3nLLzRDXV3W5sV8Z994l3kKKQaRUId+ChequdzYrjBJL5hHIcd8t+l0tmvMdVb7cLWxDTSZAnkOO+W/S6YJXmHPz7shLRcSll5khrxXG4D10Moog0jkMKQY/TaYPXGPetFtj60NKzElNlVi4yVafgrDGTw/gt1xGrYxhhJ5PJttx5yjUz/wWfet90FrjJZZJxH1HSZ8SVVb7HTbfK4XBmtCllknk8m21uuUijIhU+9cr+3EZddcfc4YznGapsd0P4y80Q19263C1kUwwk8jk22t1dKpDUK372xJWSiYZSlKVzq9yka05Bz0XYAPlutoddCMNKkCeSELdXRKPmIx/Z73/H8daafbt2uFCYzjOM84yUPhy8bXQqKOOLkiuSEKcXRqOmHR0Vs1+FNMyMcbEmeZCFuLoFERHL6SwVuNsYdkqsjXCPIlKlqotHTFNdMQMwezbNdkRuPEhC3FUiiJiW3HGv46f8/Lfr0STSUKSE/zxjOc0GlKAT1dhrEVZWbDWZKuEcfz8odGwOnrTgApIS1a+Nh8/j7xjOc0ahpH7G468bPwUKSERjGc5olC/qz2VzZp7rNOjqeNNer/AP/aAAgBAwABBQDqsqS052dxuQleYePNIMqmx1IywQwU12Fwt41cGKJfNJ+oC0Steer1kjLEx11xtrFcEJJIMI4DFEhvVvZSFJaeafa6u424evClFEmv8hhnzH6lDkwcL1Vvtw1aFLKfOJ5Djvlv0+nj10fqrXaxq6EccVJFchx3inqbTWK+lz/b3TZMGNQy80Q14rdaB68CYYTIE8hx3inqhTx68z71ktUbXB5SXPmCa3bpGvOwlgjp4bwWu1h1wc48qSK5NtuPOUim5gmfettxCrzZppciR9R8kbFE1W+ATLfK2WkSuCnHFSRXJttbq6XSm4hv3rdfWIlLzzpDvBKspzVdiuDJZeaIa+7VaRa2GccVJFcm23HnKZSEQ6PevsvJxMOpSlq51i3n1xyFnI+eF+WyzDVwE44qSK5NtrdXRaOmMR/Hvfn4602+3btdqYSpOU55xcvIQxTW0RFRMhImShfJCFOKo9KaiE9FaKGDMokI4yKL8yELcXRqKiO6Wcr8bPCWOqyNdf8AIlKlqpFITFsdMSMwWza9fPx/jQhTiqRR24tLjjWeo/Py26+GksFCEhP80pyrNEpiQUdXYa1G2Juw1mRrhHHGM5zR6TgbHWmgiyA9p1+XEYzjOM/WMZVmk0VDKewt2vmTckDPiPYxlWaRRf6ezujNSfRVI+miyvq//9oACAEBAAEFAOqddaHe7Pbe342ggmTcufK6v/YZ4bIEgDJidht3bwdAEkpEyXP+qLsqz0Ayg7JruwI/rtvbVC17HSMidLHcAJA6LL1x+xiM4ELGOG6vbm2A9fxsjJHy5nKOjjZY3U9OOo1O6rbW2QdeASkmdMyHIAAyUM1DqMKhAdVtTacdr+JmJiRnpLkAAXJmag1DG0QYlxlx73Z60V6sMhmCyA3i2xtUDX0XLS0jOSPKPjjZUzUmoQqEH72x9owmvI6z2yeuEjrrbFg18XS75Xb3HeDae1YzXsdNTUlYZPkOO+W/pXUaaUP722NwxdCFl5iUnj/qAsUzWJDV+8oi4N8tq7Qj9eRMxMSU/I8mGHintN6YZqDPvbe3kNWMllknE8ELW2vVn7BERmBCxTxvvaG0YrXkbNzkpY5LkOO+U/pzSw9TZ97fFqslXqTji3V89a7bsGvCKbdq9d4v5tHZ0bryHmpqTsMnyYYeJe0npfFXZcf/AMmfdbcW1kkYcwfan6/Ox6VoW2vnW7ROVKTF/ZyNXVp6elrLKcmWXSHdL6ZaqjfRbS0bD29iwV+Xq8r5mWXiXtI6NbgldJd6DX77GbC1hYdfG+Rttbq9I6WxXWOmkY4CXC2loOQr2fE00485pjTA9bYKKYUz0+cYzjaeho2y4k4qShTeaELcXozT+YRvq77rWtbACv2t7Dr0/ilKlK0jpXAOOtmIaLnwNm6Gl6p8zjKc/SUqWrS+kkAJ7DbGhBZr4fHnRRaUqWrS2kv+lnstxCamNY1TAajjrX8YWyn1P//aAAgBAgIGPwDorGZfm/DUV+ytelHsLNQ+8kZ6JXU9eoGvZw99NcO12zVL18RPOo4Xbv8AIZCYMAswHiGnj5j82Yxrlwk9tMskLDAqQQfSOkVsrKjb0w7Qg+JvwGvZxNdXMheeRizE4kk/SJLG4PyCQWjJPcb0aHPEffwXguflSKPHExHeHWBquGf29HpaWVDu7j0IPiP8I1zy4knnkLzMaknEk+SlxazNHOpqGUkEHtHCWn+SVBwAnUejxj+Idprwk0EgaJhUEYgjowWdoO9vDj0IPiPX8I9OXDTXErPKxxJNT5cNraxM9xIwVVAqSTgABqTxa2F7cLLMjlyoxCFqeGtaGlMSMK1pUYnooW9uvf3txgNEHxN+A17OJrq5kLzyMSxOJJP6Ychh5cVtbRM87sAqqCSScAABiSeBd3fdfeHGeYjBGKqedKhmBxFQMKluivkw0fc3HhXl+ZuoffxNeXkpe4c1JPlxW1tEzzuwVVAJJJNAABiSTgAOP7q8Ifd3GeYjBGKr15hmH7IwqW/lZ/Xkfc7pIkY0HeNKnhJ4JFeFhUEGoI6j5sRwENvL5Cvsj4j2aDXs4lurqUvcOasxzJ8uK1tYWkuHNFVQSSTkABiT1DE6cf3V13ZN5cZ4ERgjFVPPMMw08IwqW+vdwzfNv2HhQUqOs8gPv04a83O7eWc6k4AcgNB+h4/lky2J9qJie6esciNKdmRNf7qyuC4VQXjb2kJ58xXUYHDLLzJhim+ZvLDBRkvW36tfXLd3cpedzUk+WkUSFpGNABiSeQHA3DcVH/6kqlShA/lIc9MHYYEjEKSNSPr/APa2oEm+MOeCDm3XyFMeziS6u5mkuHNSxzP0x3u3XLxXKnAqaeg8wciOXC2m4stvuYoFTKOSgzByDHPu4dXlpHBRt3cYDl1sOXLiW7u5S9w5JJOZJ8tIokLSMaADEknQcDctzUNvBHgTMR9f7fq7cvr021bSA+5ioaSuEeGlM2H3cPNM5aVjUk4knyQQceI7HfgZbfALJ7yjQNzAyrmBnlwk0EgeJhUEGoI8gRoO9vDg90cvzN1cS3d3KXnc1JPlpHGhaRjQAYknhdx3NQ28mhRDiI9a/t/u555fXoDtUBRJWKu4BJUUzrkK5VPo4LMSWJqScyfMfLX+dtrHxRMT3esr8J+4nPj+829/GMGU+0p5Eeo5H6KDxby4PdXl+Y9Q/wBOJry8maS4c1LHP9Ory1jjUs5NABmTwm8bkldyAxjI/pKaGpr/AMn7vb7OX194Zow0TChBFQR18PuP+PEywYloveXUleajOmY0y4IIx8xHe7dcNHcKcxr1Eag8O0m3Ab7kGHsH83V2cS3l7M0lw5xJ9XUBy8tURSXJoAMyTkOE3fcwDuxFUjP/ABg6n8/7vb7PQVxfbWFi3cGpAwV+2mR6/t4msdwt2jukOIPrHMHQjz6xxqWdjQAZk8W257xHTcSQ0daFYuTHnJqPh/a9noQW16O7cj2ZAB3l7DqOo4cMs6/MsiaJKvst2/Ceo+jzqoiksTQAa8JvO6W4k3AGvyiP6S6Fq++eXua+L2ehpLe5iV4XFCCKj/SmhzGnE247MDJt4OMeJkjH8S9eY5ebVEUlyaADMnhN33eIHdMGjjI/p4YMwPv8h7uftU7oij6IocuJNy2kC33E1LRnBH7OTfdjxJbXcDx3CmhVgQR6D68j5gACpPEW8bvAP7uQfyUYYpqHxyb4eXtYEA9GBLy2KXig92RcGGGvMdR4WO9jrA/sSAeFuzkeo+Wm87whF2DWOM/8fKRx96g9pwz6Nayv4VeEjIivpHI9fE97t9ZdvDV7uJkjHJhqB8XZXHihz+kADHhN23uL/wC3gYoWAw5M4NfQPwz6Qud22NQl0DVogKK37HI9WRyGnEtreQPFcoaMrAhgRmCDiOAAKk8R7zvSD5tAY4iAc8QzV+4fbhn0lHFv12sV1j3HQEyjDDvqqsxXIgN3agEKw8XCSz/5BHczA0hQxSoC2hYyIEGGQLUrrQY/Vf/aAAgBAwIGPwDorGVfm8qiv2Z9KTW1kQ+7yjAaRj4m/hGFT1V4fcJrlzes1S9aNXKtRSmHLiOw/wAibvR4BZaYjTx8x+btrlik9tMrwsMCpBB+zpFLazKybhItVU4hBl33HqHvU5VPE93cyF55GLMTqT+mHIYfSrWkvetSwLRMSUah5e6c8R6a8Ce2lC3g9qMkd4ddNRyPR6wwqJN2lB7o0QfG32+EanHKvEt1dTNJcOasxNST+mQyAwGHkpc2k7RzqahlJBFMcxwln/kC0fACYDDkO+PWw5VPCTQyBomFQRiCOjFRUEm8TKe6uiDV2PMe6MyaaVPElzdztJO5qWY1Jr+HUMBp5cVtbRl53YBVGJJOQHEFjdyhripY0yXvU8Nca0pmMK1pUYnopYolEm7SjwLoo+N+oVwXNj1VInu7mQtPIxZieZ/AZAaDDy4re2iZ53IAUAkknAAAYngXNyFk3hxicCIwR7K546Mw6wMKluiiqsr3kg/lx/xNyA68+Jry8lL3DmpJ9Q6hp5cVvbxl5nYAAAkkk0AAGOJ4Se5o+7MMTQER1GKr10JDN/2jCpYV5fXklvr6OCNjQFzSpzpwk8EivCwqCDUH0+b9pZN0kFESuX5mwNABiARjlUV4mvLyYyXMhqzGlSctKDIaeXHb28ZeZjQACpJ7Bj+mHAuroK+7OM6A/LBGKqccc+82o8IwqW+vEznv3rDwRqRXtPJRma55DHg3N/OXfGgyVQdFGg05mgqTx3Vcy2J9qMk05VHIjTL7CaiexmBcDxIfaU8iPxHVll5kqtJLuRT8uPl+ZuQH38S3l7MXuHNST6hyA0HlpFEhaRjQAYknq4O4bhb97dJFwU0PyweXJyMCa1AqNSPr5hgCy7sw8K1wQfE/LqXXsrxJd3s7SXDZsc/1AdQw+lLuwuGjmXUa9RGoOR6q8Ja3xWDdRgATRZKDNTkCcyv2eWe7SS6kB+WlR/5MMwo7P9Zr28mL3EjEkn1DkBoPLSONC0jGgAxJPC7huKht0YeFcCIx/u9Xbl9el2zbAJNyIIZ6+GKo05vkaaa6cSTzyF5mNSTiSfJDKSGBqCNOIdu3zxwVAWUU7yjQPlUDKuYGeXCTQSB4mFQQagjyKSAPucgPcSuX5m5AH7fXLeXkpedziT6hyA0HlpFEhaRjQAYknhNw3SMNuTAFVNCI/wD29Xb7P14HbEPelYq7gElFocajAVyqeGd2JcmpJxJJzJPmPlgfN29j4oycq5lD7p15E568LdWEwYZMp9pTqCM+w5HMfQWdg25SA/LTM/tNyUfflxNe3sxe4c1JPqHIDQeWscaFpGNABmTwm67mtdxZR3UIwjBoamvv/u465fX3imQNEwoQcQRxLumwxMYKktEaVUalOajPunEaYCnBVgQwNCDp5hLzbrho5gRXkwGjDIj9eFOJZprdl3sCiqKlGNPb71MAMypx0FeJb2+mL3DnM6DQDkBoPx8tURSXJoANSeE3PdVB3JhVU/8AjB5/n/d7fZ6Ckn29Ug3YY8kc8mpkToR9nEllfQGO4XQ6jQg6g6Eevz6xxqS5NABmTxHuW7RBtyzRDQiPkTX39fy/tez0I1tfReOnhkAHfQ8wfWDgfTwROnfsifDIMj1H4W6j6POhVFWOQ4Td91iB3JhVEI/pDQmuT9Xu6+L2ehpLe5iV4WFCCKg/podNOJb/AGRTLt4FSmJdOzMsPvHp82qIpLk4AZ8R7nvMIO4GhRCP6fIn8/Ie7mfFQL8qPoihy4kvtrjEG40qVyR/9rdYwxxyHD213A0c6nFWFD/05EYHTzAVQSxNABrwu77vEP7tgDGjCvc1Dn83LlgcwD0YIr+Lu3Sg92RfaWo+8dRww4Ed2vet2PgkA8LU9R6j+vygAMeIt43eL+fnHGdOTN+A/DPo2S1vIFkgYYggH0jkeRGPEt/tjGbba1pj30B0Ix7wGPi7AanggjH6QqipPEe67xEDOcY4yAQOTMMceQp92fSEt/s4Ed7SpjHsv+z8J6sjlTLiS3uoWjnU0KsKEHgKoqx4i3feIwZCKxxEVAr7zV15D7cM+kgm+3aQXujKC0gNBTvhFZiuRANKgEKwHe4WST/II7qetIkaKWMV5sXQJWmQ72epAx+q/wD/2gAIAQEBBj8A+VTxr0lDIpNaSx6tIWsWHUo7Rbr9wOo5enzSm27f2dy8szIZU8fFeheO0LkyrL9yylQgqyNQPjqCDVl1PtB4tvmVuOXTeMi/1NdxFWllG/SF7i1j2zMhAAOnQADQcuIbH55U2xv0Y4e+LGbVkwZVmd216aXlIejB0QD82mnVxHN27Mxc3EyB3sfKxKJVL6aajqQ8mGvMHQj0OnzH9u2swzPJ8yVTj43UaS29XbttmZvSymmPyJgNQ1iNeSgnjL3PcLvk5udemTk2ckl6UbU6ak9KIPaqjkqgAcgP4i2z5XewKuhzdoy2emBlTVtXAUMGxqupI7kyDz59QGnC5O23nDcYqWzNnybznm4tQOVFxpscnJjUg9NpE6aaMAfl64mCVzPJdznkDCxhWYXb5aqi7lkCIJhJEfXGGoezcyOgE8ZW5bllVzM7Mq18nJu3VStG/E+iqqgAKoAVVAAAAA+GOdt2VfCzMd1pHJxqNKs3UhgQykEjUcweRHI8uMbafPAVcCOPHfoIO281ZUmm5Y/S6oFU+6qAdWmrEczxHMw7yycXIQUjeLh50RvRlYcj/I/iDyPP5Z9Fi9OZ5LuUKnBxkc9GItG6K7jmXXVTMqT7fz5FdNQEBPF9w3PLvnZuS5pfJyaNSjsTrpqeSovoqjRVHIADl8eNt2349MrNzKpDHx5KXpWtGCoiKNSzuxAAGpJIA1JA4w9n3PL+rz2vTcrgMzQwKXgjzxIWejdxUCdssqymrDrm7TKUr8qGJiKmb5NuEbfQYZPTPDkT0y3HcZgFZykp1lL81W/kmrDM3TcsimVnZ13yMm9XZ3d3PoCxYic1AVF9FUADkB8ePgYGPXKzMqqRx8eE2rWtaMERERAzszMwAABJJ0HPie57msczyrMkDevTJk2ieQJL9JjGjNpkxlVjk0Ydp40CJ1BiafKnSNMfK8kyksm3bYKUDQZiAmZkzmrTOFjKdUFCO43IAjXjK3bdsquZn5lWre1WJJLEkIg9JyQHRVHJRyHx4+BgY9crLy7Thjwij0pWtnWU0REDOzPRwAACSSAOZ4huW7lMvyPNVPqMhBG6bTC6TRp7cGVU+tVLOVs1lR3UTQOrHuO8I/TR/wCVLu97+n4ffo5O/wC74O1RyKdjHfMp2GyMhpiqz7jRvUXhM6u5XpUcRzMHIjl4mQgpDIx6LWNUJI6kdCVOhBB/kRoef2dIYzRzPJM6Spt+3tVg8EOnRuOdNVcRjBG6poedWAHIc+Mvdt2yqZu451O7lZVQgetAqoCVmqTUKiAAKAAAAB8cNv2/HrlZeTRZRhFGpSjuQqqqIGdmZiAAASSQACSBxPc9xSWZ5VlBFezzj/6hLTjOmJhGrNraorRHswEqIehOtWJp9+Zsmk8ve7LYbbtMKg5VK1BAycyR1mNvkeZZyC/ooJ4bc9/3CubfVxCZPTjYcnbq7GHjg9EJLyHLVmAHUSefA7DvuW0UKjJ2nKqz4zBdArTRw6K0tAyaBSrD2surdU8/ZspKXQTpl7e3cbO2+thbuRyUoFk6vkugjTqqZqA1XIP2JlMzzd/y5N+27d3D+kV0WeXkooK4+HDq1VD7qH05cZW77vlUy87LoXrVzyA9ElJfScZLyVRyA+OOLixpkZF6LKMIo1K1o50VERQWZieP3/fZBvJs2InPGpJaLtMqTuzzkKxHb3Fp9JeqtpKJOhfrdE+/Hb9veW5eUZEaCGIlA0duSx6Wyc6qk9UVXnBR76kAnRNTxfdN4zb7hn5B1rk5DdTHT0RFULOUl19qKFVfwA/jLdNjz74GZIj9SLELRPRpVT8tJUUlWB9VJH48Q2ve6Y+zeSTXpVK1mmLuzImpyMbJsqrHJYKWaalH/wBzq9PjCoZZW/5szLbdvUglTIhqZmYerqnhNbocDTVyunpxlbtu2VTMzsyr1tajE82Yt0TXXSck6tFUaAD45Y+NKl72dZxjJS9KUc6KiKoJZieJ+Q+SSjkeSWRDj4jhbR2lHI1VyOqbZBA6noC05zOrE6Oqffsnxzxdp5u/vG0MvcOvqx9lFgwZUUBlrngEEID0y9TqdBxfMzL1ycrJo1b5FnNK1ox1ZndiSSf7h8K0mzI6Mro6MVdHUgqysCCrKRqCOYPGJsHm7UzcEPKGJvYSRycJOrSX7h1KGycbGX2pzAVdNdNC3EczCyJZWLkItYZEHWkqow1DI6kgj/Z8BGqZW/ZmPkLtO2zZQyMSVjfNidVhhYwEmAIJoV0HGRu28Zlc3OyW1etWJCINe3GKkkShIHRUHID45Y2NKl8i7rOMZI1KUo50VERQWZieIeQ+RwlkeQ0WbxxbLJp7Sj6p3JpQh3y530TuKDOLgEkkhV+/Tfx6dEpuV64OZnylW9tv27s936sOnVLGTIfqVC40QOdNCeHrV3pSjtSlKMXejuSzu7sSzOzHUk8yfsBCTHctgrTqydoyHLTmWbqfIwg5M52B9xRgY0b8689eE3PYs1bcyMzGZzPI26iqpfHy8WgDgAn20A6HHMHT+FD3J33/ADo5M9q29HLuza9uOZVh/lo4ROrE83I6R68ZW77vlUzM/Mo1LWoeQ1JInNB7ZSTXRVGgA+OePjye17OElKal3d25BVUakniPlnkcDXfXx1tDBMRWexRo0TPIyKcxDcRd0QkHrxm9QXICxj2Zp2/qP1Jy/Xsf/L+/daPRH7Xa/T/r6/z9RxbFy4yyMa6NK0LItJVmw0KujAgg8ZPkHg065OH1WyczYTqaYkgS1KbYz+98aIBLSoetR+QsBpw06KyURmR0dSro6kqysrAFWUjQg8wfsI7tsOfXCypPNmCsTDJSbhuxlR1CXi3MEHmAToQefGTfM2y0vLJIJwwZa0wcvJeDROd9S0CJwfQPZXPd15J1Dnxk7xvWZXNzsptXrV2YTQE9uEVYkSx5A6Ko5Af11PxzhGb1tZ1nKaAs9KOQqIqjmWZjoOI+SeSxS3kdkR8PAZSV2iVZived+c3zVcBD0+7HfmQWIC/Ir7p49HG2jyQB6q4CY+37rogoqZjqzqL1kUVKdKMCfcGHGTs294dMLPxW0eb80rMk9vIx6rql8eoGqupIP9oIH208fHm9rWdZylNS70dzoqIo1JYk8Ye/+VRwv/oruLbdh5zyXF2JApeGXlVsO0uYzDq0HUUmG0Bfp6fklMHecZFsB0Ym5RUfWYFnVEnbGcaBS9ZjuGms6DUEc9eGGbF8vZrUK4G8yVexddTpPIWdK/S5S+jISRr6E/apKSNSlGCIiAszMx0CqBzJJ4j5V5Lh97fik8rEwnnKk9jkciMp0zEsFoNz67JpNAXm1Jjm50T5Nk7ZumJDMws2PZviXj34VgPywQ/mnWbc1YEMrDUEEcZW+eIzfcNiTv0ttgatty2+UHSb1mjddsjGJYt0uRkTUe4Ec/s0lJHpWjBUmilndj6BVAJJPGN5N5Rjzr5Cxnl7fhURX/ZVn1kUtKpDHdRdNERVLTYfjQ/4eOHhpWOPPSte5qbZd/8AU5YPL8f8v/2v/W+UEEAggggjUEHkQQeRBHGTvnjCy2rfOmppirLs7butJjUKyjliZbMBJaIO3R/zDXnxbbt2wcrbs7HYrXFy4vGq8yAwVwOub6aq66qw5gkfYKiKzu7KiIilnd2ICqqgEszE6ADmTwvlflWDFt0ukW2jbsuU6U2zUpkT3Dt97uLnNMaSDyKKzA6o4nQfK2TdsQS3CEcgYO7Y6vDKw6AMQiOrCGRNmILRsGXlqCp58Lj7tIXwMlmG37vjq30eZ0qrsmjavjZKK3uk+jcjpqBr8QVQWZiFVVBLMxOgAA5kk8Y3l/luL/i9FttW15CgriB+WPuGX0O45k9SqwB5DkQwD/LbbXu+Di7hhZCaVxsyXdCqAQiNJdKY+RLXVHQqyn0PGZvXj3XumwK/dbF6XO47dKhYKCh6nzMZHRx3F1KABXJfU8EEEEEggjQgjkQQeYIP8VRFZ3dgqIoLMzMdFVVGpZmJ0AHrxjeV+YYa1y2QX2vaMiC3jjhpO6ZebElhRkGjdDAEEDkwID/MMryDw2SYe6IjVytmSRTHzwEV0+jXX2ZJWi8hy6NOpVJ14vgbliXws3Gdp3xsmbStJ1JBVkYAjQjhURWd3YKqqCzMzHRVVRqSxJ0AHGN5Z5dBWyDEZG2bNkY4dYo8zWefkm6GHeVQrSRvbT09wID/ADIR803XH2bd1e/bzcPFvm70bJjSlE5WNhxre2LSnTVBUwQoDOBRGq6fVZXnm3+RZCv2dj27L2Hftq66tO4OZembg/t/XJkUpM2Vj1g8wrA8Hqia9Ue3DWhToyP9QvXp1U/4R7v6fdP/2Q==); background-repeat:no-repeat;background-position: center; background-size: contain;height:auto; border: none; text-align:center;padding-top:5px; background-color: #def0d8;">
											<span style="color:#214c70;">
												<b><xsl:value-of select="DLHDon/NDHDon/NBan/Ten" /></b>
												<br/>
					Ngày ký:
												<xsl:value-of select="substring( //*[local-name() = 'SigningTime'],9,2)"/>-
												<xsl:value-of select="substring( //*[local-name() = 'SigningTime'],6,2)"/>-
												<xsl:value-of select="substring( //*[local-name() = 'SigningTime'],0,5)"/>
											</span>
										</div>
									</td>
									<td width="16%" style="border: 1px solid #214c70;">
										Chỉ số đồng hồ
										<br/> Tháng này
									</td>
									<td width="16%" style="border: 1px solid #214c70;">
										Chỉ số đồng hồ
										<br/> Tháng trước
									</td>
									<td width="16%" style="border: 1px solid #214c70;">
										Số nước tiêu thụ (m3)
										
									</td>
									<td width="28%" style="text-align:left;border: 1px solid #214c70;padding-left:5px " colspan="2">
										Số hộ dùng chung
										
									</td>
								</tr>
								<!-- </table>
							<table style="line-height: 1.5;width:70%;text-align:center; font-size:9.5pt;border-top: 0px solid #214c70;border-bottom:0px solid #214c70;border-left:1px solid #214c70;border-right:1px solid #214c70;color:#214c70" > -->
								<tr style="height:28px;">
									<td width="16%" style="border: 1px solid #214c70;">
										<xsl:value-of select="$ChiSoDHThangNay"/>
									</td>
									<td width="16%" style="border: 1px solid #214c70;">
										<xsl:value-of select="$ChiSoDHThangTruoc"/>
									</td>
									<td width="16%" style="border: 1px solid #214c70;">
										<xsl:value-of select="$Tieuthu"/>
									</td>
									<td width="12%" style="border: 1px solid #214c70;text-align:center;" >Loại giá</td>
									<td width="16%" style="border: 1px solid #214c70;text-align:center;" >Thành tiền</td>
								</tr>
								<!-- </table>
							<table style="line-height: 1.5;width:70%;text-align:center; font-size:9.5pt;border-top: 0px solid #214c70;border-bottom:0px solid #214c70;border-left:1px solid #214c70;border-right:1px solid #214c70;color:#214c70; " > -->
								<xsl:for-each select="DLHDon/NDHDon/DSHHDVu/HHDVu">
									<tr style="height:28px;">
										<!-- <xsl:if test="ThTien!=0">
											<td style ="border-right: 1px solid #214c70;" >
												<xsl:choose>
																		<xsl:when test="TChat!=4">
																			<xsl:value-of select="STT" />
																		</xsl:when>
																		<xsl:otherwise></xsl:otherwise>
																	</xsl:choose>
											</td>
										</xsl:if> -->
										<!-- <xsl:if test="ThTien!=0"> -->
										<td width="16%" style="border: none;vertical-align: text-top;">
										<xsl:choose>
											<xsl:when test="STT=1"><xsl:value-of select="THHDVu" /></xsl:when>
											<xsl:otherwise></xsl:otherwise>
										</xsl:choose>	
										</td>
										<!-- </xsl:if> -->
										<!-- <xsl:if test="ThTien!=0">
											<td width="16%" style="border: none">
												<xsl:choose>
													<xsl:when test="DVTinh !='0'">
														<xsl:value-of select="DVTinh" />
													</xsl:when>
												</xsl:choose >
											</td>
										</xsl:if> -->
										<!-- <xsl:if test="ThTien!=0"> -->
										<td width="16%" style="border: none;vertical-align: text-top;">
											<xsl:choose>
												<xsl:when test="DVTinh!='0' and TChat!=4">
													<xsl:choose>
														<xsl:when test="SLuong &gt; 1">
															<xsl:value-of select="format-number(SLuong,'###.###.###,##','number')" />
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="format-number(SLuong,'###.###.###,##','number')" />
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
											</xsl:choose >
										</td>
										<!-- </xsl:if> -->
										<!-- <xsl:if test="ThTien!=0"> -->
										<td width="12%" style="border: none;vertical-align: text-top;" >
											<xsl:choose>
												<xsl:when test="DVTinh!='0'">
													<xsl:value-of select="format-number(DGia, '###.###.###,####','number')" />
												</xsl:when>
											</xsl:choose >
										</td>
										<!-- </xsl:if> -->
										<!-- <xsl:if test="ThTien!=0"> -->
										<td width="12%" style="border: none;vertical-align: text-top;" >
											
											</td>
										<!-- </xsl:if>
										<xsl:if test="ThTien!=0"> -->
										<td width="16%" style="vertical-align: text-top;;border: none;border-left: 1px solid #214c70;border-right:1px solid #214c70;">
											<!-- <xsl:if test="ThTien!=0"> -->
											<xsl:value-of select="format-number(ThTien, '###.###.###','number')" />
											<!-- </xsl:if> -->
										</td>
										<!-- </xsl:if> -->
									</tr>
								</xsl:for-each>
								<tr style="height:28px;">
									<td style="text-align:left;width:32%;border: 1px solid #214c70;padding-left:5px;line-height: 1.8" colspan="2">
										Thông báo:
										<br/>
										&#160;&#160;&#160;Đề nghị quý khách hàng thường xuyên
										<br/>&#160;&#160;&#160;kiểm tra mức tiêu thụ trên đồng hồ
									</td>
									<td style="text-align:left;width:28%;border: 1px solid #214c70;padding-left:5px;line-height: 1.8" colspan="2">
										Cộng tiền nước
										<br/>
										Phí BVMT
										<br/>
										Thuế giá trị gia tăng
										<xsl:for-each select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat">
											<xsl:value-of   select="TSuat"/>
										</xsl:for-each>
									</td>
									<td style="text-align:center;width:16%;border: 1px solid #214c70;line-height: 1.8" >
										<xsl:for-each select="DLHDon/NDHDon/TToan">
											<xsl:value-of   select="format-number(TgTCThue, '###.##0,#######','number')"/>
										</xsl:for-each>
										<br/>
										<xsl:for-each select="DLHDon/NDHDon/TToan/DSLPhi/LPhi">
											<xsl:value-of   select="format-number(TPhi, '###.##0,#######','number')"/>
										</xsl:for-each>
										<br/>
										<xsl:for-each select="DLHDon/NDHDon/TToan">
											<xsl:value-of   select="format-number(TgTThue, '###.##0,#######','number')"/>
										</xsl:for-each>
										<br/>
									</td>
								</tr>
								<!-- </table>
							<table style="width:100%;text-align:center; font-size:9.5pt;border-top: none;border-bottom:none;border-left:1px solid #214c70;border-right:1px solid #214c70;color:#214c70;" > -->
								<tr style="border-left:1px solid #214c70;border-right:1px solid #214c70;height:25px;" >
									<td style ="text-align:left;width:60%;border-bottom:1px solid #214c70;border-right:1px solid #214c70;border-top: 1px solid #214c70;padding-left:5px " colspan="4">
										<b>Tổng cộng tiền thanh toán:</b>
									</td>
									<td style ="text-align:center;width:16%;border-bottom:1px solid #214c70;border-top: 1px solid #214c70;border-right:1px solid #214c70;" >
										<xsl:value-of   select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###','vnd')"/>
									</td>
								</tr>
								<!-- </table>
							<table style="width:100%;text-align:center; font-size:9.5pt;border-top: 0px solid #214c70;border-left:1px solid #214c70;border-right:1px solid #214c70;color:#214c70;" > -->
								<tr style="border-left:1px solid #214c70;border-right:1px solid #214c70;color:#214c70;height:25px;" >
									<td style ="text-align:center;width:24%;border-bottom:1px solid #214c70;border-right:1px solid #214c70;color:#214c70;"></td>
									<td style ="text-align:left;width:76%;border-bottom:1px solid #214c70;border-top: 1px solid #214c70;padding-left:5px " colspan="6">Số tiền viết bằng chữ:
										<xsl:variable name="AmountWord" select="DLHDon/NDHDon/TToan/TgTTTBChu"/>
										<xsl:value-of select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))"/>
                  ./.
									</td>
								</tr>
							</table>
							<div style="width:100%;padding-top:5px;text-align:center;padding-bottom:0px;color:#214c70;">
								<i>
              Giải pháp hóa đơn điện tử được cung cấp bởi:
									<b> Công ty cổ phần công nghệ thẻ Nacencomm</b>. Mã số thuế:
									<b>0103930279</b>.
								</i>
							</div>
							<div style="width:100%;padding-top:0px;text-align:center;padding-bottom:20px;color:#214c70;">
								<span>
									<i>Tra cứu hóa đơn tại địa chỉ trang web: https://hoadon78.nacencomm.vn &#160;&#160;&#160;&#160;Mã tra cứu:
										<xsl:if test="DLHDon/TTChung/TTKhac/TTin/TTruong='MTCuu'">
											<b>
												<xsl:value-of select="DLHDon/TTChung/TTKhac/TTin/DLieu" />
											</b>
										</xsl:if>
									</i>
								</span>
							</div>
						</div>
					</div>
				</body>
			</page>
		</html>
	</xsl:template>
</xsl:stylesheet>