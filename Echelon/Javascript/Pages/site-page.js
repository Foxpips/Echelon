/*jshint esversion: 6 */

var SitePage = function () {
    var cookieHelper = new CookieHelper();
    const $menubar = $("#menuGlobe");
    const $rightNav = $("#rightSideNav");
    const $leftNav = $("#leftSideNav");
    const theme = document.body.querySelectorAll("[data-attribute='theme']");
    const ajaxHelper = new AjaxHelper();
    const avatarControl = new AvatarControl(ajaxHelper);
    const $userAvatar = $("#headerAvatar");
    const $menuOverlayRight = $("#menuOverlayRight");
    const $menuOverlayLeft = $("#menuOverlayLeft");
    

    function hasLocalStorage(storagefunction, nonstoragefunction) {
        if (typeof (Storage) !== "undefined") {
            storagefunction();
        } else {
            nonstoragefunction();
        }
    }

    //constructor
    (function () {
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

        $(theme).on("click", function () {
            const themeSelected = $(this).data("target");
            cookieHelper.setCookie("theme", themeSelected, 300);
            localStorage.theme = themeSelected;
            // $('link[title=mystyle]')[0].disabled = true;
        });
    })();
};