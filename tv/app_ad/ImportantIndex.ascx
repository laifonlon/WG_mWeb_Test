<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ImportantIndex.ascx.vb" Inherits="ImportantIndex" %>
<div style="background-color:#fff; margin-top:5px;">
    <div class="hd1"><h3>全球主要指數</h3></div>
    <div class="ct"><div class="ring">
    <table cellpadding="0" cellspacing="0" class="tbset"><tr class="ev"><th class="lft w30">名稱</th><th class="hl w25">指數</th><th class="w25">漲跌</th><th class="hl w20">比例</th></tr></table>
    <ul class="gblist">
        <asp:literal ID="a" runat="server"></asp:literal>
    </ul></div></div></div>

 <asp:SqlDataSource ID="sdsStock" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    SelectCommand="SELECT StockNo, Name, Deal, Change, Time, ShowTopOrder FROM Stock WHERE stockNo='WTX&' or stockNo='DJI' or stockNo='NAS' or stockNo='NKI' or stockNo='KOR' or stockNo='SHI' or stockNo='HSI' or stockNo='FTH' or stockNo='CAC' Order by ShowTopOrder" EnableCaching="True" CacheDuration="30">
</asp:SqlDataSource>