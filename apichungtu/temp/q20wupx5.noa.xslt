<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ex="http://exslt.org/dates-and-times" xmlns:fn="http://www.w3.org/2005/02/xpath-functions" xmlns:inv="http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1" extension-element-prefixes="ex">
  <xsl:output method="html" />
  <xsl:param name="imgLogo" />
  <xsl:param name="percent" select="''" />
  <xsl:decimal-format name="vnd" decimal-separator="," grouping-separator="." />
  <xsl:decimal-format name="usd" decimal-separator="." grouping-separator="," />
  <xsl:template match="HDon">
    <xsl:variable name="xetphi" select="count(DLHDon/NDHDon/DSHHDVu/HHDVu[DVTinh='0*1'])" />
    <xsl:variable name="digest" select="//*[local-name() = 'DigestValue']" />
    <xsl:variable name="TSuat" select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat" />
    <xsl:variable name="soHHdu" select="10-(count(DLHDon/NDHDon/DSHHDVu/HHDVu/STT))" />
    <xsl:variable name="somucthue" select="count(DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat/TSuat)" />
    <xsl:variable name="DVTTe" select="DLHDon/TTChung/DVTTe" />
    <xsl:variable name="TTCKTMai" select="DLHDon/NDHDon/TToan/TTCKTMai" />
    <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>E-Invoice</title>
        <meta HTTP-EQUIV="Content-Type" CONTENT="text/html; charset=utf-8" />
        <style type="text/css">
                    @media all {
                    .page-break {
                    display: none;
                    }
                    }
                    
                    @media print {
                    .page-break {
                    display: block;
                    page-break-before: auto;
                    page-break-after: always;
                    }
                    }
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
                    font-size:12pt;
                    }
                    <!-- @page {
                         size: A4;
                         margin: 20px;
                         width: 21cm;
                         min-height: 29.7cm;
                         } --><!-- @media print {
                         html, body {
                         width: 210mm;
                         height: 297mm;
                         zoom: 98%;
                         center;
                         }
                         } --></style>
      </head>
      <div size="A4" style="width:21cm;min-height:29.7cm;font-family:Times New Roman">
        <body style="font-family:Times New Roman">
          <div style="width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;background-image: url('https://ca2einv.nacencomm.vn/Upload/blank.png'); background-size:80%; background-position: center;width:900px;background-color: hsla(0,0%,100%,0.60);background-blend-mode: overlay;background-repeat:no-repeat;border:none;">
            <div id="background" style="display:none">
                            MẪU
                        </div>
            <div id="background" style="position:absolute;z-index:0;width:auto;height:100px;border:5px solid red;background:transparent;display:none;top:65%;left:25%;color:red;font-size:50pt;text-align:center;padding-top:10px;"> </div>
            <div style="border:2px solid black;width:100%;height: auto; min-height: 100%;background-image:url(paramVien);                                     border-color: white;                                     background-size: 100% 100%;                                     background-clip: padding-box;                                     box-sizing: border-box;                                     padding: 20px;                                     border: 20px solid transparent;                                     border-width:20px;z-index:1;border-color:white">
              <div id="header" style="display:flex;flex-direction:paramOpacityHeaderFlexDirection;padding-top:10px;padding-right:10px">
                <div id="header_left" style="width:200px;display:flex;justify-content:center">
                  <img style="height:100px;align-content:center;position:static;left:0;top:0;object-fit: scale-down;" id="imgSample" src="data:image/jpg;base64,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" />
                </div>
                <div id="header_center" style="flex:1;text-align:center">
                  <span style="font-weight:bold; font-size:15pt;text-transform: uppercase;">
                    <xsl:value-of select="DLHDon/TTChung/THDon" />
                  </span>
                  <br />
                  <span style="font-weight:bold; font-size:15pt">(VAT INVOICE)</span>
                  <br />
                  <span style="font-weight:normal;font-size:10.5pt;display:none"></span>
                </div>
                <div id="header_right">   
                                    Mẫu số
                                    <i>(Form)</i>:
                                    <b><xsl:value-of select="DLHDon/TTChung/KHMSHDon" /></b><br />
                                    Ký hiệu
                                    <i>(Serial No)</i>:
                                    <b><xsl:value-of select="DLHDon/TTChung/KHHDon" /></b><br />
                                    Số
                                    <i>(No)</i>:
                                    <span style="color: red;font-size:16pt"><!--  <xsl:value-of select="substring(
                                                concat('00000000', DLHDon/TTChung/SHDon), 
                                                string-length(DLHDon/TTChung/SHDon) + 1, 
                                                8
                                            )"/> --><xsl:value-of select="DLHDon/TTChung/SHDon" /></span><br /><div style="color: red; font-size: 10pt;padding-top:5px;  display: normal;text-align:center"><div style="display:none">
                                            HOÁ ĐƠN CHUYỂN ĐỔI
                                            <br />
                                            TỪ HOÁ ĐƠN ĐIỆN TỬ
                                        </div></div></div>
              </div>
              <table>
                <tr>
                  <td style="padding-left:200px">
                    <b>
                      <xsl:if test="MCCQT !=''">
                                                MÃ CQT CẤP:
                                                <xsl:value-of select="MCCQT" /></xsl:if>
                    </b>
                  </td>
                </tr>
              </table>
              <hr style="background-color:black;width:100%;height:1px;margin-bottom:1px" />
              <table style="width:100%;line-height:25px;font-size:12pt">
                <tr style="ten_cong_ty_css_display;">
                  <td style="padding-left:10px;width:250px">
                                        Đơn vị bán hàng
                                        <i>(Seller)</i>:
                                    </td>
                  <td style="padding-left:0px;ten_cong_ty_css;" colspan="2">
                    <!-- <div style="font-weight:bold; font-size:12pt;text-transform: uppercase;ten_cong_ty_css;"> -->
                    <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" />
                    <!-- </div> -->
                    <!-- <br/> -->
                  </td>
                </tr>
                <tr style="mst_css_display;">
                  <td style="padding-left:10px;width:150px">                    
                                        Mã số thuế
                                        <i>(Tax code)</i>:
                                    </td>
                  <td style="width:75%;mst_css;">
                    <!-- <div style="font-weight:bold; font-size:12pt;mst_css;"> -->
                    <!-- <du> -->
                    <xsl:value-of select="DLHDon/NDHDon/NBan/MST" />
                    <!-- </du> -->
                    <!-- </div> -->
                  </td>
                </tr>
                <tr style="dia_chi_css_display;">
                  <td style="padding-left:10px;width:150px">
                                        Địa chỉ
                                        <i>(Address)</i>:
                                    </td>
                  <td style="dia_chi_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NBan/DChi" />
                  </td>
                </tr>
                <tr style="so_tai_khoan_css_display;">
                  <td style="padding-left:10px;">
                                        Số tài khoản
                                        <i>(Account No)</i>:
                                    </td>
                  <td style="so_tai_khoan_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NBan/STKNHang" /> 
                                        Tại:
                                        <xsl:value-of select="DLHDon/NDHDon/NBan/TNHang" /></td>
                </tr>
                <tr style="dien_thoai_css_display;">
                  <td style="padding-left:10px;">
                                        Điện thoại
                                        <i>(Tel)</i>:
                                    </td>
                  <td style="dien_thoai_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NBan/SDThoai" />
                                                     
                                        <xsl:choose><xsl:when test="DLHDon/NDHDon/NBan/Fax!=''"> Fax:
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/Fax" /></xsl:when><xsl:otherwise></xsl:otherwise></xsl:choose><xsl:choose><xsl:when test="DLHDon/NDHDon/NBan/Website!=''">
                                                                  
                                                Website:
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/Website" /></xsl:when><xsl:otherwise></xsl:otherwise></xsl:choose></td>
                </tr>
              </table>
              <hr style="background-color:black;width:100%;height:0.5px;margin-bottom:1px;margin-top:1px" />
              <table style="width:100%;line-height:25px;font-size:12pt">
                <tr style="ho_ten_nguoi_mua_css_display;">
                  <td style="padding-left:10px;width:40%">
                                        Họ tên người mua hàng
                                        <i>(Customer Name)</i>:
                                    </td>
                  <td style="width:60%;ho_ten_nguoi_mua_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NMua/HVTNMHang" />
                  </td>
                </tr>
                <tr style="don_vi_mua_hang_css_display;">
                  <td style="padding-left:10px;">
                                        Tên đơn vị
                                        <i>(Company's)</i>:
                                    </td>
                  <td style="don_vi_mua_hang_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NMua/Ten" />
                  </td>
                </tr>
                <tr style="mst_nguoi_mua_css_display;">
                  <td style="padding-left:10px;">
                                        Mã số thuế
                                        <i>(Tax code)</i>:
                                    </td>
                  <td style="mst_nguoi_mua_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NMua/MST" />
                  </td>
                </tr>
                <tr style="dia_chi_nguoi_mua_css_display;">
                  <td style="padding-left:10px;">
                                        Địa chỉ
                                        <i>(Address)</i>:
                                    </td>
                  <td style="dia_chi_nguoi_mua_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NMua/DChi" />
                  </td>
                </tr>
                <tr style="so_tai_khoan_nguoi_mua_css_display;">
                  <td style="padding-left:10px;">
                                        Số tài khoản
                                        <i>(Account No)</i>:
                                    </td>
                  <td style="so_tai_khoan_nguoi_mua_css;">
                    <xsl:value-of select="DLHDon/NDHDon/NMua/STKNHang" /> 
                                        Tại:
                                        <xsl:value-of select="DLHDon/NDHDon/NMua/TNHang" /></td>
                </tr>
                <tr>
                  <td style="padding-left:10px;">
                                        Hình thức thanh toán
                                        <i>(Payment Method)</i>:
                                    </td>
                  <td>
                    <xsl:value-of select="DLHDon/TTChung/HTTToan" />
                                                 
                                                 
                                        <xsl:choose><xsl:when test="DLHDon/NDHDon/NMua/SDThoai!=''">
                                                Số điện thoại
                                                <i>(Tel)</i>:        
                                                <xsl:value-of select="DLHDon/NDHDon/NMua/SDThoai" /></xsl:when><xsl:otherwise></xsl:otherwise></xsl:choose></td>
                </tr>
                <tr>
                  <td style="padding-left:10px;">Đồng tiền thanh toán
                                        <i>(Payment currency
                                            )</i>:
                                    </td>
                  <td>
                    <xsl:value-of select="DLHDon/TTChung/DVTTe" />    
                                        <xsl:if test="DLHDon/TTChung/TGia !=''">Tỷ giá:
                                            <xsl:value-of select="format-number(DLHDon/TTChung/TGia, '#.###','vnd')" /></xsl:if></td>
                </tr>
              </table>
              <xsl:choose>
                <xsl:when test="DLHDon/TTChung/TTHDLQuan!=''">
                  <div style="width:100%;font-weight:bold;text-align:center;font-size:11.5pt">Hóa đơn
                                        <xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=1">thay thế</xsl:if><xsl:if test="DLHDon/TTChung/TTHDLQuan/TCHDon=2">điều chỉnh</xsl:if> cho hóa đơn số
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/SHDCLQuan" />, mẫu số
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/KHMSHDCLQuan" />, ký hiệu
                                        <xsl:value-of select="DLHDon/TTChung/TTHDLQuan/KHHDCLQuan" />, ngày
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,9,2)" /> tháng
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,6,2)" /> năm
                                        <xsl:value-of select="substring(DLHDon/TTChung/TTHDLQuan/NLHDCLQuan,0,5)" /></div>
                </xsl:when>
                <xsl:otherwise></xsl:otherwise>
              </xsl:choose>
              <div style="background:url('paramWaterMarkTable;');background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;">
                <table style="width:100%;text-align:center; font-size:12pt;border-top: 1px solid black;border-bottom:1px solid black;border-left:1px solid black;border-right:none;">
                  <tr style="height:25px;">
                    <td width="5%" style="padding-top:1px;padding-bottom:1px;border: 1px solid black">
                      <span style="font-size: 12pt">
                        <b>STT
                                                    <br /><i>(No.)</i></b>
                      </span>
                    </td>
                    <td style="border: 1px solid black;" width="30%">
                      <span style="font-size: 12pt;">
                        <b>Tên hàng hóa, dịch vụ
                                                    <br /><i>(Name of goods and services)</i></b>
                      </span>
                    </td>
                    <xsl:choose>
                      <xsl:when test="//TTHHDTrung/TTin/LHHDTrung!=''">
                        <td style="border: 1px solid black;" width="15%">
                          <span style="font-size: 12pt;">
                            <b>Loại hàng hoá đặc trưng
                                                        <br /><i>(Typical goods)</i></b>
                          </span>
                        </td>
                      </xsl:when>
                      <xsl:otherwise></xsl:otherwise>
                    </xsl:choose>
                    <td width="5%" style="border: 1px solid black">
                      <span style="font-size: 12pt">
                        <b>ĐVT
                                                    <br /><i>(Unit)</i></b>
                      </span>
                    </td>
                    <td width="7%" style="border: 1px solid black">
                      <span style="font-size: 12pt">
                        <b>Số lượng
                                                    <br /><i>(Quantity)</i></b>
                      </span>
                    </td>
                    <td width="10%" style="border: 1px solid black">
                      <span style="font-size: 12pt">
                        <b>
                                                    Đơn giá
                                                    <br /><i>(Unit price
                                                        )</i><!--<br/> trước thuế<br/> GTGT--></b>
                      </span>
                    </td>
                    <xsl:if test="$TTCKTMai !=0">
                      <td width="10%" style="border: 1px solid black">
                        <span style="font-size: 12pt">
                          <b>
                     Chiết khấu
                      <br /><i>(Discount)</i></b>
                        </span>
                      </td>
                    </xsl:if>
                    <td width="13%" style="border:1px solid black">
                      <span style="font-size: 12pt">
                        <b>
                                                    Thành tiền
                                                    <br /><i>(Total amount)</i><!--<br/> trước thuế <br/>GTGT--></b>
                      </span>
                    </td>
                    <td width="7%" style="border: 1px solid black">
                      <span style="font-size: 12pt">
                        <b>
                                                    Thuế suất GTGT(%)
                                                    <br /><i>(VAT rate %)</i></b>
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
                    <xsl:choose>
                      <xsl:when test="//TTHHDTrung/TTin/LHHDTrung!=''">
                        <td style="border: 1px solid black;" width="15%">
                                                3
                                            </td>
                        <td width="5%" style="border: 1px solid black">
                                            4
                                        </td>
                        <td width="7%" style="border: 1px solid black">
                                            5
                                        </td>
                        <td width="10%" style="border: 1px solid black">
                                            6
                                        </td>
                        <xsl:choose>
                          <xsl:when test="$TTCKTMai !=0">
                            <td width="10%" style="border: 1px solid black">
                                            7
                                        </td>
                            <td width="13%" style="border:1px solid black">
                                            8=5x6-7
                                        </td>
                            <td width="7%" style="border: 1px solid black">
                                            9
                                        </td>
                          </xsl:when>
                          <xsl:otherwise>
                            <td width="13%" style="border:1px solid black">
                                            7=5x6
                                        </td>
                            <td width="7%" style="border: 1px solid black">
                                            8
                                        </td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <td width="5%" style="border: 1px solid black">
                                            3
                                        </td>
                        <td width="7%" style="border: 1px solid black">
                                            4
                                        </td>
                        <td width="10%" style="border: 1px solid black">
                                            5
                                        </td>
                        <xsl:choose>
                          <xsl:when test="$TTCKTMai !=0">
                            <td width="10%" style="border: 1px solid black">
                                            6
                                        </td>
                            <td width="13%" style="border:1px solid black">
                                            7=4x5-6
                                        </td>
                            <td width="7%" style="border: 1px solid black">
                                            8
                                        </td>
                          </xsl:when>
                          <xsl:otherwise>
                            <td width="13%" style="border:1px solid black">
                                            6=4x5
                                        </td>
                            <td width="7%" style="border: 1px solid black">
                                            7
                                        </td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </tr>
                  <xsl:variable name="lien" select="$0" />
                  <xsl:choose>
                    <xsl:when test="$lien='0'">
                      <xsl:choose>
                        <xsl:when test="count(DLHDon/NDHDon/DSHHDVu/HHDVu) &lt; 11">
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
                              <xsl:choose>
                                <xsl:when test="//TTHHDTrung/TTin/LHHDTrung!=''">
                                  <td style="width:15%;text-align:left;border-right:1px solid black;padding-left:3px">
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=1">
                                                                          
                                                                          Số Khung:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='SKhung'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Số máy:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='SMay'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=2 ">
                                                                                    
                                                                                    Biển kiểm soát:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='BKSPTVChuyen'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=3">
                                                                                         
                                                                          Tên người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='TNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Địa chỉ người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='DCNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          MST người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='MSTNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Số định danh người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='MDDNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                  </td>
                                </xsl:when>
                                <xsl:otherwise></xsl:otherwise>
                              </xsl:choose>
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
                              <xsl:if test="$TTCKTMai !=0">
                                <td width="10%" style="text-align:right;border-right:1px solid black">
                                  <xsl:if test="TLCKhau &gt; 0 and STCKhau=0">
                                    <xsl:value-of select="TLCKhau" />%</xsl:if>
                                  <xsl:if test="STCKhau &gt; 0 and TLCKhau=0">
                                    <xsl:value-of select="format-number(STCKhau, '#.###,##','vnd')" />
                                  </xsl:if>
                                  <xsl:if test="STCKhau &gt; 0 and TLCKhau &gt; 0">
                                    <xsl:value-of select="format-number(STCKhau, '#.###,##','vnd')" />
                                  </xsl:if>
                                </td>
                              </xsl:if>
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
                                                    <xsl:value-of select="$vat" />
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
                              <xsl:choose>
                                <xsl:when test="//TTHHDTrung/TTin/LHHDTrung!=''">
                                  <td style="width:15%;text-align:left;border-right:1px solid black;padding-left:3px">
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=1">
                                                                          
                                                                          Số Khung:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='SKhung'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Số máy:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='SMay'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=2 ">
                                                                                    
                                                                                    Biển kiểm soát:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='BKSPTVChuyen'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=3">
                                                                                         
                                                                          Tên người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='TNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Địa chỉ người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='DCNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          MST người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='MSTNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Số định danh người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='MDDNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                  </td>
                                </xsl:when>
                                <xsl:otherwise></xsl:otherwise>
                              </xsl:choose>
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
                              <xsl:if test="$TTCKTMai !=0">
                                <td width="10%" style="text-align:right;border-right:1px solid black">
                                  <xsl:if test="TLCKhau &gt; 0 and STCKhau=0">
                                    <xsl:value-of select="TLCKhau" />%</xsl:if>
                                  <xsl:if test="STCKhau &gt; 0 and TLCKhau=0">
                                    <xsl:value-of select="format-number(STCKhau, '#.###,##','vnd')" />
                                  </xsl:if>
                                  <xsl:if test="STCKhau &gt; 0 and TLCKhau &gt; 0">
                                    <xsl:value-of select="format-number(STCKhau, '#.###,##','vnd')" />
                                  </xsl:if>
                                </td>
                              </xsl:if>
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
                                                    <xsl:value-of select="$vat" />
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
                          <!-- <xsl:when test="floor(($line - 1) div 10) = ($lien - 1)"> -->
                          <xsl:when test="1=1">
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
                              <xsl:choose>
                                <xsl:when test="//TTHHDTrung/TTin/LHHDTrung!=''">
                                  <td style="width:15%;text-align:left;border-right:1px solid black;padding-left:3px">
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=1">
                                                                          
                                                                          Số Khung:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='SKhung'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Số máy:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='SMay'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=2 ">
                                                                                    
                                                                                    Biển kiểm soát:
                                                                            <br /><xsl:if test="TTHHDTrung/TTin/TTruong='BKSPTVChuyen'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="TTHHDTrung/TTin/LHHDTrung=3">
                                                                                         
                                                                          Tên người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='TNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Địa chỉ người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='DCNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          MST người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='MSTNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if><br />
                                                                          Số định danh người gửi hàng:
                                                                            <xsl:if test="TTHHDTrung/TTin/TTruong='MDDNGHang'"><xsl:value-of select="TTHHDTrung/TTin/DLieu" /></xsl:if></xsl:when>
                                      <xsl:otherwise></xsl:otherwise>
                                    </xsl:choose>
                                  </td>
                                </xsl:when>
                                <xsl:otherwise></xsl:otherwise>
                              </xsl:choose>
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
                              <xsl:if test="$TTCKTMai !=0">
                                <td width="10%" style="text-align:right;border-right:1px solid black">
                                  <xsl:if test="TLCKhau &gt; 0 and STCKhau=0">
                                    <xsl:value-of select="TLCKhau" />%</xsl:if>
                                  <xsl:if test="STCKhau &gt; 0 and TLCKhau=0">
                                    <xsl:value-of select="format-number(STCKhau, '#.###,##','vnd')" />
                                  </xsl:if>
                                  <xsl:if test="STCKhau &gt; 0 and TLCKhau &gt; 0">
                                    <xsl:value-of select="format-number(STCKhau, '#.###,##','vnd')" />
                                  </xsl:if>
                                </td>
                              </xsl:if>
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
                                                    <xsl:value-of select="$vat" />
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
                  <xsl:if test="DLHDon/NDHDon/TToan/DSLPhi/LPhi">
                    <table style="width:100%;text-align:left;border-left:1px solid black;border-right:1px solid black;font-size:12pt">
                      <xsl:for-each select="DLHDon/NDHDon/TToan/DSLPhi/LPhi">
                        <tr style="height:25px; border-bottom: 1px solid black">
                          <td style="width:20%;border-left: none!important; border-right: none"></td>
                          <td style="width:0%;border-left: none!important; border-right: none"></td>
                          <td style="width:15%;border-left: none!important; border-right: none;"></td>
                          <td style="width:10%;border-left: none!important; border-right: none"></td>
                          <td style="width:35%;border-left: none!important; border-right: none;text-align:right">
                            <xsl:value-of select="TLPhi" />
                          </td>
                          <td style="width:20%;border-right: none!important;border-left: none;text-align:right">
                            <xsl:choose>
                              <xsl:when test="TPhi!=''">
                                <xsl:value-of select="format-number(TPhi, '#.###.###,#######','vnd')" />
                              </xsl:when>
                              <xsl:otherwise></xsl:otherwise>
                            </xsl:choose>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </table>
                  </xsl:if>
                  <table style="width:100%;text-align:left;border-left:1px solid black;border-top:1px solid black;border-right:1px solid black;font-size:12pt ">
                    <tr style="border:1px solid black">
                      <td style="border-right:1px solid none;text-align:left">
                                                Tổng tiền chưa có thuế GTGT
                                                <i>(Total amount without VAT)</i>:
                                            </td>
                      <td style="border-right:1px solid black;text-align:right">
                        <xsl:choose>
                          <xsl:when test="DLHDon/NDHDon/TToan/TgTCThue!=''">
                            <xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTCThue, '#.###,#######','vnd')" />
                          </xsl:when>
                          <xsl:otherwise></xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                    <xsl:for-each select="DLHDon/NDHDon/TToan/THTTLTSuat/LTSuat">
                      <xsl:sort select="TSuat" />
                      <tr>
                        <td style="border-bottom: 1px solid black; border-left: 1px solid black">
                                                    Tổng thuế
                                                    <i>(Total tax)</i>:
                                                    <xsl:choose><xsl:when test="TSuat!='KHAC:7%'"><xsl:choose><xsl:when test="TSuat!='KHAC:3.5%'"><xsl:value-of select="TSuat" /></xsl:when><xsl:otherwise>5% x 70%</xsl:otherwise></xsl:choose></xsl:when><xsl:otherwise>10% x 70%</xsl:otherwise></xsl:choose></td>
                        <td style="border-bottom: 1px solid black; border-right: 1px solid black;text-align:right ">
                          <xsl:value-of select="format-number(TThue, '#.###,#######','vnd')" />
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
                            <xsl:value-of select="'\'" />
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:choose>
                              <xsl:when test="TSuat='0'">
                                <xsl:value-of select="'0'" />
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:choose>
                                  <xsl:when test="DLHDon/NDHDon/TToan/TgTThue!=''">
                                    <xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTThue, '#.###,#######','vnd')" />
                                  </xsl:when>
                                  <xsl:otherwise></xsl:otherwise>
                                </xsl:choose>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                    <xsl:choose>
                      <xsl:when test="DLHDon/NDHDon/TToan/TTCKTMai!='0'">
                        <tr style="border:1px solid black;font-weight:bold">
                          <td style="border-right:1px solid nonek;text-align:left">
                                               Tổng tiền chiết khấu thương mại
                                                <i>
                                                    (Total trade discount
                                                    )
                                                </i>:
                                            </td>
                          <td style="border-right:1px solid black;text-align:right">
                            <xsl:choose>
                              <xsl:when test="DLHDon/NDHDon/TToan/TTCKTMai!=''">
                                <xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TTCKTMai, '#.###,#######','vnd')" />
                              </xsl:when>
                              <xsl:otherwise></xsl:otherwise>
                            </xsl:choose>
                          </td>
                        </tr>
                      </xsl:when>
                      <xsl:otherwise></xsl:otherwise>
                    </xsl:choose>
                    <tr style="border:1px solid black;font-weight:bold">
                      <td style="border-right:1px solid nonek;text-align:left">
                                                Tổng cộng tiền thanh toán
                                                <i>
                                                    (Total payment
                                                    )
                                                </i>:
                                            </td>
                      <td style="border-right:1px solid black;text-align:right">
                        <xsl:choose>
                          <xsl:when test="DLHDon/NDHDon/TToan/TgTTTBSo!=''">
                            <xsl:value-of select="format-number(DLHDon/NDHDon/TToan/TgTTTBSo, '#.###,#######','vnd')" />
                          </xsl:when>
                          <xsl:otherwise></xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                  </table>
                  <table style="width:100%;text-align:left;border-left:1px solid black;font-size:12pt">
                    <tr style="height:25px;border-right:1px solid black">
                      <td width="100%" style="border-left:none!important; border-right:none!important; text-align:left;padding-left:3px" colspan="6">
                                                Số tiền viết bằng chữ
                                                <i>(In words)</i>:
                                                <xsl:variable name="AmountWord" select="DLHDon/NDHDon/TToan/TgTTTBChu" /><xsl:value-of select="concat(translate(substring($AmountWord, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($AmountWord,2,string-length($AmountWord)-1))" />
                                                ./.
                                            </td>
                    </tr>
                  </table>
                </div>
              </div>
              <table style="width: 100%;border-top:1px solid black;font-size:12pt;display:paramfooter" class="textfont">
                <tr>
                  <td style="border: none; padding-top: 1px; text-align: center;width:30%">
                                        Người mua hàng
                                        <i>(Buyer)</i><br />
                                        (Ký, ghi rõ họ tên)
                                        <br /><i>(Signature and full name)</i></td>
                  <td style="border: none; padding-top: 1px; text-align: center;width:40%">
                    <div style="width:100%;text-align:center;width:100%;text-align:center;display:none">
                                            Người chuyển đổi
                                            <i>(Converter)</i><br />
                                            (Ký, ghi rõ họ tên)
                                            <i><br />
                                                (Signature and full name)
                                            </i></div>
                  </td>
                  <td style="border: none; padding-top: 1px; text-align: center;width:30%">
                                        Người bán hàng
                                        <i>(Seller)</i><br />
                                        (Ký, ghi rõ họ tên)
                                        <br /><i>(Signature and full name)</i></td>
                </tr>
                <tr>
                  <td style="width: 30%"></td>
                  <td style="width: 40%"></td>
                  <td style="text-align:right;  height:80px; width: 30%;text-align:center;">
                    <div style="display:normal;background-image:url(data:image/jpeg;base64,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); background-repeat:no-repeat;background-position: left; background-size: contain;height:auto; border: 1px solid red; text-align:left;padding-top:5px; ">
                      <span style="color:red;">
                        <b> Signature valid</b>
                        <br />
                                                Được ký bởi:
                                                <xsl:value-of select="DLHDon/NDHDon/NBan/Ten" /><br />
                                                Ngày ký:
                                                <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],9,2)" />-
                                                <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],6,2)" />-
                                                <xsl:value-of select="substring( //*[local-name() = 'SigningTime'],0,5)" /></span>
                    </div>
                  </td>
                </tr>
                <tr>
                  <td colspan="3" style="text-align: center; border: none; padding-top: 1px;padding-bottom:15px">
                                        (Cần kiểm tra đối chiếu khi lập, giao, nhận hoá đơn)
                                    </td>
                </tr>
              </table>
            </div>
            <div style="width:100%;padding-top:15px;text-align:center;padding-bottom:5px;">
              <span style="font-size:12px;">
                                Chuỗi xác thực
                                <i>(Digest Value)</i>:
                                <b><xsl:value-of select="$digest" /></b></span>
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
                <i>Tra cứu hóa đơn tại địa chỉ trang web: https://ca2einv.nacencomm.vn </i>
                <xsl:if test="DLHDon/TTChung/TTKhac/TTin/TTruong='MTCuu'">    Mã tra cứu: <b><xsl:value-of select="DLHDon/TTChung/TTKhac/TTin/DLieu" /></b></xsl:if>
              </span>
            </div>
            <xsl:variable name="lien" select="0" />
            <xsl:choose>
              <xsl:when test="$lien &gt; 1">
                <div style="text-align:center;padding-top:0px">
                                    Tiep theo trang truoc -
                                    <span style="text-align:center;padding-top:3px"> Trang  </span></div>
              </xsl:when>
              <xsl:otherwise>
                <div style="text-align:center;padding-top:3px"> Trang  </div>
              </xsl:otherwise>
            </xsl:choose>
          </div>
        </body>
      </div>
    </html>
  </xsl:template>
</xsl:stylesheet>