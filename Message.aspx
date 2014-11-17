<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="Message.aspx.vb" Inherits="Message" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>玩股快訊</title>
</head>
<body><form id="form1" runat="server">

<asp:SqlDataSource ID="sdsOfficalPost" runat="server" CacheDuration="600" EnableCaching="true"
ConnectionString="<%$ ConnectionStrings:WantGooConnection %>" 
SelectCommand="SELECT TOP (1) Blog.MemberNo,Blog.ArticleID, Blog.BlogID, Blog.ArticleTitle, Blog.PublishTime 
                FROM  Blog WHERE (Blog.MemberNo = '356' or Blog.MemberNo = '59020') and Blog.BlogID<>72864 and Blog.IsCopy = 0 ORDER BY Blog.PublishTime DESC" EnableViewState="False">
</asp:SqlDataSource>
</form></body></html>

