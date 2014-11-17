    function ReImgWid(img) {
        var width = img.width;
        var scale = 300 / width;
        var height = img.height;
        if (width > 300) {
            img.width = 300;
            img.height = height * scale;
        };
    }
(function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/zh_TW/all.js#xfbml=1&appId=288694587893836";
        fjs.parentNode.insertBefore(js, fjs);
    } (document, 'script', 'facebook-jssdk'));