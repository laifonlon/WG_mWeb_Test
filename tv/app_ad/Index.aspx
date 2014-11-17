<%@ Page Title="" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="index.aspx.vb" Inherits="index" %>
<%@ Register src="SubNews.ascx" tagname="SubNews" tagprefix="uc1" %>
<%@ Register src="RealTimeChart.ascx" tagname="RealTimeChart" tagprefix="uc2" %>

<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<div id="gs">
    <div style="background-color:#fff; margin-top:5px;">
        <uc3:DealInfo ID="deal1" runat="server" EnableViewState="false" Head="即時指數 <span>INDEX</span>" />
</div>

<%--<div class="mg5">
    <div class="hd1"><h3>即時走勢圖 <span>CHART</span></h3></div>
    <div class="ct" style="padding:0px;"><div><uc2:RealTimeChart ID="rtc1" runat="server" /></div></div></div>--%>
<table cellpadding="0" cellspacing="0" style="width:100%;"><tr><td style="width:49%; padding-right:10px;vertical-align:top;">

    <%--<div class="hd1">
        <table><tr><td><span class="hl">即時新聞</span></td></tr></table>
    </div>--%>
    <uc1:SubNews ID="sn1" runat="server" EnableViewState="false"/>

</td><td style="vertical-align:top;">
<div class="mg5">
    <div class="hd1"><h3>K線圖 <span>K-CHART</span><table style="float:right"><tr><td style="color:Blue;font-size:12px"> ━ 5日</td><td style="color:darkgreen;font-size:12px">━ 10日</td><td style="color:orangered;font-size:12px">━ 20日</td></tr></table></div></h3>        
    <div class="ct" style="padding:0px;"><asp:Label ID="lblTechChat" runat="server" Text=""></asp:Label></div></div>
</td></tr></table>
</div>
</asp:Content>

