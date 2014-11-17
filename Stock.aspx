<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="stock.aspx.vb" Inherits="Default2" %>
<%--<%@ OutputCache Duration="15" VaryByParam="no" %>--%>

<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc1" %>
<%@ Register src="Three.ascx" tagname="Three" tagprefix="uc2" %>
<%@ Register src="StockClassSimple.ascx" tagname="StockClassSimple" tagprefix="uc3" %>
<%@ Register src="SubNews.ascx" tagname="SubNews" tagprefix="uc4" %>
<%@ Register src="Margin.ascx" tagname="Margin" tagprefix="uc5" %>
<%@ Register src="RealTimeChart.ascx" tagname="RealTimeChart" tagprefix="uc6" %>
<%@ Register src="AD/Mob_320x50.ascx" tagname="Mob_320x50" tagprefix="uc7" %>
<%@ Register src="AD/Mob_300x250.ascx" tagname="Mob_300x250" tagprefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
<style type="text/css">
tr.gvtable_row {text-align:center; color:#333; font-size:14px; vertical-align:middle; height:20px;}
tr.gvtable_al:hover, tr.gvtable_row:hover { background-color:#FFF2DA; color:#000; cursor:default;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<uc7:Mob_320x50 ID="Mob_320x501" runat="server" />
<div id="gs" style="">
    <div style="background-color:#fff; margin-top:5px;">
        <uc1:DealInfo ID="di1" runat="server" EnableViewState="False" Head="個股報價 <span>STOCK</span>" />
    </div>
<div class="mg5">
    <div class="hd1"><h3>即時走勢圖 <span>CHART</span></h3></div>
    <div class="ct" style="padding:0px;"><div><uc6:RealTimeChart ID="rtc1" runat="server" /></div></div></div>
<div style="margin-left: -5px;margin-top:5px;">
<uc7:Mob_320x50 ID="Mob_320x502" runat="server" />
</div>
<div class="mg5">
    <div class="hd1"><h3>K線圖 <span>K-CHART</span><table style="float:right"><tr><td style="color:Blue;font-size:12px"> ━ 5日</td><td style="color:darkgreen;font-size:12px">━ 10日</td><td style="color:orangered;font-size:12px">━ 20日</td></tr></table></div></h3>        
    <div class="ct" style="padding:0px;"><asp:Label ID="lblTechChat" runat="server" Text=""></asp:Label></div></div>

<uc5:Margin ID="Margin1" runat="server" EnableViewState="False" />
<uc2:Three ID="Three1" runat="server" EnableViewState="False" />
<uc3:StockClassSimple ID="scs1" runat="server" EnableViewState="False" />
<uc4:SubNews ID="sn1" runat="server" EnableViewState="False" />
</div>
<div style="margin:0 auto -5px; width:300px;">
<%--廣告--%><uc8:Mob_300x250 ID="Mob_300x2501" runat="server" /></div>
</asp:Content>

