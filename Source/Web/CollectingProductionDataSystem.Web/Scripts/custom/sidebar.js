$(document).ready(function () {
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
});