<%@ Page Language="VB" AutoEventWireup="false"  MasterPageFile="~/m_search.master" CodeFile="Default2.aspx.vb" Inherits="Default2" %>
<%@ Register src="MainIndex.ascx" tagname="MainIndex" tagprefix="uc1" %>
<%@ Register src="DealInfo.ascx" tagname="DealInfo" tagprefix="uc2" %>
<%@ Register src="RealTimeChart.ascx" tagname="RealTimeChart" tagprefix="uc3" %>
<%@ Register src="BillBoard_T.ascx" tagname="BillBoard_T" tagprefix="uc4" %>
<%@ Register src="Analysis.ascx" tagname="Analysis" tagprefix="uc5" %>
<%@ Register src="Hottip.ascx" tagname="Hottip" tagprefix="uc6" %>
<%@ Register src="PostViwer.ascx" tagname="PostViwer" tagprefix="uc7" %>
<%@ Register src="Footer.ascx" tagname="Footer" tagprefix="uc8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server" EnableViewState="False">
<link rel="stylesheet" type="text/css" href="/styles/homepage_wrapper.css" />
<link rel="stylesheet" type="text/css" href="/styles/Favo.css" />
<%--<script type="text/javascript" src="/scripts/wrapper/jquery.easing.js"></script>--%>
<script type="text/javascript" src="/scripts/Favo.min.js"></script>
<script type="text/javascript" src="/scripts/wrapper/script.min.js"></script>
<script type="text/javascript" src="/scripts/default.min.js"></script>
<%--<script type="text/javascript" src="http://apis.google.com/js/plusone.js">
    { lang: 'zh-TW' }
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server" EnableViewState="False">
    <div id="div_choice" style="display:none;" class="fixed-ad">
    <asp:Panel ID="pnlShoc" runat="server" EnableViewState="False">
    <div class="org_box">
     <span class="org_bot_cor"></span>
    <span style="color:white;">按下<img width="20px;" src="/image/iosfa.png" />把玩股網加入主畫面螢幕，一鍵看最新指數！</span>
    </div>
    </asp:Panel>
</div>
<div id="gs">
<div style="background-color:#fff; margin-top:5px;">
<div class="hd1"><h3>頭版頭條 <span>HEADLINE</span></h3></div>
<div class="ct">
<div id="jslidernews3" class="lof-slidecontent outline" style="height:95px;">
<div class="preload"><div></div></div>
    <div  class="button-previous"></div>
    <div  class="button-next"></div>
    <!-- MAIN CONTENT --> 
    <div class="main-slider-content" style="height:95px;">
    <ul class="sliders-wrap-inner">
    <asp:Literal ID="lHeadLine" runat="server" EnableViewState="False"/></ul></div></div></div>
</div>
<uc7:PostViwer ID="pv1" runat="server" EnableViewState="False" />
<div class="mg5">
<uc2:DealInfo ID="deal1" runat="server" Visible="True" EnableViewState="False" Head="即時指數 <span>INDEX</span>" />
</div>
<div class="mg5">
<div class="hd1"><h3>即時走勢圖 <span>CHART</span></h3></div>
<div class="ct" style="padding:0px;"><div> 
<uc3:RealTimeChart ID="rt1" runat="server" EnableViewState="False" />
</div></div>
</div>
<uc6:Hottip ID="Hottip1" runat="server" EnableViewState="False" />
<uc5:Analysis ID="als1" runat="server" EnableViewState="False" />
<uc4:BillBoard_T ID="bbt1" runat="server" EnableViewState="False" />
<uc1:MainIndex ID="mi1" runat="server" EnableViewState="False" />
<uc8:Footer ID="Footer1" runat="server" EnableViewState="False" />
</div>
</asp:Content>

