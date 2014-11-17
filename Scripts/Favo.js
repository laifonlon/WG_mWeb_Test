function isiDecive() {
        return (((navigator.platform.indexOf("iPhone") != -1)
    || (navigator.platform.indexOf("iPod") != -1)) && (navigator.userAgent.indexOf("5.1 Mobile") != -1));
    }

    function SetFav(isFav, key) {
        var Layer_choice;
        //抓div 
        if (document.getElementById) { //Netscape 6.x
            Layer_choice = eval("document.getElementById('div_choice')");
        }
        else { // IE 5.x
            Layer_choice = eval("document.all.choice.div_choice");
        }
        if (isFav == 0) { Layer_choice.style.display = 'none'; };
        if (isFav == 1) {
            Layer_choice.style.display = '';
            createCookie(key, '1234', 365)
            setTimeout(function(){SetFav(0,key)}, 6000);
        };
    }

    function checkCookie(key) {
        var udata;
        udata = getCookie(key);
        if (udata == null) {
            SetFav(1, key);
        }
    }

    function createCookie(name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "";
            expires = "" + date.toGMTString();
        }
        else var expires = "";
        document.cookie = name + "=" + value + "; expires=" + expires;
    }

    function getCookie(c_name) {
        if (document.cookie.length > 0) {
            var c_list = document.cookie.split("\;");
            for (i in c_list) {
                var cook = c_list[i].split("=");
                var a;
                a = cook[0].replace(" ", "");
                if (a == c_name) {
                    return unescape(cook[1]);
                }
            }
        }
        return null;
    }