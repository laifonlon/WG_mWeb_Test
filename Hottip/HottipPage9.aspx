<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_search.master" CodeFile="HottipPage9.aspx.vb" Inherits="Hottip_HottipPage9" Title="自選股 相關新聞 - WantGoo 玩股網 手機版" EnableViewState="false" %>

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
                        <asp:ListItem Value="2">法人/資券</asp:ListItem>
                        <asp:ListItem Value="3">基本面</asp:ListItem>
                        <asp:ListItem Value="9" Selected="True">相關新聞</asp:ListItem>
                </asp:DropDownList>
                    <%--群組 選單--%>
                    <asp:DropDownList ID="ddlGroupList" runat="server" AutoPostBack="true"></asp:DropDownList>
                </div>
                <!-- hp-colt-bx:end -->
                <span class ="ElapseTime">30秒更新</span>
                <%--<span class="btns-rt btns-et"><a href="#">編輯</a></span>--%>
            </div>
            <!-- hd-bx:end -->
            
            <%--自選股相關新聞--%>
            <asp:Literal ID="Literal_aboutnews" runat="server"></asp:Literal>
            <%--自選股相關新聞--%>

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