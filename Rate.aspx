<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Rate.aspx.vb" Inherits="Rate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<meta name="viewport" content="width=device-width, initial-scale=1" />
<meta http-equiv="refresh" content="120">
<style>
body, div, dl, dt, dd, ul, ol, li, h1, h2, h3, h4, h5, h6 {
padding: 0;
margin: 0;
font-weight: 100;
list-style-type: none;
font-size: 1em;
-webkit-text-size-adjust: none;
font-family: helvetica,'Nokia Sans','Apple LiGothic','Lihei pro','微軟正黑體', Arial,Sans-serif;
}
body{ background-color:Black; color:White;}
.tb{width:100%;border-collapse: collapse;}
.tb tr{ border-bottom:1px solid #888888;line-height:30px;}
.name a{color:white; text-decoration:none;}
 
.sub {
background: #5A5A5A;
padding: 5px;
clear: left;
font-weight: bold;
font-size:1.1em;
border-bottom: 1px solid #007FFF;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
 <asp:Panel ID="s" runat="server"  Visible="False">
    <asp:Label ID="lblUpdateInterval" runat="server" Text="60"></asp:Label>
        <div style="float: right;"><asp:HyperLink ID="Update" runat="server" NavigateUrl="globals.aspx"></asp:HyperLink></div>
        <h3><asp:Literal ID="lblTitle" runat="server"></asp:Literal></h3>
</asp:Panel>

<div>
    <div class="sub">美元外匯報價<a href="rate.aspx" style="text-decoration:underline;color:#55AAFF; float:right;cursor: pointer;"><asp:Label ID="lblElapseTime" runat="server" Text="0"></asp:Label></a></div>
</div>
<asp:Label ID="a" runat="server" class="subblock_cont"></asp:Label>
<asp:SqlDataSource ID="sdsRate" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    SelectCommand="Select FromID, ToID, Date, Rate From Rate Order By [Index]">
</asp:SqlDataSource>
    </form>
</body>
</html>
