(function () {
    'use strict';
    var elementSelectors = [
           { selector: "#factories", controlType: "kendoDropDownList" },
           { selector: "#processunits", controlType: "kendoDropDownList" },
           { selector: "#factoriesD", controlType: "kendoDropDownList" },
           { selector: "#processunitsD", controlType: "kendoDropDownList" },
           { selector: "#areas", controlType: "kendoDropDownList" },
           { selector: "#parks", controlType: "kendoDropDownList" },
           { selector: "#directions", controlType: "kendoDropDownList" }];

    $(window).unload(function () {
        elementSelectors.forEach(function (element) {
            var domElement = $(element.selector);
            if (domElement) {
                var kendoElement = domElement.data(element.controlType);
                if (kendoElement) {
                    localStorage.setItem(domElement.attr("id") + "_" + $("#user").val(), JSON.stringify(kendoElement.value()));
                }
            }
        });
    });

    $(document).ready(function () {
        elementSelectors.forEach(function (element) {
            var domElement = $(element.selector);
            if (domElement) {
                var kendoElement = domElement.data(element.controlType);
                if (kendoElement && (kendoElement.ns === ".kendoDropDownList")) {
                    kendoElement.bind("dataBound", setSelection);
                }
            }
        });
    });

    function setSelection() {
        var kendoElement = this;
        kendoElement.ns = kendoElement.ns || "";
        if (kendoElement.ns === ".kendoDropDownList") {
            var selected = JSON.parse(localStorage.getItem(kendoElement.element.attr("id") + "_" + $("#user").val()))
            kendoElement.dataSource.select = selected;
            kendoElement.value(selected);
        }
    }
}());


