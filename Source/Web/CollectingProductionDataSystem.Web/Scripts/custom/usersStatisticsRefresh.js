(function () {
    var refreshInterval = setInterval(function () {
        $("#users").data("kendoGrid").dataSource.read()
        $("#logedin-users").data("kendoChart").dataSource.read();
    }, 10000);
}());