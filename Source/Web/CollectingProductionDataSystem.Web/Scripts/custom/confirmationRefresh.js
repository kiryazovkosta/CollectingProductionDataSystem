var refreshInterval
(function () {
     refreshInterval = setInterval(function () {
        $("#confirmation").data("kendoGrid").dataSource.read();
    }, 20000);
}());