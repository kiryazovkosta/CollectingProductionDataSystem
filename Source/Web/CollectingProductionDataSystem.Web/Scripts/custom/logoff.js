(function () {
    $(function () {
        $('#logoff').bind("click", function () {
            $.connection.hub.stop();
            document.getElementById('logoutForm').submit();
        });
    })
}());