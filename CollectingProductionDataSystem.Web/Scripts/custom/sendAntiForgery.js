function sendAntiForgery() {
    return { "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val() }
}