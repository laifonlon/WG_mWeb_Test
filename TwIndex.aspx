<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_search.master" CodeFile="TwIndex.aspx.vb" Inherits="TwIndex" EnableViewState="False" Title="台股大盤 即時報價 - 玩股網手機版" %>
<%@ OutputCache Duration="60" VaryByParam="None" %>

<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc1" %>
<%@ Register src="Three.ascx" tagname="Three" tagprefix="uc2" %>
<%@ Register src="StockClassSimple.ascx" tagname="StockClassSimple" tagprefix="uc3" %>
<%@ Register src="SubNews.ascx" tagname="SubNews" tagprefix="uc4" %>
<%@ Register src="Margin.ascx" tagname="Margin" tagprefix="uc5" %>
<%@ Register src="RealTimeChart.ascx" tagname="RealTimeChart" tagprefix="uc6" %>
<%@ Register src="TwIndexRelate.ascx" tagname="TwIndexRelate" tagprefix="uc7" %>
<%@ Register src="Hottip.ascx" tagname="Hottip" tagprefix="uc8" %>
<%@ Register src="AD/Mob_320x50.ascx" tagname="Mob_320x50" tagprefix="uc9" %>
<%@ Register src="AD/Mob_300x250.ascx" tagname="Mob_300x250" tagprefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<link rel="stylesheet" type="text/css" href="/styles/Favo.css" />
<script language="javascript" type="text/javascript" src="/scripts/wrapper/jquery.easing.js"></script>
<script language="javascript" type="text/javascript" src="/scripts/Favo.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        if (isiDecive() == true) {
            if (navigator.userAgent.indexOf("5.1 Mobile") != -1) {
                checkCookie('mwantgoo002')
            };
        };
    });
</script>
<div id="div_choice" style="display:none;" class="fixed-ad">
    <asp:Panel ID="pnlShoc" runat="server">
    <div class="org_box">
     <span class="org_bot_cor"></span>
    <span style="color:white;">按下<img width="20px;" src="/image/iosfa.png" />把玩股網加入主畫面螢幕，一鍵看最新指數！</span>
    </div>
    </asp:Panel>
</div>
<uc9:Mob_320x50 ID="Mob_320x501" runat="server" />
<div id="gs">
    <div style="background-color:#fff; margin-top:5px;">
        <uc1:DealInfo ID="di1" runat="server" EnableViewState="False" Head="加權指數" />
    </div>
<div class="mg5">
    <div class="hd1"><h3>即時走勢圖 <span>CHART</span></h3></div>
    <div class="ct" style="padding:0px;"><div><uc6:RealTimeChart ID="rtc1" runat="server" /></div></div></div>
<div style="margin-left: -5px;margin-top:5px;"">
<uc9:Mob_320x50 ID="Mob_320x502" runat="server" />
</div>
<div class="mg5">
    <div class="hd1"><h3>K線圖 <span>K-CHART</span><table style="float:right"><tr><td style="color:Blue;font-size:12px"> ━ 5日</td><td style="color:darkgreen;font-size:12px">━ 10日</td><td style="color:orangered;font-size:12px">━ 20日</td></tr></table>   
    </div></h3>        
    <div class="ct" style="padding:0px;"><asp:Label ID="lblTechChat" runat="server" Text=""></asp:Label></div></div>
 
<uc7:TwIndexRelate ID="tir1" runat="server" EnableViewState="False" />
<uc5:Margin ID="Margin1" runat="server" EnableViewState="False" />
<uc2:Three ID="Three1" runat="server" EnableViewState="False" />
<uc8:Hottip ID="Hottip1" runat="server" EnableViewState="False" />
<uc4:SubNews ID="sn1" runat="server" EnableViewState="False" />
</div>
<div style="margin:0 auto -5px; width:300px;">
<%--廣告--%><uc10:Mob_300x250 ID="Mob_300x2501" runat="server" /></div>
</asp:Content>

