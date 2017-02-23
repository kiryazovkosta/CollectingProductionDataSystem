(function () {
    sendFactory = function () {
        var result = sendDate();
        result.factoryId = $('#factories').val() || $('#factoriesD').val();
        return { "date": result.date, "factoryId": result.factoryId }
    }

    sendDate = function (selector) {
        if (!selector||!selector.length) {
            selector = $("input[name=date]");    
        }
        
        var result = {};
        result.date = new Date().toISOString(kendo.culture().name);
        var datePicker = selector.data('kendoDatePicker');
        if (datePicker !== undefined && datePicker !== null) {
            var date = datePicker.value();
            result.date = date.toISOString(kendo.culture().name);
        } else {
            var dateString = kendo.parseDate(selector.val(), 'dd.MM.yyyy г.', kendo.culture().name);
            if (dateString) {
                result.date = dateString.toISOString(kendo.culture().name) || result.date;
            } else {
                var dateSelector = selector;
                if (dateSelector.len > 0) {    
                    var months = {
                        'януари': 1,
                        'февруари': 2,
                        'март': 3,
                        'април': 4,
                        'май': 5,
                        'юни': 6,
                        'юли': 7,
                        'август': 8,
                        'септември': 9,
                        'октомври': 10,
                        'ноември': 11,
                        'декември': 12
                    };
                    var date_split =dateSelector.val().split(" ");
                    result.date = new Date(parseInt(date_split[1]), months[date_split[0]], 1).toISOString(kendo.culture().name);
                }
            }
        }

        return result;
    }

    return {
        SendFactory: sendFactory,
        SendDate: sendDate
    }
}());
