<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_search.master" CodeFile="HottipPage2.aspx.vb" Inherits="Hottip_HottipPage2" Title="自選股 法人/資券 - WantGoo 玩股網 手機版" EnableViewState="false" %>

<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="uc8" %>
<%@ Register Src="~/AD/Mob_300x250.ascx" TagName="Mob_300x250" TagPrefix="uc10" %>

<%--主內容區 加載到Header--%>
<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" runat="Server" EnableViewState="false">
    <%--all for 頭版頭條--%>
    <link rel="stylesheet" type="text/css" href="/styles/homepage_wrapper.css?20140318" />
    <link rel="stylesheet" type="text/css" href="/styles/Favo.css?20140318" />
</asp:Content>
<%--主內容區 加載到Header--%>

<%--主內容區 加載到 Body--%>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" runat="Server" EnableViewState="false">

    <%--stage:start--%>
    <div id="stage">

        <%--自選股 列表--%>

        <div id="wg-mystk" class="mblock">
            <div class="hd-bx2 gdt">
                <div class="hp-colt-bx">
                    <%--功能 選單--%>
                    <asp:DropDownList ID="ddlFunctionList" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="1">即時行情</asp:ListItem>
                        <asp:ListItem Value="4">技術面</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">法人/資券</asp:ListItem>
                        <asp:ListItem Value="3">基本面</asp:ListItem>
                        <asp:ListItem Value="9">相關新聞</asp:ListItem>
                    </asp:DropDownList>
                    <%--群組 選單--%>
                    <asp:DropDownList ID="ddlGroupList" runat="server" AutoPostBack="true"></asp:DropDownList>
                </div>
                <!-- hp-colt-bx:end -->
                <span class ="ElapseTime">30秒更新</span>
                <%--<span class="btns-rt btns-et"><a href="#">編輯</a></span>--%>
            </div>
            <!-- hd-bx:end -->

            <div class="tb-fm bgline tb-fix-rt">

                <div class="mystk-tb-bx">
                    <table cellspacing="0" cellpadding="0" id="mystk-data">
                        <tr>
                            <th>股票</th>
                            <th>資增</th>
                            <th>券增</th>
                            <th>資餘</th>
                            <th>券餘</th>
                            <th>券資比</th>
                            <th>資券互抵</th>
                            <th>當沖率</th>
                            <th>收盤價</th>
                            <th>漲跌幅</th>
                            <th>成交量</th>
                        </tr>
                        <%--列表--%>
                        <asp:ListView ID="lvMyCollect" runat="server" DataSourceID="sdsCollect" EnableViewState="False">
                            <ItemTemplate>
                                <tr>
                                    <td><a href='/stock.aspx?no=<%# Eval("StockNo")%>'><%# Eval("Name")%><asp:Literal ID="ltlStockNo" Text='<%# Eval("StockNo")%>' Visible="false" runat="server"></asp:Literal></a></td>
                                    <td><asp:label id="lblmarginadiff" runat="server" text='<%# Format(Eval("Diff1"),"0") %>'></asp:label></td>
                                    <td><asp:label id="lblshortdiff" runat="server" text='<%# Format(Eval("Diff2"),"0") %>'></asp:label></td>
                                    <td><asp:label id="lblmargina" runat="server" text='<%# Format(Eval("Today1"),"0") %>'></asp:label></td>
                                    <td><asp:label id="lblmarginb" runat="server" text='<%# Format(Eval("Today2"),"0") %>'></asp:label></td>
                                    <td><asp:label id="lblmarginr" runat="server" text='<%# Format(Eval("Diff3"),"0.00%") %>'></asp:label></td>
                                    <td><asp:label id="lbldiff" runat="server" text='<%# Format(Eval("Diff"),"0") %>'></asp:label></td>
                                    <td><asp:label id="lbldiffrate" runat="server"  text='<%# Format(Eval("Diff4")*1000,"0.00%") %>' ></asp:label></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("Deal")%></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("Change2")%></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("perc")%></td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div style="text-align:center; margin:15px;">
                                    無符合的資料
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                        <%--列表--%>
                    </table>
                </div>                

                <%--SqlDataSource--%>
                <asp:SqlDataSource ID="sdsCollect" runat="server"
                    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
                    SelectCommand="HottipCollectPage2 @MemberNo ,@Group;"
                    CacheDuration="60" EnableCaching="true">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="MemberNo" />
                        <asp:Parameter DefaultValue="1" Name="Group" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="sdsTAIEX" runat="server"
                    ConnectionString="<%$ ConnectionStrings:twStocksConnectionString %>"
                    SelectCommand="SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                                                            SELECT Top(1) d1.Buy1, d1.Sell1, d1.Repay1, d1.Before1, d1.Today1, d1.Today1 - d1.Before1 AS Diff1,Finances.Date, Finances.Buy2, Finances.Sell2, Finances.Repay2, (Finances.Today2-Finances.Before2) As Diff2, Finances.Today2, 
                                                                                  HistoryPriceDaily.[Close], HistoryPriceDaily.Volume, HistoryPriceDaily.ChangeRatio
                                                            FROM         Finances INNER JOIN
                                                                                      (SELECT     Date, Buy1, Sell1, Repay1, Before1, Today1
                                                                                        FROM          Finances AS Finances_1
                                                                                        WHERE      (StockNo = '0000A')) AS d1 ON Finances.Date = d1.Date INNER JOIN
                                                                                  HistoryPriceDaily ON Finances.StockNo = HistoryPriceDaily.StockNo AND Finances.Date = HistoryPriceDaily.Date
                                                            WHERE     (Finances.StockNo = '0000') Order by Finances.Date Desc"
                    CacheDuration="60" EnableCaching="True"></asp:SqlDataSource>
                <%--SqlDataSource--%>

                <!-- tb-fm:end -->
            </div>
            <!-- mystk-tb-bx:end -->
        </div>
        <!-- wg-hprslt:end -->

        <%--自選股 列表--%>

        <%--廣告:start--%>
        <div style="margin: 0 auto -5px; width: 300px;">
            <uc10:Mob_300x250 ID="Mob_300x2501" runat="server" />
        </div>
        <%--廣告:end--%>
    </div>
    <%--stage:end--%>

    <%--Footer:start--%>
    <uc8:Footer ID="Footer1" runat="server" EnableViewState="False" />
    <%--Footer:end--%>
