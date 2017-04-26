/*jshint esversion: 6 */

var SitePage = function () {
    var cookieHelper = new CookieHelper();
    const $menubar = $("#menuGlobe");
    const $sideNav = $("#mySidenav");
    const $htmlContainer = $("#htmlContainer");
    const theme = document.body.querySelectorAll("[data-attribute='theme']");

    function hasLocalStorage(storagefunction, nonstoragefunction) {
        if (typeof (Storage) !== "undefined") {
            storagefunction();
        } else {
            nonstoragefunction();
        }
    }

    //constructor
    (function () {
        $menubar.on("click", () => { $sideNav.toggle("slide"); });

        hasLocalStorage(function () {
            $htmlContainer.addClass(localStorage.theme);
        });

        $(theme).on("click", function () {
            hasLocalStorage(function () {
                $htmlContainer.removeClass(localStorage.theme);
                localStorage.theme = $(this).data("target");
                $htmlContainer.addClass(localStorage.theme);
            }, function () {
                cookieHelper.setCookie("theme", $(this).data("target"), 300);
            });
        });
    })();
};