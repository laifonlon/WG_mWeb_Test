<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Globals2.aspx.vb" Inherits="Globals" %>
<%@ Register src="footer.ascx" tagname="footer" tagprefix="uc1" %>
<%@ Register src="StockViewr.ascx" tagname="StockViewr" tagprefix="uc2" %>
 
<%@ Register src="ExchangeRate.ascx" tagname="ExchangeRate" tagprefix="uc3" %>
 
<%@ Register src="ImportantIndex.ascx" tagname="ImportantIndex" tagprefix="uc4" %>
<%@ Register src="RestMarket.ascx" tagname="RestMarket" tagprefix="uc5" %>
 
<!DOCTYPE html>
<html>
<head id="head1" runat="server">
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<title>WantGoo玩股網 國際股市</title>
<meta name="description" content="Stock Money 國際指數 股票 基金 期貨 投資 理財 即時國際股市期貨報價 人工智慧選股功能 高手盤勢個股分析" />
<meta name="keywords" content ="玩股網,股票,基金,理財,股市行情報價,投資,投顧,期貨,財經新聞,股市新聞,期指,選擇權,興櫃,未上市,理財講座,投資競賽,部落格,金融,證券, 虛擬金融市場, 虛擬股票,投資社群,交友,練習操作股票" />
<meta content="initial-scale=1.0,maximum-scale=1.0,user-scalable=0,width=device-width" name="viewport" />
<meta content="YES" name="apple-touch-fullscreen" />
<meta name="HandheldFriendly" content="True" />
<link href="http://www.wantgoo.com/favicon.ico" type="image/x-icon" rel="shortcut icon" />
<script type="text/javascript">
    P = { t: (new Date()).getTime() }; ; P.r = '5c2f48c1'; var Bp = {}; Bp.get = function (a) { if (typeof (a) == "object") { return a } return document.getElementById(a) }; Bp.query = function (b, a) { a = a || document.body; var c = a.querySelectorAll(b); return !c.length ? null : c[0] }; Bp.on = function (g, f, e, d, a) { g = Bp.get(g); d = d || this; var c = false; if (Bp.env.Webkit && typeof (window.ontouchstart) === "undefined") { switch (f) { case "touchstart": f = "mousedown"; c = true; break; case "touchmove": f = "mousemove"; c = true; break; case "touchend": f = "mouseup" } } var b = function (h) { var h = h || window.event; if (!h.target && h.srcElement) { h.target = h.srcElement } if (c && !h.touches) { h.touches = [h] } e.call(d, h) }; if (typeof (g.addEventListener) != "undefined") { if (e.handleEvent) { g.addEventListener(f, e, a) } else { g.addEventListener(f, b, a) } } else { if (g.attachEvent) { g.attachEvent("on" + f, b) } else { g["on" + f] = b } } }; Bp.preventDefault = function (a) { if (a.preventDefault) { a.preventDefault() } else { a.returnValue = false } }; Bp.stopPropagation = function (a) { if (a.stopPropagation) { a.stopPropagation() } else { a.cancelBubble = true } }; Bp.dom = { addClass: function (b, a) { b = Bp.get(b); if (b && b.className != null && !b.className.match(a)) { if (b.className != "") { a = " " + a } b.className += a } }, removeClass: function (b, a) { b = Bp.get(b); if (b && b.className != null) { b.className = b.className.replace(a, "").replace(/^\s+|\s+$/g, "") } }, hasClass: function (b, a) { b = Bp.get(b); if (b && b.className != null) { return b.className.match(new RegExp("(?:^|\\s+)" + a + "(?:\\s+|$)")) !== null } return false }, getXY: function (d, c) { d = Bp.get(d); var e = [d.offsetLeft || 0, d.offsetTop || 0]; var b = d.offsetParent; var a = c ? (this.getStyle(d, "position") === "absolute" && d.offsetParent == d.ownerDocument.body) : false; if (b != d) { while (b) { e[0] += b.offsetLeft || 0; e[1] += b.offsetTop || 0; if (c && !a && this.getStyle(b, "position") === "absolute") { a = true } b = b.offsetParent } } if (a) { e[0] -= d.ownerDocument.body.offsetLeft; e[1] -= d.ownerDocument.body.offsetTop } return e }, getStyle: function (b, a) { b = Bp.get(b); return window.getComputedStyle ? document.defaultView.getComputedStyle(b, null).getPropertyValue(a) : (b.currentStyle ? b.currentStyle[a] : b.style[a]) } }; Bp.env = (function () { var b = navigator.userAgent; var a = {}; a.RIM = RegExp("BlackBerry").test(b); a.RIM6 = a.RIM && RegExp("Version/6").test(b); a.IE = RegExp("MSIE ").test(b); a.OperaMobile = RegExp("Opera Mobi").test(b); a.Webkit = RegExp(" AppleWebKit/").test(b); a.Apple = RegExp("iPhone").test(b) || RegExp("iPad").test(b); a.iPhone3 = a.Apple && RegExp("OS 3").test(b); a.iPhone4 = a.Apple && RegExp("OS 4").test(b); a.iPhone5 = a.Apple && RegExp("OS 5").test(b); a.Android = RegExp("Android").test(b); a.Android2 = RegExp("Android 2").test(b); a.WebOS = RegExp("webOS").test(b); return a })(); Bp.locationHref = function (c, b, a) { Bp.on(c, b, function (d) { if (!(d.target.tagName == "A" || Bp.dom.getAncestorByTagName(d.target, "a"))) { location.href = a } }) }; Bp.getTextDirection = function () { return document.body.getAttribute("dir") || "ltr" }; Bp.defaultFocus = false; ;
