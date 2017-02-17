(function () {
    sendFactory = function () {
        var result = sendDate();
        result.factoryId = $('#factories').val() || $('#factoriesD').val();
        return { "date": result.date, "factoryId": result.factoryId }
    }

    sendDate = function () {
        var result = {};
        result.date = new Date().toISOString(kendo.culture().name);
        var datePicker = $('input[name=date]').data('kendoDatePicker');
        if (datePicker !== undefined && datePicker !== null) {
            var date = datePicker.value();
            result.date = date.toISOString(kendo.culture().name);
        } else {
            result.date = kendo.parseDate($("input[name=date]").val(), 'dd.MM.yyyy г.', kendo.culture().name).toISOString(kendo.culture().name)
                ||result.date;
        }

        return result;
    }

    return {
        SendFactory: sendFactory,
        SendDate: sendDate
    }
}());
