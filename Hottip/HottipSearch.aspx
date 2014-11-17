<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_search.master" CodeFile="HottipSearch.aspx.vb" Inherits="Hottip_HottipSearch" Title="WantGoo 玩股網 手機版" EnableViewState="False" %>

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

        <%--飆股搜尋--%>

        <div id="wg-hpsrch" class="mblock">
            <h2 class="mbk-hds gdt">飆股搜尋</h2>
            <ul class="hpsrch-rdo" style="display:none;"> 
                <li>
                    <asp:RadioButton ID="radBuy" runat="server" GroupName="mode" OnCheckedChanged="RadioButtonDirection_CheckedChanged" Text="多頭" CssClass="rdo" AutoPostBack="true" EnableViewState="False" Visible="false" />
                </li>
                <li>
                    <asp:RadioButton ID="radSell" runat="server" GroupName="mode" OnCheckedChanged="RadioButtonDirection_CheckedChanged" Text="空頭" CssClass="rdo" AutoPostBack="true" EnableViewState="False" Visible="false" />
                </li>
            </ul>
            <ul class="hpsrch-rdo">
                <li>
                    <asp:RadioButton ID="radWeek" runat="server" Text="近一周" OnCheckedChanged="RadioButtonDay_CheckedChanged" GroupName="x" CssClass="rdo" AutoPostBack="true" EnableViewState="False" />
                </li>
                <li>
                    <asp:RadioButton ID="radToday" runat="server" Text="今日" OnCheckedChanged="RadioButtonDay_CheckedChanged" GroupName="x" CssClass="rdo" AutoPostBack="true" EnableViewState="False" />
                </li>
                <li>
                    <asp:RadioButton ID="radYestoday" runat="server" Text="昨日" OnCheckedChanged="RadioButtonDay_CheckedChanged" GroupName="x" CssClass="rdo" AutoPostBack="true" EnableViewState="False"/>
                </li>
            </ul>

            <%--我設定的選股工具--%>
            <h4 class="mbk-hds2">我設定的選股工具：</h4>
            <div class="hpbtns-bx">

                <asp:ListView ID="lvMyRule" runat="server" DataSourceID="sdsMyRule" EnableViewState="False">
                    <ItemTemplate>
                        <span class="hpbtns btn-be">
                            <asp:LinkButton ID="btnName" runat="server" CssClass="hp-btn" PostBackUrl='<%# "/Hottip/HottipResult.aspx?Type=MyRule&CommandName=search&RuleId=" + Eval("RuleId").ToString() + "&RuleName=" + Eval("Name").ToString() + "&rd=" + GetRequestString("rd", "近一周")+ "&direction=" + GetRequestString("direction", "多頭")%>' Text='<%# Eval("Name") %>' />
                        </span>
                        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="sexybutton sexysimple sexygray" Text='<%# Eval("UpdateTitle") %>' CommandName='<%# Eval("UpdateState") %>' Visible="false" />
                        <asp:Literal ID="lblName" runat="server" Text='<%# Eval("Name") %>' Visible="False" />
                        <asp:Literal ID="lblRuleId" runat="server" Text='<%# Eval("RuleId") %>' Visible="False" />
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div style="text-align:center; margin:15px;">
                            無符合的資料
                        </div>
                    </EmptyDataTemplate>
                </asp:ListView>
                
                <asp:sqldatasource id="sdsMyRule" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>"  
                        InsertCommand="INSERT INTO HottipRule(MemberNo, SQL, [Rule], Name) VALUES (@MemberNo, @SQL, @Rule, @Name)" 
                        SelectCommand="SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                                                                SELECT RuleId, Name, M.MemberNo,
                                                                Case When Trend = 0 Then 'sexybutton sexysimple sexyred' 
                                                                Else 'sexybutton sexysimple sexygreen' End as Trend, 
                                                                Case When IsApproved = 1 And EditDeadline <= GetDate() Then 'ask' 
                                                                Else 'update' End as UpdateState, 
                                                                Case When IsApproved = 1 And EditDeadline <= GetDate() Then '<b>申請修改</b>' 
                                                                Else '<b>更新設定</b>' End as UpdateTitle 
                                                                FROM HottipRule R
                                                                Inner Join WantGoo.dbo.Member M
                                                                On R.MemberNo = M.MemberNo
                                                                Where R.MemberNo = @MemberNo and IsDelete = 0
                                                                And M.RichUserDeadLine >= GetDate()
                                                                Order By CreateTime Desc"  
                        DeleteCommand="DELETE FROM HottipRule WHERE (RuleId = @RuleId)" 
                        UpdateCommand="UPDATE HottipRule SET SQL =@SQL , [Rule] =@Rule Where RuleId = @RuleId and MemberNo = @MemberNo" 
                        EnableViewState="False">
                    <SelectParameters>
                        <asp:Parameter Name="MemberNo" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RuleId" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="SQL" />
                        <asp:Parameter Name="Rule" />
                        <asp:Parameter Name="RuleId" />
                        <asp:Parameter Name="MemberNo" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="MemberNo" />
                        <asp:Parameter Name="SQL" />
                        <asp:Parameter Name="Rule" />
                        <asp:Parameter Name="Name" />
                    </InsertParameters>
                </asp:sqldatasource>

            </div>
            <!-- hpbtns-bx:end -->
            <%--我設定的選股工具--%>

            <%--訂閱的選股工具--%>
            <h4 class="mbk-hds2">訂閱的選股工具：</h4>
            <div class="hpbtns-bx">
                <asp:ListView ID="lvMySubscribe" runat="server" DataSourceID="sdsMySubscribe"  EnableViewState="False">
                    <ItemTemplate>
                        <%--<span class="hpbtns btn-rd"><a href="#" class="hp-btn">整理成功乘勝追擊</a></span>--%>
                        <%--<span style="height:100%" class="querytip" title='<%# Eval("Tip") %>'>--%>
                        <span class="<%# Eval("Css") %>"  title='<%# Eval("Tip") %>'>
                            <asp:LinkButton ID="btnName" runat="server" CssClass='hp-btn' Enabled='<%# Eval("BeforeDeadline") %>' PostBackUrl='<%# "/Hottip/HottipResult.aspx?Type=MyRule&CommandName=search&RuleId=" + Eval("RuleId").ToString() + "&RuleName=" + Eval("Name").ToString() + "&rd=" + GetRequestString("rd", "近一周") + "&direction=" + GetRequestString("direction", "多頭")%>'>
                                <%# Eval("Name") %>
                            </asp:LinkButton>
                            <asp:literal id="lblName" runat="server" text='<%# Eval("Name") %>' Visible="false"/>
                            <asp:literal id="lblRuleId" runat="server" text='<%# Eval("RuleId") %>' Visible="False"/>
                            <%--<asp:literal id="lblTrend" runat="server" text='<%# Eval("Trend") %>' Visible="False"/>--%>
                        </span>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div style="text-align:center; margin:15px;">
                            無符合的資料
                        </div>
                    </EmptyDataTemplate>
                </asp:ListView>
                
                <asp:sqldatasource id="sdsMySubscribe" runat="server" 
                    connectionstring="<%$ ConnectionStrings:twStocksConnectionString %>"  
                        SelectCommand="SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
                                                            SELECT HottipRuleSubscribe.RuleID, HottipRule.Name, 
                                                            Case When (HottipRuleSubscribe.Deadline > Getdate()) Then 1 Else 0 END as BeforeDeadline,
                                                            Case When HottipRule.NeedRich = 0 Then 'hpbtns btn-be' 
                                                            When HottipRule.Trend = 0 Then 'hpbtns btn-rd' 
                                                            Else 'hpbtns btn-gn' End as Css,
                                                            Case When (HottipRuleSubscribe.Deadline > Getdate()) Then '使用期限到 ' + CONVERT(varchar(100), HottipRuleSubscribe.Deadline, 111)  
                                                            Else '使用期限己過，請到飆股市集續約！' END as Tip 
                                                            FROM HottipRuleSubscribe INNER JOIN HottipRule ON HottipRuleSubscribe.RuleID = HottipRule.RuleId
                                                            WHERE     (HottipRuleSubscribe.MemberNo = @MemberNo) AND (HottipRuleSubscribe.Enable = 1)
                                                            ORDER BY HottipRuleSubscribe.ShowOrder Desc"  
                EnableViewState="False">
                    <SelectParameters>
                        <asp:Parameter Name="MemberNo" DefaultValue="0" />
                    </SelectParameters>
                </asp:sqldatasource>

            </div>

            <%--查詢結果列表--%>

            <!-- hpbtns-bx:end -->
            <%--訂閱的選股工具--%>
        </div>
        <!-- wg-hpsrch:end -->
        <%--飆股搜尋--%>

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
    </script>
</asp:Content>
<%--主內容區 加載到 Footer--%>