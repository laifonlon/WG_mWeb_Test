<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_search.master" CodeFile="HottipResult.aspx.vb" Inherits="Hottip_HottipResult" Title="WantGoo 玩股網 手機版" EnableViewState="False" %>

<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="uc8" %>
<%@ Register Src="~/AD/Mob_300x250.ascx" TagName="Mob_300x250" TagPrefix="uc10" %>

<%--主內容區 加載到Header--%>
<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" runat="Server" EnableViewState="False">
    <%--all for 頭版頭條--%>
    <link rel="stylesheet" type="text/css" href="/styles/homepage_wrapper.css?20140318" />
    <link rel="stylesheet" type="text/css" href="/styles/Favo.css?20140318" />
</asp:Content>
<%--主內容區 加載到Header--%>

<%--主內容區 加載到 Body--%>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" runat="Server" EnableViewState="False">

    <%--stage:start--%>
    <div id="stage">

        <%--飆股搜尋結果--%>
        <div id="wg-hprslt" class="mblock">
            <div class="hd-bx">
                <h2 class="mbk-hds gdt">飆股搜尋結果</h2>
                <span class="btns-rt"><a href="/Hottip/HottipSearch.aspx">重新選股</a><asp:LinkButton ID="btnAddAll" runat="server" Text="+自選股" /></span>
            </div>
            <!-- hd-bx:end -->
            <p class="rslt-wds">
                <em><asp:Literal ID ="ltRuleName" runat="server"></asp:Literal></em>
                <em>/ <asp:Literal ID ="ltRadioDate" runat="server"></asp:Literal></em>
                <em style="display:none;">/ <asp:Literal ID ="ltDirection" runat="server"></asp:Literal></em>
            </p>

            <div class="tb-fm bgline tb-fix-rt" id="Result">
                <table border="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th width="20%">股票</th>
                            <th width="20%">股價</th>
                            <th width="20%">漲跌</th>
                            <th width="20%">漲跌幅</th>
                            <th width="20%">
                                <%--<input type="checkbox" name="checkbox2" id="checkbox2" />全選--%>
                                <asp:CheckBox ID="CheckAll" runat="server" onclick="javascript: SelectAllCheckboxes(this);"  Text="全選" /> 
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:ListView ID="lvMyRule" runat="server" DataSourceID="sdsCollect" EnableViewState="False">
                            <ItemTemplate>
                                <tr>
                                    <td><a href='/stock.aspx?no=<%# Eval("StockNo")%>'><%# Eval("Name")%></a></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("Deal")%></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("Change2")%></td>
                                    <td class='<%# Eval("ColorStyle")%>'><%# Eval("perc")%></td>
                                    <td>
                                        <asp:Label ID="stockNo" Text='<%# Eval("StockNo")%>' runat="server" Visible="false" /> 
                                        <asp:CheckBox ID="select" runat="server"/> 
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div style="text-align:center; margin:15px;">
                                    無符合的資料
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </tbody>
                </table>
            </div>
            <!-- tb-fm:end -->
        </div>

        
            <%--我設定的選股工具 - 搜尋--%>
            <asp:sqldatasource id="sdsRule" runat="server" 
                connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>"  
                    SelectCommand="SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT SQL, [Rule] FROM HottipRule Where RuleId = @RuleId; " >
                <SelectParameters>
                    <asp:Parameter Name="RuleId" />
                </SelectParameters>
            </asp:sqldatasource>
            <%--我設定的選股工具 - 搜尋--%>
            
            <%--查詢用物件--%>

            <asp:SqlDataSource ID="sdsDate" runat="server" 
                connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>" 
                selectcommand="SELECT distinct top 2 Date FROM Selection1 Order by Date Desc" 
                CacheDuration="600" EnableCaching="True">
            </asp:SqlDataSource>

            <asp:Panel ID="pnlAdmin" runat="server" Visible="False">
                <asp:Literal ID="lblQuery" runat="server"/>
                <br />
                <asp:TextBox ID="tbxQuery" runat="server" TextMode="MultiLine" Width="100%" 
                        Height="100px"></asp:TextBox>
                <br />
                <asp:Button ID="btnSearch2" runat="server" Text="工程查詢" />
            </asp:Panel>

            <%--查詢用物件--%>

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

        <!-- wg-hprslt:end -->
        <%--飆股搜尋結果--%>

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

    <script type="text/javascript" src="/scripts/Favo.min.js?20140318"></script>
    <script type="text/javascript" src="/scripts/wrapper/script.min.js?20140318"></script>
    <script type="text/javascript" src="/scripts/default.min.js?20140318"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.bgline tr:nth-child(2n)').addClass('odd');
        });
        function SelectAllCheckboxes(spanChk) {
            range = document.getElementById("Result");
            elm = range.getElementsByTagName("input");
            for (i = 0; i <= elm.length - 1; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != spanChk.id) {
                    if (elm[i].checked != spanChk.checked) {
                        elm[i].click();
                    };
                };
            };
        };
    </script>

</asp:Content>
<%--主內容區 加載到 Footer--%>