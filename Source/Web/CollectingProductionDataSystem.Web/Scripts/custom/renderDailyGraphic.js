(function ($) {
    $(document).ready(function () {
        $("#apply").click(function () {
            var value = $('input[name=processunits]').val() || $('input[name=processunitsD]').val();
            if (value) {
                $.ajax({
                    url: '/DailyReporting/UnitsDaily/DailyChart',
                    method: 'GET',
                    data:{'processUnitId':value}
                }).done(function (data) {
                    $('#chart-wrapper').html(data);
                });
            }
        });
    });
}(jQuery));