(function () {

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

                var options = grid.options;
                options.columns = state.columns;
                options.dataSource.pageSize = state.pageSize;
                options.dataSource.sort = state.sort;
                options.dataSource.filter = state.filter;
                options.dataSource.group = state.group;
                

                grid.destroy();

                gridElement
                   .empty()
                   .kendoGrid(options);

                $(".k-grid-content").css("height", state.height);
            }
        }
    });

    $(window).unload(function () {
        var gridElement = $(".k-widget.k-grid");
        if (gridElement) {
            var grid = gridElement.data("kendoGrid");

            var dataSource = grid.dataSource;

            var state = {
                columns: grid.columns,
                pageSize: dataSource.pageSize(),
                sort: dataSource.sort(),
                filter: dataSource.filter(),
                group: dataSource.group(),
                height: $(".k-grid-content").css("height")
            };
            localStorage.setItem(gridElement.attr("id") + "_"+$("#user").val(), JSON.stringify(state))
        }
    });
})();