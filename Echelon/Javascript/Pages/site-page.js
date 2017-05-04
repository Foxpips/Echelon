/*jshint esversion: 6 */

var SitePage = function () {
    var cookieHelper = new CookieHelper();
    const $menubar = $("#menuGlobe");
    const $sideNav = $("#mySidenav");
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
        $(theme).on("click", function () {
            const themeSelected = $(this).data("target");
            
            cookieHelper.setCookie("theme", themeSelected, 300);
//            $('link[title=mystyle]')[0].disabled = true;

            localStorage.theme = themeSelected;
        });
    })();
};