var endDate;
var beginDate;
var unitGridsData = (function () {
    //Exclusion list of grids without command button hiding
    var exclusionList = ["#measuringpoints",
                         "#highwaypipelines",
                         "#inner-pipes",
                         "#tanks-statuses",
                         "#messages-grid",
                         "#plan-grid"
    ];

    // ----------------- autorun function on document ready -------------------
    'use strict';
    $(document).ready(function () {
        var ctrlParamsElement = $('#control-params');
        if ($("#monthly-recalc-data-report").val() !== undefined) {
            if (ctrlParamsElement.val() !== undefined) {
                ctrlParamsElement.attr('data-params', JSON.stringify(sendDate()));
            }

            kendoAdditional.RefreshGrid("#monthly-recalc-data-report");
        }

        if ($("#plan-grid").val() !== undefined) {
            $('#confirm').remove();
        }
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

            if ($("#inner-pipes").val() !== undefined) {

                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(sendOnlyDate()));
                }

                kendoAdditional.RefreshGrid("#inner-pipes");
            }

            if ($("#monthly-hc-units").val() !== undefined) {

                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(sendDate()));
                }

                kendoAdditional.RefreshGrid("#monthly-hc-units");
            }

            if ($("#monthly-pw-units").val() !== undefined) {

                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(sendDate()));
                }

                kendoAdditional.RefreshGrid("#monthly-pw-units");
            }

            if ($("#monthly-recalc-data-report").val() !== undefined) {
                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(sendDate()));
                }

                kendoAdditional.RefreshGrid("#monthly-recalc-data-report");
            }

            if ($("#technological-data").val() !== undefined) {
                kendoAdditional.RefreshGrid("#technological-data");
            }


            if ($("#tanks-statuses").val() !== undefined) {

                if (ctrlParamsElement.val() !== undefined) {
                    ctrlParamsElement.attr('data-params', JSON.stringify(SendTanksData()));
                }

                kendoAdditional.RefreshGrid("#tanks-statuses");
            }

            if ($("#confirmation").val() !== undefined) {
                kendoAdditional.RefreshGrid("#confirmation");
            }

            if ($("#highwaypipelines").val() !== undefined) {

                kendoAdditional.RefreshGrid("#highwaypipelines");
            }

            if ($("#plan-grid").val() !== undefined) {
                kendoAdditional.RefreshGrid("#plan-grid");
            }

        });

        nameGridCommancolumn();
        hideCommandButtons();
        attachCallendatEvents();
        attachExcellExportToGrid();

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

                                var reportButton = $("#report");
                                if (reportButton) {
                                    reportButton.show();
                                }

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

        if ($("#report")) {
            $("#report").click(function () {
                if ($("#monthly-pw-units").val() !== undefined) {
                    if (ctrlParamsElement.val() !== undefined) {
                        ctrlParamsElement.attr('data-params', JSON.stringify(sendDate()));
                    }
                }

                if ($("#monthly-recalc-data-report").val() !== undefined) {
                    if (ctrlParamsElement.val() !== undefined) {
                        ctrlParamsElement.attr('data-params', JSON.stringify(sendDate()));
                    }
                }
            });
        }

    });

    //------------------ private functions ------------------------------------

    function getControlData() {
        var controlParams = $('#control-params');
        if (controlParams.val() !== undefined) {
            if (controlParams.attr('data-params') !== '') {
                return JSON.parse(controlParams.attr('data-params'));
            }
            else {
                return {};
            }

        } else {
            return {};
        }
    }

    function attachExcellExportToGrid() {
        var unitsGrid = $('#units').data('kendoGrid');
        if (unitsGrid) {
            attachEventToExportBtn("#excel-export", "#units");
        }

        var tanksGrid = $('#tanks').data('kendoGrid');
        if (tanksGrid) {
            attachEventToExportBtn("#excel-export", "#tanks");
        }

        var monthlyHCGrid = $('#monthly-hc-units').data('kendoGrid');
        if (monthlyHCGrid) {
            attachEventToExportBtn("#excel-export", "#monthly-hc-units");
        }

        var monthlyInnerPipelines = $('#inner-pipes').data('kendoGrid');
        if (monthlyInnerPipelines) {
            attachEventToExportBtn("#excel-export", "#inner-pipes");
        }

        var monthlyPWGrid = $('#monthly-pw-units').data('kendoGrid');
        if (monthlyPWGrid) {
            attachEventToExportBtn("#excel-export", "#monthly-pw-units");
        }

        var monthlyPWReportGrid = $('#monthly-recalc-data-report').data('kendoGrid');
        if (monthlyPWReportGrid) {
            attachEventToExportBtn("#excel-export", "#monthly-recalc-data-report");
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

    function sendMonthlyReportTypeId() {
        return { "monthlyReportTypeId": $('input#monthlyReportTypeId').val() }
    }

    function hideCommandButtons() {
        // skip if grid is excluded
        var IsExcluded = false
        exclusionList.forEach(function (value, index) {
            if ($(value).val() !== undefined) {
                IsExcluded = true;
            }
        });

        if (!IsExcluded) {
            var confirmButton = $("#confirm");
            if (confirmButton) {
                confirmButton.hide();
            }

            var unitsGrids = $(".k-widget.k-grid");
            Array.prototype.forEach.call(unitsGrids, function (unitsGrid) {
                unitsGrid = $(unitsGrid).data("kendoGrid");
                if (unitsGrid) {
                    unitsGrid.hideColumn('commands');
                }
            });

            var grid = $("#monthly-pw-units").data('kendoGrid');
            if (!(grid === null || grid === undefined)) {
                var count = grid.dataSource.total();
                var reportButton = $("#report");
                if (count === 0) {
                    if (reportButton) {
                        reportButton.hide();
                    }
                }
                else {
                    if (reportButton) {
                        reportButton.show();
                    }
                }
            }
        }
    }

    function showCommandButtons() {
        var confirmButton = $("#confirm");
        if (confirmButton) {
            confirmButton.show();
        }

        var reportButton = $("#report");
        if (reportButton) {
            reportButton.show();
        }

        var unitsGrids = $(".k-widget.k-grid");
        Array.prototype.forEach.call(unitsGrids, function (unitsGrid) {
            unitsGrid = $(unitsGrid).data("kendoGrid");
            if (unitsGrid) {
                unitsGrid.showColumn('commands');
            }
        });
    }

    function checkConfirmedStatus() {
        var grid = $(".k-widget.k-grid").data("kendoGrid");
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
        var grids = $(".k-widget.k-grid");
        Array.prototype.forEach.call(grids, function (grid) {
            grid = $(grid).data("kendoGrid");
            if (grid) {
                $.each(grid.columns, function (index, value) {
                    if (!this.field) {
                        this.field = "commands";
                    }
                });
            }
        });
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

    function attachCallendatEvents() {
        var ctrlParamsElement = $('#control-params');
        var dateElement = $('#date');
        if (dateElement.val() !== undefined) {
            var datePicker = dateElement.data('kendoDatePicker');
            if (datePicker !== undefined) {
                datePicker.bind("change", function () {
                    if ($("#inner-pipes").val() !== undefined) {

                        if (ctrlParamsElement.val() !== undefined) {
                            ctrlParamsElement.attr('data-params', JSON.stringify(sendOnlyDate()));
                        }

                        kendoAdditional.RefreshGrid("#inner-pipes");
                    }
                });
            }
        }
    }

    //------------------ public functions ------------------------------------

    function sendDate() {

        var datePicker = $('input[name=date]').data('kendoDatePicker');
        if (datePicker !== undefined) {
            var date = datePicker.value();
        } else {
            return;
        }

        var result = { "date": date.toISOString(kendo.culture().name) };
        $.extend(result, sendProcessUnit());
        if ($('input[name=shifts]')) {
            $.extend(result, sendShift());
        }
        if ($('input#materialTypeId')) {
            $.extend(result, sendMaterialTypeId());
        }
        if ($('input#monthlyReportTypeId')) {
            $.extend(result, sendMonthlyReportTypeId());
        }
        $.extend(result, sendFactoryId());
        $.extend(result, sendAntiForgery());
        return result;
    }

    function SendTanksData() {
        var result = { "date": $('input[name=date]').val() };
        $.extend(result, sendZoneId());
        if ($('input#shifts')) {
            $.extend(result, sendShift());
        }
        $.extend(result, sendAreaId());
        $.extend(result, sendAntiForgery());
        return result;
    }

    function sendOnlyDate() {
        var result = { "date": $('input[name=date]').val() };
        $.extend(result, sendAntiForgery());
        return result;
    }

    function sendMonth() {
        var result = $('input[name=date]').data('kendoDatePicker').value();
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

    function blinkDemo() {
        var firstRowUid = this.dataSource.data()[0].items[0].uid;
        $("#tanks" + " tbody").find("tr[data-uid=" + firstRowUid + "]").addClass("blink");
    }

    function DataBound() {
        var dataView = this.dataSource.view();

        //added
        var grid = this;
        var data = [].splice.call(dataView, [-1, 1]);
        [].forEach.call(data, function (value) {
            ConvertGridRows(value, grid);
        });

        if ($('#confirm').val() === "") {
            checkConfirmedStatus();
        }

        if ($("#productionPlan").val() !== undefined) {
            kendoAdditional.RefreshGrid("#productionPlan");
        }
    }

    function ConvertGridRows(dataCollection, grid) {
        if (dataCollection.items == undefined) {
            //stop recursion
            return true;
        } else {
            //go dipper
            $.each(dataCollection.items, function (key, value) {
                if (ConvertGridRows(value, grid) === true) {
                    modifyGridRow(value, grid);
                }
            })

            return false;
        }

    }

    function modifyGridRow(row, grid) {
        //make grid changes

        var manualIndicator = $('#manualIndicator').val();
        var manualCalcumated = $('#manualCalcumated').val();
        var manualSelfCalculated = $('#manualSelfCalculated').val();
        var currentUid, currenRow, editButton
        if (row) {
            if (!row.IsEditable) {
                currentUid = row.uid;
                currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                editButton = $(currenRow).find(".k-grid-edit");
                editButton.attr("style", "display:none !important");
            }
            if (row.HasManualData === true) {
                var uid = row.uid;
                $("#" + $(grid.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("bg-danger");
            }

            if (row.Status !== undefined) {
                if (row.Status === 1) {
                    var uid = row.uid;
                    $("#" + $(grid.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("warning-animation");
                }

                if (row.Status === 2) {
                    var uid = row.uid;
                    $("#" + $(grid.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("error-animation");
                }
            }


            if (row.UnitConfig) {
                if (row.UnitConfig.CollectingDataMechanism === manualIndicator) {
                    currentUid = row.uid;
                    currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                    editButton = $(currenRow).find(".k-grid-edit");
                    editButton.click({ data: row, url: "/ShiftReporting/Units/ShowManualDataModal" }, manualEntry);
                }

                if (row.UnitConfig.CollectingDataMechanism === manualCalcumated) {
                    currentUid = row.uid;
                    currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                    editButton = $(currenRow).find(".k-grid-edit");
                    editButton.click({ data: row, url: "/ShiftReporting/Units/ShowManualCalculatedDataModal" }, manualEntry);
                }

                if (row.UnitConfig.CollectingDataMechanism === manualSelfCalculated) {
                    currentUid = row.uid;
                    currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                    editButton = $(currenRow).find(".k-grid-edit");
                    editButton.click({ data: row, url: "/ShiftReporting/Units/ShowManualSelfCalculatedDataModal" }, manualEntry);
                }
            }

            if (row.IsTotalPosition && !row.IsTotalInputPosition) {
                currentUid = row.uid;
                currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
                var prevRow = $(currenRow).prev('tr');
                if (prevRow) {
                    if (prevRow.hasClass("k-grouping-row")) {
                        prevRow.remove();
                    }
                }
                $(currenRow).addClass("total");
                var groupCells = $(currenRow).find("td");
                $(groupCells[6]).attr("colspan", 5);
                $.each(groupCells, function (ix, cell) {
                    $(cell).removeClass("k-group-cell");
                    if (ix < 6) {
                        $(cell).attr("style", "display:none !important");
                    }
                });
            }

            //// monthly data total input and external rows
            //if (row.IsTotalInputPosition) {
            //    var uid = row.uid;
            //    $("#" + $(grid.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("total-input-position");
            //}

            //if (row.IsExternalOutputPosition) {
            //    var uid = row.uid;
            //    $("#" + $(grid.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("external-output-position");
            //}
        }
    }


    function DataSave(ev) {
        if (ev.type === 'update') {
            ev.sender.read();
            //kendoAdditional.RefreshGrid('#units');
            //kendoAdditional.RefreshGrid('#productionPlan');
        }

        if (ev.response !== undefined) {

            if (ev.type === 'read' && ev.response.Errors !== null) {
                ev.sender.data([]);
            }
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
        //page.options.set("pdf.paperSize", "A4");
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
        var result = { "date": kendo.parseDate($('input[name=date]').val()).toISOString() };
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

    function OnMonthlyExcelExport(ev) {
        var dataRows = ev.sender._data;
        var totalRowCodes = [];
        dataRows.forEach(function (value) {
            if (value.IsTotalPosition === true) {
                totalRowCodes.push(value.UnitMonthlyConfig.Code);
            }
        });
        var excelRows = ev.workbook.sheets[0].rows;
        excelRows.forEach(function (row, index, excelRows) {
            if (row.cells[2]) {
                if (isInArray(row.cells[2].value, totalRowCodes)) {
                    row.cells[0].value = row.cells[4].value;
                    row.cells[0].colSpan = 5;
                    row.cells.splice(1, 4);
                    if (excelRows[index - 1]) {
                        if (excelRows[index - 1].type === "group-header") {
                            excelRows[index - 1].markForDelete = true;
                        }
                    }
                    row.cells.forEach(function (cell) {
                        cell.background = "#7a7a7a";
                        cell.color = "#fff";
                    });
                }
            }
        });

        index = excelRows.length - 1;

        while (index >= 0) {
            if (excelRows[index].markForDelete) {
                excelRows.splice(index, 1);
            }

            index -= 1;
        }
    }

    function isInArray(value, array) {
        return array.indexOf(value) > -1;
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
        SendDateForSummaryReports: sendDateForSummaryReports,
        BlinkDemo: blinkDemo,
        OnMonthlyExcelExport: OnMonthlyExcelExport
    };
})();
