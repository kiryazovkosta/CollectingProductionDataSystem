$(document).ready(function () {
    var sidebar = $("a#naw-collapse");
    if (sidebar) {
        sidebar.click(function () {
            if ($(this).attr("aria-expanded") == "true") {
                $(this).attr("aria-expanded", "false");
                $("a#naw-collapse span").attr("class", "glyphicon glyphicon-chevron-down");
            } else {
                $(this).attr("aria-expanded", "true");
                $("a#naw-collapse span").attr("class", "glyphicon glyphicon-chevron-up");
            }
        });
    }
});