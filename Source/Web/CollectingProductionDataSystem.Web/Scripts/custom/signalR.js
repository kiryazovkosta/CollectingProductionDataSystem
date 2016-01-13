(function ($) {
    $(function () {
        
        // Proxy created on the fly
        var job = $.connection.messagesHub;

        // Declare a function on the job hub so the server can invoke it
        job.client.displayNewMessage = function (message) {
            displayMessage(message);
        }

        // Start the connection
        $.connection.hub.start();
        getMessagesCount();
    });

    function displayMessage(message) {
        var template = '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title popover-title-info"></h3><div class="popover-content"></div></div>';//info
        if (message.MessageType===2) {
            template = '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title popover-title-warning"></h3><div class="popover-content"></div></div>';//warning
        }
        if (message.MessageType===3) {
            template = '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title popover-title-danger"></h3><div class="popover-content"></div></div>';//danger
        }
        $('#messages').popover({
            'content': message.MessageText,
            'html':'true',
            'placement': 'bottom',
            'trigger': 'click',
            'template': template
        });
        $('#messages').popover('show');
        $('h3.popover-title').append('<span class="pull-right" aria-hidden="true">&times;</span>');

        $('.popover').off('click').on('click', function (e) {
            e.stopPropagation();
            $(e.currentTarget).popover('destroy');
        });

        getMessagesCount();
        if (typeof kendoAdditional !== 'undefined') {
            kendoAdditional.RefreshGrid('#messages-list');
        }
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