var uploadNom = (function () {
    $(function () {
        $('#close').click(function () {
            $('#alert').hide();
        })
    });
    function OnSuccess(ev) {
        var result = ev.response;
        if (result.IsValid == true) {
            var message = "Успешно създадохте " + result.ResultRecordsCount + " записа.";
            $('pre#succ-message').text(message);
            $('div#success-window').data("kendoWindow").open();
        }
    }
    function OnError(ev) {
        var message = ev.XMLHttpRequest.responseText;
        $('pre#err-message').text(message);
        $('div#err-window').data("kendoWindow").open();
    }

    return {
        OnSuccess: OnSuccess,
        OnError: OnError,
    };
})();