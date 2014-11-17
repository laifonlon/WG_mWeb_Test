<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_search.master" CodeFile="index_s.aspx.vb" Inherits="_index_s" Title="玩股首頁 - WantGoo 玩股網 手機版" EnableViewState="False" %>
<%@ OutputCache Duration="60" VaryByParam="None"  %>
<%@ Register src="Footer.ascx" tagname="Footer" tagprefix="uc8" %>
<%@ Register src="AD/Mob_300x250.ascx" tagname="Mob_300x250" tagprefix="uc10" %>

<%--主內容區 加載到Header--%>
<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server" EnableViewState="False">
<%--all for 頭版頭條--%>
<link rel="stylesheet" type="text/css" href="/styles/homepage_wrapper.css?20140318" />
<link rel="stylesheet" type="text/css" href="/styles/Favo.css?20140318" />
</asp:Content>
<%--主內容區 加載到Header--%>

<%--主內容區 加載到 Body--%>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server" EnableViewState="False">
    <div id="gs">

        <%--stage:start--%>
        <div id="stage">

            <%--<!-- 精選文章:start -->--%>
            <div id="wg-news" class="mblock">
                <!--分頁 tab-inner_0:start -->
                <ul class="tab-mn gdt">
                    <li><a href="javascript:tabSwitch(1, 4, 'tabmn_', 'tabinner_');" id="tabmn_1" class="active">頭條</a></li>
                    <li><a href="javascript:tabSwitch(2, 4, 'tabmn_', 'tabinner_');" id="tabmn_2">文章</a></li>
                    <li><a href="javascript:tabSwitch(3, 4, 'tabmn_', 'tabinner_');" id="tabmn_3">新聞</a></li>
                    <li><a href="javascript:tabSwitch(4, 4, 'tabmn_', 'tabinner_');" id="tabmn_4">台股</a></li>
                </ul>
                <!--分頁 tab-inner_0:end -->
                <!--頭條 tab-inner_1:start -->
                <asp:Literal ID="lbGoodHeadLine" runat="server" Text="Label"></asp:Literal>
                <!--頭條 tab-inner_1:end -->
                <!--文章 tab-inner_2:start -->
                <asp:Literal ID="lbGoodArticle" runat="server" Text="Label"></asp:Literal>
                <!--文章 tab-inner_2:end -->
                <!--新聞 tab-inner_3:start -->
                <asp:Literal ID="lbGoodNews" runat="server" Text="Label"></asp:Literal>
                <!--新聞 tab-inner_3:end -->
                <!--台股 tab-inner_4:start -->
                <asp:Literal ID="lbGoodTwStock" runat="server" Text="Label"></asp:Literal>
                <!--台股 tab-inner_4:end -->
            </div>
            <%--<!-- 精選文章:end -->--%>

            <%--台股資訊:start--%>
            <div id="wg-real" class="mblock">
                <h2 class="mbk-hds gdt">台股資訊</h2>
                <div class="tb-fm tb-txtrt">
                    <h4 class="tb-hdname">加權指數</h4>
                    <cite class="updt">
                        <asp:Literal ID="lUpdateTime" runat="server" /></cite>

                    <!-- chart-bx:start -->
                    <div class="chart-bx">
                        <asp:Literal ID="lbRealTimeChart" runat="server" />
                        <span style='font:14px Verdana,"LiHei Pro","微軟正黑體",sans-serif;color:gray;'>
                            <asp:Literal id="lbVolumeEstimate" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <!-- chart-bx:end -->

                    <asp:FormView ID="fvTwStock" runat="server" EnableViewState="False" Width="100%" CssClass="FormView">
                        <ItemTemplate>
                            <asp:Label ID="lblStockNo" runat="server" Text='<%# Eval("StockNo") %>' Visible="False"></asp:Label>
                            <asp:Label ID="lblMarket" runat="server" Text='<%# Eval("Market") %>' Visible="False"></asp:Label>
                            <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Time") %>' Visible="False"></asp:Label>
                            <table cellpadding="0" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th width="25%">指數</th>
                                        <th width="25%">漲跌</th>
                                        <th width="25%">比例</th>
                                        <th width="25%">開盤價</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class='<%# Eval("ColorStyle") %>'>
                                            <asp:Label ID="lblDeal" runat="server" Text='<%# Eval("Deal") %>'></asp:Label></td>
                                        <td class='<%# Eval("ColorStyle") %>'>
                                            <asp:Label ID="lblChange" runat="server" Text='<%# Eval("Change") %>'></asp:Label></td>
                                        <td class='<%# Eval("ColorStyle") %>'>
                                            <asp:Label ID="lblPercentage" runat="server" Text='<%# Format(Eval("Change")/(Eval("Deal")-Eval("Change")),"0.00%") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblOpen" runat="server" Text='<%# Eval("Open") %>'></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>

                            <table cellpadding="0" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th width="25%">最高價</th>
                                        <th width="25%">最低價</th>
                                        <th width="25%">昨收價</th>
                                        <th width="25%">成交量</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHigh" runat="server" Text='<%# Eval("High") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblLow" runat="server" Text='<%# Eval("Low") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblLast" runat="server" Text='<%# Eval("Deal")-Eval("Change") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblTotalVolume" runat="server" Text='<%# Eval("TotalVolume") %>'></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                            <!-- tb-fm:end -->
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:SqlDataSource ID="sdsStock" runat="server" ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
                        SelectCommand="SELECT Stock.StockNo, Stock.Name, Stock.Market, Stock.Time, Stock.Deal, Stock.Last, Case when Stock.Change > 0 then 'r' when Change < 0 then 'g' else '' End as ColorStyle, Stock.[Open], Stock.Buy, Stock.Up1Price, Stock.Sell, Stock.Down1Price, Stock.High, Stock.Low, Stock.Change, Stock.SingleVolume, Stock.TotalVolume, Stock.EPS, Stock.Mean60Distance, Stock.Mean60DistanceRate, Stock.NoNewHighLowDates, Stock.PriceVolume, Stock.NPTT, Stock.Up1Price, Stock.Up2Price, Stock.Up3Price, Stock.Up4Price, Stock.Up5Price, Stock.Up1Volume, Stock.Up2Volume, Stock.Up3Volume, Stock.Up4Volume, Stock.Up5Volume, Stock.Down1Price, Stock.Down2Price, Stock.Down3Price, Stock.Down4Price, Stock.Down5Price, Stock.Down1Volume, Stock.Down2Volume, Stock.Down3Volume, Stock.Down4Volume, Stock.Down5Volume, Stock.RankDuration, Market.Name AS MarketName FROM Stock INNER JOIN Market ON Stock.Market = Market.MarketId WHERE (Stock.StockNo = @StockNo) and ShowOrder < 100" 
                        EnableViewState="False" EnableCaching="True" CacheDuration="60">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="0000" Name="StockNo" />
                        </SelectParameters>
                    </asp:SqlDataSource>                    
                    <asp:SqlDataSource ID="sdsVolumeEstimate" runat="server" ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
                            SelectCommand="Select Top 1 Rate From TradeVolumeEstimate Where [Time] >= Cast(GETDATE() as time(7))"
                            EnableViewState="False" EnableCaching="True" CacheDuration="60">
                    </asp:SqlDataSource>
                </div>
                <div class="readmore"><a href="/TwIndex.aspx" class="btn-i">看更多台股資訊 +</a></div>
            </div>
            <%--台股資訊:end--%>
            
            <%--國際股市:start--%>
            <asp:Literal ID="lbInterNationalStock" runat="server" Text="Label"></asp:Literal>
            <%--國際股市:end--%>
            
            <%--飆股搜尋:start--%>
            <div id="wg-selt" class="mblock">
                <h2 class="mbk-hd">飆股搜尋</h2>
                <cite class="updt"><asp:Label ID="lblHottipDate" runat="server"></asp:Label></cite>
                <ul class="tab-mn gdt tab-pdlt">
                    <li><a href="javascript:tabSwitch(1, 2, 'tabmn2_', 'tabinner2_');" id="tabmn2_1" class="active">多方</a></li>
                    <li><a href="javascript:tabSwitch(2, 2, 'tabmn2_', 'tabinner2_');" id="tabmn2_2">空方</a></li>
                </ul>
                <div id="tabinner2_1" style="display: block;">
                    <div class="tb-fm">
                        <h4 class="tb-hdname r">多方強勢股</h4>
                        <table id="HottipGood" cellpadding="0" cellspacing="0" class="bgline tdf">
                            <thead>
                                <tr>
                                    <th width="100">項目</th>
                                    <th>內容</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <!-- tb-fm:end -->
                </div>
                <!-- 多方:end -->
                <div id="tabinner2_2" style="display: none;">
                    <div class="tb-fm">
                        <h4 class="tb-hdname g">空方強勢股</h4>
                        <table id="HottipBad" cellpadding="0" cellspacing="0" class="bgline tdf">
                            <thead>
                                <tr>
                                    <th width="100">項目</th>
                                    <th>內容</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <!-- tb-fm:end -->
                </div>
                <!-- 空方:end -->
                <div class="readmore"><a href="http://www.wantgoo.com/hottipselect.aspx" class="btn-i">找更多強勢股 +</a></div>
                <asp:SqlDataSource ID="sdsNewestDate" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                    selectcommand="SELECT MAX(Date) AS LastDate FROM Selection1 With(NoLock)" 
                    CacheDuration="3600" EnableCaching="True">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="sdsLastDate" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                    selectcommand="Select Top 1 Date as LastDate From (SELECT distinct top 2 Date FROM Selection1 With(NoLock) Order by Date Desc) t Order by Date" 
                    CacheDuration="3600" EnableCaching="True">
                </asp:SqlDataSource>
            </div>
            <%--飆股搜尋:end--%>
            
            <%--大盤即時力道:start--%>            
            <div id="wg-idxreal" class="mblock">
                <h2 class="mbk-hds gdt">大盤即時力道</h2>
                <div class="tb-fm tb-txtrt">
                    <asp:FormView ID="fvGeneralStock" runat="server" DataSourceID="sdsADline" Width="100%" CssClass="FormView">
                        <ItemTemplate>
                            <asp:Label ID="diffK" runat="server" Text='<%# eval("RaiseKCount") - eval("FallKCount") %>' Font-Bold="True" Visible="False"></asp:Label>
                            <table cellpadding="0" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th width="25%">今日</th>
                                        <th width="25%">昨日</th>
                                        <th width="25%">3日平均</th>
                                        <th width="25%">5日平均</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="g">
                                        <td><asp:Label ID="d1" runat="server" Text='<%# format(eval("d1"),"0.00%") %>'></asp:Label></td>
                                        <td><asp:Label ID="d2" runat="server" Text='<%# format(eval("d2"),"0.00%") %>'></asp:Label></td>
                                        <td><asp:Label ID="a3" runat="server" Text='<%# format(eval("a3"),"0.00%") %>'></asp:Label></td>
                                        <td><asp:Label ID="a5" runat="server" Text='<%# format(eval("a5"),"0.00%") %>'></asp:Label></td>
                                    </tr>
                                </tbody>
                                <thead>
                                    <tr>
                                        <th width="25%">漲停</th>
                                        <th width="25%">紅K</th>
                                        <th width="25%">跌停</th>
                                        <th width="25%">黑K</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="r"><asp:Label ID="raiseLimit" runat="server" Text='<%# eval("RaiseLimitCount") %>' ForeColor="Red"></asp:Label></td>
                                        <td class="r"><asp:Label ID="raiseK" runat="server" Text='<%# eval("RaiseKCount") %>' ForeColor="Red"></asp:Label></td>
                                        <td class="g"><asp:Label ID="fallLimit" runat="server" Text='<%# eval("FallLimitCount") %>' ForeColor="Green"></asp:Label></td>
                                        <td class="g"><asp:Label ID="fallK" runat="server" Text='<%# eval("FallKCount") %>' ForeColor="Green"></asp:Label></td>
                                </tbody>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                </div>
                <!-- tb-fm:end -->
                    <asp:SqlDataSource ID="sdsADline" runat="server" 
                        connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                        selectcommand="exec GeneralStockSelect;" 
                    CacheDuration="300" EnableCaching="True" EnableViewState="true">
                    </asp:SqlDataSource>
            </div>
            <%--大盤即時力道:end--%>
            
            <%--台股分類:start--%>
            <div id="wg-twctg" class="mblock">
                <h2 class="mbk-hd">台股分類</h2>
                <ul class="tab-mn gdt tab-pdlt">
                    <li><a href="javascript:tabSwitch(1, 2, 'tabmn3_', 'tabinner3_');" id="tabmn3_1" class="active">漲幅</a></li>
                    <li><a href="javascript:tabSwitch(2, 2, 'tabmn3_', 'tabinner3_');" id="tabmn3_2">跌幅</a></li>
                </ul>
                <div id="tabinner3_1" style="display: block;">
                    <div class="tb-fm tb-flt tb-txtrt">
                        <table cellpadding="0" cellspacing="0" class="bgline">
                            <thead>
                                <tr>
                                    <th width="64%">類股</th>
                                    <th width="36%">比例%</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:ListView ID="TwStock_Top_1_5" DataSourceID="ds_TwStock_Top_1_5" runat="server" EnableViewState="False">
                                    <ItemTemplate>
                                        <tr>
                                            <td><span class="rnum"><%# Eval("NUM")%></span><a href="/class.aspx?id=<%# Eval("ClassId")%>"><%# Eval("ShortName")%></a></td>
                                            <td class="<%# Eval("StyleType")%>"><%# Eval("ChangeDay")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </tbody>
                        </table>
                    </div>
                    <!-- tb-fm:end -->

                    <div class="tb-fm tb-frt tb-txtrt">
                        <table cellpadding="0" cellspacing="0" class="bgline">
                            <thead>
                                <tr>
                                    <th width="60%">類股</th>
                                    <th width="40%">比例%</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:ListView ID="TwStock_Top_6_10" DataSourceID="ds_TwStock_Top_6_10" runat="server" EnableViewState="False">
                                    <ItemTemplate>
                                        <tr>
                                            <td><span class="rnum"><%# Eval("NUM")%></span><a href="/class.aspx?id=<%# Eval("ClassId")%>"><%# Eval("ShortName")%></a></td>
                                            <td class="<%# Eval("StyleType")%>"><%# Eval("ChangeDay")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </tbody>
                        </table>
                    </div>
                    <!-- tb-fm:end -->
                </div>
                <!-- 漲幅:end -->
                <div id="tabinner3_2" style="display: none;">
                    <div class="tb-fm tb-flt tb-txtrt">
                        <table cellpadding="0" cellspacing="0" class="bgline">
                            <thead>
                                <tr>
                                    <th width="60%">類股</th>
                                    <th width="40%">比例%</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:ListView ID="TwStock_Down_1_5" DataSourceID="ds_TwStock_Down_1_5" runat="server" EnableViewState="False">
                                    <ItemTemplate>
                                        <tr>
                                            <td><span class="rnum"><%# Eval("NUM")%></span><a href="/class.aspx?id=<%# Eval("ClassId")%>"><%# Eval("ShortName")%></a></td>
                                            <td class="<%# Eval("StyleType")%>"><%# Eval("ChangeDay")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </tbody>
                        </table>
                    </div>
                    <!-- tb-fm:end -->

                    <div class="tb-fm tb-frt tb-txtrt">
                        <table cellpadding="0" cellspacing="0" class="bgline">
                            <thead>
                                <tr>
                                    <th width="60%">類股</th>
                                    <th width="40%">比例%</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:ListView ID="TwStock_Down_6_10" DataSourceID="ds_TwStock_Down_6_10" runat="server" EnableViewState="False">
                                    <ItemTemplate>
                                        <tr>
                                            <td><span class="rnum"><%# Eval("NUM")%></span><a href="/class.aspx?id=<%# Eval("ClassId")%>"><%# Eval("ShortName")%></a></td>
                                            <td class="<%# Eval("StyleType")%>"><%# Eval("ChangeDay")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </tbody>
                        </table>
                    </div>
                    <!-- tb-fm:end -->
                </div>
                <!-- 跌幅:end -->
                <div class="clear"></div>
                <%--<div class="readmore"><a href="http://www.wantgoo.com/stock/class.aspx" class="btn-i">看更多台股分類資訊 +</a></div>--%>
                
                <asp:SqlDatasource id="ds_TwStock_Top_1_5" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                    selectcommand="SELECT NUM,[ShortName],[ClassId],
                                                        Case When ChangeDay &gt; 0 Then '▲' When ChangeDay &lt; 0 then '▼' Else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay))) as [ChangeDay],
                                                        Case When ChangeDay &gt; 0 Then 'r' When ChangeDay &lt; 0 then 'g' Else '' End as [StyleType]
                                                        FROM
                                                        (
                                                           SELECT 
                                                            ROW_NUMBER() OVER(ORDER BY ChangeDay Desc) NUM, *
                                                            FROM [Class] With(NoLock)
                                                            Where [Group] = 2 and classID &lt;&gt;39
                                                        ) A
                                                        WHERE NUM &gt;=1 AND NUM &lt;=5" 
                    EnableCaching="True" CacheDuration="600">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDatasource>

                <asp:SqlDatasource id="ds_TwStock_Top_6_10" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                    selectcommand="SELECT (NUM+5) as NUM,[ShortName],[ClassId],
                                                        Case When ChangeDay &gt; 0 Then '▲' When ChangeDay &lt; 0 then '▼' Else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay))) as [ChangeDay],
                                                        Case When ChangeDay &gt; 0 Then 'r' When ChangeDay &lt; 0 then 'g' Else '' End as [StyleType]
                                                        FROM
                                                        (
                                                           SELECT 
                                                            ROW_NUMBER() OVER(ORDER BY ChangeDay Desc) NUM, *
                                                            FROM [Class] With(NoLock)
                                                            Where [Group] = 2 and classID &lt;&gt;39
                                                        ) A
                                                        WHERE NUM &gt;=1 AND NUM &lt;=5" 
                    EnableCaching="True" CacheDuration="600">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDatasource>
                
                <asp:SqlDatasource id="ds_TwStock_Down_1_5" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                    selectcommand="SELECT NUM,[ShortName],[ClassId],
                                                        Case When ChangeDay &gt; 0 Then '▲' When ChangeDay &lt; 0 then '▼' Else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay))) as [ChangeDay],
                                                        Case When ChangeDay &gt; 0 Then 'r' When ChangeDay &lt; 0 then 'g' Else '' End as [StyleType]
                                                        FROM
                                                        (
                                                           SELECT 
                                                            ROW_NUMBER() OVER(ORDER BY ChangeDay) NUM, *
                                                            FROM [Class] With(NoLock)
                                                            Where [Group] = 2 and classID &lt;&gt;39
                                                        ) A
                                                        WHERE NUM &gt;=1 AND NUM &lt;=5" 
                    EnableCaching="True" CacheDuration="600">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDatasource>
                
                <asp:SqlDatasource id="ds_TwStock_Down_6_10" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                    selectcommand="SELECT (NUM+5) as NUM,[ShortName],[ClassId],
                                                        Case When ChangeDay &gt; 0 Then '▲' When ChangeDay &lt; 0 then '▼' Else '' End + convert(varchar(10),abs( Convert(decimal(10,2),100 *ChangeDay))) as [ChangeDay],
                                                        Case When ChangeDay &gt; 0 Then 'r' When ChangeDay &lt; 0 then 'g' Else '' End as [StyleType]
                                                        FROM
                                                        (
                                                           SELECT 
                                                            ROW_NUMBER() OVER(ORDER BY ChangeDay) NUM, *
                                                            FROM [Class] With(NoLock)
                                                            Where [Group] = 2 and classID &lt;&gt;39
                                                        ) A
                                                        WHERE NUM &gt;=1 AND NUM &lt;=5" 
                    EnableCaching="True" CacheDuration="600">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDatasource>
            </div>
            <%--台股分類:end--%>
            
            <%--個股排行:start--%>
            <div id="wg-astk" class="mblock">
                <h2 class="mbk-hd">個股排行</h2>
                <ul class="tab-mn gdt tab-pdlt">
                    <li><a href="javascript:tabSwitch(1, 3, 'tabmn4_', 'tabinner4_');" id="tabmn4_1" class="active">漲幅</a></li>
                    <li><a href="javascript:tabSwitch(2, 3, 'tabmn4_', 'tabinner4_');" id="tabmn4_2">跌幅</a></li>
                    <li><a href="javascript:tabSwitch(3, 3, 'tabmn4_', 'tabinner4_');" id="tabmn4_3">成交量</a></li>
                </ul>
                <div class="tb-fm tb-txtrt">
                    <div id="tabinner4_1" style="display: block;">
                        <table cellspacing="0" cellpadding="0" class="bgline">
                            <thead>
                                <tr>
                                    <th>股票</th>
                                    <th>成交價</th>
                                    <th>漲跌</th>
                                    <th>比例%</th>
                                    <th>成交量</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal ID="lbTwStockRank_Up" runat="server" Text="Label"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                    <!-- 漲幅:end -->

                    <div id="tabinner4_2" style="display: none;">
                        <table cellspacing="0" cellpadding="0" class="bgline">
                            <thead>
                                <tr>
                                    <th>股票</th>
                                    <th>成交價</th>
                                    <th>漲跌</th>
                                    <th>比例%</th>
                                    <th>成交量</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal ID="lbTwStockRank_Down" runat="server" Text="Label"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                    <!-- 跌幅:end -->

                    <div id="tabinner4_3" style="display: none;">
                        <table cellspacing="0" border="0" class="bgline">
                            <thead>
                                <tr>
                                    <th>股票</th>
                                    <th>成交價</th>
                                    <th>漲跌</th>
                                    <th>比例%</th>
                                    <th>成交量</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal ID="lbTwStockRank_Volumn" runat="server" Text="Label"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                    <!-- 成交量:end -->
                </div>
                <!-- tb-fm:end -->
            </div>
            <%--個股排行:end--%>
            
            <%--資券變化:start--%>
            <div id="wg-fina" class="mblock">
                <h4 class="mbk-hd">資券變化</h4>
                <cite class="updt"><%= Now.ToString("yyyy-MM-dd")%></cite>
                <ul class="tab-mn gdt tab-pdlt">
                    <li><a href="javascript:tabSwitch(1, 2, 'tabmn5_', 'tabinner5_');" id="tabmn5_1" class="active">融資</a></li>
                    <li><a href="javascript:tabSwitch(2, 2, 'tabmn5_', 'tabinner5_');" id="tabmn5_2">融券</a></li>
                </ul>
                <div class="tb-fm tb-txtrt">
                    <div id="tabinner5_1" style="display: block;">
                        <h4 class="tb-hdname">融資增減變化</h4>
                        <div class="tb-flt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th width="50%">股票</th>
                                        <th>融資增加</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbFinance_Up" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 融資增加:end -->

                        <div class="tb-frt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th width="50%">股票</th>
                                        <th>融資減少</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbFinance_Down" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 融資減少:end -->
                    </div>
                    <!-- 融資:end -->

                    <div id="tabinner5_2" style="display: none;">
                        <h4 class="tb-hdname">融券增減變化</h4>
                        <div class="tb-flt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>融券增加</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbBearish_Up" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 融券增加:end -->

                        <div class="tb-frt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>融券減少</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbBearish_Down" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 融券減少:end -->
                    </div>
                    <!-- 融券:end -->

                    <div class="clear"></div>
                </div>
                <!-- tb-fm:end -->

            </div>
            <%--資券變化:end--%>
            
            <%--法人買賣超:start--%>
            <div id="wg-trad" class="mblock">
                <h4 class="mbk-hd">法人買賣超</h4>
                <cite class="updt"><%= Now.ToString("yyyy-MM-dd")%></cite>
                <ul class="tab-mn gdt tab-pdlt">
                    <li><a href="javascript:tabSwitch(1, 3, 'tabmn6_', 'tabinner6_');" id="tabmn6_1" class="active">外資</a></li>
                    <li><a href="javascript:tabSwitch(2, 3, 'tabmn6_', 'tabinner6_');" id="tabmn6_2">投信</a></li>
                    <li><a href="javascript:tabSwitch(3, 3, 'tabmn6_', 'tabinner6_');" id="tabmn6_3">自營商</a></li>
                </ul>
                <div class="tb-fm tb-txtrt">
                    <div id="tabinner6_1" style="display: block;">
                        <h4 class="tb-hdname">外資買賣超</h4>
                        <div class="tb-flt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>買超</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbTwLegalPerson_Foreign_Add" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 外資買超:end -->
                        <div class="tb-frt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>賣超</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbTwLegalPerson_Foreign_Reduce" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 外資賣超:end -->
                    </div>
                    <!-- 外資:end -->

                    <div id="tabinner6_2" style="display: none;">
                        <h4 class="tb-hdname">投信買賣超</h4>
                        <div class="tb-flt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>買超</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbTwLegalPerson_ING_Add" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 投信買超:end -->
                        <div class="tb-frt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>賣超</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbTwLegalPerson_ING_Reduce" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 投信賣超:end -->
                    </div>
                    <!-- 投信:end -->

                    <div id="tabinner6_3" style="display: none;">
                        <h4 class="tb-hdname">自營商買賣超</h4>
                        <div class="tb-flt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>買超</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbTwLegalPerson_Dealer_Add" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 自營商買超:end -->
                        <div class="tb-frt">
                            <table cellspacing="0" border="0" class="bgline">
                                <thead>
                                    <tr>
                                        <th>股票</th>
                                        <th>賣超</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Literal ID="lbTwLegalPerson_Dealer_Reduce" runat="server" Text="Label"></asp:Literal>
                                </tbody>
                            </table>
                        </div>
                        <!-- 自營商賣超:end -->
                    </div>
                    <!-- 自營商:end -->

                    <div class="clear"></div>
                </div>
                <!-- tb-tm:end -->
            </div>
            <%--法人買賣超:end--%>
            
            <%--績效統計:start--%>
            <div id="wg-perf" class="mblock">
                <h4 class="mbk-hd">績效統計</h4>
                <ul class="tab-mn gdt tab-pdlt">
                    <li><a href="javascript:tabSwitch(1, 3, 'tabmn7_', 'tabinner7_');" id="tabmn7_1" class="active">一週</a></li>
                    <li><a href="javascript:tabSwitch(2, 3, 'tabmn7_', 'tabinner7_');" id="tabmn7_2">一月</a></li>
                    <li><a href="javascript:tabSwitch(3, 3, 'tabmn7_', 'tabinner7_');" id="tabmn7_3">一季</a></li>
                </ul>
                <div class="chart-perf">
                    <div id="tabinner7_1" style="display: block;">
                        <asp:Literal ID="lbPerformanceStatistic01" runat="server"></asp:Literal>
                    </div>
                    <div id="tabinner7_2" style="display: none;">
                        <asp:Literal ID="lbPerformanceStatistic02" runat="server"></asp:Literal>
                    </div>
                    <div id="tabinner7_3" style="display: none;">
                        <asp:Literal ID="lbPerformanceStatistic03" runat="server"></asp:Literal>
                    </div>
                    <div style="color:white; font-size:small;"><%= Now.ToString() %></div>
                </div>
                <!-- chart-perf:end -->
            </div>
            <%--績效統計:end--%>
        
            <%--廣告:start--%>
            <div style="margin:0 auto -5px; width:300px;">
                <uc10:Mob_300x250 ID="Mob_300x2501" runat="server" />
            </div>
            <%--廣告:end--%>
        </div>
        <%--stage:end--%>

        <%--Footer:start--%>
        <uc8:Footer ID="Footer1" runat="server" EnableViewState="False" />
        <%--Footer:end--%>
    </div>
    
</asp:Content>
<%--主內容區 加載到 Body--%>

<%--主內容區 加載到 Footer--%>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer_ContentPlaceHolder" Runat="Server" EnableViewState="False">

<script type="text/javascript" src="/scripts/Favo.min.js?20140318"></script>
<script type="text/javascript" src="/scripts/wrapper/script.min.js?20140318"></script>
<script type="text/javascript" src="/scripts/default.min.js?20140318"></script>
    
    
<script type="text/javascript">
    function HottipUpdate() {
        $.ajax({
            type: "post",
            cache: false,
            url: "/MobileGetData.asmx/Hottip",
            success: function (response) {
                $.each(response, function (key, value) {
                    if (key == "HottipGood") {
                        $("#HottipGood tbody").html(value);
                    };
                    if (key == "HottipBad") {
                        if ($("#HottipBad tbody").text.length < 5) {
                            $("#HottipBad tbody").html(value);
                        };
                    };
                });
            }
        });
    };
    HottipUpdate();
</script>
</asp:Content>
<%--主內容區 加載到 Footer--%>