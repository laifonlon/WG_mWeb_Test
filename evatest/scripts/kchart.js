
    function CheckserAgent() {
        var userAgentInfo = navigator.userAgent;
        var userAgentKeywords = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod", "MQQBrowser");
        var flag = false;
        //排除windows系统
        if (userAgentInfo.indexOf("Windows NT") == -1) {
            flag = true;
        }
        return flag;
    }

    function setOrientation(direction, url) {
        var orient = 'portrait';

        if (window.orientation) {
            orient = Math.abs(window.orientation) === 90 ? 'landscape' : 'portrait';
        }
        else if (window.screen) {
            var width = screen.width;
            var height = screen.height;
            orient = (width > height) ? 'landscape' : 'portrait';
        }
        else {
            orient = 'portrait';
        }

        if (orient.toString != direction.toString)
            window.location = url;
    }
