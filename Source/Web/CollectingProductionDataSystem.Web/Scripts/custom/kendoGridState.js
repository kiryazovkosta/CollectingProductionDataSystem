﻿(function () {

    function addAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    $(document).ready(function () {
        var gridElement = $(".k-widget.k-grid");
        if (gridElement) {
            var grid = gridElement.data("kendoGrid");

            var dataSource = grid.dataSource;

            var state = localStorage.getItem(gridElement.attr("id")+"_"+ $("#user").val());
            if (state) {
                state = JSON.parse(state);
                grid.setOptions(state.options);
                //grid.destroy();

                //gridElement
                //   .empty()
                //   .kendoGrid(options);

                $(".k-grid-content").css("height", state.height);
            }
        }
    });

    $(window).unload(function () {
        var gridElement = $(".k-widget.k-grid");
        if (gridElement) {
            var grid = gridElement.data("kendoGrid");

            var state = {
                options: grid.getOptions(),
                height: $(".k-grid-content").css("height")
            };
            localStorage.setItem(gridElement.attr("id") + "_"+$("#user").val(), JSON.stringify(state))
        }
    });
})();