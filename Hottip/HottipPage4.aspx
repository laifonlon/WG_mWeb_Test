<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_search.master" CodeFile="HottipPage4.aspx.vb" Inherits="Hottip_HottipPage4" Title="自選股 技術面 - WantGoo 玩股網 手機版" EnableViewState="false" %>

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
                        <asp:ListItem Value="4" Selected="True">技術面</asp:ListItem>
                        <asp:ListItem Value="2">法人/資券</asp:ListItem>
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
                            <th>股價</th>
                            <th>漲跌</th>
                            <th>漲幅%</th>
                            <th>5日</th>
                            <th>10日</th>
                            <th>20日</th>
                            <th>60日</th>
                            <th>季線扣抵</th>
                            <th>月線扣抵</th>
                            <th>偏多比率</th>
                        </tr>
                        <%--列表--%>
                        <asp:ListView ID="lvMyCollect" runat="server" DataSourceID="sdsCollect" EnableViewState="False">
                            <ItemTemplate>
                                <tr>
                                    <td><a href='/stock.aspx?no=<%# Eval("StockNo")%>'><%# Eval("Name")%><asp:Literal ID="ltlStockNo" Text='<%# Eval("StockNo")%>' Visible="false" runat="server"></asp:Literal></a></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("Deal")%></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("Change2")%></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("perc")%></td>
                                    <td><asp:literal id="lblMean5" runat="server" text='<%# Eval("Mean5")%>'/></td>
                                    <td><asp:literal id="lblMean10" runat="server" text='<%# Eval("Mean10")%>'/></td>
                                    <td><asp:literal id="lblMean20" runat="server" text='<%# Eval("Mean20")%>'/></td>
                                    <td><asp:literal id="lblMean60" runat="server" text='<%# Eval("Mean60")%>'/></td>
                                    <td><asp:literal id="lblMonthClose" runat="server" text='<%# Format(Eval("MonthClose"), "0.00")%>'/></td>
                                    <td><asp:literal id="lblSeasonClose" runat="server" text='<%# Format(Eval("SeasonClose"), "0.00")%>'/></td>
                                    <td><asp:literal id="lblRisePercent" runat="server" text='<%# Eval("risepercent")%>'/></td>
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
                    SelectCommand="HottipCollectPage4 @MemberNo,@Group"
                    DeleteCommand="DELETE FROM Collection WHERE (MemberNo = @MemberNo) AND (StockNo = @StockNo) And [Group] = @Group
                                                            EXEC dbo.ReOrderCollection @MemberNo,@Group"
                    InsertCommand="IF Not Exists (Select * From Collection Where MemberNo = @MemberNo And StockNo = @StockNo And [Group] = @Group)
                                                            BEGIN
                                                                Declare @Order int
                                                                Set @Order = (Select Max([Order]) From Collection Where MemberNo = @MemberNo)
                                                                IF @Order is null
                                                                    Set @Order = 1
                                                                Else
                                                                    Set @Order = @Order + 1
                                                                INSERT INTO Collection(MemberNo, StockNo, [Order],[Group]) VALUES (@MemberNo,@StockNo,@Order,@Group)
                                                            END"
                    UpdateCommand="Update Collection Set [Group] = @Group Where MemberNo = @MemberNo And StockNo = @StockNo">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="MemberNo" />
                        <asp:Parameter DefaultValue="1" Name="Group" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="MemberNo" />
                        <asp:Parameter Name="StockNo" />
                        <asp:Parameter DefaultValue="1" Name="Group" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="MemberNo" />
                        <asp:Parameter Name="StockNo" />
                        <asp:Parameter DefaultValue="1" Name="Group" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MemberNo" />
                        <asp:Parameter Name="StockNo" />
                        <asp:Parameter DefaultValue="1" Name="Group" />
                    </UpdateParameters>
                </asp:SqlDataSource>
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