<%@ Page Title="歐美股市指數行情, 全球主要指數, 全球休市預告 - 玩股網手機版" Language="VB" MasterPageFile="~/m_search.master" AutoEventWireup="false" CodeFile="globals.aspx.vb" Inherits="globals2" %>
<%@ Register src="footer.ascx" tagname="footer" tagprefix="uc1" %>
<%@ Register src="StockViewr.ascx" tagname="StockViewr" tagprefix="uc2" %>
<%@ Register src="ExchangeRate.ascx" tagname="ExchangeRate" tagprefix="uc3" %>
<%@ Register src="ImportantIndex.ascx" tagname="ImportantIndex" tagprefix="uc4" %>
<%@ Register src="RestMarket.ascx" tagname="RestMarket" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header_ContentPlaceHolder" Runat="Server">
<style type="text/css">
tr.gvtable_row, li.linav {text-align:center; color:#333; font-size:1em; vertical-align:middle;}
tr.gvtable_al:hover, tr.gvtable_row:hover, li.linav:hover { background-color:#FFF2DA; color:#000; cursor:default;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body_ContentPlaceHolder" Runat="Server">
<link rel="stylesheet" type="text/css" href="/styles/Favo.css" />
<script language="javascript" type="text/javascript" src="/scripts/wrapper/jquery.easing.js"></script>
<script language="javascript" type="text/javascript" src="/scripts/Favo.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        if (isiDecive() == true) {
            checkCookie('mwantgoo003');
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
 <div id="gm">
<%--    <ul class="tabs_sub">
        <li><asp:LinkButton ID="tab_1" runat="server">歐美</asp:LinkButton></li>
        <li><asp:LinkButton ID="tab_2" runat="server">亞洲</asp:LinkButton></li>
        <li><asp:LinkButton ID="tab_3" runat="server">期貨</asp:LinkButton></li>
        <li><asp:LinkButton ID="tab_4" runat="server">其他</asp:LinkButton></li>
        <li class="list"><asp:LinkButton ID="tab_5" runat="server">匯率</asp:LinkButton></li>
    </ul>--%>
    <div id="gs">
<table cellpadding="0" cellspacing="0" style="width:100%;"><tr><td style="width:49%; padding-right:10px;vertical-align:top;">
<uc2:StockViewr ID="sv1" runat="server" Title="美洲股市指數行情" UpdateInterval ="29" Market="12" ShowCount ="50" HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />
<uc2:StockViewr ID="sv2" runat="server" Title="歐洲股市指數行情" UpdateInterval ="29" Market="11" ShowCount ="50" HideCount="0" StartUpdateTime="14" EndUpdateTime="23" EnableViewState ="false" ShowUpdate="False" />
<uc2:StockViewr ID="sv41" runat="server"  Title="期貨指數即時行情"  UpdateInterval ="0" Market="3" ShowCount ="50" HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />
<uc4:ImportantIndex ID="ii1" runat="server" EnableViewState="False" />
<uc3:ExchangeRate ID="er1" runat="server" EnableViewState="false"/>
</td><td style="vertical-align:top;">
<uc2:StockViewr ID="sv3" runat="server" Title="亞洲股市指數行情" UpdateInterval ="29" Market="10" ShowCount ="50"  HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />
<uc2:StockViewr ID="sv4" runat="server" Title="台指期貨即時行情" UpdateInterval ="29" Market="5" ShowCount ="50" HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />
<uc2:StockViewr ID="sv5" runat="server" Title="能源" UpdateInterval ="29" Market="22" ShowCount ="50" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False"/>
<uc2:StockViewr ID="sv6" runat="server" Title="農產品" UpdateInterval ="29" Market="21" ShowCount ="50" StartUpdateTime="0" EndUpdateTime="0" EnableViewState ="false" ShowUpdate="False"/>
<uc2:StockViewr ID="sv7" runat="server" Title="波羅的海航運指數" UpdateInterval ="29" Market="24" ShowCount ="10" StartUpdateTime="9" EndUpdateTime="10" EnableViewState ="false" ShowUpdate="false"/>
<uc2:StockViewr ID="sv8" runat="server" Title="風險指數" UpdateInterval ="29" Market="20" ShowCount ="50"  HideCount="0" StartUpdateTime="0" EndUpdateTime="0" EnableViewState ="false" ShowUpdate="False"/>
<uc5:RestMarket ID="rm1" runat="server" EnableViewState="False" />
</td></tr></table>
    
</div></div>
 
<uc1:footer ID="footer1" runat="server" />
</asp:Content>
