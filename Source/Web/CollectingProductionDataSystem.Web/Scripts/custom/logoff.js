(function () {

    function addAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    }

    $(function () {
        var url = window.location.protocol + '//' + window.location.host + '/Account/LogOff';
        $('#logoff').bind("click", function () {
            $.connection.hub.stop();
            $.ajax({
                url: url,
                type: 'POST',
                data: addAntiForgeryToken({})
            }).done(function (data) {
                document.location.href = window.location.protocol + '//' + window.location.host + '/Home/Index';
            });
        });
    })
}());
