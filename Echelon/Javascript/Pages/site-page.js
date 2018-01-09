/*jshint esversion: 6 */

var SitePage = function () {
    var $menubar = $("#menuHome");
    var $rightNav = $("#rightSideNav");
    var $leftNav = $("#leftSideNav");
    var $closeNav = $(".closebtn");

    var $userAvatar = $("#headerAvatar");
    var $menuOverlayRight = $("#menuOverlayRight");
    var $menuOverlayLeft = $("#menuOverlayLeft");
    var theme = document.body.querySelectorAll("[data-attribute='theme']");

    var cookieHelper = new CookieHelper();
    var ajaxHelper = new AjaxHelper();
    var avatarControl = new AvatarControl(ajaxHelper);

    //    function hasLocalStorage(storagefunction, nonstoragefunction) {
    //        if (typeof (Storage) !== "undefined") {
    //            storagefunction();
    //        } else {
    //            nonstoragefunction();
    //        }
    //    }

    //constructor
    (function () {
        if (localStorage.theme === undefined) {
            localStorage.theme = "theme-Light";
        }

        let href = $('link[rel=stylesheet]')[0].href;
        let currentTheme = href.replace(/theme-\w*/, localStorage.theme);
        $('link[rel=stylesheet]')[0].href = currentTheme;

        $(window).load(function () {
            $("html").removeClass("preload");
        });

        if ($userAvatar.length > 0) {
            avatarControl.setUserAvatar($userAvatar.data("target"));
        }

        $userAvatar.on("click", () => {
            $menuOverlayRight.hide();
            $leftNav.toggle("slide");
            $menuOverlayLeft.toggle("slide");
            $rightNav.hide();
        });

        $menubar.on("click", () => {
            $menuOverlayLeft.hide();
            $rightNav.toggle("slide");
            $menuOverlayRight.toggle("slide");
            $leftNav.hide();
        });

        $closeNav.on("click", () => {
            if ($rightNav.is(":visible")) {
                $rightNav.toggle("slide");
                $menuOverlayRight.toggle("slide");
            }

            if ($leftNav.is(":visible")) {
                $leftNav.toggle("slide");
                $menuOverlayLeft.toggle("slide");
            }
        });

        $(theme).on("click", function () {
            const themeSelected = $(this).data("target");
            console.log(themeSelected);
            cookieHelper.setCookie("theme", themeSelected, 300);
            localStorage.theme = themeSelected;
            let hreftheme = $('link[rel=stylesheet]')[0].href;
            let replace = hreftheme.replace(/theme-\w*/, localStorage.theme);
            $('link[rel=stylesheet]')[0].href = replace;
            console.log(hreftheme);
        });
    })();
};