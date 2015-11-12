var uploadNom = (function () {
    $(function () { $('#close').click(function () { $('#alert').hide(); }) });
    function OnSuccess(ev) {
        var result = ev.response;
        if (result.IsValid == true) {
            var message = "Успешно създадохте " + result.ResultRecordsCount + " записа.";
            showAlert('#alert', 'alert-success', message);
        }
    }
    function OnError(ev) {
        alert("error");
    }

    function showAlert(id, type, message) {
        var alert = $(id);
        var classes = ['alert-success', 'alert-warning', 'alert-danger'];
        alert.find('p').text(message);
        $.each(classes, function (index, value) { alert.removeClass(value); });
        alert.addClass(type);
        alert.show();
    }

    return {
        OnSuccess: OnSuccess,
        OnError: OnError,
    };
})();