<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
 xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" encoding="UTF-8" indent="yes"/>
<xsl:template match="/">
<html>
<head>
<meta charset="UTF-8"/>
<style>
body{
    font-family: "Times New Roman";
    font-size:14px;
   
}
.header{
    text-align:center;
    font-weight:bold;
}
.title{
    text-align:center;
    font-weight:bold;
    font-size:18px;
    margin-top:20px;
}
.line{
    margin-top:10px;
}
table{
    width:100%;
    border-collapse:collapse;
    margin-top:20px;
}
table, th, td{
    border:1px solid black;
}
th{
    text-align:center;
    font-weight:bold;
}
td{
    padding:5px;
}
.sign{
    margin-top:40px;
    width:300px;
    margin-left:auto;
    margin-right:40px; /* lùi vào so với lề phải */
    text-align:center;
}
.signBox{
    border:2px solid red;
    color:red;
    padding:10px;
    width:230px;

    margin-top:15px;
    margin-left:30px; /* đẩy cả box vào trong */

    text-align:left; /* nội dung căn trái */
    font-size:13px;
    line-height:1.5;
}

.signCheck{
    color:green;
    font-weight:bold;
    margin-left:8px;
}
</style>
</head>
<body>
<div class="header">
    CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM<br/>
    Độc lập - Tự do - Hạnh phúc
</div>
<div class="title">
    THÔNG BÁO CHỨNG TỪ ĐIỆN TỬ ĐÃ LẬP SAI
</div>
<div style="text-align:center">
    Kính gửi:
</div>
<div class="line">
Tên tổ chức, cá nhân lập chứng từ:
<b>
<xsl:value-of select="//DLTBao/TNNT"/>
</b>
</div>
<div class="line">
Mã số thuế:
<b>
<xsl:value-of select="//DLTBao/MST"/>
</b>
</div>
<div class="line">
Tổ chức, cá nhân lập chứng từ thông báo về việc chứng từ điện tử đã lập sai như sau:
</div>
<table>
<tr>
<th>STT</th>
<th>Ký hiệu mẫu chứng từ</th>
<th>Ký hiệu chứng từ</th>
<th>Số chứng từ điện tử</th>
<th>Ngày lập chứng từ</th>
<th>Loại chứng từ điện tử</th>
<th>Thông báo/Giải trình</th>
<th>Lý do</th>
</tr>
<xsl:for-each select="//DSCTu/CTu">
<tr>
<td>
<xsl:value-of select="STT"/>
</td>
<td>
<xsl:value-of select="KHMSCTu"/>
</td>
<td>
<xsl:value-of select="KHCTu"/>
</td>
<td>
<xsl:value-of select="SCTu"/>
</td>
<td>

<xsl:variable name="ngayLap" select="NLap"/>

<xsl:value-of select="substring($ngayLap,9,2)"/>
<xsl:text>-</xsl:text>

<xsl:value-of select="substring($ngayLap,6,2)"/>
<xsl:text>-</xsl:text>

<xsl:value-of select="substring($ngayLap,1,4)"/>

</td>
<td>

<xsl:choose>

<xsl:when test="LCTDT='1'">
Chứng từ điện tử khấu trừ thuế TNCN theo Nghị định 70
</xsl:when>

<xsl:when test="LCTDT='2'">
Chứng từ điện tử khấu trừ thuế đối với hoạt động kinh doanh trên nền tảng TMĐT
</xsl:when>

<xsl:when test="LCTDT='3'">
Biên lai thu thuế, phí, lệ phí không in sẵn mệnh giá theo Nghị định 70
</xsl:when>

<xsl:when test="LCTDT='4'">
Biên lai thu thuế, phí, lệ phí in sẵn mệnh giá theo Nghị định 70
</xsl:when>

<xsl:when test="LCTDT='5'">
Biên lai thu thuế, phí, lệ phí của cơ quan thuế sử dụng khi thu của cá nhân
</xsl:when>

<xsl:when test="LCTDT='6'">
Biên lai thu thuế, phí, lệ phí đặt in, tự in, điện tử theo TT303/2016/TT-BTC
</xsl:when>

<xsl:when test="LCTDT='7'">
Chứng từ khấu trừ thuế TNCN theo Nghị định 123/2020/NĐ-CP
</xsl:when>

<xsl:otherwise>
<xsl:value-of select="LCTDT"/>
</xsl:otherwise>

</xsl:choose>
</td>

<td>
</td>

<td>
<xsl:value-of select="LDo"/>
</td>

</tr>
</xsl:for-each>
</table>
<div class="sign">

<xsl:variable name="ngay" select="//DLTBao/NTBao"/>

<xsl:text>Ngày </xsl:text>

<xsl:value-of select="substring($ngay,9,2)"/>

<xsl:text> tháng </xsl:text>

<xsl:value-of select="substring($ngay,6,2)"/>

<xsl:text> năm </xsl:text>

<xsl:value-of select="substring($ngay,1,4)"/>

<br/>

<b>
TỔ CHỨC, CÁ NHÂN LẬP CHỨNG TỪ
</b>
<br/>

(Chữ ký số tổ chức, cá nhân lập chứng từ)
<div class="signBox">

<div>
Signature valid
<span class="signCheck">✔</span>
</div>

<div>

Được ký bởi:
<xsl:value-of select="
substring-before(
substring-after(
//*[local-name()='X509SubjectName'],
'CN='
),
','
)
"/>

</div>

<div>

Ngày ký:

<xsl:variable name="signTime"
select="//*[local-name()='SigningTime']"/>

<xsl:value-of select="substring($signTime,9,2)"/>
<xsl:text>-</xsl:text>
<xsl:value-of select="substring($signTime,6,2)"/>
<xsl:text>-</xsl:text>
<xsl:value-of select="substring($signTime,1,4)"/>

</div>

</div>
</div>
</body>
</html>
</xsl:template>
</xsl:stylesheet>