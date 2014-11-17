$(document).ready(function () {
    // buttons for next and previous item						 
    var buttons = { previous: $('#jslidernews3 .button-previous'),
        next: $('#jslidernews3 .button-next')
    };
    var _complete = function (slider, index) {
        $('#jslidernews3 .slider-description').animate({ height: 0 });
        slider.find(".slider-description").animate({ height: 100 })
    };
    $('#jslidernews3').lofJSidernews({ interval: 8000,
        direction: 'opacity',
        easing: 'easeOutBounce',
        duration: 600,
        auto: true,
        maxItemDisplay: 5,
        startItem: 0,
        navPosition: 'horizontal', // horizontal
        navigatorHeight: 20,
        navigatorWidth: 20,
        mainWidth: 610,
        mainHeight: 115,
        buttons: buttons,
        isPreloaded: false,
        onComplete: _complete
    });

    if (isiDecive() == true) {
        checkCookie('mwantgoo001');
    };
});