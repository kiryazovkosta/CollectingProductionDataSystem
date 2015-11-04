$(function () {
    var culture = $('#culture').val();
    kendo.culture(culture);
    prepareWindow('div#err-window', 'Възникна грешка в приложението');
    prepareWindow('div#success-window', 'Успешна операция');
});

function error_handler(e) {
    if (this.data) {
        this.data([]);
    }
    if (e.errors) {
        var message = "";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        $('pre#err-message').text(message);
        $('div#err-window').data("kendoWindow").open();
    }
}

function prepareWindow(selector, title) {

    var window = $(selector)
    if (window) {
        window.kendoWindow({
            width: "650px",
            title: title,
            actions: ["Close"],
            modal: true,
            position: {
                top: "30%", // or "100px"
                left: "30%"
            },
            minHeight: 100,
            maxHeight: 350
        });
    }
}

function closeWindow(selector) {
    $(selector).data("kendoWindow").close();
}

var refreshGrid = function (selector) {
    var grid = $(selector).data('kendoGrid');
    if (grid !== null) {
        grid.dataSource.read();
    }
}

function valueMapper(options) {
    var url = this.dataSource.options.transport.read.url.replace("Read", "ValueMapper");

    $.ajax({
        url: url,
        data: convertValues(options.value),
        success: function (data) {
            options.success(data);
        }
    });
}

function convertValues(value) {
    var data = {};

    value = $.isArray(value) ? value : [value];

    for (var idx = 0; idx < value.length; idx++) {
        data["values[" + idx + "]"] = value[idx];
    }

    return data;
}


function serialize(data) {
    for (var property in data) {
        if ($.isArray(data[property])) {
            serializeArray(property, data[property], data);
        }
    }
    $.extend(data, sendAntiForgery());
}

function serializeArray(prefix, array, result) {
    for (var i = 0; i < array.length; i++) {
        if ($.isPlainObject(array[i])) {
            for (var property in array[i]) {
                result[prefix + "[" + i + "]." + property] = array[i][property];
            }
        }
        else {
            result[prefix + "[" + i + "]"] = array[i];
        }
    }
}

function boundEmptyRelatedRecords() {
    var grid = this;
    var propertyName = "RelatedUnitConfigs";
    if (grid) {
        var records = grid._data;
        if (records) {
            var len = records.length;
            for (var i = 0; i < len; i++) {
                var element = records[i][propertyName];
                if (element.length == 0) {
                    element.push({ Id: 0, Code: null, Name: null });
                }
            }
        }
    }
}

function sendHistoryData() {
    var result = { 'id': $('input[name=id]').val(), 'entityName': $('input[name=entityName]').val() };
    $.extend(result, sendAntiForgery());
    return result;
}

var deletableDataBound = function () {
    var items = this.dataSource.view();
    for (var i = 0; i < items.length; i++) {
        if (items[i].IsDeleted) {
            var currentUid = items[i].uid;
            var currenRow = this.table.find("tr[data-uid='" + currentUid + "']");
            var editButton = $(currenRow).find(".k-grid-edit,.k-grid-delete");
            editButton.attr("style", "display:none !important");
            var cellsForColloring = $("#" + $(this.element).attr('id') + " tbody").find("tr[data-uid=" + currentUid + "] td");
            cellsForColloring.addClass("text-danger");
            cellsForColloring.find('a').addClass("text-warning");
        }
    }
}

var rowEdited = function (ev) {
    if (ev.model.UnitConfig.CollectingDataMechanism == "M") {
        ev.preventDefault();
        alert("Row Edited");
    }
}