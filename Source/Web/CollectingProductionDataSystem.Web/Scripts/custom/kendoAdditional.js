﻿$(function () {
    var culture = $('#culture').val();
    kendo.culture(culture);
    prepareWindow('div#err-window', 'Възникна грешка в приложението');
    prepareWindow('div#success-window', 'Успешна операция');
});

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

var refreshGrid = function (selector) {
    var grid = $(selector).data('kendoGrid');
    if (grid !== null) {
        grid.dataSource.read();
    }
}