function sendProcessUnit() {
    return { "processUnitId": $('input[name=processunits]').val() };
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
    var dataView = this.dataSource.view();
    var manualIndicator = $('#manualIndicator').val();
    var manualCalcumated = $('#manualCalcumated').val();
    var manualSelfCalculated = $('#manualSelfCalculated').val();

    for (var i = 0; i < dataView.length; i++) {
        if (dataView[i].items) {
            var recordLen = dataView[i].items.length;
            if (recordLen) {
                for (var j = 0; j < recordLen; j++) {
                    if (!dataView[i].items[j].IsEditable) {
                        var currentUid = dataView[i].items[j].uid;
                        var currenRow = this.table.find("tr[data-uid='" + currentUid + "']");
                        var editButton = $(currenRow).find(".k-grid-edit");
                        editButton.attr("style", "display:none !important");
                    }
                    if (dataView[i].items[j].HasManualData === true) {
                        var uid = dataView[i].items[j].uid;
                        $("#" + $(this.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("bg-danger");
                    }

                    if (dataView[i].items[j].UnitConfig.CollectingDataMechanism == manualIndicator) {
                        var currentUid = dataView[i].items[j].uid;
                        var currenRow = this.table.find("tr[data-uid='" + currentUid + "']");
                        var editButton = $(currenRow).find(".k-grid-edit");
                        editButton.click(dataView[i].items[j], manualEntry);
                    }




                }
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
    if (ev.type === 'update') {
        refreshGrid('#units');
        refreshGrid('#productionPlan');
    }
}

var AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

var manualEntry = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();
    var culture = $('#culture').val();
    $.ajax({
        url: "/ShiftReporting/Units/ShowManualDataModal",
        type: "POST",
        content: document.body,
        data: JSON.parse(JSON.stringify(AddAntiForgeryToken(ev.data), replacer))
    }).done(function (data) {
        if (data.success === undefined) {
            $("#modal-dialog-body").html(data);
            PrepareValidationScripts();
            showRecordHistoriModal();
        } else {
            if (data.success == false) {
                manual_error_handler(data.errors);
            }
        }
    }).fail(function (data) {
        alert("Error!!!");
    });
}

function replacer(key, value) {

    if (value instanceof Date) {
        return value.toLocaleString(culture.value);
    }

    if (value === parseInt(value)) {
        return value.toString().replace(' ', '');
    }

    if (value === parseFloat(value)) {
        return value.toLocaleString(culture.value).replace(/\s/g, "");
    }

    return value;
}

var manualEntryFailure = function (data) {
    alert("Error");
}

var successCalculateManualEntry = function (data) {
    if (data) {
        if (data == "success") {
            hideRecordHistoriModal();
            refreshGrid('#units');
        }
    }
}

function manual_error_handler(errors) {
    if (errors) {
        var message = "";
        $.each(errors, function (key, value) {
            message += value.ErrorMessage + "\n";
        });
        $('pre#err-message').text(message);
        $('div#err-window').data("kendoWindow").open();
    }
}

function PrepareValidationScripts() {
    var form = $('#modal-dialog-body');
    if (form.executed)
        return;

    form.removeData("validator");
    $.validator.unobtrusive.parse(document);
    form.executed = true;
}


