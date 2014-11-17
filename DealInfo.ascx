<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DealInfo.ascx.vb" Inherits="DealInfo" %>
<div class="hd1">
    <span class="dt" style="left:-80px;"><asp:Literal ID="lUpdateTime" runat="server"/></span>
    <span class="btns-rt"><asp:LinkButton ID="btnAddAll" runat="server" Text="+自選股" style="position: absolute;top: 175px;left: -70px;height: 18px;line-height: 18px;" /></span>
    <h3><asp:Literal ID="lHead" runat="server"/></h3>
</div>
<div class="ct">
<asp:FormView ID="fv" runat="server" EnableViewState="False" Width="100%">
<ItemTemplate>
<div>
    <table class="oul">
        <tr class="ev"><th class="lft">名稱</th><th class="hl lft">指數</th><th>漲跌</th><th class="hl">比例</th></tr>
        <tr><td class="lft"><asp:Label ID="lblHideName" runat="server" Text='<%# Eval("Name") %>'></asp:Label></td>
        <td class="lft"><asp:label ID="lblDeal" runat="server" Text='<%# Eval("Deal") %>'></asp:label></td>
        <td><asp:label ID="lblChange" runat="server" Text='<%# Eval("Change") %>'></asp:label></td>
        <td><asp:label ID="lblPercentage" runat="server" Text='<%# Format(Eval("Change")/(Eval("Deal")-Eval("Change")),"0.00%") %>'></asp:label></td></tr>
    </table>
</div>
<div>
    <table class="oul">
        <tr class="ev"><th class="lft">開盤價</th><th class="hl lft">最高價</th><th>最低價</th><th class="hl">昨收價</th> <th>成交量</th></tr>
        <tr><td class="lft"><asp:label ID="lblOpen" runat="server" Text='<%# Eval("Open") %>'></asp:label></td>
        <td class="lft"><asp:label ID="lblHigh" runat="server" Text='<%# Eval("High") %>'></asp:label></td>
        <td><asp:label ID="lblLow" runat="server" Text='<%# Eval("Low") %>'></asp:label></td>
        <td><asp:label ID="lblLast" runat="server" Text='<%# Eval("Deal")-Eval("Change") %>'></asp:label></td>
        <td><span class="gn"><asp:label ID="lblTotalVolume" runat="server" Text='<%# Eval("TotalVolume") %>'></asp:label></span></td></tr>   
    </table>
</div>
<asp:Label ID="lblStockNo" runat="server" Text='<%# Eval("StockNo") %>' Visible="False"></asp:Label>
<asp:Label ID="lblMarket" runat="server" Text='<%# Eval("Market") %>' Visible="False"></asp:Label>
<asp:Label ID="lblTime" runat="server" Text='<%# Eval("Time") %>' Visible="False"></asp:Label>
</ItemTemplate></asp:FormView>

<asp:FormView ID="fvTw" runat="server"  
        EnableViewState="False" Width="100%" Visible="False">
<ItemTemplate>
<%--<div>
    <table class="oul">
        <tr class="ev"><th class="lft">名稱</th><th class="hl lft">指數</th><th>漲跌</th><th class="hl">比例</th></tr>
        <tr><td class="lft"><asp:Label ID="lblHideName" runat="server" Text='<%# Eval("Name") %>'></asp:Label></td>
        <td class="lft"><asp:label ID="lblDeal" runat="server" Text='<%# Eval("Deal") %>'></asp:label></td>
        <td><asp:label ID="lblChange" runat="server" Text='<%# Eval("Change") %>'></asp:label></td>
        <td><asp:label ID="lblPercentage" runat="server" Text='<%# Format(Eval("Change")/(Eval("Deal")-Eval("Change")),"0.00%") %>'></asp:label></td></tr>
    </table>
</div>--%>
<div>
    <table class="oul">
        <tr class="ev"><th class="lft">名稱</th><th class="hl lft">代號</th></tr>
        <tr><td class="lft"><asp:Label ID="lblHideName" runat="server" Text='<%# Eval("Name") %>'></asp:Label></td>
        <td class="lft"><asp:Label ID="lblStockNo" runat="server" Text='<%# Eval("StockNo") %>'></asp:Label></td></tr>
    </table>
</div>

