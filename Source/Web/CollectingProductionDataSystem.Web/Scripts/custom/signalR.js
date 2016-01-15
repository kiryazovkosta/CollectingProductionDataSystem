(function ($) {
    $(function () {
        
        // Proxy created on the fly
        var job = $.connection.messagesHub;

        // Declare a function on the job hub so the server can invoke it
        job.client.displayNewMessage = function (message) {
            displayMessage(message);
        }

        job.client.getMessagesCount = function ()
        {
            getMessagesCount();
        }

        // Start the connection
        $.connection.hub.start({ transport: ['webSockets', 'serverSentEvents', 'longPolling'] });
        getMessagesCount();
    });

    function displayMessage(message) {
        var template = '<div class="popover"><div class="arrow"></div><div class="popover-icon col-xs-2"><img src="/Content/Images/PNG/64x64/Info.png" alt="Info" width="55px" height="55px"></div><pre class="popover-content col-xs-10"></pre></div>';//info
        if (message.MessageType===2) {
            template = '<div class="popover"><div class="arrow"></div><div class="popover-icon col-xs-2"><img src="/Content/Images/PNG/64x64/Warning.png" alt="Warning" width="55px" height="55px"></div><pre class="popover-content col-xs-10"></pre></div>';//warning
        }
        if (message.MessageType===3) {
            template = '<div class="popover"><div class="arrow"></div><div class="popover-icon col-xs-2"><img src="/Content/Images/PNG/64x64/ErrorCircle.png" alt="Critical" width="55px" height="55px"></div><pre class="popover-content col-xs-10"></pre></div>';//danger
        }

        var existPopup = $('#messages').data('bs.popover');
        if (typeof existPopup !== 'undefined') {
            changePopoverIcon(existPopup, message.MessageType);
            existPopup.options.content = message.MessageText;
            existPopup.setContent();
            existPopup.show();
            return;
        }
        
        $('#messages').popover({
            'content': message.MessageText,
            'html':'true',
            'placement': 'bottom',
            'trigger': 'manual',
            'template': template
        });
        $('#messages').popover('show');

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
            cache: false,
            datatype: 'json',
            success: function (data) {
                if (typeof data !=='undefined') {
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

    function changePopoverIcon(popover, messageType)
    {
        var iconSrc = '';
        switch (messageType) {
            case 2:
                iconSrc = "/Content/Images/PNG/64x64/Warning.png";
                break;
            case 3:
                iconSrc = "/Content/Images/PNG/64x64/ErrorCircle.png";
                break;
            default:
                iconSrc = "/Content/Images/PNG/64x64/Info.png";

        }
        var selector = '#' + popover.$tip.attr('id');
        var img = $(selector).find('img');
        img.attr('src', iconSrc);
    }

}(jQuery));