<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Stocks.aspx.vb" Inherits="Stock" %>

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
<%--<ul id="nav">
<li id="mail"><a ontouchstart="" href="pe/mail/*http://tw.m.yahoo.com/w/ygo-mail"><span class="no">Yahoo!信箱</span></a></li>
<li id="ec"><a ontouchstart="" href="pe/shop/*http://m.buy.yahoo.com.tw/?&amp;co_servername=sMPE&amp;c2=sMPE"><span class="no">Yahoo!購物</span></a></li>
<li id="wch"><a ontouchstart="" href="pe/wretch/*http://tw.m.yahoo.com/w/wretch"><span class="no">無名小站</span></a></li>
<li id="stock"><a ontouchstart="" href="pe/st/*http://tw.m.yahoo.com/w/twstock"><span class="no">股市</span></a></li>
<li id="all"><button id="all-btn"><span class="txt">全部服務</span></button></li>
</ul>--%>
<div id="gs" style="">
    <div style="background-color:#fff;">
    <div class="hd1">
        <table><tr><td><span class="hl">個股報價</span></td><td class="rt"><span class="dt">更新: 2012/07/03 12:00</span></td></tr></table>
    </div>
    <div class="ct">
        <div>
            <table>
                <tr class="ev"><th class="lft">名稱</th><th class="hl">指數</th><th>漲跌</th><th class="hl">比例</th></tr>
                <tr><td class="lft">台積電</td><td>1000</td><td><span class="rd">+100.00</span></td><td><span class="rd">+1.00%</span></td></tr>
            </table>
        </div>
        <div>走勢圖</div>
    </div></div>
    <div class="mg">
    <div class="hd1">
        <table><tr><td><span class="hl">三大法人</span></td><%--<td class="rt"><span class="dt">更新: 2012/07/03 12:00</span></td>--%></tr></table>
    </div>
        <div class="ct"><div>
        <ul>
            <li class="al lt"><span class="t4l clr1">日期</span><span class="t4r clr2">外資</span><span class="t4r clr1">投信</span><span class="t4r clr2">自營</span><span class="t4r clr1">總和</span></li>
            <li><a><span class="tin">7/4</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tut">10000</span></a></li>
            <li class="al"><a><span class="tin">7/4</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tut">10000</span></a></li>
            <li><a><span class="tin">7/4</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tut">10000</span></a></li>
            <li class="al"><a><span class="tin">7/4</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tut">10000</span></a></li>
            <li><a><span class="tin">7/4</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tup">+1000</span><span class="tut">10000</span></a></li>
            <li class="al"><a><span class="tin">7/4</span><span class="tdn">-1000</span><span class="tdn">+1000</span><span class="tdn">-1000</span><span class="tut">10000</span></a></li>
        </ul><span class="clear"></span></div></div></div>
    <div class="mg">
    <div class="hd1">
        <table><tr><td><span class="hl">資券變化</span></td><%--<td class="rt"><span class="dt">更新: 2012/07/03 12:00</span></td>--%></tr></table>
    </div>
    <div class="ct">
        <div>
            <table>
                <tr class="ev"><th class="lft">日期</th><th class="hl">融資變化</th><th>融資餘額</th><th class="hl">資券互抵</th></tr>
                <tr><td class="lft" rowspan="3">7/4</td><td>10000</td><td><span class="rd">10000</span></td><td><span class="rd">10000</span></td></tr>
                <tr class="ev"><th class="hl">融券變化</th><th>融券餘額</th><th class="hl">當沖率</th></tr>
                <tr><td>10000</td><td><span class="rd">10000</span></td><td><span class="rd">100.00%</span></td></tr>
            </table>
        </div>
    </div></div>
    <div class="mg">
    <div class="hd1">
        <table><tr><td><span class="hl">相關個股</span></td><%--<td class="rt"><span class="dt">更新: 2012/07/03 12:00</span></td>--%></tr></table>
    </div>
        <div class="ct"><div>
        <ul>
            <li class="al lt"><span class="t1l clr1">股票</span><span class="t2 clr2">價格</span><span class="t2 clr1">漲跌</span><span class="t2 clr2">成交量</span></li>
            <li><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="up">+72.4</span><span class="ix">13002</span></a></li>
            <li class="al"><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="dn">-72.4</span><span class="ix">13002</span></a></li>
            <li><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="up">+72.4</span><span class="ix">13002</span></a></li>
            <li class="al"><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="dn">-72.4</span><span class="ix">13002</span></a></li>
            <li><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="up">+72.4</span><span class="ix">13002</span></a></li>
            <li class="al"><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="dn">-72.4</span><span class="ix">13002</span></a></li>
            <li><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="up">+72.4</span><span class="ix">13002</span></a></li>
            <li class="al"><a href="#"><span class="in_a">台機電</span><span class="ix">943.8</span><span class="dn">-72.4</span><span class="ix">13002</span></a></li>
        </ul><span class="clear"></span></div></div></div>
        <div class="mg">
    <div class="hd1">
        <table><tr><td><span class="hl">即時新聞</span></td><%--<td class="rt"><span class="dt">更新: 2012/07/03 12:00</span></td>--%></tr></table>
    </div>
    <div class="nws">
        <div>
            <ul>
                <li>
                    <a class="red" title="短線將拉回，要注意，切記勿追高"  href="/53059/12">短線將拉回，要注意，切記勿追高</a>
                    <p>(2012-07-03 08:53)&nbsp;&nbsp;<span>遊戲股王傳奇網路遊戲 (4994) 今(2)日股價早盤直奔漲停，來到381.5元，直逼高價股F4，僅次於大立光</span></p>
                </li>
                <li>
                    <a class="red" title="短線將拉回，要注意，切記勿追高"  href="/53059/12">短線將拉回，要注意，切記勿追高</a>
                    <p>(2012-07-03 08:53)&nbsp;&nbsp;<span>遊戲股王傳奇網路遊戲 (4994) 今(2)日股價早盤直奔漲停，來到381.5元，直逼高價股F4，僅次於大立光</span></p>
                </li>
            </ul>
<%--            <table>
                <tr class="ev"><th class="lft">名稱</th><th class="hl">指數</th><th>漲跌</th><th class="hl">比例</th></tr>
                <tr><td class="lft">台積電</td><td>1000</td><td><span class="rd">+100.00</span></td><td><span class="rd">+1.00%</span></td></tr>
            </table>--%>
        </div>
    </div></div>
</div>

<%--<script type="text/javascript">
    function tabSwitch(active, number, tab_prefix, content_prefix) {
        for (var i = 1; i < number + 1; i++) {
            document.getElementById(content_prefix + i).style.display = 'none';
            document.getElementById(tab_prefix + i).className = '';
        }
        document.getElementById(content_prefix + active).style.display = 'block';
        document.getElementById(tab_prefix + active).className = 'active';
    }  
</script>--%>
</form>
</body>
</html>
