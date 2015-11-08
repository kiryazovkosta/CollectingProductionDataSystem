var unitGridsData = (function () {

    // ----------------- autorun function on document ready -------------------
    $(document).ready(function () {
        $("#apply").click(function () {
            if ($("#units")) {
                kendoAdditional.RefreshGrid("#units");
            }
            if ($("#productionPlan")) {
                kendoAdditional.RefreshGrid("#productionPlan");
            }
            if ($("#tanks")) {
                kendoAdditional.RefreshGrid("#tanks");
            }

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
        attachEventToExportBtn("#excel-export", "#units");

        if ($("#confirm")) {
            $("#confirm").click(function () {
                var date = SendDate();
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
        }
    });

    //------------------ private functions ------------------------------------

    var sendProcessUnit = function () {
        return { "processUnitId": $('input[name=processunits]').val() };
    }

    var sendShift = function sendShift() {
        return { "shiftId": $('input[name=shifts]').val() }
    }

    var sendZoneId = function() {
        return { "parkId": $('input[name=parks]').val() }
    }

    var sendShiftsOffset = function() {
        return { "shiftMinutesOffset": $('input[name=shifts]').val() }
    }

    var hideCommandButtons = function () {
        if ($("#confirm")) {
            $("#confirm").hide();
        }
        if ($("#units")) {
            $("#units").data("kendoGrid").hideColumn('commands');
        }
    }

    var showCommandButtons = function () {
        $("#confirm").show();
        $("#units").data("kendoGrid").showColumn('commands');
    }

    var checkConfirmedStatus = function () {
        var date = SendDate();
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

    var attachEventToExportBtn = function (buttonSelector, targetSelector) {
        $(buttonSelector).click(function () {
            $(targetSelector).data("kendoGrid").saveAsExcel();
        });
    }

    var nameGridCommancolumn = function () {
        var grid = $("#units").data("kendoGrid");
        if (grid) {
            $.each(grid.columns, function (index, value) {
                if (!this.field) {
                    this.field = "commands";
                }
            });
        }
    }

    var addAntiForgeryToken = function (data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    var manualEntry = function (ev) {
        ev.preventDefault();
        ev.stopPropagation();
        var culture = $('#culture').val();
        $.ajax({
            url: ev.data.url,
            type: "POST",
            content: document.body,
            data: JSON.parse(JSON.stringify(addAntiForgeryToken(ev.data.data), replacer))
        }).done(function (data) {
            if (data.success === undefined) {
                $("#modal-dialog-body").html(data);
                prepareValidationScripts();
                showRecordHistoriModal();
            } else {
                if (data.success === false) {
                    manualEntryDoneErrorHandler(data.errors);
                }
            }
        }).fail(function (data) {
            var message = "Възникна грешка";
            if (data) {
                var status = data.status + 0;
                if (400 <= status && status < 500) {
                    message = "Не e намерена заявената страница";
                }
                if (status >= 500) {
                    message = "Възникна грешка при обработка на вашата заявка";
                }
            }
            showKendoError(message);
        });
    }

    var replacer = function replacer(key, value) {

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

    var showKendoError = function (message)
    {
        $('pre#err-message').text(message);
        $('div#err-window').data("kendoWindow").open();
    }

    var manualEntryDoneErrorHandler = function (errors) {
        if (errors) {
            var message = "";
            $.each(errors, function (key, value) {
                message += value.ErrorMessage + "\n";
            });
            $('pre#err-message').text(message);
            $('div#err-window').data("kendoWindow").open();
        }
    }

    var prepareValidationScripts = function () {
        var form = $('#modal-dialog-body');
        if (form.executed)
            return;

        form.removeData("validator");
        $.validator.unobtrusive.parse(document);
        form.executed = true;
    }

    //------------------ public functions ------------------------------------

    var SendDate = function () {
        var result = { "date": $('input[name=date]').val() };
        $.extend(result, sendProcessUnit());
        if ($('input[name=shifts]')) {
            $.extend(result, sendShift());
        }
        $.extend(result, sendAntiForgery());
        return result;
    }

    var SendTanksData = function () {
        var result = { "date": $('input[name=date]').val() };
        $.extend(result, sendZoneId());
        $.extend(result, sendShift());
        $.extend(result, sendAntiForgery());
        return result;
    }

    var DataBound = function () {
        var dataView = this.dataSource.view();
        var manualIndicator = $('#manualIndicator').val();
        var manualCalcumated = $('#manualCalcumated').val();
        var manualSelfCalculated = $('#manualSelfCalculated').val();
        var i, j;
        var currentUid, currenRow, editButton
        for (i = 0; i < dataView.length; i += 1) {
            if (dataView[i].items) {
                var recordLen = dataView[i].items.length;
                if (recordLen) {
                    for (j = 0; j < recordLen; j += 1) {
                        if (!dataView[i].items[j].IsEditable) {
                            currentUid = dataView[i].items[j].uid;
                            currenRow = this.table.find("tr[data-uid='" + currentUid + "']");
                            editButton = $(currenRow).find(".k-grid-edit");
                            editButton.attr("style", "display:none !important");
                        }
                        if (dataView[i].items[j].HasManualData === true) {
                            var uid = dataView[i].items[j].uid;
                            $("#" + $(this.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("bg-danger");
                        }

                        if (dataView[i].items[j].UnitConfig.CollectingDataMechanism === manualIndicator) {
                            currentUid = dataView[i].items[j].uid;
                            currenRow = this.table.find("tr[data-uid='" + currentUid + "']");
                            editButton = $(currenRow).find(".k-grid-edit");
                            editButton.click({ data: dataView[i].items[j], url: "/ShiftReporting/Units/ShowManualDataModal" }, manualEntry);
                        }

                        if (dataView[i].items[j].UnitConfig.CollectingDataMechanism === manualCalcumated) {
                            currentUid = dataView[i].items[j].uid;
                            currenRow = this.table.find("tr[data-uid='" + currentUid + "']");
                            editButton = $(currenRow).find(".k-grid-edit");
                            editButton.click({ data: dataView[i].items[j], url: "/ShiftReporting/Units/ShowManualCalculatedDataModal" }, manualEntry);
                        }

                        if (dataView[i].items[j].UnitConfig.CollectingDataMechanism === manualSelfCalculated) {
                            currentUid = dataView[i].items[j].uid;
                            currenRow = this.table.find("tr[data-uid='" + currentUid + "']");
                            editButton = $(currenRow).find(".k-grid-edit");
                            editButton.click({ data: dataView[i].items[j], url: "/ShiftReporting/Units/ShowManualSelfCalculatedDataModal" }, manualEntry);
                        }
                    }
                }
            }
        }
    }

    var DataSave = function (ev) {
        if (ev.type === 'update') {
            kendoAdditional.RefreshGrid('#units');
            kendoAdditional.RefreshGrid('#productionPlan');
        }
    }

    var ManualEntryFailure = function (data) {

        showKendoError("Възникна грешка при обработка на вашата заявка");
    }

    var SuccessCalculateManualEntry = function (data) {
        if (data) {
            if (data == "success") {
                hideRecordHistoriModal();
                kendoAdditional.RefreshGrid('#units');
            }
        }
    }

    return {
        SendDate: SendDate,
        SendTanksData: SendTanksData,
        DataBound: DataBound,
        DataSave: DataSave,
        ManualEntryFailure: ManualEntryFailure,
        SuccessCalculateManualEntry: SuccessCalculateManualEntry,
    };
})();
