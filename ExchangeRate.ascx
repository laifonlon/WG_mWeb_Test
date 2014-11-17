<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ExchangeRate.ascx.vb" Inherits="ExchangeRate" %>
 
<div style="background-color:#fff;">
    <div class="hd1">
    <div style="float: right;"><asp:HyperLink ID="Update" runat="server" NavigateUrl="globals.aspx?g=e"><asp:Label ID="lblElapseTime" runat="server" Text="0"></asp:Label></asp:HyperLink></div>
    <h3>國際匯率</h3></div>
    <div class="ct"><div class="ring">
    <table cellpadding="0" cellspacing="1px" class="tbset"><tr class="ev">
        <th>幣別</th>
        <th class="hl">現買</th>
        <th>現賣</th>
        <th class="hl">即買</th>
        <th>即賣</th>
        <th class="hl">時間</th></tr>
    <asp:literal ID="a" runat="server"/></table>
    </div></div>
</div>
 
<asp:SqlDataSource ID="sdsExchangeRate" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    SelectCommand="SELECT Name,SubName,CRIn,CROut,SERIn,SEROut,Time,GIndex FROM ExchangeRate Order by GIndex">
</asp:SqlDataSource>