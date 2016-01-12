(function ($) {
    $(function () {

        // Proxy created on the fly
        var job = $.connection.messagesHub;

        // Declare a function on the job hub so the server can invoke it
        job.client.displayStatus = function () {
            getData();
        };

        job.client.displayNewMessage = function (message) {
            displayMessage(message);
        }

        // Start the connection
        $.connection.hub.start();
        getMessagesCount();
    });

    function getData() {
        var messageWrapper = $('#message-box-body pre');
        var timeout = window.setTimeout(function () { }, 500);
        window.clearTimeout(timeout);
        $.ajax({
            url: '/Tests/GetMessages',
            type: 'GET',
            datatype: 'json',
            success: function (data) {
                if (data.length > 0) {

                    var message = '';
                    $.each(data, function (index, value) { message += value + '\n'; });
                    messageWrapper.html(message);
                    $('#massage-box').modal('show');
                }
            }
        });
    }

    function displayMessage(message) {
        $('#messages').popover({
            'content': message,
            'placement': 'bottom',
            'trigger': 'click',
        });
        $('#messages').popover('show');
        $('.popover').off('click').on('click', function (e) {
            e.stopPropagation();
            $(e.currentTarget).popover('destroy');
        });

        getMessagesCount();
    }

    function getMessagesCount() {
        $.ajax({
            url: '/Ajax/GetMessagesCount',
            type: 'GET',
            datatype: 'json',
            success: function (data) {
                if (data) {
                    var counter = $("#message-counter");
                    var data = parseInt(data);
                    if (data > 0) {
                        if (data <= 99) {
                            counter.html(data);
                        }
                        else {
                            counter.html('99+');
                        }
                    } else {
                        counter.html('');
                    }
                }
            }
        });
    }
}(jQuery));