</asp:Content>
<%--主內容區 加載到 Body--%>

<%--主內容區 加載到 Footer--%>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer_ContentPlaceHolder" runat="Server" EnableViewState="False">
    
    <script type="text/javascript" src="/scripts/responsivetable.min.js"></script>
    <script type="text/javascript" src="/scripts/Favo.min.js?20140318"></script>
    <script type="text/javascript" src="/scripts/wrapper/script.min.js?20140318"></script>
    <script type="text/javascript" src="/scripts/default.min.js?20140318"></script>

    <script type="text/javascript">
        var cdSeconds = 30;
        var cdMiliSeconds = 30000;
        var cdInterSet = 1000;
        var leastSeconds = 30;
        window.setInterval("ReloadPage();", cdMiliSeconds);
        window.setInterval("leastSeconds-=1;UpdateElapseTime();", cdInterSet);
        function ReloadPage() {
            leastSeconds = cdSeconds;
            location.reload();
        };
        function UpdateElapseTime() {
            $('.ElapseTime').html('' + leastSeconds.toString() + '秒更新');
        };
        $('.bgline tr:nth-child(2n)').addClass('odd');
        $('#mystk-data').responsiveTable({
            staticColumns: 1,
            scrollRight: false,
            scrollHintEnabled: true
        });
    </script>

</asp:Content>
<%--主內容區 加載到 Footer--%>