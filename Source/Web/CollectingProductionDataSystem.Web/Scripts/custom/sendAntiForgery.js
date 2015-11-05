(function () {
    sendAntiForgery = function () {
        return { "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val() }
    }
})();