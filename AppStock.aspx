<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AppStock.aspx.vb" Inherits="AppStock" %>

<%@ Register src="StockViewr.ascx" tagname="StockViewr" tagprefix="uc1" %>

<%@ Register src="ExchangeRate.ascx" tagname="ExchangeRate" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <link rel="apple-touch-icon" href="/custom_icon.png"/>
<%--    <meta name="apple-touch-fullscreen" content="YES" />--%>
    <link href="http://www.wantgoo.com/favicon.ico" type="image/x-icon" rel="shortcut icon" />
    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
<%--    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />--%>
    <link href="http://www.wantgoo.com/favicon.ico" type="image/x-icon" rel="shortcut icon" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
    <link href="styles/style.css?20120723" media="all" rel="stylesheet" type="text/css" />
    <link href="styles/responsive.css" media="all" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/styles/hp.css" />
<%--    <link href="m.css" media="all" rel="stylesheet" type="text/css" />--%>
    <script src="scripts/script.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
<link rel="stylesheet" type="text/css" href="/styles/Favo.css" />
<script language="javascript" type="text/javascript" src="/scripts/wrapper/jquery.easing.js"></script>
<script language="javascript" type="text/javascript" src="/scripts/Favo.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        if (isiDecive() == true) {
            if (navigator.userAgent.indexOf("5.1 Mobile") != -1) {
                checkCookie('mwantgoo003')
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
 <div id="gm">
    <div id="gs">
    <asp:Panel ID="p1" runat="server" Visible="False">
    <div id="content_1" class="active" style="display:block;">
        <uc1:StockViewr ID="sv1" runat="server" Title="美洲股市指數行情" UpdateInterval ="29" Market="12" ShowCount ="50" HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />  
    </div>
    </asp:Panel>
 
     <asp:Panel ID="p2" runat="server" Visible="False">
    <div id="content_2" class="active" style="display:block">
        <uc1:StockViewr ID="sv3" runat="server" Title="亞洲股市指數行情" UpdateInterval ="29" Market="10" ShowCount ="50"  HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />
    </div>
    </asp:Panel>

     <asp:Panel ID="p3" runat="server" Visible="False">
    <div id="content_3" class="active" style="display:block">
        <uc1:StockViewr ID="sv4" runat="server" Title="台指期貨即時行情" UpdateInterval ="29" Market="5" ShowCount ="50" HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />
        <uc1:StockViewr ID="sv41" runat="server"  Title="期貨指數即時行情"  UpdateInterval ="0" Market="3" ShowCount ="50" HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False" />
    </div>
    </asp:Panel>

     <asp:Panel ID="p4" runat="server" Visible="False">
    <div id="content_4" class="active" style="display:block">
        <uc1:StockViewr ID="sv5" runat="server" Title="能源" UpdateInterval ="29" Market="22" ShowCount ="50" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false" ShowUpdate="False"/>
    </div>
    </asp:Panel>
 
     <asp:Panel ID="p5" runat="server" Visible="False">
    <div id="content_5" class="active" style="display:block">  
        <uc2:ExchangeRate ID="er1" runat="server" EnableViewState="false"/>
    </div>
    </asp:Panel>

    <asp:Panel ID="p6" runat="server" Visible="False">
    <div id="content_6" class="active" style="display:block;">
        <uc1:StockViewr ID="sv2" runat="server" Title="歐洲股市指數行情" UpdateInterval ="29" Market="11" ShowCount ="50" HideCount="0" StartUpdateTime="14" EndUpdateTime="23" EnableViewState ="false" ShowUpdate="False" />
    </div>
    </asp:Panel>
 
    <asp:Panel ID="p7" runat="server" Visible="False">
    <div id="content_7" class="active" style="display:block">
        <uc1:StockViewr ID="sv6" runat="server" Title="農產品" UpdateInterval ="29" Market="21" ShowCount ="50" StartUpdateTime="0" EndUpdateTime="0" EnableViewState ="false" ShowUpdate="False"/>
        <%--<uc1:StockViewr ID="sv7" runat="server" Title="波羅的海航運指數" UpdateInterval ="29" Market="24" ShowCount ="10" StartUpdateTime="9" EndUpdateTime="10" EnableViewState ="false" ShowUpdate="false"/>--%>
        <%--<uc1:StockViewr ID="sv8" runat="server" Title="風險指數" UpdateInterval ="29" Market="20" ShowCount ="50"  HideCount="0" StartUpdateTime="0" EndUpdateTime="0" EnableViewState ="false" ShowUpdate="False"/>--%>
    </div>
    </asp:Panel>
</div></div>
    </form>
</body>
</html>
