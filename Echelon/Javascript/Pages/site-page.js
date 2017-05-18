/*jshint esversion: 6 */

var avatarUrlUser;
var SitePage = function () {
    var cookieHelper = new CookieHelper();
    var $menubar = $("#menuGlobe");
    var $rightNav = $("#rightSideNav");
    var $leftNav = $("#leftSideNav");
    var theme = document.body.querySelectorAll("[data-attribute='theme']");
    var ajaxHelper = new AjaxHelper();
    var avatarControl = new AvatarControl(ajaxHelper);
    var $userAvatar = $("#headerAvatar");
    var $menuOverlayRight = $("#menuOverlayRight");
    var $menuOverlayLeft = $("#menuOverlayLeft");
    

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