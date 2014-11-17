<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TwIndexRelate.ascx.vb" Inherits="TwIndexRelate" %>

<div class="mg5">
    <div style="background-color:#fff;">
        <div class="hd1"><h3>相關指數 <span>RELATED INDEX</span></h3></div>

        <div class="ct">
        <table class="oul"><tr class="ev"><th class="lft">股市</th><th class="hl">指數</th><th>漲跌</th><th class="hl">比例</th></tr></table>
<asp:GridView ID="g" runat="server" CssClass="gvtable" BorderStyle="None" GridLines="None" DataSourceID="sdsStock" AutoGenerateColumns="False" Width="100%" ShowHeader="false">
<RowStyle CssClass="gvtable_row"></RowStyle>
<Columns>                   
<asp:templatefield><itemtemplate>
<table cellpadding="0" cellspacing="0" class="tbset" style="height:20px;"><tr>
<td class="w25 lft"><a href='<%# "index.aspx?no=" + Eval("StockNo").Replace("&","$") %>'><%# Eval("Name") %></a></td>
<td class="w25"><asp:literal id="lblPrice" runat="server" text='<%# Format(Eval("Deal"),"0.00") %>'/></td>
<td class="w25"><asp:literal id="lblChange" runat="server" text='<%# Format(Eval("Change"),"0.00") %>'/></td>
<td class="w25"><asp:literal id="lblPercent" runat="server"/></td>
</tr></table>
</itemtemplate></asp:templatefield>
<%--<asp:templatefield headertext="時間"><HeaderStyle CssClass="gvtable_right" /><ItemStyle CssClass="gvtable_right" /><itemtemplate><asp:literal id="lblTime" runat="server"  text='<%# Format(Eval("Time"),"HH:mm") %>'/></itemtemplate></asp:templatefield>     --%>
</Columns></asp:GridView>
 
        </div></div></div>

<asp:SqlDataSource ID="sdsStock" runat="server" 
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>" 
    SelectCommand="SELECT StockNo, Name, Deal, Change, Time, ShowTopOrder FROM Stock WHERE stockNo='S2TWZ1' or stockNo='WTX&' or stockNo='WMT&' or stockNo='WTE&' or stockNo='WTF&' or stockNo='TWO' or stockNo='0050' Order by ShowOrder" EnableCaching="True" CacheDuration="30">
</asp:SqlDataSource>