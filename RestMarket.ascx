<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RestMarket.ascx.vb" Inherits="RestMarket" %>
<div style="background-color:#fff; margin-top:5px;">
    <div class="hd1"><h3>全球休市預告</h3></div>
    <div class="ct"><div class="ring">
    <table cellpadding="0" cellspacing="0" class="tbset"><tr class="ev"><th class="lft w20">日期</th><th class="hl">國家</th></tr></table>
    <ul class="gblist">
        <asp:literal ID="a" runat="server"></asp:literal>
    </ul></div></div></div>
 
<asp:SqlDataSource ID="sdsRest" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    ProviderName="<%$ ConnectionStrings:twStocksConnectionString.ProviderName %>" 
    SelectCommand="SELECT DISTINCT Holiday,
                                (SELECT          Name + ','  
                                  FROM               GlobalClosedMarket AS s2
                                  WHERE           (s1.Holiday = Holiday) FOR XML PATH('')) AS Name
FROM              GlobalClosedMarket AS s1
WHERE          (Holiday BETWEEN DATEADD(day, -1, GETDATE()) AND DATEADD(day, 9, GETDATE())) AND (HolidayContent NOT LIKE N'%半日%') 
GROUP BY   Holiday, Name" CacheDuration="86400" 
    EnableCaching="True">
</asp:SqlDataSource>