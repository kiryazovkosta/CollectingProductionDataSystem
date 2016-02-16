(function ($) {
    'use strict';
    var interval = 0;
    var expirationInterval = parseInt($('#expiration-interval').val()) || 30;
    var myInterval;
    $(document).ready(function () {
        myInterval = setInterval(function () {
                interval += 1;
                $('#counter').html("<h3>" + interval + "</h3>");
                if (interval >= expirationInterval) {
                    $.connection.hub.stop();
                    $.ajax({
                        url: '/Account/LogOff',
                        type: 'POST',
                        data: addAntiForgeryToken({})
                    }).done(function () {
                        interval = 0;
                        window.location.replace("/Home/Index");
                    });
                }
        }, 1000);

        if ($('#loginLink').val()) {
            clearInterval(myInterval);
        }

        $(window).click(function () {
            interval = 0;
        });
        $(window).keydown(function () {
            interval = 0;
        });
        $(window).keypress(function () {
            interval = 0;
        });
    });

    function addAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };
}(jQuery));