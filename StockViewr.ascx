<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StockViewr.ascx.vb" Inherits="StockViewr" %>
<asp:Panel ID="s" runat="server"  Visible="False">
<asp:literal ID="lblUpdateInterval" runat="server" Text="0"/>
<asp:literal ID="lblMarket" runat="server" Text="1"/>
<asp:literal ID="lblShowCount" runat="server" Text="20"/>
<asp:literal ID="lblHideCount" runat="server" Text="0"/>
<asp:literal ID="lblStartUpdateTime" runat="server" Text="0"/>
<asp:literal ID="lblEndUpdateTIme" runat="server" Text="24"/>
<asp:literal ID="lblShowAd" runat="server" Text="0"/>
<asp:literal ID="lblLanguage" runat="server" Text="ch"/>
<asp:literal ID="lblShowUpdate" runat="server" Text="true"/>
</asp:Panel>
 
<asp:Panel ID="p" runat="server">

<div style="background-color:#fff; margin-top:5px;">
    <div class="hd1">
        <div style="float: right;"><asp:HyperLink ID="Update" runat="server" NavigateUrl="globals.aspx"><t id="ElapseTime" style="font-size:15px;">10</t> 秒</asp:HyperLink></div>
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