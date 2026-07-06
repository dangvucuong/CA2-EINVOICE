<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewChungTu.aspx.vb" Inherits="ViewChungTu" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Xem chứng từ</title>
</head>
<body>
<form id="form1" runat="server">
    <div style="margin:10px">
   <a target="_blank"
   href='DownloadPdf.aspx?q=<%=Request.QueryString("q")%>'
   style="
        padding:8px 14px;
        background:#1976d2;
        color:white;
        text-decoration:none;
        border-radius:4px;
        font-family:Arial;
   ">

    Download PDF

</a>

        <a target="_blank"
   href='DownloadXML.aspx?q=<%=Request.QueryString("q")%>'
   style="
        padding:8px 14px;
        background:#1976d2;
        color:white;
        text-decoration:none;
        border-radius:4px;
        font-family:Arial;
   ">

    Download XML

</a>
</div>
    <hr />
    <asp:Literal ID="ltrHtml" runat="server"></asp:Literal>
</form>
</body>
</html>