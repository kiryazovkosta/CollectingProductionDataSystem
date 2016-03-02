$(function () {
    var mainNav = $('#main-nav ul');
    Array.prototype.forEach.call(mainNav, function (element) {
        var lis = $(element).find('li');
        var included = 0;
        var divider;
        var firstDivider;
        var previousIncluded = 0;
        Array.prototype.forEach.call(lis, function (li) {

            if ($(li).hasClass('divider')) {
                if (included === 0 && divider !== undefined) {
                    $(divider).attr('style', 'display:none !important');
                }
                divider = li;
                previousIncluded = included;
                included = 0;
            } else {
                if ($(li).find('a').length > 0) {
                    included += 1;
                }

                if (divider !== undefined && firstDivider === undefined && included > 0 && previousIncluded === 0) {
                    firstDivider = divider;
                    $(firstDivider).addClass('divider-top');
                }
            }
        });
    });
});