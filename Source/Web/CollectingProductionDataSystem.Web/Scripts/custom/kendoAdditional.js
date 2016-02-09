﻿var kendoAdditional = (function () {
    // ----------------- autorun function on document ready -------------------
    'use strict';
    $(function () {
        var culture = $('#culture').val();
        kendo.culture(culture);
        prepareWindow('div#err-window', 'Възникна грешка в приложението');
        prepareWindow('div#success-window', 'Успешна операция');
    });

    //------------------ private functions ------------------------------------

    function prepareWindow(selector, title) {

        var window = $(selector);
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

    function convertValues(value) {
        var data = {};
        value = $.isArray(value) ? value : [value];
        value.forEach(function (element, index, array) {
            data["values[" + index + "]"] = element;
        });
        return data;
    }

    function serialize(data) {
        var dataKeys = Object.keys(data);
        dataKeys.forEach(function (element, index, array) {
            if ($.isArray(element)) {
                serializeArray(property, element, array);
            }
        });
        $.extend(data, sendAntiForgery());
    }

    function serializeArray(prefix, array, result) {
        array = $.isArray(array) ? array : [array];
        array.forEach(function (element, index, array) {
            if ($.isPlainObject(element)) {
                var elementKeys = Object.keys(element);
                elementKeys.forEach(function (property, ix, arr) {
                    result[prefix + "[" + index + "]." + property] = element[property];
                });
            } else {
                result[prefix + "[" + index + "]"] = element;
            }
        });

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

    //------------------ public functions ------------------------------------

    function ErrorHandler(e) {
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

        // Cancel the changes
        var grid = $("#grid").data("kendoGrid");
        grid.cancelChanges();

    }

    function CloseWindow(selector) {
        $(selector).data("kendoWindow").close();
    }

    function RefreshGrid(selector) {
        var grid = $(selector).data('kendoGrid');
        if (grid !== null) {
            grid.dataSource.read();
        }
    }

    function ValueMapper(options) {

        var splitUrl = this.dataSource.options.transport.read.url.split('/');
        var actionName = splitUrl[splitUrl.length - 1];
        var url = this.dataSource.options.transport.read.url.replace(actionName, "ValueMapper");

        $.ajax({
            url: url,
            data: convertValues(options.value),
            success: function (data) {
                options.success(data);
            }
        });
    }


    var SendHistoryData = function () {
        var result = { 'id': $('input[name=id]').val(), 'entityName': $('input[name=entityName]').val() };
        $.extend(result, sendAntiForgery());
        return result;
    }

    function DeletableDataBound() {
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

    function afterterNomGridValidation(ev) {
        if (ev.type === 'update') {
            RefreshGrid('#grid');
        }
    }

    function messageBound() {
        var dataView = this.dataSource.view();
        var sender = this;
        $.each(dataView, function (key, row) {
            var uid = row.uid;
            if (row.MessageType === 1) {
                $("#" + $(sender.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("bg-info");
            }
            if (row.MessageType === 2) {
                $("#" + $(sender.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("bg-warning");
            }
            if (row.MessageType === 3) {
                $("#" + $(sender.element).attr('id') + " tbody").find("tr[data-uid=" + uid + "]").addClass("bg-danger");
            }
        });
    }

    function MessagesDataBound(ev) {
        var dataView = this.dataSource.view();
        var table = ev.sender.element;
        if (dataView.length > 0) {
            $.each(dataView, function (key, row) {
                var currentUid = row.uid;
                var currenRow = table.find("tr[data-uid='" + currentUid + "']");
                if (row.IsDeleted) {
                    var editButton = $(currenRow).find(".k-grid-edit");
                    if (typeof editButton !== 'undefined') {
                        editButton.attr("style", "display:none !important");
                    }
                    var deleteButton = $(currenRow).find(".k-grid-delete");
                    if (typeof deleteButton !== 'undefined') {
                        deleteButton.attr("style", "display:none !important");
                    }

                    table.find("tr[data-uid=" + currentUid + "]").addClass("bg-danger");
                } else {
                    if (row.IsExpired) {
                        table.find("tr[data-uid=" + currentUid + "]").addClass("bg-disabled");
                    }
                }
            });
        }
    }

    function onChange(ev) {
        if (ev.action === 'add') {
            var addedRecord = ev.items[0];
            if (addedRecord !== null) {
                var targetDate = $('input[name=date]').data('kendoDatePicker').value();
                addedRecord.RecordTimestamp = targetDate;
            }
        }
    }

    return {
        ErrorHandler: ErrorHandler,
        CloseWindow: CloseWindow,
        RefreshGrid: RefreshGrid,
        ValueMapper: ValueMapper,
        SendHistoryData: SendHistoryData,
        DeletableDataBound: DeletableDataBound,
        AfterterNomGridValidation: afterterNomGridValidation,
        MessageBound: messageBound,
        MessagesDataBound: MessagesDataBound,
        OnChange: onChange
    }
})();