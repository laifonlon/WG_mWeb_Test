<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TwClass.aspx.vb" Inherits="TwClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<title>WantGoo 玩股網</title>
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
    <div id="gs">
        <div style="background-color:#fff;">
        
        </div>
    </div>
    </form>
</body>
</html>
