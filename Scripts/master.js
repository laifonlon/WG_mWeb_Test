$(document).ready(function () {
    var pagebody = $("#pagebody");
    var themenu = $("#navmenu");
    var topbar = $("#toolbarnav");
    var content = $("#content");
    var viewport = {
        width: $(window).width(),
        height: $(window).height()
    };
    // retrieve variables as 
    // viewport.width / viewport.height

    function openme() {
        $(function () {
            topbar.animate({
                left: "250px"
            }, { duration: 300, queue: false });
            pagebody.animate({
                left: "250px"
            }, { duration: 300, queue: false });
        });
    }

    function closeme() {
        var closeme = $(function () {
            topbar.animate({
                left: "0px"
            }, { duration: 180, queue: false });
            pagebody.animate({
                left: "0px"
            }, { duration: 180, queue: false });
        });
    }

//    // checking whether to open or close nav menu
//    $("#menu-btn").live("click", function (e) {
//        e.preventDefault();
//        var leftval = pagebody.css('left');

//        if (leftval == "0px") {
//            openme();
//        }
//        else {
//            closeme();
//        }
//    });

    // loading page content for navigation
    $("a.navlink").live("click", function (e) {
        e.preventDefault();
        var linkurl = $(this).attr("href");
        var linkhtmlurl = linkurl.substring(1, linkurl.length);

        var imgloader = '<center style="margin-top: 30px;"><img src="img/preloader.gif" alt="loading..." /></center>';

        closeme();

        $(function () {
            topbar.css("top", "0px");
            window.scrollTo(0, 1);
        });

        content.html(imgloader);

        setTimeout(function () { content.load(linkhtmlurl, function () { /* no callback */ }) }, 1200);
    });
});

P = { t: (new Date()).getTime() }; ; P.r = '5c2f48c1'; var Bp = {}; Bp.get = function (a) { if (typeof (a) == "object") { return a } return document.getElementById(a) }; Bp.query = function (b, a) { a = a || document.body; var c = a.querySelectorAll(b); return !c.length ? null : c[0] }; Bp.on = function (g, f, e, d, a) { g = Bp.get(g); d = d || this; var c = false; if (Bp.env.Webkit && typeof (window.ontouchstart) === "undefined") { switch (f) { case "touchstart": f = "mousedown"; c = true; break; case "touchmove": f = "mousemove"; c = true; break; case "touchend": f = "mouseup" } } var b = function (h) { var h = h || window.event; if (!h.target && h.srcElement) { h.target = h.srcElement } if (c && !h.touches) { h.touches = [h] } e.call(d, h) }; if (typeof (g.addEventListener) != "undefined") { if (e.handleEvent) { g.addEventListener(f, e, a) } else { g.addEventListener(f, b, a) } } else { if (g.attachEvent) { g.attachEvent("on" + f, b) } else { g["on" + f] = b } } }; Bp.preventDefault = function (a) { if (a.preventDefault) { a.preventDefault() } else { a.returnValue = false } }; Bp.stopPropagation = function (a) { if (a.stopPropagation) { a.stopPropagation() } else { a.cancelBubble = true } }; Bp.dom = { addClass: function (b, a) { b = Bp.get(b); if (b && b.className != null && !b.className.match(a)) { if (b.className != "") { a = " " + a } b.className += a } }, removeClass: function (b, a) { b = Bp.get(b); if (b && b.className != null) { b.className = b.className.replace(a, "").replace(/^\s+|\s+$/g, "") } }, hasClass: function (b, a) { b = Bp.get(b); if (b && b.className != null) { return b.className.match(new RegExp("(?:^|\\s+)" + a + "(?:\\s+|$)")) !== null } return false }, getXY: function (d, c) { d = Bp.get(d); var e = [d.offsetLeft || 0, d.offsetTop || 0]; var b = d.offsetParent; var a = c ? (this.getStyle(d, "position") === "absolute" && d.offsetParent == d.ownerDocument.body) : false; if (b != d) { while (b) { e[0] += b.offsetLeft || 0; e[1] += b.offsetTop || 0; if (c && !a && this.getStyle(b, "position") === "absolute") { a = true } b = b.offsetParent } } if (a) { e[0] -= d.ownerDocument.body.offsetLeft; e[1] -= d.ownerDocument.body.offsetTop } return e }, getStyle: function (b, a) { b = Bp.get(b); return window.getComputedStyle ? document.defaultView.getComputedStyle(b, null).getPropertyValue(a) : (b.currentStyle ? b.currentStyle[a] : b.style[a]) } }; Bp.env = (function () { var b = navigator.userAgent; var a = {}; a.RIM = RegExp("BlackBerry").test(b); a.RIM6 = a.RIM && RegExp("Version/6").test(b); a.IE = RegExp("MSIE ").test(b); a.OperaMobile = RegExp("Opera Mobi").test(b); a.Webkit = RegExp(" AppleWebKit/").test(b); a.Apple = RegExp("iPhone").test(b) || RegExp("iPad").test(b); a.iPhone3 = a.Apple && RegExp("OS 3").test(b); a.iPhone4 = a.Apple && RegExp("OS 4").test(b); a.iPhone5 = a.Apple && RegExp("OS 5").test(b); a.Android = RegExp("Android").test(b); a.Android2 = RegExp("Android 2").test(b); a.WebOS = RegExp("webOS").test(b); return a })(); Bp.locationHref = function (c, b, a) { Bp.on(c, b, function (d) { if (!(d.target.tagName == "A" || Bp.dom.getAncestorByTagName(d.target, "a"))) { location.href = a } }) }; Bp.getTextDirection = function () { return document.body.getAttribute("dir") || "ltr" }; Bp.defaultFocus = false; ;

var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-6993262-2']);
_gaq.push(['_trackPageview']);
(function () {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();
_gaq.push(['_setAccount', 'UA-33609668-1']);
_gaq.push(['_trackPageview']);
(function () {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();