/* by @manufosela - browserclosurenotice.js - Prevention Close Browser - 20150525 - v1.1 */
/* works IE8+, FF, Chrome */
/* MIT License (MIT) Copyright (c) 2015 @manufosela */
/* It is independent of any library or framework */
BrowserClosureNotice = function () {
    "use strict";

    var BrowserClosureNotice = function (objArgs) {
        /* OPTIONAL ARGUMENTS */
        this.stepsTakenToClose = objArgs.stepsTakenToClose || 10;
        this.distanceNearClose = objArgs.distanceNearClose || 100;
        this.maxTimes = (typeof objArgs.maxTimes !== "undefined" && !!~objArgs.maxTimes) ? objArgs.maxTimes : 1;
        this.callbackBrowserClosureNotice = objArgs.callback || function () { alert("really you want to go?"); };

        /* PARAMETERS, NOT CHANGE */
        this.oldx = 0;
        this.oldy = 0;
        this.w = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
        this.distanceWidth = (this.isApple()) ? this.distanceNearClose : (this.w - this.distanceNearClose);
        this.goup = 0;
        this.godown = 0;
        this.goright = 0;
        this.goleft = 0;
        this.moreTimes = true;
        this.times = 0;
        //console.log( "W: " + this.w+" - W-distanceNearClose: " + ( this.w - this.distanceNearClose ) + " -TIMES: " + this.times );
        //document.getElementById( "data" ).innerHTML = "W: " + this.w+" - W-distanceNearClose: " + ( this.w - this.distanceNearClose ) + " -TIMES: " + this.times;
    };
    BrowserClosureNotice.prototype.detect = function () {
        var _this = this;
        if (document.addEventListener) { document.addEventListener('mousemove', function (e) { _this.mousemovemethod(e); }); }
        if (document.attachEvent) { document.attachEvent('onmousemove', function (e) { _this.mousemovemethod(e); }); }
        // console.log ( "detecting..." );
    };
    BrowserClosureNotice.prototype.unDetect = function () {
        var _this = this;
        if (document.removeEventListener) { document.removeEventListener('mousemove', _this.mousemovemethod, false); }
        if (document.detachEvent) { document.detachEvent('onmousemove', _this.mousemovemethod); }
    };
    BrowserClosureNotice.prototype.mousemovemethod = function (e) {
        var p = this.getMousePos(e), side,
            isApple = this.isApple(),
            pxCond = (isApple) ? (p.x < this.distanceWidth) : (p.x > this.distanceWidth);
        if (p.x < this.oldx) { this.goright = 0; this.goleft++; }
        else if (p.x > this.oldx) { this.goright++; this.goleft = 0; }
        if (p.y < this.oldy) { this.goup++; this.godown = 0; }
        else if (p.y > this.oldy) { this.goup = 0; this.godown++; }
        side = (isApple) ? this.goleft : this.goright;
        if (
            (p.y < this.distanceNearClose && this.goup >= this.stepsTakenToClose) &&
            (pxCond && side >= this.stepsTakenToClose) &&
            this.moreTimes
        ) {
            // console.log( "ALERT: CLOSE BROWSER. Times " + this.times );
            this.times++;
            this.moreTimes = (this.times < this.maxTimes || this.maxTimes === 0);
            if (!this.moreTimes) { this.unDetect(); }
            this.callbackBrowserClosureNotice(e);
        }
        this.oldx = p.x;
        this.oldy = p.y;
        //console.log( this.oldx + ", " + this.oldy + " - " + this.goup + ">="+this.stepsTakenToClose+", " + side + " >= "+this.stepsTakenToClose+" --- " + this.moreTimes );        
    };
    BrowserClosureNotice.prototype.isIE = function () {
        var myNav = navigator.userAgent.toLowerCase();
        return (!!~myNav.indexOf('msie')) ? parseInt(myNav.split('msie')[1]) : false;
    };
    BrowserClosureNotice.prototype.isApple = function () {
        var myNav = navigator.userAgent.toLowerCase();
        return (!!~myNav.indexOf('mac os x'));
    };
    BrowserClosureNotice.prototype.getMousePos = function (e) {
        var tempX, tempY;
        if (this.isIE()) {
            tempX = event.clientX + document.body.scrollLeft;
            tempY = event.clientY + document.body.scrollTop;
        } else {
            tempX = e.clientX;
            tempY = e.clientY;
        }
        if (tempX < 0) { tempX = 0; }
        if (tempY < 0) { tempY = 0; }
        return { x: tempX, y: tempY };
    };

    return BrowserClosureNotice;
}();