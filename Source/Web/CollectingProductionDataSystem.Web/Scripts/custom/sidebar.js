$(document).ready(function () {
    var sidebarContent = $('div.sidebar');
    if (sidebarContent) {
        $("a#slide").click(function () {
            $("div#content").attr("style", "display:none;");
            var slideButton = $(this);
            sidebarContent.animate({ width: 'toggle' }, function () {
                if (slideButton.attr("aria-expanded") == "true") {
                    slideButton.attr("aria-expanded", "false");
                    $("a#slide span").attr("class", "glyphicon glyphicon-chevron-down");
                    //$("div#aside").attr("style", "max-width:20px;");
                    $("div#content").attr("style", "width:99%; margin-left:30px;");
                } else {
                    slideButton.attr("aria-expanded", "true");
                    $("a#slide span").attr("class", "glyphicon glyphicon-chevron-up");
                    $("div#aside").attr("style", "");
                    $("div#content").attr("style", "");
                }
            });
        });
    }
});