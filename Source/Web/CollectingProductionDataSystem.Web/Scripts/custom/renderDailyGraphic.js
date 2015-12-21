(function ($) {
    $(document).ready(function () {
        $("#apply").click(function () {
            var value = $('input[name=processunits]').val() || $('input[name=processunitsD]').val();
            var date = kendo.format("{0:u}", $('#date').data('kendoDatePicker').value());
            if (value) {
                $.ajax({
                    url: '/DailyReporting/UnitsDaily/DailyChart',
                    method: 'GET',
                    data: { 'processUnitId': value, 'date': date || Date() }
                }).done(function (data) {
                    $('#chart-wrapper').html(data);
                });
            }
        });
    });
}(jQuery));
