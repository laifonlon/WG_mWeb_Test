<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StockViewr.ascx.vb" Inherits="StockViewr" %>
<asp:Panel ID="s" runat="server"  Visible="False">
<asp:Label ID="lblUpdateInterval" runat="server" Text="0"></asp:Label>
<asp:Label ID="lblMarket" runat="server" Text="1"></asp:Label>
<asp:Label ID="lblShowCount" runat="server" Text="20"></asp:Label>
<asp:Label ID="lblHideCount" runat="server" Text="0"></asp:Label>
<asp:Label ID="lblStartUpdateTime" runat="server" Text="0"></asp:Label>
<asp:Label ID="lblEndUpdateTIme" runat="server" Text="24"></asp:Label>
<asp:Label ID="lblShowAd" runat="server" Text="0"></asp:Label>
<asp:Label ID="lblLanguage" runat="server" Text="ch"></asp:Label>
<asp:Label ID="lblShowUpdate" runat="server" Text="true"></asp:Label>
</asp:Panel>
 
<asp:Panel ID="p" runat="server">
<%--<div class="subblock_title backtitle_color stockview">
    <div class="subtitle" style=" float: right;">
        
    </div>        

</div>--%>
 
<div style="background-color:#fff; margin-top:5px;">
    <div class="hd1">
        <div style="float: right; font-size:1.5em"><asp:HyperLink ID="Update" runat="server" NavigateUrl="globals.aspx"><asp:Label ID="lblElapseTime" runat="server" Text="0"></asp:Label></asp:HyperLink></div>
        <h3><asp:Literal ID="lblTitle" runat="server"></asp:Literal></h3>
    </div>
    <div class="ct"><div class="ring">
    <table cellpadding="0" cellspacing="0" class="tbset"><tr class="ev"><th class="lft w30">名稱</th><th class="hl w25">指數</th><th class="w25">漲跌</th><th class="hl w20">比例</th></tr></table>
    <ul class="gblist">
        <asp:literal ID="a" runat="server"></asp:literal>
    </ul></div></div></div>
</asp:Panel>

<asp:SqlDataSource ID="sdsStock" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    SelectCommand="IF @Market = 5 
        SELECT Top (@Count) StockNo, Name, EnglishName, EnglishFullName, Deal, Change, Time FROM Stock WHERE (Market = 5 or StockNo ='0000') and showorder <99 Order by ShowOrder
    ELSE
        SELECT Top (@Count) StockNo, Name, EnglishName, EnglishFullName, Deal, Change, Time FROM Stock WHERE Market = @Market and showorder <99 Order by ShowOrder">
    <SelectParameters>
        <asp:Parameter DefaultValue="20" Name="Count" DbType="Int32" />
        <asp:Parameter DefaultValue="1" Name="Market" DbType="Int32" />
    </SelectParameters>
</asp:SqlDataSource>