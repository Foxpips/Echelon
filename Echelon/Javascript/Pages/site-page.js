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
            $userAvatar.on("click", () => {
                $leftNav.toggle("slide");
            });
        }

        $menubar.on("click", () => { $rightNav.toggle("slide"); });
        $(theme).on("click", function () {
            const themeSelected = $(this).data("target");
            cookieHelper.setCookie("theme", themeSelected, 300);
            localStorage.theme = themeSelected;
            // $('link[title=mystyle]')[0].disabled = true;
        });
    })();
};