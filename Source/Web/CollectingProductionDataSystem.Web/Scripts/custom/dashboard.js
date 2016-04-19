var Charts = [];
(function () {
    $(function () {
        regreshTab(1, true);

        $('#collapseOne').on('hidden.bs.collapse', function () {
            var span = $('#collapse span');
            span.removeClass();
            span.addClass("glyphicon glyphicon-chevron-down");
        });

        $('#collapseOne').on('shown.bs.collapse', function () {
            var span = $('#collapse span');
            span.removeClass();
            span.addClass("glyphicon glyphicon-chevron-up");
        })

        $("#convert").click(
            function () {
                $.ajax({
                    url: "/ManagementDashBoard/Dashboard/CheckDates",
                    type: 'POST',
                    data: {
                        beginDate: GetDate("#begin-date"),
                        endDate: GetDate("#end-date"),
                        "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val()
                    }
                }).done(function () {
                    var tab = $("#charts-holder").data("kendoTabStrip").select();
                    var id = parseInt(tab.attr("aria-controls").replace(/^\D+/g, ''));
                    regreshTab(id, true);
                })
                    .fail(function (data) {
                        errHandler(data);
                    });
            });

        $("#charts-holder").data("kendoTabStrip").bind("activate", onActivate);

    });


    function onActivate(ev, populate) {
        var id = parseInt($(ev.item).attr("aria-controls").replace(/^\D+/g, ''));
        regreshTab(id, true);
    }

    function regreshTab(id, populate) {
        $('[role=tabpanel]').html('');
        $.ajax({
            url: "/ManagementDashBoard/Dashboard/ReloadTabContent",
            data: { tabId: id },
            success: function (data) {
                $("#charts-holder-" + id).html(data);
                if (populate) {
                    populateTab(id);
                }
            }
        });
    }

    function populateTab(id) {

        if (id == 1) {

            var processUnits = $("[role=pu-load-chart-holder]");
            processUnits.each(function (index, holder) {
                var puElementId = $(holder).attr("id").replace("pul-", "");
                GetHolderValue(puElementId, holder, "/ManagementDashBoard/Dashboard/DailyLoadChart");
            });

        }

        if (id == 2) {
            var processUnits = $("[role=pu-chart-holder]");
            processUnits.each(function (index, holder) {
                var publElenentId = $(holder).attr("id").replace("pu-", "");
                GetHolderValue(publElenentId, holder, "/ManagementDashBoard/Dashboard/DailyMaterialChart");
            });
        }
    }

    function GetHolderValue(elementId, holder, url) {
        $.ajax({
            url: url,
            data: {
                processUnitId: elementId,
                beginDate: GetDate("#begin-date"),
                endDate: GetDate("#end-date"),
                height: 200,
                shortTitle: true
            },
            success: function (data) {
                $(holder).html(data);
            }
        });
    }


    function GetTabValue(id) {
        $.ajax({
            url: "/ManagementDashBoard/Dashboard/DailyMaterialChart?processUnitId=" + id + "&date=2016-03-31+00%3A00%3A00Z",
            success: function (data) {
                $("#value" + (id)).html(data);
            }
        });
    }

    function GetDate(selector) {
        var datePicker = $(selector).data('kendoDatePicker');
        if (datePicker !== undefined) {
            var date = datePicker.value();
        } else {
            return;
        }

        return date.toISOString(kendo.culture().name);
    }


    function errHandler(data) {
        var errorMessage = "";
        var response = JSON.parse(data.responseText).data;
        $.each(response.errors, function (key, value) {
            errorMessage += this + "\n";
        });
        $('pre#err-message').text(errorMessage);
        $('div#err-window').data("kendoWindow").open();
    }
}());