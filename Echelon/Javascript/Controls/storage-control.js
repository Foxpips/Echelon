/*jshint esversion: 6 */
var StorageControl = function () {
    var self = this;
    var cookieHelper = new CookieHelper();
    var localStorageEnabled = false;
    //ctor
    (function () {
        if (hasLocalStorage()) {
            localStorageEnabled = true;
            console.log(localStorageEnabled);
        }
    }());

    self.add = function (key, value) {
        console.log("adding");
        console.log(key);
        if (localStorageEnabled) {
            localStorage.setItem(key, value);
        } else {
            cookieHelper.setCookie(key, value);
        }
    };

    self.get = function (key) {
        console.log("getting");
        console.log(key);
        console.log(localStorageEnabled);
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
        console.log(localStorageEnabled);
        console.log(localStorage === "undefined");
        if (localStorage === "undefined") {
            console.log("has no storage");
            localStorageEnabled = false;
            return;
        }
        console.log(localStorageEnabled);
        var test = "test";
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            localStorageEnabled = true;
            console.log(localStorageEnabled);
        } catch (e) {
            localStorageEnabled = false;
        }
    }
};