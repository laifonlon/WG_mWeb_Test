<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="stock.aspx.vb" Inherits="Default2" %>
<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc1" %>
<%@ Register src="Three.ascx" tagname="Three" tagprefix="uc2" %>
<%@ Register src="StockClassSimple.ascx" tagname="StockClassSimple" tagprefix="uc3" %>
<%@ Register src="SubNews.ascx" tagname="SubNews" tagprefix="uc4" %>
<%@ Register src="Margin.ascx" tagname="Margin" tagprefix="uc5" %>
<%@ Register src="RealTimeChart.ascx" tagname="RealTimeChart" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
<style type="text/css">
tr.gvtable_row {text-align:center; color:#333; vertical-align:middle; height:20px;}
tr.gvtable_al:hover, tr.gvtable_row:hover { background-color:#FFF2DA; color:#000; cursor:default;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">

<div id="gs" style="">
    <div style="background-color:#fff;">
        <uc1:DealInfo ID="di1" runat="server" EnableViewState="False" Head="個股報價 <span>STOCK</span>" />
    </div>
<table cellpadding="0" cellspacing="0" style="width:100%;"><tr><td style="width:49%; padding-right:5px;vertical-align:top;">
<uc2:Three ID="Three1" runat="server" EnableViewState="False" />
<uc3:StockClassSimple ID="scs1" runat="server" EnableViewState="False" />

</td><td style="vertical-align:top; width:50%;">
<uc5:Margin ID="Margin1" runat="server" EnableViewState="False" />
<div class="mg5">
    <div class="hd1"><h3>K線圖 <span>K-CHART</span><table style="float:right"><tr><td style="color:Blue;font-size:12px"> ━ 5日</td><td style="color:darkgreen;font-size:12px">━ 10日</td><td style="color:orangered;font-size:12px">━ 20日</td></tr></table></div></h3>        
    <div class="ct" style="padding:0px;"><asp:Label ID="lblTechChat" runat="server" Text=""></asp:Label></div></div>
<uc4:SubNews ID="sn1" runat="server" EnableViewState="False" />
</td></tr></table>
<%--<div class="mg5">
    <div class="hd1"><h3>即時走勢圖 <span>CHART</span></h3></div>
    <div class="ct" style="padding:0px;"><div><uc6:RealTimeChart ID="rtc1" runat="server" /></div></div></div>--%>



</div>

</asp:Content>

