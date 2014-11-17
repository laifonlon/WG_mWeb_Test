// 登出登入
function showState() {
    var urls = readCookie("urls");
    if (urls == 'm.wantgoo.com') {
        var username = readCookie("UserName");
        if (username != null) {
            $("#menu-btn").html("<b>登出</b>").attr("href", "/logout.aspx");
            $("#login-btn").html("<b>登出</b>").attr("href", "/logout.aspx");
        };
    };
};
function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    };
    return null;
};
showState();
// 主選單切換
var urls = location.pathname;
function setClassById(Id, className) { document.getElementById(Id).className = className; };
function indexUrl(keyWord) {
    if (urls.toLowerCase().indexOf(keyWord) >= 0) { return true; }
    else { return false; };
};
// 主選單-重置
setClassById('linka', ''); setClassById('linkb', ''); setClassById('linkc', ''); setClassById('linkd', ''); setClassById('linke', ''); setClassById('linkf', '');
// 主選單-切換
if (indexUrl("twindex")) { setClassById('linka', 'on'); }
else if (indexUrl("globals")) { setClassById('linkb', 'on'); }
else if (indexUrl("article")) { setClassById('linkc', 'on'); }
else if (indexUrl("club") || indexUrl("topic")) { setClassById('linkd', 'on'); }
else if (indexUrl("lesson")) { setClassById('linke', 'on'); }
else if (indexUrl("search")) { setClassById('linkf', 'on'); }
else { setClassById('linkf', ''); };
// Go Top 回最上方
$('#backtop').hide();
$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('#backtop').fadeIn();
    } else {
        $('#backtop').fadeOut();
    };
});
$('.gotop').click(function () {
    $('body,html').animate({ scrollTop: 0 }, 1);
    return false;
});