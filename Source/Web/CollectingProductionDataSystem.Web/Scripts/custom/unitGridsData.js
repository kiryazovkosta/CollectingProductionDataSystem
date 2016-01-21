var endDate;
var beginDate;
var unitGridsData = (function () {

    // ----------------- autorun function on document ready -------------------
    'use strict';
    $(document).ready(function () {
        var ctrlParamsElement = $('#control-params');
        $("#apply").click(function () {
            if ($("#units").val() !== undefined) {

                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(sendDate()));
                }

                kendoAdditional.RefreshGrid("#units");
            }

           

            if ($("#tanks").val() !== undefined) {

                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(SendTanksData()));
                }

                kendoAdditional.RefreshGrid("#tanks");
            }

            if ($("#measuringpoints").val() !== undefined) {

                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(sendOnlyDate()));
                }

                kendoAdditional.RefreshGrid("#measuringpoints");
            }

            if ($("#confirmation").val() !== undefined) {
                kendoAdditional.RefreshGrid("#confirmation");
            }

        });

        nameGridCommancolumn();
        hideCommandButtons();
        var unitsGrid = $('#units').data('kendoGrid');
        if (unitsGrid) {
            attachEventToExportBtn("#excel-export", "#units");
        }

        var tanksGrid = $('#tanks').data('kendoGrid');
        if (tanksGrid) {
            attachEventToExportBtn("#excel-export", "#tanks");
        }

        var summaryGrid = $('#confirmation').data('kendoGrid');
        if (summaryGrid) {
            $('#pdf-export').click(function (ev) {
                var pageSize = summaryGrid.dataSource.pageSize();
                summaryGrid.dataSource.pageSize(9);
                var timeout = setTimeout(function () {
                    clearInterval(refreshInterval);
                    clearTimeout(timeout);
                    summaryGrid.saveAsPDF().done(function (e) {
                        summaryGrid.dataSource.pageSize(pageSize);
                        refreshInterval = setInterval(function () {
                            $("#confirmation").data("kendoGrid").dataSource.read();
                        }, 20000);
                    });
                }, 1000);
            });
        }

        if ($("#confirm")) {
            $("#confirm").click(function () {
                var dataParam = sendDate();
                var controlData = getControlData();
                var differences = checkEquals(dataParam, controlData)
                if (differences.length === 0) {
                    $.ajax({
                        url: 'Confirm',
                        type: 'POST',
                        data: dataParam,
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
                } else {
                    var errorMessage = "Има разлика между параметрите:\n";
                    $.each(differences, function (key, value) {
                        errorMessage += "\t\t -" + value + "\n";
                    });
                    errorMessage += "за които е генериран отчета и тези, които се опитвате да потвърдите!"
                    $('pre#err-message').text(errorMessage);
                    $('div#err-window').data("kendoWindow").open();
                }
            });
        }
    });

    //------------------ private functions ------------------------------------

    function getControlData() {
        var controlParams = $('#control-params');
        if (controlParams.val() !== undefined) {
            return JSON.parse(controlParams.attr('data-params'));
        } else {
            return {};
        }

    }

    function checkEquals(dataParam, controlData) {
        var result = [];
        for (var d in dataParam) {
            if ((dataParam[d] || 0) !== (controlData[d] || 0)) {
                var selector = d.replace('Id', 's').toLowerCase();
                if (selector.charAt(selector.length - 2) === 'y') {
                    selector = selector.slice(0, selector.length - 2) + 'ies';
                }
                var fieldName = $('label[for=' + selector + ']').text() || d.replace('Id', 's');
                result.push(fieldName);
            }
        }

        return result;
    }

    function sendProcessUnit() {
        var value = $('input[name=processunits]').val() || $('input[name=processunitsD]').val();
        if (value) {
            return { "processUnitId": value };
        }
    }

    function sendShift() {
        return { "shiftId": $('input[name=shifts]').val() }
    }

    function sendZoneId() {
        return { "parkId": $('input[name=parks]').val() }
    }

    function sendAreaId() {
        return { "areaId": $('input[name=areas]').val() }
    }

    function sendFactoryId() {
        return { "factoryId": $('input[name=factories]').val() || $('input[name=factoriesD]').val() }
    }

    function sendShiftsOffset() {
        return { "shiftMinutesOffset": $('input[name=shifts]').val() }
    }

    function sendMaterialTypeId() {
        return { "materialTypeId": $('input#materialTypeId').val() }
    }

    function hideCommandButtons() {
        var confirmButton = $("#confirm");
        if (confirmButton) {
            confirmButton.hide();
        }
        var unitsGrid = $("#units").data("kendoGrid");
        if (unitsGrid) {
            unitsGrid.hideColumn('commands');
        }
    }

    function showCommandButtons() {
        var confirmButton = $("#confirm");
        if (confirmButton) {
            confirmButton.show();
        }

        var unitsGrid = $("#units").data("kendoGrid");
        if (unitsGrid) {
            unitsGrid.showColumn('commands');
        }
    }

    function checkConfirmedStatus() {
        var grid = $("#units").data("kendoGrid");
        if (grid) {
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
    }

    function attachEventToExportBtn(buttonSelector, targetSelector) {
        $(buttonSelector).click(function () {
            $(targetSelector).data("kendoGrid").saveAsExcel();
        });

        $(targetSelector).data("kendoGrid").bind("excelExport", function (e) {
            // e.workbook.fileName = "Grid.xlsx";
        });
    }


    function nameGridCommancolumn() {
        var grid = $("#units").data("kendoGrid");
        if (grid) {
            $.each(grid.columns, function (index, value) {
                if (!this.field) {
                    this.field = "commands";
                }
            });
        }
    }

    function addAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };

    function manualEntry(ev) {
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
                showRecordHistoriModal();
                prepareValidationScripts();
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

    function showKendoError(message) {
        $('pre#err-message').text(message);
        $('div#err-window').data("kendoWindow").open();
    }

    function manualEntryDoneErrorHandler(errors) {
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

    function sendDate() {
        var result = { "date": $('input[name=date]').val() };
        $.extend(result, sendProcessUnit());
        if ($('input[name=shifts]')) {
            $.extend(result, sendShift());
        }
        if ($('input#materialTypeId')) {
            $.extend(result, sendMaterialTypeId());
        }
        $.extend(result, sendFactoryId());
        $.extend(result, sendAntiForgery());
        return result;
    }

    function SendTanksData() {
        var result = { "date": $('input[name=date]').val() };
        $.extend(result, sendZoneId());
        $.extend(result, sendShift());
        $.extend(result, sendAreaId());
        $.extend(result, sendAntiForgery());
        return result;
    }

    function sendOnlyDate() {
        var result = { "date": $('input[name=date]').val() };
        $.extend(result, sendAntiForgery());
        return result;
    }

    function ConfirmationDataBound() {
        endDate = kendo.parseDate($('input[name=date]').val());
        beginDate = kendo.parseDate($('input[name=date]').val());
        beginDate.setDate(1);
        $.each($(".calendar"), function (index, element) {

            var dates = JSON.parse($(element).attr('data-json'), function (key, val) {
                if (key === 'Day') {
                    return kendo.parseDate(val);
                } else {
                    return val;
                }
            });

            //construct calendar
            $(element).kendoCalendar({
                value: endDate,
                dates: dates,
                month: {
                    content:
                    ' <div class="' + '# if (beginDate <= data.date && data.date <= endDate) { #'
                                            + '#   var daily = $.grep(dates, function(e){ return e.Day.getDate() === data.date.getDate(); });#'
                                            + '#if(daily[0] != undefined){#'
                                            + '#if( daily[0].IsConfirmed){#'
                                                + "small-green-light"
                                                + '#}else{#'
                                                + "small-red-light"
                                                + '#}#'
                                            + '#}else{#'
                                            + "small-red-light"
                                            + "# }#"
                                            + "#}#"
                                            + '">#= data.value #</div>'
                },
                footer: false,
                header: false,
            });
        });

        $('div.k-calendar table.k-content tbody tr td a.k-link').attr("style", "text-align:center;");
        $('div.k-calendar div.k-header').attr("style", "display:none;")
    }

    function DataBound() {
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

                        if (dataView[i].items[j].UnitConfig) {
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

        if ($('#confirm').val() === "") {
            checkConfirmedStatus();
        }

        if ($("#productionPlan").val() !== undefined) {
            kendoAdditional.RefreshGrid("#productionPlan");
        }

    }

    function DataSave(ev) {
        if (ev.type === 'update') {
            kendoAdditional.RefreshGrid('#units');
            //kendoAdditional.RefreshGrid('#productionPlan');
        }
    }

    function ManualEntryFailure(data) {

        showKendoError("Възникна грешка при обработка на вашата заявка");
    }

    function SuccessCalculateManualEntry(data) {
        if (data) {
            if (data == "success") {
                hideRecordHistoriModal();
                kendoAdditional.RefreshGrid('#units');
            }
        }
    }

    function FormatGridToPdfExport(e) {
        e.promise.progress(function (e) {
            e.page = formatPage(e);
        });
    }

    // ----------------- pdf page setup Begin

    // Import Drawing API namespaces
    var draw = kendo.drawing;
    var geom = kendo.geometry;

    // See
    // http://docs.telerik.com/kendo-ui/framework/drawing/drawing-dom#dimensions-and-css-units-for-pdf-output
    function mm(val) {
        return val * 2.8347;
    }

    // A4 Sheet with 1 cm borders
    var PAGE_RECT = new geom.Rect(
      [mm(0), 0], [mm(210 - 20), mm(297 - 20)]
    );

    // Spacing between header, content and footer
    var LINE_SPACING = mm(5);

    function formatPage(e) {
        var header = createHeader();
        var content = e.page;
        var footer = createFooter(e.pageNumber, e.totalPages);

        // Remove header, footer and spacers from the page size
        var contentRect = PAGE_RECT.clone();
        contentRect.size.height -= header.bbox().height() + footer.bbox().height() + 2 * LINE_SPACING;

        // Fit the content in the available space
        draw.fit(content, contentRect)

        // Do a final layout with content
        var page = new draw.Layout(PAGE_RECT, {
            // "Rows" go below each other
            orientation: "vertical",

            // Center rows relative to each other
            alignItems: "center",

            // Center the content block horizontally
            alignContent: "center",

            // Leave spacing between rows
            spacing: LINE_SPACING
        });
        page.append(header, content);
        page.reflow();
        draw.align([header], PAGE_RECT, "start");

        // Move the footer to the bottom-right corner
        page.append(footer);
        draw.vAlign([footer], PAGE_RECT, "end");
        draw.align([footer], PAGE_RECT, "end");

        return page;
    }

    function createHeader() {
        return new kendo.drawing.Text("        " + $('#confirmation').data('kendoGrid').options.pdf.title, [0, 0], {
            font: mm(5) + "px 'DejaVu Sans'"
        });
    }

    function createFooter(page, total) {
        return new kendo.drawing.Text(
          kendo.format("{0} от {1}", page, total),
          [0, 0], {
              font: mm(3) + "px 'DejaVu Sans'"
          }
        );
    }

    function sendDateForSummaryReports() {
        var result = { "date": kendo.parseDate($('input[name=date]').val()).toISOString()};
        $.extend(result, sendProcessUnit());
        if ($('input[name=shifts]')) {
            $.extend(result, sendShift());
        }
        if ($('input#materialTypeId')) {
            $.extend(result, sendMaterialTypeId());
        }
        $.extend(result, sendFactoryId());
        $.extend(result, sendAntiForgery());
        return result;
    }

    return {
        SendDate: sendDate,
        SendTanksData: SendTanksData,
        DataBound: DataBound,
        DataSave: DataSave,
        ManualEntryFailure: ManualEntryFailure,
        SuccessCalculateManualEntry: SuccessCalculateManualEntry,
        ConfirmationDataBound: ConfirmationDataBound,
        FormatGridToPdfExport: FormatGridToPdfExport,
        SendDateForSummaryReports: sendDateForSummaryReports
    };
})();
