﻿/*jshint esversion: 6 */

var CookieHelper = function() {
    var self = this;

    self.setCookie = function(cname, cvalue, exdays) {
        const d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        const expires = `expires=${d.toUTCString()}`;
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    };

    self.getCookie = function(cname) {
        const name = cname + "=";
        const ca = document.cookie.split(";");
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === " ") {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    };
};
