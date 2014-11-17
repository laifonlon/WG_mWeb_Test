<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Three.ascx.vb" Inherits="Three" %>

<div class="mg5"><div style="background-color:#fff;">
    <div class="hd1"><h3>三大法人</h3></div>
        <div class="ct"><div class="ring">
<%--        <ul class="gblist">--%>
        <table class="oul"><tr class="ev"><th class="lft">日期</th><th class="hl">外資</th><th>投信</th><th class="hl">自營</th><th>總和</th></tr></table>
<asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="False" GridLines="None" DataSourceID="sdsmain" BorderStyle="None" Width="100%" RowStyle-HorizontalAlign="Center" ShowHeader="false">
        <RowStyle CssClass="gvtable_row" ></RowStyle>
        <Columns>
        <asp:TemplateField>
        <ItemTemplate>
<table cellpadding="0" cellspacing="0" class="tbset" style="height:20px;"><tr>
<td class="w20 lft"><asp:label id="lblDate_main" runat="server" text='<%# Format(Eval("Date"),"MM/dd") %>'></asp:label></td>
<td class="w20"><asp:label id="lblSumForeign" runat="server" text='<%# Format(Eval("SumForeign")/1000,"#,###,###,##0") %>' Visible="false"></asp:label>
                              <asp:label id="lblSumForeigna" runat="server" text='<%# Format(Eval("SumForeign")/100000000,"#,###,###,###.#0") %>' Visible="false"></asp:label></td>
<td class="w20"><asp:label id="lblSumING" runat="server" text='<%# Format(Eval("SumING")/1000,"#,###,###,##0") %>' Visible="false"></asp:label>
                              <asp:label id="lblSumINGa" runat="server" text='<%# Format(Eval("SumING")/100000000,"#,###,###,###.#0") %>' Visible="false"></asp:label>   </td>
<td class="w20"><asp:label id="lblSumDealer" runat="server" text='<%# Format(Eval("SumDealer")/1000,"#,###,###,##0") %>' Visible="false"></asp:label>
                              <asp:label id="lblSumDealera" runat="server" text='<%# Format(Eval("SumDealer")/100000000,"#,###,###,###.#0") %>' Visible="false"></asp:label></td>
<td class="w20"><asp:label id="lblSum" runat="server" text='<%# Format((Eval("SumForeign")+Eval("SumING")+Eval("SumDealer"))/1000,"#,###,###,##0") %>' Visible="false"></asp:label>
                              <asp:label id="lblSuma" runat="server" text='<%# Format((Eval("SumForeign")+Eval("SumING")+Eval("SumDealer"))/100000000,"#,###,###,###.#0") %>' Visible="false"></asp:label></td>
</tr></table>    
        </ItemTemplate>
        </asp:TemplateField>
        </Columns></asp:GridView>
<%--        </ul>--%>
        <span class="clear"></span>
            </div></div></div></div>

<asp:sqldatasource id="sdsmain" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
        selectcommand="SELECT TOP (10) StockNo, Date, SumForeign, SumING, SumDealer FROM TWTU WHERE (StockNo = @StockNo) ORDER BY Date DESC" EnableCaching="True" CacheDuration="3600" EnableViewState="False">
    <SelectParameters><asp:QueryStringParameter DefaultValue="0000" Name="StockNo" QueryStringField="StockNo" /></SelectParameters>
</asp:sqldatasource>