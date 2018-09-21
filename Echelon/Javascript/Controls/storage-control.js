/*jshint esversion: 6 */
var StorageControl = function () {
    var self = this;
    var cookieHelper = new CookieHelper();
    var localStorageEnabled = false;
    //ctor
    (function () {
        if (hasLocalStorage()) {
            localStorageEnabled = true;
        }
    }());

    self.add = function (key, value) {
        if (localStorageEnabled) {
            localStorage.setItem(key, value);
        } else {
            cookieHelper.setCookie(key, value);
        }
    };

    self.get = function (key) {
        if (localStorageEnabled) {
            return localStorage.getItem(key) || undefined;
        } else {
            return cookieHelper.getCookie(key);
        }
    };

    self.remove = function(key) {
        if (localStorageEnabled) {
            localStorage.removeItem(key);
        } else {
            cookieHelper.deleteCookie(key);
        }
    };

    function hasLocalStorage() {
        if (localStorage === "undefined") {
            localStorageEnabled = false;
            return;
        }
        var test = "test";
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            localStorageEnabled = true;
        } catch (e) {
            localStorageEnabled = false;
        }
    }
};