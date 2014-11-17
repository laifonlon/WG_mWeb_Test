<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Margin.ascx.vb" Inherits="Margin" %>

<div class="mg5"><div style="background-color:#fff;">
    <div class="hd1">
        <h3>資券變化 <span>MARGIN</span></h3></div>

    <div class="ct">
<asp:FormView ID="fv" runat="server" DataSourceID="sdsStock_marginadd" EnableViewState="False" Visible="False" Width="100%" EmptyDataText="無資券進出資料">
    <ItemTemplate>
        <div class="ring">
            <table cellpadding="0" cellspacing="0" class="tbset">
                <tr class="ev"><th class="lft">日期</th><th class="hl">融資變化</th><th>融資餘額</th><th class="hl">資券互抵</th></tr>
                <tr><td class="lft" rowspan="3"><%# Format(Eval("Date"),"MM/dd") %></td><td><%# Format(Eval("Diff1"),"0") %></td><td><span class="rd"><%# Format(Eval("Today1"),"0") %></span></td><td><span class="rd"><%# Format(Eval("Diff"),"0") %></span></td></tr>
                <tr class="ev"><th class="hl">融券變化</th><th>融券餘額</th><th class="hl">當沖率</th></tr>
                <tr><td><%# Format(Eval("today2")-Eval("before2"),"0") %></td><td><span class="rd"><%# Format(Eval("Today2"),"0") %></span></td><td><span class="rd"><%# Format((Eval("Diff")/Eval("Volume"))*1000,"0.00%") %></span></td></tr>
            </table>
        </div>
</ItemTemplate></asp:FormView>
<asp:sqldatasource id="sdsStock_marginadd" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT TOP (30) Finances.StockNo,  Finances.Date, Finances.Before1, Finances.Today1, Finances.Today1 - Finances.Before1 AS Diff1, Finances.Before2, Finances.Today2, 
                      Finances.Diff, HistoryPriceDaily.Volume, HistoryPriceDaily.[Close], ISNULL( HistoryPriceDaily.[ChangeRatio],'0') as ChangeRatio 
FROM Finances INNER JOIN HistoryPriceDaily ON Finances.StockNo = HistoryPriceDaily.StockNo AND Finances.Date = HistoryPriceDaily.Date
WHERE (Finances.StockNo = @StockNo)
ORDER BY Finances.Date DESC" 
    CacheDuration="3600" EnableCaching="True">
    <SelectParameters><asp:QueryStringParameter DefaultValue="2317" Name="StockNo" QueryStringField="StockNo" /></SelectParameters>
</asp:sqldatasource>

<asp:FormView ID="fv0000" runat="server" DataSourceID="sds0000_marginadd" EnableViewState="False" Visible="false"  Width="100%">
    <ItemTemplate>
        <div class="ring">
            <table cellpadding="0" cellspacing="0" class="tbset">
                <tr class="ev"><th class="lft">日期</th><th class="hl">融資變化(億)</th><th>融資餘額(億)</th></tr>
                <tr><td class="lft" rowspan="3"><%# Format(Eval("Date"),"MM/dd") %></td><td><%# Format(Eval("Diff1")/100000,"#,##0.0") %></td><td><span class="rd"><%# Format(Eval("Today1")/100000,"#,##0.0") %></span></td></tr>
                <tr class="ev"><th class="hl">融券變化(張)</th><th>融券餘額(張)</th></tr>
                <tr><td><%# Format(Eval("today2")-Eval("before2"),"#,##0") %></td><td><span class="rd"><%# Format(Eval("Today2"),"#,##0") %></span></td></tr>
            </table>
        </div>
</ItemTemplate></asp:FormView>
<asp:sqldatasource id="sds0000_marginadd" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Top(30) d1.Buy1, d1.Sell1, d1.Repay1, d1.Before1, d1.Today1, d1.Today1 - d1.Before1 AS Diff1,Finances.Date, Finances.Buy2, Finances.Sell2, Finances.Repay2, Finances.Before2, Finances.Today2, 
                      HistoryPriceDaily.[Close], HistoryPriceDaily.Volume, HistoryPriceDaily.ChangeRatio
FROM         Finances INNER JOIN
                          (SELECT     Date, Buy1, Sell1, Repay1, Before1, Today1
                            FROM          Finances AS Finances_1
                            WHERE      (StockNo = '0000A')) AS d1 ON Finances.Date = d1.Date INNER JOIN
                      HistoryPriceDaily ON Finances.StockNo = HistoryPriceDaily.StockNo AND Finances.Date = HistoryPriceDaily.Date
WHERE     (Finances.StockNo = '0000') Order by Finances.Date Desc" 
    CacheDuration="3600" EnableCaching="True">
<SelectParameters><asp:QueryStringParameter DefaultValue="2317" Name="StockNo" QueryStringField="StockNo" /></SelectParameters>
</asp:sqldatasource>

</div></div></div>