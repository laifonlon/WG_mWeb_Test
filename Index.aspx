<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="index.aspx.vb" Inherits="index" %>
<%@ OutputCache Duration="60" VaryByParam="no" %>
<%@ Register src="SubNews.ascx" tagname="SubNews" tagprefix="uc1" %>
<%@ Register src="RealTimeChart.ascx" tagname="RealTimeChart" tagprefix="uc2" %>
<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc3" %>
<%@ Register src="AD/Mob_320x50.ascx" tagname="Mob_320x50" tagprefix="uc4" %>
<%@ Register src="AD/Mob_300x250.ascx" tagname="Mob_300x250" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<uc4:Mob_320x50 ID="Mob_320x501" runat="server" />
<div id="gs">
    <div style="background-color:#fff; margin-top:5px;">
        <uc3:DealInfo ID="deal1" runat="server" EnableViewState="false" Head="即時指數 <span>INDEX</span>" />
</div>

<div class="mg5">
    <div class="hd1"><h3>即時走勢圖 <span>CHART</span></h3></div>
    <div class="ct" style="padding:0px;"><div><uc2:RealTimeChart ID="rtc1" runat="server" /></div></div></div>
<div style="margin-left: -5px;margin-top:5px;">
<uc4:Mob_320x50 ID="Mob_320x502" runat="server" />
</div>
<div class="mg5">
    <div class="hd1"><h3>K線圖 <span>K-CHART</span><table style="float:right"><tr><td style="color:Blue;font-size:12px"> ━ 5日</td><td style="color:darkgreen;font-size:12px">━ 10日</td><td style="color:orangered;font-size:12px">━ 20日</td></tr></table></div></h3>        
    <div class="ct" style="padding:0px;"><asp:Label ID="lblTechChat" runat="server" Text=""></asp:Label></div></div>
<div class="mg5">
    <%--<div class="hd1">
        <table><tr><td><span class="hl">即時新聞</span></td></tr></table>
    </div>--%>
    <uc1:SubNews ID="sn1" runat="server" EnableViewState="false"/>
     </div>
<div style="margin:0 auto -5px; width:300px;">
<%--廣告--%><uc5:mob_300x250 ID="Mob_300x2501" runat="server" /></div>
</div>
</asp:Content>