<div>
    <table class="oul">
        <tr class="ev"><th class="lft">指數</th><th class="hl lft">漲跌</th><th>比例</th><th class="hl">成交量</th></tr>
        <tr><td class="lft"><asp:Label ID="lblDeal" runat="server" Text='<%# Eval("Deal") %>'></asp:Label></td>
        <td class="lft"><asp:label ID="lblChange" runat="server" Text='<%# Eval("Change") %>'></asp:label></td>
        <td><asp:label ID="lblPercentage" runat="server" Text='<%# Format(Eval("Change")/(Eval("Deal")-Eval("Change")),"0.00%") %>'></asp:label></td>
        <td><asp:label ID="lblTotalVolume" runat="server" Text='<%# Eval("TotalVolume") %>'></asp:label></td></tr>
    </table>
</div>

<div>
    <table class="oul">
        <tr class="ev"><th class="lft">開盤價</th><th class="hl lft">最高價</th><th>最低價</th><th class="hl">昨收價</th> </tr>
        <tr><td class="lft"><asp:label ID="lblOpen" runat="server" Text='<%# Eval("Open") %>'></asp:label></td>
        <td class="lft"><asp:label ID="lblHigh" runat="server" Text='<%# Eval("High") %>'></asp:label></td>
        <td><asp:label ID="lblLow" runat="server" Text='<%# Eval("Low") %>'></asp:label></td>
        <td><asp:label ID="lblLast" runat="server" Text='<%# Eval("Deal")-Eval("Change") %>'></asp:label></td></tr>   
    </table>
</div>
<asp:Label ID="lblMarket" runat="server" Text='<%# Eval("Market") %>' Visible="False"></asp:Label>
<asp:Label ID="lblTime" runat="server" Text='<%# Eval("Time") %>' Visible="False"></asp:Label>
</ItemTemplate></asp:FormView>
</div>
<asp:SqlDataSource ID="sdsStock" runat="server" ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
    SelectCommand="SELECT Stock.StockNo, Stock.Name, Stock.Market, Stock.Time, Stock.Deal, Stock.Last, Stock.[Open], Stock.Buy, Stock.Up1Price, Stock.Sell, Stock.Down1Price, Stock.High, Stock.Low, Stock.Change, Stock.SingleVolume, Stock.TotalVolume, Stock.EPS, Stock.Mean60Distance, Stock.Mean60DistanceRate, Stock.NoNewHighLowDates, Stock.PriceVolume, Stock.NPTT, Stock.Up1Price, Stock.Up2Price, Stock.Up3Price, Stock.Up4Price, Stock.Up5Price, Stock.Up1Volume, Stock.Up2Volume, Stock.Up3Volume, Stock.Up4Volume, Stock.Up5Volume, Stock.Down1Price, Stock.Down2Price, Stock.Down3Price, Stock.Down4Price, Stock.Down5Price, Stock.Down1Volume, Stock.Down2Volume, Stock.Down3Volume, Stock.Down4Volume, Stock.Down5Volume, Stock.RankDuration, Market.Name AS MarketName FROM Stock INNER JOIN Market ON Stock.Market = Market.MarketId WHERE (Stock.StockNo = @StockNo) and ShowOrder < 100" 
    EnableViewState="False" EnableCaching="True" CacheDuration="60">
<SelectParameters>
    <asp:Parameter DefaultValue="0000" Name="StockNo" />
</SelectParameters>
</asp:SqlDataSource>

<%--新增會員股票追蹤清單--%>
<asp:SqlDataSource ID="sdsCollect" runat="server"
    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
    InsertCommand="IF Not Exists (Select * From Collection Where MemberNo = @MemberNo And StockNo = @StockNo And [Group] = @Group)
                                            BEGIN
                                                Declare @Order int
                                                Set @Order = (Select Max([Order]) From Collection Where MemberNo = @MemberNo)
                                                IF @Order is null
                                                    Set @Order = 1
                                                Else
                                                    Set @Order = @Order + 1
                                                INSERT INTO Collection(MemberNo, StockNo, [Order],[Group]) VALUES (@MemberNo,@StockNo,@Order,@Group)
                                            END">
    <InsertParameters>
        <asp:Parameter Name="MemberNo" />
        <asp:Parameter Name="StockNo" />
        <asp:Parameter DefaultValue="1" Name="Group" />
    </InsertParameters>
</asp:SqlDataSource>
<%--新增會員股票追蹤清單--%>