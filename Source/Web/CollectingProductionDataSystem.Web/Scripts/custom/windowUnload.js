﻿/*jslint browser: true*/
/*global window, document */
/*global $, jQuery*/

var validNavigation = false;
(function ($) {
    'use strict';
    function addAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    }

    $(document).ready(function () {

        // block back and forward buttons

        if (window.history && window.history.pushState) {

            $(window).on('popstate', function () {
                validNavigation = false;
            });

        }

        $(window).bind("beforeunload", function closingCode() {
            if (!validNavigation) {
                event.preventDefault();
                $.ajax({
                    url: '/Ajax/UserCloseWindow',
                    type: 'POST',
                    data: addAntiForgeryToken({})
                });
                window.location.replace("/Home/Index");
                return;
            }
        });

        // Attach the event click for all links in the page
        $("a").bind("click", function () {
            validNavigation = true;
        });

        // Attach the event submit for all forms in the page
        $("form").bind("submit", function () {
            validNavigation = true;
        });

        // Attach the event click for all inputs in the page
        $("input[type=submit]").bind("click", function () {
            validNavigation = true;
        });

        $(document.body).on("keydown", this, function (event) {
            if (event.keyCode === 116) {
                validNavigation = true;
            }
        });

        $(document.body).on("unload", this, function (event) {
            if (!(event.keyCode === 116)) {
                validNavigation = false;
            }
        });
    });
}(jQuery));