</script>
<link type="text/css" media="all" href="m.css" rel="Stylesheet" />
</head>
<body id="bp-bd">
<form id="form1" runat="server">
<div id="hd">
    <h1><span class="no">WantGoo玩股網首頁</span></h1>
</div>

<div id="gm">
    <ul class="tabs_sub">
        <li><a href="javascript:tabSwitch(1, 4, 'tab_', 'content_');" id="tab_1" class="active">美歐</a></li>
        <li><a href="javascript:tabSwitch(2, 4, 'tab_', 'content_');" id="tab_2">亞洲</a></li>
        <li><a href="javascript:tabSwitch(3, 4, 'tab_', 'content_');" id="tab_3">期貨</a></li>
        <li class="list"><a href="javascript:tabSwitch(4, 4, 'tab_', 'content_');" id="tab_4">其他指數</a></li>
    </ul>
    <div id="content_1" class="content">
        <uc2:StockViewr ID="sv1" runat="server" Title="美洲股市指數行情" UpdateInterval ="29" Market="12" ShowCount ="50" HideCount="0" StartUpdateTime="18" EndUpdateTime="6" EnableViewState ="false"/>
        <uc2:StockViewr ID="sv2" runat="server" Title="歐洲股市指數行情" UpdateInterval ="29" Market="11" ShowCount ="50" HideCount="0" StartUpdateTime="14" EndUpdateTime="23" EnableViewState ="false" ShowUpdate="False" />
    </div>
    <div id="content_2" class="content">
        <uc2:StockViewr ID="sv3" runat="server" Title="亞洲股市指數行情" UpdateInterval ="29" Market="10" ShowCount ="8"  HideCount="0" StartUpdateTime="6" EndUpdateTime="18" EnableViewState ="false"/>
    </div>
    <div id="content_3" class="content">
        <uc2:StockViewr ID="sv4" runat="server" Title="期貨指數即時行情" UpdateInterval ="29" Market="5" ShowCount ="10" HideCount="0" StartUpdateTime="0" EndUpdateTime="24" EnableViewState ="false"/>
    </div>
    <div id="content_4" class="content">
        <uc2:StockViewr ID="sv5" runat="server" Title="能源" UpdateInterval ="29" Market="22" ShowCount ="50" StartUpdateTime="0" EndUpdateTime="0" EnableViewState ="false" ShowUpdate="False"/>
        <uc2:StockViewr ID="sv6" runat="server" Title="農產品" UpdateInterval ="29" Market="21" ShowCount ="50" StartUpdateTime="0" EndUpdateTime="0" EnableViewState ="false" ShowUpdate="False"/>
        <uc2:StockViewr ID="sv7" runat="server" Title="波羅的海航運指數" UpdateInterval ="29" Market="24" ShowCount ="10" StartUpdateTime="9" EndUpdateTime="10" EnableViewState ="false" ShowUpdate="false"/>
        <uc2:StockViewr ID="sv8" runat="server" Title="風險指數" UpdateInterval ="29" Market="20" ShowCount ="50"  HideCount="0" StartUpdateTime="0" EndUpdateTime="0" EnableViewState ="false" ShowUpdate="False"/>
        <uc3:ExchangeRate ID="er1" runat="server" EnableViewState="false"/>
    </div>
</div>
<div id="gs" style=" padding-top:0px;">
    <uc4:ImportantIndex ID="ii1" runat="server" EnableViewState="False" />
    <uc5:RestMarket ID="rm1" runat="server" EnableViewState="False" />
</div>

<script type="text/javascript">
    function tabSwitch(active, number, tab_prefix, content_prefix) {
        for (var i = 1; i < number + 1; i++) {
            document.getElementById(content_prefix + i).style.display = 'none';
            document.getElementById(tab_prefix + i).className = '';
        }
        document.getElementById(content_prefix + active).style.display = 'block';
        document.getElementById(tab_prefix + active).className = 'active';
    }  
</script>

<uc1:footer ID="footer1" runat="server" />


</form>
</body></html>