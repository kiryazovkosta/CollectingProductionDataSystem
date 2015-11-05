(function () {
    $(document).ready(function () {
        attachEventHandlerForSidebarButton();
        attachWritingSidebarBanerTextToApplyButton();
    });

    function attachWritingSidebarBanerTextToApplyButton() {
        var applyButton = $('#apply');
        if (applyButton) {
            applyButton.click(function () {
                //select all the inputs in the form
                var text = '<strong> | </strong>&nbsp;&nbsp;';
                var inputs = $("div.sidebar :input[type='text']");
                var selects = $("select");

                $.each(inputs, function (ix, input) {
                    var jInput = $(input);
                    var role = jInput.data().role;
                    if (role) {//kendo input
                        if (role == 'datepicker') {
                            text += '<span class="gothic">' + esc(jInput.val()) + '</span>' + '&nbsp;&nbsp;<strong> | </strong>&nbsp;&nbsp;';
                        } else {
                            var objectKeys = $.map(jInput.data(), function (value, key) {
                                return key;
                            });
                            text += '<span class="gothic">' + esc(jInput.data(objectKeys[1]).text()) + '</span>' + '&nbsp;&nbsp;<strong> | </strong>&nbsp;&nbsp;';
                        }
                    } else {//normal input
                        text += '<span class="gothic">' + esc(jInput.val()) + '</span>' + '&nbsp;&nbsp;<strong> | </strong>&nbsp;&nbsp;';
                    }
                })

                $.each(selects, function (index, select) {
                    var jSelect = $(select);
                    if (!jSelect.data().role) {
                        text += '<span class="gothic">' + esc(jSelect.find(":selected").text()) + '</span>' + '&nbsp;&nbsp;<strong> | </strong>&nbsp;&nbsp;';
                    }
                })
                $("div#slide-text").html(text);
            });
        }
    }

    function attachEventHandlerForSidebarButton() {
        var sidebarContent = $('div.sidebar');
        if (sidebarContent) {
            $("a#slide").click(function () {
                var slideButton = $(this);
                sidebarContent.animate({ width: 'toggle' }
                );
                if (slideButton.attr("aria-expanded") == "true") {
                    slideButton.attr("aria-expanded", "false");
                    $("a#slide span").attr("class", "glyphicon glyphicon-chevron-down");
                } else {
                    slideButton.attr("aria-expanded", "true");
                    $("a#slide span").attr("class", "glyphicon glyphicon-chevron-up");
                }
            });
        }
    }

    var esc = function (string) {
        return string.replace(/[-[\]{}()*+?,\\^$|#<>\s]/g, "\$&");
    }
})();