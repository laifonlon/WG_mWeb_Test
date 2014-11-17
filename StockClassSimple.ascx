<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StockClassSimple.ascx.vb" Inherits="Hottip_StockClassSimple" %>
<div class="mg5"><div style="background-color:#fff;">
    <div class="hd1"><h3>同類個股</h3></div>
    <div class="ct"><div class="ring">
        <%--<ul class="gblist"><asp:literal ID="a" runat="server"></asp:literal></ul>--%>
<asp:GridView ID="gvCollect" runat="server" autogeneratecolumns="False" BorderWidth="0px" datasourceid="sdsCollect" Width="100%" GridLines="None" CellPadding="0" EmptyDataText="無同類個股資料">
    <RowStyle CssClass="gvtable_row" ></RowStyle>
    <columns>
        <asp:templatefield headertext="代號" Visible="false"><itemtemplate>
                <asp:Label ID="lbNumber" runat="server" text='<%# Eval("StockNo") %>'></asp:Label>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="股票" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"><itemtemplate>
                <asp:HyperLink ID="hyStock" runat="server" NavigateUrl='<%# "/stock.aspx?no=" + Eval("StockNo").Replace("&","$") %>'  text='<%# Eval("Name") %>'></asp:HyperLink>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="股價" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"><itemtemplate>
                <asp:Label ID="lbPrice" runat="server" text='<%# Format(Eval("Deal"),"0.00") %>'></asp:Label>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="漲跌" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"><itemtemplate>
                <asp:Label ID="lbDiffence" runat="server" text='<%# Format(Eval("Change"),"0.00") %>'></asp:Label>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="%" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"><itemtemplate>
                &nbsp;<asp:Label ID="lbPercent" runat="server"></asp:Label>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="成交量" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="20%"><itemtemplate>
                <asp:Label ID="lbVolume" runat="server" text='<%# Format(Eval("TotalVolume")/1000,"#,###,###,##0") %>' ToolTip='<%# Format(Eval("TotalVolume"),"#,###,###,##0 股") %>'></asp:Label>
                </itemtemplate></asp:templatefield>
    </columns></asp:GridView>
    </div></div>
    <div class="hd1"><h3>相關個股</h3></div>
    <div class="ct"><div class="ring">
        <%--<ul class="gblist"><asp:literal ID="b" runat="server"></asp:literal></ul>--%>
<asp:GridView ID="gvRelate" runat="server" autogeneratecolumns="False" BorderWidth="0px" datasourceid="sdsRelate" Width="100%" GridLines="None" CellPadding="0" EmptyDataText="無相關個股資料">
    <RowStyle CssClass="gvtable_row" ></RowStyle>
    <columns>
        <asp:templatefield headertext="代號" Visible="false"><itemtemplate>
                <asp:Label ID="lbNumber" runat="server" text='<%# Eval("StockNo") %>'></asp:Label></itemtemplate></asp:templatefield>
        <asp:templatefield headertext="股票" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"><itemtemplate>
                <asp:HyperLink ID="hyStock" runat="server" NavigateUrl='<%# "/stock.aspx?no=" + Eval("StockNo").Replace("&","$") %>'  text='<%# Eval("Name") %>'></asp:HyperLink>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="股價" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"><itemtemplate>
                <asp:Label ID="lbPrice" runat="server" text='<%# Format(Eval("Deal"),"0.00") %>'></asp:Label>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="漲跌" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"><itemtemplate>
                <asp:Label ID="lbDiffence" runat="server" text='<%# Format(Eval("Change"),"0.00") %>'></asp:Label>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="%" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"><itemtemplate>
                &nbsp;<asp:Label ID="lbPercent" runat="server"></asp:Label>
                </itemtemplate></asp:templatefield>
        <asp:templatefield headertext="成交量" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%"><itemtemplate>
                <asp:Label ID="lbVolume" runat="server" text='<%# Format(Eval("TotalVolume")/1000,"#,###,###,##0") %>' ToolTip='<%# Format(Eval("TotalVolume"),"#,###,###,##0 股") %>'></asp:Label>
                </itemtemplate></asp:templatefield>
    </columns></asp:GridView>
    </div></div>
</div></div>

<asp:sqldatasource id="sdsCollect" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Top(10) Stock.Name, Stock.StockNo, Stock.Deal, Stock.Change,  Stock.TotalVolume FROM Stock INNER JOIN StockClass ON Stock.StockNo = StockClass.StockNo WHERE (StockClass.ClassId = @ClassId) ORDER BY Stock.Change DESC" 
    EnableCaching="True" CacheDuration="600">
    <SelectParameters><asp:Parameter DefaultValue="0" Name="ClassId" /></SelectParameters>
</asp:sqldatasource>

<asp:sqldatasource id="sdsRelate" runat="server" 
    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
    selectcommand="SELECT Top(10) Stock.Name, Stock.StockNo, Stock.Deal, Stock.Change,  Stock.TotalVolume From RelateStock Inner Join Stock ON RelateStock.RelatedNo=Stock.StockNo Where RelateStock.StockNo=@StockNo ORDER BY Stock.Change DESC" 
    EnableCaching="True" CacheDuration="600">
    <SelectParameters><asp:Parameter DefaultValue="2317" Name="StockNo" /></SelectParameters>
</asp:sqldatasource>