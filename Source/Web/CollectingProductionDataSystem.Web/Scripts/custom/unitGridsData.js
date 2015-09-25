$(function () {
    var culture = $('#culture').val();
    kendo.culture(culture);
});

function sendProcessUnit() {
    return { "processUnitId": $('input[name=processunits]').val() }
}

function sendShift() {
    return { "shiftId": $('input[name=shifts]').val() }
}

var sendDate = function () {
    var result = { "date": $('input[name=date]').val() };
    $.extend(result, sendProcessUnit());
    if ($('input[name=shifts]')) {
        $.extend(result, sendShift());
    }
    $.extend(result, sendAntiForgery());
    return result;
}

function error_handler(e) {
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

function closeWindow(selector) {
    $(selector).data("kendoWindow").close();
}

function recordTimeStampFilter(element) {
    element.kendoDateTimePicker();
}

$(document).ready(function () {
    $("#apply").click(function () {
        refreshGrid("#units");
        refreshGrid("#productionPlan");
        checkConfirmedStatus();

        var grid = $('#units').data('kendoGrid');
        grid.dataSource.fetch(function () {
            var totalRows = grid.dataSource.total();
            if (totalRows === 0) {
                hideCommandButtons();
            }
        });
    });
    prepareWindow();
    prepareSuccessWindow();
    nameGridCommancolumn();
    hideCommandButtons();
    AttachEventToExportBtn("#excel-export", "#units");

    $("#confirm").click(function () {
        var date = sendDate();
        $.ajax({
            url: 'Confirm',
            type: 'POST',
            data: date,
            success: function (data) {
                var confirmed = data.IsConfirmed;
                if (confirmed === true) {
                    hideCommandButtons();
                    var message = "Вие потвърдихте отчета успешно."
                    $('pre#succ-message').text(message);
                    $('div#success-window').data("kendoWindow").open();

                } else {
                    if (data.errors) {
                        var errorMessage = "";
                        $.each(data.errors, function (key, value) {
                            if ('errors' in value) {
                                $.each(value.errors, function () {
                                    errorMessage += this + "\n";
                                });
                            }
                        });
                        $('pre#err-message').text(errorMessage);
                        $('div#err-window').data("kendoWindow").open();
                    }
                    showCommandButtons();
                }
            },
            error: function (data) {
                var errorMessage = "";
                var response = JSON.parse(data.responseText).data;
                $.each(response.errors, function (key, value) {
                    errorMessage += this + "\n";
                });
                $('pre#err-message').text(errorMessage);
                $('div#err-window').data("kendoWindow").open();
            }
        });
    });
});

function prepareWindow() {
    var window = $('div#err-window')
    window.kendoWindow({
        width: "650px",
        title: "Възникна грешка в приложението",
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

function prepareSuccessWindow() {
    var window = $('div#success-window')
    window.kendoWindow({
        width: "650px",
        title: "Успешна операция",
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

function hideCommandButtons() {
    $("#confirm").hide();
    $("#units").data("kendoGrid").hideColumn('commands');
}

function showCommandButtons() {
    $("#confirm").show();
    $("#units").data("kendoGrid").showColumn('commands');
}

function checkConfirmedStatus() {
    var date = sendDate();
    $.ajax({
        url: 'IsConfirmed',
        type: 'POST',
        data: date,
        success: function (result) {
            var confirmed = result.IsConfirmed;
            if (confirmed === false) {
                showCommandButtons();
            } else {
                hideCommandButtons();
            }
        },
        error: function (result) {
            hideCommandButtons();
        }
    });
}
function AttachEventToExportBtn(buttonSelector, targetSelector) {
    $(buttonSelector).click(function () {
        $(targetSelector).data("kendoGrid").saveAsExcel();
    });
}

var dataBound = function () {
    dataView = this.dataSource.view();
    for (var i = 0; i < dataView.length; i++) {
        for (var j = 0; j < dataView[i].items.length; j++) {

            if (dataView[i].items[j].HasManualData === true) {
                var uid = dataView[i].items[j].uid;
                $("#" + $(this.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("bg-danger");
            }
        }
    }
}

nameGridCommancolumn = function () {
    var grid = $("#units").data("kendoGrid");
    if (grid) {
        $.each(grid.columns, function (index, value) {
            if (!this.field) {
                this.field = "commands";
            }
        });
    }

}

var dataSave = function (ev) {
    if (ev.type == 'update') {
        refreshGrid('#unit');
        refreshGrid('#productionPlan');
    }
}

var refreshGrid = function (selector) {
    var grid = $(selector).data('kendoGrid');
    if (grid !== null) {
        grid.dataSource.read();
    }
}
